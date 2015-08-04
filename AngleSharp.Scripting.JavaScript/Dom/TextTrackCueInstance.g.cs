namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TextTrackCueInstance : EventTargetInstance
    {
        public TextTrackCueInstance(Engine engine)
            : base(engine)
        {
        }

        public static TextTrackCueInstance CreateTextTrackCueObject(Engine engine)
        {
            var obj = new TextTrackCueInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TextTrackCue"; }
        }

        public ITextTrackCue RefTextTrackCue
        {
            get;
            set;
        }
    }
}