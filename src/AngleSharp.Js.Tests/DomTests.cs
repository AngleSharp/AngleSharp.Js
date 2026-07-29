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

        //  Two closed forms of IHtmlCollection<T> in one document - each resolves its own
        //  indexer, and the explicit IReadOnlyList<T> re-implementation behind them is the
        //  one accessor that cannot be invoked as declared.
        [Test]
        public async Task NumericIndexerWorksForHtmlCollectionsOfDifferentItemTypes()
        {
            var result = await "(function () { var d = new DOMParser().parseFromString(`<img id=x>`, 'text/html'); return d.getElementsByTagName('img')[0].id + ',' + d.images[0].id; })()".EvalScriptAsync();
            Assert.AreEqual("x,x", result);
        }

        //  col and colgroup are separate classes sharing the HTMLTableColElement prototype.
        [Test]
        public async Task NumericIndexerWorksForElementsSharingAPrototype()
        {
            var result = await "(function () { var d = new DOMParser().parseFromString(`<table><colgroup class='a b'><col class='c d'></colgroup></table>`, 'text/html'); var g = d.getElementsByTagName('colgroup')[0], c = d.getElementsByTagName('col')[0]; return g.classList[1] + ',' + c.classList[1]; })()".EvalScriptAsync();
            Assert.AreEqual("b,d", result);
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

        //  Reading an index, testing it for existence and enumerating it are three different
        //  questions the engine asks, and the node answers each of them from a different
        //  method. They have to agree.
        [Test]
        public async Task IndexedEntryIsReportedAsAnOwnPropertyAndReads()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); return c.hasOwnProperty(0) + ',' + (0 in c) + ',' + c[0].nodeName; })()".EvalScriptAsync();
            Assert.AreEqual("true,true,SCRIPT", result);
        }

        [Test]
        public async Task IndexBeyondTheEndIsNoOwnPropertyAndReadsUndefined()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); return c.hasOwnProperty(5) + ',' + (5 in c) + ',' + (typeof c[5]); })()".EvalScriptAsync();
            Assert.AreEqual("false,false,undefined", result);
        }

        //  A member of an indexed collection must not be mistaken for an index. "length" is
        //  the collection's own property rather than an inherited one, which is what the
        //  array-like projection reports for it - a browser has it on the prototype instead.
        [Test]
        public async Task MemberOfAnIndexedCollectionIsNotMistakenForAnIndex()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); return ('length' in c) + ',' + c.length + ',' + c.propertyIsEnumerable('length'); })()".EvalScriptAsync();
            Assert.AreEqual("true,1,false", result);
        }

        //  A DOM collection is array-like, not an array - which is exactly what a browser
        //  reports for one.
        [Test]
        public async Task CollectionIsNotAnArray()
        {
            var result = await "Array.isArray(document.getElementsByTagName('script'))".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

        [Test]
        public async Task CollectionKeepsItsDomIdentity()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); return Object.prototype.toString.call(c) + ',' + (c instanceof HTMLCollection); })()".EvalScriptAsync();
            Assert.AreEqual("[object HTMLCollection],true", result);
        }

        [Test]
        public async Task CollectionCanBeIterated()
        {
            var result = await "(function () { var n = 0; for (var s of document.getElementsByTagName('script')) { n += s.nodeName.length; } return n; })()".EvalScriptAsync();
            Assert.AreEqual("6", result);
        }

        [Test]
        public async Task CollectionCanBeSpread()
        {
            var result = await "[...document.getElementsByTagName('script')].length".EvalScriptAsync();
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task ArrayGenericsRunOverACollection()
        {
            var result = await "Array.prototype.map.call(document.getElementsByTagName('script'), function (e) { return e.nodeName; }).join()".EvalScriptAsync();
            Assert.AreEqual("SCRIPT", result);
        }

        [Test]
        public async Task IndicesOfACollectionAreEnumerated()
        {
            var result = await "JSON.stringify(Object.keys(document.getElementsByTagName('script')))".EvalScriptAsync();
            Assert.AreEqual("[\"0\"]", result);
        }

        //  The platform-object shape: the projection owns its indices, so script cannot delete
        //  one or define over it.
        [Test]
        public async Task IndexOfACollectionCannotBeDeleted()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); return delete c[0]; })()".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

        [Test]
        public async Task ExpandoOnACollectionIsStillPossible()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); c.marker = 'kept'; return c.marker + ',' + c.hasOwnProperty('marker'); })()".EvalScriptAsync();
            Assert.AreEqual("kept,true", result);
        }

        //  Existence is answered from the collection's length alone, without producing the
        //  element - so it has to keep agreeing with what reading it would say.
        [Test]
        public async Task ExistenceOfAnIndexAgreesWithReadingIt()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); var r = []; for (var i = 0; i < 3; i++) { r.push((i in c) + ':' + (c[i] !== undefined)); } return r.join(); })()".EvalScriptAsync();
            Assert.AreEqual("true:true,false:false,false:false", result);
        }

        [Test]
        public async Task NumericIndexerOfNamedNodeMapStillReadsBothWays()
        {
            var result = await "(function () { var d = document.createElement('div'); d.setAttribute('title', 't'); var a = d.attributes; return a.length + ',' + a[0].name + ',' + a.title.value; })()".EvalScriptAsync();
            Assert.AreEqual("1,title,t", result);
        }

        //  An element whose id happens to be numeric must not surface as an index of the
        //  collection. The array-like projection owns every array-index key - an index past the
        //  end is authoritatively absent - so the named entry answers only non-index names,
        //  which is also how WebIDL resolves the collision. Every answer has to say the same
        //  thing, the descriptor included.
        [Test]
        public async Task NamedEntryWithAnIndexShapedNameIsNotAnIndex()
        {
            var result = await "(function () { var f = document.createElement('form'); f.id = '5'; document.documentElement.appendChild(f); var c = document.forms; return (Object.getOwnPropertyDescriptor(c, '5') === undefined) + ',' + ('5' in c) + ',' + (c['5'] === undefined) + ',' + c.hasOwnProperty('5') + ',' + (c[0] === f); })()".EvalScriptAsync();
            Assert.AreEqual("true,false,true,false,true", result);
        }

        //  The guard above must not overreach: a named entry whose name is not an array index
        //  keeps resolving, descriptor and all.
        [Test]
        public async Task NamedEntryWithAnOrdinaryNameStillResolves()
        {
            var result = await "(function () { var f = document.createElement('form'); f.id = 'login'; document.documentElement.appendChild(f); var c = document.forms; return (c.login === f) + ',' + (Object.getOwnPropertyDescriptor(c, 'login') !== undefined); })()".EvalScriptAsync();
            Assert.AreEqual("true,true", result);
        }

        //  A symbol cannot be an index, and it must not be turned into one either - the
        //  well-known symbols are probed on every kind of object by library code.
        [Test]
        public async Task SymbolKeyOnAnIndexedCollectionIsNotTreatedAsAnIndex()
        {
            var result = await "(function () { var c = document.getElementsByTagName('script'); return c.hasOwnProperty(Symbol.toStringTag) + ',' + (typeof c[Symbol.toStringTag]); })()".EvalScriptAsync();
            Assert.AreEqual("false,string", result);
        }

        //  The node's own property set lives in the DOM, so nothing in the engine changes
        //  when an entry appears there. A name that was absent has to start resolving on the
        //  node from the very next read, rather than staying on whatever the prototype said.
        [Test]
        public async Task NamedEntryStartsResolvingOnTheNodeAsSoonAsItExists()
        {
            var result = await "(function () { var d = document.createElement('div'), a = d.attributes, before = typeof a.title; d.setAttribute('title', 't'); return before + ',' + a.title.value + ',' + a.hasOwnProperty('title'); })()".EvalScriptAsync();
            Assert.AreEqual("undefined,t,true", result);
        }

        [Test]
        public async Task AccessorDefinedOnANodeByScriptIsInvokedOnRead()
        {
            var result = await "(function () { var d = document.createElement('div'); Object.defineProperty(d, 'marker', { get: function () { return 'from getter'; } }); return d.marker + ',' + d.hasOwnProperty('marker'); })()".EvalScriptAsync();
            Assert.AreEqual("from getter,true", result);
        }

        [Test]
        public async Task NonEnumerablePropertyOfANodeIsSeenButNotEnumerated()
        {
            var result = await "(function () { var d = document.createElement('div'); Object.defineProperty(d, 'hidden2', { value: 1 }); return d.hasOwnProperty('hidden2') + ',' + d.propertyIsEnumerable('hidden2') + ',' + Object.keys(d).length; })()".EvalScriptAsync();
            Assert.AreEqual("true,false,0", result);
        }

        [Test]
        public async Task PropertyAssignedToANodeIsCopiedAndSerialized()
        {
            var result = await "(function () { var d = document.createElement('div'); d.custom = 'v'; return JSON.stringify(d) + ',' + JSON.stringify(Object.assign({}, d)); })()".EvalScriptAsync();
            Assert.AreEqual("{\"custom\":\"v\"},{\"custom\":\"v\"}", result);
        }
    }
}
