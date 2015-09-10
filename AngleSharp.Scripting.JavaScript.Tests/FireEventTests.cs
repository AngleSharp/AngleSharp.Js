namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom.Events;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class FireEventTests
    {
        [Test]
        public async Task InvokeFunctionOnLoadEventShouldFireDelayed()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><div id=result></div><script>document.addEventListener('load', function () { document.querySelector('#result').textContent = 'done'; }, false);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var div = document.QuerySelector("#result");
            Assert.AreEqual("", div.TextContent);
            await Task.Delay(50);
            Assert.AreEqual("done", div.TextContent);
        }

        [Test]
        public async Task InvokeFunctionOnCustomEvent()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><div id=result>0</div><script>var i = 0; document.addEventListener('hello', function () { i++; document.querySelector('#result').textContent = i.toString(); }, false);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var div = document.QuerySelector("#result");
            document.Dispatch(new CustomEvent("hello"));
            Assert.AreEqual("1", div.TextContent);
            document.Dispatch(new CustomEvent("hello"));
            Assert.AreEqual("2", div.TextContent);
        }
    }
}
