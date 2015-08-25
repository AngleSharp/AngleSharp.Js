namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DOMExceptionInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public DOMExceptionInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static DOMExceptionInstance CreateDOMExceptionObject(EngineInstance engine)
        {
            var obj = new DOMExceptionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DOMException"; }
        }

        public IDomException RefDOMException
        {
            get;
            set;
        }
    }
}