namespace AngleSharp.Js
{
    using Jint.Native.Object;
    using Jint.Runtime.Descriptors;
    using System;

    sealed class DomNodeInstance : ObjectInstance
    {
        private readonly EngineInstance _instance;
        private readonly Object _value;

        public DomNodeInstance(EngineInstance engine, Object value)
            : base(engine.Jint)
        {
            _instance = engine;
            _value = value;
            
            Extensible = true;
            Prototype = engine.GetDomPrototype(value.GetType());
        }

        public Object Value => _value;

        public override String Class => Prototype.Class;

        public override PropertyDescriptor GetOwnProperty(String propertyName)
        {
            if (Prototype is DomPrototypeInstance prototype && prototype.TryGetFromIndex(_value, propertyName, out var descriptor))
            {
                return descriptor;
            }

            return base.GetOwnProperty(propertyName);
        }
    }
}
