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
        public MediaControllerInstance(Engine engine)
            : base(engine)
        {
        }

        public static MediaControllerInstance CreateMediaControllerObject(Engine engine)
        {
            var obj = new MediaControllerInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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