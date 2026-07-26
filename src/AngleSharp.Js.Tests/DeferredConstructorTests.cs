namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    /// <summary>
    /// The constructor of an exposed type is only built once script reads the property it
    /// is published under, so these cover what a reader is entitled to see either way.
    /// </summary>
    [TestFixture]
    public class DeferredConstructorTests
    {
        [Test]
        public async Task ConstructorIsSameObjectOnWindowAndGlobal()
        {
            var result = await "String(window.HTMLDivElement === HTMLDivElement)".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructorIsSameObjectOnEveryRead()
        {
            var result = await "String((function () { var a = HTMLDivElement; var b = window.HTMLDivElement; return a === b && a === HTMLDivElement; })())".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructorIsFunction()
        {
            var result = await "typeof HTMLDivElement".EvalScriptAsync();
            Assert.AreEqual("function", result);
        }

        [Test]
        public async Task UnreadConstructorIsStillOwnPropertyOfWindow()
        {
            var result = await "String(window.hasOwnProperty('HTMLTableColElement'))".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task UnreadConstructorIsStillEnumerable()
        {
            var result = await "String(Object.keys(window).indexOf('HTMLTableColElement') !== -1)".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task UnreadConstructorIsStillFoundByForIn()
        {
            var result = await "String((function () { for (var k in window) { if (k === 'HTMLTableColElement') { return true; } } return false; })())".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructorKeepsItsAttributes()
        {
            var result = await "(function () { var d = Object.getOwnPropertyDescriptor(window, 'HTMLDivElement'); return d.writable + ',' + d.enumerable + ',' + d.configurable; })()".EvalScriptAsync();
            Assert.AreEqual("false,true,false", result);
        }

        [Test]
        public async Task DescriptorValueIsTheConstructor()
        {
            var result = await "String(Object.getOwnPropertyDescriptor(window, 'HTMLDivElement').value === HTMLDivElement)".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ReadingAConstructorDoesNotChangeTheKeysOfWindow()
        {
            var result = await "String((function () { var before = Object.keys(window).length; var c = HTMLDivElement; return before === Object.keys(window).length; })())".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructedInstanceIsInstanceOfItsConstructor()
        {
            var result = await "String(new CustomEvent('foo') instanceof CustomEvent)".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructorBuildsInstances()
        {
            var result = await "new CustomEvent('foo').type".EvalScriptAsync();
            Assert.AreEqual("foo", result);
        }

        [Test]
        public async Task PrototypePointsBackAtItsConstructor()
        {
            var result = await "String(HTMLDivElement.prototype.constructor === HTMLDivElement)".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        //  Reaching a prototype through an instance is the one path that never names the
        //  type, so it is the one that has to pull the constructor in by itself.
        [Test]
        public async Task InstanceReportsItsConstructorWhenTheNameWasNeverRead()
        {
            var result = await "screen.constructor.name".EvalScriptAsync();
            Assert.AreEqual("Screen", result);
        }

        [Test]
        public async Task InstanceReportsTheSameConstructorTheWindowPublishes()
        {
            var result = await "String(screen.constructor === Screen)".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructorIsNotWritable()
        {
            var result = await "String((function () { var before = HTMLDivElement; window.HTMLDivElement = 5; return window.HTMLDivElement === before; })())".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructorIsNotConfigurable()
        {
            var result = await "String((delete window.HTMLDivElement) === false && typeof HTMLDivElement === 'function')".EvalScriptAsync();
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ConstructorStringifiesAsNativeCode()
        {
            var result = await "String(HTMLDivElement)".EvalScriptAsync();
            Assert.AreEqual("function HTMLDivElement() { [native code] }", result);
        }

        [Test]
        public async Task NonConstructableTypeStillRejectsNew()
        {
            var result = await "(function () { try { new Node(); return 'no throw'; } catch (e) { return 'threw'; } })()".EvalScriptAsync();
            Assert.AreEqual("threw", result);
        }
    }
}
