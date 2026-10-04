namespace AngleSharp.Js
{
    /// <summary>
    /// Creates a window's Jint engine. Register this service with the configuration
    /// to customize engine creation before page scripts run.
    /// </summary>
    /// <param name="options">Options containing the DOM wrapper, module loader, and stack guard.</param>
    /// <returns>A fresh engine constructed with the supplied options.</returns>
    /// <remarks>
    /// DOM bindings are installed after this delegate returns. Configure the engine
    /// and host globals here; access DOM objects and run page scripts afterwards.
    /// </remarks>
    public delegate Jint.Engine EngineCreator(Jint.Options options);
}
