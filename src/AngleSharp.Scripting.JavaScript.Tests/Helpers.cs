namespace AngleSharp.Scripting.JavaScript.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.NetworkInformation;
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

        public static Boolean IsNetworkAvailable()
        {
            if (IsNetworkAvailable(0))
            {
                return true;
            }

            Assert.Inconclusive("No network has been detected. Test skipped.");
            return false;
        }

        public static Boolean IsNetworkAvailable(Int64 minimumSpeed)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // discard because of standard reasons
                    if ((ni.OperationalStatus != OperationalStatus.Up) ||
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                        continue;

                    // this allow to filter modems, serial, etc.
                    // I use 10000000 as a minimum speed for most cases
                    if (ni.Speed < minimumSpeed)
                        continue;

                    // discard virtual cards (virtual box, virtual pc, etc.)
                    if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                        continue;

                    // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                    if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                        continue;

                    return true;
                }
            }

            return false;
        }
    }
}
