namespace AngleSharp.Scripting.JavaScript.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class JqueryTests
    {
        public static async Task<String> EvaluateScriptWithJqueryAsync(params String[] sources)
        {
            var list = new List<String>(sources);
            list.Insert(0, Sources.Jquery);
            var cfg = Configuration.Default.WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true).WithJavaScript();
            var scripts = "<script>" + String.Join("</script><script>", list) + "</script>";
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
    }
}
