namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The TextEncoder and TextDecoder of the Encoding standard.
    /// </summary>
    [TestFixture]
    public class TextEncodingTests
    {
        private static Task<String> Eval(String body) => $"(function () {{ {body} }})()".EvalScriptAsync();

        [Test]
        public async Task EncoderIsAvailableOnTheWindowAndGlobalThis()
        {
            Assert.AreEqual("function,function,function", await Eval("return [typeof TextEncoder, typeof window.TextEncoder, typeof globalThis.TextEncoder].join();"));
        }

        [Test]
        public async Task EncoderTellsItsEncoding()
        {
            Assert.AreEqual("utf-8", await Eval("return new TextEncoder().encoding;"));
        }

        [TestCase("'abc'", "97,98,99")]
        [TestCase("'ä€😀'", "195,164,226,130,172,240,159,152,128")]
        [TestCase("''", "")]
        [TestCase("undefined", "")]
        [TestCase("'\\uD800'", "239,191,189")]
        public async Task EncoderEncodes(String input, String expected)
        {
            Assert.AreEqual(expected, await Eval($"return Array.from(new TextEncoder().encode({input})).join();"));
        }

        [Test]
        public async Task EncoderGivesAUint8Array()
        {
            Assert.AreEqual("true,3", await Eval("var r = new TextEncoder().encode('abc'); return [r instanceof Uint8Array, r.length].join();"));
        }

        [Test]
        public async Task EncodeIntoWritesWhatFits()
        {
            var result = await Eval(@"
                var target = new Uint8Array(5);
                var r = new TextEncoder().encodeInto('a€😀', target);
                return [r.read, r.written, Array.from(target).join('+')].join();");

            Assert.AreEqual("2,4,97+226+130+172+0", result);
        }

        [Test]
        public async Task EncodeIntoReadsWholeSurrogatePairs()
        {
            var result = await Eval(@"
                var target = new Uint8Array(4);
                var r = new TextEncoder().encodeInto('😀', target);
                return [r.read, r.written].join();");

            Assert.AreEqual("2,4", result);
        }

        [TestCase("new Uint8Array([104, 105])", "hi")]
        [TestCase("new Uint8Array([104, 105]).buffer", "hi")]
        [TestCase("new DataView(new Uint8Array([104, 105]).buffer)", "hi")]
        [TestCase("undefined", "")]
        public async Task DecoderDecodes(String input, String expected)
        {
            Assert.AreEqual(expected, await Eval($"return new TextDecoder().decode({input});"));
        }

        [Test]
        public async Task RoundTripKeepsText()
        {
            Assert.AreEqual("ä€😀", await Eval("return new TextDecoder().decode(new TextEncoder().encode('ä€😀'));"));
        }

        [Test]
        public async Task DecoderTellsWhatItWasCreatedWith()
        {
            Assert.AreEqual("utf-8,false,false,true,true", await Eval(@"
                var a = new TextDecoder();
                var b = new TextDecoder('utf-8', { fatal: true, ignoreBOM: true });
                return [a.encoding, a.fatal, a.ignoreBOM, b.fatal, b.ignoreBOM].join();"));
        }

        [Test]
        public async Task DecoderKnowsOtherEncodings()
        {
            Assert.AreEqual("ä", await Eval("return new TextDecoder('iso-8859-1').decode(new Uint8Array([228]));"));
        }

        [Test]
        public async Task DecoderRefusesUnknownEncodings()
        {
            Assert.AreEqual("RangeError", await Eval("try { new TextDecoder('no-such-encoding'); return 'no error'; } catch (e) { return e.name; }"));
        }

        [Test]
        public async Task DecoderReplacesInvalidInput()
        {
            Assert.AreEqual("a�b", await Eval("return new TextDecoder().decode(new Uint8Array([97, 255, 98]));"));
        }

        [Test]
        public async Task FatalDecoderThrowsOnInvalidInput()
        {
            Assert.AreEqual("TypeError", await Eval("try { new TextDecoder('utf-8', { fatal: true }).decode(new Uint8Array([255])); return 'no error'; } catch (e) { return e.name; }"));
        }

        [Test]
        public async Task DecoderDropsAByteOrderMark()
        {
            Assert.AreEqual("hi", await Eval("return new TextDecoder().decode(new Uint8Array([239, 187, 191, 104, 105]));"));
        }

        [Test]
        public async Task DecoderKeepsAByteOrderMarkWhenAskedTo()
        {
            Assert.AreEqual("3", await Eval("return String(new TextDecoder('utf-8', { ignoreBOM: true }).decode(new Uint8Array([239, 187, 191, 104, 105])).length);"));
        }

        [Test]
        public async Task StreamingDecoderWaitsForTheRestOfACharacter()
        {
            Assert.AreEqual("|€", await Eval(@"
                var d = new TextDecoder();
                var first = d.decode(new Uint8Array([226, 130]), { stream: true });
                var second = d.decode(new Uint8Array([172]));
                return first + '|' + second;"));
        }
    }
}
