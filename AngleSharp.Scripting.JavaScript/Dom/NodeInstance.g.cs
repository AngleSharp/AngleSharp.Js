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
        public NodeInstance(Engine engine)
            : base(engine)
        {
        }

        public static NodeInstance CreateNodeObject(Engine engine)
        {
            var obj = new NodeInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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