namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
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
            if (obj != null)
            {
                if (obj is String)
                {
                    return new JsValue((String)obj);
                }
                else if (obj is Int32)
                {
                    return new JsValue((Int32)obj);
                }
                else if (obj is UInt32)
                {
                    return new JsValue((UInt32)obj);
                }
                else if (obj is Double)
                {
                    return new JsValue((Double)obj);
                }
                else if (obj is Single)
                {
                    return new JsValue((Single)obj);
                }
                else if (obj is Boolean)
                {
                    return new JsValue((Boolean)obj);
                }
                else if (obj is Enum)
                {
                    return new JsValue(Convert.ToInt32(obj));
                }

                return engine.GetDomNode(obj);
            }

            return JsValue.Null;
        }

        public static ClrFunctionInstance AsValue(this Engine engine, Func<JsValue, JsValue[], JsValue> func) =>
            new ClrFunctionInstance(engine, func);

        public static PropertyDescriptor AsProperty(this Engine engine, Func<JsValue, JsValue> getter, Action<JsValue, JsValue> setter) =>
            new PropertyDescriptor(new GetterFunctionInstance(engine, getter), new SetterFunctionInstance(engine, setter), true, true);

        public static PropertyDescriptor AsProperty(this Engine engine, Func<JsValue, JsValue> getter) =>
            new PropertyDescriptor(new GetterFunctionInstance(engine, getter), null, true, false);

        public static PropertyDescriptor AsProperty(this Engine engine, Action<JsValue, JsValue> setter) =>
            new PropertyDescriptor(null, new SetterFunctionInstance(engine, setter), true, false);

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
                    return node != null ? node.Value : obj;
                case Types.Undefined:
                    return Undefined.Text;
                case Types.Null:
                    return null;
            }

            return val.ToObject();
        }

        public static Object As(this JsValue value, Type targetType, EngineInstance engine)
        {
            if (value != JsValue.Null)
            {
                if (targetType == typeof(Int32))
                {
                    return TypeConverter.ToInt32(value);
                }
                else if (targetType == typeof(Double))
                {
                    return TypeConverter.ToNumber(value);
                }
                else if (targetType == typeof(String))
                {
                    return value.IsPrimitive() ? TypeConverter.ToString(value) : value.ToString();
                }
                else if (targetType == typeof(Boolean))
                {
                    return TypeConverter.ToBoolean(value);
                }
                else if (targetType == typeof(UInt32))
                {
                    return TypeConverter.ToUint32(value);
                }
                else if (targetType == typeof(UInt16))
                {
                    return TypeConverter.ToUint16(value);
                }
                else
                {
                    return value.AsComplex(targetType, engine);
                }
            }

            return null;
        }

        public static Object GetDefaultValue(this Type type) =>
            type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;

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
            var offset = 0;

            if (parameters.Length > 0 && parameters[0].ParameterType == typeof(IWindow))
            {
                args[offset++] = context.Window.Value;
            }

            if (max > 0 && parameters[max - 1].GetCustomAttribute<ParamArrayAttribute>() != null)
            {
                max--;
            }

            var n = Math.Min(arguments.Length, max - offset);

            for (var i = 0; i < n; i++)
            {
                if (parameters[i].IsOptional && arguments[i].IsUndefined())
                {
                    args[i + offset] = parameters[i].DefaultValue;
                }
                else
                {
                    args[i + offset] = arguments[i].As(parameters[i].ParameterType, context);
                }
            }

            for (var i = n + offset; i < max; i++)
            {
                if (parameters[i].IsOptional)
                {
                    args[i] = parameters[i].DefaultValue;
                }
                else
                {
                    args[i] = parameters[i].ParameterType.GetDefaultValue();
                }
            }
                

            if (max != parameters.Length)
            {
                var array = Array.CreateInstance(parameters[max].ParameterType.GetElementType(), Math.Max(0, arguments.Length - max));

                for (var i = max; i < arguments.Length; i++)
                {
                    array.SetValue(arguments[i].FromJsValue(), i - max);
                }

                args[max] = array;
            }

            return args;
        }

        public static String[] GetParameterNames(this MethodInfo method) =>
            method?.GetParameters().Select(m => m.Name).ToArray();

        public static Assembly GetAssembly(this Type type) => type.GetTypeInfo().Assembly;

        public static void AddConstructors(this EngineInstance engine, ObjectInstance ctx, Assembly assembly)
        {
            foreach (var exportedType in assembly.ExportedTypes)
            {
                engine.AddConstructor(ctx, exportedType);
            }
        }

        public static void AddInstances(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.ExportedTypes)
            {
                engine.AddInstance(obj, exportedType);
            }
        }

        public static void AddConstructor(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            var ti = type.GetTypeInfo();
            var names = ti.GetCustomAttributes<DomNameAttribute>();
            var name = names.FirstOrDefault();

            if (name != null && !ti.IsEnum)
            {
                var info = ti.DeclaredConstructors.FirstOrDefault(m => m.GetCustomAttributes<DomConstructorAttribute>().Any());
                var constructor = info != null ? new DomConstructorInstance(engine, info) : new DomConstructorInstance(engine, type);
                obj.FastSetProperty(name.OfficialName, new PropertyDescriptor(constructor, false, true, false));
            }
        }

        public static void AddInstance(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            var attributes = type.GetTypeInfo().GetCustomAttributes<DomInstanceAttribute>();
            var info = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(m => m.GetParameters().Length == 0);

            if (info != null)
            {
                foreach (var attribute in attributes)
                {
                    var instance = info.Invoke(null);

                    if (instance != null)
                    {
                        var node = engine.GetDomNode(instance);
                        obj.FastSetProperty(attribute.Name, new PropertyDescriptor(node, false, true, false));
                    }
                }
            }
        }

        public static JsValue RunScript(this EngineInstance engine, String source) =>
            engine.RunScript(source, engine.Window);

        public static JsValue RunScript(this EngineInstance engine, String source, INode context) =>
            engine.RunScript(source, context.ToJsValue(engine));

        public static String GetOfficialName(this MemberInfo member)
        {
            var names = member.GetCustomAttributes<DomNameAttribute>();
            var officalNameAttribute = names.FirstOrDefault();
            return officalNameAttribute?.OfficialName ?? member.Name;
        }

        public static String GetOfficialName(this Type currentType, Type baseType)
        {
            var ti = currentType.GetTypeInfo();
            var name = ti.GetCustomAttribute<DomNameAttribute>(true)?.OfficialName;

            if (name == null)
            {
                var interfaces = ti.ImplementedInterfaces;

                if (baseType != null)
                {
                    var bi = baseType.GetTypeInfo();
                    var exclude = bi.ImplementedInterfaces;
                    interfaces = interfaces.Except(exclude);
                }

                foreach (var impl in interfaces)
                {
                    name = impl.GetTypeInfo().GetCustomAttribute<DomNameAttribute>(false)?.OfficialName;

                    if (name != null)
                        break;
                }
            }

            return name;
        }

        private static Object AsComplex(this JsValue value, Type targetType, EngineInstance engine)
        {
            var obj = value.FromJsValue();
            var sourceType = obj.GetType();

            if (sourceType == targetType || sourceType.GetTypeInfo().IsSubclassOf(targetType) || targetType.GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo()))
            {
                return obj;
            }
            else if (targetType.GetTypeInfo().IsSubclassOf(typeof(Delegate)))
            {
                var f = obj as FunctionInstance;

                if (f == null && obj is String b)
                {
                    var e = engine.Jint;
                    var p = new[] { new JsValue(b) };
                    f = new ClrFunctionInstance(e, (_this, args) => e.Eval.Call(_this, p));
                }

                if (f != null)
                {
                    return targetType.ToDelegate(f, engine);
                }
            }

            var method = sourceType.PrepareConvert(targetType) ??
                throw new JavaScriptException("[Internal] Could not find corresponding cast target.");

            return method.Invoke(obj, null);
        }
    }
}
