namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using System;

    /// <summary>
    /// Defines a set of extensions for the window object.
    /// </summary>
    [DomExposed("Window")]
    public static class WindowExtensions
    {
        /// <summary>
        /// Posts a message.
        /// </summary>
        [DomName("postMessage")]
        public static void PostMessage(this IWindow window, String message, String targetOrigin = "*", Object transfer = null)
        {
            var ev = new MessageEvent("message", false, false, message, targetOrigin);
            var document = window.Document;
            var loop = document.Context.GetService<IEventLoop>();
            loop.EnqueueAsync(_ => window.Fire(ev));
        }

        /// <summary>
        /// Gets the top window context.
        /// </summary>
        [DomName("top")]
        [DomAccessor(Accessors.Getter)]
        public static IWindow Top(this IWindow window) => window.Document.Context?.Creator?.DefaultView;

        /// <summary>
        /// Gets the console instance.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        [DomName("console")]
        [DomAccessor(Accessors.Getter)]
        public static Console Console(this IWindow window)
        {
            return new Console(window);
        }
    }
}
