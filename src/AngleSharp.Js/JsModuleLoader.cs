using AngleSharp.Io;
using AngleSharp.Text;
using Jint;
using Jint.Runtime.Modules;

namespace AngleSharp.Js
{
    internal class JsModuleLoader : DefaultModuleLoader
    {
        private readonly EngineInstance _instance;

        public JsModuleLoader(EngineInstance instance, string basePath, bool restrictToBasePath = true) : base (basePath, restrictToBasePath)
        {
            _instance = instance;
        }

        protected override string LoadModuleContents(Engine engine, ResolvedSpecifier resolved)
        {
            if (resolved.Uri?.Scheme.IsOneOf(ProtocolNames.Http, ProtocolNames.Https) == true)
            {
                return _instance.FetchModule(resolved.Uri);
            }
            else
            {
                return base.LoadModuleContents(engine, resolved);
            }
        }
    }
}
