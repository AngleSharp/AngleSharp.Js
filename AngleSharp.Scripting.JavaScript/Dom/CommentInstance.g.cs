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
        public CommentInstance(Engine engine)
            : base(engine)
        {
        }

        public static CommentInstance CreateCommentObject(Engine engine)
        {
            var obj = new CommentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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