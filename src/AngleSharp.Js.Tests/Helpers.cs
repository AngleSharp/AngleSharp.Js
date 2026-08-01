namespace AngleSharp.Js.Tests
{
    using AngleSharp.Browser.Dom.Events;
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
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
            var cfg = Configuration.Default.WithJs().WithEventLoop().WithConsoleLogger(context => console);
            var html = $"<!doctype html><script>console.log({source})</script>";
            await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            return console.Content.ToString().Trim();
        }

        internal static IConfiguration GetCssConfig() =>
            Configuration.Default
                .WithJs()
                .WithNavigator()
                .WithEventLoop()
                .WithCss()
                .WithRenderDevice();

        internal static List<Exception> CollectErrors(this IBrowsingContext context)
        {
            var errors = new List<Exception>();
            context.AddEventListener("error", (_, ev) =>
            {
                if (ev is TrackEvent trackEvent)
                {
                    errors.Add(trackEvent.Error);
                }
            });
            return errors;
        }

        internal static async Task<IElement> WaitForSelectorAsync(this IDocument document, String selector, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow.Add(timeout);

            while (DateTime.UtcNow < deadline)
            {
                IElement element = null;
                await document.Then(current => element = current.QuerySelector(selector)).ConfigureAwait(false);

                if (element != null)
                {
                    return element;
                }

                await Task.Delay(100).ConfigureAwait(false);
            }

            return null;
        }

        public static async Task<String> EvalScriptsAsync(this IEnumerable<String> sources)
        {
            var cfg = GetCssConfig()
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var content = String.Join("</script><script>", sources);
            var html = $"<!doctype html><div id=result></div><script>{content}</script>";
            var document = await BrowsingContext.New(cfg)
                .OpenAsync(m => m.Content(html))
                .WhenStable()
                .ConfigureAwait(false);
            return document.GetElementById("result").InnerHtml;
        }

        public static Boolean IsNetworkAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Assert.Inconclusive("No network has been detected. Test skipped.");
                return false;
            }

            return true;
        }
    }
}
