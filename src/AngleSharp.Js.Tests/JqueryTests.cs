namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Js.Tests.Mocks;
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
            list.Insert(0, Constants.Jquery2_1_4);
            return list.EvalScriptsAsync();
        }

        static String SetResult(String eval)
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
            var cfg = Configuration.Default.WithJs().With(req).WithDefaultLoader();
            var sources = new [] { Constants.Jquery2_1_4, @"
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
            var result = await (new [] { Constants.Jquery1_11_2, SetResult("$.toString()") }).EvalScriptsAsync();
            Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryVersionTwoTwoFour_Issue43()
        {
            Assert.Inconclusive();
            //var result = await (new[] { Constants.Jquery2_2_4, SetResult("$.toString()") }).EvalScriptsAsync();
            //Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryVersionThreeTwoOne_Issue43()
        {
            Assert.Inconclusive();
            //var result = await (new[] { Constants.Jquery3_2_1, SetResult("$.toString()") }).EvalScriptsAsync();
            //Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryVersionOneTwelveFour_Issue43()
        {
            Assert.Inconclusive();
            //var result = await (new[] { Constants.Jquery1_12_4, SetResult("$.toString()") }).EvalScriptsAsync();
            //Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task UnknownSelectorShouldFailAndBeCaught()
        {
            var script = @"
	var div = document.createElement('div');
    document.body.appendChild(div);
    div.id = 'foo';

    try {
        var x = document.querySelectorAll('*,:x');
        div.textContent = 'succcess';
    }
    catch (e) {
        div.textContent = 'failed';
    }";
            var result = await (new[] { script, SetResult("document.querySelector('#foo').textContent") }).EvalScriptsAsync();
            Assert.AreEqual("failed", result);
        }
    }
}
