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
        public VideoTrackInstance(Engine engine)
            : base(engine)
        {
        }

        public static VideoTrackInstance CreateVideoTrackObject(Engine engine)
        {
            var obj = new VideoTrackInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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