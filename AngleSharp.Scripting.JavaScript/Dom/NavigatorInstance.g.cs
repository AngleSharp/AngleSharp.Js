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
        public NavigatorInstance(Engine engine)
            : base(engine)
        {
        }

        public static NavigatorInstance CreateNavigatorObject(Engine engine)
        {
            var obj = new NavigatorInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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