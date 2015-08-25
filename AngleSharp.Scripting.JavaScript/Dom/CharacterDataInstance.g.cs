namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CharacterDataInstance : NodeInstance
    {
        readonly EngineInstance _engine;

        public CharacterDataInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CharacterDataInstance CreateCharacterDataObject(EngineInstance engine)
        {
            var obj = new CharacterDataInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CharacterData"; }
        }

        public ICharacterData RefCharacterData
        {
            get;
            set;
        }
    }
}