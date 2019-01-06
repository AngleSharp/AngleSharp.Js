namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class PageTests
    {
        private static Task<IDocument> LoadPage(String url)
        {
            var configuration = Helpers.GetCssConfig().WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(configuration);
            return context.OpenAsync(url);
        }

        //[Test]
        public async Task RunHtml5Test()
        {
            if (Helpers.IsNetworkAvailable())
            {
                var target = "http://html5test.com";
                var document = await LoadPage(target);
                var points = document.QuerySelector("#score > .pointsPanel > h2 > strong").TextContent;
                Assert.AreNotEqual("0", points);
            }
        }

        //[Test]
        public async Task RunTaobao()
        {
            if (Helpers.IsNetworkAvailable())
            {
                var target = "https://meadjohnson.world.tmall.com/search.htm?search=y&orderType=defaultSort&scene=taobao_shop";
                var document = await LoadPage(target);
                var prices = document.QuerySelectorAll("span.c-price");
                Assert.AreNotEqual(0, prices.Length);
            }
        }
    }
}
