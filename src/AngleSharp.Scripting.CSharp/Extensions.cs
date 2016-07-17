namespace AngleSharp.Scripting.CSharp
{
    using AngleSharp.Dom;
    using System;

    /// <summary>
    /// A set of extensions to use the C# scripting library.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts the provided object to a dynamic node.
        /// </summary>
        /// <param name="node">The node to treat dynamically.</param>
        /// <returns>The dynamic object.</returns>
        public static dynamic ToDynamic(this INode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            return new DynamicDomObject(node);
        }

        /// <summary>
        /// Creates a dynamic document from the provided browsing context.
        /// </summary>
        /// <param name="context">The host of the current document.</param>
        /// <returns>The dynamic object.</returns>
        public static dynamic GetDynamicDocument(this IBrowsingContext context)
        {
            return context.Active.ToDynamic();
        }

        /// <summary>
        /// Uses the provided creator function to supply a special wrapper for
        /// the (default) IWindow instance.
        /// </summary>
        /// <param name="configuration">The configuration to extend.</param>
        /// <param name="creator">The function to create a new window.</param>
        /// <returns>The new configuration with the supplied service.</returns>
        public static IConfiguration WithWindowContext(this IConfiguration configuration, Func<IWindow, WindowContext> creator)
        {
            return configuration.With<IWindow>(context =>
            {
                var window = creator.Invoke(context.Current);
                window.Run();
                return window;
            });
        }
    }
}
