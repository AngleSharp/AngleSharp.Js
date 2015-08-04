namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class NodeIteratorInstance : ObjectInstance
    {
        public NodeIteratorInstance(Engine engine)
            : base(engine)
        {
        }

        public static NodeIteratorInstance CreateNodeIteratorObject(Engine engine)
        {
            var obj = new NodeIteratorInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "NodeIterator"; }
        }

        public INodeIterator RefNodeIterator
        {
            get;
            set;
        }
    }
}