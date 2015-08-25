namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MediaControllerPlaybackStateInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public MediaControllerPlaybackStateInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
            FastAddProperty("waiting", (UInt32)(AngleSharp.Dom.Media.MediaControllerPlaybackState.Waiting), false, true, false);
            FastAddProperty("playing", (UInt32)(AngleSharp.Dom.Media.MediaControllerPlaybackState.Playing), false, true, false);
            FastAddProperty("ended", (UInt32)(AngleSharp.Dom.Media.MediaControllerPlaybackState.Ended), false, true, false);
        }

        public static MediaControllerPlaybackStateInstance CreateMediaControllerPlaybackStateObject(EngineInstance engine)
        {
            var obj = new MediaControllerPlaybackStateInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MediaControllerPlaybackState"; }
        }

        public MediaControllerPlaybackState RefMediaControllerPlaybackState
        {
            get;
            set;
        }
    }
}