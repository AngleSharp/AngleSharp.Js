namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using Jint.Runtime;
    using System.Reflection;

    sealed class DomConstructorInstance : FunctionInstance, IConstructor
    {
        readonly ConstructorInfo _constructor;
        readonly EngineInstance _engine;

        public DomConstructorInstance(EngineInstance engine, ConstructorInfo constructor)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            _constructor = constructor;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            throw new JavaScriptException("Only call the constructor with the new keyword.");
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
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
    }
}
