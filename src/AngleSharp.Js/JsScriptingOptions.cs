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
        /// Gets or sets the callback used to configure each window's Jint engine
        /// before any script runs. Use this to install execution constraints or
        /// other host options. The DOM module loader, object wrapper, and call
        /// stack guard are installed after this callback.
        /// </summary>
        /// <remarks>
        /// The callback is retained when the scripting service copies these
        /// options. Captured state remains owned by the caller. The callback can
        /// run more than once when the service is used for multiple windows.
        /// </remarks>
        public Action<Jint.Options> ConfigureEngine { get; set; }

        //  An engine is built per window, long after the options were handed over, so
        //  reading them then would let a later edit of the caller's object decide how
        //  the next document behaves. The service takes this copy instead.
        internal JsScriptingOptions Clone() => new JsScriptingOptions
        {
            MaxCallStackDepth = MaxCallStackDepth,
            ConfigureEngine = ConfigureEngine,
        };
    }
}
