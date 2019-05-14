namespace AngleSharp.Js.Tests
{
    using AngleSharp.Browser;
    using AngleSharp.Common;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The default event loop.
    /// </summary>
    sealed class MockEventLoop : IEventLoop
    {
        private readonly Dictionary<TaskPriority, Queue<TaskEventLoopEntry>> _queues;
        private Int32 _count;
        private TaskEventLoopEntry _current;

        public MockEventLoop(IBrowsingContext context)
        {
            _queues = new Dictionary<TaskPriority, Queue<TaskEventLoopEntry>>();
            _count = 0;
            _current = null;
            //Scan();
        }

        async Task Scan()
        {
            while (true)
            {
                var c = _current;
                Debug.WriteLine("Id: {0}, Running: {1}, Completed: {2}", c?.Id, c?.IsRunning, c?.IsCompleted);
                Debug.WriteLine("#{0}", _count);
                await Task.Delay(100);
            }
        }

        private readonly Object _lockObj = new object();

        public ICancellable Enqueue(Action<CancellationToken> task, TaskPriority priority)
        {
            var entry = new TaskEventLoopEntry(task);

            lock (_lockObj)
            {
                Debug.WriteLine("Enter lock 1");
                if (!_queues.TryGetValue(priority, out var entries))
                {
                    entries = new Queue<TaskEventLoopEntry>();
                    _queues.Add(priority, entries);
                }

                _count++;

                if (_current == null)
                {
                    SetCurrent(entry);
                }
                else
                {
                    entries.Enqueue(entry);
                }
                Debug.WriteLine("Exit lock 1");
            }

            return entry;
        }

        public void Spin()
        {
            lock (_lockObj)
            {
                Debug.WriteLine("Enter lock 2");
                SpinInternal();
                Debug.WriteLine("Exit lock 2");
            }
        }

        private void SpinInternal()
        {
            var completed = _current?.IsCompleted ?? true;

            if (completed)
            {
                SetCurrent(
                    Dequeue(TaskPriority.Critical) ??
                    Dequeue(TaskPriority.Microtask) ??
                    Dequeue(TaskPriority.Normal) ??
                    Dequeue(TaskPriority.None));
            }
        }

        public void CancelAll()
        {
            lock (_lockObj)
            {
                foreach (var queue in _queues)
                {
                    var entries = queue.Value;

                    while (entries.Count > 0)
                    {
                        entries.Dequeue().Cancel();
                    }
                }

                _queues.Clear();
                _current?.Cancel();
            }
        }

        private void SetCurrent(TaskEventLoopEntry entry)
        {
            _current = entry;
            entry?.Run(() =>
            {
                _count--;

                lock (_lockObj)
                {
                    Debug.WriteLine("Enter lock 4");
                    _current = null;
                    SpinInternal();
                    Debug.WriteLine("Exit lock 4");
                }
            });
        }

        private TaskEventLoopEntry Dequeue(TaskPriority priority)
        {
            if (_queues.ContainsKey(priority) && _queues[priority].Count != 0)
            {
                return _queues[priority].Dequeue();
            }

            return null;
        }

        private sealed class TaskEventLoopEntry : ICancellable
        {
            private readonly CancellationTokenSource _cts;
            private readonly Action<CancellationToken> _action;
            private readonly Guid _id = Guid.NewGuid();
            private Task _task;

            public TaskEventLoopEntry(Action<CancellationToken> action)
            {
                _cts = new CancellationTokenSource();
                _action = action;
            }

            public Guid Id => _id;

            public Boolean IsCompleted => _task != null && _task.IsCompleted;

            public Boolean IsRunning => _task != null && (
                _task.Status == TaskStatus.Running ||
                _task.Status == TaskStatus.WaitingForActivation ||
                _task.Status == TaskStatus.WaitingToRun ||
                _task.Status == TaskStatus.WaitingForChildrenToComplete);

            public void Run(Action callback)
            {
                if (_task == null)
                {
                    _task = Task.Run(() => _action.Invoke(_cts.Token), _cts.Token);
                    _task.ContinueWith(_ => callback.Invoke());
                }
            }

            public void Cancel() => _cts.Cancel();
        }
    }
}
