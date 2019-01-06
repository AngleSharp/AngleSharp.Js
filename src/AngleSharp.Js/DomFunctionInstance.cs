namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System.Reflection;

    sealed class DomFunctionInstance : FunctionInstance
    {
        private readonly EngineInstance _instance;
        private readonly MethodInfo _method;

        public DomFunctionInstance(EngineInstance engine, MethodInfo method)
            : base(engine.Jint, method.Name, method.GetParameterNames(), null, false)
        {
            var toString = new ClrFunctionInstance(Engine, "toString", ToString);
            _instance = engine;
            _method = method;
            FastAddProperty("toString", toString, true, false, true);
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            if (_method != null && thisObject.Type == Types.Object)
            {
                var node = thisObject.AsObject() as DomNodeInstance;

                if (node != null)
                {
                    try
                    {
                        var parameters = _instance.BuildArgs(_method, arguments);
                        return _method.Invoke(node.Value, parameters).ToJsValue(_instance);
                    }
                    catch (TargetInvocationException)
                    {
                        throw new JavaScriptException(Engine.Error);
                    }
                }
            }

            return JsValue.Undefined;
        }

        private JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            var func = thisObj.TryCast<FunctionInstance>();

            if (func == null)
            {
                throw new JavaScriptException(Engine.TypeError, "Function object expected.");
            }

            var officialName = _method.GetOfficialName();
            return $"function {officialName}() {{ [native code] }}";
        }
    }
}
