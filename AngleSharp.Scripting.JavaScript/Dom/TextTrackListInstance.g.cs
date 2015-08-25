namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TextTrackListInstance : EventTargetInstance
    {
        readonly EngineInstance _engine;

        public TextTrackListInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static TextTrackListInstance CreateTextTrackListObject(EngineInstance engine)
        {
            var obj = new TextTrackListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TextTrackList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefTextTrackList[index]);
            return base.Get(propertyName);
        }


        public ITextTrackList RefTextTrackList
        {
            get;
            set;
        }
    }
}