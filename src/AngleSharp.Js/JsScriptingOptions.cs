namespace AngleSharp.Js
{
    using System;

    /// <summary>
    /// Options tuning the JavaScript engine.
    /// </summary>
    public sealed class JsScriptingOptions
    {
        /// <summary>
        /// Gets or sets the JavaScript call stack depth that has to be supported
        /// before the engine gives up with a "Maximum call stack size exceeded"
        /// error. Defaults to 10000, which is roughly what browsers allow. Values
        /// of zero or less remove the limit - the engine is then bounded by the
        /// native stack alone, so a runaway recursion terminates the process with
        /// an uncatchable StackOverflowException.
        /// </summary>
        public Int32 MaxCallStackDepth { get; set; } = 10000;

        /// <summary>
        /// Gets or sets if the source text of functions is kept, which is what
        /// <c>Function.prototype.toString()</c> gives, like in browsers (some pages
        /// store the code of functions this way, e.g. MediaWiki its modules).
        /// Defaults to true. Without it the source of scripts is not kept in memory,
        /// and <c>toString()</c> gives <c>function name() { [native code] }</c>.
        /// </summary>
        public Boolean RetainFunctionSourceText { get; set; } = true;

        //  An engine is built per window, long after the options were handed over, so
        //  reading them then would let a later edit of the caller's object decide how
        //  the next document behaves. The service takes this copy instead.
        internal JsScriptingOptions Clone() => new JsScriptingOptions
        {
            MaxCallStackDepth = MaxCallStackDepth,
            RetainFunctionSourceText = RetainFunctionSourceText,
        };
    }
}
