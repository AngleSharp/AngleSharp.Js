namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class DomTests
    {
        [Test]
        public async Task NodeHasChildNodesIsAFunction()
        {
            var result = await "document.createElement('div').hasChildNodes".EvalScriptAsync();
            Assert.AreEqual("function hasChildNodes() { [native code] }", result);
        }

        [Test]
        public async Task NodeHasChildNodesWithoutChildren()
        {
            var result = await "document.createElement('div').hasChildNodes()".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

        [Test]
        public async Task NodeHasChildNodesWithChildren()
        {
            var result = await "new DOMParser().parseFromString(`<div><input/></div>`, 'text/html').body.firstChild.hasChildNodes()".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task NumericIndexerOfHtmlCollectionYieldsTheElement()
        {
            var result = await "document.getElementsByTagName('script')[0].nodeName".EvalScriptAsync();
            Assert.AreEqual("SCRIPT", result);
        }

        [Test]
        public async Task NumericIndexerOfHtmlCollectionOutOfRangeIsUndefined()
        {
            var result = await "typeof document.getElementsByTagName('script')[5]".EvalScriptAsync();
            Assert.AreEqual("undefined", result);
        }

        [Test]
        public async Task NumericIndexerOfNodeListYieldsTheNode()
        {
            var result = await "new DOMParser().parseFromString(`<div><input/></div>`, 'text/html').body.childNodes[0].nodeName".EvalScriptAsync();
            Assert.AreEqual("DIV", result);
        }

        [Test]
        public async Task NumericIndexerOfTokenListYieldsTheToken()
        {
            var result = await "new DOMParser().parseFromString(`<div class='a b'></div>`, 'text/html').body.firstChild.classList[1]".EvalScriptAsync();
            Assert.AreEqual("b", result);
        }
    }
}
