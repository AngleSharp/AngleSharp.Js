namespace AngleSharp.Scripting.JavaScript.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Dom.Events;
    using AngleSharp.Scripting.JavaScript.Generator;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class GeneratorTests
    {
        static List<Type> ListOf(params Type[] types)
        {
            return new List<Type>(types);
        }

        [Test]
        public void IDocumentShouldNotHaveAnyDuplicateTypes()
        {
            var name = "Document";
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(IDocument)) }
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
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(MutationObserver)) }
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
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(IParentNode)) }
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
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(IElement)) }
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
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(FilterSettings)) }
            }).First();
            var bindingFields = binding.GetAll<BindingField>();
            Assert.AreEqual(name, binding.Name);
            Assert.AreEqual(fields.Length, bindingFields.Count());
            Assert.AreEqual(fields.Length, binding.GetMembers().Count());

            for (int i = 0; i < fields.Length; i++)
            {
                Assert.IsTrue(bindingFields.Any(m => m.Key == fields[i]));
            }
        }

        [Test]
        public void IDocumentHasGlobalEventHandlers()
        {
            var name = "Document";
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(IDocument)) }
            }).First() as BindingClass;
            var bindingEvents = binding.GetAll<BindingEvent>();
            var click = bindingEvents.Where(m => m.Key == "onclick").FirstOrDefault();
            Assert.AreEqual(0, binding.Constructors.Count());
            Assert.AreEqual(0, binding.Deleters.Count());
            Assert.IsNotNull(click);
            Assert.IsFalse(click.Value.IsLenient);
            Assert.AreEqual("Clicked", click.Value.OriginalName);
            Assert.AreEqual(typeof(DomEventHandler), click.Value.HandlerType);
        }

        [Test]
        public void GeneratingFilesShouldYieldSome()
        {
            var files = Files.Generate();
            Assert.IsTrue(files.Any());
        }

        [Test]
        public void InputEventDataPropertyShouldBeReadOnly()
        {
            var name = "InputEvent";
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(InputEvent)) }
            }).First() as BindingClass;
            var bindingProperties = binding.GetAll<BindingProperty>();
            var data = bindingProperties.Where(m => m.Key == "data").FirstOrDefault();
            Assert.IsFalse(data.Value.AllowSet);
            Assert.IsTrue(data.Value.AllowGet);
            Assert.IsFalse(data.Value.IsLenient);
            Assert.IsNull(data.Value.ForwardedTo);
            Assert.AreEqual("Data", data.Value.OriginalName);
        }

        [Test]
        public void DependencyTreeWithHoles()
        {
            var tree = new DependencyTree<Int32>();
            tree.Include(1, 0);
            tree.Include(2, 1);
            tree.Include(-1, 0);
            tree.Include(20, 10);
            tree.Include(30, 20);
            tree.Include(300, 200);
            tree.Include(200, 100);
            tree.Include(100, 0);

            var controllers = tree.Controllers().ToArray();

            Assert.AreEqual(2, controllers.Length);
            Assert.AreEqual(0, controllers[0]);
            Assert.AreEqual(10, controllers[1]);

            var zeros = tree.Dependencies(0).ToArray();
            var tens = tree.Dependencies(10).ToArray();

            Assert.AreEqual(6, zeros.Length);
            Assert.AreEqual(2, tens.Length);

            CollectionAssert.AreEqual(new[] { 1, 2, -1, 100, 200, 300 }, zeros);
            CollectionAssert.AreEqual(new[] { 20, 30 }, tens);
        }

        [Test]
        public void ParentNodeShouldHaveParamsOnAppend()
        {
            var name = "Element";
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(IElement)) }
            }).First() as BindingClass;
            var bindingMethods = binding.GetAll<BindingMethod>();
            var append = bindingMethods.Where(m => m.Key == "append").FirstOrDefault();
            Assert.AreEqual("Append", append.Value.OriginalName);
            Assert.IsFalse(append.Value.IsLenient);
            Assert.AreEqual(1, append.Value.Parameters.Count());
            Assert.AreEqual(typeof(INode), append.Value.Parameters.First().Type);
            Assert.IsTrue(append.Value.Parameters.First().IsParams);
        }

        [Test]
        public void HtmlElementShouldNotDuplicateParentNodeMethods()
        {
            var name = "HTMLElement";
            var binding = GeneralExtensions.GetBindings(new Dictionary<String, List<Type>>
            {
                { name, ListOf(typeof(IHtmlElement)) }
            }).First() as BindingClass;
            var bindingMethods = binding.GetAll<BindingMethod>();
            var appends = bindingMethods.Count(m => m.Key == "append");
            Assert.AreEqual(0, appends);
        }
    }
}
