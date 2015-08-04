namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLAllCollectionInstance : HTMLCollectionInstance
    {
        public HTMLAllCollectionInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLAllCollectionInstance CreateHTMLAllCollectionObject(Engine engine)
        {
            var obj = new HTMLAllCollectionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLAllCollection"; }
        }

        public IHtmlAllCollection RefHTMLAllCollection
        {
            get;
            set;
        }
    }
}