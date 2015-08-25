namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MediaErrorInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public MediaErrorInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
            FastAddProperty("MEDIA_ERR_ABORTED", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.Aborted), false, true, false);
            FastAddProperty("MEDIA_ERR_NETWORK", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.Network), false, true, false);
            FastAddProperty("MEDIA_ERR_DECODE", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.Decode), false, true, false);
            FastAddProperty("MEDIA_ERR_SRC_NOT_SUPPORTED", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.SourceNotSupported), false, true, false);
        }

        public static MediaErrorInstance CreateMediaErrorObject(EngineInstance engine)
        {
            var obj = new MediaErrorInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MediaError"; }
        }

        public IMediaError RefMediaError
        {
            get;
            set;
        }
    }
}