namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [DomName("ProbeIterable")]
    [DomExposed("Window")]
    public sealed class ProbeIterable
    {
        [DomConstructor]
        public ProbeIterable()
        {
        }

        [DomName("entries")]
        public String[][] Entries() => new[] { new[] { "a", "1" }, new[] { "b", "2" } };
    }

    [DomExposed("Element")]
    public static class ProbeCallbackExtensions
    {
        [DomName("probeWait")]
        public static async Task<Object> ProbeWait(this IElement element, Func<String, Task<Object>> callback)
        {
            await Task.Delay(1).ConfigureAwait(false);
            return await callback.Invoke("given").ConfigureAwait(false);
        }

        [DomName("probeDenied")]
        public static void ProbeDenied(this IElement element) => throw new DomException("NotAllowedError");

        [DomName("probeFailed")]
        public static void ProbeFailed(this IElement element) => throw new DomException("something went wrong");

        [DomName("probeClone")]
        public static Object ProbeClone(this IElement element, Object value = null) =>
            element.Owner.Context.GetService<IScriptValueCloner>().Clone(value);

        [DomName("probeUndefined")]
        public static Object ProbeUndefined(this IElement element) =>
            element.Owner.Context.GetService<IScriptValueCloner>().Undefined;

        [DomName("probeWaitPlain")]
        public static async Task ProbeWaitPlain(this IElement element, Func<String, Task> callback)
        {
            await Task.Delay(1).ConfigureAwait(false);
            await callback.Invoke("given").ConfigureAwait(false);
        }
    }

    /// <summary>
    /// What gives entries (WebIDL iterable&lt;K, V&gt;) is iterated like them.
    /// </summary>
    [TestFixture]
    public class IterableTests
    {
        private static async Task<String> Eval(String script)
        {
            var cfg = Configuration.Default.WithJs().With(new ProbeService());
            var context = BrowsingContext.New(cfg);
            var document = await context.OpenNewAsync();
            return document.ExecuteScript(script)?.ToString();
        }

        [TestCase("var r = []; for (const [k, v] of new ProbeIterable()) { r.push(k + '=' + v); } r.join()", "a=1,b=2")]
        [TestCase("Array.from(new ProbeIterable()).length.toString()", "2")]
        [TestCase("[...new ProbeIterable()][1][0]", "b")]
        [TestCase("new Map(new ProbeIterable()).get('b')", "2")]
        [TestCase("typeof ProbeIterable.prototype[Symbol.iterator]", "function")]
        [TestCase("typeof new ProbeIterable()[Symbol.iterator]().next", "function")]
        [TestCase("Object.keys(ProbeIterable.prototype).indexOf('entries') !== -1 ? 'yes' : 'no'", "yes")]
        [TestCase("typeof Node.prototype[Symbol.iterator]", "undefined")]
        public async Task IteratesAsEntries(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("var d = document.createElement('div'); d.append('a', document.createElement('b'), 1); d.innerHTML", "a<b></b>1")]
        [TestCase("var d = document.createElement('div'); d.append('<i>'); d.childNodes[0].nodeType.toString()", "3")]
        [TestCase("var d = document.createElement('div'); d.innerHTML = '<a></a>'; d.prepend('x'); d.innerHTML", "x<a></a>")]
        [TestCase("var d = document.createElement('div'); d.innerHTML = '<a></a>'; d.replaceChildren('x', document.createElement('u')); d.innerHTML", "x<u></u>")]
        [TestCase("var d = document.createElement('div'); d.innerHTML = '<a></a>'; d.replaceChildren(); d.childNodes.length.toString()", "0")]
        [TestCase("var d = document.createElement('div'); d.innerHTML = '<a></a>'; d.firstChild.before('x'); d.firstChild.after('y'); d.innerHTML", "xy<a></a>")]
        public async Task TextBecomesATextNode(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("new EventTarget() instanceof EventTarget", "True")]
        [TestCase("var r = 0, t = new EventTarget(); t.addEventListener('x', function () { r++; }); t.dispatchEvent(new Event('x')); r.toString()", "1")]
        [TestCase("class T extends EventTarget { constructor() { super(); this.a = 1; } m() { return this.a + 1; } } var t = new T(); (t.m() + ',' + (t instanceof T) + ',' + (t instanceof EventTarget)).toString()", "2,true,true")]
        [TestCase("class T extends EventTarget { #s = 5; get s() { return this.#s; } set(v) { this.#s = v; this.dispatchEvent(new Event('c')); } } var t = new T(), seen = 0; t.addEventListener('c', function () { seen = t.s; }); t.set(9); seen.toString()", "9")]
        [TestCase("class E extends Event { constructor() { super('x'); } get k() { return 1; } } var t = new EventTarget(), r = 0; t.addEventListener('x', function (e) { r = e.k + (e instanceof E ? 1 : 0); }); t.dispatchEvent(new E()); r.toString()", "2")]
        [TestCase("class U extends URLSearchParams { two() { return this.get('a') + '2'; } } new U('a=1').two()", "12")]
        [TestCase("new URLSearchParams('a=1') instanceof URLSearchParams", "True")]
        public async Task DerivedClassesAreOnTheChain(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("var r = 0, d = document.createElement('div'), l = { handleEvent: function () { r++; } }; d.addEventListener('x', l); d.dispatchEvent(new Event('x')); r.toString()", "1")]
        [TestCase("var s, d = document.createElement('div'), l = { handleEvent: function () { s = this; } }; d.addEventListener('x', l); d.dispatchEvent(new Event('x')); (s === l).toString()", "true")]
        [TestCase("var r = 0, d = document.createElement('div'), l = { handleEvent: function () { r++; } }; d.addEventListener('x', l); d.removeEventListener('x', l); d.dispatchEvent(new Event('x')); r.toString()", "0")]
        [TestCase("var r = 0, d = document.createElement('div'), l = { handleEvent: function () { r++; } }; d.addEventListener('x', l, { once: true }); d.dispatchEvent(new Event('x')); d.dispatchEvent(new Event('x')); r.toString()", "1")]
        [TestCase("var r = 0, d = document.createElement('div'); d.handleEvent = function (e) { r += e.type === 'x' ? 1 : 0; }; d.addEventListener('x', d, { capture: true }); d.dispatchEvent(new Event('x')); r.toString()", "1")]
        [TestCase("var r = 0, d = document.createElement('div'), l = { handleEvent: function () { r = 1; } }; d.addEventListener('x', l); l.handleEvent = function () { r = 2; }; d.dispatchEvent(new Event('x')); r.toString()", "2")]
        public async Task ObjectsCanListen(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        private static async Task<String> EvalInLoop(String script)
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop().With(new ProbeService());
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content($"<!doctype html><title>start</title><body><script>{script}</script>"));

            for (var i = 0; i < 200 && document.Title == "start"; i++)
            {
                await Task.Delay(10);
            }

            return document.Title;
        }

        [TestCase("document.body.probeWait(function (v) { return v + '!'; }).then(function (r) { document.title = r; })", "given!")]
        [TestCase("document.body.probeWait(async function (v) { await null; return v + '?'; }).then(function (r) { document.title = r; })", "given?")]
        [TestCase("document.body.probeWait(function (v) { return Promise.reject('no'); }).catch(function (r) { document.title = r; })", "no")]
        [TestCase("document.body.probeWait(function (v) { throw 'thrown'; }).catch(function (r) { document.title = r; })", "thrown")]
        [TestCase("document.body.probeWait(function (v) { return { then: function (a) { a('thenable'); } }; }).then(function (r) { document.title = r; })", "thenable")]
        [TestCase("document.body.probeWaitPlain(function (v) { document.title = v; })", "given")]
        [TestCase("document.body.probeWaitPlain(async function (v) { await null; throw 'late'; }).catch(function (r) { document.title = r; })", "late")]
        public async Task CallbacksAreWaitedFor(String script, String expected)
        {
            Assert.AreEqual(expected, await EvalInLoop(script));
        }

        [TestCase("var o = { a: 1 }, c = document.body.probeClone(o); c.a = 2; o.a + ',' + c.a", "1,2")]
        [TestCase("JSON.stringify(document.body.probeClone({ a: { b: [1, 'x', null, true] }, c: 'd' }))", "{\"a\":{\"b\":[1,\"x\",null,true]},\"c\":\"d\"}")]
        [TestCase("var d = document.body.probeClone(new Date(5)); (d instanceof Date) + ',' + d.getTime()", "true,5")]
        [TestCase("var d = document.body.probeClone({ d: new Date(7) }).d; (d instanceof Date) + ',' + d.getTime()", "true,7")]
        [TestCase("var b = document.body.probeClone(new Uint8Array([1, 2])); (b instanceof ArrayBuffer) + ',' + b.byteLength", "true,2")]
        [TestCase("var b = document.body.probeClone({ b: new Uint8Array([1, 2, 3]) }).b; (b instanceof ArrayBuffer) + ',' + b.byteLength", "true,3")]
        [TestCase("try { document.body.probeClone(function () {}); } catch (e) { e.name; }", "DataCloneError")]
        [TestCase("try { document.body.probeClone({ a: document.body }); } catch (e) { e.name; }", "DataCloneError")]
        [TestCase("String(document.body.probeUndefined() === undefined)", "true")]
        public async Task ValuesAreClonedForTheDom(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("document.body.innerHTML = '<a></a><b></b><i></i>'; var w = document.createTreeWalker(document.body, 1, { acceptNode: function (n) { return n.tagName === 'B' ? 1 : 3; } }); w.nextNode().tagName + ',' + w.nextNode()", "B,null")]
        [TestCase("document.body.innerHTML = '<a></a><b></b>'; var w = document.createTreeWalker(document.body, 1, function (n) { return n.tagName === 'A' ? NodeFilter.FILTER_SKIP : NodeFilter.FILTER_ACCEPT; }); w.nextNode().tagName", "B")]
        [TestCase("document.body.innerHTML = '<a></a><b></b>'; var o = { skip: 'A', acceptNode: function (n) { return n.tagName === this.skip ? 2 : 1; } }; var i = document.createNodeIterator(document.body, 1, o); i.nextNode().tagName + ',' + i.nextNode().tagName", "BODY,B")]
        [TestCase("document.body.innerHTML = '<a></a>'; var w = document.createTreeWalker(document.body, 1, null); w.nextNode().tagName", "A")]
        [TestCase("document.body.innerHTML = '<a></a>'; try { document.createTreeWalker(document.body, 1, {}).nextNode(); } catch (e) { e.name; }", "TypeError")]
        public async Task ObjectsCanFilterNodes(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("JSON.stringify(new CustomEvent('x', { detail: { a: 1 } }).detail)", "{\"a\":1}")]
        [TestCase("String(new CustomEvent('x', { detail: 5 }).detail)", "5")]
        [TestCase("String(new CustomEvent('x').detail)", "null")]
        [TestCase("var d = {}; String(new CustomEvent('x', { detail: d }).detail === d)", "true")]
        [TestCase("class E extends CustomEvent { constructor(d) { super('x', { detail: d }); } } JSON.stringify(new E({ a: 2 }).detail)", "{\"a\":2}")]
        [TestCase("var r; document.body.addEventListener('y', function (e) { r = e.detail; }); document.body.dispatchEvent(new CustomEvent('y', { detail: 'data' })); r", "data")]
        [TestCase("var e = new CustomEvent('x', { detail: 1, bubbles: true, cancelable: true }); e.bubbles + ',' + e.cancelable", "true,true")]
        [TestCase("var t = document.body; String(new FocusEvent('focus', { relatedTarget: t }).relatedTarget === t)", "true")]
        public async Task EventDetailsGetThroughTheirConstructors(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("document.body.innerHTML = '<a href=x></a><b></b>'; var n = 0; for (var e of document.body.children) { n++; } n.toString()", "2")]
        [TestCase("document.body.innerHTML = '<a href=x></a><a href=y></a>'; [...document.links].length.toString()", "2")]
        [TestCase("document.body.innerHTML = '<img>'; var n = 0; for (var e of document.images) { n++; } n.toString()", "1")]
        [TestCase("document.body.innerHTML = '<form></form>'; Array.from(document.forms).length.toString()", "1")]
        [TestCase("String(document.body.children === document.body.children)", "true")]
        [TestCase("document.body.innerHTML = '<a></a>'; var c = document.body.children; document.body.appendChild(document.createElement('b')); c.length + c[1].tagName", "2B")]
        [TestCase("document.body.className = 'x y'; var l = document.body.classList; l.length + ',' + l[1] + ',' + l.contains('y')", "2,y,true")]
        public async Task CollectionsOfAttributesAreIterable(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("var o = { a: [1, { b: 2 }] }; var c = structuredClone(o); c.a[1].b = 3; o.a[1].b + ',' + c.a[1].b", "2,3")]
        [TestCase("structuredClone(5) + ',' + structuredClone('x') + ',' + structuredClone(null)", "5,x,null")]
        [TestCase("var d = structuredClone(new Date(9)); (d instanceof Date) + ',' + d.getTime()", "true,9")]
        [TestCase("try { structuredClone(function () {}); } catch (e) { e.name; }", "DataCloneError")]
        [TestCase("try { structuredClone(document.body); } catch (e) { e.name; }", "DataCloneError")]
        [TestCase("String(typeof window.isSecureContext)", "boolean")]
        [TestCase("document.body.innerHTML = '<iframe></iframe>'; String(window.length)", "1")]
        [TestCase("document.body.className = 'a b'; var r = []; document.body.classList.forEach(function (t) { r.push(t); }); r.join()", "a,b")]
        [TestCase("document.body.className = 'a b'; Array.from(document.body.classList.values()).join()", "a,b")]
        [TestCase("document.body.className = 'a b'; var r = []; for (var t of document.body.classList) { r.push(t); } r.join()", "a,b")]
        [TestCase("document.body.className = 'a b'; document.body.classList.replace('a', 'c') + ',' + document.body.className", "true,c b")]
        [TestCase("document.body.className = 'a'; document.body.classList.value = 'x y'; document.body.className + ',' + document.body.classList.value", "x y,x y")]
        [TestCase("document.body.className = 'a'; document.body.classList.add('b'); document.body.classList.contains('b') + ',' + document.body.classList.length", "true,2")]
        [TestCase("var e = new KeyboardEvent('keydown', { key: 'a', code: 'KeyA', ctrlKey: true, shiftKey: true, repeat: true, bubbles: true }); [e.key, e.code, e.ctrlKey, e.shiftKey, e.altKey, e.repeat, e.bubbles].join()", "a,KeyA,true,true,false,true,true")]
        [TestCase("var e = new KeyboardEvent('keydown'); [e.type, e.key, e.code, e.ctrlKey].join()", "keydown,,,false")]
        [TestCase("var e = new KeyboardEvent('keydown', {}); [e.key, e.code, e.ctrlKey].join()", ",,false")]
        [TestCase("var t = document.body; var e = new MouseEvent('click', { clientX: 3, button: 2, buttons: 2, ctrlKey: true, relatedTarget: t }); [e.clientX, e.x, e.button, e.buttons, e.ctrlKey, e.relatedTarget === t].join()", "3,3,2,2,true,true")]
        [TestCase("var e = new MouseEvent('click', {}); [e.clientX, e.button, e.buttons].join()", "0,0,0")]
        [TestCase("var e = new MouseEvent('click'); [e.type, e.buttons].join()", "click,0")]
        [TestCase("var t = document.body; var e = new WheelEvent('wheel', { deltaY: 5, ctrlKey: true, relatedTarget: t }); [e.deltaY, e.deltaX, e.ctrlKey, e.relatedTarget === t].join()", "5,0,true,true")]
        [TestCase("var e = new PointerEvent('pointerdown', { pointerId: 7, pointerType: 'pen', clientX: 2, pressure: 0.5, isPrimary: true }); [e.pointerId, e.pointerType, e.clientX, e.pressure, e.isPrimary, e instanceof MouseEvent].join()", "7,pen,2,0.5,true,true")]
        [TestCase("var e = new Event('x', { cancelable: true }); e.returnValue = false; e.defaultPrevented + ',' + e.returnValue", "true,false")]
        [TestCase("JSON.stringify(new DOMPoint(1, 2))", "{\"x\":1,\"y\":2,\"z\":0,\"w\":1}")]
        [TestCase("document.body.innerHTML = '<div a=1 b=2></div>'; document.body.firstChild.getAttributeNames().join()", "a,b")]
        [TestCase("document.body.innerHTML = '<div></div>'; var d = document.body.firstChild; d.insertAdjacentElement('beforeend', document.createElement('i')); d.insertAdjacentText('beforeend', 'x'); d.innerHTML", "<i></i>x")]
        [TestCase("document.scrollingElement === document.documentElement ? document.visibilityState + ',' + document.hidden : 'no'", "visible,false")]
        [TestCase("String(document.body.isSameNode(document.body))", "true")]
        public async Task MembersOfTheDomWork(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("try { document.body.probeDenied(); } catch (e) { e.name + ',' + (e instanceof DOMException) + ',' + e.code; }", "NotAllowedError,true,0")]
        [TestCase("try { document.body.probeFailed(); } catch (e) { e.name + ',' + e.message; }", "Error,something went wrong")]
        public async Task ExceptionsWithoutACodeKeepTheirName(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [Test]
        public async Task EntriesAreAskedForByName()
        {
            var script = @"var p = new ProbeIterable(); p.entries = function () { return [['x', 'y']]; }; [...p][0].join()";
            Assert.AreEqual("x,y", await Eval(script));
        }
    }
}
