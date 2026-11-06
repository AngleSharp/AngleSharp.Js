namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    [TestFixture]
    public class WorkerTests
    {
        private sealed class SourcesRequester : BaseRequester
        {
            private readonly System.Collections.Generic.Dictionary<String, String> _sources;

            public SourcesRequester(System.Collections.Generic.Dictionary<String, String> sources)
            {
                _sources = sources;
            }

            public override Boolean SupportsProtocol(String protocol) => true;

            protected override Task<AngleSharp.Io.IResponse> PerformRequestAsync(AngleSharp.Io.Request request, System.Threading.CancellationToken cancel)
            {
                var found = _sources.TryGetValue(request.Address.Href, out var source);
                return Task.FromResult<AngleSharp.Io.IResponse>(new AngleSharp.Io.DefaultResponse
                {
                    Address = request.Address,
                    StatusCode = found ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                    Content = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(source ?? String.Empty)),
                });
            }
        }

        private static async Task<AngleSharp.Dom.IDocument> OpenWithSourcesAsync(params (String Address, String Source)[] sources)
        {
            var map = new System.Collections.Generic.Dictionary<String, String>();

            foreach (var (address, source) in sources)
            {
                map[address] = source;
            }

            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new SourcesRequester(map))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            return await BrowsingContext.New(config).OpenAsync(m => m.Address("https://example.com/").Content("<!doctype html>"));
        }

        private static async Task<Boolean> WaitFor(Func<Boolean> condition)
        {
            for (var i = 0; i < 300 && !condition(); i++)
            {
                await Task.Delay(10);
            }

            return condition();
        }

        [Test]
        public async Task WorkerRunsAModuleWithItsImports()
        {
            var document = await OpenWithSourcesAsync(
                ("https://example.com/worker.js", "import { v } from './dep.js'; self.__v = v + 1;"),
                ("https://example.com/dep.js", "export const v = 41;"));
            var worker = new AngleSharp.Js.Dom.Worker(document.DefaultView, "https://example.com/worker.js", new AngleSharp.Js.Dom.WorkerOptions { Type = "module" });

            Assert.IsTrue(await WaitFor(() => worker.IsInitialized || worker.StartupError != null));
            Assert.IsNull(worker.StartupError, worker.StartupError?.ToString());
            Assert.AreEqual("42", worker.EvaluateInWorker("String(self.__v)")?.ToString());
            worker.Terminate();
        }

        [Test]
        public async Task WorkerKnowsItsAddress()
        {
            var document = await OpenWithSourcesAsync(("https://example.com/a/worker.js", ""));
            var worker = new AngleSharp.Js.Dom.Worker(document.DefaultView, "https://example.com/a/worker.js");

            Assert.IsTrue(await WaitFor(() => worker.IsInitialized || worker.StartupError != null));
            Assert.AreEqual("https://example.com/a/worker.js", worker.EvaluateInWorker("location.href")?.ToString());
            worker.Terminate();
        }

        [Test]
        public async Task ModuleSyntaxInAClassicWorkerGivesAnErrorEventAndPostingStaysQuiet()
        {
            var document = await OpenWithSourcesAsync(("https://example.com/worker.js", "import 'https://example.com/dep.js';"));
            var worker = new AngleSharp.Js.Dom.Worker(document.DefaultView, "https://example.com/worker.js");
            var failed = false;
            worker.Failed += (_, _) => failed = true;

            Assert.IsTrue(await WaitFor(() => failed));
            Assert.IsNotNull(worker.StartupError);
            Assert.DoesNotThrow(() => worker.PostMessage("x"));
            worker.Terminate();
        }

        [Test]
        public async Task ScriptGetsAnErrorEventOfAWorkerAndCanPostToIt()
        {
            var document = await OpenWithSourcesAsync(("https://example.com/worker.js", "not valid javascript ("));
            document.ExecuteScript(@"
                var w = new Worker('https://example.com/worker.js', { type: 'classic' });
                w.onerror = function (e) { document.title = 'error:' + e.type; };
                w.postMessage('sent right away');");

            Assert.IsTrue(await WaitFor(() => document.Title == "error:error"), document.Title);
        }

        [Test]
        public async Task UnknownWorkerTypeIsRefused()
        {
            var document = await OpenWithSourcesAsync();

            Assert.Throws<ArgumentException>(() => new AngleSharp.Js.Dom.Worker(document.DefaultView, "https://example.com/w.js", new AngleSharp.Js.Dom.WorkerOptions { Type = "other" }));
        }

        [Test]
        public async Task WorkerConstructorShouldExistOnWindow()
        {
            var result = await "typeof Worker".EvalScriptAsync().ConfigureAwait(false);
            Assert.AreEqual("function", result);
        }

        [Test]
        public async Task WorkerReceivesBytesAsArrayBuffer()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new DelayedRequester(0, "self.onmessage = function (event) { self.__kind = event.data instanceof ArrayBuffer ? new Uint8Array(event.data)[0] : 'other'; };"))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(request => request.Content("<!doctype html><div></div>")).ConfigureAwait(false);
            var worker = new AngleSharp.Js.Dom.Worker(document.DefaultView, "https://example.com/worker.js");
            var retries = 200;

            while (retries-- > 0 && !worker.IsInitialized && worker.StartupError == null)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            worker.PostMessage(new Byte[] { 42 });
            retries = 200;

            while (retries-- > 0 && worker.EvaluateInWorker("String(self.__kind)")?.ToString() == "undefined")
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            Assert.AreEqual("42", worker.EvaluateInWorker("String(self.__kind)")?.ToString());
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
        [NonParallelizable]
        public async Task RejectedWorkersShouldNotLeaveRunningThreads()
        {
            using (var context = BrowsingContext.New(Configuration.Default.WithJs()))
            {
                var document = await context.OpenAsync(request => request.Content("<!doctype html>")).ConfigureAwait(false);
                Assert.Throws<DomException>(() => new AngleSharp.Js.Dom.Worker(document.DefaultView, "/worker.js"));
                var before = CountProcessThreads();

                for (var i = 0; i < 32; i++)
                {
                    Assert.Throws<DomException>(() => new AngleSharp.Js.Dom.Worker(document.DefaultView, "/worker.js"));
                }

                // Allow unrelated runtime threads without hiding one surviving loop per failure.
                Assert.Less(CountProcessThreads() - before, 16);
            }
        }

        private static Int32 CountProcessThreads()
        {
            using (var process = Process.GetCurrentProcess())
            {
                return process.Threads.Count;
            }
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
