namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Js.Cache;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DomNameResolutionTests
    {
        [Test]
        public void TypeOfficialNameWithMultipleDomNamesUsesFirst()
        {
            var name = typeof(MultiNamedType).GetOfficialName(null);
            Assert.AreEqual("PrimaryTypeName", name);
        }

        [Test]
        public void TypeOfficialNamesWithMultipleDomNamesUsesAll()
        {
            var names = typeof(MultiNamedType).GetOfficialNames(null);
            CollectionAssert.AreEqual(new[] { "PrimaryTypeName", "SecondaryTypeName" }, names);
        }

        [Test]
        public void InterfaceOfficialNameWithMultipleDomNamesUsesFirst()
        {
            var name = typeof(MultiNamedImplementation).GetOfficialName(null);
            Assert.AreEqual("PrimaryInterfaceName", name);
        }

        [Test]
        public void EnumLiteralDefinitionWithMultipleDomNamesUsesFirst()
        {
            var definition = typeof(MultiNamedEnum).GetEnumLiteralDefinition();
            Assert.NotNull(definition);
            Assert.AreEqual("PrimaryEnumName", definition.Name);
            Assert.AreEqual("PrimaryMemberName", definition.Members[0].Name);
        }

        [Test]
        public void ConstructorDefinitionWithMultipleDomNamesUsesAll()
        {
            var definition = typeof(MultiNamedConstructorType).GetConstructorDefinition();

            Assert.NotNull(definition);
            CollectionAssert.AreEqual(new[] { "DOMRect", "SVGRect" }, definition.Names);
            Assert.AreEqual("DOMRect", definition.Name);
        }

        [Test]
        public void ConstructorSelectionPublishesAllAliases()
        {
            var selected = EngineExtensions.SelectConstructors(new[] { typeof(MultiNamedConstructorType) });

            Assert.IsTrue(selected.ContainsKey("DOMRect"));
            Assert.IsTrue(selected.ContainsKey("SVGRect"));
            Assert.AreSame(selected["DOMRect"], selected["SVGRect"]);
            Assert.AreEqual(typeof(MultiNamedConstructorType), selected["DOMRect"].Type);
        }

        [Test]
        public void ConstructorSelectionPrefersFirstClassOverInterfacesForSharedName()
        {
            var selected = EngineExtensions.SelectConstructors(new[]
            {
                typeof(IFirstSharedName),
                typeof(FirstSharedNameClass),
                typeof(SecondSharedNameClass),
                typeof(ISecondSharedName),
            });

            Assert.IsTrue(selected.ContainsKey("SharedName"));
            Assert.AreEqual(typeof(FirstSharedNameClass), selected["SharedName"].Type);
        }

        [Test]
        public void ConstructorSelectionUsesFirstInterfaceIfNoClassExists()
        {
            var selected = EngineExtensions.SelectConstructors(new[]
            {
                typeof(IFirstSharedName),
                typeof(ISecondSharedName),
            });

            Assert.IsTrue(selected.ContainsKey("SharedName"));
            Assert.AreEqual(typeof(IFirstSharedName), selected["SharedName"].Type);
        }

        [DomName("PrimaryTypeName")]
        [DomName("SecondaryTypeName")]
        private sealed class MultiNamedType
        {
        }

        [DomName("PrimaryInterfaceName")]
        [DomName("SecondaryInterfaceName")]
        private interface IMultiNamedInterface
        {
        }

        private sealed class MultiNamedImplementation : IMultiNamedInterface
        {
        }

        [DomName("DOMRect")]
        [DomName("SVGRect")]
        private sealed class MultiNamedConstructorType
        {
        }

        [DomName("SharedName")]
        private interface IFirstSharedName
        {
        }

        [DomName("SharedName")]
        private interface ISecondSharedName
        {
        }

        [DomName("SharedName")]
        private sealed class FirstSharedNameClass
        {
        }

        [DomName("SharedName")]
        private sealed class SecondSharedNameClass
        {
        }

        [DomName("PrimaryEnumName")]
        [DomName("SecondaryEnumName")]
        private enum MultiNamedEnum
        {
            [DomName("PrimaryMemberName")]
            [DomName("SecondaryMemberName")]
            Value = 1,
        }
    }
}
