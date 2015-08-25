namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Xml;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class XMLDocumentInstance : DocumentInstance
    {
        readonly EngineInstance _engine;

        public XMLDocumentInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static XMLDocumentInstance CreateXMLDocumentObject(EngineInstance engine)
        {
            var obj = new XMLDocumentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "XMLDocument"; }
        }

        public IXmlDocument RefXMLDocument
        {
            get;
            set;
        }
    }
}