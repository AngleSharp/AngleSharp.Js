namespace AngleSharp.Js.Dom
{
    using AngleSharp;
    using AngleSharp.Attributes;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom;
    using AngleSharp.Js.Attributes;
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

        //  The interval between two animation frames of a 60 Hz display.
        private const Int32 FrameInterval = 16;

        /// <summary>
        /// Posts a message.
        /// </summary>
        [DomName("postMessage")]
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
