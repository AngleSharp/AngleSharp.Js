namespace AngleSharp.Js
{
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;

    sealed class DomDelegateInstance : FunctionInstance
    {
        private readonly EngineInstance _instance;
        private readonly Func<JsValue, JsValue[], JsValue> _func;
        private readonly String _officialName;

        public DomDelegateInstance(EngineInstance engine, String officialName, Func<JsValue, JsValue[], JsValue> func)
            : base(engine.Jint, Array.Empty<String>(), null, false)
        {
            var toString = new ClrFunctionInstance(Engine, ToString);
            _instance = engine;
            _func = func;
            _officialName = officialName;
            FastAddProperty("toString", toString, true, false, true);
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments) =>
            _func.Invoke(thisObject, arguments);

        private JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            var func = thisObj.TryCast<FunctionInstance>() ??
                throw new JavaScriptException(Engine.TypeError, "Function object expected.");
            
            return $"function {_officialName}() {{ [native code] }}";
        }
    }
}
