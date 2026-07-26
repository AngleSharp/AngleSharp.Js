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
        public async Task PrototypeChainOfElementIsBuiltCompletely()
        {
            var result = await "(function () { var p = Object.getPrototypeOf(document.createElement('div')), t = []; while (p) { t.push(p[Symbol.toStringTag]); p = Object.getPrototypeOf(p); } return t.join(); })()".EvalScriptAsync();
            Assert.AreEqual("HTMLDivElement,HTMLElement,Element,Node,EventTarget,", result);
        }

        [Test]
        public async Task ConstructorPropertyOfPrototypeRefersBackToTheConstructor()
        {
            var result = await "HTMLDivElement.prototype.constructor === HTMLDivElement".EvalScriptAsync();
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

        [Test]
        public async Task ConsoleIsTheSameObjectOnEveryAccess()
        {
            var result = await "window.console === window.console".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task ConsoleKeepsPropertiesAssignedToIt()
        {
            var result = await "(function () { window.console.marker = 'kept'; return window.console.marker; })()".EvalScriptAsync();
            Assert.AreEqual("kept", result);
        }

        [Test]
        public async Task InheritedMemberIsNotAnOwnPropertyOfTheNode()
        {
            var result = await "document.createElement('div').hasOwnProperty('firstChild')".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

        [Test]
        public async Task InheritedMemberIsStillVisibleOnTheNode()
        {
            var result = await "('firstChild' in document.documentElement) + ',' + (typeof document.documentElement.appendChild)".EvalScriptAsync();
            Assert.AreEqual("true,function", result);
        }

        [Test]
        public async Task InheritedAccessorStillReadsAndWrites()
        {
            var result = await "(function () { var d = document.createElement('div'); d.id = 'jint'; return d.id; })()".EvalScriptAsync();
            Assert.AreEqual("jint", result);
        }

        [Test]
        public async Task AssignedPropertyIsReportedConsistently()
        {
            var result = await "(function () { var d = document.createElement('div'); d.custom = 1; return d.hasOwnProperty('custom') + ',' + Object.getOwnPropertyNames(d).join(); })()".EvalScriptAsync();
            Assert.AreEqual("true,custom", result);
        }
    }
}
