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
