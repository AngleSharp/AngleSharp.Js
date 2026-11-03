namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    [DomLiterals]
    [DomName("BindingColor")]
    public enum BindingColor
    {
        [DomName("light-red")]
        LightRed,
        [DomName("blue")]
        Blue,
    }

    [DomName("BindingConstant")]
    public enum BindingConstant
    {
        [DomName("FIRST")]
        First = 1,
    }

    public sealed class BindingUnion
    {
        private BindingUnion(String description)
        {
            Description = description;
        }

        public String Description { get; }

        public static implicit operator BindingUnion(String value) => new BindingUnion("single:" + value);

        public static implicit operator BindingUnion(String[] values) => new BindingUnion("many:" + String.Join("|", values));

        public static implicit operator BindingUnion(Double value) => new BindingUnion("number:" + value.ToString(CultureInfo.InvariantCulture));
    }

    public sealed class BindingGivenUnion
    {
        public String[] Values { get; set; }

        public static implicit operator BindingGivenUnion(String value) => new BindingGivenUnion { Values = new[] { value } };

        public static implicit operator BindingGivenUnion(String[] values) => new BindingGivenUnion { Values = values };

        public static implicit operator String[](BindingGivenUnion union) => union.Values;
    }

    [DomName("BindingGivenOptions")]
    public sealed class BindingGivenOptions
    {
        [DomName("urls")]
        public BindingGivenUnion Urls { get; set; }
    }

    [DomName("BindingStringifier")]
    public class BindingStringifier
    {
        [DomConstructor]
        public BindingStringifier()
        {
        }

        public override String ToString() => "stringified";
    }

    [DomName("BindingDerivedStringifier")]
    public sealed class BindingDerivedStringifier : BindingStringifier
    {
        [DomConstructor]
        public BindingDerivedStringifier()
        {
        }
    }

    [DomName("BindingOptions")]
    public sealed class BindingOptions
    {
        [DomName("union")]
        public BindingUnion Union { get; set; }

        [DomName("unions")]
        public Dictionary<String, BindingUnion> Unions { get; set; }

        [DomName("names")]
        public String[] Names { get; set; }

        [DomName("children")]
        public BindingOptions[] Children { get; set; }

        [DomName("groups")]
        public Dictionary<String, String[]> Groups { get; set; }

        [DomName("data")]
        public Object Data { get; set; }

        [DomName("big")]
        public UInt64? Big { get; set; }

        [DomName("flag")]
        public Boolean? Flag { get; set; }
    }

    [DomName("BindingBase")]
    public class BindingBase
    {
        [DomName("inherited")]
        public static String Inherited() => "base";

        [DomName("hidden")]
        public static String Hidden() => "base";

        [DomName("hiddenInstance")]
        public String HiddenInstance(Double value) => "base:" + value;
    }

    [DomName("BindingProbe")]
    [DomExposed("Window")]
    public sealed class BindingProbe : BindingBase
    {
        private static String Stored = "initial";

        [DomConstructor]
        public BindingProbe() => Created = "none";

        [DomConstructor]
        public BindingProbe(String text) => Created = "text:" + text;

        [DomConstructor]
        public BindingProbe(IWindow window, Double a, Double b) => Created = "window:" + (window != null) + ":" + (a + b);

        [DomConstructor]
        public BindingProbe(BindingOptions options) => Created = "options:" + String.Join("|", options.Names ?? Array.Empty<String>());

        [DomName("created")]
        public String Created { get; }

        [DomName("hidden")]
        public static new String Hidden() => "derived";

        [DomName("hiddenInstance")]
        public new String HiddenInstance(Double value) => "derived:" + value;

        [DomName("constant")]
        public static String Constant => "property";

        [DomName("stored")]
        public static String StoredValue
        {
            get => Stored;
            set => Stored = value;
        }

        [DomName("permission")]
        [DomAccessor(Accessors.Getter)]
        public static String GetPermission(IWindow window) => "window:" + (window != null);

        [DomName("request")]
        public static Task<BindingColor> Request(IWindow window, Boolean first = false) =>
            Task.FromResult(first && window != null ? BindingColor.LightRed : BindingColor.Blue);

        [DomName("noWindow")]
        public static String NoWindow(String value) => "value:" + value;

        [DomName("noWindow")]
        public static String NoWindow(Double a, Double b) => "sum:" + (a + b);

        [DomName("failing")]
        public static String Failing() => throw new InvalidOperationException("static failed");

        [DomName("overloaded")]
        public String Overloaded() => "none";

        [DomName("overloaded")]
        public String Overloaded(BindingOptions options) => "options:" + (options?.Flag?.ToString() ?? "-");

        [DomName("overloaded")]
        public String Overloaded(String text) => "text:" + text;

        [DomName("overloaded")]
        public String Overloaded(Byte[] bytes) => "bytes:" + String.Join(",", bytes);

        [DomName("overloaded")]
        public String Overloaded(Double[] numbers) => "numbers:" + String.Join(",", numbers);

        [DomName("union")]
        public String Union(BindingUnion value) => value?.Description ?? "null";

        [DomName("unionInOptions")]
        public String UnionInOptions(BindingOptions options) => options.Union?.Description + ";" +
            String.Join("|", (options.Unions ?? new Dictionary<String, BindingUnion>()).Select(entry => entry.Key + "=" + entry.Value.Description));

        [DomName("echoGiven")]
        public BindingGivenOptions EchoGiven(BindingGivenOptions options) => options;

        [DomName("givenNothing")]
        public BindingGivenUnion GivenNothing => null;

        [DomName("view")]
        public ArraySegment<Byte> View => new ArraySegment<Byte>(new Byte[] { 1, 2, 3, 4 }, 1, 2);

        [DomName("emptyView")]
        public ArraySegment<Byte> EmptyView => default;

        [DomName("names")]
        public String Names(String[] names) => String.Join("|", names);

        [DomName("sequence")]
        public String Sequence(IEnumerable<Int32> values) => String.Join("|", values);

        [DomName("options")]
        public String Options(BindingOptions options) => String.Join(";",
            String.Join("|", options.Names ?? Array.Empty<String>()),
            (options.Children ?? Array.Empty<BindingOptions>()).Length.ToString(CultureInfo.InvariantCulture),
            String.Join("|", (options.Groups ?? new Dictionary<String, String[]>()).Select(group => group.Key + "=" + String.Join(",", group.Value))),
            options.Big?.ToString(CultureInfo.InvariantCulture) ?? "-",
            options.Flag?.ToString() ?? "-");

        [DomName("kindOfData")]
        public String KindOfData(BindingOptions options) => options.Data?.GetType().Name ?? "null";

        [DomName("bytes")]
        public String Bytes(Byte[] bytes) => String.Join(",", bytes);

        [DomName("segment")]
        public String Segment(ArraySegment<Byte> bytes) => String.Join(",", bytes);

        [DomName("anything")]
        public String Anything(Object value) => value switch
        {
            Byte[] bytes => "bytes:" + String.Join(",", bytes),
            String text => "text:" + text,
            null => "null",
            _ => "other",
        };

        [DomName("numeric")]
        public String Numeric(UInt64 a, Int64 b, Single c, Int16 d, Byte e, SByte f, Decimal g) =>
            String.Join("|", a, b, c.ToString(CultureInfo.InvariantCulture), d, e, f, g.ToString(CultureInfo.InvariantCulture));

        [DomName("optionalNumber")]
        public String OptionalNumber(UInt64? value) => value?.ToString(CultureInfo.InvariantCulture) ?? "none";

        [DomName("color")]
        public BindingColor Color(BindingColor value) => value;

        [DomName("constantValue")]
        public BindingConstant ConstantValue => BindingConstant.First;

        [DomName("buffer")]
        public Byte[] Buffer => new Byte[] { 1, 2, 3 };

        [DomName("sharedBytes")]
        public Byte[] SharedBytes { get; } = new Byte[] { 7 };

        [DomName("big")]
        public UInt64 Big => 5000000000UL;

        [DomName("long")]
        public Int64 Long => -5000000000L;

        [DomName("short")]
        public Int16 Short => -5;

        [DomName("byte")]
        public Byte ByteValue => 200;

        [DomName("decimal")]
        public Decimal DecimalValue => 1.5m;
    }

    /// <summary>
    /// How the attributes of a library end up on the objects of a script, and what a script can
    /// pass to and get from its members.
    /// </summary>
    [TestFixture]
    public class BindingTests
    {
        private static async Task<IDocument> OpenAsync()
        {
            var cfg = Configuration.Default.WithJs().With(new ProbeService());
            return await BrowsingContext.New(cfg).OpenAsync(m => m.Content("<!doctype html>"));
        }

        private static async Task<String> Eval(String script)
        {
            var document = await OpenAsync();
            return document.ExecuteScript(script)?.ToString();
        }

        [TestCase("BindingProbe.constant", "property")]
        [TestCase("BindingProbe.permission", "window:True")]
        [TestCase("BindingProbe.noWindow('x')", "value:x")]
        [TestCase("BindingProbe.noWindow(1, 2)", "sum:3")]
        [TestCase("BindingProbe.inherited()", "base")]
        [TestCase("BindingProbe.hidden()", "derived")]
        [TestCase("BindingBase.hidden()", "base")]
        [TestCase("typeof BindingProbe.request", "function")]
        [TestCase("const f = BindingProbe.noWindow; f('detached')", "value:detached")]
        public async Task StaticMembersAreOnTheConstructor(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [Test]
        public async Task StaticMethodsGetTheWindowAndGivePromises()
        {
            var document = await OpenAsync();

            document.ExecuteScript("window.seen = []; BindingProbe.request(true).then(v => seen.push(v)); BindingProbe.request().then(v => seen.push(v));");

            for (var i = 0; i < 200 && document.ExecuteScript("seen.length")?.ToString() != "2"; i++)
            {
                await Task.Delay(10);
            }

            Assert.AreEqual("light-red,blue", document.ExecuteScript("seen.join()")?.ToString());
        }

        [Test]
        public async Task StaticPropertiesCanBeWritten()
        {
            Assert.AreEqual("changed", await Eval("BindingProbe.stored = 'changed'; const r = BindingProbe.stored; BindingProbe.stored = 'initial'; r"));
        }

        [Test]
        public async Task ErrorsOfStaticMembersKeepTheirMessage()
        {
            Assert.AreEqual("static failed", await Eval("try { BindingProbe.failing(); } catch (e) { e.message; }"));
        }

        [TestCase("typeof new BindingProbe().noWindow")]
        [TestCase("typeof new BindingProbe().constant")]
        [TestCase("typeof new BindingProbe().permission")]
        [TestCase("typeof window.noWindow")]
        [TestCase("typeof window.permission")]
        [TestCase("typeof window.overloaded")]
        [TestCase("typeof window.created")]
        public async Task MembersStayOnWhatTheyBelongTo(String script)
        {
            Assert.AreEqual("undefined", await Eval(script));
        }

        [Test]
        public async Task ExposedTypesAreStillConstructors()
        {
            Assert.AreEqual("function", await Eval("typeof window.BindingProbe"));
        }

        [TestCase("new BindingProbe().created", "none")]
        [TestCase("new BindingProbe('a').created", "text:a")]
        [TestCase("new BindingProbe(1, 2).created", "window:True:3")]
        [TestCase("new BindingProbe({ names: ['x', 'y'] }).created", "options:x|y")]
        public async Task ConstructorFittingTheArgumentsIsCalled(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [Test]
        public async Task ConstructorOverloadsOfTheDomWork()
        {
            Assert.AreEqual("1", await Eval("new URLSearchParams('a=1').get('a')"));
            Assert.AreEqual("False", await Eval("new URLSearchParams().has('a')"));
        }

        [Test]
        public async Task StaticMembersOfTheDomAreReachable()
        {
            // a fixture of the test assembly claims the name, so this assembly must not be a library here
            var cfg = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content("<!doctype html>"));

            Assert.AreEqual("3", document.ExecuteScript("DOMRect.fromRect({ x: 1, y: 2, width: 3, height: 4 }).width")?.ToString());
            Assert.AreEqual("undefined", document.ExecuteScript("typeof new DOMRect().fromRect")?.ToString());
        }

        [TestCase("CSS.escape('a b')", "a\\ b")]
        [TestCase("CSS.supports('color', 'red')", "True")]
        [TestCase("CSS.supports('(color: red)')", "True")]
        public async Task NamespacesOfOtherLibrariesAreReachable(String script, String expected)
        {
            var document = await BrowsingContext.New(Helpers.GetCssConfig()).OpenAsync(m => m.Content("<!doctype html>"));

            Assert.AreEqual(expected, document.ExecuteScript(script)?.ToString());
        }

        [TestCase("overloaded()", "none")]
        [TestCase("overloaded('a')", "text:a")]
        [TestCase("overloaded({ flag: true })", "options:True")]
        [TestCase("overloaded(new Uint8Array([1, 2]))", "bytes:1,2")]
        [TestCase("overloaded([1.5, 2])", "numbers:1.5,2")]
        [TestCase("hiddenInstance(1)", "derived:1")]
        public async Task MethodOverloadFittingTheArgumentsIsCalled(String call, String expected)
        {
            Assert.AreEqual(expected, await Eval($"new BindingProbe().{call}"));
        }

        [TestCase("names(['a', 'b'])", "a|b")]
        [TestCase("names([])", "")]
        [TestCase("names(new Set(['a', 'b']))", "a|b")]
        [TestCase("names([1, true])", "1|true")]
        [TestCase("sequence([1, 2.9])", "1|2")]
        [TestCase("options({ names: ['a'], children: [{}, {}], groups: { g: ['x', 'y'] }, big: 5000000000, flag: false })", "a;2;g=x,y;5000000000;False")]
        [TestCase("options({})", ";0;;-;-")]
        public async Task SequencesAreConverted(String call, String expected)
        {
            Assert.AreEqual(expected, await Eval($"new BindingProbe().{call}"));
        }

        [TestCase("names({ length: 1 })")]
        [TestCase("names('text')")]
        [TestCase("names(1)")]
        public async Task WhatIsNotASequenceIsRejected(String call)
        {
            Assert.AreEqual("thrown", await Eval($"try {{ new BindingProbe().{call}; 'passed'; }} catch (e) {{ 'thrown'; }}"));
        }

        [TestCase("bytes(new Uint8Array([1, 2]))", "1,2")]
        [TestCase("bytes(new Uint8Array([1, 2]).buffer)", "1,2")]
        [TestCase("bytes(new Uint8Array([1, 2, 3, 4]).subarray(1, 3))", "2,3")]
        [TestCase("bytes(new Uint16Array([513]))", "1,2")]
        [TestCase("bytes(new DataView(new Uint8Array([1, 2, 3]).buffer, 1))", "2,3")]
        [TestCase("bytes([1, 2])", "1,2")]
        [TestCase("segment(new Uint8Array([1, 2]))", "1,2")]
        [TestCase("anything(new Uint8Array([1, 2]).buffer)", "bytes:1,2")]
        [TestCase("anything(new DataView(new Uint8Array([1]).buffer))", "bytes:1")]
        [TestCase("anything('a')", "text:a")]
        [TestCase("anything({})", "other")]
        [TestCase("anything([1])", "other")]
        [TestCase("kindOfData({ data: new Uint8Array(1) })", "Byte[]")]
        [TestCase("kindOfData({ data: 'a' })", "String")]
        public async Task BuffersAreConverted(String call, String expected)
        {
            Assert.AreEqual(expected, await Eval($"new BindingProbe().{call}"));
        }

        [TestCase("numeric(5000000000, -5000000000, 1.5, -2, 200, -3, 2.5)", "5000000000|-5000000000|1.5|-2|200|-3|2.5")]
        [TestCase("numeric(1.9, -1.9, 0, 0, 1.9, 0, 0)", "1|-1|0|0|1|0|0")]
        [TestCase("numeric(NaN, Infinity, 0, NaN, NaN, NaN, NaN)", "0|0|0|0|0|0|0")]
        [TestCase("numeric('7', '8', '1', '2', '3', '4', '5')", "7|8|1|2|3|4|5")]
        [TestCase("numeric(0, 0, 0, 65537, 257, 129, 0)", "0|0|0|1|1|-127|0")]
        [TestCase("optionalNumber(3)", "3")]
        [TestCase("optionalNumber(undefined)", "none")]
        [TestCase("optionalNumber(null)", "none")]
        public async Task NumbersAreConverted(String call, String expected)
        {
            Assert.AreEqual(expected, await Eval($"new BindingProbe().{call}"));
        }

        [TestCase("big", "5000000000")]
        [TestCase("long", "-5000000000")]
        [TestCase("short", "-5")]
        [TestCase("byte", "200")]
        [TestCase("decimal", "1.5")]
        public async Task NumbersAreGivenAsNumbers(String member, String expected)
        {
            Assert.AreEqual(expected, await Eval($"new BindingProbe().{member}"));
            Assert.AreEqual("number", await Eval($"typeof new BindingProbe().{member}"));
        }

        [TestCase("union('a')", "single:a")]
        [TestCase("union(['a', 'b'])", "many:a|b")]
        [TestCase("union([])", "many:")]
        [TestCase("union(new Set(['a']))", "many:a")]
        [TestCase("union(1.5)", "number:1.5")]
        [TestCase("union(null)", "null")]
        [TestCase("unionInOptions({ union: 'a' })", "single:a;")]
        [TestCase("unionInOptions({ unions: { x: 'a', y: ['b', 'c'] } })", ";x=single:a|y=many:b|c")]
        public async Task ImplicitConversionsStandForUnions(String call, String expected)
        {
            Assert.AreEqual(expected, await Eval($"new BindingProbe().{call}"));
        }

        [TestCase("Array.isArray(new BindingProbe().echoGiven({ urls: 'a' }).urls)", "True")]
        [TestCase("new BindingProbe().echoGiven({ urls: 'a' }).urls.join()", "a")]
        [TestCase("new BindingProbe().echoGiven({ urls: ['a', 'b'] }).urls.join()", "a,b")]
        [TestCase("String(new BindingProbe().givenNothing)", "null")]
        public async Task UnionsAreGivenThroughTheirImplicitConversion(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [Test]
        public async Task ATypeOnlyConvertingAwayIsNoUnion()
        {
            Assert.AreEqual("https://a.example/", await Eval("new URL('https://a.example/').href"));
        }

        [TestCase("new BindingProbe().view instanceof Uint8Array", "True")]
        [TestCase("Array.from(new BindingProbe().view).join()", "2,3")]
        [TestCase("new BindingProbe().view.buffer.byteLength", "2")]
        [TestCase("new BindingProbe().emptyView.length", "0")]
        public async Task ByteSegmentsAreGivenAsUint8Arrays(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("String(new BindingStringifier())", "stringified")]
        [TestCase("new BindingStringifier() + ''", "stringified")]
        [TestCase("`${new BindingStringifier()}`", "stringified")]
        [TestCase("new BindingStringifier().toString()", "stringified")]
        [TestCase("String(new BindingDerivedStringifier())", "stringified")]
        [TestCase("Object.prototype.toString.call(new BindingStringifier())", "[object BindingStringifier]")]
        [TestCase("String(new BindingProbe())", "[object BindingProbe]")]
        [TestCase("BindingStringifier.prototype.hasOwnProperty('toString')", "True")]
        public async Task OverridingToStringMakesAStringifier(String script, String expected)
        {
            Assert.AreEqual(expected, await Eval(script));
        }

        [TestCase("String(new URLSearchParams('a=1&b=2'))", "a=1&b=2")]
        [TestCase("new URLSearchParams('a=1').toString()", "a=1")]
        [TestCase("new URLSearchParams().toString()", "")]
        [TestCase("String(new URL('https://a.example/b?c#d'))", "https://a.example/b?c#d")]
        [TestCase("document.body.classList.add('x', 'y'); String(document.body.classList)", "x y")]
        [TestCase("const r = document.createRange(); r.selectNodeContents(document.getElementById('t')); String(r)", "text")]
        [TestCase("String(new DOMMatrix())", "matrix(1, 0, 0, 1, 0, 0)")]
        [TestCase("String(location)", "https://page.example/path")]
        [TestCase("location + ''", "https://page.example/path")]
        [TestCase("String(document.createElement('div'))", "[object HTMLDivElement]")]
        [TestCase("String(document)", "[object HTMLDocument]")]
        public async Task StringifiersOfTheDomGiveTheirText(String script, String expected)
        {
            var cfg = Configuration.Default.WithJs();
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Address("https://page.example/path").Content("<!doctype html><div id=t>text</div>"));

            Assert.AreEqual(expected, document.ExecuteScript(script)?.ToString());
        }

        [TestCase("color('blue')", "blue")]
        [TestCase("color('light-red')", "light-red")]
        public async Task LiteralEnumsAreStrings(String call, String expected)
        {
            Assert.AreEqual(expected, await Eval($"new BindingProbe().{call}"));
            Assert.AreEqual("string", await Eval($"typeof new BindingProbe().{call}"));
        }

        [Test]
        public async Task LiteralEnumsHaveNoObjectWhileConstantsKeepTheirs()
        {
            Assert.AreEqual("undefined", await Eval("typeof BindingColor"));
            Assert.AreEqual("1", await Eval("BindingConstant.FIRST"));
            Assert.AreEqual("number", await Eval("typeof new BindingProbe().constantValue"));
        }

        [Test]
        public async Task LiteralEnumsOfTheDomAreStrings()
        {
            Assert.AreEqual("open", await Eval("document.createElement('div').attachShadow({ mode: 'open' }).mode"));
        }

        [Test]
        public async Task BytesAreGivenAsArrayBuffers()
        {
            Assert.AreEqual("True", await Eval("new BindingProbe().buffer instanceof ArrayBuffer"));
            Assert.AreEqual("2", await Eval("new Uint8Array(new BindingProbe().buffer)[1]"));
            Assert.AreEqual("3", await Eval("new BindingProbe().buffer.byteLength"));
        }

        [Test]
        public async Task ChangingAGivenBufferDoesNotChangeTheObjectOwningIt()
        {
            Assert.AreEqual("7", await Eval("const p = new BindingProbe(); new Uint8Array(p.sharedBytes)[0] = 9; new Uint8Array(p.sharedBytes)[0]"));
        }
    }
}
