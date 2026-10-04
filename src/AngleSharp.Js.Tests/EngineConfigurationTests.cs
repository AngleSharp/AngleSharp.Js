namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Runtime;
    using NUnit.Framework;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class EngineConfigurationTests
    {
        [Test]
        public async Task ConstraintsAreInstalledBeforeInlineScriptsRun()
        {
            var config = Configuration.Default
                .With(new EngineCreator((window, options) => new Engine(options.MaxStatements(64))))
                .WithJs()
                .WithEventLoop();

            using (var context = BrowsingContext.New(config))
            {
                var document = await context.OpenAsync(request => request.Content(
                    "<div id='result'>before</div><script>for (var i = 0; i < 1000; i++) {} " +
                    "document.getElementById('result').textContent = 'after';</script>")).ConfigureAwait(false);

                Assert.AreEqual("before", document.QuerySelector("#result").TextContent);
                Assert.Throws<StatementsCountOverflowException>(() => document.ExecuteScript(
                    "for (var i = 0; i < 1000; i++) {}"));
                Assert.AreEqual("before", document.ExecuteScript("document.getElementById('result').textContent"));
            }
        }

        [Test]
        public async Task CreatorServiceCanBeCreatedForEachBrowsingContext()
        {
            var services = 0;
            Func<IBrowsingContext, EngineCreator> createService = _ =>
            {
                services++;
                var limit = services == 1 ? 64 : 2000;
                return (window, options) => new Engine(options.MaxStatements(limit));
            };
            var config = Configuration.Default.With(createService).WithJs().WithEventLoop();

            using (var first = BrowsingContext.New(config))
            using (var second = BrowsingContext.New(config))
            {
                var firstDocument = await first.OpenNewAsync().ConfigureAwait(false);
                var secondDocument = await second.OpenNewAsync().ConfigureAwait(false);
                Assert.Throws<StatementsCountOverflowException>(() => firstDocument.ExecuteScript(
                    "for (var i = 0; i < 1000; i++) {}"));
                Assert.AreEqual(100, secondDocument.ExecuteScript("var i = 0; for (; i < 100; i++) {} i;"));
                Assert.AreEqual(2, services);
            }
        }

        [Test]
        public async Task CreatorRunsOncePerWindowAndRetainsDomWrapping()
        {
            var calls = 0;
            var windows = new ConcurrentQueue<IWindow>();
            var config = Configuration.Default.With(new EngineCreator((window, options) =>
            {
                calls++;
                windows.Enqueue(window);
                return new Engine(options.MaxStatements(1000));
            })).WithJs().WithEventLoop();

            using (var firstContext = BrowsingContext.New(config))
            using (var secondContext = BrowsingContext.New(config))
            {
                var first = await firstContext.OpenAsync(request => request.Content("<p>first</p>")).ConfigureAwait(false);
                var second = await secondContext.OpenAsync(request => request.Content("<p>second</p>")).ConfigureAwait(false);

                Assert.AreEqual("first", first.ExecuteScript("document.querySelector('p').textContent"));
                Assert.AreEqual("first", first.ExecuteScript("document.querySelector('p').textContent"));
                Assert.AreEqual("second", second.ExecuteScript("document.querySelector('p').textContent"));
                Assert.AreEqual(2, calls);
                CollectionAssert.AreEqual(new[] { first.DefaultView, second.DefaultView }, windows.ToArray());
            }
        }

        [Test]
        public async Task CreatorReturnsTheEngineUsedForInlineScriptsAndDomBindings()
        {
            Engine created = null;
            var config = Configuration.Default.With(new EngineCreator((window, options) =>
            {
                created = new Engine(options.Configure(engine => engine.SetValue("hostValue", "configured")));
                return created;
            })).WithJs().WithEventLoop();

            using (var context = BrowsingContext.New(config))
            {
                var document = await context.OpenAsync(request => request.Content(
                    "<p>before</p><script>document.querySelector('p').textContent = hostValue;</script>")).ConfigureAwait(false);
                Assert.AreEqual("configured", document.QuerySelector("p").TextContent);
                Assert.AreSame(created, context.GetService<JsScriptingService>().GetOrCreateJint(document));
                created.SetValue("hostNode", document.QuerySelector("p"));
                Assert.AreEqual(true, document.ExecuteScript("hostNode === document.querySelector('p')"));
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task WorkersInheritTheEngineCreatorService(Boolean contextFactory)
        {
            var services = 0;
            var windows = new ConcurrentQueue<IWindow>();
            EngineCreator CreateEngine(String hostValue) => (window, options) =>
            {
                windows.Enqueue(window);
                return new Engine(options.MaxStatements(256)
                    .Configure(engine => engine.SetValue("hostValue", hostValue)));
            };
            Object registration = contextFactory
                ? (Object)new Func<IBrowsingContext, EngineCreator>(_ => CreateEngine("context-" + Interlocked.Increment(ref services)))
                : CreateEngine("configured");
            var config = Configuration.Default
                .With(registration)
                .WithJs()
                .WithEventLoop()
                .With(new DelayedRequester(0, "self.__host = hostValue; for (var i = 0; i < 10000; i++) {} self.__after = true;"))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

            using (var context = BrowsingContext.New(config))
            {
                var document = await context.OpenNewAsync().ConfigureAwait(false);
                Assert.AreEqual(contextFactory ? "context-1" : "configured", document.ExecuteScript("hostValue"));
                var worker = new Dom.Worker(document.DefaultView, "https://example.com/worker.js");

                try
                {
                    for (var retries = 200; retries > 0 && !worker.IsInitialized && worker.StartupError is null; retries--)
                    {
                        await Task.Delay(10).ConfigureAwait(false);
                    }

                    Assert.IsInstanceOf<StatementsCountOverflowException>(worker.StartupError);
                    Assert.AreEqual(contextFactory ? "context-2" : "configured", worker.EvaluateInWorker("self.__host"));
                    Assert.AreEqual("undefined", worker.EvaluateInWorker("typeof self.__after"));
                    var createdWindows = windows.ToArray();
                    Assert.AreEqual(2, createdWindows.Length);
                    Assert.AreSame(document.DefaultView, createdWindows[0]);
                    Assert.AreNotSame(document.DefaultView, createdWindows[1]);
                    Assert.AreNotSame(context, createdWindows[1].Document.Context);
                    Assert.AreSame(createdWindows[1], createdWindows[1].Document.DefaultView);
                    if (contextFactory)
                    {
                        Assert.AreEqual(2, services);
                    }
                }
                finally
                {
                    worker.Terminate();
                }
            }
        }
    }
}
