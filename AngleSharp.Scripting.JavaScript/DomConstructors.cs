namespace AngleSharp.Scripting.JavaScript
{
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

        partial void Setup(EngineInstance engine);
    }
}
