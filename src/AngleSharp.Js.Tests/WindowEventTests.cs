namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// window.event is the event a listener runs for, and undefined otherwise - a script reading
    /// the global event outside of a listener (Elementor's carousels) threw a ReferenceError.
    /// </summary>
    [TestFixture]
    public class WindowEventTests
    {
        [Test]
        public async Task EventIsUndefinedOutsideOfListeners()
        {
            var document = await OpenAsync();

            Assert.AreEqual("undefined", document.ExecuteScript("typeof event"));
            Assert.AreEqual("undefined", document.ExecuteScript("typeof window.event"));
            Assert.AreEqual("true", document.ExecuteScript("'event' in window").ToString().ToLowerInvariant());
        }

        [Test]
        public async Task EventIsTheOneOfTheListenerRunning()
        {
            var document = await OpenAsync();

            var result = document.ExecuteScript("""
                var seen = [];
                var p = document.querySelector('p');
                p.addEventListener('click', function (e) {
                    seen.push(event === e, window.event === e);
                    p.dispatchEvent(new Event('inner'));
                    seen.push(event === e);
                });
                p.addEventListener('inner', function (e) { seen.push(event.type); });
                p.dispatchEvent(new Event('click'));
                seen.push(typeof event);
                seen.join(',')
                """);

            Assert.AreEqual("true,true,inner,true,undefined", result);
        }

        [Test]
        public async Task EventHandlersSeeTheEventToo()
        {
            var document = await OpenAsync("<p onclick=\"window.seen = window.event.type\">x</p>");

            var result = document.ExecuteScript("document.querySelector('p').click(); window.seen");

            Assert.AreEqual("click", result);
        }

        [Test]
        public async Task EventCanBeReplacedByScript()
        {
            var document = await OpenAsync();

            Assert.AreEqual("5", document.ExecuteScript("window.event = 5; window.event").ToString());
        }

        private static Task<AngleSharp.Dom.IDocument> OpenAsync(String body = "<p>x</p>")
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            return context.OpenAsync(m => m.Content("<!doctype html>" + body));
        }
    }
}
