namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class PseudoPropertiesTests
    {
        [Test]
        public async Task GetScrollLeftOfElement()
        {
            var result = await "document.createElement('div').scrollLeft".EvalScriptAsync();
            Assert.AreEqual("0", result);
        }

        [Test]
        public async Task GetScrollLeftOfDocument()
        {
            var result = await "document.scrollLeft".EvalScriptAsync();
            Assert.AreEqual("undefined", result);
        }

        [Test]
        public async Task GetFocusInOfDocument()
        {
            var result = await "document.focusIn".EvalScriptAsync();
            Assert.AreEqual("undefined", result);
        }

        [Test]
        public async Task GetFocusInOfElement()
        {
            var result = await "document.createElement('div').focusin".EvalScriptAsync();
            Assert.AreEqual("null", result);
        }
    }
}
