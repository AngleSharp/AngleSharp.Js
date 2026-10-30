using AngleSharp.Attributes;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace AngleSharp.Js.Dom
{
    /// <summary>
    /// Defines the console. Every method of the console standard is present, as
    /// scripts routinely wrap or call any of them (e.g. console.error or console.info)
    /// and would fail on the first missing one. All output goes to the logger.
    /// </summary>
    [DomName("Console")]
    public sealed class Console
    {
        private const String DefaultLabel = "default";

        private readonly IConsoleLogger _logger;
        private readonly Dictionary<String, Int32> _counters = new Dictionary<String, Int32>();
        private readonly Dictionary<String, Stopwatch> _timers = new Dictionary<String, Stopwatch>();

        /// <summary>
        /// Creates a new console.
        /// </summary>
        /// <param name="window"></param>
        [DomConstructor]
        public Console(IWindow window)
        {
            var context = window.Document.Context;
            _logger = context.GetService<IConsoleLogger>();
        }

        /// <summary>
        /// Outputs a message to the console.
        /// </summary>
        /// <param name="objs"></param>
        [DomName("log")]
        public void Log(params Object[] objs)
        {
            _logger?.Log(objs);
        }

        /// <summary>
        /// Outputs an error message to the console.
        /// </summary>
        [DomName("error")]
        public void Error(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs a warning to the console.
        /// </summary>
        [DomName("warn")]
        public void Warn(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs an informational message to the console.
        /// </summary>
        [DomName("info")]
        public void Info(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs a debug message to the console.
        /// </summary>
        [DomName("debug")]
        public void Debug(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs a trace message to the console.
        /// </summary>
        [DomName("trace")]
        public void Trace(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs the given object to the console.
        /// </summary>
        [DomName("dir")]
        public void Dir(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs the given node to the console.
        /// </summary>
        [DomName("dirxml")]
        public void DirXml(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs the given tabular data to the console.
        /// </summary>
        [DomName("table")]
        public void Table(params Object[] objs) => Log(objs);

        /// <summary>
        /// Outputs the given data if the assertion fails.
        /// </summary>
        [DomName("assert")]
        public void Assert(Boolean condition = false, params Object[] objs)
        {
            if (!condition)
            {
                var data = new Object[(objs?.Length ?? 0) + 1];
                data[0] = "Assertion failed";
                objs?.CopyTo(data, 1);
                Log(data);
            }
        }

        /// <summary>
        /// Starts a new group, which is not indented by the logger.
        /// </summary>
        [DomName("group")]
        public void Group(params Object[] objs) => LogIfAny(objs);

        /// <summary>
        /// Starts a new collapsed group, which is not indented by the logger.
        /// </summary>
        [DomName("groupCollapsed")]
        public void GroupCollapsed(params Object[] objs) => LogIfAny(objs);

        /// <summary>
        /// Ends the current group.
        /// </summary>
        [DomName("groupEnd")]
        public void GroupEnd()
        {
        }

        /// <summary>
        /// Clears the console, which the logger is unable to do.
        /// </summary>
        [DomName("clear")]
        public void Clear()
        {
        }

        /// <summary>
        /// Counts the number of calls with the given label and outputs it.
        /// </summary>
        [DomName("count")]
        public void Count(String label = DefaultLabel)
        {
            _counters.TryGetValue(label, out var count);
            _counters[label] = ++count;
            Log(label + ": " + count.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Resets the counter with the given label.
        /// </summary>
        [DomName("countReset")]
        public void CountReset(String label = DefaultLabel)
        {
            _counters.Remove(label);
        }

        /// <summary>
        /// Starts a timer with the given label.
        /// </summary>
        [DomName("time")]
        public void Time(String label = DefaultLabel)
        {
            if (!_timers.ContainsKey(label))
            {
                _timers[label] = Stopwatch.StartNew();
            }
        }

        /// <summary>
        /// Outputs the elapsed time of the timer with the given label.
        /// </summary>
        [DomName("timeLog")]
        public void TimeLog(String label = DefaultLabel, params Object[] objs)
        {
            if (_timers.TryGetValue(label, out var timer))
            {
                var data = new Object[(objs?.Length ?? 0) + 1];
                data[0] = FormatElapsed(label, timer);
                objs?.CopyTo(data, 1);
                Log(data);
            }
        }

        /// <summary>
        /// Outputs the elapsed time of the timer with the given label and stops it.
        /// </summary>
        [DomName("timeEnd")]
        public void TimeEnd(String label = DefaultLabel)
        {
            if (_timers.TryGetValue(label, out var timer))
            {
                _timers.Remove(label);
                Log(FormatElapsed(label, timer));
            }
        }

        private void LogIfAny(Object[] objs)
        {
            if (objs?.Length > 0)
            {
                Log(objs);
            }
        }

        private static String FormatElapsed(String label, Stopwatch timer) =>
            label + ": " + timer.Elapsed.TotalMilliseconds.ToString("0.###", CultureInfo.InvariantCulture) + " ms";
    }
}
