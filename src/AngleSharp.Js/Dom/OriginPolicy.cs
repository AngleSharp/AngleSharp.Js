namespace AngleSharp.Js.Dom
{
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using System;

    /// <summary>
    /// The same-origin policy (HTML 7.1). A document in a browsing context of its own is only
    /// reachable by script of the same origin. Everything crossing between two documents passes
    /// through <see cref="EngineInstance.GetDomNode(Object)"/>, so that is where it is enforced,
    /// whichever member gave it out (contentWindow, parent, event.source, ...).
    /// </summary>
    static class OriginPolicy
    {
        /// <summary>
        /// Whether the document is out of reach for script of the viewing one. A document nobody
        /// navigated to (e.g. of DOMParser) has no origin of its own to protect.
        /// </summary>
        public static Boolean IsCrossOrigin(IDocument viewer, IDocument document)
        {
            if (ReferenceEquals(viewer, document) || document?.Context is null || ReferenceEquals(viewer.Context, document.Context) ||
                !ReferenceEquals(document.Context.Active, document))
            {
                return false;
            }

            return !Equals(GetOriginKey(viewer), GetOriginKey(document));
        }

        /// <summary>
        /// The origin as sent with a message: "null" for an opaque one.
        /// </summary>
        public static String Serialize(IDocument document) => GetOriginKey(document) as String ?? "null";

        /// <summary>
        /// What identifies the origin: its serialization, or for an opaque origin (sandboxed,
        /// data:, file:, ...) the document having it, which equals nothing else. A document
        /// without an address of its own (about:blank, about:srcdoc) has the origin of its creator.
        /// </summary>
        private static Object GetOriginKey(IDocument document)
        {
            for (var current = document; current is not null; current = current.Context?.Parent?.Active)
            {
                var context = current.Context;

                if (context is not null && (context.Security & Sandboxes.Origin) == Sandboxes.Origin)
                {
                    return current;
                }

                var url = new Url(current.Url ?? String.Empty);

                if (!url.Scheme.Equals("about", StringComparison.OrdinalIgnoreCase) && url.Href.Length > 0)
                {
                    return url.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase) || String.IsNullOrEmpty(url.Origin)
                        ? current
                        : url.Origin;
                }

                if (context?.Parent?.Active is null)
                {
                    return current;
                }
            }

            return document;
        }

        /// <summary>
        /// Whether the context is not allowed to run script (the `sandbox` of an iframe).
        /// </summary>
        public static Boolean IsScriptingBlocked(IDocument document) =>
            document?.Context is { } context && (context.Security & Sandboxes.Scripts) == Sandboxes.Scripts;
    }
}
