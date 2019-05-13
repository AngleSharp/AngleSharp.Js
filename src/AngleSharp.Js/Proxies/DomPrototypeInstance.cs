namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Text;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime.Descriptors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class DomPrototypeInstance : ObjectInstance
    {
        private readonly Type _type;
        private readonly String _name;
        private readonly EngineInstance _instance;

        private PropertyInfo _numericIndexer;
        private PropertyInfo _stringIndexer;

        public DomPrototypeInstance(EngineInstance engine, Type type)
            : base(engine.Jint)
        {
            var baseType = type.GetTypeInfo().BaseType ?? typeof(Object);
            _type = type;
            _name = type.GetOfficialName(baseType);
            _instance = engine;

            SetAllMembers();
            SetExtensionMembers();

            //  DOM objects can have properties added dynamically
            Extensible = true;
            Prototype = engine.GetDomPrototype(baseType);
        }

        public override String Class => _name;

        public Boolean TryGetFromIndex(Object value, String index, out PropertyDescriptor result)
        {
            //  If we have a numeric indexer and the property is numeric
            result = default;

            if (_numericIndexer != null && Int32.TryParse(index, out var numericIndex))
            {
                var args = new Object[] { numericIndex };

                try
                {
                    var orig = _numericIndexer.GetMethod.Invoke(value, args);
                    var prop = orig.ToJsValue(_instance);
                    result = new PropertyDescriptor(prop, false, false, false);
                    return true;
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is ArgumentOutOfRangeException)
                    {
                        var prop = JsValue.Undefined;
                        result = new PropertyDescriptor(prop, false, false, false);
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
            if (_stringIndexer != null && !Properties.ContainsKey(index))
            {
                var args = new Object[] { index };
                var prop = _stringIndexer.GetMethod.Invoke(value, args).ToJsValue(_instance);
                result = new PropertyDescriptor(prop, false, false, false);
                return true;
            }

            return false;
        }

        private void SetExtensionMembers()
        {
            foreach (var type in _name.GetExtensionTypes())
            {
                var typeInfo = type.GetTypeInfo();
                SetExtensionMethods(typeInfo.DeclaredMethods);
            }
        }

        private void SetAllMembers()
        {
            foreach (var type in _type.GetTypeTree())
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
                var names = eventInfo.GetCustomAttributes<DomNameAttribute>()
                    .Select(m => m.OfficialName);

                foreach (var name in names)
                {
                    SetEvent(name, eventInfo.AddMethod, eventInfo.RemoveMethod);
                }
            }
        }

        private void SetEvent(String name, MethodInfo adder, MethodInfo remover)
        {
            var eventInstance = new DomEventInstance(_instance, adder, remover);
            FastSetProperty(name, new PropertyDescriptor(eventInstance.Getter, eventInstance.Setter, false, false));
        }

        private void SetExtensionMethods(IEnumerable<MethodInfo> methods)
        {
            var entries = new Dictionary<String, ExtensionEntry>();

            foreach (var method in methods)
            {
                var names = method.GetCustomAttributes<DomNameAttribute>()
                    .Select(m => m.OfficialName);
                var accessors = method.GetCustomAttributes<DomAccessorAttribute>()
                    .Select(m => m.Type);
                var isEvent = method.GetCustomAttribute<DomEventAttribute>() != null;
                var forward = method.GetCustomAttribute<DomPutForwardsAttribute>();

                foreach (var name in names)
                {
                    if (!entries.TryGetValue(name, out var entry))
                    {
                        entry = new ExtensionEntry
                        {
                            Forward = forward,
                        };
                        entries.Add(name, entry);
                    }

                    if (accessors.Any())
                    {
                        var accessor = accessors.FirstOrDefault();

                        if (isEvent)
                        {
                            switch (accessor)
                            {
                                case Accessors.Deleter:
                                    entry.Remover = method;
                                    break;
                                case Accessors.Setter:
                                    entry.Adder = method;
                                    break;
                            }
                        }
                        else
                        {
                            switch (accessor)
                            {
                                case Accessors.Setter:
                                    entry.Setter = method;
                                    break;
                                case Accessors.Getter:
                                    entry.Getter = method;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        entry.Other = method;
                    }
                }
            }

            foreach (var entry in entries)
            {
                var name = entry.Key;
                var value = entry.Value;

                if (!Properties.ContainsKey(name))
                {
                    if (value.Adder != null && value.Remover != null)
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
        }

        class ExtensionEntry
        {
            public MethodInfo Getter;
            public MethodInfo Setter;
            public MethodInfo Adder;
            public MethodInfo Remover;
            public MethodInfo Other;
            public DomPutForwardsAttribute Forward;
        }

        private void SetNormalProperties(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var indexParameters = property.GetIndexParameters();
                var index = property.GetCustomAttribute<DomAccessorAttribute>();
                var putsForward = property.GetCustomAttribute<DomPutForwardsAttribute>();
                var names = property
                    .GetCustomAttributes<DomNameAttribute>()
                    .Select(m => m.OfficialName);

                if (index != null || names.Any(m => m.Is("item")))
                {
                    SetIndexer(property, indexParameters);
                }

                foreach (var name in names)
                {
                    SetProperty(name, property.GetMethod, property.SetMethod, putsForward);
                }
            }
        }

        private void SetProperty(String name, MethodInfo getter, MethodInfo setter, DomPutForwardsAttribute putsForward)
        {
            FastSetProperty(name, new PropertyDescriptor(
                new DomDelegateInstance(_instance, name, (obj, values) =>
                    _instance.Call(getter, obj, values)),
                new DomDelegateInstance(_instance, name, (obj, values) =>
                {
                    if (putsForward != null)
                    {
                        var ep = Array.Empty<Object>();
                        var that = obj.AsObject() as DomNodeInstance;
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
            if (indexParameters.Length == 1)
            {
                if (indexParameters[0].ParameterType == typeof(Int32))
                {
                    _numericIndexer = property;
                }
                else if (indexParameters[0].ParameterType == typeof(String))
                {
                    _stringIndexer = property;
                }
            }
        }

        private void SetNormalMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                var names = method.GetCustomAttributes<DomNameAttribute>()
                    .Select(m => m.OfficialName);

                foreach (var name in names)
                {
                    SetMethod(name, method);
                }
            }
        }

        private void SetMethod(String name, MethodInfo method)
        {
            //TODO Jint
            // If it already has a property with the given name (usually another method),
            // then convert that method to a two-layer method, which decides which one
            // to pick depending on the number (and probably types) of arguments.
            if (!Properties.ContainsKey(name))
            {
                var func = new DomFunctionInstance(_instance, method);
                FastAddProperty(name, func, false, false, false);
            }
        }
    }
}

