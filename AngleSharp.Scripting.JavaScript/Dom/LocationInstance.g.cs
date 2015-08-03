namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class LocationInstance : ObjectInstance
    {
        public LocationInstance(Engine engine)
            : base(engine)
        {
        }

        public static LocationInstance CreateLocationObject(Engine engine)
        {
            var obj = new LocationInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Location"; }
        }

        public ILocation RefLocation
        {
            get;
            set;
        }
    }
}