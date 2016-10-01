namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using AngleSharp.Scripting.JavaScript.Services;
    using System;
    using System.Linq;

    /// <summary>
    /// Useful extensions for the DOM.
    /// </summary>
    public static class ApiExtensions
    {
        /// <summary>
        /// Executes the given script code in the context of the provided node.
        /// </summary>
        /// <param name="node">The node acting as context.</param>
        /// <param name="scriptCode">The script to run.</param>
        /// <returns>The result of running the script, if any.</returns>
        public static Object Execute(this INode node, String scriptCode)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var document = node.Owner;
            var options = document?.Context.Configuration;
            var providers = options?.Services.OfType<JavaScriptProvider>();
            var engine = providers?.FirstOrDefault()?.Engine;
            return engine?.EvaluateScript(node, scriptCode);
        }
    }
}
