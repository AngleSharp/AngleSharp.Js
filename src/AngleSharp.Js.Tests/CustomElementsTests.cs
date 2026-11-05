namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Custom elements as scripts see them: classes deriving from the elements of the DOM.
    /// </summary>
    [TestFixture]
    public class CustomElementsTests
    {
        private static Task<IDocument> OpenAsync(String html)
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            return BrowsingContext.New(config).OpenAsync(m => m.Address("https://example.com/").Content($"<!doctype html><html><body>{html}</body></html>"));
        }

        private static async Task<String> Eval(String script, String body = "")
        {
            var document = await OpenAsync($"{body}<script>document.title = String((function () {{ {script} }})());</script>");
            return document.Title;
        }

        [Test]
        public async Task RegistryIsOnTheWindow()
        {
            Assert.AreEqual("object,true,function", await Eval("return [typeof customElements, window.customElements === customElements, typeof customElements.define].join();"));
        }

        [Test]
        public async Task ClassIsDefinedAndFound()
        {
            Assert.AreEqual("true,my-el,true", await Eval(@"
                class MyEl extends HTMLElement {}
                customElements.define('my-el', MyEl);
                return [customElements.get('my-el') === MyEl, customElements.getName(MyEl), customElements.get('other-el') === undefined].join();"));
        }

        [Test]
        public async Task CreatedElementIsAnInstanceOfItsClass()
        {
            Assert.AreEqual("true,true,true,MY-EL,hello", await Eval(@"
                class MyEl extends HTMLElement {
                    constructor() { super(); this.greeting = 'hello'; }
                    greet() { return this.greeting; }
                }
                customElements.define('my-el', MyEl);
                var el = document.createElement('my-el');
                return [el instanceof MyEl, el instanceof HTMLElement, el instanceof Element, el.tagName, el.greet()].join();"));
        }

        [Test]
        public async Task ElementCreatedWithNewIsTheElementOfTheDefinition()
        {
            Assert.AreEqual("MY-EL,true,x", await Eval(@"
                class MyEl extends HTMLElement {}
                customElements.define('my-el', MyEl);
                var el = new MyEl();
                el.setAttribute('a', 'x');
                document.body.appendChild(el);
                return [el.tagName, el instanceof MyEl, document.querySelector('my-el').getAttribute('a')].join();"));
        }

        [Test]
        public async Task WrapperStaysTheSameObject()
        {
            Assert.AreEqual("true", await Eval(@"
                class MyEl extends HTMLElement {}
                customElements.define('my-el', MyEl);
                var el = new MyEl();
                document.body.appendChild(el);
                return document.querySelector('my-el') === el;"));
        }

        [Test]
        public async Task ExistingElementsAreUpgradedWhenDefined()
        {
            Assert.AreEqual("false,true,upgraded", await Eval(@"
                var el = document.getElementById('a');
                var before = el instanceof HTMLUnknownElement && el.state === 'upgraded';
                class MyEl extends HTMLElement { constructor() { super(); this.state = 'upgraded'; } }
                customElements.define('my-el', MyEl);
                return [before, el instanceof MyEl, el.state].join();", "<my-el id=a></my-el>"));
        }

        [Test]
        public async Task ParsedElementsAfterTheDefinitionAreUpgraded()
        {
            var document = await OpenAsync(@"<script>
                var log = [];
                class MyEl extends HTMLElement {
                    constructor() { super(); log.push('constructed ' + this.id); }
                    connectedCallback() { log.push('connected ' + this.id + ' ' + this.isConnected); }
                }
                customElements.define('my-el', MyEl);
                </script><my-el id=a></my-el><script>document.title = log.join();</script>");

            Assert.AreEqual("constructed a,connected a true", document.Title);
        }

        [Test]
        public async Task CallbacksAreCalledWithTheElementAsThis()
        {
            Assert.AreEqual("connected:true,attr:color::red,disconnected:true", await Eval(@"
                var log = [];
                class MyEl extends HTMLElement {
                    static get observedAttributes() { return ['color']; }
                    connectedCallback() { log.push('connected:' + (this instanceof MyEl)); }
                    disconnectedCallback() { log.push('disconnected:' + (this instanceof MyEl)); }
                    attributeChangedCallback(name, oldValue, newValue) { log.push('attr:' + name + ':' + (oldValue === null ? '' : oldValue) + ':' + newValue); }
                }
                customElements.define('my-el', MyEl);
                var el = document.createElement('my-el');
                document.body.appendChild(el);
                el.setAttribute('color', 'red');
                el.setAttribute('other', 'x');
                el.remove();
                return log.join();"));
        }

        [Test]
        public async Task ObservedAttributesMayBeAStaticField()
        {
            Assert.AreEqual("a", await Eval(@"
                var log = [];
                class MyEl extends HTMLElement {
                    static observedAttributes = ['a'];
                    attributeChangedCallback(name) { log.push(name); }
                }
                customElements.define('my-el', MyEl);
                var el = document.createElement('my-el');
                el.setAttribute('b', '1');
                el.setAttribute('a', '1');
                return log.join();"));
        }

        [Test]
        public async Task ClassCanExtendACustomElementClass()
        {
            Assert.AreEqual("base,true,true", await Eval(@"
                class Base extends HTMLElement { who() { return 'base'; } }
                class Derived extends Base {}
                customElements.define('my-derived', Derived);
                var el = new Derived();
                return [el.who(), el instanceof Base, el instanceof Derived].join();"));
        }

        [Test]
        public async Task ClassCanExtendABuiltInElement()
        {
            Assert.AreEqual("BUTTON,true,true,my-button", await Eval(@"
                class MyButton extends HTMLButtonElement {}
                customElements.define('my-button', MyButton, { extends: 'button' });
                var el = new MyButton();
                return [el.tagName, el instanceof MyButton, el instanceof HTMLButtonElement, el.outerHTML.indexOf('is=""my-button""') > 0 ? 'my-button' : el.outerHTML].join();"));
        }

        [Test]
        public async Task ElementCanHaveAShadowRoot()
        {
            Assert.AreEqual("inner", await Eval(@"
                class MyEl extends HTMLElement {
                    constructor() { super(); this.attachShadow({ mode: 'open' }).innerHTML = '<span>inner</span>'; }
                }
                customElements.define('my-el', MyEl);
                return new MyEl().shadowRoot.querySelector('span').textContent;"));
        }

        [Test]
        public async Task ClassNotDefinedCannotBeCreated()
        {
            Assert.AreEqual("TypeError", await Eval(@"
                class MyEl extends HTMLElement {}
                try { new MyEl(); return 'no error'; } catch (e) { return e.name; }"));
        }

        [Test]
        public async Task HtmlElementItselfCannotBeCreated()
        {
            Assert.AreEqual("TypeError", await Eval("try { new HTMLElement(); return 'no error'; } catch (e) { return e.name; }"));
        }

        [TestCase("customElements.define('nohyphen', class extends HTMLElement {})", "SyntaxError")]
        [TestCase("customElements.define('my-el', class extends HTMLElement {}); customElements.define('my-el', class extends HTMLElement {})", "NotSupportedError")]
        [TestCase("customElements.define('my-el', function () {}.bind(null))", "TypeError")]
        [TestCase("customElements.define('my-el', 42)", "TypeError")]
        [TestCase("customElements.define('my-el', class extends HTMLElement { get connectedCallback() { return 1; } })", "TypeError")]
        public async Task WhatCannotBeDefinedIsRefused(String statement, String expected)
        {
            Assert.AreEqual(expected, await Eval($"try {{ {statement}; return 'no error'; }} catch (e) {{ return e.name; }}"));
        }

        [Test]
        public async Task WhenDefinedGivesTheClass()
        {
            var document = await OpenAsync(@"<script>
                class MyEl extends HTMLElement {}
                customElements.whenDefined('my-el').then(function (type) { document.title = String(type === MyEl); });
                customElements.define('my-el', MyEl);
                </script>");

            for (var i = 0; i < 200 && document.Title.Length == 0; i++)
            {
                await Task.Delay(10);
            }

            Assert.AreEqual("true", document.Title);
        }

        [Test]
        public async Task ConstructorThrowingIsReportedAndLeavesTheElement()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(m => m.Content(@"<!doctype html><my-el id=a></my-el><script>
                class MyEl extends HTMLElement { constructor() { super(); throw new RangeError('broken'); } connectedCallback() { document.title = 'wrong'; } }
                customElements.define('my-el', MyEl);
                document.title = String(document.querySelector('my-el:defined'));
                </script>"));

            Assert.AreEqual("null", document.Title);
            Assert.AreEqual(1, errors.Count);
            StringAssert.Contains("broken", errors[0].Message);
        }

        [Test]
        public async Task CallbackThrowingIsReported()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(m => m.Content(@"<!doctype html><body><script>
                class MyEl extends HTMLElement { connectedCallback() { throw new RangeError('broken'); } }
                customElements.define('my-el', MyEl);
                document.body.appendChild(new MyEl());
                document.title = 'went on';
                </script>"));

            Assert.AreEqual("went on", document.Title);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public async Task ConstructorNotCallingSuperIsReported()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            await context.OpenAsync(m => m.Content(@"<!doctype html><my-el></my-el><script>
                class MyEl extends HTMLElement { constructor() { } }
                customElements.define('my-el', MyEl);
                </script>"));

            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public async Task ConstructorMayCreateOtherCustomElements()
        {
            Assert.AreEqual("MY-INNER,true", await Eval(@"
                class Inner extends HTMLElement {}
                class Outer extends HTMLElement { constructor() { super(); this.inner = new Inner(); } }
                customElements.define('my-inner', Inner);
                customElements.define('my-outer', Outer);
                var outer = new Outer();
                return [outer.inner.tagName, outer.inner instanceof Inner].join();"));
        }

        [Test]
        public async Task ReflectConstructWorksAsForOldClasses()
        {
            Assert.AreEqual("true", await Eval(@"
                function MyEl() { return Reflect.construct(HTMLElement, [], MyEl); }
                MyEl.prototype = Object.create(HTMLElement.prototype);
                MyEl.prototype.constructor = MyEl;
                customElements.define('my-el', MyEl);
                return document.createElement('my-el') instanceof MyEl;"));
        }

        [Test]
        public async Task DefinedPseudoClassFollowsTheDefinition()
        {
            Assert.AreEqual("0,1", await Eval(@"
                var before = document.querySelectorAll('my-el:defined').length;
                customElements.define('my-el', class extends HTMLElement {});
                return [before, document.querySelectorAll('my-el:defined').length].join();", "<my-el></my-el>"));
        }

        [Test]
        public async Task CallbacksSeeWhatTheCallOfTheScriptDidCompletely()
        {
            Assert.AreEqual("a:my-b,b:", await Eval(@"
                var log = [];
                class Item extends HTMLElement {
                    connectedCallback() { log.push(this.id + ':' + (this.nextElementSibling ? this.nextElementSibling.localName : '')); }
                }
                customElements.define('my-a', class extends Item {});
                customElements.define('my-b', class extends Item {});
                var parent = document.createElement('div');
                document.body.appendChild(parent);
                parent.innerHTML = '<my-a id=a></my-a><my-b id=b></my-b>';
                return log.join();"));
        }

        [Test]
        public async Task CallbacksAreDoneWhenTheCallOfTheScriptReturns()
        {
            Assert.AreEqual("setting,changed,set", await Eval(@"
                var log = [];
                class MyEl extends HTMLElement {
                    static observedAttributes = ['a'];
                    attributeChangedCallback() { log.push('changed'); }
                }
                customElements.define('my-el', MyEl);
                var el = new MyEl();
                log.push('setting');
                el.setAttribute('a', '1');
                log.push('set');
                return log.join();"));
        }

        [Test]
        public async Task AppendTellsAfterAllOfItIsIn()
        {
            Assert.AreEqual("2,2", await Eval(@"
                var log = [];
                class MyEl extends HTMLElement { connectedCallback() { log.push(this.parentNode.childElementCount); } }
                customElements.define('my-el', MyEl);
                var parent = document.createElement('div');
                document.body.appendChild(parent);
                parent.append(new MyEl(), new MyEl());
                return log.join();"));
        }

        [Test]
        public async Task CallbackChangingTheTreeWorksInTheMiddleOfAnotherCall()
        {
            Assert.AreEqual("1", await Eval(@"
                class MyEl extends HTMLElement {
                    connectedCallback() {
                        var inner = document.createElement('span');
                        this.appendChild(inner);
                        inner.remove();
                        this.appendChild(document.createElement('b'));
                    }
                }
                customElements.define('my-el', MyEl);
                var el = new MyEl();
                document.body.appendChild(el);
                return el.childElementCount;"));
        }

        [Test]
        public async Task CreateElementTakesWhatItIs()
        {
            Assert.AreEqual("true,true,true", await Eval(@"
                class MyButton extends HTMLButtonElement {}
                customElements.define('my-button', MyButton, { extends: 'button' });
                var el = document.createElement('button', { is: 'my-button' });
                var plain = document.createElement('button');
                return [el instanceof MyButton, el.getAttribute('is') === null, !(plain instanceof MyButton)].join();"));
        }

        [Test]
        public async Task CreateElementKeepsWorkingWithoutOptions()
        {
            Assert.AreEqual("DIV,DIV", await Eval(@"
                return [document.createElement('div').tagName, document.createElement('div', {}).tagName].join();"));
        }

        [Test]
        public async Task ClonedElementIsAnInstanceOfItsClass()
        {
            Assert.AreEqual("true,false,constructed", await Eval(@"
                class MyEl extends HTMLElement { constructor() { super(); this.state = 'constructed'; } }
                customElements.define('my-el', MyEl);
                var el = new MyEl();
                var clone = el.cloneNode();
                return [clone instanceof MyEl, clone === el, clone.state].join();"));
        }

        [Test]
        public async Task AdoptedCallbackGetsTheDocuments()
        {
            Assert.AreEqual("true,true", await Eval(@"
                var log = [];
                class MyEl extends HTMLElement { adoptedCallback(oldDocument, newDocument) { log.push(oldDocument === document, newDocument === other); } }
                customElements.define('my-el', MyEl);
                var other = document.implementation.createHTMLDocument('other');
                var el = new MyEl();
                other.body.appendChild(el);
                return log.join();"));
        }

        [Test]
        public async Task ThrowingMemberStillRunsTheReactionsOfItsCall()
        {
            Assert.AreEqual("connected,caught", await Eval(@"
                var log = [];
                class MyEl extends HTMLElement { connectedCallback() { log.push('connected'); } }
                customElements.define('my-el', MyEl);
                var el = new MyEl();
                document.body.appendChild(el);
                try { document.body.appendChild(document.documentElement); } catch (e) { log.push('caught'); }
                return log.join();"));
        }

        [Test]
        public async Task OtherElementsAreNoInstancesOfAClass()
        {
            Assert.AreEqual("true,false,false,true,true", await Eval(@"
                class Base extends HTMLElement {}
                class Derived extends Base {}
                class Other extends HTMLElement {}
                customElements.define('my-derived', Derived);
                customElements.define('my-other', Other);
                var el = new Derived();
                var div = document.createElement('div');
                return [el instanceof Base, el instanceof Other, div instanceof Base, div instanceof HTMLElement, el instanceof HTMLElement].join();"));
        }

        [Test]
        public async Task NodesCanBeReplacedWith()
        {
            Assert.AreEqual("B", await Eval(@"
                var parent = document.createElement('div');
                var a = parent.appendChild(document.createElement('a'));
                a.replaceWith(document.createElement('b'));
                return parent.firstChild.tagName;"));
        }

        [Test]
        public async Task DomExceptionsOfTheRegistryAreRealOnes()
        {
            Assert.AreEqual("true", await Eval("try { customElements.define('x', class extends HTMLElement {}); } catch (e) { return e instanceof DOMException; }"));
        }
    }
}
