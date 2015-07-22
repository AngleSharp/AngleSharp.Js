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
            }).First() as BindingClass;
            Assert.AreEqual(name, binding.Name);
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
            }).First() as BindingClass;
            Assert.AreEqual(name, binding.Name);
            Assert.AreEqual(1, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
            Assert.AreEqual(members.Length, binding.Members.Count());
            
            for (int i = 0; i < members.Length; i++)
			{
                Assert.IsTrue(binding.Members.Any(m => m.Key == members[i]));
			}
        }

        [Test]
        public void IParentNodeIsNoInterfaceAndShouldNotBeCreated()
        {
            var name = "ParentNode";
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { name, typeof(IParentNode) }
            }).FirstOrDefault();
            Assert.IsNull(binding);
        }

        [Test]
        public void IElementShouldContainMembersFromParentNode()
        {
            var name = "Element";
            var members = new[] 
            { 
                "childElementCount", "children", "firstElementChild", "lastElementChild", 
                "append", "prepend", "querySelector", "querySelectorAll" 
            };
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { name, typeof(IElement) }
            }).First() as BindingClass;
            Assert.AreEqual(name, binding.Name);
            Assert.AreEqual(0, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
            Assert.AreEqual(0, binding.Getters.Count());
            Assert.AreEqual(0, binding.Setters.Count());

            for (int i = 0; i < members.Length; i++)
            {
                Assert.IsTrue(binding.Members.Any(m => m.Key == members[i]));
            }
        }

        [Test]
        public void NodeFilterShouldHaveCorrectNameAndLocations()
        {
            var name = "NodeFilter";
            var fields = new[] 
            { 
                "SHOW_ELEMENT", "SHOW_ATTRIBUTE", "SHOW_TEXT", "SHOW_CDATA_SECTION", 
                "SHOW_ENTITY_REFERENCE", "SHOW_ENTITY", "SHOW_PROCESSING_INSTRUCTION", "SHOW_COMMENT" ,
                "SHOW_DOCUMENT", "SHOW_DOCUMENT_TYPE", "SHOW_DOCUMENT_FRAGMENT", "SHOW_NOTATION", "SHOW_ALL"
            };
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, Type>
            {
                { name, typeof(FilterSettings) }
            }).First() as BindingEnum;
            Assert.AreEqual(name, binding.Name);
            Assert.AreEqual(fields.Length, binding.Fields.Count());

            for (int i = 0; i < fields.Length; i++)
            {
                Assert.IsTrue(binding.Fields.Any(m => m.Key == fields[i]));
            }
        }
    }
}
