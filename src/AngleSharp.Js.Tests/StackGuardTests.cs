namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using Jint.Runtime;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    public class StackGuardTests
    {
        //  A recursion that never ends. Jint implements no tail calls, so this really does
        //  grow the call stack. Without a stack guard it exhausts the native one, and the
        //  resulting StackOverflowException takes the whole process down - which is why the
        //  tests below cannot assert anything weaker than "we got here at all".
        private const String RunawayRecursion = "function boom() { return boom(); } boom();";

        //  Deeper than a 1 MB stack holds, so the engine has to keep going on a fresh one
        //  instead of reporting the depth as an error.
        private const String DeepRecursion = "function depth(n) { return n === 0 ? 0 : 1 + depth(n - 1); } depth(1500);";

        [Test]
        public async Task RunawayRecursionInPageScriptDoesNotEscapeOpenAsync()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop();

            var content = $"<!doctype html><script>{RunawayRecursion}</script>";
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(content));

            Assert.IsNotNull(document);
        }

        [Test]
        public async Task RunawayRecursionReportsMaximumCallStackSizeExceeded()
        {
            //  A small limit keeps the failure quick and independent of how much native
            //  stack the test runner happens to have left.
            var config = Configuration.Default
                .WithJs(new JsScriptingOptions { MaxCallStackDepth = 200 })
                .WithEventLoop();

            var document = await BrowsingContext.New(config).OpenNewAsync();
            var error = Assert.Throws<JavaScriptException>(() => document.ExecuteScript(RunawayRecursion));

            Assert.AreEqual("Maximum call stack size exceeded", error.Message);
        }

        [Test]
        public async Task DeepButFiniteRecursionStillSucceeds()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop();

            var document = await BrowsingContext.New(config).OpenNewAsync();
            var result = document.ExecuteScript(DeepRecursion);

            Assert.AreEqual(1500.0, result);
        }

        [Test]
        public async Task EditingTheOptionsAfterwardsLeavesTheServiceAlone()
        {
            var options = new JsScriptingOptions();
            var config = Configuration.Default
                .WithJs(options)
                .WithEventLoop();

            //  The engine is only built once a document asks for it, so an edit landing
            //  in between must not be the one deciding how that document behaves.
            options.MaxCallStackDepth = 200;

            var document = await BrowsingContext.New(config).OpenNewAsync();
            var result = document.ExecuteScript(DeepRecursion);

            Assert.AreEqual(1500.0, result);
        }
    }
}
