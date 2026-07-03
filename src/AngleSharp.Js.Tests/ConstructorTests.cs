using System.Linq;
using AngleSharp.Dom;

namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public async Task CustomEventConstructedViaConstructor()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><div id=result></div><script>var ev = new CustomEvent('foo'); document.querySelector('#result').textContent = ev.type;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.QuerySelector("#result").TextContent;
            Assert.AreEqual("foo", result);
        }

        [Test]
        public async Task CustomEventConstructedWithBubblesFalse()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><div id=result></div><script>var ev = new CustomEvent('foo', { bubbles: false }); document.querySelector('#result').textContent = ev.bubbles;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.QuerySelector("#result").TextContent;
            Assert.AreEqual("false", result);
        }

        [Test]
        public async Task CustomEventConstructedWithBubblesTrue()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><div id=result></div><script>var ev = new CustomEvent('foo', { bubbles: true }); document.querySelector('#result').textContent = ev.bubbles;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var result = document.QuerySelector("#result").TextContent;
            Assert.AreEqual("true", result);
        }

        [Test]
        public void CustomEventConstructedConcurrently()
        {
            var ctx1 = BrowsingContext.New(Configuration.Default.WithJs());
            var ctx2 = BrowsingContext.New(Configuration.Default.WithJs());
            var html = "<!doctype html><div id=result></div><script>var ev = new CustomEvent('foo'); document.querySelector('#result').textContent = ev.type;</script>";
            Parallel.Invoke(() => Assert(ctx1), () => Assert(ctx2));
            return;

            void Assert(IBrowsingContext context) => Task
                .Run(async () =>
                {
                    var document = await context.OpenAsync(m => m.Content(html));
                    var result = document.QuerySelector("#result").TextContent;
                    NUnit.Framework.Assert.AreEqual("foo", result);
                })
                .Wait();
        }
    }
}
