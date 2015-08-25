namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Navigator;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class NavigatorInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public NavigatorInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static NavigatorInstance CreateNavigatorObject(EngineInstance engine)
        {
            var obj = new NavigatorInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Navigator"; }
        }

        public INavigator RefNavigator
        {
            get;
            set;
        }
    }
}