namespace AngleSharp.Js.Tests
{
    using AngleSharp.Js;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class FunctionSourceTextTests
    {
        [Test]
        public async Task FunctionOfAScriptGivesItsSource()
        {
            // MediaWiki stores its modules in localStorage as the source of their functions
            var result = await ScriptEvalTests.EvaluateComplexScriptAsync(
                "function f(a, b) { return a + b; } var arrow = (x) => x * 2; class A { m() { return 1; } }",
                ScriptEvalTests.SetResult("[String(f), String(arrow), String(A.prototype.m)].join('|')"));

            // the result is read as HTML
            Assert.AreEqual("function f(a, b) { return a + b; }|(x) =&gt; x * 2|m() { return 1; }", result);
        }

        [Test]
        public async Task FunctionOfEvalAndTheFunctionConstructorGivesItsSource()
        {
            var result = await ScriptEvalTests.EvaluateComplexScriptAsync(
                ScriptEvalTests.SetResult("[String(eval('(function g() { return 3; })')), new Function('a', 'return a').toString().indexOf('return a') >= 0].join('|')"));

            Assert.AreEqual("function g() { return 3; }|true", result);
        }

        [Test]
        public async Task SourceOfAFunctionCanBeEvaluatedAgain()
        {
            var result = await ScriptEvalTests.EvaluateComplexScriptAsync(
                "var stored = String(function (x) { return x + 1; });",
                ScriptEvalTests.SetResult("eval('(' + stored + ')')(41)"));

            Assert.AreEqual("42", result);
        }

        [Test]
        public async Task BuiltInFunctionGivesNativeCode()
        {
            var result = await ScriptEvalTests.EvaluateComplexScriptAsync(ScriptEvalTests.SetResult("String(Math.max)"));

            StringAssert.Contains("[native code]", result);
        }

        [Test]
        public async Task SourceIsNotKeptIfTurnedOff()
        {
            var config = Configuration.Default
                .WithJs(new JsScriptingOptions { RetainFunctionSourceText = false })
                .WithEventLoop();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content("<script>function f() { return 1; }</script>"));

            StringAssert.Contains("[native code]", document.ExecuteScript("String(f)")?.ToString());
        }
    }
}
