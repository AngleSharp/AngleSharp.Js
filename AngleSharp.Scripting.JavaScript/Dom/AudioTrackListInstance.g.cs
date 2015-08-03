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
        public AudioTrackListInstance(Engine engine)
            : base(engine)
        {
        }

        public static AudioTrackListInstance CreateAudioTrackListObject(Engine engine)
        {
            var obj = new AudioTrackListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefAudioTrackList[index]);
            return base.Get(propertyName);
        }


        public IAudioTrackList RefAudioTrackList
        {
            get;
            set;
        }
    }
}