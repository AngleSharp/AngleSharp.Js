namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TextInstance : CharacterDataInstance
    {
        public TextInstance(Engine engine)
            : base(engine)
        {
        }

        public static TextInstance CreateTextObject(Engine engine)
        {
            var obj = new TextInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Text"; }
        }

        public IText RefText
        {
            get;
            set;
        }
    }
}