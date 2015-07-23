namespace AngleSharp.Scripting.JavaScript
{
    using Attributes;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;
    using System.Linq;
    using System.Reflection;

    sealed class DomFunctionInstance : FunctionInstance
    {
        readonly MethodInfo _method;
        readonly DomNodeInstance _host;

        public DomFunctionInstance(DomNodeInstance host, MethodInfo method)
            : base(host.Engine, GetParameters(method), null, false)
        {
            _host = host;
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
                        return _method.Invoke(node.Value, BuildArgs(arguments)).ToJsValue(Engine);
                    }
                    catch
                    {
                        throw new JavaScriptException(Engine.Error);
                    }
                }
            }

            return JsValue.Undefined;
        }

        Object[] BuildArgs(JsValue[] arguments)
        {
            var parameters = _method.GetParameters();
            var max = parameters.Length;
            var args = new Object[max];

            if (max > 0 && parameters[max - 1].GetCustomAttribute<ParamArrayAttribute>() != null)
                max--;

            var n = Math.Min(arguments.Length, max);

            for (int i = 0; i < n; i++)
                args[i] = arguments[i].FromJsValue().As(parameters[i].ParameterType);

            for (int i = n; i < max; i++)
                args[i] = parameters[i].IsOptional ? parameters[i].DefaultValue : parameters[i].ParameterType.GetDefaultValue();

            if (max != parameters.Length)
            {
                var array = Array.CreateInstance(parameters[max].ParameterType.GetElementType(), Math.Max(0, arguments.Length - max));

                for (int i = max; i < arguments.Length; i++)
                    array.SetValue(arguments[i].FromJsValue(), i - max);

                args[max] = array;
            }

            return args;
        }

        static String[] GetParameters(MethodInfo method)
        {
            if (method == null)
                return new String[0];

            return method.GetParameters().Select(m => m.Name).ToArray();
        }

        private JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            var func = thisObj.TryCast<FunctionInstance>();

            if (func == null)
            {
                throw new JavaScriptException(Engine.TypeError, "Function object expected.");
            }

            var names = _method.GetCustomAttributes<DomNameAttribute>();

            var officialName = _method.Name;
            var officalNameAttribute = names.FirstOrDefault();

            if (officalNameAttribute != null)
                officialName = officalNameAttribute.OfficialName;

            return string.Format("function {0}() {{ [native code] }}", officialName);
        }
    }
}
