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
        readonly EngineInstance _engine;

        public LocationInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static LocationInstance CreateLocationObject(EngineInstance engine)
        {
            var obj = new LocationInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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