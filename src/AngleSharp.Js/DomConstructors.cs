namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native.Function;
    using Jint.Native.Object;
    using System.Reflection;

    partial class DomConstructors
    {
        private readonly EngineInstance _engine;

        public DomConstructors(EngineInstance engine)
        {
            Object = engine.Jint.Object;
            _engine = engine;
        }

        public ObjectConstructor Object
        {
            get;
            private set;
        }

        public void Configure()
        {
            Setup(_engine);
        }

        public void AttachConstructors(ObjectInstance obj)
        {
            var properties = GetType().GetTypeInfo().DeclaredProperties;

            foreach (var property in properties)
            {
                var func = property.GetValue(this) as FunctionInstance;
                obj.FastAddProperty(property.Name, func, true, false, true);
            }
        }

        partial void Setup(EngineInstance engine);
    }
}
