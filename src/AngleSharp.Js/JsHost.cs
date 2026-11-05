namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Text;
    using Jint.Native;
    using Jint.Runtime;
    using Jint.Runtime.Modules;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Tells a module what the engine leaves to the host: where it comes from (import.meta.url).
    /// </summary>
    sealed class JsHost : Host
    {
        /// <summary>
        /// Marks the module of a script written into a document, as it has to be told apart from
        /// the others in there, while its address is the one of the document.
        /// </summary>
        public const String InlineModuleMark = "#inline-module-";

        private readonly IDocument _document;

        public JsHost(IDocument document)
        {
            _document = document;
        }

        public override List<KeyValuePair<JsValue, JsValue>> GetImportMetaProperties(Module module)
        {
            var properties = base.GetImportMetaProperties(module) ?? new List<KeyValuePair<JsValue, JsValue>>();
            properties.Add(new KeyValuePair<JsValue, JsValue>("url", GetAddress(module.Location)));
            return properties;
        }

        private String GetAddress(String location)
        {
            var mark = location?.IndexOf(InlineModuleMark, StringComparison.Ordinal) ?? -1;

            if (mark >= 0)
            {
                return location.Substring(0, mark);
            }

            return location != null && Uri.TryCreate(location, UriKind.Absolute, out var uri) && uri.Scheme.IsOneOf(ProtocolNames.Http, ProtocolNames.Https)
                ? location
                : _document.BaseUri;
        }
    }
}
