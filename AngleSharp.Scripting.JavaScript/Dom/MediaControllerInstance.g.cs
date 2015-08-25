namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MediaControllerInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public MediaControllerInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static MediaControllerInstance CreateMediaControllerObject(EngineInstance engine)
        {
            var obj = new MediaControllerInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MediaController"; }
        }

        public IMediaController RefMediaController
        {
            get;
            set;
        }
    }
}