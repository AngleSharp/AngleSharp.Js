namespace AngleSharp.Scripting.JavaScript.Tests
{
    using Jint.Runtime;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class InteractionTests
    {
        [Test]
        public async Task ReadStoredJavaScriptValueFromCSharp()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><script>var foo = 'test';</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var foo = service.Engine.GetJint(document).GetValue("foo");
            Assert.AreEqual(Types.String, foo.Type);
            Assert.AreEqual("test", foo.AsString());
        }

        [Test]
        public async Task RunJavaScriptFunctionFromCSharp()
        {
            var service = new ScriptingService();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><script>function square(x) { return x * x; }</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var square = service.Engine.GetJint(document).GetValue("square");
            var result = square.Invoke(4);
            Assert.AreEqual(Types.Number, result.Type);
            Assert.AreEqual(16.0, result.AsNumber());
        }
    }
}
