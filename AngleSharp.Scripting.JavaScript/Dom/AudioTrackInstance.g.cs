namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class AudioTrackInstance : ObjectInstance
    {
        public AudioTrackInstance(Engine engine)
            : base(engine)
        {
        }

        public static AudioTrackInstance CreateAudioTrackObject(Engine engine)
        {
            var obj = new AudioTrackInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "AudioTrack"; }
        }

        public IAudioTrack RefAudioTrack
        {
            get;
            set;
        }
    }
}