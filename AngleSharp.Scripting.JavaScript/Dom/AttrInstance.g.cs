namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class AttrInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public AttrInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static AttrInstance CreateAttrObject(EngineInstance engine)
        {
            var obj = new AttrInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Attr"; }
        }

        public IAttr RefAttr
        {
            get;
            set;
        }
    }
}