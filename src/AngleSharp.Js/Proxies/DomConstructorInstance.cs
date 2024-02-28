namespace AngleSharp.Js
{
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Reflection;

    sealed class DomConstructorInstance : Constructor
    {
        private readonly ConstructorInfo _constructor;
        private readonly EngineInstance _instance;
        private readonly ObjectInstance _objectPrototype;

        public DomConstructorInstance(EngineInstance engine, Type type)
            : base(engine.Jint, type.GetOfficialName())
        {
            var toString = new ClrFunction(Engine, "toString", ToString);
            _objectPrototype = engine.GetDomPrototype(type);
            _instance = engine;
            FastSetProperty("toString", new PropertyDescriptor(toString, true, false, true));
            SetOwnProperty("prototype", new PropertyDescriptor(_objectPrototype, false, false, false));
            _objectPrototype.FastSetProperty("constructor", new PropertyDescriptor(this, true, false, true));
        }

        public DomConstructorInstance(EngineInstance engine, ConstructorInfo constructor)
            : this(engine, constructor.DeclaringType)
        {
            _constructor = constructor;
        }

        public override ObjectInstance Construct(JsValue[] arguments, JsValue newTarget)
        {
            if (_constructor == null)
            {
                throw new JavaScriptException("Illegal constructor.");
            }

            try
            {
                var parameters = _instance.BuildArgs(_constructor, arguments);
                var obj = _constructor.Invoke(parameters);
                return _instance.GetDomNode(obj);
            }
            catch
            {
                throw new JavaScriptException(_instance.Jint.Intrinsics.Error);
            }
        }

        protected override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            if (_constructor != null)
            {
                throw new JavaScriptException("Only call the constructor with the new keyword.");
            }

            return Construct(arguments, null);
        }

        private JsValue ToString(JsValue thisObj, JsValue[] arguments) => ToString();
    }
}
