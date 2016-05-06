namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom.Html;
    using AngleSharp.Extensions;
    using AngleSharp.Scripting.JavaScript.Dom;
    using AngleSharp.Scripting.JavaScript.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ScriptEvalTests
    {
        public static async Task<String> EvaluateComplexScriptAsync(params String[] sources)
        {
            var cfg = Configuration.Default.WithJavaScript();
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
            var result = await EvaluateComplexScriptAsync("var ev = new CustomEvent('bar', false, false, 'baz');", SetResult("ev.type + ev.detail"));
            Assert.AreEqual("barbaz", result);
        }

        [Test]
        public async Task CreateXmlHttpRequestShouldWork()
        {
            var result = await EvaluateComplexScriptAsync("var xhr = new XMLHttpRequest(); xhr.open('GET', 'foo');", SetResult("xhr.readyState.toString()"));
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task PerformXmlHttpRequestSynchronousToDataUrlShouldWork()
        {
            var cfg = Configuration.Default.WithJavaScript().WithDefaultLoader();
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
            var cfg = Configuration.Default.WithJavaScript().WithDefaultLoader(requesters: new[] { new DelayedRequester(10, message) });
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
            var cfg = Configuration.Default.WithJavaScript().WithDefaultLoader(requesters: new [] { req });
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
            var cfg = Configuration.Default.
                WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true).
                WithJavaScript();
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
    }
}
