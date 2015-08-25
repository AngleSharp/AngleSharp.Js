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
        readonly EngineInstance _engine;

        public AudioTrackInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static AudioTrackInstance CreateAudioTrackObject(EngineInstance engine)
        {
            var obj = new AudioTrackInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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