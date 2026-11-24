namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    /// <summary>
    /// Reads the arguments of a call to a registered member, one per engine, so that the
    /// <see cref="DomArguments"/> handed over cost no allocation.
    /// </summary>
    /// <remarks>
    /// The state is the JsValue[] Jint passed. It is read back as Object[] on purpose: Jint may
    /// pass a pooled array whose runtime type is Object[] although it is typed JsValue[], which a
    /// cast to JsValue[] rejects, while array covariance lets a cast to Object[] accept both. Each
    /// value is then cast on its own.
    /// </remarks>
    sealed class JsArgumentReader : DomArgumentReader
    {
        private readonly EngineInstance _engine;

        public JsArgumentReader(EngineInstance engine)
        {
            _engine = engine;
        }

        public DomArguments Create(JsValue[] values) =>
            new DomArguments(this, values ?? Array.Empty<JsValue>(), values?.Length ?? 0);

        protected override IWindow Window => _engine.Window.Value as IWindow;

        protected override Boolean IsPresent(Object state, Int32 index) => !At(state, index).IsUndefined();

        protected override Object Convert(Object state, Int32 index, Type type) => At(state, index).As(type, _engine);

        protected override Boolean TryGetMember(Object state, Int32 index, String name, Type type, out Object value)
        {
            if (At(state, index) is ObjectInstance obj && obj.HasProperty(name))
            {
                var member = obj.Get(name);

                if (!member.IsUndefined())
                {
                    value = member.As(type, _engine);
                    return true;
                }
            }

            value = null;
            return false;
        }

        protected override DomCallback GetCallback(Object state, Int32 index)
        {
            var current = At(state, index);
            var function = current.IsUndefined() ? null : current.AsFunction(_engine);
            return function != null ? new JsCallback(function, _engine) : null;
        }

        private static JsValue At(Object state, Int32 index) => (JsValue)((Object[])state)[index];
    }
}
