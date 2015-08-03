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
        public ElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static ElementInstance CreateElementObject(Engine engine)
        {
            var obj = new ElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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