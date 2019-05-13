namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class EcoTests
    {
        [Test]
        public async Task GetCssSheetRuleFromJavaScript()
        {
            var config = Configuration.Default.WithCss().WithJs();
            var context = BrowsingContext.New(config);
            var style = "body { color: red; }";
            var script = "var sheet = document.querySelector('style').sheet; var color = sheet.cssRules[0].style.color; document.querySelector('div').textContent = color;";
            var document = await context.OpenAsync(r => r.Content($"<style>{style}</style><div></div><script>{script}</script>"));
            Assert.AreEqual("rgba(255, 0, 0, 1)", document.QuerySelector("div").TextContent);
        }
    }
}
