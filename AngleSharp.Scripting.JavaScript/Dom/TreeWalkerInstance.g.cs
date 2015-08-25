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
        readonly EngineInstance _engine;

        public TreeWalkerInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static TreeWalkerInstance CreateTreeWalkerObject(EngineInstance engine)
        {
            var obj = new TreeWalkerInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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