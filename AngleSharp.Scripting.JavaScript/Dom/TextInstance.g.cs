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
        readonly EngineInstance _engine;

        public TextInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static TextInstance CreateTextObject(EngineInstance engine)
        {
            var obj = new TextInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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