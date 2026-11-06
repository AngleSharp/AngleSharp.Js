namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Browser;
    using AngleSharp.Common;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Js;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Native;
    using Jint.Native.Array;
    using Jint.Native.Object;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// A pair of entangled message ports.
    /// </summary>
    [DomName("MessageChannel")]
    [DomExposed("Window")]
    public sealed class MessageChannel
    {
        /// <summary>
        /// Creates a new message channel.
        /// </summary>
        /// <param name="window">The window owning the channel.</param>
        [DomConstructor]
        public MessageChannel(IWindow window)
        {
            var document = window?.Document ?? throw new ArgumentNullException(nameof(window));
            var gate = new Object();
            var port1 = new MessagePort(document, gate);
            var port2 = new MessagePort(document, gate);
            port1.Entangle(port2);
            Port1 = port1;
            Port2 = port2;
        }

        /// <summary>
        /// Gets the first port.
        /// </summary>
        [DomName("port1")]
        public MessagePort Port1 { get; }

        /// <summary>
        /// Gets the second port.
        /// </summary>
        [DomName("port2")]
        public MessagePort Port2 { get; }
    }

    /// <summary>
    /// A message endpoint that can exchange structured-cloneable values with its entangled port.
    /// </summary>
    [DomName("MessagePort")]
    [DomExposed("Window")]
    public sealed class MessagePort : EventTarget, IMessagePort
    {
        private readonly Object _gate;
        private PortState _state;

        internal MessagePort(IDocument document, Object gate)
        {
            _gate = gate;
            _state = new PortState(this, document);
        }

        private MessagePort(Object gate, PortState state)
        {
            _gate = gate;
            _state = state;
        }

        /// <summary>
        /// Raised when a message is received.
        /// </summary>
        [DomName("onmessage")]
        public event DomEventHandler Message
        {
            add
            {
                AddEventListener("message", value, false);
                Start();
            }
            remove { RemoveEventListener("message", value, false); }
        }

        /// <summary>
        /// Raised when a message cannot be deserialized.
        /// </summary>
        [DomName("onmessageerror")]
        public event DomEventHandler MessageError
        {
            add { AddEventListener("messageerror", value, false); }
            remove { RemoveEventListener("messageerror", value, false); }
        }

        /// <summary>
        /// Raised when a message is received through the AngleSharp port contract.
        /// </summary>
        public event DomEventHandler MessageReceived
        {
            add { AddEventListener("message", value, false); }
            remove { RemoveEventListener("message", value, false); }
        }

        /// <summary>
        /// Posts a message to the entangled port.
        /// </summary>
        public void PostMessage(Object message, Object transfer = null)
        {
            var state = _state;

            if (state == null)
            {
                return;
            }

            PortState peer;

            lock (_gate)
            {
                if (state.Closed || state.Peer == null || state.Peer.Closed)
                {
                    return;
                }

                peer = state.Peer;
            }

            var receiver = peer.GetDocument();

            if (receiver == null && !peer.AwaitingDocument)
            {
                return;
            }

            var serialized = MessageTransfer.Clone(message, transfer, receiver);
            peer.Enqueue(serialized);
        }

        /// <summary>
        /// Starts dispatching messages that were queued before the port started.
        /// </summary>
        [DomName("start")]
        public void Start()
        {
            var state = _state;

            if (state != null)
            {
                lock (_gate)
                {
                    state.Started = true;
                }

                Schedule(state);
            }
        }

        /// <summary>
        /// Closes both ports in the channel.
        /// </summary>
        [DomName("close")]
        public void Close()
        {
            var state = _state;

            if (state == null)
            {
                return;
            }

            lock (_gate)
            {
                Close(state);
                _state = null;
            }
        }

        void IMessagePort.Send(Object message) => PostMessage(message);

        void IMessagePort.Open() => Start();

        void IMessagePort.Close() => Close();

        internal Boolean CanTransfer => _state != null && !_state.Closed;

        internal static MessagePort[] GetPorts(Object value)
        {
            value = Unwrap(value);

            if (value == null)
            {
                return Array.Empty<MessagePort>();
            }

            if (value is ObjectInstance options && options is not ArrayInstance)
            {
                var transfer = options.Get("transfer");
                value = transfer.IsUndefined() ? null : transfer;
            }

            value = Unwrap(value);

            if (value == null)
            {
                return Array.Empty<MessagePort>();
            }

            var list = new List<MessagePort>();

            if (value is ArrayInstance array)
            {
                var length = (Int32)array.Get("length").AsNumber();

                for (var i = 0; i < length; i++)
                {
                    AddPort(list, array.Get(i));
                }
            }
            else if (value is IEnumerable items && value is not String)
            {
                foreach (var item in items)
                {
                    AddPort(list, item);
                }
            }
            else
            {
                throw new DomException(DomError.DataClone);
            }

            return list.ToArray();
        }

        internal static Object Unwrap(Object value)
        {
            if (value is JsValue js)
            {
                if (js.IsNull() || js.IsUndefined())
                {
                    return null;
                }

                if (js is ObjectInstance instance)
                {
                    return instance is IDomProxy proxy ? proxy.Value : instance;
                }

                return js.ToObject();
            }

            return value;
        }

        internal MessagePort PrepareTransfer()
        {
            var state = _state;

            if (state == null || state.Closed)
            {
                throw new DomException(DomError.DataClone);
            }

            return new MessagePort(_gate, state);
        }

        internal void CommitTransfer(MessagePort transferred, IDocument document)
        {
            lock (_gate)
            {
                var state = _state;

                if (state == null || state.Closed)
                {
                    throw new DomException(DomError.DataClone);
                }

                transferred._state = state;
                state.Owner = transferred;
                state.SetDocument(document);
                state.Generation++;
                _state = null;
            }

            Schedule(transferred._state);
        }

        internal void SetDocument(IDocument document)
        {
            var state = _state;

            if (state != null)
            {
                lock (_gate)
                {
                    state.SetDocument(document);
                    state.Generation++;
                }

                Schedule(state);
            }
        }

        internal void Entangle(MessagePort other)
        {
            lock (_gate)
            {
                var left = _state;
                var right = other._state;
                left.Peer = right;
                right.Peer = left;
            }
        }

        private static void AddPort(List<MessagePort> ports, Object value)
        {
            value = Unwrap(value);

            if (value is not MessagePort port || !port.CanTransfer)
            {
                throw new DomException(DomError.DataClone);
            }

            for (var i = 0; i < ports.Count; i++)
            {
                if (ReferenceEquals(ports[i], port))
                {
                    throw new DomException(DomError.DataClone);
                }
            }

            ports.Add(port);
        }

        private static void Close(PortState state)
        {
            var peer = state.Peer;
            state.Closed = true;
            state.Generation++;
            state.Messages.Clear();
            state.Peer = null;
            state.Owner = null;

            if (peer != null)
            {
                peer.Closed = true;
                peer.Generation++;
                peer.Messages.Clear();
                peer.Peer = null;
                peer.Owner = null;
            }
        }

        private static void Schedule(PortState state)
        {
            IDocument document;
            Int32 generation;

            lock (state.Gate)
            {
                if (state.Closed || !state.Started || state.Scheduled || state.Messages.Count == 0)
                {
                    return;
                }

                document = state.GetDocument();

                if (document == null)
                {
                    if (!state.AwaitingDocument)
                    {
                        state.Messages.Clear();
                    }

                    return;
                }

                state.Scheduled = true;
                generation = state.Generation;
            }

            var weakState = new WeakReference<PortState>(state);

            try
            {
                MessageTaskQueue.Enqueue(document, () =>
                {
                    if (weakState.TryGetTarget(out var pending))
                    {
                        Deliver(pending, generation);
                    }
                });
            }
            catch
            {
                lock (state.Gate)
                {
                    state.Scheduled = false;
                }

                throw;
            }
        }

        private static void Deliver(PortState state, Int32 generation)
        {
            MessagePort owner;
            IDocument document;
            SerializedMessage message;

            lock (state.Gate)
            {
                state.Scheduled = false;

                if (state.Closed || !state.Started || state.Messages.Count == 0)
                {
                    return;
                }

                if (generation != state.Generation)
                {
                    Schedule(state);
                    return;
                }

                document = state.GetDocument();
                owner = state.Owner;

                if (document == null || owner == null)
                {
                    state.Messages.Clear();
                    return;
                }

                message = state.Messages.Dequeue();
            }

            Object data = message.Data;

            try
            {
                var scripting = document.Context?.GetService<JsScriptingService>();

                if (scripting != null)
                {
                    data = data.ToJsValue(scripting.GetOrCreateInstance(document));
                }
            }
            catch (Exception)
            {
                try
                {
                    owner.Dispatch(new Event("messageerror"));
                }
                finally
                {
                    Schedule(state);
                }

                return;
            }

            try
            {
                owner.Dispatch(new MessageEvent("message", false, false, data, String.Empty, String.Empty, null, message.Ports));
            }
            finally
            {
                Schedule(state);
            }
        }

        private sealed class PortState
        {
            public PortState(MessagePort owner, IDocument document)
            {
                Gate = owner._gate;
                Owner = owner;
                SetDocument(document);
                Messages = new Queue<SerializedMessage>();
            }

            public Object Gate { get; }

            public MessagePort Owner { get; set; }

            public PortState Peer { get; set; }

            public Queue<SerializedMessage> Messages { get; }

            public Boolean Started { get; set; }

            public Boolean Closed { get; set; }

            public Boolean Scheduled { get; set; }

            public Int32 Generation { get; set; }

            private WeakReference<IDocument> Document { get; set; }

            public Boolean AwaitingDocument { get; private set; }

            public IDocument GetDocument() =>
                Document != null && Document.TryGetTarget(out var document) ? document : null;

            public void SetDocument(IDocument document)
            {
                AwaitingDocument = document == null;
                Document = document == null ? null : new WeakReference<IDocument>(document);
            }

            public void Enqueue(SerializedMessage message)
            {
                lock (Gate)
                {
                    if (Closed)
                    {
                        return;
                    }

                    Messages.Enqueue(message);
                }

                Schedule(this);
            }
        }
    }

    /// <summary>
    /// Adds the transfer-list overload to AngleSharp's message-port contract.
    /// </summary>
    [DomExposed("MessagePort")]
    public static class MessagePortExtensions
    {
        /// <summary>
        /// Posts a structured-cloneable message with ports to transfer.
        /// </summary>
        [DomName("postMessage")]
        public static void PostMessage(this MessagePort port, Object message, Object transfer) =>
            port.PostMessage(message, transfer);
    }

    internal static class MessageTaskQueue
    {
        private static readonly ConditionalWeakTable<IDocument, TaskQueue> Queues =
            new ConditionalWeakTable<IDocument, TaskQueue>();

        public static void Enqueue(IDocument document, Action action)
        {
            var loop = document.Context?.GetService<IEventLoop>();

            if (loop != null)
            {
                loop.Enqueue(action, TaskPriority.Normal);
                return;
            }

            Queues.GetValue(document, static _ => new TaskQueue()).Enqueue(document, action);
        }

        private sealed class TaskQueue
        {
            private readonly Queue<Action> _actions = new Queue<Action>();
            private Boolean _scheduled;

            public void Enqueue(IDocument document, Action action)
            {
                var shouldSchedule = false;

                lock (_actions)
                {
                    _actions.Enqueue(action);

                    if (!_scheduled)
                    {
                        _scheduled = true;
                        shouldSchedule = true;
                    }
                }

                if (shouldSchedule)
                {
                    Schedule(document);
                }
            }

            private void Schedule(IDocument document)
            {
                if (!ThreadPool.QueueUserWorkItem(_ => Run(document)))
                {
                    lock (_actions)
                    {
                        _scheduled = false;
                    }

                    throw new InvalidOperationException("The message task could not be queued.");
                }
            }

            private void Run(IDocument document)
            {
                Action action;

                lock (_actions)
                {
                    if (_actions.Count == 0)
                    {
                        _scheduled = false;
                        return;
                    }

                    action = _actions.Dequeue();
                }

                try
                {
                    var scripting = document.Context?.GetService<JsScriptingService>();

                    if (scripting != null)
                    {
                        scripting.GetOrCreateInstance(document).RunTask(action);
                    }
                    else
                    {
                        action();
                    }
                }
                catch (Exception error)
                {
                    document.Context?.TrackError(error);
                }

                var hasMoreActions = false;

                lock (_actions)
                {
                    hasMoreActions = _actions.Count > 0;

                    if (!hasMoreActions)
                    {
                        _scheduled = false;
                    }
                }

                if (hasMoreActions)
                {
                    try
                    {
                        Schedule(document);
                    }
                    catch (Exception error)
                    {
                        document.Context?.TrackError(error);
                    }
                }
            }
        }
    }

    internal sealed class SerializedMessage
    {
        public SerializedMessage(Object data, MessagePort[] ports)
        {
            Data = data;
            Ports = ports;
        }

        public Object Data { get; }

        public MessagePort[] Ports { get; }

        public void SetDocument(IDocument document)
        {
            for (var i = 0; i < Ports.Length; i++)
            {
                Ports[i].SetDocument(document);
            }
        }
    }

    internal static class MessageTransfer
    {
        public static SerializedMessage Clone(Object message, Object transfer, IDocument receiver)
        {
            var ports = MessagePort.GetPorts(transfer);

            if (ports.Length == 0)
            {
                return new SerializedMessage(StructuredClone.Clone(message), ports);
            }

            var replacements = new Dictionary<MessagePort, MessagePort>();

            for (var i = 0; i < ports.Length; i++)
            {
                replacements.Add(ports[i], ports[i].PrepareTransfer());
            }

            var data = StructuredClone.Clone(message, replacements);

            for (var i = 0; i < ports.Length; i++)
            {
                ports[i].CommitTransfer(replacements[ports[i]], receiver);
            }

            var transferred = new MessagePort[ports.Length];

            for (var i = 0; i < ports.Length; i++)
            {
                transferred[i] = replacements[ports[i]];
            }

            return new SerializedMessage(data, transferred);
        }
    }
}
