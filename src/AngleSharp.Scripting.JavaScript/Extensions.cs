namespace AngleSharp.Scripting.JavaScript
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
                    return node != null ? node.Value : obj;
                case Types.Undefined:
                    return Undefined.Text;
                case Types.Null:
                    return null;
            }

            return val.ToObject();
        }

        public static Object As(this Object value, Type targetType, EngineInstance engine)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                if (sourceType == targetType || sourceType.GetTypeInfo().IsSubclassOf(targetType) || targetType.GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo()))
                {
                    return value;
                }
                else if (targetType == typeof(Int32))
                {
                    if (sourceType != typeof(Double))
                    {
                        var v = value.ToJsValue(engine);
                        return TypeConverter.ToInt32(v);
                    }

                    return (Int32)(Double)value;
                }
                else if (targetType == typeof(Double))
                {
                    var v = value.ToJsValue(engine);
                    return TypeConverter.ToNumber(v);
                }
                else if (targetType == typeof(String))
                {
                    var v = value.ToJsValue(engine);

                    if (v.IsPrimitive())
                    {
                        return TypeConverter.ToString(v);
                    }

                    return v.ToString();
                }
                else if (targetType == typeof(Boolean))
                {
                    var v = value.ToJsValue(engine);
                    return TypeConverter.ToBoolean(v);
                }
                else if (targetType.GetTypeInfo().IsSubclassOf(typeof(Delegate)))
                {
                    var f = value as FunctionInstance;

                    if (f == null)
                    {
                        var b = value as String;

                        if (b != null)
                        {
                            var e = engine.Jint;
                            var p = new[] { new JsValue(b) };
                            f = new ClrFunctionInstance(e, (_this, args) => e.Eval.Call(_this, p));
                        }
                    }

                    if (f != null)
                    {
                        return targetType.ToDelegate(f, engine);
                    }
                }

                var method = sourceType.PrepareConvert(targetType);

                if (method == null)
                    throw new JavaScriptException("[Internal] Could not find corresponding cast target.");

                return method.Invoke(value, null);
            }

            return value;
        }

        public static Object GetDefaultValue(this Type type)
        {
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
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
                    args[i + offset] = arguments[i].FromJsValue().As(parameters[i].ParameterType, context);
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

        public static String[] GetParameterNames(this MethodInfo method)
        {
            return method != null ? method.GetParameters().Select(m => m.Name).ToArray() : null;
        }

        public static void AddConstructors(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            foreach (var exportedType in type.GetTypeInfo().Assembly.ExportedTypes)
            {
                engine.AddConstructor(obj, exportedType);
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
            var info = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(m => 
                m.GetCustomAttributes<DomConstructorAttribute>().Any());

            if (info != null)
            {
                var name = type.GetTypeInfo().GetOfficialName();
                var constructor = new DomConstructorInstance(engine, info);
                obj.FastSetProperty(name, new PropertyDescriptor(constructor, false, true, false));
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

        public static JsValue RunScript(this EngineInstance engine, String source)
        {
            return engine.RunScript(source, engine.Window);
        }

        public static JsValue RunScript(this EngineInstance engine, String source, INode context)
        {
            return engine.RunScript(source, context.ToJsValue(engine));
        }

        public static String GetOfficialName(this MemberInfo member)
        {
            var names = member.GetCustomAttributes<DomNameAttribute>();
            var officalNameAttribute = names.FirstOrDefault();
            return officalNameAttribute != null ? officalNameAttribute.OfficialName : member.Name;
        }
    }
}
