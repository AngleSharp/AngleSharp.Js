namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class NodeFilterInstance : ObjectInstance
    {
        public NodeFilterInstance(Engine engine)
            : base(engine)
        {
            FastAddProperty("FILTER_ACCEPT", (UInt32)(AngleSharp.Dom.FilterResult.Accept), false, true, false);
            FastAddProperty("FILTER_REJECT", (UInt32)(AngleSharp.Dom.FilterResult.Reject), false, true, false);
            FastAddProperty("FILTER_SKIP", (UInt32)(AngleSharp.Dom.FilterResult.Skip), false, true, false);
            FastAddProperty("SHOW_ALL", (UInt32)(AngleSharp.Dom.FilterSettings.All), false, true, false);
            FastAddProperty("SHOW_ELEMENT", (UInt32)(AngleSharp.Dom.FilterSettings.Element), false, true, false);
            FastAddProperty("SHOW_ATTRIBUTE", (UInt32)(AngleSharp.Dom.FilterSettings.Attribute), false, true, false);
            FastAddProperty("SHOW_TEXT", (UInt32)(AngleSharp.Dom.FilterSettings.Text), false, true, false);
            FastAddProperty("SHOW_CDATA_SECTION", (UInt32)(AngleSharp.Dom.FilterSettings.CharacterData), false, true, false);
            FastAddProperty("SHOW_ENTITY_REFERENCE", (UInt32)(AngleSharp.Dom.FilterSettings.EntityReference), false, true, false);
            FastAddProperty("SHOW_ENTITY", (UInt32)(AngleSharp.Dom.FilterSettings.Entity), false, true, false);
            FastAddProperty("SHOW_PROCESSING_INSTRUCTION", (UInt32)(AngleSharp.Dom.FilterSettings.ProcessingInstruction), false, true, false);
            FastAddProperty("SHOW_COMMENT", (UInt32)(AngleSharp.Dom.FilterSettings.Comment), false, true, false);
            FastAddProperty("SHOW_DOCUMENT", (UInt32)(AngleSharp.Dom.FilterSettings.Document), false, true, false);
            FastAddProperty("SHOW_DOCUMENT_TYPE", (UInt32)(AngleSharp.Dom.FilterSettings.DocumentType), false, true, false);
            FastAddProperty("SHOW_DOCUMENT_FRAGMENT", (UInt32)(AngleSharp.Dom.FilterSettings.DocumentFragment), false, true, false);
            FastAddProperty("SHOW_NOTATION", (UInt32)(AngleSharp.Dom.FilterSettings.Notation), false, true, false);
        }

        public static NodeFilterInstance CreateNodeFilterObject(Engine engine)
        {
            var obj = new NodeFilterInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "NodeFilter"; }
        }

        public FilterResult RefNodeFilter
        {
            get;
            set;
        }
    }
}