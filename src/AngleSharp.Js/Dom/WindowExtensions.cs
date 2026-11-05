namespace AngleSharp.Js.Dom
{
    using AngleSharp;
    using AngleSharp.Attributes;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom;
    using AngleSharp.Js.Attributes;
    using Jint;
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines a set of extensions for the window object.
    /// </summary>
    [DomExposed("Window")]
    public static class WindowExtensions
    {
        private static readonly ConditionalWeakTable<IWindow, Console> Consoles =
            new ConditionalWeakTable<IWindow, Console>();
        private static readonly ConditionalWeakTable<IWindow, Worker> WorkerOwners =
            new ConditionalWeakTable<IWindow, Worker>();
        private static readonly ConditionalWeakTable<IWindow, Performance> Performances =
            new ConditionalWeakTable<IWindow, Performance>();
        private static readonly ConditionalWeakTable<IWindow, Crypto> Cryptos =
            new ConditionalWeakTable<IWindow, Crypto>();

        //  The interval between two animation frames of a 60 Hz display.
        private const Int32 FrameInterval = 16;

        /// <summary>
        /// Posts a message to the window (HTML 9.4.3), from the document of the script calling.
        /// </summary>
        [DomName("postMessage")]
        public static void PostMessage(this IWindow window, Object message, Object targetOrigin = null, Object transfer = null)
        {
            if (WorkerOwners.TryGetValue(window, out var owner))
            {
                owner.PostMessageToOwner(message);
                return;
            }

            PostMessageFrom(EngineInstance.Current?.Window.Value is IWindow sender ? sender.Document : window.Document, window, message, targetOrigin);
        }

        internal static void PostMessageFrom(IDocument sender, IWindow target, Object message, Object targetOrigin)
        {
            var origin = ResolveTargetOrigin(sender, targetOrigin);
            var data = StructuredClone.Clone(message);

            // who it is for may have moved on; then it isn't delivered
            if (origin != "*" && origin != OriginPolicy.Serialize(target.Document))
            {
                return;
            }

            var ev = new MessageEvent("message", false, false, data, OriginPolicy.Serialize(sender), String.Empty, sender.DefaultView);
            target.Document.Context.GetService<IEventLoop>().EnqueueAsync(_ => target.Fire(ev));
        }

        // given as a string, or in the options
        private static String ResolveTargetOrigin(IDocument sender, Object targetOrigin)
        {
            if (targetOrigin is Jint.Native.Object.ObjectInstance options)
            {
                var value = options.Get("targetOrigin");
                targetOrigin = value.IsString() ? value.AsString() : null;
            }

            var origin = targetOrigin as String ?? "/";

            if (origin == "*")
            {
                return origin;
            }

            if (origin == "/")
            {
                return OriginPolicy.Serialize(sender);
            }

            var url = new Url(origin);

            if (url.IsInvalid || String.IsNullOrEmpty(url.Scheme))
            {
                throw new DomException(DomError.Syntax);
            }

            return url.Origin ?? "null";
        }

        /// <summary>
        /// Posts a text message.
        /// </summary>
        public static void PostMessage(this IWindow window, String message, String targetOrigin = "*", Object transfer = null)
        {
            if (WorkerOwners.TryGetValue(window, out var owner))
            {
                owner.PostMessageToOwner(message);
                return;
            }

            var ev = new MessageEvent("message", false, false, message, targetOrigin);
            var document = window.Document;
            var loop = document.Context.GetService<IEventLoop>();
            loop.EnqueueAsync(_ => window.Fire(ev));
        }

        internal static void RegisterWorkerWindow(IWindow workerWindow, Worker owner)
        {
            if (workerWindow == null || owner == null)
            {
                return;
            }

            WorkerOwners.Remove(workerWindow);
            WorkerOwners.Add(workerWindow, owner);
        }

        /// <summary>
        /// Gets a copy of a value that shares nothing with it (HTML 2.7), which fails with a
        /// DataCloneError for what can't be copied, e.g. a function.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="value">The value to copy.</param>
        /// <param name="options">The options, of which transfer has no meaning here.</param>
        /// <returns>The copy.</returns>
        [DomName("structuredClone")]
        public static Object StructuredCloneOf(this IWindow window, Object value = null, Object options = null) =>
            StructuredClone.Clone(value);

        /// <summary>
        /// Gets the origin of the window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>The serialized origin.</returns>
        [DomName("origin")]
        [DomAccessor(Accessors.Getter)]
        public static String GetOrigin(this IWindow window) => OriginPolicy.Serialize(window.Document);

        /// <summary>
        /// Gets if the window is a secure context, which an https address and the local machine are.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>True if it is one.</returns>
        [DomName("isSecureContext")]
        [DomAccessor(Accessors.Getter)]
        public static Boolean GetIsSecureContext(this IWindow window)
        {
            var address = window.Document?.DocumentUri;

            if (String.IsNullOrEmpty(address))
            {
                return false;
            }

            var url = new Url(address);
            return url.Scheme == "https" || url.Scheme == "file" || url.HostName == "localhost" || url.HostName == "127.0.0.1" || url.HostName == "[::1]";
        }

        /// <summary>
        /// Gets the number of frames in the window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>The number of iframes and frames of its document.</returns>
        [DomName("length")]
        [DomAccessor(Accessors.Getter)]
        public static Int32 GetLength(this IWindow window) =>
            window.Document?.QuerySelectorAll("iframe,frame").Length ?? 0;

        /// <summary>
        /// Gets the parent window context.
        /// </summary>
        [DomName("parent")]
        [DomAccessor(Accessors.Getter)]
        public static IWindow Parent(this IWindow window)
        {
            var context = window.Document.Context;
            return GetWindow(context?.Parent) ?? window;
        }

        /// <summary>
        /// Gets the top window context.
        /// </summary>
        [DomName("top")]
        [DomAccessor(Accessors.Getter)]
        public static IWindow Top(this IWindow window)
        {
            var context = window.Document.Context;

            while (context?.Parent != null)
            {
                context = context.Parent;
            }

            return GetWindow(context) ?? window;
        }

        /// <summary>
        /// Gets the console instance. The same instance is returned for the same
        /// window, so that `window.console === window.console` holds and anything
        /// script puts on the console is still there on the next access.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        [DomName("console")]
        [DomAccessor(Accessors.Getter)]
        public static Console Console(this IWindow window)
        {
            return Consoles.GetValue(window, w => new Console(w));
        }

        /// <summary>
        /// Gets the high resolution time of the window. The same instance is returned
        /// for the same window, so its time origin stays.
        /// </summary>
        [DomName("performance")]
        [DomAccessor(Accessors.Getter)]
        public static Performance Performance(this IWindow window)
        {
            return Performances.GetValue(window, _ => new Performance());
        }

        /// <summary>
        /// Requests to call the given callback before the next frame. Without a display to
        /// synchronize with, the frames follow the rate of a 60 Hz display.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="callback">The callback receiving the time of the frame.</param>
        /// <returns>The handle to cancel the request with.</returns>
        [DomName("requestAnimationFrame")]
        public static Int32 RequestAnimationFrame(this IWindow window, FrameRequestCallback callback)
        {
            var performance = window.Performance();
            return window.SetTimeout(w => callback?.Invoke(w, performance.Now()), FrameInterval);
        }

        /// <summary>
        /// Cancels a request made with requestAnimationFrame.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="handle">The handle of the request.</param>
        [DomName("cancelAnimationFrame")]
        public static void CancelAnimationFrame(this IWindow window, Int32 handle)
        {
            window.ClearTimeout(handle);
        }

        /// <summary>
        /// Gets the crypto object of the window.
        /// </summary>
        /// <param name="window"></param>
        /// <returns>The crypto object.</returns>
        [DomName("crypto")]
        [DomAccessor(Accessors.Getter)]
        public static Crypto Crypto(this IWindow window) => Cryptos.GetValue(window, _ => new Crypto());

        /// <summary>
        /// Requests the callback to be called when there is time for it, which is as soon as what is
        /// waiting on the event loop right now is done.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="callback">The callback receiving how much time it has.</param>
        /// <param name="options">The options, whose timeout isn't needed by running that soon.</param>
        /// <returns>The handle to cancel the request with.</returns>
        [DomName("requestIdleCallback")]
        public static Int32 RequestIdleCallback(this IWindow window, IdleRequestCallback callback, Object options = null)
        {
            var performance = window.Performance();
            return window.SetTimeout(w => callback?.Invoke(w, new IdleDeadline(performance)), 1);
        }

        /// <summary>
        /// Cancels a request made with requestIdleCallback.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="handle">The handle of the request.</param>
        [DomName("cancelIdleCallback")]
        public static void CancelIdleCallback(this IWindow window, Int32 handle)
        {
            window.ClearTimeout(handle);
        }

        /// <summary>
        /// Queues the given callback to run after the current script. It is queued as a task
        /// (like setTimeout with no delay), not as a microtask, which is close enough for scripts
        /// deferring work.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="callback">The callback to run.</param>
        [DomName("queueMicrotask")]
        public static void QueueMicrotask(this IWindow window, Action callback)
        {
            window.SetTimeout(_ => callback?.Invoke(), 0);
        }

        /// <summary>
        /// Creates a new IHtmlImageElement instance.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [DomConstructorFunction("Image")]
        public static IHtmlImageElement Image(this IWindow window, int? width = null, int? height = null)
        {
            var imageElement = window.Document.CreateElement(TagNames.Img) as IHtmlImageElement;

            if (width.HasValue)
            {
                imageElement.DisplayWidth = width.Value;
            }

            if (height.HasValue)
            {
                imageElement.DisplayHeight = height.Value;
            }

            return imageElement;
        }

        private static IWindow GetWindow(IBrowsingContext context) => context?.Active?.DefaultView;
    }
}
