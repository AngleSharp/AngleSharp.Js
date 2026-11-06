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

        private static async Task<String> Dispatched(String setup)
        {
            var document = await OpenAsync($@"
                var button = document.getElementById('b');
                {setup}
                var ev = new Event('click', {{ cancelable: true }});
                button.dispatchEvent(ev);
                document.title = String(ev.defaultPrevented);");
            return document.Title;
        }

        [Test]
        public async Task EventHandlerPropertyReturningFalseCancelsTheEvent()
        {
            Assert.AreEqual("true", await Dispatched("button.onclick = function () { return false; };"));
        }

        [Test]
        public async Task EventHandlerAttributeReturningFalseCancelsTheEvent()
        {
            Assert.AreEqual("true", await Dispatched("button.setAttribute('onclick', 'return false');"));
        }

        [TestCase("return true;")]
        [TestCase("return 0;")]
        [TestCase("")]
        public async Task EventHandlerPropertyReturningAnythingElseDoesNotCancelTheEvent(String body)
        {
            Assert.AreEqual("false", await Dispatched($"button.onclick = function () {{ {body} }};"));
        }

        [Test]
        public async Task EventListenerReturningFalseDoesNotCancelTheEvent()
        {
            Assert.AreEqual("false", await Dispatched("button.addEventListener('click', function () { return false; });"));
        }

        [Test]
        public async Task BoundEventHandlerReturningFalseCancelsTheEvent()
        {
            Assert.AreEqual("true", await Dispatched("button.onclick = function () { return false; }.bind(null);"));
        }

        [Test]
        public async Task ChangingAnEventHandlerAttributeReplacesTheHandler()
        {
            var document = await OpenAsync(@"
                var calls = [];
                var button = document.getElementById('b');
                button.setAttribute('onclick', 'calls.push(1)');
                button.setAttribute('onclick', 'calls.push(2)');
                button.dispatchEvent(new Event('click'));
                document.title = calls.join();");

            Assert.AreEqual("2", document.Title);
        }

        [Test]
        public async Task RemovingAnEventHandlerAttributeRemovesTheHandler()
        {
            var document = await OpenAsync(@"
                var calls = 0;
                var button = document.getElementById('b');
                button.setAttribute('onclick', 'calls++');
                button.removeAttribute('onclick');
                button.dispatchEvent(new Event('click'));
                document.title = String(calls);");

            Assert.AreEqual("0", document.Title);
        }

        [Test]
        public async Task EventHandlerAttributeWithASyntaxErrorIsReported()
        {
            var document = await OpenAsync(@"
                window.onerror = function (message) { document.title = 'reported'; };
                var button = document.getElementById('b');
                button.setAttribute('onclick', 'function (');
                document.body.setAttribute('data-after', 'yes');
                button.dispatchEvent(new Event('click'));");

            Assert.AreEqual("reported", document.Title);
            Assert.AreEqual("yes", document.Body.GetAttribute("data-after"));
        }

        [Test]
        public async Task IdleCallbackGetsItsDeadline()
        {
            var document = await OpenAsync(@"
                var called = false;
                requestIdleCallback(function (deadline) {
                    document.title = [deadline.didTimeout, deadline.timeRemaining() > 0, deadline.timeRemaining() <= 50].join();
                }, { timeout: 100 });");

            await WaitForTitle(document, "false,true,true");
        }

        [Test]
        public async Task CancelledIdleCallbackIsNotCalled()
        {
            var document = await OpenAsync(@"
                var handle = requestIdleCallback(function () { document.title = 'wrong'; });
                cancelIdleCallback(handle);
                setTimeout(function () { document.title = 'fine'; }, 30);");

            await WaitForTitle(document, "fine");
        }

        [Test]
        public async Task DomParserLeavesThePageAlone()
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(m => m.Content(@"<!doctype html><p id=mine>page</p><script>
                var parsed = new DOMParser().parseFromString('<!doctype html><p id=parsed>other</p>', 'text/html');
                document.title = [parsed.getElementById('parsed').textContent, parsed === document, document.getElementById('mine').textContent].join();
                </script>"));

            Assert.AreSame(document, context.Active);
            Assert.AreEqual("other,false,page", document.Title);
            Assert.IsNotNull(document.GetElementById("mine"));
        }

        [Test]
        public async Task DomParserDoesNotRunWhatItParses()
        {
            var document = await OpenAsync(@"
                new DOMParser().parseFromString('<script>document.title = 1;</' + 'script><img src=x onerror=""window.parent.document.title = 2"">', 'text/html');
                document.title = 'untouched';");

            await Task.Delay(50).ConfigureAwait(false);

            Assert.AreEqual("untouched", document.Title);
        }

        [TestCase("Uint8Array")]
        [TestCase("Int32Array")]
        [TestCase("Uint16Array")]
        public async Task CryptoFillsTheArrayAndGivesItBack(String type)
        {
            var document = await OpenAsync($@"
                var a = new {type}(32);
                var r = crypto.getRandomValues(a);
                document.title = [r === a, Array.from(a).some(function (v) {{ return v !== 0; }})].join();");

            Assert.AreEqual("true,true", document.Title);
        }

        [TestCase("new Float64Array(1)")]
        [TestCase("new Uint8Array(65537)")]
        public async Task CryptoRefusesWhatItCannotFill(String array)
        {
            var document = await OpenAsync($"try {{ crypto.getRandomValues({array}); document.title = 'no error'; }} catch (e) {{ document.title = e.name; }}");

            StringAssert.EndsWith("Error", document.Title);
        }

        [Test]
        public async Task CryptoGivesUuids()
        {
            var document = await OpenAsync("var a = crypto.randomUUID(); document.title = [/^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/.test(a), a !== crypto.randomUUID()].join();");

            Assert.AreEqual("true,true", document.Title);
        }

        private static async Task<String> Listened(String script)
        {
            var document = await OpenAsync($@"
                var button = document.getElementById('b');
                var calls = 0;
                function listener() {{ calls++; }}
                function click() {{ button.dispatchEvent(new Event('click', {{ bubbles: true, cancelable: true }})); }}
                {script}
                document.title = String(calls);");
            return document.Title;
        }

        [TestCase("button.addEventListener('click', listener, { once: true }); click(); click();", "1")]
        [TestCase("button.addEventListener('click', listener, { once: false }); click(); click();", "2")]
        [TestCase("button.addEventListener('click', listener, {}); click();", "1")]
        [TestCase("button.addEventListener('click', listener, undefined); click();", "1")]
        [TestCase("button.addEventListener('click', listener, true); click();", "1")]
        [TestCase("button.addEventListener('click', listener, false); click();", "1")]
        [TestCase("button.addEventListener('click', listener, { passive: true }); button.removeEventListener('click', listener); click();", "0")]
        [TestCase("button.addEventListener('click', listener, { capture: true }); button.removeEventListener('click', listener); click();", "1")]
        [TestCase("button.addEventListener('click', listener, { capture: true }); button.removeEventListener('click', listener, { capture: true }); click();", "0")]
        [TestCase("button.addEventListener('click', listener, true); button.removeEventListener('click', listener, { capture: true }); click();", "0")]
        [TestCase("button.addEventListener('click', listener, { capture: true }); button.removeEventListener('click', listener, true); click();", "0")]
        [TestCase("button.addEventListener('click', listener, { passive: false }); button.removeEventListener('click', listener, false); click();", "0")]
        public async Task ListenerOptionsAreTakenIntoAccount(String script, String expected)
        {
            Assert.AreEqual(expected, await Listened(script));
        }

        [Test]
        public async Task PassiveListenerCannotCancel()
        {
            Assert.AreEqual("false", await Dispatched("button.addEventListener('click', function (ev) { ev.preventDefault(); }, { passive: true });"));
        }

        [Test]
        public async Task CapturingListenerIsCalledBeforeBubblingOne()
        {
            var document = await OpenAsync(@"
                var log = [];
                var button = document.getElementById('b');
                document.body.addEventListener('click', function () { log.push('bubble'); }, { capture: false });
                document.body.addEventListener('click', function () { log.push('capture'); }, { capture: true });
                button.dispatchEvent(new Event('click', { bubbles: true }));
                document.title = log.join();");

            Assert.AreEqual("capture,bubble", document.Title);
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
