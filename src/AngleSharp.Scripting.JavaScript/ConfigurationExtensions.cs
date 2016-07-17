namespace AngleSharp
{
    using AngleSharp.Dom.Navigator;
    using AngleSharp.Scripting.JavaScript.Dom;
    using AngleSharp.Scripting.JavaScript.Services;
    using System;
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
            if (!configuration.Services.OfType<JavaScriptProvider>().Any())
            {
                var service = new JavaScriptProvider();

                if (!configuration.Services.OfType<Func<IBrowsingContext, INavigator>>().Any())
                {
                    configuration = configuration.With<INavigator>(context => new Navigator());
                }

                return configuration.With(service);
            }

            return configuration;
        }
    }
}
