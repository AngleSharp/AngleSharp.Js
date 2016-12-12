namespace AngleSharp
{
    using AngleSharp.Browser.Dom;
    using AngleSharp.Scripting.JavaScript;
    using AngleSharp.Scripting.JavaScript.Dom;
    using System;

    /// <summary>
    /// Additional extensions for JavaScript scripting.
    /// </summary>
    public static class JsConfigurationExtensions
    {
        /// <summary>
        /// Includes a service to create a new console logger for the given context.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <param name="createLogger">The delegate to create a new logger.</param>
        /// <returns>The new configuration.</returns>
        public static IConfiguration WithConsoleLogger(this IConfiguration configuration, Func<IBrowsingContext, IConsoleLogger> createLogger)
        {
            if (!configuration.Has<IConsoleLogger>())
            {
                configuration = configuration.With(createLogger);
            }

            return configuration;
        }

        /// <summary>
        /// Sets scripting to true, registers the JavaScript engine and returns
        /// a new configuration with the scripting service and possible
        /// auxiliary services, if not yet registered.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <returns>The new configuration.</returns>
        public static IConfiguration WithJavaScript(this IConfiguration configuration)
        {
            var service = new JsScriptingService();
            var observer = new EventAttributeObserver(service);

            if (!configuration.Has<INavigator>())
            {
                configuration = configuration.With<INavigator>(context => new Navigator());
            }

            return configuration.WithOnly(observer).With(service);
        }
    }
}
