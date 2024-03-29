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
