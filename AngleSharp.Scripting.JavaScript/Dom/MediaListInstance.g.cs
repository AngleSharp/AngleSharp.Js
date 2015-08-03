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
        public MediaListInstance(Engine engine)
            : base(engine)
        {
        }

        public static MediaListInstance CreateMediaListObject(Engine engine)
        {
            var obj = new MediaListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefMediaList[index]);
            return base.Get(propertyName);
        }


        public IMediaList RefMediaList
        {
            get;
            set;
        }
    }
}