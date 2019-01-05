namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native.Object;
    using Jint.Runtime.Descriptors;
    using System;

    sealed class DomNodeInstance : ObjectInstance
    {
        private readonly EngineInstance _engine;
        private readonly Object _value;

        public DomNodeInstance(EngineInstance engine, Object value)
            : base(engine.Jint)
        {
            _engine = engine;
            _value = value;
            
            Extensible = true;
            Prototype = engine.GetDomPrototype(value.GetType());
        }

        public override String Class
        {
            get { return Prototype.Class; }
        }

        public Object Value
        {
            get { return _value; }
        }

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
