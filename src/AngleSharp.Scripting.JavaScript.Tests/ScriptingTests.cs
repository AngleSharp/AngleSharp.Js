namespace AngleSharp.Scripting.JavaScript.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class ScriptingTests
    {
        [Test]
        public async Task PerformSimpleCalculation()
        {
            var result = await "2 + 3".EvalScriptAsync();
            Assert.AreEqual("5", result);
        }

        [Test]
        public async Task GetPropertyOfDocument()
        {
            var result = await "document.nodeName".EvalScriptAsync();
            Assert.AreEqual("#document", result);
        }

        [Test]
        public async Task GetNodeTypeOfDocument()
        {
            var type = await "typeof document.nodeType".EvalScriptAsync();
            var value = await "document.nodeType".EvalScriptAsync();
            Assert.AreEqual("number", type);
            Assert.AreEqual("9", value);
        }

        [Test]
        public async Task GetChildNodesLengthOfDocument()
        {
            var result = await "document.childNodes.length".EvalScriptAsync();
            Assert.AreEqual("2", result);
        }

        [Test]
        public async Task GetQuerySelectorOfDocument()
        {
            var result = await "document.querySelectorAll('script').length".EvalScriptAsync();
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task GetElementsByTagNameOfDocument()
        {
            var result = await "document.getElementsByTagName('script').length".EvalScriptAsync();
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task GetQuerySelectorScriptEqualsGetTagNameScript()
        {
            var result = await "document.querySelector('script') === document.getElementsByTagName('script')[0]".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task GetQuerySelectorScriptNotEqualsGetTagNameBody()
        {
            var result = await "document.querySelector('script') === document.getElementsByTagName('body')[0]".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }
    }
}
