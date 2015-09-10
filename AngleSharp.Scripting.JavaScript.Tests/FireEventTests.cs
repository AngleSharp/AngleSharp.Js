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

        [Test]
        public async Task InvokeLoadEventFromJsAndCustomEventFromJsAndCs()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = @"<!doctype html>
<html>
<body>
<script>
var log = [];
log.push('1');
document.addEventListener('load', function() {
    log.push('5');
}, false);
document.addEventListener('hello', function() {
    log.push('3');
}, false);
log.push('2');
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var log = service.Engine.GetJint(document).GetValue("log").AsArray();
            document.AddEventListener("hello", (s, ev) =>
            {
                log.Put(log.Get("length").AsNumber().ToString(), "4", false);
            });

            var e = document.CreateEvent("event");
            e.Init("hello", false, false);
            document.Dispatch(e);
            
            await Task.Delay(50);
            Assert.AreEqual(5.0, log.Get("length").AsNumber());
            Assert.AreEqual("1", log.Get("0").AsString());
            Assert.AreEqual("2", log.Get("1").AsString());
            Assert.AreEqual("5", log.Get("2").AsString());
            Assert.AreEqual("3", log.Get("3").AsString());
            Assert.AreEqual("4", log.Get("4").AsString());
        }
    }
}
