namespace AngleSharp.Scripting.JavaScript.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public async Task PageEcoEnergyShouldLoadFine()
        {
            var config = Configuration.Default.WithDefaultLoader().WithCss().WithCookies().WithJavaScript();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync("http://www.eco2energie.biz/");
            Assert.IsNotNull(document);
        }
    }
}
