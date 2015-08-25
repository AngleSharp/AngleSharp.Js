namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DOMSettableTokenListInstance : DOMTokenListInstance
    {
        readonly EngineInstance _engine;

        public DOMSettableTokenListInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static DOMSettableTokenListInstance CreateDOMSettableTokenListObject(EngineInstance engine)
        {
            var obj = new DOMSettableTokenListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DOMSettableTokenList"; }
        }

        public ISettableTokenList RefDOMSettableTokenList
        {
            get;
            set;
        }
    }
}