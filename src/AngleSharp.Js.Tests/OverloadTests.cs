namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    [DomName("ProbeMode")]
    public enum ProbeMode
    {
        [DomName("near")]
        Near,
        [DomName("far")]
        Far,
    }

    [DomName("ProbeOptions")]
    public sealed class ProbeOptions
    {
        [DomName("left")]
        public Double? Left { get; set; }

        [DomName("headers")]
        public Dictionary<String, String> Headers { get; set; }

        [DomName("mode")]
        public ProbeMode Mode { get; set; } = ProbeMode.Near;
    }

    [DomExposed("Element")]
    public static class ProbeElementExtensions
    {
        [DomName("probe")]
        public static String Probe(this IElement element) => "none";

        [DomName("offsetHeight")]
        [DomAccessor(Accessors.Getter)]
        public static Int32 GetOffsetHeight(this IElement element) => 42;

        [DomName("probe")]
        public static String Probe(this IElement element, Double x, Double y) =>
            String.Format(CultureInfo.InvariantCulture, "xy:{0},{1}", x, y);

        [DomName("probe")]
        public static String Probe(this IElement element, Boolean flag) => "flag:" + flag;

        [DomName("probe")]
        public static String Probe(this IElement element, ProbeOptions options) =>
            String.Format(CultureInfo.InvariantCulture, "options:{0},{1}", options.Left?.ToString(CultureInfo.InvariantCulture) ?? "-", options.Mode) +
            (options.Headers is null ? "" : ",headers:" + String.Join("|", options.Headers.Select(header => header.Key + "=" + header.Value)));

        [DomName("probeRecord")]
        public static String ProbeRecord(this IElement element, IDictionary<String, Double> record) =>
            String.Join("|", record.Select(entry => entry.Key + "=" + entry.Value.ToString(CultureInfo.InvariantCulture)));
    }

    /// <summary>
    /// Registered so that this assembly counts as a library the engine exposes.
    /// </summary>
    sealed class ProbeService
    {
    }

    /// <summary>
    /// Several extension methods may share a name (scrollTo(x, y) and scrollTo(options)); the one
    /// fitting the arguments is called, and a plain object becomes a dictionary.
    /// </summary>
    [TestFixture]
    public class OverloadTests
    {
        [TestCase("probe()", "none")]
        [TestCase("probe(1, 2)", "xy:1,2")]
        [TestCase("probe(true)", "flag:True")]
        [TestCase("probe({ left: 5, mode: 'far' })", "options:5,Far")]
        [TestCase("probe({})", "options:-,Near")]
        [TestCase("probe({ headers: { Accept: 'application/json', 'X-Count': 2 } })", "options:-,Near,headers:Accept=application/json|X-Count=2")]
        [TestCase("probeRecord({ a: 1, b: '2.5' })", "a=1|b=2.5")]
        [TestCase("probe(undefined)", "none")]
        public async Task OverloadFittingTheArgumentsIsCalled(String call, String expected)
        {
            var cfg = Configuration.Default.WithJs().With(new ProbeService());
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content("<!doctype html><div id=d></div>"));

            Assert.AreEqual(expected, document.ExecuteScript($"document.getElementById('d').{call}")?.ToString());
        }

        [Test]
        public async Task MemberOfALibraryWinsOverTheStubOfThisOne()
        {
            var cfg = Configuration.Default.WithJs().With(new ProbeService());
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content("<!doctype html><div id=d></div>"));

            Assert.AreEqual("42", document.ExecuteScript("document.getElementById('d').offsetHeight")?.ToString());
        }
    }
}
