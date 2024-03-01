namespace AngleSharp.Js.Tests
{
    using AngleSharp.Scripting;
    using Jint;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ComponentTests
    {
        private static async Task<String> RunScriptComponent(String script)
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.WithXml().WithEventLoop().With(service);
            var html = String.Concat("<!doctype html><script>", script, "</script>");
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var value = service.GetOrCreateJint(document).GetValue("assert");
            return value.AsString();
        }

        [Test]
        public async Task DomParserShouldWorkWithHtml()
        {
            var script = @"var htmlSource = '<span>Hello World!</span>';
var parser = new DOMParser();
var doc = parser.parseFromString(htmlSource, 'text/html');
var assert = doc.querySelector('span').textContent;";
            var value = await RunScriptComponent(script);
            Assert.AreEqual("Hello World!", value);
        }

        [Test]
        public async Task DomParserShouldWorkWithXml()
        {
            var script = @"var xmlSource = '<parsererror xmlns=""http://www.mozilla.org/newlayout/xml/parsererror.xml"">(error description)<sourcetext></sourcetext></parsererror>';
var parser = new DOMParser();
var doc = parser.parseFromString(xmlSource, 'application/xml');
var assert = doc.querySelector('parsererror').textContent;";
            var value = await RunScriptComponent(script);
            Assert.AreEqual("(error description)", value);
        }

        [Test]
        public async Task DomParserShouldWorkWithMathMl()
        {
            var script = @"var xmlSource = '<math> <mrow> <msup><mi> a </mi><mn>2</mn></msup> <mo> + </mo> <msup><mi> b </mi><mn>2</mn></msup> <mo> = </mo> <msup><mi> c </mi><mn>2</mn></msup> </mrow> </math>';
var parser = new DOMParser();
var doc = parser.parseFromString(xmlSource, 'application/mathml+xml');
var assert = 'success';";
            var value = await RunScriptComponent(script);
            Assert.AreEqual("success", value);
        }

        [Test]
        public async Task DomParserShouldShowErrorDescriptionForMalformedXml()
        {
            var script = @"var xmlSource = '<foo>';
var parser = new DOMParser();
var doc = parser.parseFromString(xmlSource, 'application/xml');
var assert = doc.querySelector('parsererror').textContent;";
            var value = await RunScriptComponent(script);
            Assert.AreEqual("Error while parsing the provided XML document.", value);
        }
    }
}
