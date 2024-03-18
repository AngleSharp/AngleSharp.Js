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
                    return JsValue.FromObjectWithType(engine.Jint, obj, typeof(String));
                }
                else if (obj is Int32)
                {
                    return JsValue.FromObjectWithType(engine.Jint, obj, typeof(Int32));
                }
                else if (obj is UInt32)
                {
                    return JsValue.FromObjectWithType(engine.Jint, obj, typeof(UInt32));
                }
                else if (obj is Double)
                {
                    return JsValue.FromObjectWithType(engine.Jint, obj, typeof(Double));
                }
                else if (obj is Single)
                {
                    return JsValue.FromObjectWithType(engine.Jint, obj, typeof(Single));
                }
                else if (obj is Boolean)
                {
                    return JsValue.FromObjectWithType(engine.Jint, obj, typeof(Boolean));
                }
                else if (obj is Enum)
                {
                    switch (obj)
                    {
                        case DocumentReadyState _:
                            var name = ((Enum)obj).GetOfficialName();
                            if (name != null)
                            {
                                return JsValue.FromObjectWithType(engine.Jint, name, typeof(String));
                            }
                            break;
                    }
                    return JsValue.FromObjectWithType(engine.Jint, obj, typeof(Enum));
                }

                return engine.GetDomNode(obj);
            }

            return JsValue.Null;
        }

        public static ClrFunction AsValue(this Engine engine, string name, Func<JsValue, JsValue[], JsValue> func) =>
            new ClrFunction(engine, name, func);

        public static PropertyDescriptor AsProperty(this Engine engine, JsValue getter = null, JsValue setter = null) =>
            new GetSetPropertyDescriptor(getter, setter, true, getter != null && setter != null);

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

        public static JsValue RunScript(this EngineInstance engine, String source, String type, String sourceUrl) =>
            engine.RunScript(source, type, sourceUrl, engine.Window);

        public static JsValue RunScript(this EngineInstance engine, String source, String type, String sourceUrl, INode context) =>
            engine.RunScript(source, type, sourceUrl, context.ToJsValue(engine));

        public static JsValue Call(this EngineInstance instance, MethodInfo method, JsValue thisObject, JsValue[] arguments)
        {
            if (method != null)
            {
                DomNodeInstance nodeInstance;

                if (thisObject.Type == Types.Object && thisObject.AsObject() is DomNodeInstance node)
                {
                    nodeInstance = node;
                }
                else
                {
                    nodeInstance = instance.Window;
                }

                try
                {
                    if (method.IsStatic)
                    {
                        var newArgs = new List<JsValue>
                        {
                            nodeInstance,
                        };
                        newArgs.AddRange(arguments);
                        var parameters = instance.BuildArgs(method, newArgs.ToArray());
                        return method.Invoke(null, parameters).ToJsValue(instance);
                    }
                    else
                    {
                        var parameters = instance.BuildArgs(method, arguments);
                        return method.Invoke(nodeInstance.Value, parameters).ToJsValue(instance);
                    }
                }
                catch (TargetInvocationException)
                {
                    throw new JavaScriptException(instance.Jint.Intrinsics.Error);
                }
            }

            return JsValue.Undefined;
        }
    }
}
