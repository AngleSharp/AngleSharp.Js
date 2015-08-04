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
        public NamedNodeMapInstance(Engine engine)
            : base(engine)
        {
        }

        public static NamedNodeMapInstance CreateNamedNodeMapObject(Engine engine)
        {
            var obj = new NamedNodeMapInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefNamedNodeMap[index]);
            if (propertyName != null)
                return Engine.Select(RefNamedNodeMap[propertyName]);

            return base.Get(propertyName);
        }


        public INamedNodeMap RefNamedNodeMap
        {
            get;
            set;
        }
    }
}