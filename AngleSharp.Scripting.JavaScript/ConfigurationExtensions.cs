namespace AngleSharp
{
    using AngleSharp.Scripting.JavaScript;
    using System.Linq;

    /// <summary>
    /// Additional extensions for JavaScript scripting.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Sets scripting to true, registers the JavaScript engine and returns
        /// the same instance.
        /// </summary>
        /// <typeparam name="TConfiguration">Type of Configuration.</typeparam>
        /// <param name="configuration">The configuration to modify.</param>
        /// <returns>The same object, for chaining.</returns>
        public static TConfiguration WithJavaScript<TConfiguration>(this TConfiguration configuration)
            where TConfiguration : Configuration
        {
            if (!configuration.Services.OfType<ScriptingService>().Any())
                configuration.Register(new ScriptingService());

            return configuration;
        }
    }
}
