namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MediaListInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public MediaListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static MediaListInstance CreateMediaListObject(EngineInstance engine)
        {
            var obj = new MediaListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MediaList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefMediaList[index]);
            return base.Get(propertyName);
        }


        public IMediaList RefMediaList
        {
            get;
            set;
        }
    }
}