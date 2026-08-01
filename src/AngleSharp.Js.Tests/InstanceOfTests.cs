namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class InstanceOfTests
    {
        //  The two cases reported in https://github.com/AngleSharp/AngleSharp.Js/issues/103

        [Test]
        public async Task WindowIsAnInstanceOfWindow()
        {
            var result = await "window instanceof Window".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task CreatedElementIsAnInstanceOfElement()
        {
            var result = await "document.createElement('div') instanceof Element".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        //  Every level of the chain on its own - the leaf passing says nothing about the rest.

        [Test]
        public async Task CreatedElementIsAnInstanceOfItsOwnType()
        {
            var result = await "document.createElement('div') instanceof HTMLDivElement".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task CreatedElementIsAnInstanceOfHtmlElement()
        {
            var result = await "document.createElement('div') instanceof HTMLElement".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task CreatedElementIsAnInstanceOfNode()
        {
            var result = await "document.createElement('div') instanceof Node".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task CreatedElementIsAnInstanceOfEventTarget()
        {
            var result = await "document.createElement('div') instanceof EventTarget".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task CreatedElementIsNotAnInstanceOfAnUnrelatedType()
        {
            var result = await "document.createElement('div') instanceof HTMLAnchorElement".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

        [Test]
        public async Task DocumentIsAnInstanceOfHtmlDocument()
        {
            var result = await "document instanceof HTMLDocument".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task DocumentIsAnInstanceOfDocument()
        {
            var result = await "document instanceof Document".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        //  An anchor reaches HTMLElement through the URLUtils mixin, which sits in the chain
        //  because AngleSharp implements it as an abstract class of its own.

        [Test]
        public async Task AnchorIsAnInstanceOfHtmlElement()
        {
            var result = await "document.createElement('a') instanceof HTMLElement".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        //  An element class carrying no DOM name of its own - a "b" is an HTMLElement in a
        //  browser, and now here as well.

        [Test]
        public async Task UnnamedElementIsAnInstanceOfHtmlElement()
        {
            var result = await "document.createElement('b') instanceof HTMLElement".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task UnnamedElementUsesThePrototypeOfHtmlElement()
        {
            var result = await "Object.getPrototypeOf(document.createElement('b')) === HTMLElement.prototype".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        //  Asserted without "instanceof" on purpose: a Symbol.hasInstance answer would hide a
        //  broken chain from every test above, but not from these.

        [Test]
        public async Task PrototypeOfElementIsThePrototypeOfItsConstructor()
        {
            var result = await "Object.getPrototypeOf(document.createElement('div')) === HTMLDivElement.prototype".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task PrototypeOfConstructorIsChainedToItsBase()
        {
            var result = await "Object.getPrototypeOf(HTMLDivElement.prototype) === HTMLElement.prototype".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task PrototypeOfWindowIsThePrototypeOfItsConstructor()
        {
            var result = await "Object.getPrototypeOf(window) === Window.prototype".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task ConstructorOfAnElementIsTheExposedConstructor()
        {
            var result = await "document.createElement('div').constructor === HTMLDivElement".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        //  Types reached only through a property, never constructed from script.

        [Test]
        public async Task TokenListIsAnInstanceOfDomTokenList()
        {
            var result = await "document.createElement('div').classList instanceof DOMTokenList".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task CollectionIsAnInstanceOfHtmlCollection()
        {
            var result = await "document.getElementsByTagName('div') instanceof HTMLCollection".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        //  A type constructed from script keeps working - these already did.

        [Test]
        public async Task ConstructedEventIsAnInstanceOfEvent()
        {
            var result = await "new Event('foo') instanceof Event".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task ConstructedCustomEventIsAnInstanceOfEvent()
        {
            var result = await "new CustomEvent('foo') instanceof Event".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task ImageIsAnInstanceOfHtmlImageElement()
        {
            var result = await "new Image() instanceof HTMLImageElement".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        //  A DOM constructor is a function, so it inherits from Function.prototype.

        [Test]
        public async Task DomConstructorIsAFunction()
        {
            var result = await "typeof HTMLDivElement".EvalScriptAsync();
            Assert.AreEqual("function", result);
        }

        [Test]
        public async Task DomConstructorIsAnInstanceOfFunction()
        {
            var result = await "Window instanceof Function".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task DomConstructorInheritsCallFromFunctionPrototype()
        {
            var result = await "typeof HTMLDivElement.call".EvalScriptAsync();
            Assert.AreEqual("function", result);
        }
    }
}
