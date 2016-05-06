namespace AngleSharp.Scripting.JavaScript.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class PageTests
    {
        //[Test]
        public async Task RunHtml5Test()
        {
            var target = "http://html5test.com";
            var config = Configuration.Default.WithJavaScript().WithCss().WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true);
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(target);
            var points = document.QuerySelector("#score > .pointsPanel > h2 > strong").TextContent;
            Assert.AreNotEqual("0", points);
        }

        //[Test]
        public async Task RunTaobao()
        {
            var target = "https://meadjohnson.world.tmall.com/search.htm?search=y&orderType=defaultSort&scene=taobao_shop";
            var config = Configuration.Default.WithJavaScript().WithCss().WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true);
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(target);
            var prices = document.QuerySelectorAll("span.c-price");
            Assert.AreNotEqual(0, prices.Length);
        }
    }
}
