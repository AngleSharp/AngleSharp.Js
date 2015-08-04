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
        public DOMSettableTokenListInstance(Engine engine)
            : base(engine)
        {
        }

        public static DOMSettableTokenListInstance CreateDOMSettableTokenListObject(Engine engine)
        {
            var obj = new DOMSettableTokenListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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