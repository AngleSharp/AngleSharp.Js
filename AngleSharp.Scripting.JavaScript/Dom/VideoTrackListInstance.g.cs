namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class VideoTrackListInstance : EventTargetInstance
    {
        readonly EngineInstance _engine;

        public VideoTrackListInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static VideoTrackListInstance CreateVideoTrackListObject(EngineInstance engine)
        {
            var obj = new VideoTrackListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "VideoTrackList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefVideoTrackList[index]);
            return base.Get(propertyName);
        }


        public IVideoTrackList RefVideoTrackList
        {
            get;
            set;
        }
    }
}