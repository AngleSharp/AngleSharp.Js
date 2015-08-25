namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class NamedNodeMapInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public NamedNodeMapInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static NamedNodeMapInstance CreateNamedNodeMapObject(EngineInstance engine)
        {
            var obj = new NamedNodeMapInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "NamedNodeMap"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefNamedNodeMap[index]);
            if (propertyName != null)
                return _engine.GetDomNode(RefNamedNodeMap[propertyName]);

            return base.Get(propertyName);
        }


        public INamedNodeMap RefNamedNodeMap
        {
            get;
            set;
        }
    }
}