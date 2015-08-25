namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CommentInstance : CharacterDataInstance
    {
        readonly EngineInstance _engine;

        public CommentInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CommentInstance CreateCommentObject(EngineInstance engine)
        {
            var obj = new CommentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Comment"; }
        }

        public IComment RefComment
        {
            get;
            set;
        }
    }
}