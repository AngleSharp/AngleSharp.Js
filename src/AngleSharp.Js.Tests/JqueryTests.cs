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
        private static Task<String> EvaluateScriptWithJqueryAsync(params String[] sources)
        {
            var list = new List<String>(sources);
            list.Insert(0, Constants.Jquery2_1_4);
            return list.EvalScriptsAsync();
        }

        private static String SetResult(String eval) =>
            $"document.querySelector('#result').textContent = {eval};";

        [Test]
        public async Task LoadJqueryWithoutErrors()
        {
            var result = await EvaluateScriptWithJqueryAsync(SetResult("$.toString()"))
                .ConfigureAwait(false);
            Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryWithSimpleSelector()
        {
            var result = await EvaluateScriptWithJqueryAsync(SetResult("$('#result').length.toString()"))
                .ConfigureAwait(false);
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task JqueryWithSettingAttribute()
        {
            var result = await EvaluateScriptWithJqueryAsync("$('#result').attr('foo', 'bar')", SetResult("$('#result').attr('foo')"))
                .ConfigureAwait(false);
            Assert.AreEqual("bar", result);
        }

        [Test]
        public async Task JqueryWithSettingTextProperty()
        {
            var result = await EvaluateScriptWithJqueryAsync("$('#result').text('<span>foo&gt;</span>');")
                .ConfigureAwait(false);
            Assert.AreEqual("&lt;span&gt;foo&amp;gt;&lt;/span&gt;", result);
        }

        [Test]
        public async Task JqueryWithSettingHtmlProperty()
        {
            var result = await EvaluateScriptWithJqueryAsync("$('#result').html('<span>foo&gt;</span>')")
                .ConfigureAwait(false);
            Assert.AreEqual("<span>foo&gt;</span>", result);
        }

        [Test]
        public async Task JqueryWithAjaxToDelayedResponse()
        {
            var message = "Hi!";
            var req = new DelayedRequester(10, message);
            var cfg = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(req)
                .WithDefaultLoader()
                .WithNavigator()
                .WithCookies();
            var sources = new[] { Constants.Jquery2_1_4, @"
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
            var result = await (new[] { Constants.Jquery1_11_2, SetResult("$.toString()") }).EvalScriptsAsync()
                .ConfigureAwait(false);
            Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryVersionTwoTwoFour_Issue43()
        {
            var result = await (new[] { Constants.Jquery2_2_4, SetResult("$.toString()") }).EvalScriptsAsync()
                .ConfigureAwait(false);
            Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryVersionThreeTwoOne_Issue43()
        {
            var result = await (new[] { Constants.Jquery3_2_1, SetResult("$.toString()") }).EvalScriptsAsync()
                .ConfigureAwait(false);
            Assert.AreNotEqual("", result);
        }

        [Test]
        public async Task JqueryVersionOneTwelveFour_Issue43()
        {
            var result = await (new[] { Constants.Jquery1_12_4, SetResult("$.toString()") }).EvalScriptsAsync()
                .ConfigureAwait(false);
            Assert.AreNotEqual("", result);
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

        [Test]
        public async Task JqueryCanParseAndAppendHtmlFragments()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
var nodes = $.parseHTML('<ul><li data-id=\'1\'>A</li><li data-id=\'2\'>B</li></ul>');
$(document.body).append(nodes);
document.querySelector('#result').textContent = $('li').map(function () { return $(this).data('id'); }).get().join(',');")
                .ConfigureAwait(false);
            Assert.AreEqual("1,2", result);
        }

        [Test]
        public async Task JqueryDelegatedClickHandlerShouldFire()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
$(document.body).append('<ul id=\'todos\'><li><button class=\'remove\'>remove</button></li></ul>');
$('#todos').on('click', '.remove', function () {
    $('#result').text('delegated');
});
$('.remove').trigger('click');")
                .ConfigureAwait(false);
            Assert.AreEqual("delegated", result);
        }

        [Test]
        public async Task JqueryDeferredShouldResolveThenCallback()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
var deferred = $.Deferred();
deferred.then(function (value) {
    $('#result').text(value);
});
deferred.resolve('done');")
                .ConfigureAwait(false);
            Assert.AreEqual("done", result);
        }

        [Test]
        public async Task JqueryCanSerializeFormFields()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
$(document.body).append('<form id=\'f\'><input name=\'q\' value=\'anglesharp\'><input type=\'checkbox\' name=\'x\' value=\'1\' checked></form>');
$('#result').text($('#f').serialize());")
                .ConfigureAwait(false);
            Assert.AreEqual("q=anglesharp&amp;x=1", result);
        }

        [Test]
        public async Task JqueryToggleClassShouldAffectSelectorMatches()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
$('#result').addClass('active');
$('#result').toggleClass('active');
$('#result').text($('.active').length.toString());")
                .ConfigureAwait(false);
            Assert.AreEqual("0", result);
        }

        [Test]
        public async Task JqueryDataShouldStoreAndReadObjectValues()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
$('#result').data('config', { enabled: true, retries: 2 });
var cfg = $('#result').data('config');
$('#result').text(cfg.enabled.toString() + ':' + cfg.retries.toString());")
                .ConfigureAwait(false);
            Assert.AreEqual("true:2", result);
        }

        [Test]
        public async Task JqueryCloneTrueShouldCopyEventHandlers()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
var source = $('<button id=\'source\'>go</button>');
var clicks = 0;
source.on('click', function () { clicks++; });
var clone = source.clone(true);
$(document.body).append(clone);
clone.trigger('click');
clone.trigger('click');
$('#result').text(clicks.toString());")
                .ConfigureAwait(false);
            Assert.AreEqual("2", result);
        }

        [Test]
        public async Task JqueryMapShouldProjectDomCollection()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
$(document.body).append('<div class=\'item\'>a</div><div class=\'item\'>b</div><div class=\'item\'>c</div>');
var joined = $('.item').map(function (_, el) { return el.textContent.toUpperCase(); }).get().join('-');
$('#result').text(joined);")
                .ConfigureAwait(false);
            Assert.AreEqual("A-B-C", result);
        }

        [Test]
        public async Task JqueryClosestShouldFindParentMatch()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
$(document.body).append('<section class=\'panel\'><div><span id=\'inner\'>x</span></div></section>');
var hasPanel = $('#inner').closest('.panel').length;
$('#result').text(hasPanel.toString());")
                .ConfigureAwait(false);
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task JqueryQueueShouldRunInOrder()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
var steps = [];
var el = $('#result');
el.queue(function (next) { steps.push('a'); next(); });
el.queue(function (next) { steps.push('b'); next(); });
el.dequeue();
el.promise().then(function () {
    el.text(steps.join(''));
});")
                .ConfigureAwait(false);
            Assert.AreEqual("ab", result);
        }

        [Test]
        public async Task JqueryWrapShouldCreateWrapperElement()
        {
            var result = await EvaluateScriptWithJqueryAsync(@"
$(document.body).append('<span id=\'target\'>wrapped</span>');
$('#target').wrap('<div class=\'wrapper\'></div>');
$('#result').text($('#target').parent().hasClass('wrapper').toString());")
                .ConfigureAwait(false);
            Assert.AreEqual("true", result);
        }
    }
}
