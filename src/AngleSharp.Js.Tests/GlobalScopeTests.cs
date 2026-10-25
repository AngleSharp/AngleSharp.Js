namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The members of the window are configurable and writable, so a page's own globals replace
    /// them as in a browser - a script declaring var item lost to window's item(), and every use
    /// of it failed.
    /// </summary>
    [TestFixture]
    public class GlobalScopeTests
    {
        [TestCase("var item = 1; typeof item", "number")]
        [TestCase("var open = 'x'; typeof open", "string")]
        [TestCase("var stop = 1; stop + 1", "2")]
        [TestCase("function close() { return 'mine'; } close()", "mine")]
        [TestCase("var Node = 5; typeof Node", "number")]
        [TestCase("var unrelated = 1; typeof unrelated", "number")]
        public async Task GlobalsNamedLikeWindowMembersReplaceThem(String script, String expected)
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><p>x</p>"));

            Assert.AreEqual(expected, document.ExecuteScript(script)?.ToString());
        }

        [Test]
        public async Task WindowMembersStayAvailableWithoutOwnGlobals()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = await context.OpenAsync(m => m.Content("<!doctype html><p>x</p>"));

            Assert.AreEqual("function", document.ExecuteScript("typeof setTimeout"));
            Assert.AreEqual("function", document.ExecuteScript("typeof Node"));
            Assert.AreEqual("P", document.ExecuteScript("document.querySelector('p').tagName"));
        }
    }
}
