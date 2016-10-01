namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Scripting.JavaScript.Services;
    using AngleSharp.Extensions;
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
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><script>var foo = 'test';</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var foo = service.Engine.GetOrCreateJint(document).GetValue("foo");
            Assert.AreEqual(Types.String, foo.Type);
            Assert.AreEqual("test", foo.AsString());
        }

        [Test]
        public async Task RunJavaScriptFunctionFromCSharp()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><script>function square(x) { return x * x; }</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var square = service.Engine.GetOrCreateJint(document).GetValue("square");
            var result = square.Invoke(4);
            Assert.AreEqual(Types.Number, result.Type);
            Assert.AreEqual(16.0, result.AsNumber());
        }

        [Test]
        public async Task RunCSharpFunctionFromJavaScript()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var storedValue = 0.0;
            service.Engine.External["square"] = new Action<Double>(x => storedValue = x);
            var html = "<!doctype html><script>square(4 * 4);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            Assert.AreEqual(16.0, storedValue);
        }

        [Test]
        public async Task AccessCSharpInstanceMembersFromJavaScript()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            service.Engine.External["person"] = new Person { Age = 20, Name = "Foobar" };
            var html = "<!doctype html><script>var str = person.Name + ' is ' + person.Age + ' years old';</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var str = service.Engine.GetOrCreateJint(document).GetValue("str");
            Assert.AreEqual(Types.String, str.Type);
            Assert.AreEqual("Foobar is 20 years old", str.AsString());
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetStringContent()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJavaScript();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("document.querySelector('#test').innerHTML");
            Assert.AreEqual("Test", result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetComplexObjectFromProperty()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJavaScript();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("document.defaultView");
            Assert.AreEqual(document.DefaultView, result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetsWindow()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJavaScript();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("window");
            Assert.AreEqual(document.DefaultView, result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetComplexObjectFromQuerySelector()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJavaScript();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("document.querySelector('#test')");
            Assert.AreEqual(document.QuerySelector("#test"), result);
        }

        [Test]
        public async Task RunScriptSnippetDirectlyGetSimpleValueFromCalculation()
        {
            var html = "<!doctype html><span id=test>Test</span>";
            var config = Configuration.Default.WithJavaScript();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(html));
            var result = document.ExecuteScript("1 + 2 * 3 - 4");
            Assert.AreEqual(3.0, result);
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
