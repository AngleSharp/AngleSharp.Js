namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class IntegrationTests
    {
        private static Task<IDocument> LoadPage(String url)
        {
            var configuration = Helpers.GetCssConfig().WithDefaultLoader().WithCookies();
            var context = BrowsingContext.New(configuration);
            return context.OpenAsync(url);
        }

        [Test]
        public async Task PageEcoEnergyShouldLoadFine()
        {
            if (Helpers.IsNetworkAvailable())
            {
                var document = await LoadPage("http://www.eco2energie.biz/");
                Assert.IsNotNull(document);
            }
        }

        [Test]
        public async Task PageGamestarShouldLoadFine()
        {
            if (Helpers.IsNetworkAvailable())
            {
                var document = await LoadPage("http://www.gamestar.de/index.cfm?pid=103&op=10");
                Assert.IsNotNull(document);
            }
        }
    }
}
