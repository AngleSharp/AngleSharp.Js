namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using System;

    /// <summary>
    /// The arguments script passed to a registered operation, converted on request.
    /// </summary>
    /// <remarks>
    /// A struct handed by value, so a call costs no allocation beyond what Jint made already. The
    /// conversion is the one the reflection path applies to a parameter of the same type, which
    /// keeps both paths agreeing on what e.g. a number passed for a string becomes.
    /// </remarks>
    public readonly struct JsDomArguments
    {
        private readonly EngineInstance _engine;
        private readonly JsValue[] _values;

        internal JsDomArguments(EngineInstance engine, JsValue[] values)
        {
            _engine = engine;
            _values = values ?? Array.Empty<JsValue>();
        }

        /// <summary>
        /// Gets the number of arguments passed.
        /// </summary>
        public Int32 Length => _values.Length;

        /// <summary>
        /// Gets the window of the engine, which some operations take implicitly.
        /// </summary>
        public IWindow Window => _engine.Window.Value as IWindow;

        /// <summary>
        /// Gets if the argument was passed, and is not undefined - the test WebIDL applies to an
        /// optional argument.
        /// </summary>
        /// <param name="index">The position of the argument.</param>
        /// <returns>True if there is a value for it.</returns>
        public Boolean Has(Int32 index) => index >= 0 && index < _values.Length && !_values[index].IsUndefined();

        /// <summary>
        /// Gets the argument converted to the given type, or the default of the type if none was
        /// passed.
        /// </summary>
        /// <typeparam name="TValue">The type of the parameter.</typeparam>
        /// <param name="index">The position of the argument.</param>
        /// <returns>The converted argument.</returns>
        public TValue Get<TValue>(Int32 index) => Get(index, default(TValue));

        /// <summary>
        /// Gets the argument converted to the given type, or the fallback if it was not passed.
        /// </summary>
        /// <typeparam name="TValue">The type of the parameter.</typeparam>
        /// <param name="index">The position of the argument.</param>
        /// <param name="fallback">The default value of an optional parameter.</param>
        /// <returns>The converted argument.</returns>
        public TValue Get<TValue>(Int32 index, TValue fallback) =>
            Has(index) && _values[index].As(typeof(TValue), _engine) is TValue value ? value : fallback;

        /// <summary>
        /// Gets the argument as a callback, or null if it is not callable.
        /// </summary>
        /// <param name="index">The position of the argument.</param>
        /// <returns>The callback to invoke.</returns>
        public JsCallback GetCallback(Int32 index)
        {
            var function = Has(index) ? _values[index].AsFunction(_engine) : null;
            return function != null ? new JsCallback(function, _engine) : null;
        }
    }
}
