namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class NodeListInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public NodeListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static NodeListInstance CreateNodeListObject(EngineInstance engine)
        {
            var obj = new NodeListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "NodeList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefNodeList[index]);
            return base.Get(propertyName);
        }


        public INodeList RefNodeList
        {
            get;
            set;
        }
    }
}