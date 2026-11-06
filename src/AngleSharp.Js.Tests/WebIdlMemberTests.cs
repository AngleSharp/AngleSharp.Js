namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The members of the DOM are what WebIDL says they are, as pages rely on patching them.
    /// </summary>
    [TestFixture]
    public class WebIdlMemberTests
    {
        private static Task<String> Eval(String body) => $"(function () {{ {body} }})()".EvalScriptAsync();

        private static async Task<String> EvalInPage(String script)
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content($"<!doctype html><html><body><div id=d><span id=s></span></div><script>document.title = String((function () {{ {script} }})());</script></body></html>"));
            return document.Title;
        }

        [TestCase("Document.prototype", "querySelector")]
        [TestCase("Element.prototype", "querySelector")]
        [TestCase("Element.prototype", "setAttribute")]
        [TestCase("Node.prototype", "appendChild")]
        [TestCase("EventTarget.prototype", "addEventListener")]
        [TestCase("HTMLElement.prototype", "click")]
        public async Task OperationsAreWritableAndConfigurable(String prototype, String name)
        {
            var result = await EvalInPage($"var d = Object.getOwnPropertyDescriptor({prototype}, '{name}'); return d ? [typeof d.value, d.writable, d.configurable].join() : 'none';");

            Assert.AreEqual("function,true,true", result);
        }

        [TestCase("Node.prototype", "firstChild")]
        [TestCase("Element.prototype", "id")]
        public async Task AttributesAreConfigurableAccessors(String prototype, String name)
        {
            var result = await EvalInPage($"var d = Object.getOwnPropertyDescriptor({prototype}, '{name}'); return d ? [typeof d.get, d.configurable].join() : 'none';");

            Assert.AreEqual("function,true", result);
        }

        [Test]
        public async Task PatchingAMethodOfElementReachesEveryElement()
        {
            Assert.AreEqual("patched-s,patched-s,s", await EvalInPage(@"
                var original = Element.prototype.querySelector;
                Object.defineProperty(Element.prototype, 'querySelector', { value: function (selector) { return { id: 'patched-' + original.call(this, selector).id }; } });
                var viaDiv = document.getElementById('d').querySelector('span').id;
                var viaBody = document.body.querySelector('span').id;
                Object.defineProperty(Element.prototype, 'querySelector', { value: original });
                return [viaDiv, viaBody, document.body.querySelector('span').id].join();"));
        }

        [Test]
        public async Task AssigningAMethodOfEventTargetReachesEveryTarget()
        {
            Assert.AreEqual("1,1,1", await EvalInPage(@"
                var calls = { el: 0, doc: 0, win: 0 }, original = EventTarget.prototype.addEventListener;
                EventTarget.prototype.addEventListener = function (type, listener) {
                    if (this === window) { calls.win++; } else if (this === document) { calls.doc++; } else { calls.el++; }
                    return original.apply(this, arguments);
                };
                document.getElementById('d').addEventListener('x', function () {});
                document.addEventListener('x', function () {});
                window.addEventListener('x', function () {});
                EventTarget.prototype.addEventListener = original;
                return [calls.el, calls.doc, calls.win].join();"));
        }

        [Test]
        public async Task OverridingAnAccessorOnAPrototypeWorks()
        {
            Assert.AreEqual("wrapped-d,d", await EvalInPage(@"
                var original = Object.getOwnPropertyDescriptor(Element.prototype, 'id');
                Object.defineProperty(Element.prototype, 'id', { get: function () { return 'wrapped-' + original.get.call(this); }, set: original.set, configurable: true });
                var patched = document.getElementById('d').id;
                Object.defineProperty(Element.prototype, 'id', original);
                return [patched, document.getElementById('d').id].join();"));
        }

        [Test]
        public async Task ALibraryMethodOnAPrototypeIsFoundByItsElements()
        {
            Assert.AreEqual("mine,mine", await EvalInPage(@"
                Element.prototype.mine = function () { return 'mine'; };
                return [document.body.mine(), document.getElementById('d').mine()].join();"));
        }

        [Test]
        public async Task ASpecificElementTypeMayOverrideOnItsOwnPrototype()
        {
            Assert.AreEqual("div,x", await EvalInPage(@"
                HTMLDivElement.prototype.getAttribute = function () { return 'div'; };
                var div = document.getElementById('d').getAttribute('id');
                document.body.setAttribute('a', 'x');
                return [div, document.body.getAttribute('a')].join();"));
        }

        [Test]
        public async Task PrototypeChainIsTheOneOfTheStandard()
        {
            Assert.AreEqual("true,true,true,true", await EvalInPage(@"
                var div = document.getElementById('d');
                return [
                    Object.getPrototypeOf(div) === HTMLDivElement.prototype,
                    Object.getPrototypeOf(HTMLDivElement.prototype) === HTMLElement.prototype,
                    Object.getPrototypeOf(HTMLElement.prototype) === Element.prototype,
                    Object.getPrototypeOf(Node.prototype) === EventTarget.prototype
                ].join();"));
        }

        [Test]
        public async Task InterfaceObjectsCanBeReplacedOnTheWindow()
        {
            Assert.AreEqual("replaced,replaced", await EvalInPage(@"
                var original = window.HTMLElement;
                window.HTMLElement = 'replaced';
                var results = [window.HTMLElement, HTMLElement];
                window.HTMLElement = original;
                return results.join();"));
        }

        [Test]
        public async Task PopoverPolyfillPatternWorks()
        {
            Assert.AreEqual("s,s", await EvalInPage(@"
                function M(e, t, n) { var o = e[t]; Object.defineProperty(e, t, { value: function (e) { return o.call(this, n(e)); } }); }
                function fix(s) { return s && s.replace(':popover-open', '.popover-open'); }
                M(Document.prototype, 'querySelector', fix);
                M(Document.prototype, 'querySelectorAll', fix);
                M(Element.prototype, 'querySelector', fix);
                M(Element.prototype, 'querySelectorAll', fix);
                return [document.querySelector('span').id, document.getElementById('d').querySelectorAll('span')[0].id].join();"));
        }
    }
}
