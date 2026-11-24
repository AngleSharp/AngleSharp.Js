namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom;
    using AngleSharp.Scripting;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class DomRegistrationTests
    {
        public sealed class Thing
        {
            public Int32 Value => 42;
        }

        public class ProbeBase
        {
        }

        public sealed class ProbeDerived : ProbeBase
        {
        }

        [DomName("Oops")]
        public sealed class Oops : Exception
        {
            public Oops(String message)
                : base(message)
            {
            }
        }

        private static void RegisterDom(DomRegistry registry)
        {
            registry.Type<IWindow>("Window")
                .Property("document", window => window.Document);

            registry.Mixin<IParentNode>("ParentNode")
                .Property("childElementCount", node => node.ChildElementCount);

            registry.Type<INode>("Node")
                .Constant("ELEMENT_NODE", 1)
                .Property("textContent", node => node.TextContent, (node, value) => node.TextContent = value)
                .Property("childNodes", node => node.ChildNodes, flags: DomPropertyFlags.SameObject)
                .Method("forEachChild", (node, args) =>
                {
                    var callback = args.GetCallback(0);

                    foreach (var child in node.ChildNodes)
                    {
                        callback?.Invoke(child);
                    }
                }, length: 1)
                .Method("kind", (node, args) => "number", new DomSignature(typeof(Double)))
                .Method("kind", (node, args) => "string", new DomSignature(typeof(String)))
                .Method("kind", (node, args) => "none", new DomSignature());

            registry.Type<IElement>("Element")
                .Inherits<INode>()
                .Includes<IParentNode>()
                .Alias("Elem")
                .Method("getAttribute", (element, args) => element.GetAttribute(args.Get<String>(0)), length: 1)
                .ForwardingProperty("self", element => element, "textContent")
                .EventHandler("onping", (element, handler) => element.AddEventListener("ping", handler), (element, handler) => element.RemoveEventListener("ping", handler))
                .Static("twice", args => args.Get<Double>(0) * 2, length: 1)
                .StaticProperty("answer", () => 42);

            registry.Type<IHtmlElement>("HTMLElement")
                .Inherits<IElement>()
                .Property("dataset", element => element.Dataset, flags: DomPropertyFlags.SameObject);

            registry.Type<IDocument>("Document")
                .Inherits<INode>()
                .Includes<IParentNode>()
                .Property("readyState", document => document.ReadyState)
                .Method("isState", (document, args) => args.Get<DocumentReadyState>(0) == document.ReadyState, length: 1)
                .Method("querySelector", (document, args) => document.QuerySelector(args.Get<String>(0)), length: 1);

            registry.Type<INodeList>("NodeList")
                .Collection(list => list.Length, (list, index) => list[index]);

            registry.Type<IStringMap>("DOMStringMap")
                .NamedIndexer<String>((map, name) => map[name], (map, name, value) => map[name] = value);

            registry.Type<Event>("Event")
                .Constructor(args => new Event(args.Get<String>(0)), length: 1)
                .Property("type", ev => ev.Type);

            registry.Type<Thing>("Thing")
                .Property("value", thing => thing.Value);

            registry.Literals<DocumentReadyState>()
                .Literal(DocumentReadyState.Loading, "loading")
                .Literal(DocumentReadyState.Interactive, "interactive")
                .Literal(DocumentReadyState.Complete, "complete");

            registry.Constants("NodeFilter")
                .Constant("SHOW_ALL", 0xFFFFFFFF);

            registry.ConstructorFunction<IElement>("Para", args => args.Window.Document.CreateElement("p"));
            registry.Instance("thing", window => new Thing());
        }

        private static async Task<(JsScriptingService Service, IDocument Document)> OpenAsync(DomRegistration registration, String html)
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs(registration));
            var document = await context.OpenAsync(m => m.Content(html));
            return (context.GetService<JsScriptingService>(), document);
        }

        private static async Task<Object> EvalAsync(String html, String source)
        {
            var (service, document) = await OpenAsync(RegisterDom, html);
            return Eval(service, document, source);
        }

        private static Object Eval(JsScriptingService service, IDocument document, String source) =>
            service.EvaluateScript(document, source, "text/javascript", null);

        [Test]
        public async Task RegisteredMembersAreReachableFromScript()
        {
            Assert.AreEqual("Hello", await EvalAsync("<p id=a>Hello</p>", "document.querySelector('p').textContent"));
            Assert.AreEqual("a", await EvalAsync("<p id=a>Hello</p>", "document.querySelector('p').getAttribute('id')"));
        }

        [Test]
        public async Task RegisteredSetterWritesThroughToTheDom()
        {
            var (service, document) = await OpenAsync(RegisterDom, "<p>Hello</p>");
            Eval(service, document, "document.querySelector('p').textContent = 'Bye'");
            Assert.AreEqual("Bye", document.QuerySelector("p").TextContent);
        }

        [Test]
        public async Task OnlyTheCoreDomAndTheRegistrationAreExposed()
        {
            //  AngleSharp.Css is a service of the context, which reflection would expose; a
            //  registration naming it not leaves it out
            var context = BrowsingContext.New(Configuration.Default.WithCss().WithJs(RegisterDom));
            var document = await context.OpenAsync(m => m.Content("<p>Hello</p>"));
            var service = context.GetService<JsScriptingService>();
            Assert.AreEqual("undefined", Eval(service, document, "typeof CSSStyleSheet"));
            Assert.AreEqual("function", Eval(service, document, "typeof document.createElement"));
            Assert.AreEqual("function", Eval(service, document, "typeof HTMLDivElement"));
            Assert.AreEqual(42.0, Eval(service, document, "thing.value"));
        }

        [Test]
        public async Task PrototypesFollowTheRegisteredInheritance()
        {
            Assert.AreEqual("[object HTMLElement]", await EvalAsync("<b>Hello</b>", "Object.prototype.toString.call(document.querySelector('b'))"));
            Assert.AreEqual("[object HTMLParagraphElement]", await EvalAsync("<p>Hello</p>", "Object.prototype.toString.call(document.querySelector('p'))"));
            Assert.AreEqual(true, await EvalAsync("<p>Hello</p>", "Object.getPrototypeOf(HTMLElement.prototype) === Element.prototype"));
            Assert.AreEqual(true, await EvalAsync("<p>Hello</p>", "document.querySelector('p') instanceof Node"));
        }

        [Test]
        public async Task InterfaceObjectsArePublishedUnderTheirNamesAndAliases()
        {
            Assert.AreEqual("function", await EvalAsync("", "typeof Element"));
            Assert.AreEqual(true, await EvalAsync("", "Elem === Element"));
            Assert.AreEqual(true, await EvalAsync("<p></p>", "document.querySelector('p').constructor === HTMLParagraphElement"));
        }

        [Test]
        public async Task ConstructorsCreateRegisteredObjects()
        {
            Assert.AreEqual("ping", await EvalAsync("", "new Event('ping').type"));
            Assert.AreEqual(true, await EvalAsync("", "new Event('ping') instanceof Event"));
            Assert.AreEqual(1.0, await EvalAsync("", "Event.length"));
        }

        [Test]
        public async Task TypesWithoutConstructorAreIllegalToConstruct()
        {
            Assert.AreEqual("TypeError", await EvalAsync("", "try { new Element(); } catch (e) { e.name }"));
        }

        [Test]
        public async Task OverloadsArePickedByTheArguments()
        {
            Assert.AreEqual("number", await EvalAsync("<p></p>", "document.querySelector('p').kind(1)"));
            Assert.AreEqual("string", await EvalAsync("<p></p>", "document.querySelector('p').kind('a')"));
            Assert.AreEqual("none", await EvalAsync("<p></p>", "document.querySelector('p').kind()"));
        }

        [Test]
        public async Task ConstantsAreOnInterfaceObjectAndPrototype()
        {
            Assert.AreEqual(1.0, await EvalAsync("<p></p>", "Node.ELEMENT_NODE"));
            Assert.AreEqual(1.0, await EvalAsync("<p></p>", "document.querySelector('p').ELEMENT_NODE"));
            Assert.AreEqual((Double)0xFFFFFFFF, await EvalAsync("", "NodeFilter.SHOW_ALL"));
        }

        [Test]
        public async Task StaticMembersAreOnTheInterfaceObject()
        {
            Assert.AreEqual(8.0, await EvalAsync("", "Element.twice(4)"));
            Assert.AreEqual(42.0, await EvalAsync("", "Element.answer"));
            Assert.AreEqual("undefined", await EvalAsync("<p></p>", "typeof document.querySelector('p').twice"));
        }

        [Test]
        public async Task MixinMembersAreOnTheIncludingTypes()
        {
            Assert.AreEqual(2.0, await EvalAsync("<div><b></b><i></i></div>", "document.querySelector('div').childElementCount"));
            Assert.AreEqual(1.0, await EvalAsync("", "document.childElementCount"));
            Assert.AreEqual("[object HTMLDocument]", await EvalAsync("", "Object.prototype.toString.call(document)"));
        }

        [Test]
        public async Task CollectionsAreArrayLike()
        {
            const String html = "<div><b>1</b><i>2</i></div>";
            Assert.AreEqual(2.0, await EvalAsync(html, "document.querySelector('div').childNodes.length"));
            Assert.AreEqual("2", await EvalAsync(html, "document.querySelector('div').childNodes[1].textContent"));
            Assert.AreEqual("12", await EvalAsync(html, "[...document.querySelector('div').childNodes].map(n => n.textContent).join('')"));
            Assert.AreEqual(true, await EvalAsync(html, "document.querySelector('div').childNodes === document.querySelector('div').childNodes"));
        }

        [Test]
        public async Task NamedIndexerReadsAndWritesTheDataset()
        {
            var (service, document) = await OpenAsync(RegisterDom, "<p data-foo-bar=x></p>");
            Assert.AreEqual("x", Eval(service, document, "document.querySelector('p').dataset.fooBar"));
            Eval(service, document, "document.querySelector('p').dataset.otherValue = 'y'");
            Assert.AreEqual("y", document.QuerySelector("p").GetAttribute("data-other-value"));
        }

        [Test]
        public async Task EventHandlersSubscribeAndUnsubscribe()
        {
            var (service, document) = await OpenAsync(RegisterDom, "<p>0</p>");
            var p = document.QuerySelector("p");
            Eval(service, document, "var p = document.querySelector('p'); p.onping = function () { p.textContent = 'pinged'; }");
            p.Dispatch(new Event("ping"));
            Assert.AreEqual("pinged", p.TextContent);
            Eval(service, document, "p.textContent = '0'; p.onping = null");
            p.Dispatch(new Event("ping"));
            Assert.AreEqual("0", p.TextContent);
        }

        [Test]
        public async Task PutForwardsAssignsTheAttributeOfTheValue()
        {
            var (service, document) = await OpenAsync(RegisterDom, "<p>0</p>");
            Eval(service, document, "document.querySelector('p').self = 'forwarded'");
            Assert.AreEqual("forwarded", document.QuerySelector("p").TextContent);
        }

        [Test]
        public async Task LiteralsAreStringsBothWays()
        {
            Assert.AreEqual("complete", await EvalAsync("", "document.readyState"));
            Assert.AreEqual(true, await EvalAsync("", "document.isState('complete')"));
            Assert.AreEqual(false, await EvalAsync("", "document.isState('loading')"));
        }

        [Test]
        public async Task ConstructorFunctionsAndInstancesArePublished()
        {
            Assert.AreEqual("[object HTMLParagraphElement]", await EvalAsync("", "Object.prototype.toString.call(new Para())"));
            Assert.AreEqual(true, await EvalAsync("", "new Para() instanceof Element"));
            Assert.AreEqual(0.0, await EvalAsync("", "Para.length"));
            Assert.AreEqual(42.0, await EvalAsync("", "thing.value"));
            Assert.AreEqual(true, await EvalAsync("", "thing instanceof Thing"));
        }

        [Test]
        public async Task NodeIdentityIsPreserved()
        {
            Assert.AreEqual(true, await EvalAsync("<p>Hello</p>", "document.querySelector('p') === document.querySelector('p')"));
        }

        [Test]
        public async Task CallbacksArePassedWithoutCompiledDelegates()
        {
            Assert.AreEqual("12", await EvalAsync("<div><b>1</b><i>2</i></div>",
                "var s = ''; document.querySelector('div').forEachChild(function (c) { s += c.textContent; }); s"));
        }

        [Test]
        public async Task ClrErrorsBecomeScriptErrors()
        {
            DomRegistration registration = registry => registry
                .Type<IWindow>("Window")
                .Method("fail", (window, args) => throw new InvalidOperationException("nope"));
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual("nope", Eval(service, document, "try { fail(); } catch (e) { e.message }"));
        }

        [Test]
        public async Task RegisteredExceptionsAreErrorsOfTheirClass()
        {
            DomRegistration registration = registry =>
            {
                registry.Type<IWindow>("Window").Method("fail", (window, args) => throw new Oops("nope"));
                registry.Type<Oops>("Oops");
            };
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual("true|true|nope", Eval(service, document, "try { fail(); } catch (e) { (e instanceof Oops) + '|' + (e instanceof Error) + '|' + e.message }"));
        }

        [Test]
        public async Task UnregisteredDomExceptionsArePlainErrors()
        {
            DomRegistration registration = registry => registry
                .Type<IWindow>("Window")
                .Method("fail", (window, args) => throw new Oops("nope"));
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual("true|nope", Eval(service, document, "try { fail(); } catch (e) { (e instanceof Error) + '|' + e.message }"));
        }

        [Test]
        public async Task RegisteredDelegatesAreFunctions()
        {
            DomRegistration registration = registry => registry
                .Type<IWindow>("Window")
                .Property("twice", window => (Func<Double, Double>)(x => x * 2));
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual(8.0, Eval(service, document, "twice(4)"));
        }

        [Test]
        public async Task RegistrationRunsOncePerServiceAndIsCombinable()
        {
            var calls = 0;
            var registration = DomRegistrations.Combine(RegisterDom, registry =>
            {
                calls++;
                registry.Type<IElement>("Element").Property("tag", element => element.LocalName);
            });
            var context = BrowsingContext.New(Configuration.Default.WithJs(registration));
            var service = context.GetService<JsScriptingService>();
            var first = await context.OpenAsync(m => m.Content("<p>1</p>"));
            Assert.AreEqual("p", Eval(service, first, "document.querySelector('p').tag"));
            var second = await context.OpenAsync(m => m.Content("<b>2</b>"));
            Assert.AreEqual("b", Eval(service, second, "document.querySelector('b').tag"));
            Assert.AreEqual(1, calls);
        }

        [Test]
        public async Task RequiredArgumentsConvertUndefinedOptionalOnesFallBack()
        {
            DomRegistration registration = registry => registry
                .Type<IWindow>("Window")
                .Method("required", (window, args) => args.Get<String>(0) ?? "<none>", length: 1)
                .Method("optional", (window, args) => args.Get<String>(0, "fallback"));
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual("undefined", Eval(service, document, "required(undefined)"));
            Assert.AreEqual("<none>", Eval(service, document, "required()"));
            Assert.AreEqual("fallback", Eval(service, document, "optional(undefined)"));
            Assert.AreEqual("fallback", Eval(service, document, "optional()"));
            Assert.AreEqual("1", Eval(service, document, "optional(1)"));
        }

        [Test]
        public async Task DictionaryMembersAreReadByName()
        {
            DomRegistration registration = registry => registry
                .Type<IWindow>("Window")
                .Method("dict", (window, args) => args.GetMember<String>(0, "name", "none") + ":" + args.GetMember<Double>(0, "count", -1.0));
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual("a:2", Eval(service, document, "dict({ name: 'a', count: '2' })"));
            Assert.AreEqual("none:-1", Eval(service, document, "dict({ name: undefined })"));
            Assert.AreEqual("none:-1", Eval(service, document, "dict(5)"));
            Assert.AreEqual("none:-1", Eval(service, document, "dict()"));
        }

        [Test]
        public async Task CallbackResultsAreConverted()
        {
            DomRegistration registration = registry => registry
                .Type<IWindow>("Window")
                .Method("check", (window, args) => args.GetCallback(0)?.Invoke<Int32>(3) ?? -1, length: 1);
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual(7.0, Eval(service, document, "check(function (x) { return '' + (x + 4); })"));
        }

        [Test]
        public async Task TypesOfTheSameNameShareTheInterfaceObject()
        {
            DomRegistration registration = registry =>
            {
                registry.Type<IWindow>("Window")
                    .Property("elements", window => window.Document.Body.Children)
                    .Property("images", window => window.Document.Images);
                registry.Type<IHtmlCollection<IElement>>("HTMLCollection")
                    .Property("length", list => list.Length)
                    .Constant("ANSWER", 42);
                registry.Type<IHtmlCollection<IHtmlImageElement>>("HTMLCollection")
                    .Property("length", list => list.Length);
            };
            var (service, document) = await OpenAsync(registration, "<img><img>");
            Assert.AreEqual("true|true|true|2|2|42", Eval(service, document,
                "[elements instanceof HTMLCollection, images instanceof HTMLCollection, Object.getPrototypeOf(images).constructor === HTMLCollection, elements.length, images.length, images.ANSWER].join('|')"));
        }

        [Test]
        public async Task NamespacesArePlainObjectsHoldingTheirMembers()
        {
            DomRegistration registration = registry =>
            {
                RegisterDom(registry);
                registry.Namespace("Maths")
                    .Static("twice", args => args.Get<Double>(0) * 2, length: 1)
                    .StaticProperty("answer", () => 42)
                    .Constant("ONE", 1);
                registry.Type<Thing>("Thing").InNamespace("Maths");
            };
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual("8|42|1|function|undefined|[object Maths]|true", Eval(service, document,
                "[Maths.twice(4), Maths.answer, Maths.ONE, typeof Maths.Thing, typeof Thing, Object.prototype.toString.call(Maths), thing instanceof Maths.Thing].join('|')"));
        }

        [Test]
        public async Task TypeWithoutInterfaceObjectSharesNoneOfItsName()
        {
            //  an unrelated type of another library named like the Location of AngleSharp
            DomRegistration registration = registry =>
            {
                registry.Type<Thing>("Location").WithoutInterfaceObject().Property("value", thing => thing.Value);
                registry.Instance("thing", window => new Thing());
            };
            var (service, document) = await OpenAsync(registration, "");
            Assert.AreEqual("function|42|false|true", Eval(service, document,
                "[typeof Location, thing.value, thing instanceof Location, location instanceof Location].join('|')"));
        }

        [Test]
        public void TypeInAnUnregisteredNamespaceIsRejected() =>
            AssertRejected(registry => registry.Type<Thing>("Thing").InNamespace("Nowhere"));

        private static void RegisterPageDom(DomRegistry registry)
        {
            registry.Type<IWindow>("Window")
                .Property("document", window => window.Document)
                .Property("customElements", window => window.CustomElements());

            registry.Type<CustomElementRegistry>("CustomElementRegistry")
                .Method("define", (customElements, args) => customElements.Define(args.Get<String>(0), args.Get<Object>(1), null), length: 2);

            registry.Type<INode>("Node")
                .Method("appendChild", (node, args) => node.AppendChild(args.Get<INode>(0)), length: 1);

            registry.Type<IElement>("Element")
                .Inherits<INode>()
                .Property("id", element => element.Id)
                .Property("localName", element => element.LocalName)
                .Property("nextElementSibling", element => element.NextElementSibling)
                .Property("innerHTML", element => element.InnerHtml, (element, value) => element.InnerHtml = value)
                .Method("remove", (element, args) => element.Remove());

            registry.Type<IHtmlElement>("HTMLElement")
                .Inherits<IElement>();

            registry.Type<IHtmlScriptElement>("HTMLScriptElement")
                .Inherits<IHtmlElement>()
                .Property("text", script => script.Text, (script, value) => script.Text = value);

            registry.Type<IDocument>("Document")
                .Inherits<INode>()
                .Property("documentElement", document => document.DocumentElement)
                .Property("head", document => document.Head)
                .Property("body", document => document.Body)
                .Method("createElement", (document, args) => document.CreateElement(args.Get<String>(0)), length: 1);
        }

        private static async Task<Object> RunPageAsync(String script, String result)
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs(RegisterPageDom).WithEventLoop());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><body><script>" + script + "</script>"));
            return Eval(context.GetService<JsScriptingService>(), document, result);
        }

        [Test]
        public async Task InsertedScriptRunsBeforeAppendChildReturns()
        {
            Assert.AreEqual("1,2,3", await RunPageAsync(
                "var order = []; var s = document.createElement('script'); s.text = 'order.push(1)'; document.head.appendChild(s); order.push(2); s.remove(); document.body.appendChild(s); order.push(3);",
                "order.join()"));
        }

        [Test]
        public async Task CallbacksSeeWhatTheCallOfTheScriptDidCompletely()
        {
            Assert.AreEqual("a:my-b,b:", await RunPageAsync(@"
                var log = [];
                class Item extends HTMLElement {
                    connectedCallback() { log.push(this.id + ':' + (this.nextElementSibling ? this.nextElementSibling.localName : '')); }
                }
                customElements.define('my-a', class extends Item {});
                customElements.define('my-b', class extends Item {});
                var host = document.createElement('div');
                document.body.appendChild(host);
                host.innerHTML = '<my-a id=a></my-a><my-b id=b></my-b>';",
                "log.join()"));
        }

        [Test]
        public async Task ThrowingMemberStillRunsTheReactionsOfItsCall()
        {
            Assert.AreEqual("connected,caught", await RunPageAsync(@"
                var log = [];
                class MyEl extends HTMLElement { connectedCallback() { log.push('connected'); } }
                customElements.define('my-el', MyEl);
                var el = new MyEl();
                document.body.appendChild(el);
                try { document.body.appendChild(document.documentElement); } catch (e) { log.push('caught'); }",
                "log.join()"));
        }

        [Test]
        public async Task TypesWithoutInterfaceObjectAreNotPublished()
        {
            DomRegistration registration = registry =>
            {
                RegisterDom(registry);
                registry.Mixin<IParentNode>("ParentNode").WithoutInterfaceObject();
                registry.Type<Thing>("Thing").WithoutInterfaceObject().Constant("ANSWER", 42);
            };
            var (service, document) = await OpenAsync(registration, "<div><b></b></div>");
            Assert.AreEqual("undefined", Eval(service, document, "typeof ParentNode"));
            Assert.AreEqual(1.0, Eval(service, document, "document.querySelector('div').childElementCount"));
            Assert.AreEqual("undefined", Eval(service, document, "typeof Thing"));
            Assert.AreEqual(42.0, Eval(service, document, "thing.value"));
            Assert.AreEqual(42.0, Eval(service, document, "thing.ANSWER"));
            Assert.AreEqual(false, Eval(service, document, "Object.getPrototypeOf(thing).hasOwnProperty('constructor')"));
        }

        [Test]
        public void ConstructorWithoutInterfaceObjectIsRejected() =>
            AssertRejected(registry => registry.Type<Event>("Event").WithoutInterfaceObject().Constructor(args => new Event("x")));

        [Test]
        public void InheritingFromAnUnregisteredTypeIsRejected() =>
            AssertRejected(registry => registry.Type<ProbeDerived>("ProbeDerived").Inherits<ProbeBase>());

        [Test]
        public void IncludingWhatIsNoMixinIsRejected() =>
            AssertRejected(registry =>
            {
                registry.Type<IParentNode>("ParentNode");
                registry.Type<IElement>("Element").Includes<IParentNode>();
            });

        [Test]
        public void ConstructibleMixinIsRejected() =>
            AssertRejected(registry => registry.Mixin<IParentNode>("ParentNode").Constructor(args => null));

        [Test]
        public void RegisteringATypeUnderTwoNamesIsRejected() =>
            AssertRejected(registry =>
            {
                registry.Type<IElement>("Element");
                registry.Type<IElement>("Other");
            });

        private static void AssertRejected(DomRegistration registration)
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs(registration));
            var document = context.OpenNewAsync().Result;
            Assert.Throws<InvalidOperationException>(() => Eval(context.GetService<JsScriptingService>(), document, "1"));
        }
    }
}
