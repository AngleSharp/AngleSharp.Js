namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native.Object;

    partial class DomConstructors
    {
        public DomConstructors(EngineInstance engine)
        {
            Object = engine.Jint.Object;
            Setup(engine);
        }

        public ObjectConstructor Object
        {
            get;
            private set;
        }

        partial void Setup(EngineInstance engine);
    }
}
