namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TextTrackCueListInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public TextTrackCueListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static TextTrackCueListInstance CreateTextTrackCueListObject(EngineInstance engine)
        {
            var obj = new TextTrackCueListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TextTrackCueList"; }
        }

        public ITextTrackCueList RefTextTrackCueList
        {
            get;
            set;
        }
    }
}