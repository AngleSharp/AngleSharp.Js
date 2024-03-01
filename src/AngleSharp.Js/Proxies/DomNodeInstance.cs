namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using Jint.Native;
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

            Prototype = engine.GetDomPrototype(value.GetType());
        }

        public Object Value => _value;

        public override object ToObject() => _value;

        public override PropertyDescriptor GetOwnProperty(JsValue property)
        {
            if (Prototype is DomPrototypeInstance prototype)
            {
                if (prototype.TryGetFromIndex(_value, property.ToString(), out var descriptor))
                {
                    return descriptor;
                }
                else if (prototype.HasProperty(property))
                {
                    return prototype.GetProperty(property);
                }
            }

            return base.GetOwnProperty(property);
        }

        protected override void SetOwnProperty(JsValue property, PropertyDescriptor desc)
        {
            base.SetOwnProperty(property, desc);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(property, desc);
            }
        }

        public new void FastSetProperty(string name, PropertyDescriptor value)
        {
            base.FastSetProperty(name, value);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(name, value);
            }
        }

        public new void FastSetProperty(JsValue property, PropertyDescriptor value)
        {
            base.FastSetProperty(property, value);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(property, value);
            }
        }

        public new void FastSetDataProperty(string name, JsValue value)
        {
            base.FastSetDataProperty(name, value);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(name, new PropertyDescriptor(value, true, true, true));
            }
        }
    }
}
