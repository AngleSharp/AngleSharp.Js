namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom.Events;
    using AngleSharp.Scripting.JavaScript.Services;
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
log.push('a');
document.addEventListener('hello', function() {
    log.push('c');
}, false);
log.push('b');
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var log = service.Engine.GetJint(document).GetValue("log").AsArray();

            document.AddEventListener("hello", (s, ev) =>
            {
                log.Put(log.Get("length").AsNumber().ToString(), "d", false);
            });

            document.Dispatch(new Event("hello"));
            
            Assert.AreEqual(4.0, log.Get("length").AsNumber());
            Assert.AreEqual("a", log.Get("0").AsString());
            Assert.AreEqual("b", log.Get("1").AsString());
            Assert.AreEqual("c", log.Get("2").AsString());
            Assert.AreEqual("d", log.Get("3").AsString());
        }

        [Test]
        public async Task AddClickHandlerClassicallyWillExecute()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = @"<!doctype html>
<html>
<body>
<script>
var clicked = false;
document.onclick = function () {
    clicked = true;
};
document.dispatchEvent(new MouseEvent('click'));
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var clicked = service.Engine.GetJint(document).GetValue("clicked").AsBoolean();
            Assert.IsTrue(clicked);
        }

        [Test]
        public async Task AddAndRemoveClickHandlerWontExecute()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = @"<!doctype html>
<html>
<body>
<script>
var clicked = false;
document.onclick = function () {
    clicked = true;
};
document.onclick = undefined;
document.dispatchEvent(new MouseEvent('click'));
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var clicked = service.Engine.GetJint(document).GetValue("clicked").AsBoolean();
            Assert.IsFalse(clicked);
        }

        [Test]
        public async Task AddAndInvokeClickHandlerWillChangeCapturedValue()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = @"<!doctype html>
<html>
<body>
<script>
var clicked = false;
document.onclick = function () {
    clicked = true;
};
document.onclick();
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var clicked = service.Engine.GetJint(document).GetValue("clicked").AsBoolean();
            Assert.IsTrue(clicked);
        }
    }
}
