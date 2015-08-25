namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TextTrackInstance : EventTargetInstance
    {
        readonly EngineInstance _engine;

        public TextTrackInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static TextTrackInstance CreateTextTrackObject(EngineInstance engine)
        {
            var obj = new TextTrackInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TextTrack"; }
        }

        public ITextTrack RefTextTrack
        {
            get;
            set;
        }
    }
}