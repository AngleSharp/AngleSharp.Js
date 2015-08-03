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
        public TextTrackInstance(Engine engine)
            : base(engine)
        {
        }

        public static TextTrackInstance CreateTextTrackObject(Engine engine)
        {
            var obj = new TextTrackInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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