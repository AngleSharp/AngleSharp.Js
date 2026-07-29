namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Js.Proxies;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Native.Object;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Pins the property access semantics Jint derives for the proxy types.
    /// </summary>
    /// <remarks>
    /// Every proxy here is ordinary, and the engine works that out by itself: a type gets the
    /// short read path because it does not override Get, and loses it because it does. Nothing
    /// declares that anywhere, so adding a Get override - the obvious way to intercept a read -
    /// would silently move every read against that type onto the long path, with no test failing
    /// and no behaviour changing. These assertions are the alarm for that.
    /// </remarks>
    [TestFixture]
    public class PropertyAccessSemanticsTests
    {
        private static Task AssertOrdinaryAsync(String expression, Type expected) =>
            AssertOrdinaryAsync(expression, (engine, instance) =>
                //  Assert what it is as well as how it reads: if the expression stops producing
                //  the proxy under test the semantics assertion would still pass, and pass for
                //  the wrong object.
                Assert.AreEqual(expected, instance.GetType(), expression + " produced an unexpected proxy type."));

        private static async Task AssertOrdinaryAsync(String expression, Action<Engine, ObjectInstance> identify)
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs());
            var document = await context.OpenAsync(m => m.Content(
                "<!doctype html><html><body><span id='s'>x</span></body></html>")).ConfigureAwait(false);
            var engine = context.GetService<JsScriptingService>().GetOrCreateJint(document);
            var value = engine.Evaluate(expression);

            Assert.IsInstanceOf<ObjectInstance>(value, expression + " did not evaluate to an object.");

            var instance = (ObjectInstance)value;

            identify.Invoke(engine, instance);

            Assert.AreEqual(PropertyAccessSemantics.Ordinary, engine.Advanced.GetPropertyAccessSemantics(instance),
                instance.GetType().Name + " must keep ordinary read semantics; overriding Get forfeits them.");

            context.Dispose();
        }

        [Test]
        public Task NodeProxyReadsAsOrdinary() =>
            AssertOrdinaryAsync("document.createElement('div')", typeof(DomNodeInstance));

        [Test]
        public Task CollectionProxyReadsAsOrdinary() =>
            AssertOrdinaryAsync("document.getElementsByTagName('span')", typeof(DomCollectionInstance));

        /// <summary>
        /// The prototype is identified by its representation rather than by its type, because it
        /// no longer has one of ours: it is an object over a shared member layout, and that is
        /// exactly the property carrying the win - a host subclass used as a prototype is refused
        /// as an inline-cache holder by design, an object in the shared layout is not.
        /// </summary>
        /// <remarks>
        /// The member read is what settles the answer: the representation is installed on first
        /// touch, and the diagnostic deliberately does not perturb an untouched object into it.
        /// </remarks>
        [Test]
        public Task PrototypeReadsAsOrdinary() =>
            AssertOrdinaryAsync(
                "(function () { var d = document.createElement('div'); d.tagName; return Object.getPrototypeOf(d); })()",
                (engine, instance) =>
                    Assert.AreEqual(ObjectRepresentation.SharedBuiltinLayout, engine.Advanced.GetObjectRepresentation(instance),
                        "A DOM prototype must be an object over a shared member layout."));

        [Test]
        public Task ConstructorReadsAsOrdinary() =>
            AssertOrdinaryAsync("HTMLDivElement", typeof(DomConstructorInstance));

        [Test]
        public Task ConstructorFunctionReadsAsOrdinary() =>
            AssertOrdinaryAsync("Image", typeof(DomConstructorFunctionInstance));
    }
}
