namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Scripting;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class FireEventTests
    {
        [Test]
        public async Task InvokeFunctionOnLoadEventShouldFireDelayed()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
            var html = "<!doctype html><div id=result></div><script>document.addEventListener('load', function () { document.querySelector('#result').textContent = 'done'; }, false);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html))
                .WhenStable();
            var div = document.QuerySelector("#result");
            Assert.AreEqual("done", div.TextContent);
        }

        [Test]
        public async Task InvokeFunctionOnCustomEvent()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
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
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
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
            var log = service.GetOrCreateJint(document).GetValue("log").AsArray();

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
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
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
            var clicked = service.GetOrCreateJint(document).GetValue("clicked").AsBoolean();
            Assert.IsTrue(clicked);
        }

        [Test]
        public async Task AddAndRemoveClickHandlerWontExecute()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
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
            var clicked = service.GetOrCreateJint(document).GetValue("clicked").AsBoolean();
            Assert.IsFalse(clicked);
        }

        [Test]
        public async Task AddAndInvokeClickHandlerWillChangeCapturedValue()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
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
            var clicked = service.GetOrCreateJint(document).GetValue("clicked").AsBoolean();
            Assert.IsTrue(clicked);
        }

        [Test]
        public async Task AddAndInvokeClickHandlerWithStringFunctionWontWork()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
            var html = @"<!doctype html>
<html>
<body>
<script>
var clicked = false;
document.onclick = 'clicked = true;';
document.onclick();
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var clicked = service.GetOrCreateJint(document).GetValue("clicked").AsBoolean();
            Assert.IsFalse(clicked);
        }

        [Test]
        public async Task BodyOnloadWorksWhenSetAsAttributeInitially()
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop();
            var html = @"<!doctype html>
<html>
<body onload='window.foo = 2+3'>
<script>
window.foo = 1.0;
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html))
                .WhenStable();

            var value = document.ExecuteScript("window.foo");
            Assert.AreEqual(5.0, value);
        }

        [Test]
        public async Task BodyOnloadWorksWhenSetAsAttributeLater()
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop();
            var html = @"<!doctype html>
<html>
<body>
<script>
window.foo = 1.0;
document.body.setAttribute('onload', 'window.foo = 2+3');
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html))
                .WhenStable();
            var value = document.ExecuteScript("window.foo");
            Assert.AreEqual(5.0, value);
        }

        [Test]
        public async Task SetTimeoutWithNormalFunction()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
            var html = @"<!doctype html>
<html>
<body>
<script>
var completed = false;
setTimeout(function () {
  completed = true;
}, 0);
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html))
                .WhenStable();
            var result = service.GetOrCreateJint(document).GetValue("completed").AsBoolean();
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DomContentLoadedEventIsFired_Issue50()
        {
            //TODO Check this as well on the window level - currently works
            //only against document (se AngleSharp#789)
            var cfg = Configuration.Default.WithJs().WithEventLoop();
            var html = @"<!doctype html>
<html>
<body>
<script>
document.addEventListener('DOMContentLoaded', function() {
  var element = document.createElement('div');
  element.textContent = 'Success!';
  document.body.appendChild(element);
});
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html))
                .WhenStable();

            var div = document.QuerySelector("div");
            Assert.AreEqual("Success!", div?.TextContent);
        }

        [Test]
        public async Task DocumentLoadEventIsFired_Issue42()
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop();
            var html = @"<!doctype html>
<html>
<body>
<script>
window.onload = function() {
  var element = document.createElement('div');
  element.textContent = 'Success!';
  document.body.appendChild(element);
};
</script>
</body>";
            var context = BrowsingContext.New(cfg);
            var document = await context.OpenAsync(m => m.Content(html))
                .WhenStable();

            var div = document.QuerySelector("div");
            Assert.AreEqual("Success!", div?.TextContent);
        }

        [Test]
        public async Task SetTimeoutWithStringAsFunction()
        {
            var service = new JsScriptingService();
            var cfg = Configuration.Default.With(service).WithEventLoop();
            var html = @"<!doctype html>
<html>
<body>
<script>
var completed = false;
setTimeout('completed = true;', 0);
</script>
</body>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html))
                .WhenStable();

            var result = service.GetOrCreateJint(document).GetValue("completed").AsBoolean();
            Assert.IsTrue(result);
        }
    }
}
