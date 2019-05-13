namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class ExtendedTests
    {
        [Test]
        public async Task BooleanValueCanBeConvertedToString()
        {
            var cfg = Configuration.Default.WithJs();
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
            var cfg = Configuration.Default.WithJs();
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
            var cfg = Configuration.Default.WithJs();
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
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><option></option><script>document.querySelector('option').disabled = 'a';</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(true, option.IsDisabled);
        }

        [Test]
        public async Task BooleanConversionForNumbersTakesPlace()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><option></option><script>document.querySelector('option').disabled = 1;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(true, option.IsDisabled);
        }

        [Test]
        public async Task BooleanConversionForObjectsTakesPlace()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><option></option><script>document.querySelector('option').selected = this;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(true, option.IsSelected);
        }

        [Test]
        public async Task BooleanConversionForArraysTakesPlace()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><option></option><script>document.querySelector('option').selected = [];</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(true, option.IsSelected);
        }

        [Test]
        public async Task BooleanConversionForZeroIsFalse()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><option></option><script>document.querySelector('option').selected =0;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(false, option.IsSelected);
        }

        [Test]
        public async Task BooleanConversionForUndefinedIsFalse()
        {
            var cfg = Configuration.Default.WithJs();
            var html = "<!doctype html><option></option><script>document.querySelector('option').selected = undefined;</script>";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            var option = document.QuerySelector<IHtmlOptionElement>("option");
            Assert.AreEqual(false, option.IsSelected);
        }
    }
}
