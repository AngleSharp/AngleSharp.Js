namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMediaElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLMediaElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("NETWORK_EMPTY", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.Empty), false, true, false);
            FastAddProperty("NETWORK_IDLE", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.Idle), false, true, false);
            FastAddProperty("NETWORK_LOADING", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.Loading), false, true, false);
            FastAddProperty("NETWORK_NO_SOURCE", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.NoSource), false, true, false);
            FastAddProperty("HAVE_NOTHING", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.Nothing), false, true, false);
            FastAddProperty("HAVE_METADATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.Metadata), false, true, false);
            FastAddProperty("HAVE_CURRENT_DATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.CurrentData), false, true, false);
            FastAddProperty("HAVE_FUTURE_DATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.FutureData), false, true, false);
            FastAddProperty("HAVE_ENOUGH_DATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.EnoughData), false, true, false);
        }

        public static HTMLMediaElementInstance CreateHTMLMediaElementObject(EngineInstance engine)
        {
            var obj = new HTMLMediaElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMediaElement"; }
        }

        public IHtmlMediaElement RefHTMLMediaElement
        {
            get;
            set;
        }
    }
}