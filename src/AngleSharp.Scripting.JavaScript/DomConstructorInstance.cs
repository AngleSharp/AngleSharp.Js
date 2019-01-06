namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;
    using System.Reflection;

    sealed class DomConstructorInstance : FunctionInstance, IConstructor
    {
        private readonly ConstructorInfo _constructor;
        private readonly EngineInstance _engine;
        private readonly ObjectInstance _prototype;

        public DomConstructorInstance(EngineInstance engine, Type type)
            : base(engine.Jint, null, null, false)
        {
            var toString = new ClrFunctionInstance(Engine, ToString);
            _prototype = engine.GetDomPrototype(type);
            _engine = engine;
            FastAddProperty("toString", toString, true, false, true);
            FastAddProperty("prototype", _prototype, false, false, false);
            _prototype.FastAddProperty("constructor", this, true, false, true);
        }

        public DomConstructorInstance(EngineInstance engine, ConstructorInfo constructor)
            : this(engine, constructor.DeclaringType)
        {
            _constructor = constructor;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            if (_constructor != null)
                throw new JavaScriptException("Only call the constructor with the new keyword.");

            return ((IConstructor)this).Construct(arguments);
        }

        ObjectInstance IConstructor.Construct(JsValue[] arguments)
        {
            if (_constructor == null)
                throw new JavaScriptException("Illegal constructor.");

            try
            {
                var parameters = _engine.BuildArgs(_constructor, arguments);
                var obj = _constructor.Invoke(parameters);
                return _engine.GetDomNode(obj);
            }
            catch
            {
                throw new JavaScriptException(_engine.Jint.Error);
            }
        }

        private JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return $"function {_prototype.Class}() {{ [native code] }}";
        }
    }
}
