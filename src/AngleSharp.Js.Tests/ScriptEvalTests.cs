namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Dom;
    using AngleSharp.Js.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ScriptEvalTests
    {
        public static async Task<String> EvaluateComplexScriptAsync(params String[] sources)
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop();
            var scripts = "<script>" + String.Join("</script><script>", sources) + "</script>";
            var html = "<!doctype html><div id=result></div>" + scripts;
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            return document.GetElementById("result").InnerHtml;
        }

        public static String SetResult(String eval)
        {
            return "document.querySelector('#result').textContent = " + eval + ";";
        }

        [Test]
        public async Task AccessUndefinedGlobalVariable()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("$.toString()"));
            Assert.AreEqual("", result);
        }

        [Test]
        public async Task AccessGlobalVariablesFromOtherScriptShouldWork()
        {
            var result = await EvaluateComplexScriptAsync("var a = 5;", SetResult("a.toString()"));
            Assert.AreEqual("5", result);
        }

        [Test]
        public async Task AccessLocalVariablesFromOtherScriptShouldNotWork()
        {
            var result = await EvaluateComplexScriptAsync("(function () { var a = 5; })();", SetResult("a.toString()"));
            Assert.AreEqual("", result);
        }

        [Test]
        public async Task ExtendingWindowExplicitlyShouldWork()
        {
            var result = await EvaluateComplexScriptAsync("window.foo = 'bla';", SetResult("window.foo"));
            Assert.AreEqual("bla", result);
        }

        [Test]
        public async Task CreateCustomEventViaGeneralConstructorShouldWork()
        {
            var result = await EvaluateComplexScriptAsync("var ev = new Event('foo');", SetResult("ev.type"));
            Assert.AreEqual("foo", result);
        }

        [Test]
        public async Task CreateCustomEventViaCustomConstructorWithDetailShouldWork()
        {
            var result = await EvaluateComplexScriptAsync("var ev = new CustomEvent('bar', { bubbles: false, cancelable: false, details: 'baz' });", SetResult("ev.type + ev.detail"));
            Assert.AreEqual("barbaz", result);
        }

        [Test]
        public async Task CreateXmlHttpRequestShouldWork()
        {
            var result = await EvaluateComplexScriptAsync("var xhr = new XMLHttpRequest(); xhr.open('GET', 'foo');", SetResult("xhr.readyState.toString()"));
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task CreateImageShouldWork()
        {
            var result = await EvaluateComplexScriptAsync("var img = new Image(400, 200); img.src = '/image.jpg';", SetResult("img.width"));
            Assert.AreEqual("400", result);
        }

        [Test]
        public async Task PerformXmlHttpRequestSynchronousToDataUrlShouldWork()
        {
            var req = new DataRequester();
            var cfg = Configuration.Default.With(req).WithJs().WithEventLoop().WithDefaultLoader();
            var script = "var xhr = new XMLHttpRequest(); xhr.open('GET', 'data:plain/text,Hello World!', false);xhr.send();document.querySelector('#result').textContent = xhr.responseText;";
            var html = "<!doctype html><div id=result></div><script>" + script + "</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.QuerySelector("#result").TextContent;
            Assert.AreEqual("Hello World!", result);
        }

        [Test]
        public async Task PerformXmlHttpRequestSynchronousToDelayedResponseShouldWork()
        {
            var message = "Hi!";
            var req = new DelayedRequester(10, message);
            var cfg = Configuration.Default.WithJs().WithEventLoop().With(req).WithDefaultLoader();
            var script = @"
var xhr = new XMLHttpRequest(); 
xhr.open('GET', 'http://example.com/', false);
xhr.send();
document.querySelector('#result').textContent = xhr.responseText;";
            var html = "<!doctype html><div id=result></div><script>" + script + "</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.QuerySelector("#result");
            Assert.AreEqual(message, result.TextContent);
        }

        [Test]
        public async Task PerformXmlHttpRequestAsynchronousToDelayedResponseShouldWork()
        {
            var message = "Hi!";
            var req = new DelayedRequester(10, message);
            var cfg = Configuration.Default.WithJs().WithEventLoop().With(req).WithDefaultLoader();
            var script = @"
var xhr = new XMLHttpRequest(); 
xhr.open('GET', 'http://example.com/');
xhr.addEventListener('load', function (ev) { 
    var res = document.querySelector('#result');
    res.textContent = xhr.responseText;
    res.dispatchEvent(new CustomEvent('xhrdone'));
}, false);
xhr.send();";
            var html = "<!doctype html><div id=result></div><script>" + script + "</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.QuerySelector("#result");
            Assert.AreEqual("", result.TextContent);
            Assert.IsTrue(req.IsStarted);
            await result.AwaitEventAsync("xhrdone").ConfigureAwait(false);
            Assert.AreEqual(message, result.TextContent);
        }

        [Test]
        public async Task SetContentOfIFrameElement()
        {
            var cfg = Configuration.Default
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
                .WithJs()
                .WithEventLoop();
            var html = @"<!doctype html><iframe id=myframe srcdoc=''></iframe><script>
var iframe = document.querySelector('#myframe');
var doc = iframe.contentWindow.document;
doc.body.textContent = 'Hello world.';
</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.GetElementById("myframe") as IHtmlInlineFrameElement;
            Assert.AreEqual("Hello world.", result.ContentDocument.Body.TextContent);
        }

        [Test]
        public async Task RunMainScriptFromHtml5Test()
        {
            var script = @"var p=[],w=window,d=document,e=f=0;p.push('ua='+encodeURIComponent(navigator.userAgent));e|=w.ActiveXObject?1:0;e|=w.opera?2:0;e|=w.chrome?4:0;
e|='getBoxObjectFor' in d || 'mozInnerScreenX' in w?8:0;e|=('WebKitCSSMatrix' in w||'WebKitPoint' in w||'webkitStorageInfo' in w||'webkitURL' in w)?16:0;
e|=(e&16&&({}.toString).toString().indexOf(""\n"")===-1)?32:0;p.push('e='+e);f|='sandbox' in d.createElement('iframe')?1:0;f|='WebSocket' in w?2:0;
f|=w.Worker?4:0;f|=w.applicationCache?8:0;f|=w.history && history.pushState?16:0;f|=d.documentElement.webkitRequestFullScreen?32:0;f|='FileReader' in w?64:0;
p.push('f='+f);p.push('r='+Math.random().toString(36).substring(7));p.push('w='+screen.width);p.push('h='+screen.height);var s=d.createElement('script');
s.src='//api.whichbrowser.net/rel/detect.js?' + p.join('&');d.getElementsByTagName('head')[0].appendChild(s);";
            var result = await EvaluateComplexScriptAsync(script, SetResult("p.join('').toString()"));
            Assert.AreNotEqual("undefined", result);
        }

        [Test]
        public async Task QueryUserAgentShouldMatchAgent()
        {
            var userAgent = new Navigator().UserAgent;
            var result = await EvaluateComplexScriptAsync(SetResult("navigator.userAgent"));
            Assert.AreEqual(userAgent, result);
        }

        [Test]
        public async Task ScreenPixelDepthShouldYield24()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("screen.pixelDepth.toString()"));
            Assert.AreEqual("24", result);
        }

        [Test]
        public async Task PrototypeObjectOfHtmlDocumentIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf(document).toString()"));
            Assert.AreEqual("[object HTMLDocument]", result);
        }

        [Test]
        public async Task PrototypeObjectOfDocumentIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf(Object.getPrototypeOf(document)).toString()"));
            Assert.AreEqual("[object Document]", result);
        }

        [Test]
        public async Task PrototypeObjectOfNodeIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf(Object.getPrototypeOf(Object.getPrototypeOf(document))).toString()"));
            Assert.AreEqual("[object Node]", result);
        }

        [Test]
        public async Task PrototypeObjectOfEventTargetIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf(Object.getPrototypeOf(Object.getPrototypeOf(Object.getPrototypeOf(document)))).toString()"));
            Assert.AreEqual("[object EventTarget]", result);
        }

        [Test]
        public async Task PrototypeObjectOfObjectIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf(Object.getPrototypeOf(Object.getPrototypeOf(Object.getPrototypeOf(Object.getPrototypeOf(document))))).toString()"));
            Assert.AreEqual("[object Object]", result);
        }

        [Test]
        public async Task PrototypeObjectOfObjectLiteralIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf({}).toString()"));
            Assert.AreEqual("[object Object]", result);
        }

        [Test]
        public async Task PrototypeObjectOfBodyIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf(document.body).toString()"));
            Assert.AreEqual("[object HTMLBodyElement]", result);
        }

        [Test]
        public async Task PrototypeObjectOfNavigatorIsCorrect()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("Object.getPrototypeOf(navigator).toString()"));
            Assert.AreEqual("[object Navigator]", result);
        }

        [Test]
        public async Task ConstructorOfHTMLDocumentIsAvailable()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("HTMLDocument.toString()"));
            Assert.AreEqual("function HTMLDocument() { [native code] }", result);
        }

        [Test]
        public async Task StringOfHTMLDocumentIsAvailable()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("HTMLDocument.prototype.toString()"));
            Assert.AreEqual("[object HTMLDocument]", result);
        }

        [Test]
        public async Task QuerySelectorOfHTMLDocumentPrototypeIsAvailable()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("HTMLDocument.prototype.querySelector.toString()"));
            Assert.AreEqual("function querySelector() { [native code] }", result);
        }

        [Test]
        public async Task StringOfMutationObserverIsAvailable()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("MutationObserver.prototype.toString()"));
            Assert.AreEqual("[object MutationObserver]", result);
        }

        [Test]
        public async Task ConstructorOfMutationObserverIsAvailable()
        {
            var result = await EvaluateComplexScriptAsync(SetResult("MutationObserver.toString()"));
            Assert.AreEqual("function MutationObserver() { [native code] }", result);
        }
    }
}
