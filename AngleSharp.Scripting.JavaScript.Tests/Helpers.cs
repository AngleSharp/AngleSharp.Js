namespace AngleSharp.Scripting.JavaScript.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    static class Helpers
    {
        public static async Task<String> EvalScriptAsync(this String source)
        {
            var cfg = Configuration.Default.WithJavaScript();
            var stdOut = Console.Out;
            var output = new StringBuilder();
            Console.SetOut(new StringWriter(output));
            var html = "<!doctype html><script>console.log(" + source + ")</script>";
            await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            Console.SetOut(stdOut);
            return output.ToString().Trim();
        }

        public static async Task<String> EvalScriptsAsync(this IEnumerable<String> sources)
        {
            var cfg = Configuration.Default.WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true).WithJavaScript().WithCss();
            var scripts = "<script>" + String.Join("</script><script>", sources) + "</script>";
            var html = "<!doctype html><div id=result></div>" + scripts;
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content(html));
            return document.GetElementById("result").InnerHtml;
        }
    }
}
