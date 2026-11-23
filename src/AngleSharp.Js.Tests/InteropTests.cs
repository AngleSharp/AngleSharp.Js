namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// What a library can give to scripts besides objects of the DOM: namespaces with interfaces
    /// of their own, functions, BigInts, shared buffers, promises and errors of its own.
    /// </summary>
    [TestFixture]
    public class InteropTests
    {
        private static async Task<String> Run(String body)
        {
            //  a service of this assembly makes it one of the libraries of the scripts
            var cfg = Configuration.Default.WithJs().WithEventLoop().WithOnly<InteropService>(_ => new InteropService());
            var script = $"(async function () {{ {body} }})().then(function (r) {{ document.title = 'ok:' + r; }}, function (e) {{ document.title = 'error:' + e; }});";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content($"<!doctype html><title></title><script>{script}</script>"));

            for (var i = 0; i < 200 && String.IsNullOrEmpty(document.Title); i++)
            {
                await Task.Delay(20);
            }

            return document.Title;
        }

        [Test]
        public async Task LibraryOfAServiceGivenByACreatorIsFound()
        {
            Assert.AreEqual("ok:object", await Run("return typeof Interop;"));
        }

        [Test]
        public async Task NamespaceIsAnObjectWithItsMembersAndInterfaces()
        {
            var result = await Run(@"
                return [typeof Interop, Object.prototype.toString.call(Interop), Interop.twice(21), typeof Interop.Thing,
                    new Interop.Thing(5).size, new Interop.Thing(5) instanceof Interop.Thing, typeof Thing, 'twice' in window,
                    Object.keys(window).indexOf('Interop')].join();");

            Assert.AreEqual("ok:object,[object Interop],42,function,5,true,undefined,false,-1", result);
        }

        [Test]
        public async Task DelegateIsAFunction()
        {
            var result = await Run(@"
                var add = Interop.adder();
                var join = Interop.joiner();
                return [typeof add, add(2, 3), add.length, Interop.adder() === add, join(1, 'a', undefined, true), join.length, Interop.echo(join) === join, Interop.isJoiner(join)].join();");

            Assert.AreEqual("ok:function,5,2,true,Double:1|String:a|Missing|Boolean:True,0,true,true", result);
        }

        [Test]
        public async Task FunctionOfAScriptIsAVariadicDelegate()
        {
            var result = await Run(@"
                var f = function () { return Array.prototype.join.call(arguments, '+'); };
                return [Interop.call(f), Interop.echo(f) === f].join();");

            Assert.AreEqual("ok:1+x+,true", result);
        }

        [Test]
        public async Task BigIntegerIsABigInt()
        {
            var result = await Run("var b = Interop.big(); return [typeof b, b === 2n ** 70n, Interop.bigOf(12n)].join();");
            Assert.AreEqual("ok:bigint,true,BigInteger:12", result);
        }

        [Test]
        public async Task MemoryIsASharedArrayBuffer()
        {
            var result = await Run(@"
                var buffer = Interop.buffer();
                new Uint8Array(buffer)[1] = 7;
                return [buffer instanceof ArrayBuffer, buffer.byteLength, buffer === Interop.buffer(), Interop.byteAt(1)].join();");

            Assert.AreEqual("ok:true,4,true,7", result);
        }

        [Test]
        public async Task PromiseIsATask()
        {
            var result = await Run(@"
                var a = await Interop.later(Promise.resolve('given'));
                var b = await Interop.later('value');
                var c = await Interop.later(Promise.reject(new Error('no'))).catch(function (e) { return 'caught ' + e.message; });
                return [a, b, c].join();");

            Assert.AreEqual("ok:given!,value!,caught no", result);
        }

        [Test]
        public async Task ExceptionOfTheDomIsAnErrorOfItsClass()
        {
            var result = await Run(@"
                try { Interop.fail(); } catch (e) {
                    return [e instanceof Interop.Failure, e instanceof Error, e.message, e.name, new Interop.Failure('made') instanceof Error].join();
                }");

            Assert.AreEqual("ok:true,true,it broke,Failure,true", result);
        }

        [Test]
        public async Task ExceptionOfTheDomCanBeAwaited()
        {
            var result = await Run(@"
                var made = new Interop.Failure('made');
                try { await Promise.reject(made); } catch (e) { return [e === made, e.message, typeof e.stack].join(); }");

            Assert.AreEqual("ok:true,made,string", result);
        }

        [Test]
        public async Task FunctionOfADelegateIsTheDelegateForAnObject()
        {
            Assert.AreEqual("ok:Variadic", await Run("return Interop.typeOf(Interop.joiner());"));
        }

        [Test]
        public async Task CssIsANamespace()
        {
            var document = await BrowsingContext.New(Helpers.GetCssConfig()).OpenAsync(m => m.Content("<!doctype html><title></title><script>document.title = [typeof CSS, CSS.escape('a b'), 'supports' in window].join()</script>"));
            Assert.AreEqual("object,a\\ b,false", document.Title);
        }

        [Test]
        public async Task MixinHasNoInterfaceObject()
        {
            Assert.AreEqual("undefined,function", await "[typeof WindowBase64, typeof btoa].join()".EvalScriptAsync());
        }
    }

    public sealed class InteropService
    {
    }

    public delegate Object Variadic(Object[] arguments);

    [DomName("Interop")]
    [DomExposed("Window")]
    public static class InteropNamespace
    {
        private static readonly Byte[] Bytes = new Byte[4];
        private static readonly Func<Double, Double, Double> Add = (a, b) => a + b;
        private static readonly Variadic Join = arguments => String.Join("|", Array.ConvertAll(arguments, m => m == System.Reflection.Missing.Value ? "Missing" : m.GetType().Name + ":" + m));

        [DomName("twice")]
        public static Double Twice(this IWindow window, Double value) => 2 * value;

        [DomName("adder")]
        public static Func<Double, Double, Double> Adder() => Add;

        [DomName("joiner")]
        public static Variadic Joiner() => Join;

        [DomName("call")]
        public static Object Call(Variadic callback) => callback(new Object[] { 1, "x", null });

        [DomName("isJoiner")]
        public static Boolean IsJoiner(Variadic callback) => ReferenceEquals(callback, Join);

        [DomName("echo")]
        public static Variadic Echo(Variadic callback) => callback;

        [DomName("big")]
        public static Object Big() => BigInteger.Pow(2, 70);

        [DomName("bigOf")]
        public static String BigOf(Object value) => value.GetType().Name + ":" + value;

        [DomName("buffer")]
        public static Object Buffer() => new Memory<Byte>(Bytes);

        [DomName("byteAt")]
        public static Int32 ByteAt(Int32 index) => Bytes[index];

        [DomName("later")]
        public static async Task<String> Later(Task<String> value) => await value + "!";

        [DomName("typeOf")]
        public static String TypeOf(Object value) => value.GetType().Name;

        [DomName("fail")]
        public static void Fail() => throw new InteropFailure("it broke");
    }

    [DomName("Thing")]
    [DomExposed("Interop")]
    public sealed class InteropThing
    {
        [DomConstructor]
        public InteropThing(Int32 size)
        {
            Size = size;
        }

        [DomName("size")]
        public Int32 Size { get; }
    }

    [DomName("Failure")]
    [DomExposed("Interop")]
    public sealed class InteropFailure : Exception
    {
        [DomConstructor]
        public InteropFailure(String message)
            : base(message)
        {
        }

        [DomName("name")]
        public String Name => "Failure";
    }
}
