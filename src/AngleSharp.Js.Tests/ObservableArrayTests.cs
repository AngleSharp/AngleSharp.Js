namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    [DomExposed("Document")]
    public static class ProbeObservableArrayExtensions
    {
        private static readonly ConditionalWeakTable<IDocument, List<IElement>> Lists = new ConditionalWeakTable<IDocument, List<IElement>>();

        public static List<IElement> Of(IDocument document) => Lists.GetValue(document, _ => new List<IElement>());

        [DomName("probeElements")]
        [DomAccessor(Accessors.Getter)]
        public static IList<IElement> GetProbeElements(this IDocument document) => Of(document);

        [DomName("probeElements")]
        [DomAccessor(Accessors.Setter)]
        public static void SetProbeElements(this IDocument document, IEnumerable<IElement> elements)
        {
            var list = Of(document);
            list.Clear();
            list.AddRange(elements);
        }
    }

    /// <summary>
    /// A mutable IList of the DOM is a WebIDL ObservableArray (e.g. adoptedStyleSheets): an
    /// array for scripts, whose changes go to the list.
    /// </summary>
    [TestFixture]
    public class ObservableArrayTests
    {
        [TestCase("Array.isArray(document.probeElements)", "True")]
        [TestCase("document.probeElements === document.probeElements", "True")]
        [TestCase("document.probeElements = document.probeElements.concat([document.body]); document.probeElements[0] === document.body", "True")]
        [TestCase("document.probeElements.push(document.body, document.head); document.probeElements.pop(); document.probeElements.length", "1")]
        [TestCase("try { document.probeElements.push(1); 'none' } catch (e) { e.name }", "TypeError")]
        public async Task MutableListIsAnArray(String script, String expected)
        {
            var document = await BrowsingContext.New(Configuration.Default.WithJs().With(new ProbeService())).OpenAsync(m => m.Content("<!doctype html><body>"));

            Assert.AreEqual(expected, document.ExecuteScript(script)?.ToString());
        }

        [Test]
        public async Task ChangesOfTheArrayGoToTheList()
        {
            var document = await BrowsingContext.New(Configuration.Default.WithJs().With(new ProbeService())).OpenAsync(m => m.Content("<!doctype html><body>"));

            document.ExecuteScript("document.probeElements.push(document.body)");

            Assert.AreEqual(new[] { document.Body }, ProbeObservableArrayExtensions.Of(document));
        }
    }
}
