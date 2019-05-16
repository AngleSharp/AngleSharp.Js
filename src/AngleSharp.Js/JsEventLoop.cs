namespace AngleSharp.Js
{
    using AngleSharp.Browser;
    using AngleSharp.Common;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// A thread-based event loop implementation.
    /// </summary>
    public sealed class JsEventLoop : IEventLoop, IDisposable
    {
        private readonly Dictionary<TaskPriority, Queue<LoopEntry>> _queues = new Dictionary<TaskPriority, Queue<LoopEntry>>();
        private readonly Object _lockObj = new Object();
        private CancellationTokenSource _cts;

        ICancellable IEventLoop.Enqueue(Action<CancellationToken> action, TaskPriority priority)
        {
            var entry = new LoopEntry(action);

            lock (_lockObj)
            {
                if (!_queues.TryGetValue(priority, out var entries))
                {
                    entries = new Queue<LoopEntry>();
                    _queues.Add(priority, entries);
                }

                entries.Enqueue(entry);
            }

            return entry;
        }

        private LoopEntry Dequeue()
        {
            LoopEntry Dequeue(TaskPriority priority)
            {
                if (_queues.ContainsKey(priority) && _queues[priority].Count != 0)
                {
                    return _queues[priority].Dequeue();
                }

                return null;
            }

            var current = default(LoopEntry);

            lock (_lockObj)
            {
                current =
                    Dequeue(TaskPriority.Critical) ??
                    Dequeue(TaskPriority.Microtask) ??
                    Dequeue(TaskPriority.Normal) ??
                    Dequeue(TaskPriority.None);
            }

            return current;
        }

        void IEventLoop.Spin()
        {
            if (_cts == null)
            {
                var thread = new Thread(Runner)
                {
                    IsBackground = true,
                    Name = "AngleSharpEventLoop",
                    Priority = ThreadPriority.Highest,
                };
                _cts = new CancellationTokenSource();
                thread.Start(_cts.Token);
            }
        }

        void IEventLoop.CancelAll()
        {
            lock (_lockObj)
            {
                _queues.Clear();
                _cts?.Cancel();
            }
        }

        private void Runner(Object state)
        {
            if (state is CancellationToken token)
            {
                while (!token.IsCancellationRequested)
                {
                    var current = Dequeue();

                    if (current == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    using (token.Register(current.Cancel))
                    {
                        current.Run();
                    }
                }
            }
        }

        void IDisposable.Dispose() => _cts?.Cancel();

        private class LoopEntry : ICancellable
        {
            private readonly CancellationTokenSource cts = new CancellationTokenSource();
            private readonly Action<CancellationToken> _action;

            public LoopEntry(Action<CancellationToken> action)
            {
                _action = action;
            }

            public Boolean IsCompleted { get; set; } = false;

            public Boolean IsRunning { get; set; } = false;

            public void Run()
            {
                IsCompleted = false;
                IsRunning = true;

                try { _action.Invoke(cts.Token); }
                catch { }

                IsRunning = false;
                IsCompleted = true;
            }

            public void Cancel() => cts.Cancel();
        }
    }
}
