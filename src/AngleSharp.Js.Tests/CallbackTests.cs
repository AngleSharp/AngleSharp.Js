namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// What a script hands to the DOM as a callback: anything callable, not just plain functions.
    /// </summary>
    [TestFixture]
    public class CallbackTests
    {
        private static async Task<IDocument> OpenAsync(String script)
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            return await BrowsingContext.New(config).OpenAsync(m => m.Content($"<!doctype html><button id=b></button><script>{script}</script>"));
        }

        private static IHtmlElement Button(IDocument document) => (IHtmlElement)document.GetElementById("b");

        private static async Task WaitForTitle(IDocument document, String title)
        {
            for (var i = 0; i < 200 && document.Title != title; i++)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            Assert.AreEqual(title, document.Title);
        }

        [Test]
        public async Task TimerRunsBoundFunctionWithItsThisAndArguments()
        {
            var document = await OpenAsync(@"
                function done(a, b) { document.title = this.name + a + b; }
                setTimeout(done.bind({ name: 'bound' }, 1, 2), 0);");

            await WaitForTitle(document, "bound12");
        }

        [Test]
        public async Task EventListenerRunsBoundFunction()
        {
            var document = await OpenAsync(@"
                function clicked(ev) { document.title = this.name + ev.type; }
                document.getElementById('b').addEventListener('click', clicked.bind({ name: 'bound-' }));");

            Button(document).DoClick();

            await WaitForTitle(document, "bound-click");
        }

        [Test]
        public async Task EventListenerRunsBoundFunctionOfBoundFunction()
        {
            var document = await OpenAsync(@"
                function clicked() { document.title = this.name; }
                document.getElementById('b').addEventListener('click', clicked.bind({ name: 'first' }).bind({ name: 'second' }));");

            Button(document).DoClick();

            await WaitForTitle(document, "first");
        }

        [Test]
        public async Task EventListenerRunsCallableProxy()
        {
            var document = await OpenAsync(@"
                var proxy = new Proxy(function () { document.title = 'proxied'; }, {});
                document.getElementById('b').addEventListener('click', proxy);");

            Button(document).DoClick();

            await WaitForTitle(document, "proxied");
        }

        [Test]
        public async Task EventHandlerPropertyRunsAndHandsBackBoundFunction()
        {
            var document = await OpenAsync(@"
                var button = document.getElementById('b');
                var handler = function () { document.title = this.name; }.bind({ name: 'handled' });
                button.onclick = handler;
                document.body.setAttribute('data-same', String(button.onclick === handler));");

            Button(document).DoClick();

            await WaitForTitle(document, "handled");
            Assert.AreEqual("true", document.Body.GetAttribute("data-same"));
        }

        [Test]
        public async Task EventHandlerPropertyIgnoresWhatIsNotCallable()
        {
            var document = await OpenAsync(@"
                var button = document.getElementById('b');
                button.onclick = { name: 'no function' };
                document.body.setAttribute('data-value', String(button.onclick));");

            Assert.AreEqual("null", document.Body.GetAttribute("data-value"));
        }

        [Test]
        public async Task RemovedListenerIsNotCalledAnymore()
        {
            var document = await OpenAsync(@"
                var listener = function () { document.title = 'called'; };
                var button = document.getElementById('b');
                button.addEventListener('click', listener);
                button.removeEventListener('click', listener);");
            var clicks = 0;
            Button(document).AddEventListener("click", (s, e) => clicks++);

            Button(document).DoClick();

            for (var i = 0; i < 300 && clicks == 0; i++)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            Assert.AreEqual(1, clicks);
            Assert.AreEqual(String.Empty, document.Title);
        }

        [Test]
        public async Task RemovedBoundListenerIsNotCalledAnymore()
        {
            var document = await OpenAsync(@"
                var calls = 0;
                var listener = function () { document.title = 'called' + (++calls); }.bind(null);
                var button = document.getElementById('b');
                button.addEventListener('click', listener);
                button.removeEventListener('click', listener);");
            var clicks = 0;
            Button(document).AddEventListener("click", (_, _) => clicks++);

            Button(document).DoClick();

            for (var i = 0; i < 300 && clicks == 0; i++)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            Assert.AreEqual(1, clicks);
            Assert.AreEqual(String.Empty, document.Title);
        }
    }
}
