namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents the high resolution time of a window (performance.now()), measured from the
    /// moment scripting started for its document.
    /// </summary>
    [DomName("Performance")]
    public sealed class Performance
    {
        private readonly Stopwatch _clock = Stopwatch.StartNew();
        private readonly Double _timeOrigin = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        /// <summary>
        /// Gets the time of the origin in milliseconds since the Unix epoch.
        /// </summary>
        [DomName("timeOrigin")]
        public Double TimeOrigin => _timeOrigin;

        /// <summary>
        /// Gets the milliseconds elapsed since the time origin.
        /// </summary>
        [DomName("now")]
        public Double Now() => _clock.Elapsed.TotalMilliseconds;
    }
}
