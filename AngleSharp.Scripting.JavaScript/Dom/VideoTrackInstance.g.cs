namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class VideoTrackInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public VideoTrackInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static VideoTrackInstance CreateVideoTrackObject(EngineInstance engine)
        {
            var obj = new VideoTrackInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "VideoTrack"; }
        }

        public IVideoTrack RefVideoTrack
        {
            get;
            set;
        }
    }
}