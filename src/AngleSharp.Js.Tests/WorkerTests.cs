namespace AngleSharp.Js.Tests
{
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class WorkerTests
    {
        [Test]
        public async Task WorkerConstructorShouldExistOnWindow()
        {
            var result = await "typeof Worker".EvalScriptAsync().ConfigureAwait(false);
            Assert.AreEqual("function", result);
        }

        [Test]
        public async Task WorkerCanBeConstructedFromClr()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new DelayedRequester(0, "self.__ran = 1; self.onmessage = function (event) { self.postMessage(event.data); };"))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(request => request.Content("<!doctype html><div></div>")).ConfigureAwait(false);
            var worker = default(AngleSharp.Js.Dom.Worker);
            Assert.DoesNotThrow(() => worker = new AngleSharp.Js.Dom.Worker(document.DefaultView, "https://example.com/worker.js"));

            var retries = 200;

            while (retries-- > 0 && !worker.IsInitialized && worker.StartupError == null)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            Assert.IsNull(worker.StartupError);
            Assert.IsTrue(worker.IsInitialized);
            Assert.AreEqual("1", worker.EvaluateInWorker("String(self.__ran)")?.ToString());
            Assert.AreEqual("object", worker.EvaluateInWorker("typeof self")?.ToString());
            Assert.AreEqual("function", worker.EvaluateInWorker("typeof self.postMessage")?.ToString());
            Assert.AreEqual("function", worker.EvaluateInWorker("typeof self.onmessage")?.ToString());

            String fromWorker = null;
            worker.AddEventListener("message", (sender, ev) => fromWorker = (ev as AngleSharp.Dom.Events.MessageEvent)?.Data?.ToString(), false);
            var callResult = worker.EvaluateInWorker("try { self.postMessage('ping'); 'ok'; } catch (e) { 'err:' + e; }")?.ToString();
            Assert.AreEqual("ok", callResult);

            retries = 200;

            while (retries-- > 0 && fromWorker == null)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            Assert.AreEqual("ping", fromWorker);
        }

        [Test]
        public async Task WorkerShouldExchangeMessagesWithMainThread()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new DelayedRequester(0, "self.onmessage = function (event) { self.postMessage(event.data + ' world'); };"))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=result></div><script>var stage = 'new'; try { var worker = new Worker('https://example.com/worker.js'); stage = 'handler'; worker.onmessage = function(e) { document.querySelector('#result').textContent = e && e.data ? e.data : 'local'; }; worker.dispatchEvent(new Event('message')); stage = 'send'; worker.postMessage('hello'); stage = 'sent'; if (document.querySelector('#result').textContent !== 'local') { document.querySelector('#result').textContent = 'no-local'; } } catch (e) { document.querySelector('#result').textContent = 'ERR:' + stage + ':' + e; }</script>";
            var document = await context.OpenAsync(request => request.Content(html)).ConfigureAwait(false);

            await WaitForResultAsync(document, "hello world").ConfigureAwait(false);
            var result = document.QuerySelector("#result")?.TextContent;
            Assert.AreEqual("hello world", result);
        }

        [Test]
        public async Task WorkerShouldRequireResourceLoading()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop();
            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=result></div><script>var ok = false; try { new Worker('/worker.js'); } catch (e) { ok = true; } document.querySelector('#result').textContent = ok ? 'true' : 'false';</script>";
            var document = await context.OpenAsync(request => request.Content(html)).ConfigureAwait(false);
            var result = document.QuerySelector("#result")?.TextContent;
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task WorkerDirectPostToOwnerShouldRaiseMessage()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new DelayedRequester(0, ""))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(request => request.Content("<!doctype html><div></div>")).ConfigureAwait(false);
            var worker = new AngleSharp.Js.Dom.Worker(document.DefaultView, "https://example.com/worker.js");

            String result = null;
            worker.AddEventListener("message", (sender, ev) => result = (ev as AngleSharp.Dom.Events.MessageEvent)?.Data?.ToString(), false);
            worker.PostMessageToOwner("boot");

            var retries = 50;

            while (retries-- > 0 && result == null)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            Assert.AreEqual("boot", result);
        }

        private static async Task WaitForResultAsync(AngleSharp.Dom.IDocument document, String expected)
        {
            var retries = 200;

            while (retries-- > 0)
            {
                var current = document.QuerySelector("#result")?.TextContent;

                if (String.Equals(current, expected, StringComparison.Ordinal))
                {
                    return;
                }

                await Task.Delay(10).ConfigureAwait(false);
            }
        }
    }
}