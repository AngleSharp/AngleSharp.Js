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
        public VideoTrackListInstance(Engine engine)
            : base(engine)
        {
        }

        public static VideoTrackListInstance CreateVideoTrackListObject(Engine engine)
        {
            var obj = new VideoTrackListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefVideoTrackList[index]);
            return base.Get(propertyName);
        }


        public IVideoTrackList RefVideoTrackList
        {
            get;
            set;
        }
    }
}