namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using System;
    using System.Reflection;

    /// <summary>
    /// One event-handler member of a DOM type - "onclick" and its kin - as the accessor pair
    /// a prototype declares for it.
    /// </summary>
    /// <remarks>
    /// The pair is process-shared along with the rest of the type's members, so this object
    /// must stay free of anything belonging to an engine: it holds the two reflected accessors
    /// and derives everything else from the receiver. It doubles as the key a node files its
    /// handler under, which is what keeps the handler itself - the one thing here that is per
    /// node and per engine - on the node.
    /// </remarks>
    sealed class DomEventDefinition
    {
        private readonly MethodInfo _addHandler;
        private readonly MethodInfo _removeHandler;

        public DomEventDefinition(MethodInfo addHandler, MethodInfo removeHandler)
        {
            _addHandler = addHandler;
            _removeHandler = removeHandler;
        }

        public JsValue GetHandler(JsValue thisObject, JsValue[] arguments)
        {
            var node = thisObject.As<DomNodeInstance>();
            var registration = node?.GetEventHandler(this);
            return registration?.Function ?? JsValue.Null;
        }

        public JsValue SetHandler(JsValue thisObject, JsValue[] arguments)
        {
            var node = thisObject.As<DomNodeInstance>();
            var value = arguments.Length > 0 ? arguments[0] : JsValue.Undefined;

            if (node != null)
            {
                var previous = node.RemoveEventHandler(this);

                if (previous != null)
                {
                    _removeHandler?.Invoke(node.Value, new Object[] { previous.Handler });
                }

                var engine = node.Instance;

                if (value.AsFunction(engine) is Function function)
                {

                    var isWindow = node.Value is IWindow;

                    DomEventHandler handler = (s, ev) =>
                    {
                        var sender = s.ToJsValue(engine);

                        try
                        {
                            if (isWindow && ev is ErrorEvent error)
                            {
                                CallErrorHandler(function, sender, error, engine);
                            }
                            else
                            {
                                function.Call(sender, new[] { ev.ToJsValue(engine) });
                            }
                        }
                        catch (JavaScriptException thrown)
                        {
                            engine.ReportError(thrown);
                        }
                    };

                    node.SetEventHandler(this, new Registration(value, handler));
                    _addHandler?.Invoke(node.Value, new Object[] { handler });
                }
            }

            return value;
        }

        /// <summary>
        /// The onerror of a window is the one handler not getting the event (HTML 8.1.8.1), and
        /// returning true from it means that the error has been handled.
        /// </summary>
        private static void CallErrorHandler(Function function, JsValue sender, ErrorEvent error, EngineInstance engine)
        {
            var result = function.Call(sender, new[]
            {
                (error.Message ?? String.Empty).ToJsValue(engine),
                (error.FileName ?? String.Empty).ToJsValue(engine),
                error.Line.ToJsValue(engine),
                error.Column.ToJsValue(engine),
                error.Error.ToJsValue(engine),
            });

            if (result.IsBoolean() && result.AsBoolean())
            {
                error.Cancel();
            }
        }

        /// <summary>
        /// The handler currently assigned to a single node for a single event.
        /// </summary>
        public sealed class Registration
        {
            public Registration(JsValue function, DomEventHandler handler)
            {
                Function = function;
                Handler = handler;
            }

            /// <summary>
            /// The function that was assigned, as it has to be handed back on read.
            /// </summary>
            public JsValue Function { get; }

            /// <summary>
            /// The listener that was subscribed, as it has to be handed back on removal.
            /// </summary>
            public DomEventHandler Handler { get; }
        }
    }
}
