namespace AngleSharp.Scripting.JavaScript.Tests
{
    using Mocks;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;

    static class Helpers
    {
        public static async Task<String> EvalScriptAsync(this String source)
        {
            var console = new ConsoleLogger();
            var cfg = Configuration.Default.WithJavaScript().WithConsoleLogger(context => console);
            var html = "<!doctype html><script>console.log(" + source + ")</script>";
            await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            return console.Content.ToString().Trim();
        }

        public static async Task<String> EvalScriptsAsync(this IEnumerable<String> sources)
        {
            var cfg = Configuration.Default.WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true).WithJavaScript().WithCss();
            var scripts = "<script>" + String.Join("</script><script>", sources) + "</script>";
            var html = "<!doctype html><div id=result></div>" + scripts;
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            return document.GetElementById("result").InnerHtml;
        }

        public static Boolean IsNetworkAvailable()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                return true;
            }

            Assert.Inconclusive("No network has been detected. Test skipped.");
            return false;
        }
    }
}
