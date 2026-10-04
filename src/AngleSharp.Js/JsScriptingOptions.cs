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
        /// Gets or sets the factory used to create each window's Jint engine
        /// before any page script runs. The default creates a new engine with
        /// the supplied options.
        /// </summary>
        /// <remarks>
        /// The factory must construct a fresh engine using the supplied options,
        /// which contain the DOM module loader, object wrapper, and stack guard.
        /// It may configure additional Jint options before constructing the engine.
        /// DOM bindings are installed after the factory returns; do not access
        /// DOM objects or evaluate page scripts while creating the engine.
        /// The factory is retained when the scripting service copies these
        /// options. Captured state remains owned by the caller. The factory can
        /// run more than once when the service is used for multiple windows.
        /// </remarks>
        public Func<Jint.Options, Jint.Engine> EngineCreator { get; set; } = options => new Jint.Engine(options);

        //  An engine is built per window, long after the options were handed over, so
        //  reading them then would let a later edit of the caller's object decide how
        //  the next document behaves. The service takes this copy instead.
        internal JsScriptingOptions Clone() => new JsScriptingOptions
        {
            MaxCallStackDepth = MaxCallStackDepth,
            EngineCreator = EngineCreator ?? throw new ArgumentException("The engine creator cannot be null.", nameof(EngineCreator)),
        };
    }
}
