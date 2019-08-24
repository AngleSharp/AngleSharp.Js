namespace AngleSharp.Js.Tests
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
            var configuration = Helpers.GetCssConfig()
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(configuration);
            return context.OpenAsync(url);
        }

        [Test]
        public async Task RunHtml5Test()
        {
            if (Helpers.IsNetworkAvailable())
            {
                var target = "https://html5test.com";
                var document = await LoadPage(target);
                await document.WaitForReadyAsync();
                var result = document.QuerySelector("#score > .pointsPanel > h2 > strong");
                Assert.IsNotNull(result);
                var points = result?.TextContent ?? "0";
                Assert.AreNotEqual("0", points);
            }
        }
    }
}
