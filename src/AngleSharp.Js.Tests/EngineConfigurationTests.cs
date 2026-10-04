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
                .WithJs(new JsScriptingOptions { EngineCreator = options => new Engine(options.MaxStatements(64)) })
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
        public async Task EngineCreatorIsCopiedBeforeAnEngineIsCreated()
        {
            var options = new JsScriptingOptions { EngineCreator = engineOptions => new Engine(engineOptions.MaxStatements(64)) };
            var config = Configuration.Default.WithJs(options).WithEventLoop();
            options.EngineCreator = engineOptions => new Engine(engineOptions);

            using (var context = BrowsingContext.New(config))
            {
                var document = await context.OpenNewAsync().ConfigureAwait(false);
                Assert.Throws<StatementsCountOverflowException>(() => document.ExecuteScript(
                    "for (var i = 0; i < 1000; i++) {}"));
            }
        }

        [Test]
        public async Task CreatorRunsOncePerWindowAndRetainsDomWrapping()
        {
            var calls = 0;
            var config = Configuration.Default.WithJs(new JsScriptingOptions
            {
                EngineCreator = options =>
                {
                    calls++;
                    return new Engine(options.MaxStatements(1000));
                },
            }).WithEventLoop();

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
            var config = Configuration.Default.WithJs(new JsScriptingOptions
            {
                EngineCreator = options =>
                {
                    created = new Engine(options.Configure(engine => engine.SetValue("hostValue", "configured")));
                    return created;
                },
            }).WithEventLoop();

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
