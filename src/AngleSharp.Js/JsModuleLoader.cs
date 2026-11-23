namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Text;
    using Jint;
    using Jint.Native;
    using Jint.Runtime;
    using Jint.Runtime.Modules;
    using System.Collections.Generic;
    using System.IO;
    using System;
    using System.Linq;

    internal class JsModuleLoader : ModuleLoader, IModuleLoader
    {
        private readonly EngineInstance _instance;
        private readonly IResourceLoader _resourceLoader;
        private readonly IElement _scriptElement;
        private readonly IDocument _document;
        private JavaScriptException _failure;

        public JsModuleLoader(EngineInstance instance, IDocument document)
        {
            _instance = instance;
            _resourceLoader = document.Context.GetService<IResourceLoader>();
            _scriptElement = document.CreateElement(TagNames.Script);
            _document = document;
        }

        //  The key of a resolved module is its address, and its Uri stays null: Jint gives a module
        //  loaded with a Uri only the path of it as location, so the modules it imports would lose
        //  the origin. With the key as location, they are resolved against the full address.
        public override ResolvedSpecifier Resolve(string referencingModuleLocation, ModuleRequest moduleRequest)
        {
            var specifier = moduleRequest.Specifier;

            if (referencingModuleLocation != null && _instance.ImportMap.Scopes.Count > 0)
            {
                foreach (var scopePath in _instance.ImportMap.Scopes.Keys.OrderByDescending(k => k.Length))
                {
                    if (referencingModuleLocation.Contains(scopePath))
                    {
                        var scopeImports = _instance.ImportMap.Scopes[scopePath];

                        if (TryMap(scopeImports, specifier, out var scopeModuleUrl))
                        {
                            return Resolved(moduleRequest, scopeModuleUrl);
                        }
                    }
                }
            }

            if (TryMap(_instance.ImportMap.Imports, specifier, out var moduleUrl))
            {
                return Resolved(moduleRequest, moduleUrl);
            }

            if (specifier.StartsWith("/") || specifier.StartsWith("./") || specifier.StartsWith("../"))
            {
                return Resolved(moduleRequest, new Url(GetBaseUrl(referencingModuleLocation), specifier));
            }

            var url = new Url(specifier);

            if (!url.IsInvalid && url.IsAbsolute)
            {
                return Resolved(moduleRequest, url);
            }

            //  A bare name, which no address belongs to. The module scripts of the document are
            //  added to the engine under such a name, so it must stay what it is to find them.
            return new ResolvedSpecifier(moduleRequest, specifier, null, SpecifierType.Bare);
        }

        //  A CSS module script (import ... with { type: 'css' }) has the style sheet it gives as
        //  its default export, a constructed one, as the page adopts it like such one. Jint's own
        //  loading can't be overridden, so the interface is implemented anew.
        Module IModuleLoader.LoadModule(Engine engine, ResolvedSpecifier resolved)
        {
            _failure = null;

            try
            {
                return LoadModule(engine, resolved);
            }
            catch (JavaScriptException) when (_failure != null)
            {
                //  Jint replaces what went wrong with a string telling the module could not be loaded
                throw _failure;
            }
        }

        private new Module LoadModule(Engine engine, ResolvedSpecifier resolved)
        {
            if (IsCssModule(resolved.ModuleRequest))
            {
                var source = FetchModule(engine, GetModuleUrl(resolved));
                var constructor = engine.GetValue("CSSStyleSheet");

                if (!constructor.IsConstructor())
                {
                    throw new JavaScriptException(engine.Intrinsics.TypeError, $"The CSS module '{resolved.Key}' needs style sheets to be constructed.");
                }

                var sheet = engine.Construct(constructor);
                engine.Invoke(sheet.Get("replaceSync"), sheet, new Object[] { source });
                return ModuleFactory.BuildJsonModule(engine, sheet, resolved.Key);
            }

            return base.LoadModule(engine, resolved);
        }

        private static Boolean IsCssModule(ModuleRequest request) =>
            request.Attributes?.Any(attribute => attribute.Key == "type" && attribute.Value == "css") ?? false;

        //  Every module comes through AngleSharp's resource loader, whatever its scheme, so the
        //  requesters the host registered decide what a page can reach.
        protected override string LoadModuleContents(Engine engine, ResolvedSpecifier resolved)
        {
            return FetchModule(engine, GetModuleUrl(resolved));
        }

        protected override Byte[] LoadModuleContentsAsBytes(Engine engine, ResolvedSpecifier resolved)
        {
            return FetchModuleBytes(engine, GetModuleUrl(resolved));
        }

        //  A key of the map is either the whole specifier, or, ending with a slash, a prefix of it,
        //  whose address the rest of the specifier is appended to (the longest prefix wins).
        private Boolean TryMap(Dictionary<String, Uri> imports, String specifier, out Url url)
        {
            if (imports.TryGetValue(specifier, out var address))
            {
                url = new Url(GetDocumentBaseUrl(), address.OriginalString);
                return true;
            }

            var prefix = imports.Keys
                .Where(key => key.EndsWith("/") && specifier.StartsWith(key, StringComparison.Ordinal))
                .OrderByDescending(key => key.Length)
                .FirstOrDefault();

            if (prefix != null)
            {
                var target = imports[prefix].OriginalString;

                if (target.EndsWith("/"))
                {
                    url = new Url(new Url(GetDocumentBaseUrl(), target), specifier.Substring(prefix.Length));
                    return true;
                }
            }

            url = null;
            return false;
        }

        private static ResolvedSpecifier Resolved(ModuleRequest moduleRequest, Url url)
        {
            return new ResolvedSpecifier(moduleRequest, url.Href, null, SpecifierType.RelativeOrAbsolute);
        }

        //  A module resolves against the address of what imports it, which is a module or a script
        //  that was loaded; one written into the document resolves against the base URL of it.
        private Url GetBaseUrl(string referencingModuleLocation)
        {
            if (referencingModuleLocation != null)
            {
                var url = new Url(referencingModuleLocation);

                if (!url.IsInvalid && url.IsAbsolute)
                {
                    return url;
                }
            }

            return GetDocumentBaseUrl();
        }

        private Url GetDocumentBaseUrl()
        {
            return _document.BaseUrl ?? new Url(_document.Url);
        }

        private static Url GetModuleUrl(ResolvedSpecifier resolved)
        {
            if (resolved.Type != SpecifierType.RelativeOrAbsolute)
            {
                throw new NotSupportedException($"The module '{resolved.ModuleRequest.Specifier}' could not be resolved to a URL.");
            }

            return new Url(resolved.Key);
        }

        private string FetchModule(Engine engine, Url moduleUrl)
        {
            if (_resourceLoader == null)
            {
                return string.Empty;
            }

            var response = Fetch(engine, moduleUrl);

            string content;

            using (var streamReader = new StreamReader(response.Content))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
        }

        private Byte[] FetchModuleBytes(Engine engine, Url moduleUrl)
        {
            if (_resourceLoader == null)
            {
                return Array.Empty<Byte>();
            }

            var response = Fetch(engine, moduleUrl);

            using (var content = new MemoryStream())
            {
                response.Content.CopyTo(content);
                return content.ToArray();
            }
        }

        //  A module that could not be fetched, or was answered with an error (e.g. a page telling it
        //  was not found), is a TypeError, which is what an import() of it rejects with.
        private IResponse Fetch(Engine engine, Url moduleUrl)
        {
            var request = new ResourceRequest(_scriptElement, moduleUrl);
            var response = _resourceLoader.FetchAsync(request).Task.GetAwaiter().GetResult();

            if (response == null || (Int32)response.StatusCode < 200 || (Int32)response.StatusCode > 299)
            {
                response?.Dispose();
                _failure = new JavaScriptException(engine.Intrinsics.TypeError, $"Failed to fetch the module '{moduleUrl.Href}'.");
                throw _failure;
            }

            return response;
        }
    }
}
