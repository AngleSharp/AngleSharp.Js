namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System.Reflection;

    sealed class DomFunctionInstance : FunctionInstance
    {
        readonly MethodInfo _method;
        readonly EngineInstance _engine;

        public DomFunctionInstance(EngineInstance engine, MethodInfo method)
            : base(engine.Jint, method.GetParameterNames(), null, false)
        {
            _engine = engine;
            _method = method;

            FastAddProperty("toString", new ClrFunctionInstance(Engine, ToString), true, false, true);
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
                        var parameters = _engine.BuildArgs(_method, arguments);
                        return _method.Invoke(node.Value, parameters).ToJsValue(_engine);
                    }
                    catch
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
            return string.Format("function {0} () {{ [native code] }}", officialName);
        }
    }
}
