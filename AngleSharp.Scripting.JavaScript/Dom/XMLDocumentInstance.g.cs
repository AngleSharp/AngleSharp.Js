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
        public XMLDocumentInstance(Engine engine)
            : base(engine)
        {
        }

        public static XMLDocumentInstance CreateXMLDocumentObject(Engine engine)
        {
            var obj = new XMLDocumentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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