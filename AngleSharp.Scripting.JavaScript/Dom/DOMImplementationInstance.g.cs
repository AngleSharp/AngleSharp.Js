namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DOMImplementationInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public DOMImplementationInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static DOMImplementationInstance CreateDOMImplementationObject(EngineInstance engine)
        {
            var obj = new DOMImplementationInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DOMImplementation"; }
        }

        public IImplementation RefDOMImplementation
        {
            get;
            set;
        }
    }
}