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
        public void IDocumentShouldNotHaveAnyDuplicateTypes()
        {
            var name = "Document";
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { name, typeof(IDocument) }
            }).First();
            Assert.AreEqual(name, binding.Name);
            Assert.IsTrue(binding.IsInterfaced);
            Assert.AreEqual(0, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
        }

        [Test]
        public void MutationObserverShouldHaveConstructor()
        {
            var name = "MutationObserver";
            var members = new[] { "observe", "disconnect", "takeRecords" };
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { name, typeof(MutationObserver) }
            }).First();
            Assert.AreEqual(name, binding.Name);
            Assert.IsTrue(binding.IsInterfaced);
            Assert.AreEqual(1, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
            Assert.AreEqual(members.Length, binding.Members.Count());
            
            for (int i = 0; i < members.Length; i++)
			{
                Assert.IsTrue(binding.Members.Any(m => m.Key == members[i]));
			}
        }

        [Test]
        public void IParentNodeShouldSetInterfaceObjectCorrectly()
        {
            var name = "ParentNode";
            var members = new[] 
            { 
                "childElementCount", "children", "firstElementChild", "lastElementChild", 
                "append", "prepend", "querySelector", "querySelectorAll" 
            };
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { name, typeof(IParentNode) }
            }).First();
            Assert.AreEqual(name, binding.Name);
            Assert.IsFalse(binding.IsInterfaced);
            Assert.AreEqual(8, binding.Members.Count());
            Assert.AreEqual(0, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
            Assert.AreEqual(0, binding.Getters.Count());
            Assert.AreEqual(0, binding.Setters.Count());
            Assert.AreEqual(members.Length, binding.Members.Count());

            for (int i = 0; i < members.Length; i++)
            {
                Assert.IsTrue(binding.Members.Any(m => m.Key == members[i]));
            }
        }
    }
}
