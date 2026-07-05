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
    }
}
