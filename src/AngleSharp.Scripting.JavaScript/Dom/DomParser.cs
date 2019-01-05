namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Html.Parser;
    using AngleSharp.Xml.Parser;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A way for JavaScript applications to access the parser.
    /// See: https://w3c.github.io/DOM-Parsing/#the-domparser-interface
    /// </summary>
    [DomName("DOMParser")]
    [DomExposed("Window")]
    public sealed class DomParser
    {
        private static readonly Dictionary<String, Func<IBrowsingContext, String, IDocument>> SupportedTypes = new Dictionary<String, Func<IBrowsingContext, String, IDocument>>
        {
            { "text/html", ParseHtml },
            { "text/xml", ParseXml },
            { "application/xml", ParseXml },
            { "application/xhtml+xml", ParseXml },
            { "image/svg+xml", ParseXml }
        };

        private readonly IWindow _window;

        /// <summary>
        /// Creates a new DOMParser instance.
        /// </summary>
        /// <param name="window">The window to host the parser.</param>
        [DomConstructor]
        public DomParser(IWindow window)
        {
            _window = window;
        }

        /// <summary>
        /// Parses the given string for the given type. Throws a not supported
        /// exception if the type is not supported.
        /// </summary>
        /// <param name="str">The content to parse.</param>
        /// <param name="type">The type of the target to parse to.</param>
        /// <returns>The document of the given type.</returns>
        [DomName("parseFromString")]
        public IDocument Parse(String str, String type)
        {
            var parser = default(Func<IBrowsingContext, String, IDocument>);

            if (!SupportedTypes.TryGetValue(type, out parser))
            {
                throw new DomException(DomError.NotSupported);
            }

            //Potentially the resulting document should also have the following properties:
            //- the content type must be the type argument
            //- the URL value must be the URL of the active document
            //- the location value must be null
            return parser.Invoke(_window.Document.Context, str);
        }

        private static IDocument ParseHtml(IBrowsingContext context, String content)
        {
            var parser = context.GetService<IHtmlParser>();
            return parser.ParseDocument(content);
        }

        private static IDocument ParseXml(IBrowsingContext context, String content)
        {
            var parser = context.GetService<IXmlParser>();

            try
            {
                return parser.ParseDocument(content);
            } 
            catch (XmlParseException ex)
            {
                content = GetXmlErrorContent(ex, content);
                return parser.ParseDocument(content);
            }
        }

        private static String GetXmlErrorContent(XmlParseException ex, String content)
        {
            return String.Format("<parsererror xmlns=\"http://www.mozilla.org/newlayout/xml/parsererror.xml\">{0}<sourcetext>{1}</sourcetext></parsererror>",
                ex.Message,
                String.Empty
            );
        }
    }
}
