namespace AngleSharp.Scripting.JavaScript
{
    using System;

    /// <summary>
    /// Represents a console logger.
    /// </summary>
    public interface IConsoleLogger
    {
        /// <summary>
        /// Logs the given values.
        /// </summary>
        /// <param name="values">The values to log.</param>
        void Log(Object[] values);
    }
}
