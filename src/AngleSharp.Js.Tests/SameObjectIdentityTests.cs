namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Scripting;
    using Jint;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class SameObjectIdentityTests
    {
        [Test]
        public async Task MutationTargetUsesTheCanonicalNodeAndItsConcretePrototype()
        {
            using var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = await context.OpenAsync(response => response.Content("<body id=target>test</body>")).ConfigureAwait(false);
            IMutationRecord record = null;
            var observer = new MutationObserver((records, _) => record = records[0]);
            observer.Connect(document.Body, attributes: true);
            document.Body.SetAttribute("data-value", "changed");
            observer.Disconnect();
            Assert.IsNotNull(record);
            var engine = context.GetService<JsScriptingService>().GetOrCreateJint(document);
            engine.SetValue("observedRecord", record);
            engine.Execute("document.body.marker = 'retained'");
            Assert.AreEqual("target", engine.Evaluate("observedRecord.target.id").ToString());
            Assert.IsTrue(engine.Evaluate("observedRecord.target === document.body").AsBoolean());
            Assert.AreEqual("retained", engine.Evaluate("observedRecord.target.marker").ToString());
        }
    }
}
