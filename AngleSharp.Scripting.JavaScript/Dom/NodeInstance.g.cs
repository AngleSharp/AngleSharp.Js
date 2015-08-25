namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class NodeInstance : EventTargetInstance
    {
        readonly EngineInstance _engine;

        public NodeInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static NodeInstance CreateNodeObject(EngineInstance engine)
        {
            var obj = new NodeInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Node"; }
        }

        public INode RefNode
        {
            get;
            set;
        }
    }
}