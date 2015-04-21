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
        /// a new configuration with the scripting service.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <returns>The new configuration.</returns>
        public static IConfiguration WithJavaScript(this IConfiguration configuration)
        {
            if (!configuration.Services.OfType<ScriptingService>().Any())
            {
                var service = new ScriptingService();
                return configuration.With(service);
            }

            return configuration;
        }
    }
}
