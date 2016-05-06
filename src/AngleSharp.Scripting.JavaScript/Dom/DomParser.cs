namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Parser.Html;
    using AngleSharp.Parser.Xml;
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
        static readonly Dictionary<String, Func<String, IDocument>> SupportedTypes = new Dictionary<String, Func<String, IDocument>>
        {
            { "text/html", ParseHtml },
            { "text/xml", ParseXml },
            { "application/xml", ParseXml },
            { "application/xhtml+xml", ParseXml },
            { "image/svg+xml", ParseSvg }
        };

        readonly IWindow _window;

        [DomConstructor]
        public DomParser(IWindow window)
        {
            _window = window;
        }

        [DomName("parseFromString")]
        public IDocument Parse(String str, String type)
        {
            var parser = default(Func<String, IDocument>);

            if (!SupportedTypes.TryGetValue(type, out parser))
            {
                throw new DomException(DomError.NotSupported);
            }

            //Potentially the resulting document should also have the following properties:
            //- the content type must be the type argument
            //- the URL value must be the URL of the active document
            //- the location value must be null
            return parser.Invoke(str);
        }

        static IDocument ParseHtml(String content)
        {
            var options = new HtmlParserOptions
            {
                IsScripting = false,
                IsEmbedded = false
            };
            var html = new HtmlParser(options);
            return html.Parse(content);
        }

        static IDocument ParseXml(String content)
        {
            var options = new XmlParserOptions
            {
                IsSuppressingErrors = true
            };
            var xml = new XmlParser();

            try
            {
                return xml.Parse(content);
            } 
            catch (XmlParseException ex)
            {
                content = GetXmlErrorContent(ex, content);
                return xml.Parse(content);
            }
        }

        static IDocument ParseSvg(String content)
        {
            var svg = new XmlParser();
            return svg.Parse(content);
        }

        static String GetXmlErrorContent(XmlParseException ex, String content)
        {
            return String.Format("<parsererror xmlns=\"http://www.mozilla.org/newlayout/xml/parsererror.xml\">{0}<sourcetext>{1}</sourcetext></parsererror>",
                ex.Message,
                String.Empty
            );
        }
    }
}
