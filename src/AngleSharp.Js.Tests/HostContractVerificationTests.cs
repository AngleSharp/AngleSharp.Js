namespace AngleSharp.Js.Tests
{
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Turns Jint's host-contract verifiers on for this suite, and proves they are on.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The verifiers are the checks that catch a host object answering one of the engine's
    /// extension points in a way that contradicts another - the exact hazard this binding lives
    /// with, because its proxies answer four of them: TryGetOwnPropertyValue, ProbeOwnProperty,
    /// HasIndex, and the semantics derived from not overriding Get. Every one of those is trusted
    /// by the engine and never re-checked on the hot path, so a wrong answer is silent: a member
    /// disappears from every enumeration, or a read that should have found an attribute resolves
    /// on the prototype instead. A verified run is the only thing that turns any of that into a
    /// failure.
    /// </para>
    /// <para>
    /// They used to be compiled out of Release, so reaching them meant building Jint from source
    /// in Debug. Since 4.15.3 the shipped package reads an AppContext switch instead, which is
    /// what makes this a thing every CI run can do against the very package the library ships
    /// against.
    /// </para>
    /// <para>
    /// The switch is read once, at the type initialization of the gate behind it, so it has to be
    /// set before the first use of any Jint type - a fixture, a one-time setup or a static
    /// constructor all run far too late. A module initializer is the only hook early enough by
    /// construction: the runtime runs it before any code of this assembly does, and Jint is only
    /// ever reached from this assembly's code.
    /// </para>
    /// </remarks>
    [TestFixture]
    public class HostContractVerificationTests
    {
        internal const String SwitchName = "Jint.EnableHostContractVerification";

        [ModuleInitializer]
        internal static void EnableBeforeAnyJintTypeIsTouched() => AppContext.SetSwitch(SwitchName, true);

        /// <summary>
        /// That the switch was set early enough, proven from behaviour rather than from the flag:
        /// a host whose probe contradicts its own descriptors throws exactly when something is
        /// verifying, and is quietly believed when nothing is.
        /// </summary>
        /// <remarks>
        /// It is deliberately not asserted through the gate itself, which is internal to Jint. If
        /// this fails, the whole suite has been running unverified and the proxies' hooks are
        /// covered by nothing.
        /// </remarks>
        [Test]
        public void TheVerifiersAreRunningForThisSuite()
        {
            var engine = new Engine();
            engine.SetValue("host", new SelfContradictingHost(engine));

            var error = Assert.Throws<InvalidOperationException>(() => engine.Evaluate("Object.keys(host).join(',')"),
                "The host-contract verifiers must be on, or nothing checks this binding's own hooks.");

            StringAssert.Contains("lied", error.Message);
        }

        /// <summary>
        /// Denies through its probe one name its own GetOwnProperty plainly serves. The engine
        /// trusts the probe and never re-asks, so unverified the key simply vanishes from every
        /// enumeration with nothing to see.
        /// </summary>
        private sealed class SelfContradictingHost : ObjectInstance
        {
            public SelfContradictingHost(Engine engine)
                : base(engine)
            {
            }

            public override PropertyDescriptor GetOwnProperty(JsValue property)
            {
                var name = property.ToString();
                return name == "honest" || name == "lied"
                    ? new PropertyDescriptor(name, true, true, true)
                    : PropertyDescriptor.Undefined;
            }

            //  Seen as "protected" from outside the Jint assembly, which is what a host writes.
            protected override OwnPropertyProbe ProbeOwnProperty(JsValue property) =>
                property.ToString() == "lied" ? OwnPropertyProbe.Missing : base.ProbeOwnProperty(property);

            public override List<JsValue> GetOwnPropertyKeys(Types types = Types.String | Types.Symbol) =>
                new List<JsValue> { new JsString("honest"), new JsString("lied") };
        }
    }
}

#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    using System;

    /// <summary>
    /// The attribute the compiler recognizes by name rather than by identity, so declaring it
    /// here is what gives the initializer above a target framework where the BCL carries none.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    sealed class ModuleInitializerAttribute : Attribute
    {
    }
}
#endif
