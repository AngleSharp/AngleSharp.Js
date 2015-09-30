namespace AngleSharp.Scripting.JavaScript.Services
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Navigator;
    using AngleSharp.Scripting.JavaScript.Dom;
    using AngleSharp.Services;

    /// <summary>
    /// Represents the service for a navigator.
    /// </summary>
    public class NavigatorService : INavigatorService
    {
        /// <summary>
        /// Creates an INavigator object for the provided window.
        /// </summary>
        /// <param name="window">The window that needs an INavigator.</param>
        /// <returns>The INavigator instance for the window.</returns>
        public virtual INavigator Create(IWindow window)
        {
            return new Navigator();
        }
    }
}
