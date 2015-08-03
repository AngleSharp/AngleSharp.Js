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
        public DOMImplementationInstance(Engine engine)
            : base(engine)
        {
        }

        public static DOMImplementationInstance CreateDOMImplementationObject(Engine engine)
        {
            var obj = new DOMImplementationInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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