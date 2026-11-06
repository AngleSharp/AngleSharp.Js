namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Scripting;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using Jint.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The members of the registry that depend on what a script can tell apart.
    /// </summary>
    [DomExposed("CustomElementRegistry")]
    public static class CustomElementRegistryExtensions
    {
        /// <summary>
        /// Gets the constructor of the custom element with the given name, or else undefined.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="name">The name of the element.</param>
        /// <returns>The constructor, if defined.</returns>
        [DomName("get")]
        public static JsValue Get(this CustomElementRegistry registry, String name) =>
            registry.Get(name) as JsValue ?? JsValue.Undefined;
    }

    /// <summary>
    /// Runs the constructors and callbacks of custom elements for the registry of the DOM. What makes
    /// a custom element is what a constructor of the DOM does for it (see EngineInstance): the wrapper
    /// of the existing element is what its constructor gets to see as this.
    /// </summary>
    sealed class CustomElementHost : ICustomElementHost
    {
        private static readonly String[] CallbackNames = { "connectedCallback", "disconnectedCallback", "adoptedCallback", "attributeChangedCallback" };

        private readonly JsScriptingService _service;

        public CustomElementHost(JsScriptingService service)
        {
            _service = service;
        }

        public CustomElementDescription Describe(IDocument document, Object constructor)
        {
            var engine = _service.GetOrCreateInstance(document);
            var type = constructor as JsValue;

            if (type is null || !type.IsConstructor())
            {
                throw new JavaScriptException(engine.Jint.Intrinsics.TypeError, "The constructor of a custom element has to be a constructor.");
            }

            if (!(type.Get("prototype") is ObjectInstance prototype))
            {
                throw new JavaScriptException(engine.Jint.Intrinsics.TypeError, "The prototype of a custom element has to be an object.");
            }

            var callbacks = new Dictionary<String, Object>();

            foreach (var name in CallbackNames)
            {
                var callback = prototype.Get(name);

                if (!callback.IsUndefined())
                {
                    callbacks[name] = callback.AsFunction(engine) ?? throw new JavaScriptException(engine.Jint.Intrinsics.TypeError, $"The {name} of a custom element has to be a function.");
                }
            }

            return new CustomElementDescription(GetObservedAttributes(type, engine, callbacks), callbacks);
        }

        public Boolean Construct(IElement element, CustomElementDefinition definition)
        {
            var engine = _service.GetOrCreateInstance(element.Owner);
            var type = (JsValue)definition.Constructor;
            var wrapper = engine.GetDomNode(element);
            var construction = engine.BeginCustomElementConstruction(element, definition);

            try
            {
                var result = engine.Jint.Construct(type);

                if (!ReferenceEquals(result, wrapper))
                {
                    throw new JavaScriptException(engine.Jint.Intrinsics.TypeError, "The constructor of a custom element did not give the element.");
                }

                return true;
            }
            catch (JavaScriptException error)
            {
                engine.ReportError(error);
                return false;
            }
            catch (Exception error) when (error is not OperationCanceledException)
            {
                //  whatever else it is, this is in the middle of an operation of the DOM
                element.Owner.Context.TrackError(error);
                return false;
            }
            finally
            {
                engine.EndCustomElementConstruction(construction);
            }
        }

        public void Invoke(IDocument document, IElement element, Object callback, params Object[] arguments)
        {
            var engine = _service.GetOrCreateInstance(document);
            engine.EnterScript();

            try
            {
                ((Function)callback).Call(engine.GetDomNode(element), arguments.Select(m => m.ToJsValue(engine)).ToArray());
            }
            catch (JavaScriptException error)
            {
                engine.ReportError(error);
            }
            catch (Exception error) when (error is not OperationCanceledException)
            {
                document.Context.TrackError(error);
            }
            finally
            {
                engine.LeaveScript();
            }
        }

        private static IEnumerable<String> GetObservedAttributes(JsValue type, EngineInstance engine, Dictionary<String, Object> callbacks)
        {
            if (callbacks.ContainsKey("attributeChangedCallback"))
            {
                var observed = type.Get("observedAttributes");

                if (!observed.IsUndefined() && !observed.IsNull())
                {
                    return (String[])observed.As(typeof(String[]), engine);
                }
            }

            return Array.Empty<String>();
        }
    }
}
