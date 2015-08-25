namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class AudioTrackListInstance : EventTargetInstance
    {
        readonly EngineInstance _engine;

        public AudioTrackListInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static AudioTrackListInstance CreateAudioTrackListObject(EngineInstance engine)
        {
            var obj = new AudioTrackListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "AudioTrackList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefAudioTrackList[index]);
            return base.Get(propertyName);
        }


        public IAudioTrackList RefAudioTrackList
        {
            get;
            set;
        }
    }
}