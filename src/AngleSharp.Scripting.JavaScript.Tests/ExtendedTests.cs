namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom.Html;
    using AngleSharp.Extensions;
    using AngleSharp.Scripting.JavaScript.Services;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExtendedTests
    {
        [Test]
        public async Task BooleanValueCanBeConvertedToString()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><iframe></iframe><script>document.querySelector('iframe').setAttribute('allowfullscreen', true);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var iframe = document.QuerySelector("iframe");
            Assert.AreEqual(1, iframe.Attributes.Length);
            Assert.AreEqual("allowfullscreen", iframe.Attributes[0].Name);
            Assert.AreEqual("true", iframe.Attributes[0].Value);
        }

        [Test]
        public async Task ObjectValueCanBeConvertedToString()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><iframe></iframe><script>document.querySelector('iframe').setAttribute('allowfullscreen', window);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var iframe = document.QuerySelector("iframe");
            Assert.AreEqual(1, iframe.Attributes.Length);
            Assert.AreEqual("allowfullscreen", iframe.Attributes[0].Name);
            Assert.AreEqual("[object Window]", iframe.Attributes[0].Value);
        }

        [Test]
        public async Task NumericValueCanBeConvertedToString()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><iframe></iframe><script>document.querySelector('iframe').setAttribute('allowfullscreen', 2.6);</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var iframe = document.QuerySelector("iframe");
            Assert.AreEqual(1, iframe.Attributes.Length);
            Assert.AreEqual("allowfullscreen", iframe.Attributes[0].Name);
            Assert.AreEqual("2.6", iframe.Attributes[0].Value);
        }

        [Test]
        public async Task BooleanConversionForStringsTakesPlace()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><option></option><script>document.querySelector('option').disabled = 'a';</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(true, option.IsDisabled);
        }

        [Test]
        public async Task BooleanConversionForNumbersTakesPlace()
        {
            var service = new JavaScriptProvider();
            var cfg = Configuration.Default.With(service);
            var html = "<!doctype html><option></option><script>document.querySelector('option').disabled = 1;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(true, option.IsDisabled);
        }
    }
}
