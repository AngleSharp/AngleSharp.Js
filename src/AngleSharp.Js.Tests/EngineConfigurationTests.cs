namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
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
                .WithJs(new JsScriptingOptions { ConfigureEngine = options => options.MaxStatements(64) })
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
        public async Task ConfigurationCallbackIsCopiedBeforeAnEngineIsCreated()
        {
            var options = new JsScriptingOptions { ConfigureEngine = engine => engine.MaxStatements(64) };
            var config = Configuration.Default.WithJs(options).WithEventLoop();
            options.ConfigureEngine = null;

            using (var context = BrowsingContext.New(config))
            {
                var document = await context.OpenNewAsync().ConfigureAwait(false);
                Assert.Throws<StatementsCountOverflowException>(() => document.ExecuteScript(
                    "for (var i = 0; i < 1000; i++) {}"));
            }
        }

        [Test]
        public async Task CallbackRunsOncePerWindowAndRetainsDomWrapping()
        {
            var calls = 0;
            var config = Configuration.Default.WithJs(new JsScriptingOptions
            {
                ConfigureEngine = options =>
                {
                    calls++;
                    options.MaxStatements(1000);
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
    }
}
