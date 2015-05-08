namespace AngleSharp.Scripting.CSharp
{
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
        public static dynamic ToDynamic(this Object node)
        {
            return new DynamicNode(node);
        }
    }
}
