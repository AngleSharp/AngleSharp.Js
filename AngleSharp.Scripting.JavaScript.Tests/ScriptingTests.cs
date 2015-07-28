namespace AngleSharp.Scripting.JavaScript.Tests
{
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class ScriptingTests
    {
        public static async Task<String> EvaluateSimpleScriptAsync(String source)
        {
            var cfg = Configuration.Default.WithJavaScript();
            var stdOut = Console.Out;
            var output = new StringBuilder();
            Console.SetOut(new StringWriter(output));
            var html = "<!doctype html><script>console.log(" + source + ")</script>";
            await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            Console.SetOut(stdOut);
            return output.ToString().Trim();
        }

        [Test]
        public async Task PerformSimpleCalculation()
        {
            var result = await EvaluateSimpleScriptAsync("2 + 3");
            Assert.AreEqual("5", result);
        }

        [Test]
        public async Task GetPropertyOfDocument()
        {
            var result = await EvaluateSimpleScriptAsync("document.nodeName");
            Assert.AreEqual("#document", result);
        }

        [Test]
        public async Task GetChildNodesLengthOfDocument()
        {
            var result = await EvaluateSimpleScriptAsync("document.childNodes.length");
            Assert.AreEqual("2", result);
        }

        [Test]
        public async Task GetQuerySelectorOfDocument()
        {
            var result = await EvaluateSimpleScriptAsync("document.querySelectorAll('script').length");
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task GetElementsByTagNameOfDocument()
        {
            var result = await EvaluateSimpleScriptAsync("document.getElementsByTagName('script').length");
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task GetQuerySelectorScriptEqualsGetTagNameScript()
        {
            var result = await EvaluateSimpleScriptAsync("document.querySelector('script') === document.getElementsByTagName('script')[0]");
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task GetQuerySelectorScriptNotEqualsGetTagNameBody()
        {
            var result = await EvaluateSimpleScriptAsync("document.querySelector('script') === document.getElementsByTagName('body')[0]");
            Assert.AreEqual("False", result);
        }
    }
}
