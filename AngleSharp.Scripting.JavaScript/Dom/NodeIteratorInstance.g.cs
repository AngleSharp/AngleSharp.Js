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
        readonly EngineInstance _engine;

        public NodeIteratorInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static NodeIteratorInstance CreateNodeIteratorObject(EngineInstance engine)
        {
            var obj = new NodeIteratorInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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