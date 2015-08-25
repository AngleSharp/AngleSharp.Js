namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTrackElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTrackElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("NONE", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.None), false, true, false);
            FastAddProperty("LOADING", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.Loading), false, true, false);
            FastAddProperty("LOADED", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.Loaded), false, true, false);
            FastAddProperty("ERROR", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.Error), false, true, false);
        }

        public static HTMLTrackElementInstance CreateHTMLTrackElementObject(EngineInstance engine)
        {
            var obj = new HTMLTrackElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTrackElement"; }
        }

        public IHtmlTrackElement RefHTMLTrackElement
        {
            get;
            set;
        }
    }
}