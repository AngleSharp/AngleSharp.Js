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
            var prototype = Prototype as DomPrototypeInstance;
            var descriptor = default(PropertyDescriptor);
            var result = prototype?.TryGetFromIndex(_value, propertyName, out descriptor) ?? false;

            if (!result)
            {
                descriptor = base.GetOwnProperty(propertyName);
            }
            
            return descriptor;
        }
    }
}
