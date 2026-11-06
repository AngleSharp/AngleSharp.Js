namespace AngleSharp.Js.Tests
{
    using AngleSharp;
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class MessageChannelTests
    {
        private static async Task<IDocument> OpenAsync(String script)
        {
            var configuration = Configuration.Default
                .WithJs()
                .WithEventLoop();
            return await BrowsingContext.New(configuration)
                .OpenAsync(request => request.Content($"<!doctype html><title></title><script>{script}</script>"))
                .ConfigureAwait(false);
        }

        private static async Task<Boolean> WaitFor(Func<Boolean> condition)
        {
            for (var i = 0; i < 300 && !condition(); i++)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            return condition();
        }

        [Test]
        public async Task MessagesAreClonedAndDeliveredAfterMicrotasks()
        {
            var document = await OpenAsync(@"
                var channel = new MessageChannel();
                var sent = { value: 1 };
                channel.port1.onmessage = function (event) {
                    document.title += ',' + event.data.value + ',' + (event.data !== sent);
                };
                channel.port2.postMessage(sent);
                channel.port2.postMessage({ value: 2 });
                document.title = 'sync';
                Promise.resolve().then(function () { document.title += ',microtask'; });
            ");

            Assert.IsTrue(await WaitFor(() => document.Title == "sync,microtask,1,true,2,true").ConfigureAwait(false), document.Title);
        }

        [Test]
        public async Task MessagesAreDeliveredWithoutAnExplicitEventLoop()
        {
            var configuration = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(configuration)
                .OpenAsync(request => request.Content(@"<!doctype html><title></title><script>
                    var channel = new MessageChannel();
                    channel.port1.onmessage = function () { document.title += ',message'; };
                    document.title = 'sync';
                    channel.port2.postMessage('queued');
                    Promise.resolve().then(function () { document.title += ',microtask'; });
                </script>"))
                .ConfigureAwait(false);

            Assert.IsTrue(await WaitFor(() => document.Title == "sync,microtask,message").ConfigureAwait(false), document.Title);
        }

        [Test]
        public async Task WindowMessagesAreAsynchronousWithoutAnExplicitEventLoop()
        {
            var configuration = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(configuration)
                .OpenAsync(request => request.Content(@"<!doctype html><title></title><script>
                    window.onmessage = function (event) {
                        document.title = document.title + ',' + event.data;
                    };
                    document.title = 'sync';
                    window.postMessage('message', '*');
                </script>"))
                .ConfigureAwait(false);

            Assert.IsTrue(await WaitFor(() => document.Title == "sync,message").ConfigureAwait(false), document.Title);
        }

        [Test]
        public async Task AddEventListenerDoesNotStartAPort()
        {
            var document = await OpenAsync(@"
                var channel = new MessageChannel();
                channel.port1.addEventListener('message', function () { document.title = 'received'; });
                channel.port2.postMessage('queued');
            ");

            await Task.Delay(50).ConfigureAwait(false);
            Assert.AreEqual(String.Empty, document.Title);

            document.ExecuteScript("channel.port1.start();");
            Assert.IsTrue(await WaitFor(() => document.Title == "received").ConfigureAwait(false));
        }

        [Test]
        public async Task WindowPostMessageTransfersAndNeutersPorts()
        {
            var document = await OpenAsync(@"
                var channel = new MessageChannel();
                var transferred = channel.port2;
                window.onmessage = function (event) {
                    document.title = event.data.port === event.ports[0] && event.data.port !== transferred ? 'transferred' : 'invalid';
                };
                window.postMessage({ port: transferred }, '*', [transferred]);
                transferred.postMessage('dropped');
            ");

            Assert.IsTrue(await WaitFor(() => document.Title == "transferred" || document.Title == "invalid").ConfigureAwait(false), document.Title);
            Assert.AreEqual("transferred", document.Title);
        }

        [Test]
        public async Task MessagePortPostMessageTransfersPorts()
        {
            var document = await OpenAsync(@"
                var channel = new MessageChannel();
                var carrier = new MessageChannel();
                carrier.port1.onmessage = function (event) { document.title = event.data; };
                channel.port1.onmessage = function (event) {
                    if (event.data.port !== event.ports[0]) {
                        document.title = 'invalid';
                    } else {
                        event.ports[0].postMessage('transferred');
                    }
                };
                channel.port2.postMessage({ port: carrier.port2 }, [carrier.port2]);
            ");

            Assert.IsTrue(await WaitFor(() => document.Title == "transferred" || document.Title == "invalid").ConfigureAwait(false), document.Title);
            Assert.AreEqual("transferred", document.Title);
        }

        [Test]
        public async Task ClosingAPortDropsMessages()
        {
            var document = await OpenAsync(@"
                var channel = new MessageChannel();
                channel.port1.onmessage = function () { document.title = 'received'; };
                channel.port1.close();
                channel.port2.postMessage('dropped');
            ");

            await Task.Delay(50).ConfigureAwait(false);
            Assert.AreEqual(String.Empty, document.Title);
        }

        [Test]
        public async Task UncloneableMessagesThrowWithoutTransferringPorts()
        {
            var document = await OpenAsync(@"
                var channel = new MessageChannel();
                channel.port1.onmessage = function (event) {
                    document.title = event.data + ',' + window.__cloneError;
                };
                try {
                    channel.port2.postMessage(function () {}, [channel.port2]);
                } catch (error) {
                    window.__cloneError = error.name;
                }
                channel.port2.postMessage('still connected');
            ");

            Assert.IsTrue(await WaitFor(() => document.Title == "still connected,DataCloneError").ConfigureAwait(false), document.Title);
        }

        [Test]
        public async Task MessageLoopAllowsTimersToRun()
        {
            var document = await OpenAsync(@"
                var channel = new MessageChannel();
                var count = 0;
                var timerCount = -1;
                channel.port1.onmessage = function () {
                    count++;
                    if (count < 1000) {
                        channel.port2.postMessage(null);
                    } else {
                        document.title = String(timerCount);
                    }
                };
                setTimeout(function () { timerCount = count; }, 0);
                channel.port2.postMessage(null);
            ");

            Assert.IsTrue(await WaitFor(() => document.Title.Length != 0).ConfigureAwait(false));
            Assert.GreaterOrEqual(Int32.Parse(document.Title), 0);
            Assert.Less(Int32.Parse(document.Title), 1000);
        }

        [Test]
        public async Task WorkerReceivesTransferredPorts()
        {
            var configuration = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new DelayedRequester(0, "self.onmessage = function (event) { event.data.port.postMessage('worker'); };"))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var document = await BrowsingContext.New(configuration)
                .OpenAsync(request => request.Address("https://example.com/").Content("<!doctype html><title></title>"))
                .ConfigureAwait(false);
            document.ExecuteScript(@"
                var channel = new MessageChannel();
                channel.port1.onmessage = function (event) { document.title = event.data; };
                var worker = new Worker('https://example.com/worker.js');
                worker.postMessage({ port: channel.port2 }, [channel.port2]);
            ");

            Assert.IsTrue(await WaitFor(() => document.Title == "worker").ConfigureAwait(false), document.Title);
        }

        [Test]
        public async Task WorkerCanTransferAPortBackToItsOwner()
        {
            var configuration = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new DelayedRequester(0, "self.onmessage = function (event) { self.postMessage({ port: event.data.port }, [event.data.port]); };"))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var document = await BrowsingContext.New(configuration)
                .OpenAsync(request => request.Address("https://example.com/").Content("<!doctype html><title></title>"))
                .ConfigureAwait(false);
            document.ExecuteScript(@"
                var channel = new MessageChannel();
                channel.port1.onmessage = function (event) { document.title = event.data; };
                var worker = new Worker('https://example.com/worker.js');
                worker.onmessage = function (event) {
                    event.data.port.postMessage('returned');
                };
                worker.postMessage({ port: channel.port2 }, [channel.port2]);
            ");

            Assert.IsTrue(await WaitFor(() => document.Title == "returned").ConfigureAwait(false), document.Title);
        }
    }
}
