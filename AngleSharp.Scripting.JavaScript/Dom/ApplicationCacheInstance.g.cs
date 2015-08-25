namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class ApplicationCacheInstance : EventTargetInstance
    {
        readonly EngineInstance _engine;

        public ApplicationCacheInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("UNCACHED", (UInt32)(AngleSharp.Dom.CacheStatus.Uncached), false, true, false);
            FastAddProperty("IDLE", (UInt32)(AngleSharp.Dom.CacheStatus.Idle), false, true, false);
            FastAddProperty("CHECKING", (UInt32)(AngleSharp.Dom.CacheStatus.Checking), false, true, false);
            FastAddProperty("DOWNLOADING", (UInt32)(AngleSharp.Dom.CacheStatus.Downloading), false, true, false);
            FastAddProperty("UPDATEREADY", (UInt32)(AngleSharp.Dom.CacheStatus.UpdateReady), false, true, false);
            FastAddProperty("OBSOLETE", (UInt32)(AngleSharp.Dom.CacheStatus.Obsolete), false, true, false);
        }

        public static ApplicationCacheInstance CreateApplicationCacheObject(EngineInstance engine)
        {
            var obj = new ApplicationCacheInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "ApplicationCache"; }
        }

        public IApplicationCache RefApplicationCache
        {
            get;
            set;
        }
    }
}