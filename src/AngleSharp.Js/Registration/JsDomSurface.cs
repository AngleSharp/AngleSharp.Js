namespace AngleSharp.Js.Cache
{
    using AngleSharp.Dom;
    using AngleSharp.Js.Proxies;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The DOM surface a <see cref="JsDomRegistration"/> declared, standing in for the libraries
    /// otherwise reflected over.
    /// </summary>
    /// <remarks>
    /// It produces the same <see cref="DomShape"/>, <see cref="ConstructorDefinition"/> and
    /// <see cref="IndexedCollection"/> the reflection path does, so prototypes, receivers and the
    /// engine's per-realm handling stay one code path; only where they come from differs. Like
    /// <see cref="DomShapeCache"/> it is shared by every engine of the service that built it:
    /// what is filled in by the registration is only read afterwards, and what is computed on
    /// demand sits in concurrent caches.
    /// </remarks>
    sealed class JsDomSurface
    {
        private const String ConstructorName = "constructor";

        private readonly Registry _registry;
        private readonly ConcurrentDictionary<Type, Type> _canonical;
        private readonly ConcurrentDictionary<Type, DomShape> _shapes;
        private readonly Func<Type, Type> _canonicalize;
        private readonly Func<Type, DomShape> _build;

        private JsDomSurface(Registry registry)
        {
            _registry = registry;
            _canonical = new ConcurrentDictionary<Type, Type>();
            _shapes = new ConcurrentDictionary<Type, DomShape>();

            //  cached once, so that a lookup - hit or miss - allocates no delegate
            _canonicalize = FindCanonical;
            _build = BuildShape;
        }

        public static JsDomSurface Create(JsDomRegistration registration)
        {
            var registry = new Registry();
            registration.Invoke(registry);
            registry.Complete();
            return new JsDomSurface(registry);
        }

        /// <summary>
        /// Gets the registered type the prototype of an object of the given type is built from:
        /// the most derived registered type it is assignable to, or Object if there is none.
        /// </summary>
        public Type Canonicalize(Type type) => _canonical.GetOrAdd(type, _canonicalize);

        public DomShape GetShape(Type type) => _shapes.GetOrAdd(type, _build);

        public IndexedCollection GetCollection(Type type) =>
            _registry.Types.TryGetValue(type, out var registered) ? registered.Collection : null;

        public String GetLiteral(Enum value) =>
            _registry.LiteralNames.TryGetValue(value, out var literal) ? literal : null;

        public Boolean TryParseLiteral(Type type, String literal, out Object value)
        {
            value = null;
            return _registry.LiteralValues.TryGetValue(type, out var values) && values.TryGetValue(literal, out value);
        }

        /// <summary>
        /// Publishes on the window what the reflection path finds in the exported types of the
        /// libraries, in the same order: interface objects and constants, named constructors,
        /// instances.
        /// </summary>
        public void Publish(EngineInstance engine, ObjectInstance window)
        {
            foreach (var registered in _registry.Ordered)
            {
                if (registered.Definition == null)
                {
                    continue;
                }

                foreach (var name in registered.Definition.Names)
                {
                    window.FastSetProperty(name, EngineExtensions.CreateConstructorProperty(engine, registered.Definition));
                }
            }

            foreach (var constants in _registry.ConstantObjects)
            {
                EngineExtensions.SetEnumLiteral(engine, window, constants);
            }

            foreach (var function in _registry.ConstructorFunctions)
            {
                var instance = new DomConstructorFunctionInstance(engine, function.Name, function.Type, function.Create);
                instance.DefineOwnProperty("length", new PropertyDescriptor(function.Length, false, false, true));
                window.FastSetProperty(function.Name, new PropertyDescriptor(instance, false, true, false));
            }

            foreach (var instance in _registry.Instances)
            {
                var value = instance.Value.Invoke((IWindow)engine.Window.Value);

                if (value != null)
                {
                    window.FastSetProperty(instance.Key, new PropertyDescriptor(engine.GetDomNode(value), false, true, false));
                }
            }
        }

        private Type FindCanonical(Type type)
        {
            if (_registry.Types.ContainsKey(type))
            {
                return type;
            }

            var best = default(Type);
            var ordered = _registry.Ordered;

            //  Registration order breaks a tie between unrelated interfaces, so the outcome does
            //  not depend on the order reflection happens to list them in. A mixin is no link of
            //  the prototype chain, so it never is the prototype of an object.
            for (var i = 0; i < ordered.Count; i++)
            {
                var candidate = ordered[i];

                if (!candidate.IsMixin && candidate.Type.IsAssignableFrom(type) &&
                    (best == null || best.IsAssignableFrom(candidate.Type) && best != candidate.Type))
                {
                    best = candidate.Type;
                }
            }

            return best ?? typeof(Object);
        }

        private DomShape BuildShape(Type type)
        {
            var registered = _registry.Types[type];
            var builder = new JsObjectShape.Builder();
            var members = new Dictionary<String, DomShapeBuilder.Member>(StringComparer.Ordinal);

            //  the window's members are called bare, see DomShapeBuilder
            var perRealm = typeof(IWindow).IsAssignableFrom(type);
            builder.ToStringTag(registered.Name);
            Declare(builder, registered, members, perRealm);

            foreach (var mixin in registered.Includes)
            {
                Declare(builder, _registry.Types[mixin], members, perRealm);
            }

            if (perRealm)
            {
                DeclareInherited(builder, registered, members);
            }

            foreach (var constant in registered.Interface.Constants)
            {
                if (!members.ContainsKey(constant.Key))
                {
                    builder.Constant(constant.Key, JsNumber.Create(constant.Value), true);
                }
            }

            //  the very object script reads off the window under the type's name, if there is one
            if (registered.Definition != null)
            {
                builder.PerRealmSlot(ConstructorName, DomShapeBuilder.CreateConstructor, enumerable: false, writable: true, configurable: true);
            }

            return new DomShape(type, registered.BaseType ?? GetRootType(type), registered.Name, builder.Build(), registered.Indexers, registered.Definition, members);
        }

        //  An exception inheriting no registered type is an Error still, as it is by reflection.
        private static Type GetRootType(Type type) =>
            typeof(Exception).IsAssignableFrom(type) ? typeof(Exception) : typeof(Object);

        private static void Declare(JsObjectShape.Builder builder, RegisteredType registered, Dictionary<String, DomShapeBuilder.Member> members, Boolean perRealm)
        {
            for (var i = 0; i < registered.Order.Count; i++)
            {
                var name = registered.Order[i];

                if (!members.ContainsKey(name) && !String.Equals(name, ConstructorName, StringComparison.Ordinal))
                {
                    var member = registered.Members[name];
                    member.Declare(builder, name, perRealm);
                    members.Add(name, member);
                }
            }
        }

        //  A bare addEventListener(...) on the window has to work even though EventTarget declares it.
        private void DeclareInherited(JsObjectShape.Builder builder, RegisteredType registered, Dictionary<String, DomShapeBuilder.Member> members)
        {
            for (var baseType = registered.BaseType; baseType != null && _registry.Types.TryGetValue(baseType, out var ancestor); baseType = ancestor.BaseType)
            {
                DeclareInheritedOf(builder, ancestor, members);

                foreach (var mixin in ancestor.Includes)
                {
                    DeclareInheritedOf(builder, _registry.Types[mixin], members);
                }
            }
        }

        private static void DeclareInheritedOf(JsObjectShape.Builder builder, RegisteredType ancestor, Dictionary<String, DomShapeBuilder.Member> members)
        {
            for (var i = 0; i < ancestor.Order.Count; i++)
            {
                var name = ancestor.Order[i];

                if (!members.ContainsKey(name) && !String.Equals(name, ConstructorName, StringComparison.Ordinal))
                {
                    var member = ancestor.Members[name];
                    member.DeclareInherited(builder, name);
                    members.Add(name, member);
                }
            }
        }

        private sealed class RegisteredType
        {
            public RegisteredType(Type type, String name, Boolean isMixin)
            {
                Type = type;
                Name = name;
                IsMixin = isMixin;
                Names = new List<String> { name };
                Includes = new List<Type>();
                Order = new List<String>();
                Members = new Dictionary<String, DomShapeBuilder.Member>(StringComparer.Ordinal);
                Interface = new RegisteredInterface();
            }

            public Type Type { get; }

            public String Name { get; }

            public Boolean IsMixin { get; }

            public Boolean HasInterfaceObject { get; set; } = true;

            public Type BaseType { get; set; }

            public List<String> Names { get; }

            public List<Type> Includes { get; }

            public List<String> Order { get; }

            public Dictionary<String, DomShapeBuilder.Member> Members { get; }

            public RegisteredInterface Interface { get; }

            public Indexer NumericIndexer { get; set; }

            public Indexer NamedGetter { get; set; }

            public Indexer NamedSetter { get; set; }

            public IndexedCollection Collection { get; set; }

            public DomIndexers Indexers { get; private set; }

            public ConstructorDefinition Definition { get; private set; }

            //  a member declared again replaces the earlier one in place, as a later library wins
            public void Define(String name, DomShapeBuilder.Member member)
            {
                Registry.CheckName(name);

                if (!Members.ContainsKey(name))
                {
                    Order.Add(name);
                }

                Members[name] = member;
            }

            public void Complete()
            {
                if (NumericIndexer != null || NamedGetter != null || NamedSetter != null)
                {
                    Indexers = new DomIndexers(NumericIndexer, NamedGetter, NamedSetter,
                        typeof(IStringMap).IsAssignableFrom(Type) ? DomIndexers.ToDatasetName : null);
                }

                Interface.Complete();
                Definition = HasInterfaceObject ? new ConstructorDefinition(Type, Names.ToArray(), Interface) : null;
            }
        }

        private sealed class RegisteredConstructorFunction
        {
            public RegisteredConstructorFunction(String name, Type type, Func<EngineInstance, JsValue[], Object> create, Int32 length)
            {
                Name = name;
                Type = type;
                Create = create;
                Length = length;
            }

            public String Name { get; }

            public Type Type { get; }

            public Func<EngineInstance, JsValue[], Object> Create { get; }

            public Int32 Length { get; }
        }

        private sealed class Registry : IJsDomRegistry
        {
            private readonly Dictionary<String, List<EnumLiteralMember>> _constants = new Dictionary<String, List<EnumLiteralMember>>(StringComparer.Ordinal);
            private readonly List<String> _constantOrder = new List<String>();

            public Dictionary<Type, RegisteredType> Types { get; } = new Dictionary<Type, RegisteredType>();

            public List<RegisteredType> Ordered { get; } = new List<RegisteredType>();

            public List<EnumLiteralDefinition> ConstantObjects { get; } = new List<EnumLiteralDefinition>();

            public List<RegisteredConstructorFunction> ConstructorFunctions { get; } = new List<RegisteredConstructorFunction>();

            public List<KeyValuePair<String, Func<IWindow, Object>>> Instances { get; } = new List<KeyValuePair<String, Func<IWindow, Object>>>();

            //  keyed by the boxed value, whose equality is the one of the enum
            public Dictionary<Object, String> LiteralNames { get; } = new Dictionary<Object, String>();

            public Dictionary<Type, Dictionary<String, Object>> LiteralValues { get; } = new Dictionary<Type, Dictionary<String, Object>>();

            public static void CheckName(String name)
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("A DOM name must not be empty.", nameof(name));
                }
            }

            public IJsTypeRegistration<T> Type<T>(String name) where T : class => Declare<T>(name, false);

            public IJsTypeRegistration<T> Mixin<T>(String name) where T : class => Declare<T>(name, true);

            public IJsConstantsRegistration Constants(String name)
            {
                CheckName(name);

                if (!_constants.TryGetValue(name, out var members))
                {
                    members = new List<EnumLiteralMember>();
                    _constants.Add(name, members);
                    _constantOrder.Add(name);
                }

                return new ConstantsRegistration(members);
            }

            public IJsLiteralsRegistration<TEnum> Literals<TEnum>() where TEnum : struct, Enum
            {
                if (!LiteralValues.TryGetValue(typeof(TEnum), out var values))
                {
                    values = new Dictionary<String, Object>(StringComparer.Ordinal);
                    LiteralValues.Add(typeof(TEnum), values);
                }

                return new LiteralsRegistration<TEnum>(this, values);
            }

            public void ConstructorFunction<T>(String name, Func<JsDomArguments, T> create, Int32 length = 0) where T : class
            {
                CheckName(name);
                create = create ?? throw new ArgumentNullException(nameof(create));
                ConstructorFunctions.Add(new RegisteredConstructorFunction(name, typeof(T), (instance, arguments) =>
                {
                    using (var scope = RegisteredScope.Enter(instance, null, arguments))
                    {
                        try
                        {
                            var result = create.Invoke(new JsDomArguments(instance, arguments));
                            scope.Complete();
                            return result;
                        }
                        catch (Exception error) when (RegisteredCall.IsHostError(error))
                        {
                            throw RegisteredCall.ToScriptError(instance, error);
                        }
                    }
                }, length));
            }

            public void Instance(String name, Func<IWindow, Object> create)
            {
                CheckName(name);
                Instances.Add(new KeyValuePair<String, Func<IWindow, Object>>(name, create ?? throw new ArgumentNullException(nameof(create))));
            }

            private IJsTypeRegistration<T> Declare<T>(String name, Boolean isMixin) where T : class
            {
                CheckName(name);

                if (Types.TryGetValue(typeof(T), out var existing))
                {
                    if (!String.Equals(existing.Name, name, StringComparison.Ordinal) || existing.IsMixin != isMixin)
                    {
                        throw new InvalidOperationException($"{typeof(T)} is already registered as {(existing.IsMixin ? "mixin" : "type")} '{existing.Name}'.");
                    }
                }
                else
                {
                    existing = new RegisteredType(typeof(T), name, isMixin);
                    Types.Add(typeof(T), existing);
                    Ordered.Add(existing);
                }

                return new TypeRegistration<T>(existing);
            }

            //  Checked once everything is registered, as a base may be declared after its heir.
            public void Complete()
            {
                foreach (var registered in Ordered)
                {
                    if (registered.BaseType != null && (!Types.TryGetValue(registered.BaseType, out var baseType) || baseType.IsMixin))
                    {
                        throw new InvalidOperationException($"'{registered.Name}' inherits from {registered.BaseType}, which is not a registered type.");
                    }

                    foreach (var mixin in registered.Includes)
                    {
                        if (!Types.TryGetValue(mixin, out var included) || !included.IsMixin)
                        {
                            throw new InvalidOperationException($"'{registered.Name}' includes {mixin}, which is not a registered mixin.");
                        }
                    }

                    if (registered.IsMixin && (registered.BaseType != null || registered.Includes.Count > 0 || registered.Interface.IsConstructible))
                    {
                        throw new InvalidOperationException($"The mixin '{registered.Name}' can neither inherit, include nor be constructed.");
                    }

                    if (!registered.HasInterfaceObject && (registered.Names.Count > 1 || registered.Interface.IsConstructible || registered.Interface.HasStatics))
                    {
                        throw new InvalidOperationException($"'{registered.Name}' has no interface object to carry an alias, a constructor or static members.");
                    }

                    registered.Complete();
                }

                foreach (var name in _constantOrder)
                {
                    ConstantObjects.Add(new EnumLiteralDefinition(name, _constants[name].ToArray()));
                }
            }
        }

        private sealed class ConstantsRegistration : IJsConstantsRegistration
        {
            private readonly List<EnumLiteralMember> _members;

            public ConstantsRegistration(List<EnumLiteralMember> members)
            {
                _members = members;
            }

            public IJsConstantsRegistration Constant(String name, Double value)
            {
                Registry.CheckName(name);
                _members.Add(new EnumLiteralMember(name, value));
                return this;
            }
        }

        private sealed class LiteralsRegistration<TEnum> : IJsLiteralsRegistration<TEnum> where TEnum : struct, Enum
        {
            private readonly Registry _registry;
            private readonly Dictionary<String, Object> _values;

            public LiteralsRegistration(Registry registry, Dictionary<String, Object> values)
            {
                _registry = registry;
                _values = values;
            }

            public IJsLiteralsRegistration<TEnum> Literal(TEnum value, String literal)
            {
                literal = literal ?? throw new ArgumentNullException(nameof(literal));
                Object boxed = value;
                _registry.LiteralNames[boxed] = literal;
                _values[literal] = boxed;
                return this;
            }
        }

        private sealed class TypeRegistration<T> : IJsTypeRegistration<T> where T : class
        {
            private readonly RegisteredType _type;

            public TypeRegistration(RegisteredType type)
            {
                _type = type;
            }

            public IJsTypeRegistration<T> Inherits<TBase>() where TBase : class
            {
                if (typeof(TBase) == typeof(T) || !typeof(TBase).IsAssignableFrom(typeof(T)))
                {
                    throw new InvalidOperationException($"'{_type.Name}' cannot inherit from {typeof(TBase)}.");
                }

                _type.BaseType = typeof(TBase);
                return this;
            }

            public IJsTypeRegistration<T> Includes<TMixin>() where TMixin : class
            {
                if (typeof(TMixin) == typeof(T) || !typeof(TMixin).IsAssignableFrom(typeof(T)))
                {
                    throw new InvalidOperationException($"'{_type.Name}' cannot include {typeof(TMixin)}.");
                }

                if (!_type.Includes.Contains(typeof(TMixin)))
                {
                    _type.Includes.Add(typeof(TMixin));
                }

                return this;
            }

            public IJsTypeRegistration<T> WithoutInterfaceObject()
            {
                _type.HasInterfaceObject = false;
                return this;
            }

            public IJsTypeRegistration<T> Alias(String name)
            {
                Registry.CheckName(name);

                if (!_type.Names.Contains(name))
                {
                    _type.Names.Add(name);
                }

                return this;
            }

            public IJsTypeRegistration<T> Constructor(Func<JsDomArguments, T> create, Int32 length = 0)
            {
                _type.Interface.AddConstructor(create ?? throw new ArgumentNullException(nameof(create)), null, length);
                return this;
            }

            public IJsTypeRegistration<T> Constructor(Func<JsDomArguments, T> create, JsDomSignature signature)
            {
                _type.Interface.AddConstructor(
                    create ?? throw new ArgumentNullException(nameof(create)),
                    signature ?? throw new ArgumentNullException(nameof(signature)),
                    0);
                return this;
            }

            public IJsTypeRegistration<T> Property<TValue>(String name, Func<T, TValue> getter, Action<T, TValue> setter = null, JsDomPropertyFlags flags = JsDomPropertyFlags.None)
            {
                _type.Define(name, new RegisteredProperty<T, TValue>(getter ?? throw new ArgumentNullException(nameof(getter)), setter, flags, null));
                return this;
            }

            public IJsTypeRegistration<T> ForwardingProperty<TValue>(String name, Func<T, TValue> getter, String forwardTo)
            {
                Registry.CheckName(forwardTo);
                _type.Define(name, new RegisteredProperty<T, TValue>(getter ?? throw new ArgumentNullException(nameof(getter)), null, JsDomPropertyFlags.None, forwardTo));
                return this;
            }

            public IJsTypeRegistration<T> Method(String name, Func<T, JsDomArguments, Object> invoke, Int32 length = 0)
            {
                _type.Define(name, new RegisteredMethod<T>(invoke ?? throw new ArgumentNullException(nameof(invoke)), length));
                return this;
            }

            public IJsTypeRegistration<T> Method(String name, Action<T, JsDomArguments> invoke, Int32 length = 0) =>
                Method(name, ToFunc(invoke), length);

            public IJsTypeRegistration<T> Method(String name, Func<T, JsDomArguments, Object> invoke, JsDomSignature signature)
            {
                invoke = invoke ?? throw new ArgumentNullException(nameof(invoke));
                signature = signature ?? throw new ArgumentNullException(nameof(signature));

                if (_type.Members.TryGetValue(name ?? String.Empty, out var existing) && existing is RegisteredMethod<T> method && method.IsOverloadSet)
                {
                    method.Add(invoke, signature);
                }
                else
                {
                    _type.Define(name, new RegisteredMethod<T>(invoke, signature));
                }

                return this;
            }

            public IJsTypeRegistration<T> Method(String name, Action<T, JsDomArguments> invoke, JsDomSignature signature) =>
                Method(name, ToFunc(invoke), signature);

            public IJsTypeRegistration<T> EventHandler(String name, Action<T, DomEventHandler> add, Action<T, DomEventHandler> remove)
            {
                add = add ?? throw new ArgumentNullException(nameof(add));
                remove = remove ?? throw new ArgumentNullException(nameof(remove));
                var definition = new DomEventDefinition((target, handler) => add.Invoke((T)target, handler), (target, handler) => remove.Invoke((T)target, handler));
                _type.Define(name, new RegisteredEvent(definition));
                return this;
            }

            public IJsTypeRegistration<T> Constant(String name, Double value)
            {
                Registry.CheckName(name);
                _type.Interface.AddConstant(name, value);
                return this;
            }

            public IJsTypeRegistration<T> Static(String name, Func<JsDomArguments, Object> invoke, Int32 length = 0)
            {
                Registry.CheckName(name);
                _type.Interface.DefineStatic(name, new RegisteredStatic(invoke ?? throw new ArgumentNullException(nameof(invoke)), length));
                return this;
            }

            public IJsTypeRegistration<T> Static(String name, Func<JsDomArguments, Object> invoke, JsDomSignature signature)
            {
                Registry.CheckName(name);
                invoke = invoke ?? throw new ArgumentNullException(nameof(invoke));
                signature = signature ?? throw new ArgumentNullException(nameof(signature));

                if (_type.Interface.TryGetStatic(name, out var existing) && existing.IsOverloadSet)
                {
                    existing.Add(invoke, signature);
                }
                else
                {
                    _type.Interface.DefineStatic(name, new RegisteredStatic(invoke, signature));
                }

                return this;
            }

            public IJsTypeRegistration<T> StaticProperty<TValue>(String name, Func<TValue> getter, Action<TValue> setter = null)
            {
                Registry.CheckName(name);
                _type.Interface.DefineStatic(name, RegisteredStatic.ForProperty(getter ?? throw new ArgumentNullException(nameof(getter)), setter));
                return this;
            }

            public IJsTypeRegistration<T> Indexer(Func<T, Int32, Object> getter)
            {
                getter = getter ?? throw new ArgumentNullException(nameof(getter));
                _type.NumericIndexer = new Indexer((target, arguments, engine) => Guard(() => getter.Invoke((T)target, (Int32)arguments[0])));
                return this;
            }

            public IJsTypeRegistration<T> Collection(Func<T, Int32> length, Func<T, Int32, Object> item)
            {
                length = length ?? throw new ArgumentNullException(nameof(length));
                item = item ?? throw new ArgumentNullException(nameof(item));
                _type.Collection = IndexedCollection.Create(target => length.Invoke((T)target), (target, index) => item.Invoke((T)target, index));

                //  what the reflection path has for a collection too: its numeric half is the
                //  engine's, but an ordinary node proxy of it is still indexed
                if (_type.NumericIndexer == null)
                {
                    Indexer(item);
                }

                return this;
            }

            public IJsTypeRegistration<T> NamedIndexer<TValue>(Func<T, String, TValue> getter, Action<T, String, TValue> setter = null)
            {
                getter = getter ?? throw new ArgumentNullException(nameof(getter));
                _type.NamedGetter = new Indexer((target, arguments, engine) => Guard(() => getter.Invoke((T)target, (String)arguments[0])));
                _type.NamedSetter = setter == null ? null : new Indexer((target, arguments, engine) => Guard(() =>
                {
                    var value = arguments[1] is JsValue js ? js.As(typeof(TValue), engine) : arguments[1];
                    setter.Invoke((T)target, (String)arguments[0], value is TValue typed ? typed : default);
                    return null;
                }));
                return this;
            }

            private static Func<T, JsDomArguments, Object> ToFunc(Action<T, JsDomArguments> invoke)
            {
                invoke = invoke ?? throw new ArgumentNullException(nameof(invoke));
                return (target, arguments) =>
                {
                    invoke.Invoke(target, arguments);
                    return null;
                };
            }

            //  An indexer fails like a reflected one, which is how an index out of range is told.
            private static Object Guard(Func<Object> run)
            {
                try
                {
                    return run.Invoke();
                }
                catch (Exception error) when (RegisteredCall.IsHostError(error))
                {
                    throw new TargetInvocationException(error);
                }
            }
        }
    }

    /// <summary>
    /// What a registration declared of an interface object: its constructors, static members
    /// and constants. It is read by <see cref="DomConstructorInstance"/> in place of reflecting
    /// over the type.
    /// </summary>
    sealed class RegisteredInterface
    {
        private readonly List<Func<JsDomArguments, Object>> _constructors = new List<Func<JsDomArguments, Object>>();
        private readonly List<JsDomSignature> _constructorSignatures = new List<JsDomSignature>();
        private readonly List<String> _staticOrder = new List<String>();
        private readonly Dictionary<String, RegisteredStatic> _statics = new Dictionary<String, RegisteredStatic>(StringComparer.Ordinal);
        private readonly List<KeyValuePair<String, Double>> _constants = new List<KeyValuePair<String, Double>>();
        private Func<JsDomArguments, Object>[] _creates;
        private JsDomSignature[] _signatures;
        private Int32 _length;

        public Boolean IsConstructible => _constructors.Count > 0;

        public Boolean HasStatics => _statics.Count > 0;

        public IReadOnlyList<KeyValuePair<String, Double>> Constants => _constants;

        public void AddConstructor(Func<JsDomArguments, Object> create, JsDomSignature signature, Int32 length)
        {
            //  as for methods: one without a signature replaces, ones with a signature accumulate
            if (signature == null || _constructorSignatures.Count == 0 || _constructorSignatures[0] == null)
            {
                _constructors.Clear();
                _constructorSignatures.Clear();
            }

            _constructors.Add(create);
            _constructorSignatures.Add(signature);
            _length = signature == null ? length : Math.Min(_constructors.Count == 1 ? Int32.MaxValue : _length, signature.Required);
        }

        public void AddConstant(String name, Double value) => _constants.Add(new KeyValuePair<String, Double>(name, value));

        public Boolean TryGetStatic(String name, out RegisteredStatic member) => _statics.TryGetValue(name, out member);

        public void DefineStatic(String name, RegisteredStatic member)
        {
            if (!_statics.ContainsKey(name))
            {
                _staticOrder.Add(name);
            }

            _statics[name] = member;
        }

        public void Complete()
        {
            _creates = _constructors.ToArray();
            _signatures = _constructorSignatures.Count > 0 && _constructorSignatures[0] != null ? _constructorSignatures.ToArray() : null;
        }

        public Object Construct(EngineInstance instance, JsValue[] arguments)
        {
            var create = _creates[RegisteredCall.Select(_signatures, arguments)];

            using (var scope = RegisteredScope.Enter(instance, null, arguments))
            {
                try
                {
                    var result = create.Invoke(new JsDomArguments(instance, arguments));
                    scope.Complete();
                    return result;
                }
                catch (Exception error) when (RegisteredCall.IsHostError(error))
                {
                    throw RegisteredCall.ToScriptError(instance, error);
                }
            }
        }

        public void AddStaticMembers(EngineInstance instance, ObjectInstance constructor)
        {
            var engine = instance.Jint;

            if (IsConstructible)
            {
                constructor.DefineOwnProperty("length", new PropertyDescriptor(_length, false, false, true));
            }

            foreach (var constant in _constants)
            {
                constructor.FastSetProperty(constant.Key, new PropertyDescriptor(constant.Value, false, true, false));
            }

            foreach (var name in _staticOrder)
            {
                var member = _statics[name];

                if (member.IsAccessor)
                {
                    var get = new ClrFunction(engine, "get " + name, (_, arguments) => member.Read(instance));
                    var set = member.Write == null ? null : new ClrFunction(engine, "set " + name, (_, arguments) =>
                    {
                        member.Write(instance, arguments.Length > 0 ? arguments[0] : JsValue.Undefined);
                        return JsValue.Undefined;
                    });
                    constructor.FastSetProperty(name, new GetSetPropertyDescriptor(get, set, true, true));
                }
                else
                {
                    var function = new ClrFunction(engine, name, (_, arguments) => member.Invoke(instance, arguments), member.Length);
                    constructor.FastSetProperty(name, new PropertyDescriptor(function, true, true, true));
                }
            }
        }
    }
}
