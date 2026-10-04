namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Runtime;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    public class EngineConfigurationTests
    {
        [Test]
        public async Task ConstraintsAreInstalledBeforeInlineScriptsRun()
        {
            var config = Configuration.Default
                .With(new EngineCreator(options => new Engine(options.MaxStatements(64))))
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
                return options => new Engine(options.MaxStatements(limit));
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
            var config = Configuration.Default.With(new EngineCreator(options =>
            {
                calls++;
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
            }
        }

        [Test]
        public async Task CreatorReturnsTheEngineUsedForInlineScriptsAndDomBindings()
        {
            Engine created = null;
            var config = Configuration.Default.With(new EngineCreator(options =>
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
    }
}
