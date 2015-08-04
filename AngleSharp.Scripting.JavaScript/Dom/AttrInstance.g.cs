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
        public AttrInstance(Engine engine)
            : base(engine)
        {
        }

        public static AttrInstance CreateAttrObject(Engine engine)
        {
            var obj = new AttrInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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