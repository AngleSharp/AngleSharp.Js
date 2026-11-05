namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The callback receiving what a performance observer found.
    /// </summary>
    /// <param name="entries">The new entries.</param>
    /// <param name="observer">The observer.</param>
    [DomName("PerformanceObserverCallback")]
    public delegate void PerformanceObserverCallback(PerformanceObserverEntryList entries, PerformanceObserver observer);

    /// <summary>
    /// Represents the options of observing.
    /// </summary>
    [DomName("PerformanceObserverInit")]
    public sealed class PerformanceObserverOptions
    {
        /// <summary>
        /// Gets or sets the types of entries to observe.
        /// </summary>
        [DomName("entryTypes")]
        public String[] EntryTypes { get; set; }

        /// <summary>
        /// Gets or sets the single type of entries to observe.
        /// </summary>
        [DomName("type")]
        public String Type { get; set; }

        /// <summary>
        /// Gets or sets if the entries existing already of that type are wanted as well.
        /// </summary>
        [DomName("buffered")]
        public Boolean Buffered { get; set; }
    }

    /// <summary>
    /// Represents the new entries handed to the callback of a performance observer.
    /// </summary>
    [DomName("PerformanceObserverEntryList")]
    public sealed class PerformanceObserverEntryList
    {
        private readonly PerformanceEntry[] _entries;

        internal PerformanceObserverEntryList(IEnumerable<PerformanceEntry> entries)
        {
            _entries = entries.OrderBy(m => m.StartTime).ToArray();
        }

        /// <summary>
        /// Gets all of the entries.
        /// </summary>
        [DomName("getEntries")]
        public PerformanceEntry[] GetEntries() => _entries.ToArray();

        /// <summary>
        /// Gets the entries of the given type.
        /// </summary>
        [DomName("getEntriesByType")]
        public PerformanceEntry[] GetEntriesByType(String type) => _entries.Where(m => m.EntryType == type).ToArray();

        /// <summary>
        /// Gets the entries with the given name, and the type if given.
        /// </summary>
        [DomName("getEntriesByName")]
        public PerformanceEntry[] GetEntriesByName(String name, String type = null) =>
            _entries.Where(m => m.Name == name && (type == null || m.EntryType == type)).ToArray();
    }

    /// <summary>
    /// Represents the PerformanceObserver of the standard. Only the entries of the performance object
    /// are known: asking for other types (e.g. paint) is fine, as with any type a browser doesn't
    /// have, and just doesn't find anything.
    /// </summary>
    [DomName("PerformanceObserver")]
    [DomExposed("Window")]
    public sealed class PerformanceObserver
    {
        private readonly IWindow _window;
        private readonly PerformanceObserverCallback _callback;
        private readonly HashSet<String> _types = new HashSet<String>(StringComparer.Ordinal);
        private readonly List<PerformanceEntry> _queue = new List<PerformanceEntry>();
        private Boolean _isScheduled;

        /// <summary>
        /// Creates a new observer.
        /// </summary>
        /// <param name="window">The window of the observer.</param>
        /// <param name="callback">The callback receiving the entries.</param>
        [DomConstructor]
        public PerformanceObserver(IWindow window, PerformanceObserverCallback callback)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        /// <summary>
        /// Starts observing the given types of entries.
        /// </summary>
        /// <param name="options">What to observe.</param>
        [DomName("observe")]
        public void Observe(PerformanceObserverOptions options)
        {
            var performance = _window.Performance();

            if (options?.EntryTypes != null)
            {
                _types.Clear();
                _types.UnionWith(options.EntryTypes);
            }
            else if (options?.Type != null)
            {
                _types.Add(options.Type);

                if (options.Buffered)
                {
                    foreach (var entry in performance.GetEntriesByType(options.Type))
                    {
                        Notify(entry);
                    }
                }
            }

            performance.AddObserver(this);
        }

        /// <summary>
        /// Stops observing.
        /// </summary>
        [DomName("disconnect")]
        public void Disconnect()
        {
            _window.Performance().RemoveObserver(this);
            _types.Clear();
            _queue.Clear();
        }

        /// <summary>
        /// Gets the entries the callback has not been told about yet, which it won't be anymore.
        /// </summary>
        [DomName("takeRecords")]
        public PerformanceEntry[] TakeRecords()
        {
            var records = _queue.ToArray();
            _queue.Clear();
            return records;
        }

        internal void Notify(PerformanceEntry entry)
        {
            if (_types.Contains(entry.EntryType))
            {
                _queue.Add(entry);

                if (!_isScheduled)
                {
                    _isScheduled = true;
                    var loop = _window.Document.Context.GetService<IEventLoop>();

                    if (loop != null)
                    {
                        loop.Enqueue(_ => Deliver(), TaskPriority.Normal);
                    }
                    else
                    {
                        Deliver();
                    }
                }
            }
        }

        private void Deliver()
        {
            _isScheduled = false;

            if (_queue.Count > 0)
            {
                _callback.Invoke(new PerformanceObserverEntryList(TakeRecords()), this);
            }
        }
    }
}
