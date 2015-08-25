namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DOMStringMapInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public DOMStringMapInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static DOMStringMapInstance CreateDOMStringMapObject(EngineInstance engine)
        {
            var obj = new DOMStringMapInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DOMStringMap"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            if (propertyName != null)
                return _engine.GetDomNode(RefDOMStringMap[propertyName]);

            return base.Get(propertyName);
        }

        
        public override void Put(String propertyName, JsValue value, Boolean throwOnError)
        {
            if (propertyName != null)
            {
                RefDOMStringMap[propertyName] = TypeConverter.ToString(value);
                return;
            }

            base.Put(propertyName, value, throwOnError);
        }

        
        public override Boolean Delete(String propertyName, Boolean throwOnError)
        {
            if (propertyName != null)
            {
                RefDOMStringMap.Remove(propertyName);
                return true;
            }

            return base.Delete(propertyName, throwOnError);
        }

        public IStringMap RefDOMStringMap
        {
            get;
            set;
        }
    }
}