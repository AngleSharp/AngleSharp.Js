namespace AngleSharp.Js.Cache
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// What a call of a DOM member is wrapped in, the same for a registered member as in
    /// <see cref="EngineExtensions.Call"/>: the engine is the current one, the reactions of custom
    /// elements wait for the call to be complete, and a script the call inserted runs before it
    /// returns, as in browsers.
    /// </summary>
    /// <remarks>
    /// A struct disposed by a using block, so a call costs no allocation for it - and nothing at
    /// all beyond two checks when no custom element is defined and no script is inserted.
    /// </remarks>
    readonly struct RegisteredScope : IDisposable
    {
        private readonly EngineInstance _instance;
        private readonly List<IHtmlScriptElement> _inserted;
        private readonly Boolean _holdsReactions;

        private RegisteredScope(EngineInstance instance, List<IHtmlScriptElement> inserted, Boolean holdsReactions)
        {
            _instance = instance;
            _inserted = inserted;
            _holdsReactions = holdsReactions;
        }

        public static RegisteredScope Enter(EngineInstance instance, Object target, JsValue[] arguments)
        {
            EngineInstance.Current = instance;
            var inserted = InsertedScripts.FindUnstarted(target, arguments ?? Array.Empty<JsValue>());
            var holdsReactions = CustomElementRegistry.IsInUse;

            if (holdsReactions)
            {
                CustomElementRegistry.PushReactions();
            }

            return new RegisteredScope(instance, inserted, holdsReactions);
        }

        /// <summary>
        /// Marks the call as having succeeded, which is when the scripts it inserted run - still
        /// within the scope, as for a reflected member.
        /// </summary>
        public void Complete()
        {
            if (_inserted != null)
            {
                InsertedScripts.RunPrepared(_instance, _instance.Document, _inserted);
            }
        }

        public void Dispose()
        {
            if (_holdsReactions)
            {
                CustomElementRegistry.PopReactions();
            }
        }
    }

    /// <summary>
    /// What every registered member shares: finding its engine and receiver, and turning what the
    /// CLR throws into what script sees - the parts the reflection path does in
    /// <see cref="EngineExtensions.Call"/>.
    /// </summary>
    static class RegisteredCall
    {
        public static EngineInstance GetEngine(JsValue thisObject) =>
            thisObject.GetEngineInstance() ?? throw new JavaScriptException("Illegal invocation.");

        public static T GetTarget<T>(EngineInstance instance, JsValue thisObject) where T : class =>
            (thisObject is IDomProxy proxy ? proxy.Value : instance.Window.Value) as T
                ?? throw new JavaScriptException("Illegal invocation.");

        //  Jint's own exceptions (a script error, an exceeded constraint) pass as they are.
        public static Boolean IsHostError(Exception error) => !(error is JintException);

        public static JavaScriptException ToScriptError(EngineInstance instance, Exception error) =>
            new JavaScriptException(instance.ToScriptValue(error));

        /// <summary>
        /// Picks the overload for a call: the only one, or the best match of the signatures.
        /// </summary>
        public static Int32 Select(DomSignature[] signatures, JsValue[] arguments) =>
            signatures == null || signatures.Length < 2 ? 0 : signatures.SelectSignature(arguments);

        /// <summary>
        /// Adds an overload to a set, which is a single declaration without a signature until one
        /// with a signature comes along - that one starts the set anew, as declaring replaces.
        /// </summary>
        public static void Add<TDelegate>(ref TDelegate[] invokes, ref DomSignature[] signatures, TDelegate invoke, DomSignature signature)
        {
            if (signature == null || signatures == null)
            {
                invokes = new[] { invoke };
                signatures = signature != null ? new[] { signature } : null;
            }
            else
            {
                Array.Resize(ref invokes, invokes.Length + 1);
                Array.Resize(ref signatures, signatures.Length + 1);
                invokes[invokes.Length - 1] = invoke;
                signatures[signatures.Length - 1] = signature;
            }
        }

        //  What Function.length reports for a set of overloads: the fewest arguments any takes.
        public static Int32 GetLength(DomSignature[] signatures, Int32 length)
        {
            if (signatures == null)
            {
                return length;
            }

            var min = Int32.MaxValue;

            for (var i = 0; i < signatures.Length; i++)
            {
                min = Math.Min(min, signatures[i].Required);
            }

            return min;
        }
    }

    sealed class RegisteredMethod<T> : DomShapeBuilder.Member where T : class
    {
        private Func<T, DomArguments, Object>[] _invokes;
        private DomSignature[] _signatures;
        private Int32 _length;

        public RegisteredMethod(Func<T, DomArguments, Object> invoke, Int32 length)
        {
            _invokes = new[] { invoke };
            _length = length;
        }

        public RegisteredMethod(Func<T, DomArguments, Object> invoke, DomSignature signature)
        {
            Add(invoke, signature);
        }

        public Boolean IsOverloadSet => _signatures != null;

        public void Add(Func<T, DomArguments, Object> invoke, DomSignature signature)
        {
            RegisteredCall.Add(ref _invokes, ref _signatures, invoke, signature);
            _length = RegisteredCall.GetLength(_signatures, 0);
        }

        public override Boolean IsSameAs(DomShapeBuilder.Member other) => ReferenceEquals(this, other);

        public override void Declare(JsObjectShape.Builder builder, String name, Boolean perRealm)
        {
            if (perRealm)
            {
                var length = _length;
                builder.PerRealmSlot(
                    name,
                    prototype =>
                    {
                        var instance = DomPrototypeState.Of(prototype).Instance;
                        return new ClrFunction(instance.Jint, name, (thisObject, arguments) => Invoke(instance, thisObject, arguments), length);
                    },
                    enumerable: true,
                    writable: true,
                    configurable: true);
            }
            else
            {
                builder.Method(
                    name,
                    (thisObject, arguments) => Invoke(RegisteredCall.GetEngine(thisObject), thisObject, arguments),
                    _length,
                    enumerable: true,
                    writable: true,
                    configurable: true);
            }
        }

        //  Still asks the prototype above for the method, as it may have been patched there.
        public override void DeclareInherited(JsObjectShape.Builder builder, String name) =>
            builder.PerRealmSlot(
                name,
                prototype =>
                {
                    var instance = DomPrototypeState.Of(prototype).Instance;
                    return new ClrFunction(instance.Jint, name, (thisObject, arguments) =>
                        instance.Jint.Call(prototype.Prototype.Get(name), thisObject.IsObject() ? thisObject : instance.Window, arguments));
                },
                enumerable: true,
                writable: true,
                configurable: true);

        private JsValue Invoke(EngineInstance instance, JsValue thisObject, JsValue[] arguments)
        {
            var target = RegisteredCall.GetTarget<T>(instance, thisObject);
            var invoke = _invokes[RegisteredCall.Select(_signatures, arguments)];

            using (var scope = RegisteredScope.Enter(instance, target, arguments))
            {
                try
                {
                    var result = invoke.Invoke(target, instance.CreateArguments(arguments)).ToJsValue(instance);
                    scope.Complete();
                    return result;
                }
                catch (Exception error) when (RegisteredCall.IsHostError(error))
                {
                    throw RegisteredCall.ToScriptError(instance, error);
                }
            }
        }
    }

    sealed class RegisteredProperty<T, TValue> : DomShapeBuilder.Member where T : class
    {
        private readonly Func<T, TValue> _getter;
        private readonly Action<T, TValue> _setter;
        private readonly Boolean _sameObject;
        private readonly JsString _forwardTo;

        //  an IList<T> attribute is a WebIDL ObservableArray, an array for script, as by reflection
        private static readonly Type ObservableElementType = ObservableArrays.GetElementType(typeof(TValue));

        //  a WebIDL union is given to script as its conversion away, as by reflection
        private static readonly MethodInfo GivenConversion = typeof(TValue).GetGivenConversion();

        public RegisteredProperty(Func<T, TValue> getter, Action<T, TValue> setter, DomPropertyFlags flags, String forwardTo)
        {
            _getter = getter;
            _setter = setter;
            _sameObject = (flags & DomPropertyFlags.SameObject) != 0;
            _forwardTo = forwardTo != null ? new JsString(forwardTo) : null;
        }

        public override Boolean IsSameAs(DomShapeBuilder.Member other) => ReferenceEquals(this, other);

        //  An accessor always has a receiver to find its engine by - the global object itself
        //  when a window member is named bare - so it needs no per-realm variant.
        public override void Declare(JsObjectShape.Builder builder, String name, Boolean perRealm) =>
            builder.Accessor(name, Read, _forwardTo is null ? Write : Forward, enumerable: true, configurable: true);

        private JsValue Read(JsValue thisObject, JsValue[] arguments)
        {
            var instance = RegisteredCall.GetEngine(thisObject);
            var target = RegisteredCall.GetTarget<T>(instance, thisObject);

            using (var scope = RegisteredScope.Enter(instance, target, arguments))
            {
                try
                {
                    Object value = _getter.Invoke(target);

                    if (GivenConversion != null && value != null)
                    {
                        value = GivenConversion.Invoke(null, new[] { value });
                    }

                    //  untyped, so that the node keeps its identity and its own prototype
                    var result = ObservableElementType != null && value != null && !instance.IsOutOfReach(value)
                        ? instance.GetObservableArray(value, ObservableElementType)
                        : _sameObject && value != null
                            ? instance.GetSameObject(target, this, value, value.GetType())
                            : value.ToJsValue(instance);
                    scope.Complete();
                    return result;
                }
                catch (Exception error) when (RegisteredCall.IsHostError(error))
                {
                    throw RegisteredCall.ToScriptError(instance, error);
                }
            }
        }

        //  without a setter a write is ignored, as one to a readonly attribute is in sloppy mode
        private JsValue Write(JsValue thisObject, JsValue[] arguments)
        {
            if (_setter == null)
            {
                return JsValue.Undefined;
            }

            var instance = RegisteredCall.GetEngine(thisObject);
            var target = RegisteredCall.GetTarget<T>(instance, thisObject);

            using (var scope = RegisteredScope.Enter(instance, target, arguments))
            {
                try
                {
                    _setter.Invoke(target, instance.CreateArguments(arguments).Get<TValue>(0));
                    scope.Complete();
                    return JsValue.Undefined;
                }
                catch (Exception error) when (RegisteredCall.IsHostError(error))
                {
                    throw RegisteredCall.ToScriptError(instance, error);
                }
            }
        }

        //  [PutForwards] is an assignment to the attribute of the value, done as script would
        private JsValue Forward(JsValue thisObject, JsValue[] arguments)
        {
            if (Read(thisObject, arguments) is ObjectInstance value)
            {
                value.Set(_forwardTo, arguments.Length > 0 ? arguments[0] : JsValue.Undefined, value);
            }

            return JsValue.Undefined;
        }
    }

    sealed class RegisteredEvent : DomShapeBuilder.Member
    {
        private readonly DomEventDefinition _definition;

        public RegisteredEvent(DomEventDefinition definition)
        {
            _definition = definition;
        }

        public override Boolean IsSameAs(DomShapeBuilder.Member other) => ReferenceEquals(this, other);

        public override void Declare(JsObjectShape.Builder builder, String name, Boolean perRealm) =>
            builder.Accessor(name, _definition.GetHandler, _definition.SetHandler, enumerable: true, configurable: true);
    }

    /// <summary>
    /// A static member of a registered interface, found on its interface object.
    /// </summary>
    sealed class RegisteredStatic
    {
        private Func<DomArguments, Object>[] _invokes;
        private DomSignature[] _signatures;
        private Int32 _length;

        public RegisteredStatic(Func<DomArguments, Object> invoke, Int32 length)
        {
            _invokes = new[] { invoke };
            _length = length;
        }

        public RegisteredStatic(Func<DomArguments, Object> invoke, DomSignature signature)
        {
            Add(invoke, signature);
        }

        public RegisteredStatic(Func<EngineInstance, JsValue> read, Action<EngineInstance, JsValue> write)
        {
            Read = read;
            Write = write;
        }

        public Boolean IsOverloadSet => _signatures != null;

        public Boolean IsAccessor => Read != null;

        public Func<EngineInstance, JsValue> Read { get; }

        public Action<EngineInstance, JsValue> Write { get; }

        public Int32 Length => _length;

        public void Add(Func<DomArguments, Object> invoke, DomSignature signature)
        {
            RegisteredCall.Add(ref _invokes, ref _signatures, invoke, signature);
            _length = RegisteredCall.GetLength(_signatures, 0);
        }

        public JsValue Invoke(EngineInstance instance, JsValue[] arguments)
        {
            var invoke = _invokes[RegisteredCall.Select(_signatures, arguments)];

            using (var scope = RegisteredScope.Enter(instance, null, arguments))
            {
                try
                {
                    var result = invoke.Invoke(instance.CreateArguments(arguments)).ToJsValue(instance);
                    scope.Complete();
                    return result;
                }
                catch (Exception error) when (RegisteredCall.IsHostError(error))
                {
                    throw RegisteredCall.ToScriptError(instance, error);
                }
            }
        }

        public static RegisteredStatic ForProperty<TValue>(Func<TValue> getter, Action<TValue> setter) => new RegisteredStatic(
            instance => Guard(instance, () => ((Object)getter.Invoke()).ToJsValue(instance)),
            setter == null ? null : (instance, value) => Guard(instance, () =>
            {
                setter.Invoke(instance.CreateArguments(new[] { value }).Get<TValue>(0));
                return JsValue.Undefined;
            }));

        public static RegisteredStatic ForProperty<TValue>(Func<IWindow, TValue> getter, Action<IWindow, TValue> setter) => new RegisteredStatic(
            instance => Guard(instance, () => ((Object)getter.Invoke(instance.Window.Value as IWindow)).ToJsValue(instance)),
            setter == null ? null : (instance, value) => Guard(instance, () =>
            {
                setter.Invoke(instance.Window.Value as IWindow, instance.CreateArguments(new[] { value }).Get<TValue>(0));
                return JsValue.Undefined;
            }));

        //  a static accessor is rare enough for the closure this costs
        private static JsValue Guard(EngineInstance instance, Func<JsValue> run)
        {
            using (var scope = RegisteredScope.Enter(instance, null, null))
            {
                try
                {
                    var result = run.Invoke();
                    scope.Complete();
                    return result;
                }
                catch (Exception error) when (RegisteredCall.IsHostError(error))
                {
                    throw RegisteredCall.ToScriptError(instance, error);
                }
            }
        }
    }
}
