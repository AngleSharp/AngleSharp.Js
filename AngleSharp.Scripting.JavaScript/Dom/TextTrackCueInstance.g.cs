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
        readonly EngineInstance _engine;

        public TextTrackCueInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static TextTrackCueInstance CreateTextTrackCueObject(EngineInstance engine)
        {
            var obj = new TextTrackCueInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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