namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Js;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Native.Object;
    using Jint.Runtime;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>
    /// What it means for a DOM prototype to be built from a member layout the whole process
    /// shares: one description of a type, one object per engine over it, and no way for either
    /// end to reach the other.
    /// </summary>
    [TestFixture]
    public class PrototypeShapeTests
    {
        //  Reads a member before handing the prototype back, because a member layout is
        //  installed on the object at the first touch and the diagnostics below deliberately
        //  do not perturb an untouched object into it.
        private const String DivPrototype =
            "(function () { var d = document.createElement('div'); d.tagName; return Object.getPrototypeOf(d); })()";

        private static Engine Open(String content = "<!doctype html><html><body></body></html>")
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = context.OpenAsync(m => m.Content(content)).Result;
            return context.GetService<JsScriptingService>().GetOrCreateJint(document);
        }

        /// <summary>
        /// The description of a type is resolved once for the process: two engines reach the
        /// very same one, which is what makes reflecting over a type tree a cost a second
        /// document does not pay.
        /// </summary>
        [Test]
        public void ShapeIsSharedAcrossEngines()
        {
            var first = Open();
            var second = Open();

            var one = (ObjectInstance)first.Evaluate(DivPrototype);
            var other = (ObjectInstance)second.Evaluate(DivPrototype);

            Assert.AreNotSame(one, other, "Each engine must have a prototype object of its own.");
            Assert.AreSame(DomPrototypeState.Of(one).Shape, DomPrototypeState.Of(other).Shape,
                "Both engines must build their prototype from the same shared description.");
            Assert.AreEqual(ObjectRepresentation.SharedBuiltinLayout, first.Advanced.GetObjectRepresentation(one));
            Assert.AreEqual(ObjectRepresentation.SharedBuiltinLayout, second.Advanced.GetObjectRepresentation(other));
        }

        /// <summary>
        /// Sharing the description does not share the objects: what a script does to one
        /// engine's prototype is invisible to every other engine.
        /// </summary>
        [Test]
        public void PatchingAPrototypeLeavesOtherEnginesAlone()
        {
            var first = Open();
            var second = Open();

            first.Evaluate("HTMLDivElement.prototype.patched = function () { return 'yes'; };");

            Assert.AreEqual("yes", first.Evaluate("document.createElement('div').patched()").AsString());
            Assert.IsTrue(second.Evaluate("typeof document.createElement('div').patched === 'undefined'").AsBoolean(),
                "A patch applied to one engine's prototype must not be visible in another.");
        }

        /// <summary>
        /// A script may do to a DOM prototype what it does to any other object. The shared
        /// layout gives way where it cannot express something - a deleted member drops the
        /// object onto the ordinary property representation - and the members keep working.
        /// </summary>
        [Test]
        public void PrototypePatchingAndDeletingStayCorrect()
        {
            var engine = Open("<!doctype html><html><body><div id='d'>text</div></body></html>");

            engine.Evaluate("HTMLDivElement.prototype.shout = function () { return this.tagName + '!'; };");
            Assert.AreEqual("DIV!", engine.Evaluate("document.getElementById('d').shout()").AsString());

            engine.Evaluate("delete HTMLDivElement.prototype.shout;");
            Assert.IsTrue(engine.Evaluate("typeof document.getElementById('d').shout === 'undefined'").AsBoolean(),
                "A property a script added to a prototype must be deletable again.");

            //  The delete is what the shared layout cannot express, so this is the read that
            //  proves the members survive the object falling back to the ordinary one.
            Assert.AreEqual("DIV", engine.Evaluate("document.getElementById('d').tagName").AsString());
            Assert.IsTrue(engine.Evaluate("document.getElementById('d') instanceof HTMLDivElement").AsBoolean());
            Assert.IsTrue(engine.Evaluate("document.getElementById('d').constructor === HTMLDivElement").AsBoolean());
        }

        /// <summary>
        /// A window member is the one kind a script reaches without naming a receiver, and both
        /// halves of that have to keep working: the bare call, and the fact that the function it
        /// finds is the very one hanging off the window.
        /// </summary>
        [Test]
        public void WindowMembersAnswerWithoutAReceiver()
        {
            var engine = Open();

            Assert.IsTrue(engine.Evaluate("btoa === window.btoa").AsBoolean(),
                "A window method read bare and read off the window must be the same function.");
            Assert.AreEqual(engine.Evaluate("window.btoa('shape')").AsString(), engine.Evaluate("btoa('shape')").AsString());
            Assert.IsTrue(engine.Evaluate("document === window.document").AsBoolean(),
                "A window accessor read bare and read off the window must answer the same object.");
        }

        /// <summary>
        /// Registration used to write the members onto a half-built prototype and skip a name
        /// its chain already carried, which silently dropped any DOM member named after one of
        /// Object.prototype's. The rule is reproduced as it was; this pins both what it covers
        /// and the fact that nothing in the DOM currently runs into it.
        /// </summary>
        /// <remarks>
        /// If AngleSharp ever names a member "toString" - the plausible one, since a browser
        /// really does put it on HTMLAnchorElement - this fails, and the choice between keeping
        /// the quirk and fixing it has to be made rather than made by accident.
        /// </remarks>
        [Test]
        public void NoDomMemberCollidesWithAnObjectPrototypeName()
        {
            var reserved = new HashSet<String>(StringComparer.Ordinal);

            foreach (var key in new Engine().Intrinsics.Object.PrototypeObject.GetOwnPropertyKeys(Types.String))
            {
                reserved.Add(key.ToString());
            }

            CollectionAssert.Contains(reserved, "toString", "The skipped set is Object.prototype's own names.");

            var assemblies = new[]
            {
                typeof(IDocument).GetTypeInfo().Assembly,
                typeof(JsScriptingService).GetTypeInfo().Assembly,
            };

            var collisions = new List<String>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var member in type.GetTypeInfo().DeclaredMembers)
                    {
                        foreach (var name in member.GetCustomAttributes<DomNameAttribute>())
                        {
                            if (reserved.Contains(name.OfficialName))
                            {
                                collisions.Add($"{type.FullName}.{member.Name} is named '{name.OfficialName}'");
                            }
                        }
                    }
                }
            }

            CollectionAssert.IsEmpty(collisions,
                "A DOM member named after an Object.prototype member is silently dropped, as it was before shapes.");
        }

        /// <summary>
        /// The description of a type outlives every engine that used it, so it must hold nothing
        /// of theirs - a delegate closing over an engine would pin every document ever opened.
        /// </summary>
        [Test]
        public void SharedShapesDoNotRetainEngines()
        {
            var references = new List<WeakReference>();

            for (var i = 0; i < 3; i++)
            {
                references.Add(OpenAndDrop());
            }

            for (var attempt = 0; attempt < 5; attempt++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            foreach (var reference in references)
            {
                Assert.IsFalse(reference.IsAlive, "A shape held on to the engine that instantiated it.");
            }
        }

        //  Its own method so that no local of the caller's frame keeps the engine alive.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference OpenAndDrop()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = context.OpenAsync(m => m.Content(
                "<!doctype html><div id='d'></div><script>document.getElementById('d').tagName;</script>")).Result;
            var reference = new WeakReference(context.GetService<JsScriptingService>().GetOrCreateJint(document));
            context.Dispose();
            return reference;
        }

        [Test]
        public async Task PrototypeReportsTheDomNameAsItsTag()
        {
            var result = await "Object.prototype.toString.call(document.createElement('div'))".EvalScriptAsync();
            Assert.AreEqual("[object HTMLDivElement]", result);
        }

        /// <summary>
        /// "constructor" is the one member declared as a slot that produces its value on first
        /// read, so that naming a type does not drag its prototype's members into existence. That
        /// makes it the member whose mere existence has to be answerable before it has a value.
        /// </summary>
        /// <remarks>
        /// The engine answers an existence question off the member layout without materialising
        /// the member, and a slot with nothing in it yet is exactly where that can go wrong in the
        /// direction nothing else would catch: a wrong "missing" is silent, and drops the property
        /// from "in", hasOwnProperty and every enumeration. Each assertion below runs before
        /// anything has read the property, which is what makes it the interesting case.
        /// </remarks>
        [Test]
        public void UnreadConstructorStillExists()
        {
            Assert.IsTrue(Open().Evaluate(
                "'constructor' in Object.getPrototypeOf(document.createElement('div'))").AsBoolean(),
                "\"in\" must find the constructor slot before anything has read it.");

            //  The strict one, and the one that actually catches a wrong "missing": "in" above
            //  walks the chain, and Object.prototype carries a "constructor" of its own to find.
            Assert.IsTrue(Open().Evaluate(
                "Object.getPrototypeOf(document.createElement('div')).hasOwnProperty('constructor')").AsBoolean(),
                "hasOwnProperty must find the constructor slot before anything has read it.");

            Assert.IsFalse(Open().Evaluate(
                "Object.keys(Object.getPrototypeOf(document.createElement('div'))).indexOf('constructor') >= 0").AsBoolean(),
                "The constructor is not enumerable, whether or not it has been read.");

            //  And the value is still the constructor object once something does read it.
            Assert.IsTrue(Open().Evaluate(
                "Object.getPrototypeOf(document.createElement('div')).constructor === HTMLDivElement").AsBoolean());
        }
    }
}
