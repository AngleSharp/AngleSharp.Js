namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Extensions;
    using AngleSharp.Scripting.JavaScript.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class JqueryTests
    {
        public static Task<String> EvaluateScriptWithJqueryAsync(params String[] sources)
        {
            var list = new List<String>(sources);
            list.Insert(0, Sources.Jquery);
            return EvaluateScriptsAsync(list);
        }

        public static async Task<String> EvaluateScriptsAsync(List<String> sources)
        {
            var cfg = Configuration.Default.WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true).WithJavaScript().WithCss();
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
        public async Task LoadJqueryWithoutErrors()
        {
            var result = await EvaluateScriptWithJqueryAsync(SetResult("$.toString()"));
            Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryWithSimpleSelector()
        {
            var result = await EvaluateScriptWithJqueryAsync(SetResult("$('#result').length.toString()"));
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task JqueryWithSettingAttribute()
        {
            var result = await EvaluateScriptWithJqueryAsync("$('#result').attr('foo', 'bar')", SetResult("$('#result').attr('foo')"));
            Assert.AreEqual("bar", result);
        }

        [Test]
        public async Task JqueryWithSettingTextProperty()
        {
            var result = await EvaluateScriptWithJqueryAsync("$('#result').text('<span>foo&gt;</span>');");
            Assert.AreEqual("&lt;span&gt;foo&amp;gt;&lt;/span&gt;", result);
        }

        [Test]
        public async Task JqueryWithSettingHtmlProperty()
        {
            var result = await EvaluateScriptWithJqueryAsync("$('#result').html('<span>foo&gt;</span>')");
            Assert.AreEqual("<span>foo&gt;</span>", result);
        }

        [Test]
        public async Task JqueryWithAjaxToDelayedResponse()
        {
            var message = "Hi!";
            var req = new DelayedRequester(10, message);
            var cfg = Configuration.Default.WithJavaScript().WithDefaultLoader(requesters: new[] { req });
            var sources = new [] { Sources.Jquery, @"
$.ajax('http://example.com/', {
    success: function (data, status, xhr) { 
        var res = document.querySelector('#result');
        res.textContent = xhr.responseText;
        res.dispatchEvent(new CustomEvent('xhrdone'));
    }
});" };
            var scripts = String.Join("</script><script>", sources);
            var html = "<!doctype html><div id=result></div><script>" + scripts + "</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.QuerySelector("#result");
            Assert.AreEqual("", result.TextContent);
            Assert.IsTrue(req.IsStarted);
            await result.AwaitEventAsync("xhrdone").ConfigureAwait(false);
            Assert.AreEqual(message, result.TextContent);
        }

        [Test]
        public async Task JqueryVersionOne()
        {
            var result = await EvaluateScriptsAsync(new List<String> { Sources.Jquery1, SetResult("$.toString()") });
            Assert.AreNotEqual("", result);
        }
    }
}
