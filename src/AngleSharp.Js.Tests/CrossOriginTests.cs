namespace AngleSharp.Js.Tests
{
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Io;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class CrossOriginTests
    {
        private const String PageAddress = "https://a.test/page.html";
        private const String CrossAddress = "https://b.test/frame.html";
        private const String SameAddress = "https://a.test/frame.html";

        private sealed class SiteRequester : BaseRequester
        {
            private readonly Dictionary<String, String> _pages;

            public SiteRequester(Dictionary<String, String> pages) => _pages = pages;

            public override Boolean SupportsProtocol(String protocol) => true;

            protected override async Task<IResponse> PerformRequestAsync(Request request, CancellationToken cancel)
            {
                // the element is set up by the time this arrives: Core before 1.9.2 can lose a frame otherwise
                await Task.Delay(30, cancel).ConfigureAwait(false);
                _pages.TryGetValue(request.Address.Href, out var html);
                return new DefaultResponse
                {
                    Address = request.Address,
                    StatusCode = html is null ? System.Net.HttpStatusCode.NotFound : System.Net.HttpStatusCode.OK,
                    Content = new MemoryStream(Encoding.UTF8.GetBytes(html ?? String.Empty)),
                    Headers = new Dictionary<String, String> { ["Content-Type"] = "text/html" },
                };
            }
        }

        private static async Task<(IDocument Page, IDocument Frame)> LoadAsync(String frameAddress, String frameHtml, String pageScript = "")
        {
            var pages = new Dictionary<String, String>
            {
                [PageAddress] = $"<!doctype html><body><iframe id=f src=\"{frameAddress}\"></iframe><script>{pageScript}</script></body>",
                [frameAddress] = frameHtml,
            };
            var config = Configuration.Default
                .With(new SiteRequester(pages))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
                .WithJs()
                .WithEventLoop();
            var page = await BrowsingContext.New(config).OpenAsync(PageAddress);
            var frame = (IHtmlInlineFrameElement)page.GetElementById("f");
            await Until(() => frame.ContentDocument?.ReadyState == DocumentReadyState.Complete);
            return (page, frame.ContentDocument);
        }

        private static async Task Until(Func<Boolean> condition)
        {
            for (var i = 0; i < 200 && !condition(); i++)
            {
                await Task.Delay(20);
            }

            Assert.IsTrue(condition(), "The condition never held.");
        }

        private static String Eval(IDocument document, String script) => Convert.ToString(document.ExecuteScript(script), System.Globalization.CultureInfo.InvariantCulture);

        [Test]
        public async Task ContentDocumentOfAnotherOriginIsNull()
        {
            var (page, _) = await LoadAsync(CrossAddress, "<!doctype html><title>secret</title>");
            Assert.AreEqual("true", Eval(page, "String(document.getElementById('f').contentDocument === null)"));
        }

        [Test]
        public async Task ContentDocumentOfTheSameOriginIsAvailable()
        {
            var (page, _) = await LoadAsync(SameAddress, "<!doctype html><title>mine</title>");
            Assert.AreEqual("mine", Eval(page, "document.getElementById('f').contentDocument.title"));
            Assert.AreEqual("mine", Eval(page, "document.getElementById('f').contentWindow.document.title"));
        }

        [TestCase("document")]
        [TestCase("localStorage")]
        [TestCase("name")]
        [TestCase("navigator")]
        [TestCase("fetch")]
        [TestCase("setTimeout")]
        [TestCase("getComputedStyle")]
        [TestCase("eval")]
        [TestCase("secret")]
        public async Task WindowOfAnotherOriginHasNoSuchMember(String member)
        {
            var (page, _) = await LoadAsync(CrossAddress, "<!doctype html><script>var secret = 1;</script>");
            Assert.AreEqual("undefined", Eval(page, $"typeof document.getElementById('f').contentWindow.{member}"));
        }

        [TestCase("postMessage", "function")]
        [TestCase("closed", "boolean")]
        [TestCase("length", "number")]
        [TestCase("location", "object")]
        [TestCase("parent", "object")]
        [TestCase("top", "object")]
        public async Task WindowOfAnotherOriginHasWhatIsAllowed(String member, String type)
        {
            var (page, _) = await LoadAsync(CrossAddress, "<!doctype html>");
            Assert.AreEqual(type, Eval(page, $"typeof document.getElementById('f').contentWindow.{member}"));
        }

        [Test]
        public async Task WindowOfAnotherOriginStaysTheSameObject()
        {
            var (page, _) = await LoadAsync(CrossAddress, "<!doctype html>");
            Assert.AreEqual("true", Eval(page, "String(document.getElementById('f').contentWindow === document.getElementById('f').contentWindow)"));
        }

        [Test]
        public async Task LocationOfAnotherOriginCannotBeRead()
        {
            var (page, _) = await LoadAsync(CrossAddress, "<!doctype html>");
            Assert.AreEqual("thrown", Eval(page, "(function () { try { return document.getElementById('f').contentWindow.location.href; } catch (e) { return 'thrown'; } })()"));
        }

        [Test]
        public async Task FrameCannotReachThePageOfAnotherOrigin()
        {
            var (_, frame) = await LoadAsync(CrossAddress, "<!doctype html>");

            Assert.AreEqual("undefined", Eval(frame, "typeof parent.document"));
            Assert.AreEqual("undefined", Eval(frame, "typeof top.document"));
            Assert.AreEqual("undefined", Eval(frame, "typeof parent.parent.document"));
            Assert.AreEqual("true", Eval(frame, "String(parent === top)"));
        }

        [Test]
        public async Task FrameCanReachThePageOfTheSameOrigin()
        {
            var (_, frame) = await LoadAsync(SameAddress, "<!doctype html>");
            Assert.AreEqual("object", Eval(frame, "typeof parent.document"));
        }

        [Test]
        public async Task FrameCannotNavigateThePageOfAnotherOrigin()
        {
            var (page, frame) = await LoadAsync(CrossAddress, "<!doctype html>");

            Assert.AreEqual("thrown", Eval(frame, "(function () { try { top.location.href = 'https://evil.test/'; return 'set'; } catch (e) { return 'thrown'; } })()"));
            Assert.AreEqual(PageAddress, page.Url);
        }

        [Test]
        public async Task PageCanNavigateItsFrameOfAnotherOrigin()
        {
            var (page, frame) = await LoadAsync(CrossAddress, "<!doctype html>");
            Assert.AreEqual("set", Eval(page, "(function () { try { document.getElementById('f').contentWindow.location.href = 'https://c.test/'; return 'set'; } catch (e) { return 'thrown'; } })()"));
        }

        private const String EchoingFrame = "<!doctype html><script>window.addEventListener('message', function (e) { window.seen = JSON.stringify(e.data); window.seenOrigin = e.origin; e.source.postMessage('back', '*'); });</script>";

        [Test]
        public async Task MessageReachesAFrameOfAnotherOriginWithWhoSentIt()
        {
            var (page, frame) = await LoadAsync(CrossAddress, EchoingFrame, "window.addEventListener('message', function (e) { window.reply = e.data; window.replyOrigin = e.origin; });");

            Eval(page, "document.getElementById('f').contentWindow.postMessage({ a: 1, b: [2, 'x'], c: { d: true } }, 'https://b.test')");

            await Until(() => Eval(frame, "window.seen") != "undefined" && Eval(page, "window.reply") == "back");
            Assert.AreEqual("{\"a\":1,\"b\":[2,\"x\"],\"c\":{\"d\":true}}", Eval(frame, "window.seen"));
            Assert.AreEqual("https://a.test", Eval(frame, "window.seenOrigin"));
            Assert.AreEqual("https://b.test", Eval(page, "window.replyOrigin"));
        }

        [TestCase("https://c.test")]
        [TestCase("https://a.test")]
        [TestCase("/")]
        public async Task MessageForAnotherOriginIsNotDelivered(String targetOrigin)
        {
            var (page, frame) = await LoadAsync(CrossAddress, EchoingFrame);

            Eval(page, $"document.getElementById('f').contentWindow.postMessage('x', '{targetOrigin}')");
            await Task.Delay(100);

            Assert.AreEqual("undefined", Eval(frame, "typeof window.seen"));
        }

        [Test]
        public async Task MessageWithOptionsHasItsTargetOrigin()
        {
            var (page, frame) = await LoadAsync(CrossAddress, EchoingFrame);

            Eval(page, "document.getElementById('f').contentWindow.postMessage('x', { targetOrigin: 'https://b.test' })");

            await Until(() => Eval(frame, "typeof window.seen") == "string");
        }

        [TestCase("function () {}")]
        [TestCase("document")]
        [TestCase("{ a: document.body }")]
        public async Task WhatCannotBeClonedIsNotPosted(String message)
        {
            var (page, frame) = await LoadAsync(CrossAddress, EchoingFrame);

            Assert.AreEqual("thrown", Eval(page, $"(function () {{ try {{ document.getElementById('f').contentWindow.postMessage({message}, '*'); return 'posted'; }} catch (e) {{ return 'thrown'; }} }})()"));
        }

        [Test]
        public async Task CyclicObjectIsNotPosted()
        {
            var (page, _) = await LoadAsync(CrossAddress, EchoingFrame);

            Assert.AreEqual("thrown", Eval(page, "(function () { var a = {}; a.self = a; try { document.getElementById('f').contentWindow.postMessage(a, '*'); return 'posted'; } catch (e) { return 'thrown'; } })()"));
        }

        [Test]
        public async Task MessageIsACopy()
        {
            var (page, frame) = await LoadAsync(SameAddress, "<!doctype html><script>window.addEventListener('message', function (e) { e.data.a = 2; window.got = e.data; });</script>");

            Eval(page, "window.sent = { a: 1 }; document.getElementById('f').contentWindow.postMessage(window.sent, '*')");
            await Until(() => Eval(frame, "typeof window.got") == "object");

            Assert.AreEqual("1", Eval(page, "window.sent.a"));
        }

        [Test]
        public async Task SandboxWithoutScriptsRunsNothing()
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            var top = BrowsingContext.New(config);
            var context = top.CreateChild("sandboxed", Sandboxes.Scripts);
            var document = await context.OpenAsync(m => m.Content("<!doctype html><script>document.title = 'ran';</script><body onclick=\"document.title = 'clicked'\">"));

            document.Body.DoClick();

            await Task.Delay(50);
            Assert.AreNotEqual("ran", document.Title);
            Assert.AreNotEqual("clicked", document.Title);
        }

        [Test]
        public async Task SandboxWithoutSameOriginIsOfNoOrigin()
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            var top = await BrowsingContext.New(config).OpenAsync(m => m.Content("<!doctype html>").Address(PageAddress));
            var context = top.Context.CreateChild("sandboxed", Sandboxes.Origin);
            var document = await context.OpenAsync(m => m.Content("<!doctype html>").Address(PageAddress));

            Assert.AreEqual("undefined", Eval(document, "typeof parent.document"));
        }
    }
}
