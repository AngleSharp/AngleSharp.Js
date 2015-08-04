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
        public DOMExceptionInstance(Engine engine)
            : base(engine)
        {
        }

        public static DOMExceptionInstance CreateDOMExceptionObject(Engine engine)
        {
            var obj = new DOMExceptionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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