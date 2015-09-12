namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Attributes;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    static class Extensions
    {
        public static JsValue ToJsValue(this Object obj, EngineInstance engine)
        {
            if (obj == null)
                return JsValue.Undefined;

            if (obj is String)
                return new JsValue((String)obj);
            else if (obj is Int32)
                return new JsValue((Int32)obj);
            else if (obj is UInt32)
                return new JsValue((UInt32)obj);
            else if (obj is Double)
                return new JsValue((Double)obj);
            else if (obj is Single)
                return new JsValue((Single)obj);
            else if (obj is Boolean)
                return new JsValue((Boolean)obj);
            else if (obj is Enum)
                return new JsValue(Convert.ToInt32(obj));

            return engine.GetDomNode(obj);
        }

        public static ClrFunctionInstance AsValue(this Engine engine, Func<JsValue, JsValue[], JsValue> func)
        {
            return new ClrFunctionInstance(engine, func);
        }

        public static PropertyDescriptor AsProperty(this Engine engine, Func<JsValue, JsValue> getter, Action<JsValue, JsValue> setter)
        {
            return new PropertyDescriptor(new GetterFunctionInstance(engine, getter), new SetterFunctionInstance(engine, setter), true, true);
        }

        public static PropertyDescriptor AsProperty(this Engine engine, Func<JsValue, JsValue> getter)
        {
            return new PropertyDescriptor(new GetterFunctionInstance(engine, getter), null, true, false);
        }

        public static PropertyDescriptor AsProperty(this Engine engine, Action<JsValue, JsValue> setter)
        {
            return new PropertyDescriptor(null, new SetterFunctionInstance(engine, setter), true, false);
        }

        public static Object FromJsValue(this JsValue val)
        {
            switch (val.Type)
            {
                case Types.Boolean:
                    return val.AsBoolean();
                case Types.Number:
                    return val.AsNumber();
                case Types.String:
                    return val.AsString();
                case Types.Object:
                    var obj = val.AsObject();
                    var node = obj as DomNodeInstance;

                    if (node != null)
                        return node.Value;

                    return obj;
                case Types.Undefined:
                case Types.Null:
                    return null;
            }

            return val.ToObject();
        }

        public static Object As(this Object value, Type targetType, EngineInstance engine)
        {
            if (value == null)
                return value;

            var sourceType = value.GetType();

            if (sourceType == targetType || sourceType.IsSubclassOf(targetType) || targetType.IsInstanceOfType(value) || targetType.IsAssignableFrom(sourceType))
                return value;
            else if (sourceType == typeof(Double) && targetType == typeof(Int32))
                return (Int32)(Double)value;

            if (targetType.IsSubclassOf(typeof(Delegate)) && value is FunctionInstance)
                return targetType.ToDelegate((FunctionInstance)value, engine);

            var method = sourceType.PrepareConvert(targetType);

            if (method != null)
                return method.Invoke(value, null);

            throw new JavaScriptException("[Internal] Could not find corresponding cast target.");
        }

        public static Object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static MethodInfo PrepareConvert(this Type fromType, Type toType)
        {
            try
            {
                // Throws an exception if there is no conversion from fromType to toType
                var exp = Expression.Convert(Expression.Parameter(fromType, null), toType);
                return exp.Method;
            }
            catch
            {
                return null;
            }
        }

        public static Object[] BuildArgs(this EngineInstance context, MethodBase method, JsValue[] arguments)
        {
            var parameters = method.GetParameters();
            var max = parameters.Length;
            var args = new Object[max];

            if (max > 0 && parameters[max - 1].GetCustomAttribute<ParamArrayAttribute>() != null)
                max--;

            var n = Math.Min(arguments.Length, max);

            for (int i = 0; i < n; i++)
                args[i] = arguments[i].FromJsValue().As(parameters[i].ParameterType, context);

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

        public static String[] GetParameterNames(this MethodInfo method)
        {
            return method != null ? method.GetParameters().Select(m => m.Name).ToArray() : null;
        }

        public static void AddConstructors(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            foreach (var exportedType in type.Assembly.ExportedTypes)
                engine.AddConstructor(obj, exportedType);
        }

        public static void AddConstructor(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            var info = type.GetConstructors().FirstOrDefault(m => 
                m.GetCustomAttributes<DomConstructorAttribute>().Any());

            if (info != null)
            {
                var name = type.GetOfficialName();
                var constructor = new DomConstructorInstance(engine, info);
                obj.FastSetProperty(name, new PropertyDescriptor(constructor, false, true, false));
            }
        }

        public static String GetOfficialName(this MemberInfo member)
        {
            var names = member.GetCustomAttributes<DomNameAttribute>();
            var officalNameAttribute = names.FirstOrDefault();
            return officalNameAttribute != null ? officalNameAttribute.OfficialName : member.Name;
        }
    }
}
