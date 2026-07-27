namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Runtime;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class InteractionTests
    {
        [Test]
        public async Task ReadStoredJavaScriptValueFromCSharp()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><script>var foo = 'test';</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var foo = service.GetOrCreateJint(document).GetValue("foo");
            Assert.AreEqual(Types.String, foo.Type);
            Assert.AreEqual("test", foo.AsString());
        }

        [Test]
        public async Task RunJavaScriptFunctionFromCSharp()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><script>function square(x) { return x * x; }</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var engine = service.GetOrCreateJint(document);
            var square = engine.GetValue("square");
            var result = engine.Invoke(square, 4);
            Assert.AreEqual(Types.Number, result.Type);
            Assert.AreEqual(16.0, result.AsNumber());
        }

        [Test]
        public async Task RunCSharpFunctionFromJavaScript()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service);
            var storedValue = 0.0;
            service.External["square"] = new Action<Double>(x => storedValue = x);
            var html = "<!doctype html><script>square(4 * 4);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            Assert.AreEqual(16.0, storedValue);
        }

        [Test]
        public async Task AccessCSharpInstanceMembersFromJavaScript()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service);
            service.External["person"] = new Person { Age = 20, Name = "Foobar" };
            var html = "<!doctype html><script>var str = person.Name + ' is ' + person.Age + ' years old';</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var str = service.GetOrCreateJint(document).GetValue("str");
            Assert.AreEqual(Types.String, str.Type);
            Assert.AreEqual("Foobar is 20 years old", str.AsString());
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetStringContent()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("document.querySelector('#test').innerHTML");
            Assert.AreEqual("Test", result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetComplexObjectFromProperty()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("document.defaultView");
            Assert.AreEqual(document.DefaultView, result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetsWindow()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("window");
            Assert.AreEqual(document.DefaultView, result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetComplexObjectFromQuerySelector()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("document.querySelector('#test')");
            Assert.AreEqual(document.QuerySelector("#test"), result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetSimpleValueFromCalculation()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("1 + 2 * 3 - 4");
            Assert.AreEqual(3.0, result);
        }

        [Test]
        public async Task RunSameScriptSourceInSeveralDocumentsKeepsStateSeparate()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJs();
            var source = "(function () { window.counter = (window.counter || 0) + 1; return window.counter; })()";
            var first = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var second = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));

            Assert.AreEqual(1.0, first.ExecuteScript(source));
            Assert.AreEqual(1.0, second.ExecuteScript(source));
            Assert.AreEqual(2.0, first.ExecuteScript(source));
            Assert.AreEqual(2.0, second.ExecuteScript(source));
        }

        [Test]
        public async Task RunScriptSnippetWithSyntaxErrorThrows()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            Assert.Throws<JavaScriptException>(() => document.ExecuteScript("function ("));
        }

        [Test]
        public async Task RunScriptAtPressingLink_Issue47()
        {
            var html = "<!doctype html><pre id=test></pre><a href=\"javascript:document.querySelector('#test').textContent='success';\">Test</a>";
            var config = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var otherDocument = await document.QuerySelector<IHtmlAnchorElement>("a").NavigateAsync();
            Assert.AreEqual(document, otherDocument);
            Assert.AreEqual("success", document.QuerySelector("#test").TextContent);
        }

        [Test]
        public async Task SetLocationViaSimpleString_Issue31()
        {
            var html = "<!doctype html><span id=test>Test</span><script>window.location = '/foo';</script>";
            var config = Configuration.Default.WithJs();
            var context = BrowsingContext.New(config);
            await context.OpenAsync(m => m.Content(html).Address("http://example.com"))
                .Then(_ => Assert.AreEqual("http://example.com/foo", context.Active.Location.Href));
        }

        [Test]
        public async Task SetLocationViaBareIdentifier_Issue98()
        {
            var html = "<!doctype html><script>location = '/foo';</script>";
            var config = Configuration.Default.WithJs();
            var context = BrowsingContext.New(config);
            await context.OpenAsync(m => m.Content(html).Address("http://example.com"))
                .Then(_ => Assert.AreEqual("http://example.com/foo", context.Active.Location.Href));
        }

        [Test]
        public async Task SetLocationHrefViaAlias_Issue98()
        {
            var html = "<!doctype html><script>var loc = location; loc.href = '/foo';</script>";
            var config = Configuration.Default.WithJs();
            var context = BrowsingContext.New(config);
            await context.OpenAsync(m => m.Content(html).Address("http://example.com"))
                .Then(_ => Assert.AreEqual("http://example.com/foo", context.Active.Location.Href));
        }

        [Test]
        public async Task SetWindowHrefDoesNotNavigate_Issue98()
        {
            var html = "<!doctype html><script>window.href = '/foo';</script>";
            var config = Configuration.Default.WithJs();
            var context = BrowsingContext.New(config);
            await context.OpenAsync(m => m.Content(html).Address("http://example.com"))
                .Then(_ => Assert.AreEqual("http://example.com/", context.Active.Location.Href));
        }

        [Test]
        public async Task RunJavaScriptFunctionFromCSharpUpdatesDataset_Issue77()
        {
            var service = new JsScriptingService();
            var config = Configuration.Default.With(service);
            var html = @"<!doctype html>
<script>
function test() {
    var element = document.querySelector('section');
    element.dataset.level = '2';
    element.dataset.title = 'section 2';
    return 'test executed';
}
</script>
<section data-level='1' data-title='section 1'></section>";
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var engine = service.GetOrCreateJint(document);
            var test = engine.GetValue("test");
            var result = engine.Invoke(test);
            var section = document.QuerySelector<IHtmlElement>("section");

            Assert.AreEqual(Types.String, result.Type);
            Assert.AreEqual("test executed", result.AsString());
            Assert.AreEqual("2", section.Dataset["level"]);
            Assert.AreEqual("section 2", section.Dataset["title"]);
            Assert.AreEqual("2", section.GetAttribute("data-level"));
            Assert.AreEqual("section 2", section.GetAttribute("data-title"));
        }

        class Person
        {
            public String Name
            {
                get;
                set;
            }

            public Int32 Age
            {
                get;
                set;
            }
        }
    }
}
