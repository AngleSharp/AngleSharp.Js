namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Text;
    using Jint;
    using Jint.Runtime.Modules;
    using System.IO;
    using System;
    using System.Linq;

    internal class JsModuleLoader : DefaultModuleLoader, IModuleLoader
    {
        private readonly EngineInstance _instance;
        private readonly IResourceLoader _resourceLoader;
        private readonly IElement _scriptElement;
        private readonly string _documentUrl;
        private readonly IDocument _document;

        public JsModuleLoader(EngineInstance instance, IDocument document, bool restrictToBasePath = true) : base (document.Url, restrictToBasePath)
        {
            _instance = instance;
            _resourceLoader = document.Context.GetService<IResourceLoader>();
            _scriptElement = document.CreateElement(TagNames.Script);
            _documentUrl = document.Url;
            _document = document;
        }

        public override ResolvedSpecifier Resolve(string referencingModuleLocation, ModuleRequest moduleRequest)
        {
            if (referencingModuleLocation != null && _instance.ImportMap.Scopes.Count > 0)
            {
                foreach (var scopePath in _instance.ImportMap.Scopes.Keys.OrderByDescending(k => k.Length))
                {
                    if (referencingModuleLocation.Contains(scopePath))
                    {
                        var scopeImports = _instance.ImportMap.Scopes[scopePath];

                        if (scopeImports.TryGetValue(moduleRequest.Specifier, out var scopeModuleUrl))
                        {
                            if (!scopeModuleUrl.IsAbsoluteUri)
                            {
                                scopeModuleUrl = new Uri(new Uri(_documentUrl), scopeModuleUrl);
                            }

                            return new ResolvedSpecifier(
                                    moduleRequest,
                                    scopeModuleUrl.AbsoluteUri,
                                    scopeModuleUrl,
                                    SpecifierType.RelativeOrAbsolute);
                        }
                    }
                }
            }

            if (_instance.ImportMap.Imports.TryGetValue(moduleRequest.Specifier, out var moduleUrl))
            {
                if (!moduleUrl.IsAbsoluteUri)
                {
                    moduleUrl = new Uri(new Uri(_documentUrl), moduleUrl);
                }

                return new ResolvedSpecifier(
                        moduleRequest,
                        moduleUrl.AbsoluteUri,
                        moduleUrl,
                        SpecifierType.RelativeOrAbsolute);
            }

            // On the web a specifier is an address, found from the module asking for it
            if (TryResolveAddress(referencingModuleLocation, moduleRequest.Specifier, out var address))
            {
                return new ResolvedSpecifier(moduleRequest, address.AbsoluteUri, address, SpecifierType.RelativeOrAbsolute);
            }

            // Before passing to the base default module loader, make sure any relative paths are prepended with a dot
            // as Jint will otherwise resolve them as absolute file paths when running on Linux
            if (moduleRequest.Specifier.StartsWith("/"))
            {
                moduleRequest = new ModuleRequest($".{moduleRequest.Specifier}", moduleRequest.Attributes);
            }

            if (referencingModuleLocation?.StartsWith("/") == true)
            {
                referencingModuleLocation = $".{referencingModuleLocation}";
            }
            
            return base.Resolve(referencingModuleLocation, moduleRequest);
        }

        private bool TryResolveAddress(string referencingModuleLocation, string specifier, out Uri address)
        {
            address = null;
            var isRelative = specifier.StartsWith("/") || specifier.StartsWith("./") || specifier.StartsWith("../");

            if (!isRelative && !IsWebAddress(specifier))
            {
                return false;
            }

            var from = IsWebAddress(referencingModuleLocation) ? referencingModuleLocation : _document.BaseUri;
            return IsWebAddress(from) && Uri.TryCreate(new Uri(from), specifier, out address);
        }

        private static bool IsWebAddress(string value) =>
            value != null && Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme.IsOneOf(ProtocolNames.Http, ProtocolNames.Https);

        Module IModuleLoader.LoadModule(Engine engine, ResolvedSpecifier resolved)
        {
            var request = resolved.ModuleRequest;
            var isSource = !request.IsJsonModule() && !request.IsTextModule() && !request.IsBytesModule();

            if (isSource && resolved.Uri?.Scheme.IsOneOf(ProtocolNames.Http, ProtocolNames.Https) == true)
            {
                //  Jint names a module with an address by the path alone, while it is the address that
                //  errors and import.meta tell, and that other modules are found from
                var named = new ResolvedSpecifier(request, resolved.Uri.AbsoluteUri, null, resolved.Type);
                return ModuleFactory.BuildSourceTextModule(engine, named, FetchModule(resolved.Uri));
            }

            return base.LoadModule(engine, resolved);
        }

        protected override string LoadModuleContents(Engine engine, ResolvedSpecifier resolved)
        {
            if (resolved.Uri?.Scheme.IsOneOf(ProtocolNames.Http, ProtocolNames.Https) == true)
            {
                return FetchModule(resolved.Uri);
            }
            else
            {
                return base.LoadModuleContents(engine, resolved);
            }
        }

        private string FetchModule(Uri moduleUrl)
        {
            if (_resourceLoader == null)
            {
                return string.Empty;
            }

            var importUrl = Url.Convert(moduleUrl);

            var request = new ResourceRequest(_scriptElement, importUrl);

            var response = _resourceLoader.FetchAsync(request).Task.GetAwaiter().GetResult();

            if (response == null)
            {
                throw new InvalidOperationException($"The module {moduleUrl} could not be loaded.");
            }

            string content;

            using (var streamReader = new StreamReader(response.Content))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
        }
    }
}
