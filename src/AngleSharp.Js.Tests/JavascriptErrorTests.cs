using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Browser;
using AngleSharp.Common;
using AngleSharp.Html;
using NUnit.Framework;

namespace AngleSharp.Js.Tests
{
    [TestFixture]
    public class JavascriptErrorTests
    {
        [Test]
        public void JavascriptErrorInListenerShouldNotThrowJavascriptException()
        {
            var config = Configuration.Default
                .WithJs()
                .WithOnly(ctx => new CustomEventLoop());

            var context = BrowsingContext.New(config);

            const string content = @"
                <html>
                    <body>
                        <script type='text/javascript'>
                            document.addEventListener('load', function() {
                                undefinedVariable.invalidMethod();
                            });
                        </script>
                        
                    </body>
                </html>
            ";

            Assert.DoesNotThrowAsync(async () =>
            {
                try
                {
                    await context.OpenAsync(r => r.Content(content));

                    var loop = (CustomEventLoop) context.GetService<IEventLoop>();
                    await Task.WhenAll(loop.Tasks);
                }
                catch (TargetInvocationException ex)
                {
                    // unmask exception
                    var innerAggregate = ex.InnerException as AggregateException;
                    if (innerAggregate != null)
                        throw innerAggregate.InnerException;
                    throw;
                }
            });
        }

        class CustomEventLoop : IEventLoop
        {
            private readonly List<Task> tasks = new List<Task>();
            public ICancellable Enqueue(Action<CancellationToken> action, TaskPriority priority)
            {
                var task = new Task(() => action(CancellationToken.None));
                tasks.Add(task);
                task.Start();
                return new TaskCancellable(task);
            }

            public Task[] Tasks => tasks.ToArray();
            public void Spin() { }
            public void CancelAll() { }
            
            class TaskCancellable : ICancellable
            {
                private readonly Task task;
                public TaskCancellable(Task task) { this.task = task; }
                public void Cancel() { }
                public bool IsCompleted => task.IsCompleted;
                public bool IsRunning => task.Status == TaskStatus.Running;
            }
        }
    }
}
