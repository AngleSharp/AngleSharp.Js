namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DOMErrorInstance : ObjectInstance
    {
        public DOMErrorInstance(Engine engine)
            : base(engine)
        {
            FastAddProperty("INDEX_SIZE_ERR", (UInt32)(AngleSharp.Dom.DomError.IndexSizeError), false, true, false);
            FastAddProperty("DOMSTRING_SIZE_ERR", (UInt32)(AngleSharp.Dom.DomError.DomStringSize), false, true, false);
            FastAddProperty("HIERARCHY_REQUEST_ERR", (UInt32)(AngleSharp.Dom.DomError.HierarchyRequest), false, true, false);
            FastAddProperty("WRONG_DOCUMENT_ERR", (UInt32)(AngleSharp.Dom.DomError.WrongDocument), false, true, false);
            FastAddProperty("INVALID_CHARACTER_ERR", (UInt32)(AngleSharp.Dom.DomError.InvalidCharacter), false, true, false);
            FastAddProperty("NO_DATA_ALLOWED_ERR", (UInt32)(AngleSharp.Dom.DomError.NoDataAllowed), false, true, false);
            FastAddProperty("NO_MODIFICATION_ALLOWED_ERR", (UInt32)(AngleSharp.Dom.DomError.NoModificationAllowed), false, true, false);
            FastAddProperty("NOT_FOUND_ERR", (UInt32)(AngleSharp.Dom.DomError.NotFound), false, true, false);
            FastAddProperty("NOT_SUPPORTED_ERR", (UInt32)(AngleSharp.Dom.DomError.NotSupported), false, true, false);
            FastAddProperty("INUSE_ATTRIBUTE_ERR", (UInt32)(AngleSharp.Dom.DomError.InUse), false, true, false);
            FastAddProperty("INVALID_STATE_ERR", (UInt32)(AngleSharp.Dom.DomError.InvalidState), false, true, false);
            FastAddProperty("SYNTAX_ERR", (UInt32)(AngleSharp.Dom.DomError.Syntax), false, true, false);
            FastAddProperty("INVALID_MODIFICATION_ERR", (UInt32)(AngleSharp.Dom.DomError.InvalidModification), false, true, false);
            FastAddProperty("NAMESPACE_ERR", (UInt32)(AngleSharp.Dom.DomError.Namespace), false, true, false);
            FastAddProperty("INVALID_ACCESS_ERR", (UInt32)(AngleSharp.Dom.DomError.InvalidAccess), false, true, false);
            FastAddProperty("VALIDATION_ERR", (UInt32)(AngleSharp.Dom.DomError.Validation), false, true, false);
            FastAddProperty("TYPE_MISMATCH_ERR", (UInt32)(AngleSharp.Dom.DomError.TypeMismatch), false, true, false);
            FastAddProperty("SECURITY_ERR", (UInt32)(AngleSharp.Dom.DomError.Security), false, true, false);
            FastAddProperty("NETWORK_ERR", (UInt32)(AngleSharp.Dom.DomError.Network), false, true, false);
            FastAddProperty("ABORT_ERR", (UInt32)(AngleSharp.Dom.DomError.Abort), false, true, false);
            FastAddProperty("URL_MISMATCH_ERR", (UInt32)(AngleSharp.Dom.DomError.UrlMismatch), false, true, false);
            FastAddProperty("QUOTA_EXCEEDED_ERR", (UInt32)(AngleSharp.Dom.DomError.QuotaExceeded), false, true, false);
            FastAddProperty("TIMEOUT_ERR", (UInt32)(AngleSharp.Dom.DomError.Timeout), false, true, false);
            FastAddProperty("INVALID_NODE_TYPE_ERR", (UInt32)(AngleSharp.Dom.DomError.InvalidNodeType), false, true, false);
            FastAddProperty("DATA_CLONE_ERR", (UInt32)(AngleSharp.Dom.DomError.DataClone), false, true, false);
        }

        public static DOMErrorInstance CreateDOMErrorObject(Engine engine)
        {
            var obj = new DOMErrorInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DOMError"; }
        }

        public DomError RefDOMError
        {
            get;
            set;
        }
    }
}