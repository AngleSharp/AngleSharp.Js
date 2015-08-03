namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class ValidityStateInstance : ObjectInstance
    {
        public ValidityStateInstance(Engine engine)
            : base(engine)
        {
        }

        public static ValidityStateInstance CreateValidityStateObject(Engine engine)
        {
            var obj = new ValidityStateInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "ValidityState"; }
        }

        public IValidityState RefValidityState
        {
            get;
            set;
        }
    }
}