using AngleSharp.Attributes;
using AngleSharp.Js.Attributes;
using AngleSharp.Js.Proxies;
using Jint.Native.Object;
using Jint.Runtime.Descriptors;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace AngleSharp.Js.Cache
{
    static class CreatorCache
    {
        private static readonly ConcurrentDictionary<Type, Action<EngineInstance, ObjectInstance>> _constructorActions = new();

        public static Action<EngineInstance, ObjectInstance> GetConstructorAction(this Type type)
        {
            if (!_constructorActions.TryGetValue(type, out var action))
            {
                var ti = type.GetTypeInfo();
                var names = ti.GetCustomAttributes<DomNameAttribute>();
                var name = names.FirstOrDefault();

                if (name != null && !ti.IsEnum)
                {
                    var info = ti.DeclaredConstructors.FirstOrDefault(m => m.GetCustomAttributes<DomConstructorAttribute>().Any());
                    action = (engine, obj) =>
                    {
                        var constructor = info != null ? new DomConstructorInstance(engine, info) : new DomConstructorInstance(engine, type);
                        obj.FastSetProperty(name.OfficialName, new PropertyDescriptor(constructor, false, true, false));
                    };
                }
                else
                {
                    action = (e, o) => { };
                }

                _constructorActions.TryAdd(type, action);
            }

            return action;
        }

        private static readonly ConcurrentDictionary<Type, Action<EngineInstance, ObjectInstance>> _constructorFunctionActions = new();

        public static Action<EngineInstance, ObjectInstance> GetConstructorFunctionAction(this Type type)
        {
            if (!_constructorFunctionActions.TryGetValue(type, out var action))
            {
                var constructorFunctions = type.GetTypeInfo().GetMethods().Where(m => m.GetCustomAttributes<DomConstructorFunctionAttribute>().Any());

                if (constructorFunctions.Any())
                {
                    action = (engine, obj) =>
                    {
                        foreach (var constructorFunction in constructorFunctions)
                        {
                            var attribute = constructorFunction.GetCustomAttribute<DomConstructorFunctionAttribute>();

                            var constructorFunctionInstance = new DomConstructorFunctionInstance(engine, constructorFunction, attribute.OfficialName);

                            obj.FastSetProperty(attribute.OfficialName, new PropertyDescriptor(constructorFunctionInstance, false, true, false));
                        }
                    };
                }
                else
                {
                    action = (e, o) => { };
                }

                _constructorFunctionActions.TryAdd(type, action);
            }

            return action;
        }

        private static readonly ConcurrentDictionary<Type, Action<EngineInstance, ObjectInstance>> _instanceActions = new();

        public static Action<EngineInstance, ObjectInstance> GetInstanceAction(this Type type)
        {
            if (!_instanceActions.TryGetValue(type, out var action))
            {
                var info = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(m => m.GetParameters().Length == 0);

                if (info != null)
                {
                    var attributes = type.GetTypeInfo().GetCustomAttributes<DomInstanceAttribute>();
                    action = (engine, obj) =>
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
                    };
                }
                else
                {
                    action = (e, o) => { };
                }

                _instanceActions.TryAdd(type, action);
            }

            return action;
        }
    }
}
