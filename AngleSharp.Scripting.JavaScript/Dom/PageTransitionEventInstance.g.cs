namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class PageTransitionEventInstance : EventInstance
    {
        public PageTransitionEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static PageTransitionEventInstance CreatePageTransitionEventObject(Engine engine)
        {
            var obj = new PageTransitionEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "PageTransitionEvent"; }
        }

        public PageTransitionEvent RefPageTransitionEvent
        {
            get;
            set;
        }
    }
}