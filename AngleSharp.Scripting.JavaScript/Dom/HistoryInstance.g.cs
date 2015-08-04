namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HistoryInstance : ObjectInstance
    {
        public HistoryInstance(Engine engine)
            : base(engine)
        {
        }

        public static HistoryInstance CreateHistoryObject(Engine engine)
        {
            var obj = new HistoryInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "History"; }
        }

        public IHistory RefHistory
        {
            get;
            set;
        }
    }
}