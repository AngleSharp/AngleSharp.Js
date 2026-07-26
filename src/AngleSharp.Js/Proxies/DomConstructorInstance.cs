namespace AngleSharp.Js
{
    using AngleSharp.Js.Cache;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System.Reflection;

    sealed class DomConstructorInstance : Constructor
    {
        private readonly ConstructorInfo _constructor;
        private readonly EngineInstance _instance;
        private readonly ObjectInstance _objectPrototype;

        public DomConstructorInstance(EngineInstance engine, ConstructorDefinition definition)
            : base(engine.Jint, definition.Name)
        {
            var toString = new ClrFunction(Engine, "toString", ToString);
            _objectPrototype = engine.GetDomPrototype(definition.Type);
            _instance = engine;
            _constructor = definition.Info;
            FastSetProperty("toString", new PropertyDescriptor(toString, true, false, true));
            SetOwnProperty("prototype", new PropertyDescriptor(_objectPrototype, false, false, false));

            var constructor = new PropertyDescriptor(this, true, false, true);

            //  Every exposed type gets a constructor, so writing this directly would make
            //  each of their prototypes register its members right away.
            if (_objectPrototype is DomPrototypeInstance domPrototype)
            {
                domPrototype.DefineDeferredProperty("constructor", constructor);
            }
            else
            {
                _objectPrototype.FastSetProperty("constructor", constructor);
            }
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
