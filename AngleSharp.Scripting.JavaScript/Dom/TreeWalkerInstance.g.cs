namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TreeWalkerInstance : ObjectInstance
    {
        public TreeWalkerInstance(Engine engine)
            : base(engine)
        {
        }

        public static TreeWalkerInstance CreateTreeWalkerObject(Engine engine)
        {
            var obj = new TreeWalkerInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TreeWalker"; }
        }

        public ITreeWalker RefTreeWalker
        {
            get;
            set;
        }
    }
}