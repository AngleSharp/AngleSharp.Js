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
        readonly EngineInstance _engine;

        public ValidityStateInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static ValidityStateInstance CreateValidityStateObject(EngineInstance engine)
        {
            var obj = new ValidityStateInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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