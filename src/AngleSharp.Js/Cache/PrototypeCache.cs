namespace AngleSharp.Js
{
    using Jint;
    using Jint.Native.Object;
    using System;
    using System.Collections.Concurrent;

    sealed class PrototypeCache
    {
        private readonly ConcurrentDictionary<Type, ObjectInstance> _prototypes;
        private readonly Engine _engine;

        public PrototypeCache(Engine engine)
        {
            _prototypes = new ConcurrentDictionary<Type, ObjectInstance>
            {
                [typeof(Object)] = engine.Intrinsics.Object.PrototypeObject,
            };
            _engine = engine;
        }

        public ObjectInstance GetOrCreate(Type type, Func<Type, ObjectInstance> creator) =>
            _prototypes.GetOrAdd(type, creator.Invoke);
    }
}
