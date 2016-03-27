namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class PageTests
    {
        static Task<IDocument> LoadPage(String url)
        {
            var configuration = Configuration.Default.WithJavaScript().WithCss().WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true);
            var context = BrowsingContext.New(configuration);
            return context.OpenAsync(url);
        }

        //[Test]
        public async Task RunHtml5Test()
        {
            if (Helper.IsNetworkAvailable())
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
            if (Helper.IsNetworkAvailable())
            {
                var target = "https://meadjohnson.world.tmall.com/search.htm?search=y&orderType=defaultSort&scene=taobao_shop";
                var document = await LoadPage(target);
                var prices = document.QuerySelectorAll("span.c-price");
                Assert.AreNotEqual(0, prices.Length);
            }
        }
    }
}
