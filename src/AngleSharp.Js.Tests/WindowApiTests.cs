namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Js.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The members of the window that real pages rely on - Honeybadger, e.g., reads
    /// console.info.__hb_original, so a single missing console method stopped dev.to's base
    /// bundle, and every script relying on its globals failed afterwards.
    /// </summary>
    [TestFixture]
    public class WindowApiTests
    {
        [TestCase("error")]
        [TestCase("warn")]
        [TestCase("info")]
        [TestCase("debug")]
        [TestCase("trace")]
        [TestCase("dir")]
        [TestCase("table")]
        [TestCase("group")]
        [TestCase("groupEnd")]
        [TestCase("assert")]
        [TestCase("count")]
        [TestCase("time")]
        [TestCase("timeEnd")]
        public async Task ConsoleHasMethod(String name)
        {
            var result = await $"typeof console.{name}".EvalScriptAsync();
            Assert.AreEqual("function", result);
        }

        [Test]
        public async Task ConsoleMethodsWriteToTheLogger()
        {
            var console = new ConsoleLogger();
            var cfg = Configuration.Default.WithJs().WithEventLoop().WithConsoleLogger(context => console);
            var html = "<!doctype html><script>console.error('e'); console.warn('w'); console.assert(true, 'hidden'); console.assert(false, 'shown'); console.count(); console.count();</script>";
            await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var output = console.Content.ToString();

            StringAssert.Contains("e", output);
            StringAssert.Contains("w", output);
            StringAssert.Contains("shown", output);
            StringAssert.DoesNotContain("hidden", output);
            StringAssert.Contains("default: 2", output);
        }

        [TestCase("var declared = 1; String(window.declared)", "1")]
        [TestCase("implicitGlobal = 2; String(window.implicitGlobal)", "2")]
        [TestCase("function declaredFunction() {} typeof window.declaredFunction", "function")]
        [TestCase("String(window.Promise === Promise)", "true")]
        [TestCase("String(window.JSON === JSON)", "true")]
        [TestCase("String('Math' in window)", "true")]
        [TestCase("var item = 3; String(window.item)", "3")]
        [TestCase("window.assigned = 4; delete window.assigned; typeof assigned", "undefined")]
        public async Task WindowSeesTheGlobalsOfScripts(String script, String expected)
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><p>x</p>"));

            Assert.AreEqual(expected, document.ExecuteScript(script)?.ToString());
        }

        [TestCase("document.querySelectorAll('p').item(1).id", "b")]
        [TestCase("document.getElementsByTagName('p').item(0).id", "a")]
        [TestCase("document.body.children.item(1).id", "b")]
        [TestCase("document.body.children.namedItem('b').id", "b")]
        [TestCase("document.querySelector('p').attributes.item(0).name", "id")]
        [TestCase("typeof NodeList.prototype.item", "function")]
        public async Task NamedIndexersAreMethods(String script, String expected)
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><body><p id=a></p><p id=b></p></body>"));

            Assert.AreEqual(expected, document.ExecuteScript(script)?.ToString());
        }

        [Test]
        public async Task ErrorOfAMemberKeepsItsMessage()
        {
            var result = await "(function () { try { document.querySelector('!'); } catch (e) { return e.message; } })()".EvalScriptAsync();
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task PerformanceNowIsMonotonic()
        {
            var result = await "(function () { var a = performance.now(); var b = performance.now(); return (a >= 0 && b >= a && typeof performance.timeOrigin === 'number'); })()".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task AnimationFrameCallbackReceivesTheWindowAndATime()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><script>requestAnimationFrame(function (t) { document.title = (this === window) + ',' + typeof t; });</script>"));

            for (var i = 0; i < 100 && document.Title.Length == 0; i++)
            {
                await Task.Delay(20);
            }

            Assert.AreEqual("true,number", document.Title);
        }

        [Test]
        public async Task CancelledAnimationFrameDoesNotRun()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><script>var h = requestAnimationFrame(function () { document.title = 'ran'; }); cancelAnimationFrame(h); setTimeout(function () { document.title += 'done'; }, 50);</script>"));

            for (var i = 0; i < 100 && document.Title.Length == 0; i++)
            {
                await Task.Delay(20);
            }

            Assert.AreEqual("done", document.Title);
        }

        [Test]
        public async Task QueuedMicrotaskRunsAfterTheScript()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><script>document.title = 'a'; queueMicrotask(function () { document.title += 'c'; }); document.title += 'b';</script>"));

            for (var i = 0; i < 100 && document.Title != "abc"; i++)
            {
                await Task.Delay(20);
            }

            Assert.AreEqual("abc", document.Title);
        }
    }
}
