namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Scripting.JavaScript.Generator;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void IDocumentShouldHaveCorrectNameAndInterfacedPropertyWithNoConstructorsOrDeleters()
        {
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { "Document", typeof(IDocument) }
            }).First();
            Assert.AreEqual("Document", binding.Name);
            Assert.IsTrue(binding.IsInterfaced);
            Assert.AreEqual(0, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
        }

        [Test]
        public void MutationObserverShouldHaveCorrectNameAndInterfacedPropertyAndTypesWithConstructor()
        {
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { "MutationObserver", typeof(MutationObserver) }
            }).First();
            Assert.AreEqual("MutationObserver", binding.Name);
            Assert.IsTrue(binding.IsInterfaced);
            Assert.AreEqual(3, binding.Members.Count());
            Assert.AreEqual(1, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
            Assert.IsTrue(binding.Members.Any(m => m.Key == "observe"));
            Assert.IsTrue(binding.Members.Any(m => m.Key == "disconnect"));
            Assert.IsTrue(binding.Members.Any(m => m.Key == "takeRecords"));
        }
    }
}
