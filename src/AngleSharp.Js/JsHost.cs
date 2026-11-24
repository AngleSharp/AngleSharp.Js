namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Runtime.Interop;
    using Jint.Runtime;
    using Jint.Runtime.Modules;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Tells a module what the engine leaves to the host: where it comes from (import.meta.url) and
    /// what it would import (import.meta.resolve).
    /// </summary>
    sealed class JsHost : Host
    {
        /// <summary>
        /// Marks the module of a script written into a document, as it has to be told apart from
        /// the others in there, while its address is the one of the document.
        /// </summary>
        public const String InlineModuleMark = "#inline-module-";

        private readonly Engine _engine;
        private readonly IDocument _document;
        private readonly IModuleLoader _loader;

        public JsHost(Engine engine, IDocument document, IModuleLoader loader)
        {
            _engine = engine;
            _document = document;
            _loader = loader;
        }

        public override List<KeyValuePair<JsValue, JsValue>> GetImportMetaProperties(Module module)
        {
            var properties = base.GetImportMetaProperties(module) ?? new List<KeyValuePair<JsValue, JsValue>>();
            properties.Add(new KeyValuePair<JsValue, JsValue>("url", GetAddress(module.Location)));
            properties.Add(new KeyValuePair<JsValue, JsValue>("resolve", new ClrFunction(_engine, "resolve", (_, arguments) =>
                Resolve(module.Location, TypeConverter.ToString(arguments.At(0))), 1)));
            return properties;
        }

        //  the address the module would import, without importing it; a name nothing maps fails
        private JsValue Resolve(String location, String specifier)
        {
            var resolved = _loader.Resolve(location, new ModuleRequest(specifier, Array.Empty<ModuleImportAttribute>()));

            if (resolved.Type != SpecifierType.RelativeOrAbsolute)
            {
                throw new JavaScriptException(_engine.Intrinsics.TypeError, $"The specifier '{specifier}' could not be resolved.");
            }

            return resolved.Key;
        }

        private String GetAddress(String location)
        {
            var mark = location?.IndexOf(InlineModuleMark, StringComparison.Ordinal) ?? -1;

            if (mark >= 0)
            {
                return location.Substring(0, mark);
            }

            //  Any address counts, as a requester may serve any scheme; what has none is a module
            //  of a document without a web address, which goes by a generated name.
            if (location != null)
            {
                var url = new Url(location);

                if (!url.IsInvalid && url.IsAbsolute)
                {
                    return location;
                }
            }

            return _document.BaseUri;
        }
    }
}
