namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class ElementInstance : NodeInstance
    {
        readonly EngineInstance _engine;

        public ElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static ElementInstance CreateElementObject(EngineInstance engine)
        {
            var obj = new ElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Element"; }
        }

        public IElement RefElement
        {
            get;
            set;
        }
    }
}