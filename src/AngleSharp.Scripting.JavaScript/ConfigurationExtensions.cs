namespace AngleSharp
{
    using AngleSharp.Scripting.JavaScript.Services;
    using AngleSharp.Services;
    using System.Linq;

    /// <summary>
    /// Additional extensions for JavaScript scripting.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Sets scripting to true, registers the JavaScript engine and returns
        /// a new configuration with the scripting service and possible
        /// auxiliary services, if not yet registered.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <returns>The new configuration.</returns>
        public static IConfiguration WithJavaScript(this IConfiguration configuration)
        {
            if (!configuration.Services.OfType<ScriptingService>().Any())
            {
                var service = new ScriptingService();

                if (!configuration.Services.OfType<INavigatorService>().Any())
                {
                    var navigator = new NavigatorService();
                    configuration = configuration.With(navigator);
                }

                return configuration.With(service);
            }

            return configuration;
        }
    }
}
