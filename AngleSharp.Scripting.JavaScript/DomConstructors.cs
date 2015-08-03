namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native.Function;
    using Jint.Native.Object;

    partial class DomConstructors
    {
        readonly EngineInstance _engine;

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
            var constructors = GetType().GetProperties();

            foreach (var constructor in constructors)
                obj.FastAddProperty(constructor.Name, constructor.GetValue(this) as FunctionInstance, true, false, true);
        }

        partial void Setup(EngineInstance engine);
    }
}
