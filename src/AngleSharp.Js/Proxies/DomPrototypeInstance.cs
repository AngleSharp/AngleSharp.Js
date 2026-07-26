namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using AngleSharp.Text;
    using Jint.Native.Object;
    using Jint.Native.Symbol;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class DomPrototypeInstance : ObjectInstance
    {
        private readonly String _name;
        private readonly EngineInstance _instance;
        private readonly Type _type;

        private MethodInfo _numericIndexer;
        private MethodInfo _stringIndexer;

        public DomPrototypeInstance(EngineInstance engine, Type type)
            : base(engine.Jint)
        {
            var baseType = type.GetTypeInfo().BaseType ?? typeof(Object);
            _name = type.GetOfficialName(baseType);
            _instance = engine;
            _type = type;

            Set(GlobalSymbolRegistry.ToStringTag, _name);

            SetAllMembers(type);
            SetExtensionMembers();

            //  DOM objects can have properties added dynamically
            Prototype = engine.GetDomPrototype(baseType);
        }

        public Boolean TryGetFromIndex(Object value, String index, out PropertyDescriptor result)
        {
            //  If we have a numeric indexer and the property is numeric
            result = default;

            if (_numericIndexer != null && Int32.TryParse(index, out var numericIndex))
            {
                try
                {
                    var args = new Object[] { numericIndex };
                    var orig = _numericIndexer.Invoke(value, args);
                    result = new PropertyDescriptor(orig.ToJsValue(_instance), false, false, false);
                    return true;
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is ArgumentOutOfRangeException)
                    {
                        result = PropertyDescriptor.Undefined;
                        return true;
                    }

                    throw;
                }
            }

            //  Else a string property
            //  If we have a string indexer and no property exists for this name then use the string indexer
            //  Jint possibly has a limitation here - if an object has a string indexer.  How do we know whether to use the defined indexer or a property?
            //  Eg. object.callMethod1()  vs  object['callMethod1'] is not necessarily the same if the object has a string indexer?? (I'm not an ECMA expert!)
            //  node.attributes is one such object - has both a string and numeric indexer
            //  This GetOwnProperty override might need an additional parameter to let us know this was called via an indexer
            if (_stringIndexer != null && !HasProperty(index))
            {
                var args = new Object[] { index };
                var valueAtIndex = _stringIndexer.Invoke(value, args);

                if (valueAtIndex == null)
                {
                    result = PropertyDescriptor.Undefined;
                    return false;
                }

                var prop = valueAtIndex.ToJsValue(_instance);
                result = new PropertyDescriptor(prop, false, false, false);
                return true;
            }

            return false;
        }

        private void SetExtensionMembers()
        {
            foreach (var type in _instance.Libs.GetExtensionTypes(_name))
            {
                var typeInfo = type.GetTypeInfo();
                SetExtensionMethods(typeInfo.DeclaredMethods);
            }
        }

        private void SetAllMembers(Type parentType)
        {
            foreach (var type in parentType.GetTypeTree())
            {
                var typeInfo = type.GetTypeInfo();
                SetNormalProperties(typeInfo.DeclaredProperties);
                SetNormalMethods(typeInfo.DeclaredMethods);
                SetNormalEvents(typeInfo.DeclaredEvents);
            }
        }

        private void SetNormalEvents(IEnumerable<EventInfo> eventInfos)
        {
            foreach (var eventInfo in eventInfos)
            {
                foreach (var m in eventInfo.GetCustomAttributes<DomNameAttribute>())
                {
                    SetEvent(m.OfficialName, eventInfo.AddMethod, eventInfo.RemoveMethod);
                }
            }
        }

        private void SetExtensionMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var entry in methods.GetExtensions())
            {
                var name = entry.Key;
                var value = entry.Value;

                if (HasProperty(name))
                {
                    // skip
                }
                else if (value.Adder != null && value.Remover != null)
                {
                    SetEvent(name, value.Adder, value.Remover);
                }
                else if (value.Getter != null || value.Setter != null)
                {
                    SetProperty(name, value.Getter, value.Setter, value.Forward);
                }
                else if (value.Other != null)
                {
                    SetMethod(name, value.Other);
                }
            }
        }

        private void SetNormalProperties(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var indexParameters = property.GetIndexParameters();
                var accessor = property.GetCustomAttribute<DomAccessorAttribute>()?.Type;
                var putsForward = property.GetCustomAttribute<DomPutForwardsAttribute>();
                var names = property
                    .GetCustomAttributes<DomNameAttribute>()
                    .Select(m => m.OfficialName)
                    .ToArray();

                if (accessor == Accessors.Method)
                {
                    // property decorated with Method accessor, so we need to treat it as a method, not a property

                    if (property.GetMethod == null)
                    {
                        throw new InvalidOperationException("Getter not found.");
                    }

                    foreach (var name in names)
                    {
                        SetMethod(name, property.GetMethod);
                    }

                    // methods were set, so finish processing
                    return;
                }

                if (accessor == Accessors.Getter || accessor == Accessors.Setter || Array.Exists(names, m => m.Is("item")))
                {
                    SetIndexer(property, indexParameters);
                }

                foreach (var name in names)
                {
                    SetProperty(name, property.GetMethod, property.SetMethod, putsForward);
                }
            }
        }

        private void SetNormalMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                foreach (var m in method.GetCustomAttributes<DomNameAttribute>())
                {
                    SetMethod(m.OfficialName, method);
                }
            }
        }

        private void SetEvent(String name, MethodInfo adder, MethodInfo remover)
        {
            var eventInstance = new DomEventInstance(_instance, adder, remover);
            FastSetProperty(name, new GetSetPropertyDescriptor(eventInstance.Getter, eventInstance.Setter, false, false));
        }

        private void SetProperty(String name, MethodInfo getter, MethodInfo setter, DomPutForwardsAttribute putsForward)
        {
            FastSetProperty(name, new GetSetPropertyDescriptor(
                new ClrFunction(_instance.Jint, name, (obj, values) =>
                    _instance.Call(getter, obj, values)),
                new ClrFunction(_instance.Jint, name, (obj, values) =>
                {
                    if (putsForward != null)
                    {
                        var ep = Array.Empty<Object>();
                        var that = obj as DomNodeInstance;
                        var target = getter.Invoke(that.Value, ep);
                        var propName = putsForward.PropertyName;
                        var prop = getter.ReturnType
                            .GetInheritedProperties()
                            .FirstOrDefault(m => m.GetCustomAttributes<DomNameAttribute>().Any(n => n.OfficialName.Is(propName)));
                        var args = _instance.BuildArgs(prop.SetMethod, values);
                        prop.SetMethod.Invoke(target, args);
                        return prop.GetMethod.Invoke(target, ep).ToJsValue(_instance);
                    }

                    return _instance.Call(setter, obj, values);
                }), false, false));
        }

        private void SetIndexer(PropertyInfo property, ParameterInfo[] indexParameters)
        {
            if (indexParameters.Length != 1)
            {
                return;
            }

            var getter = ResolveAccessor(property.GetMethod);

            if (getter == null)
            {
                return;
            }

            if (indexParameters[0].ParameterType == typeof(Int32))
            {
                _numericIndexer = getter;
            }
            else if (indexParameters[0].ParameterType == typeof(String))
            {
                _stringIndexer = getter;
            }
        }

        private MethodInfo ResolveAccessor(MethodInfo accessor)
        {
            //  An interface may re-implement a member of one of its own base interfaces
            //  explicitly, e.g. "T IReadOnlyList<T>.this[Int32 index]" declared on an
            //  IHtmlCollection<T>. Such a member is private and abstract - invoking it
            //  reflectively throws an EntryPointNotFoundException because the actual
            //  implementation lives in a different slot. Resolve it against the type the
            //  prototype was created for, which is where the implementation can be found.
            if (accessor == null || accessor.IsPublic)
            {
                return accessor;
            }

            var name = accessor.Name;
            var simpleName = name.Substring(name.LastIndexOf('.') + 1);
            var parameters = accessor.GetParameters();
            var parameterTypes = new Type[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = parameters[i].ParameterType;
            }

            return _type.GetRuntimeMethod(simpleName, parameterTypes) ?? accessor;
        }

        private void SetMethod(String name, MethodInfo method)
        {
            //TODO Jint
            // If it already has a property with the given name (usually another method),
            // then convert that method to a two-layer method, which decides which one
            // to pick depending on the number (and probably types) of arguments.
            if (!HasProperty(name))
            {
                FastSetProperty(name, new PropertyDescriptor(
                    new ClrFunction(_instance.Jint, name, (obj, values) =>
                        _instance.Call(method, obj, values)
                    ), false, false, false));
            }
        }
    }
}

