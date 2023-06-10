namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Js.Cache;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    static class EngineExtensions
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
                    string name = ((Enum)obj).GetOfficialName();
                    if (name != null)
                    {
                        return new JsValue(name);
                    }

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

        public static Object[] BuildArgs(this EngineInstance context, MethodBase method, JsValue[] arguments)
        {
            var parameters = method.GetParameters();
            var initDict = method.GetCustomAttribute<DomInitDictAttribute>();
            var max = parameters.Length;
            var args = new Object[max];
            var offset = 0;

            if (parameters.Length > 0 && parameters[0].ParameterType == typeof(IWindow))
            {
                if (arguments.Length == 0 || arguments[0].FromJsValue() is IWindow == false)
                {
                    args[offset++] = context.Window.Value;
                }
            }

            if (max > 0 && parameters[max - 1].GetCustomAttribute<ParamArrayAttribute>() != null)
            {
                max--;
            }

            if (initDict != null && arguments.Length + offset > initDict.Offset)
            {
                arguments = ExpandInitDict(arguments, parameters, initDict, max, offset);
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

        private static JsValue[] ExpandInitDict(JsValue[] arguments, ParameterInfo[] parameters, DomInitDictAttribute initDict, Int32 max, Int32 offset)
        {
            var newArgs = new JsValue[max - offset];
            var end = initDict.Offset - offset;
            var obj = arguments[end].AsObject();

            for (var i = 0; i < end; i++)
            {
                newArgs[i] = arguments[i];
            }

            if (obj != null)
            {
                for (var i = end + offset; i < max; i++)
                {
                    var p = parameters[i];
                    var name = p.Name;

                    if (obj.HasProperty(name))
                    {
                        newArgs[i - offset] = obj.GetProperty(name).Value;
                    }
                    else
                    {
                        newArgs[i - offset] = JsValue.Undefined;
                    }
                }
            }

            arguments = newArgs;
            return arguments;
        }

        public static void AddConstructors(this EngineInstance engine, ObjectInstance ctx, Assembly assembly)
        {
            foreach (var exportedType in assembly.ExportedTypes)
            {
                engine.AddConstructor(ctx, exportedType);
            }
        }

        public static void AddInstances(this EngineInstance engine, ObjectInstance obj, Assembly assembly)
        {
            foreach (var exportedType in assembly.ExportedTypes)
            {
                engine.AddInstance(obj, exportedType);
            }
        }

        public static void AddConstructor(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            var apply = type.GetConstructorAction();
            apply.Invoke(engine, obj);
        }

        public static void AddInstance(this EngineInstance engine, ObjectInstance obj, Type type)
        {
            var apply = type.GetInstanceAction();
            apply.Invoke(engine, obj);
        }

        public static JsValue RunScript(this EngineInstance engine, String source) =>
            engine.RunScript(source, engine.Window);

        public static JsValue RunScript(this EngineInstance engine, String source, INode context) =>
            engine.RunScript(source, context.ToJsValue(engine));

        public static JsValue Call(this EngineInstance instance, MethodInfo method, JsValue thisObject, JsValue[] arguments)
        {
            if (method != null && thisObject.Type == Types.Object && thisObject.AsObject() is DomNodeInstance node)
            {
                try
                {
                    if (method.IsStatic)
                    {
                        var newArgs = new List<JsValue>
                        {
                            thisObject,
                        };
                        newArgs.AddRange(arguments);
                        var parameters = instance.BuildArgs(method, newArgs.ToArray());
                        return method.Invoke(null, parameters).ToJsValue(instance);
                    }
                    else
                    {
                        var parameters = instance.BuildArgs(method, arguments);
                        return method.Invoke(node.Value, parameters).ToJsValue(instance);
                    }
                }
                catch (TargetInvocationException)
                {
                    throw new JavaScriptException(instance.Jint.Error);
                }
            }

            return JsValue.Undefined;
        }
    }
}
