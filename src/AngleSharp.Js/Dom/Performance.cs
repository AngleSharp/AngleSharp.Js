namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents the high resolution time of a window (performance.now()), measured from the
    /// moment scripting started for its document, and the marks and measures of its scripts.
    /// </summary>
    [DomName("Performance")]
    public sealed class Performance
    {
        private readonly Stopwatch _clock = Stopwatch.StartNew();
        private readonly List<PerformanceEntry> _entries = new List<PerformanceEntry>();
        private readonly List<PerformanceObserver> _observers = new List<PerformanceObserver>();
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

        /// <summary>
        /// Adds a named point in time to the timeline.
        /// </summary>
        [DomName("mark")]
        public PerformanceMark Mark(String name, Object options = null)
        {
            RequireName(name);
            var start = Now();
            Object detail = null;

            if (options is ObjectInstance values)
            {
                start = ToNumber(values.Get("startTime")) ?? start;
                detail = ToDetail(values);
            }

            var mark = new PerformanceMark(name, start, detail);
            Add(mark);
            return mark;
        }

        /// <summary>
        /// Adds a named span of time to the timeline, given by marks, times or options.
        /// </summary>
        [DomName("measure")]
        public PerformanceMeasure Measure(String name, Object startOrOptions = null, String endMark = null)
        {
            RequireName(name);
            Double? start = null;
            Double? end = null;
            Object detail = null;

            if (startOrOptions is ObjectInstance options)
            {
                var duration = ToNumber(options.Get("duration"));
                start = ToTime(options.Get("start"));
                end = ToTime(options.Get("end"));
                detail = ToDetail(options);

                if (start.HasValue && end.HasValue && duration.HasValue)
                {
                    throw new JavaScriptException(EngineInstance.Current.Jint.Intrinsics.TypeError, "The start, end and duration cannot be given all together.");
                }

                if (duration.HasValue)
                {
                    end = end ?? start + duration;
                    start = start ?? end - duration;
                }
            }
            else if (startOrOptions != null)
            {
                start = FindMark(startOrOptions.ToString());
            }

            if (endMark != null)
            {
                end = FindMark(endMark);
            }

            var from = start ?? 0.0;
            var measure = new PerformanceMeasure(name, from, (end ?? Now()) - from, detail);
            Add(measure);
            return measure;
        }

        /// <summary>
        /// Gets everything on the timeline, sorted by the start.
        /// </summary>
        [DomName("getEntries")]
        public PerformanceEntry[] GetEntries() => Sorted(_entries);

        /// <summary>
        /// Gets what is on the timeline of the given type, sorted by the start.
        /// </summary>
        [DomName("getEntriesByType")]
        public PerformanceEntry[] GetEntriesByType(String type) => Sorted(_entries.Where(m => m.EntryType == type));

        /// <summary>
        /// Gets what is on the timeline with the given name, and the type if given, sorted by the start.
        /// </summary>
        [DomName("getEntriesByName")]
        public PerformanceEntry[] GetEntriesByName(String name, String type = null) =>
            Sorted(_entries.Where(m => m.Name == name && (type == null || m.EntryType == type)));

        /// <summary>
        /// Removes the marks of the given name, or all of them.
        /// </summary>
        [DomName("clearMarks")]
        public void ClearMarks(String name = null) => Clear(PerformanceMark.Type, name);

        /// <summary>
        /// Removes the measures of the given name, or all of them.
        /// </summary>
        [DomName("clearMeasures")]
        public void ClearMeasures(String name = null) => Clear(PerformanceMeasure.Type, name);

        /// <summary>
        /// Removes the entries of the loaded resources, of which none are kept here.
        /// </summary>
        [DomName("clearResourceTimings")]
        public void ClearResourceTimings()
        {
        }

        /// <summary>
        /// Sets how many entries of loaded resources are kept, which is none here.
        /// </summary>
        /// <param name="maxSize">The number of entries.</param>
        [DomName("setResourceTimingBufferSize")]
        public void SetResourceTimingBufferSize(Int32 maxSize = 0)
        {
        }

        internal void AddObserver(PerformanceObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        internal void RemoveObserver(PerformanceObserver observer) => _observers.Remove(observer);

        private void Add(PerformanceEntry entry)
        {
            _entries.Add(entry);

            foreach (var observer in _observers.ToArray())
            {
                observer.Notify(entry);
            }
        }

        private void Clear(String type, String name) =>
            _entries.RemoveAll(m => m.EntryType == type && (name == null || m.Name == name));

        private static PerformanceEntry[] Sorted(IEnumerable<PerformanceEntry> entries) =>
            entries.OrderBy(m => m.StartTime).ToArray();

        // the latest of the name, as the same name can be used over and over
        private Double FindMark(String name)
        {
            var mark = _entries.LastOrDefault(m => m.EntryType == PerformanceMark.Type && m.Name == name);
            return mark?.StartTime ?? throw new DomException(DomError.Syntax);
        }

        private static void RequireName(String name)
        {
            if (name == null)
            {
                throw new JavaScriptException(EngineInstance.Current.Jint.Intrinsics.TypeError, "A name is required.");
            }
        }

        private Double? ToTime(JsValue value) => value.IsString() ? FindMark(value.AsString()) : ToNumber(value);

        private static Double? ToNumber(JsValue value) => value.IsNumber() ? value.AsNumber() : (Double?)null;

        private static Object ToDetail(ObjectInstance options)
        {
            var detail = options.Get("detail");
            return detail.IsUndefined() || detail.IsNull() ? null : StructuredClone.Clone(detail);
        }
    }

    /// <summary>
    /// Represents something on the timeline of a window.
    /// </summary>
    [DomName("PerformanceEntry")]
    public abstract class PerformanceEntry
    {
        internal PerformanceEntry(String name, String entryType, Double startTime, Double duration)
        {
            Name = name;
            EntryType = entryType;
            StartTime = startTime;
            Duration = duration;
        }

        /// <summary>
        /// Gets the name of the entry.
        /// </summary>
        [DomName("name")]
        public String Name { get; }

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        [DomName("entryType")]
        public String EntryType { get; }

        /// <summary>
        /// Gets the start in milliseconds after the time origin.
        /// </summary>
        [DomName("startTime")]
        public Double StartTime { get; }

        /// <summary>
        /// Gets the length in milliseconds.
        /// </summary>
        [DomName("duration")]
        public Double Duration { get; }

        /// <summary>
        /// Gets the plain object for the entry.
        /// </summary>
        [DomName("toJSON")]
        public Object ToJson()
        {
            var jint = EngineInstance.Current.Jint;
            var json = new JsObject(jint);
            json.Set("name", Name);
            json.Set("entryType", EntryType);
            json.Set("startTime", StartTime);
            json.Set("duration", Duration);
            return json;
        }
    }

    /// <summary>
    /// Represents a named point in time.
    /// </summary>
    [DomName("PerformanceMark")]
    public sealed class PerformanceMark : PerformanceEntry
    {
        internal const String Type = "mark";

        internal PerformanceMark(String name, Double startTime, Object detail)
            : base(name, Type, startTime, 0.0)
        {
            Detail = detail;
        }

        /// <summary>
        /// Gets what the script gave along with the mark.
        /// </summary>
        [DomName("detail")]
        public Object Detail { get; }
    }

    /// <summary>
    /// Represents a named span of time.
    /// </summary>
    [DomName("PerformanceMeasure")]
    public sealed class PerformanceMeasure : PerformanceEntry
    {
        internal const String Type = "measure";

        internal PerformanceMeasure(String name, Double startTime, Double duration, Object detail)
            : base(name, Type, startTime, duration)
        {
            Detail = detail;
        }

        /// <summary>
        /// Gets what the script gave along with the measure.
        /// </summary>
        [DomName("detail")]
        public Object Detail { get; }
    }
}
