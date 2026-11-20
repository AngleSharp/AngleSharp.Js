namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using NUnit.Framework;
    using System.Threading.Tasks;

    /// <summary>
    /// Behavior the Acid3 test (acid3.acidtests.org) relies on.
    /// </summary>
    [TestFixture]
    public class Acid3Tests
    {
        [Test]
        public async Task ConstantsAreOnTheInterfacesAndTheirInstances()
        {
            var result = await "[Node.ELEMENT_NODE, document.DOCUMENT_FRAGMENT_NODE, document.documentElement.TEXT_NODE, Range.START_TO_END, document.createRange().END_TO_START, Event.CAPTURING_PHASE, NodeFilter.SHOW_ELEMENT].join()".EvalScriptAsync();
            Assert.AreEqual("1,11,3,1,3,1,1", result);
        }

        [Test]
        public async Task ConstantsCannotBeChanged()
        {
            var result = await "(function () { Node.ELEMENT_NODE = 5; document.ELEMENT_NODE = 6; return [Node.ELEMENT_NODE, document.ELEMENT_NODE].join(); })()".EvalScriptAsync();
            Assert.AreEqual("1,1", result);
        }

        [Test]
        public async Task UndefinedForAnEnumerationArgumentIsZero()
        {
            var result = await "(function () { var r = document.createRange(); return r.compareBoundaryPoints(r.NO_SUCH_CONSTANT, r.cloneRange()); })()".EvalScriptAsync();
            Assert.AreEqual("0", result);
        }

        [Test]
        public async Task EventHandlerOfAFormIsNotANamedProperty()
        {
            var result = await "(function () { var f = document.createElement('form'); var n = 0; f.onsubmit = function () { n++; }; f.appendChild(document.createElement('input')); document.documentElement.appendChild(f); f.dispatchEvent(new Event('submit')); return [n, typeof f.onsubmit, f.length, f[0].tagName].join(); })()".EvalScriptAsync();
            Assert.AreEqual("1,function,1,INPUT", result);
        }

        [Test]
        public async Task FrameCallsAFunctionOfItsParent()
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(@"<body><script>
                function notify(value) { document.title = value; }
                var f = document.createElement('iframe');
                document.body.appendChild(f);
                var d = f.contentDocument;
                d.open();
                d.write('<script>parent.notify(typeof parent.notify + \' from frame\')<\/script>');
                d.close();
            </script></body>"));
            await document.WhenStable();
            var frame = (IHtmlInlineFrameElement)document.QuerySelector("iframe");
            await frame.ContentDocument.WhenStable();

            Assert.AreEqual("function from frame", document.Title);
        }
    }
}
