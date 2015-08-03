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
        public CharacterDataInstance(Engine engine)
            : base(engine)
        {
        }

        public static CharacterDataInstance CreateCharacterDataObject(Engine engine)
        {
            var obj = new CharacterDataInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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