namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using AngleSharp.Js.Cache;
    using AngleSharp.Text;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Native.Symbol;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class DomPrototypeInstance : ObjectInstance
    {
        private readonly String _name;
        private readonly EngineInstance _instance;
        private readonly Type _type;
        private readonly Type _baseType;

        private List<KeyValuePair<String, PropertyDescriptor>> _deferred;
        private Boolean _membersSet;
        private DomConstructorInstance _constructor;
        private Indexer _numericIndexer;
        private Indexer _stringIndexerGetter;
        private Indexer _stringIndexerSetter;

        public DomPrototypeInstance(EngineInstance engine, Type type)
            : base(engine.Jint)
        {
            _baseType = type.GetTypeInfo().BaseType ?? typeof(Object);
            _name = type.GetOfficialName(_baseType);
            _instance = engine;
            _type = type;
        }

        //  A document uses a handful of the DOM types an assembly exposes, but a prototype
        //  is created for every one of them. Reflecting over the whole type tree is by far
        //  the most expensive part of that, so it waits until the prototype is looked at.
        //  Jint calls this before serving any property, and marks the instance initialized
        //  beforehand, so the registration below can use the object as usual.
        protected override void Initialize()
        {
            _membersSet = true;

            Set(GlobalSymbolRegistry.ToStringTag, _name);

            SetAllMembers(_type);
            SetExtensionMembers();

            //  The base type may fold onto this very prototype - a class carrying no DOM name of
            //  its own shares the one of its nearest named ancestor. Jint's setter answers a
            //  prototype cycle by quietly doing nothing, which would leave Object.prototype in
            //  place and silently cut the chain short, so rule it out here.
            var parent = _instance.GetDomPrototype(_baseType);

            if (!ReferenceEquals(parent, this))
            {
                //  DOM objects can have properties added dynamically
                Prototype = parent;
            }

            if (_deferred != null)
            {
                foreach (var property in _deferred)
                {
                    FastSetProperty(property.Key, property.Value);
                }

                _deferred = null;
            }

            //  It is the constructor object that registers "constructor" here, and it is
            //  only built once script names the type. A prototype reached through an
            //  instance instead - the usual way - would otherwise lack the property.
            var definition = _type.GetConstructorDefinition(_instance.Libs);

            if (definition != null)
            {
                GetConstructor(definition);
            }
        }

        /// <summary>
        /// Gets the constructor object of the type this prototype belongs to, building it
        /// on first ask. Holding it here is what keeps the one script reads off the window
        /// and the one an instance reports as its "constructor" the same object.
        /// </summary>
        public DomConstructorInstance GetConstructor(ConstructorDefinition definition) =>
            _constructor ?? (_constructor = new DomConstructorInstance(_instance, definition));

        //  The prototype link is only established once the members are known, so reading it
        //  has to initialize as well - not every reader goes through a property lookup.
        protected override ObjectInstance GetPrototypeOf()
        {
            EnsureInitialized();
            return base.GetPrototypeOf();
        }

        /// <summary>
        /// Defines a property on the prototype without forcing its members to be
        /// registered. The property is applied after the members, so it keeps
        /// overriding a member of the same name.
        /// </summary>
        public void DefineDeferredProperty(String name, PropertyDescriptor descriptor)
        {
            if (_membersSet)
            {
                FastSetProperty(name, descriptor);
            }
            else
            {
                _deferred = _deferred ?? new List<KeyValuePair<String, PropertyDescriptor>>();
                _deferred.Add(new KeyValuePair<String, PropertyDescriptor>(name, descriptor));
            }
        }

        /// <summary>
        /// Asks the indexers this prototype offers about one property name, against the object
        /// being indexed rather than against the prototype itself.
        /// </summary>
        /// <remarks>
        /// The raw CLR value comes back instead of a <see cref="PropertyDescriptor"/> because
        /// only one of the three callers wants a descriptor: the engine asks a host object for
        /// the value of an own property and for its mere existence separately, and building a
        /// descriptor for either is pure waste - and for a named entry it builds two
        /// <see cref="ClrFunction"/>s with it.
        /// </remarks>
        public IndexerResult TryGetFromIndex(Object value, JsValue property, out Object result)
        {
            result = null;

            EnsureInitialized();

            //  Reached for every property lookup on every node, and most DOM types carry no
            //  indexer at all. Nothing below may run before that is ruled out - not even
            //  turning the key into a String.
            if (_numericIndexer == null && _stringIndexerGetter == null)
            {
                return IndexerResult.None;
            }

            //  A symbol is never an index, and JsSymbol.ToString() composes "Symbol(...)"
            //  every time it is asked. Library code probes Symbol.toStringTag constantly.
            if (property is JsSymbol)
            {
                return IndexerResult.None;
            }

            var index = property.ToString();

            //  If we have a numeric indexer and the property is numeric
            if (_numericIndexer != null && Int32.TryParse(index, out var numericIndex))
            {
                try
                {
                    var args = new Object[] { numericIndex };
                    result = _numericIndexer.Invoke(value, args);
                    return IndexerResult.Value;
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is ArgumentOutOfRangeException)
                    {
                        return IndexerResult.Absent;
                    }

                    throw;
                }
            }

            //  Else a string property
            return TryGetFromNamedIndex(value, property, out result) ? IndexerResult.Named : IndexerResult.None;
        }

        /// <summary>
        /// The string-indexer half on its own, for a collection whose numeric half the engine
        /// owns and must not be asked for twice.
        /// </summary>
        public Boolean TryGetFromNamedIndex(Object value, JsValue property, out Object result)
        {
            result = null;

            EnsureInitialized();

            if (_stringIndexerGetter == null || property is JsSymbol)
            {
                return false;
            }

            var index = property.ToString();

            //  If we have a string indexer and no property exists for this name then use the string indexer
            //  Jint possibly has a limitation here - if an object has a string indexer.  How do we know whether to use the defined indexer or a property?
            //  Eg. object.callMethod1()  vs  object['callMethod1'] is not necessarily the same if the object has a string indexer?? (I'm not an ECMA expert!)
            //  node.attributes is one such object - has both a string and numeric indexer
            //  This GetOwnProperty override might need an additional parameter to let us know this was called via an indexer
            //
            //  HasProperty takes a JsValue, so handing it the String would build one per
            //  lookup - and every read of an ordinary member of an indexed collection, say
            //  the length a loop tests, comes through here. The engine already passed one in.
            if (HasProperty(property as JsString ?? (JsValue)index))
            {
                return false;
            }

            var valueAtIndex = _stringIndexerGetter.Invoke(value, new Object[] { index });

            if (valueAtIndex == null && _stringIndexerSetter == null)
            {
                return false;
            }

            //  Null with a setter present is still an own property - one that reads as
            //  undefined - because a write to it still has to reach the setter.
            result = valueAtIndex;
            return true;
        }

        /// <summary>
        /// Builds the descriptor for a name the string indexer claims. It is an accessor pair
        /// rather than a value so that both directions stay live: the getter re-reads the
        /// indexer, and the setter is what forwards a write to it.
        /// </summary>
        public PropertyDescriptor CreateNamedDescriptor(Object value, String index)
        {
            var args = new Object[] { index };

            //  With nothing to forward a write to there is nothing an accessor pair would add:
            //  the value is read at the same moment either way, and the pair costs two function
            //  objects on a path that runs per read.
            if (_stringIndexerSetter == null)
            {
                var current = _stringIndexerGetter.Invoke(value, args);
                return new PropertyDescriptor(
                    current == null ? JsValue.Undefined : current.ToJsValue(_instance), false, false, false);
            }

            var getter = new ClrFunction(_instance.Jint, index, (obj, values) =>
            {
                var current = _stringIndexerGetter.Invoke(value, args);
                return current == null ? JsValue.Undefined : current.ToJsValue(_instance);
            });

            ClrFunction setter = null;

            if (_stringIndexerSetter != null)
            {
                setter = new ClrFunction(_instance.Jint, index, (obj, values) =>
                {
                    var valueToSet = values.Length > 0 ? values[0] : JsValue.Undefined;
                    _stringIndexerSetter.Invoke(value, new Object[] { index, valueToSet }, _instance);
                    return valueToSet;
                });
            }

            return new GetSetPropertyDescriptor(getter, setter, false, false);
        }

        public Boolean TrySetToIndex(Object value, JsValue property, JsValue newValue)
        {
            EnsureInitialized();

            if (_stringIndexerSetter == null)
            {
                return false;
            }

            var index = property.ToString();

            if (HasProperty(property as JsString ?? (JsValue)index))
            {
                return false;
            }

            _stringIndexerSetter.Invoke(value, new Object[] { index, newValue }, _instance);
            return true;
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

                    // methods were set, so continue with the next property
                    continue;
                }

                var isIndexedAccessor = accessor.HasValue && (accessor.Value & (Accessors.Getter | Accessors.Setter)) != 0;

                if (isIndexedAccessor || Array.Exists(names, m => m.Is("item")))
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
                        var that = obj as IDomProxy ?? _instance.Window;
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

            var getter = property.GetMethod;
            var setter = property.SetMethod;

            if (indexParameters[0].ParameterType == typeof(Int32))
            {
                if (getter != null)
                {
                    _numericIndexer = new Indexer(getter);
                }
            }
            else if (indexParameters[0].ParameterType == typeof(String))
            {
                if (getter != null)
                {
                    _stringIndexerGetter = new Indexer(getter);
                }

                if (setter != null)
                {
                    _stringIndexerSetter = new Indexer(setter);
                }
            }
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

        /// <summary>
        /// What the indexers of a prototype have to say about a property name.
        /// </summary>
        internal enum IndexerResult
        {
            /// <summary>
            /// No indexer claims the name, so the ordinary own-property lookup decides.
            /// </summary>
            None,
            /// <summary>
            /// The numeric indexer answered, and the answer is the value handed back.
            /// </summary>
            Value,
            /// <summary>
            /// The numeric indexer claims the name but has nothing at it, so the object has no
            /// own property of that name and the ordinary lookup is not consulted either.
            /// </summary>
            Absent,
            /// <summary>
            /// The string indexer claims the name. The value handed back is what it reads as
            /// right now, and may be null - a name the indexer only accepts writes for is
            /// still an own property, described by an accessor pair.
            /// </summary>
            Named
        }
    }
}
