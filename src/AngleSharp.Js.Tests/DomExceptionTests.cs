namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The errors of the DOM, which scripts tell apart by their name.
    /// </summary>
    [TestFixture]
    public class DomExceptionTests
    {
        private static Task<String> Eval(String body) => $"(function () {{ {body} }})()".EvalScriptAsync();

        private static String Describe(String statement) =>
            $"try {{ {statement}; return 'no error'; }} catch (e) {{ return [e.name, e.code, e instanceof DOMException, e instanceof Error, e.message.length > 0].join(); }}";

        [Test]
        public async Task IsAvailableOnTheWindow()
        {
            Assert.AreEqual("function,true", await Eval("return [typeof DOMException, window.DOMException === DOMException].join();"));
        }

        [Test]
        public async Task CanBeCreatedByScripts()
        {
            var result = await Eval(@"
                var e = new DOMException('what', 'NotFoundError');
                return [e.message, e.name, e.code, e instanceof DOMException, e instanceof Error, String(e), typeof e.stack].join();");

            Assert.AreEqual("what,NotFoundError,8,true,true,NotFoundError: what,string", result);
        }

        [Test]
        public async Task ItsStackTellsWhoMadeIt()
        {
            var result = await Eval(@"
                function makesIt() { return new DOMException('m'); }
                return String(makesIt().stack.indexOf('makesIt') >= 0);");

            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task ItsStackTellsWhoCalledTheDom()
        {
            var result = await Eval(@"
                function callsTheDom() { document.querySelector('!!'); }
                try { callsTheDom(); return 'no error'; } catch (e) { return String(e.stack.indexOf('callsTheDom') >= 0); }");

            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task HasDefaults()
        {
            Assert.AreEqual(",Error,0", await Eval("var e = new DOMException(); return [e.message, e.name, e.code].join();"));
        }

        [Test]
        public async Task UnknownNameHasNoCode()
        {
            Assert.AreEqual("Custom,0", await Eval("var e = new DOMException('m', 'Custom'); return [e.name, e.code].join();"));
        }

        [Test]
        public async Task CannotBeCalledWithoutNew()
        {
            Assert.AreEqual("TypeError", await Eval("try { DOMException('m'); return 'no error'; } catch (e) { return e.name; }"));
        }

        [Test]
        public async Task HasItsConstants()
        {
            Assert.AreEqual("8,12,25,18", await Eval("return [DOMException.NOT_FOUND_ERR, DOMException.SYNTAX_ERR, DOMException.DATA_CLONE_ERR, new DOMException().SECURITY_ERR].join();"));
        }

        [Test]
        public async Task NameAndCodeCannotBeChangedByAssigning()
        {
            Assert.AreEqual("NotFoundError,8", await Eval("var e = new DOMException('m', 'NotFoundError'); e.name = 'x'; e.code = 1; return [e.name, e.code].join();"));
        }

        [TestCase("document.querySelector('!!')", "SyntaxError,12,true,true,true")]
        [TestCase("document.documentElement.appendChild(document.documentElement)", "HierarchyRequestError,3,true,true,true")]
        [TestCase("atob('*')", "InvalidCharacterError,5,true,true,true")]
        [TestCase("document.createElement('1 2')", "InvalidCharacterError,5,true,true,true")]
        [TestCase("performance.measure('m', 'no such mark')", "SyntaxError,12,true,true,true")]
        public async Task IsWhatTheDomThrows(String statement, String expected)
        {
            Assert.AreEqual(expected, await Eval(Describe(statement)));
        }

        [Test]
        public async Task OtherErrorsOfTheDomStayErrors()
        {
            var result = await Eval("try { performance.mark(); return 'no error'; } catch (e) { return [e.name, e instanceof DOMException].join(); }");

            Assert.AreEqual("TypeError,false", result);
        }
    }
}
