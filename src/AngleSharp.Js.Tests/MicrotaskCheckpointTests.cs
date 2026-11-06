namespace AngleSharp.Js.Tests
{
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The promise jobs of a callback of the host run when no script is running anymore (HTML 8.1.4.4).
    /// </summary>
    [TestFixture]
    public class MicrotaskCheckpointTests
    {
        private static async Task<IDocument> Load(String script)
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop();
            return await BrowsingContext.New(cfg).OpenAsync(m => m.Content($"<!doctype html><title>start</title><body><div id=d></div><script>{script}</script>"));
        }

        private static async Task<String> Run(String script, String expected)
        {
            var document = await Load(script);

            for (var i = 0; i < 300 && document.Title != expected; i++)
            {
                await Task.Delay(10);
            }

            return document.Title;
        }

        [TestCase("new Promise(function (s) { setTimeout(s, 5); }).then(function () { document.title = 'ok'; })")]
        [TestCase("new Promise(function (s) { setTimeout(function () { s(); }, 5); }).then(function () { document.title = 'ok'; })")]
        [TestCase("(async function () { await new Promise(function (s) { setTimeout(s, 5); }); document.title = 'ok'; })()")]
        [TestCase("(async function () { await null; await new Promise(function (s) { setTimeout(s, 5); }); await new Promise(function (s) { setTimeout(s, 5); }); document.title = 'ok'; })()")]
        [TestCase("setTimeout(async function () { await new Promise(function (s) { setTimeout(s, 5); }); document.title = 'ok'; }, 1)")]
        [TestCase("var i = setInterval(function () { clearInterval(i); Promise.resolve().then(function () { document.title = 'ok'; }); }, 5)")]
        [TestCase("requestAnimationFrame && new Promise(function (s) { setTimeout(s, 1); }).then(function () { return new Promise(function (s) { setTimeout(s, 1); }); }).then(function () { document.title = 'ok'; })")]
        [TestCase("new MutationObserver(function () { Promise.resolve().then(function () { document.title = 'ok'; }); }).observe(document.body, { attributes: true }); setTimeout(function () { document.body.setAttribute('a', '1'); }, 1)")]
        public async Task JobsOfCallbacksRun(String script)
        {
            Assert.AreEqual("ok", await Run(script, "ok"));
        }

        [Test]
        public async Task JobsRunBetweenTimers()
        {
            var script = @"var log = [];
                setTimeout(function () { log.push('a'); Promise.resolve().then(function () { log.push('b'); }); }, 1);
                setTimeout(function () { log.push('c'); document.title = log.join(''); }, 30);";

            Assert.AreEqual("abc", await Run(script, "abc"));
        }

        [Test]
        public async Task DispatchByScriptKeepsJobsWaiting()
        {
            var script = @"var log = [], d = document.getElementById('d');
                d.addEventListener('x', function () { Promise.resolve().then(function () { log.push('job'); }); });
                d.dispatchEvent(new Event('x'));
                log.push('after');
                document.title = log.join();
                setTimeout(function () { document.title = log.join(); }, 5);";

            Assert.AreEqual("after,job", await Run(script, "after,job"));
        }

        [Test]
        public async Task JobsRunBetweenListenersOfADispatchByTheHost()
        {
            var document = await Load(@"var log = window.log = [], d = document.getElementById('d');
                d.addEventListener('x', function () { log.push('1'); Promise.resolve().then(function () { log.push('job'); }); });
                d.addEventListener('x', function () { log.push('2'); document.title = log.join(); });");

            await document.Context.GetService<IEventLoop>().EnqueueAsync(_ =>
            {
                document.GetElementById("d").Dispatch(new AngleSharp.Dom.Events.Event("x"));
                return true;
            }, TaskPriority.Normal);

            Assert.AreEqual("1,job,2", document.Title);
        }

        [Test]
        public async Task AThrowingJobDoesNotStopTheOthers()
        {
            var script = @"setTimeout(function () {
                    queueMicrotask(function () { throw new Error('job'); });
                    queueMicrotask(function () { document.title = 'ok'; });
                }, 1);";

            Assert.AreEqual("ok", await Run(script, "ok"));
        }
    }
}
