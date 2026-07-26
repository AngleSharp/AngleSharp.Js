namespace AngleSharp.Js
{
    using AngleSharp.Js.Cache;
    using Jint.Native;
    using Jint.Runtime.Descriptors;

    /// <summary>
    /// The property an exposed type is published under on the window and on the global
    /// object. A document names a handful of the types an assembly exposes, but a property
    /// is registered for every one of them, so the constructor object behind it is only
    /// built once script reads the property.
    /// </summary>
    sealed class DomConstructorDescriptor : PropertyDescriptor
    {
        private readonly EngineInstance _instance;
        private readonly ConstructorDefinition _definition;
        private JsValue _resolved;

        //  The attributes an eagerly written constructor had: enumerable, but neither
        //  writable nor configurable. CustomJsValue is what routes a read through
        //  CustomValue below; Jint reads that flag on every access instead of taking a
        //  copy of the value, so the descriptor keeps working once one of the engine's
        //  property caches has taken hold of it.
        public DomConstructorDescriptor(EngineInstance instance, ConstructorDefinition definition)
            : base(PropertyFlag.OnlyEnumerable | PropertyFlag.CustomJsValue)
        {
            _instance = instance;
            _definition = definition;
        }

        protected override JsValue CustomValue
        {
            get => _resolved ?? (_resolved = _instance.GetDomConstructor(_definition));
            set => _resolved = value;
        }
    }
}
