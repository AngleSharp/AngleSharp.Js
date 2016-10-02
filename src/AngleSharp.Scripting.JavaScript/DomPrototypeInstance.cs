namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
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
        private readonly EngineInstance _engine;

        private PropertyInfo _numericIndexer;
        private PropertyInfo _stringIndexer;

        public DomPrototypeInstance(EngineInstance engine, Type type)
            : base(engine.Jint)
        {
            _type = type;
            _name = String.Format("[object {0}]", type.Name);
            _engine = engine;

            SetAllMembers();
            SetPseudoProperties();

            //  DOM objects can have properties added dynamically
            Extensible = true;
            Prototype = engine.GetDomPrototype(type.GetTypeInfo().BaseType);
        }
        
        public override String ToString()
        {
            return _name;
        }

        public Boolean TryGetFromIndex(Object value, String index, out PropertyDescriptor result)
        {
            //  If we have a numeric indexer and the property is numeric
            var numericIndex = default(Int32);
            result = default(PropertyDescriptor);

            if (_numericIndexer != null && Int32.TryParse(index, out numericIndex))
            {
                var args = new Object[] { numericIndex };

                try
                {
                    var orig = _numericIndexer.GetMethod.Invoke(value, args);
                    var prop = orig.ToJsValue(_engine);
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
                var prop = _stringIndexer.GetMethod.Invoke(value, args).ToJsValue(_engine);
                result = new PropertyDescriptor(prop, false, false, false);
                return true;
            }

            return false;
        }

        private void SetAllMembers()
        {
            var type = _type;
            var types = new List<Type>(type.GetTypeInfo().ImplementedInterfaces);

            do
            {
                types.Add(type);
                type = type.GetTypeInfo().BaseType;
            }
            while (type != null);

            SetMembers(types);
        }

        private void SetMembers(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var typeInfo = type.GetTypeInfo();
                SetProperties(typeInfo.DeclaredProperties);
                SetMethods(typeInfo.DeclaredMethods);
                SetEvents(typeInfo.DeclaredEvents);
            }
        }

        private void SetEvents(IEnumerable<EventInfo> eventInfos)
        {
            foreach (var eventInfo in eventInfos)
            {
                var names = eventInfo.GetCustomAttributes<DomNameAttribute>();

                foreach (var name in names.Select(m => m.OfficialName))
                {
                    var eventInstance = new DomEventInstance(_engine, eventInfo);
                    FastSetProperty(name, new PropertyDescriptor(eventInstance.Getter, eventInstance.Setter, false, false));
                }
            }
        }

        private void SetProperties(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var index = property.GetCustomAttribute<DomAccessorAttribute>();

                if (index != null)
                {
                    var indexParameters = property.GetIndexParameters();

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

                var names = property.GetCustomAttributes<DomNameAttribute>();

                foreach (var name in names.Select(m => m.OfficialName))
                {
                    FastSetProperty(name, new PropertyDescriptor(
                        new DomFunctionInstance(_engine, property.GetMethod),
                        new DomFunctionInstance(_engine, property.SetMethod), false, false));
                }
            }
        }

        private void SetMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                var names = method.GetCustomAttributes<DomNameAttribute>();

                foreach (var name in names.Select(m => m.OfficialName))
                {
                    //TODO Jint
                    // If it already has a property with the given name (usually another method),
                    // then convert that method to a two-layer method, which decides which one
                    // to pick depending on the number (and probably types) of arguments.
                    if (!Properties.ContainsKey(name))
                    {
                        var func = new DomFunctionInstance(_engine, method);
                        FastAddProperty(name, func, false, false, false);
                    }
                }
            }
        }

        private void SetPseudoProperties()
        {
            if (_type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IElement)))
            {
                var focusInEventInstance = new DomEventInstance(_engine);
                var focusOutEventInstance = new DomEventInstance(_engine);
                var unloadEventInstance = new DomEventInstance(_engine);
                var contextMenuEventInstance = new DomEventInstance(_engine);

                FastSetProperty("scrollLeft", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("scrollTop", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("scrollWidth", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("scrollHeight", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("clientLeft", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("clientTop", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("clientWidth", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("clientHeight", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("offsetLeft", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("offsetTop", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("offsetWidth", new PropertyDescriptor(new JsValue(0.0), false, false, false));
                FastSetProperty("offsetHeight", new PropertyDescriptor(new JsValue(0.0), false, false, false));

                FastSetProperty("focusin", new PropertyDescriptor(focusInEventInstance.Getter, focusInEventInstance.Setter, false, false));
                FastSetProperty("focusout", new PropertyDescriptor(focusOutEventInstance.Getter, focusOutEventInstance.Setter, false, false));
                FastSetProperty("unload", new PropertyDescriptor(unloadEventInstance.Getter, unloadEventInstance.Setter, false, false));
                FastSetProperty("contextmenu", new PropertyDescriptor(contextMenuEventInstance.Getter, contextMenuEventInstance.Setter, false, false));
            }
        }
    }
}
