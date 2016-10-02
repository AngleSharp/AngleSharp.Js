namespace AngleSharp.Scripting.JavaScript
{
    using Jint;
    using Jint.Native.Object;
    using System;
    using System.Collections.Generic;

    sealed class PrototypeCache
    {
        private readonly Dictionary<Type, ObjectInstance> _prototypes;

        public PrototypeCache(Engine engine)
        {
            _prototypes = new Dictionary<Type, ObjectInstance>();
            _prototypes.Add(typeof(Object), engine.Object);
        }

        public ObjectInstance GetOrCreate(Type type, Func<Type, ObjectInstance> creator)
        {
            var instance = default(ObjectInstance);

            if (!_prototypes.TryGetValue(type, out instance))
            {
                instance = creator.Invoke(type);
                _prototypes.Add(type, instance);
            }

            return instance;
        }
    }
}
