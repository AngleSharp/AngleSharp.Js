namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TextTrackModeInstance : ObjectInstance
    {
        public TextTrackModeInstance(Engine engine)
            : base(engine)
        {
            FastAddProperty("disabled", (UInt32)(AngleSharp.Dom.Media.TextTrackMode.Disabled), false, true, false);
            FastAddProperty("hidden", (UInt32)(AngleSharp.Dom.Media.TextTrackMode.Hidden), false, true, false);
            FastAddProperty("showing", (UInt32)(AngleSharp.Dom.Media.TextTrackMode.Showing), false, true, false);
        }

        public static TextTrackModeInstance CreateTextTrackModeObject(Engine engine)
        {
            var obj = new TextTrackModeInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TextTrackMode"; }
        }

        public TextTrackMode RefTextTrackMode
        {
            get;
            set;
        }
    }
}