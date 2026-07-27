namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime.Descriptors;
    using System;
    using System.Collections.Generic;

    sealed class DomNodeInstance : ObjectInstance
    {
        private readonly EngineInstance _instance;
        private readonly Object _value;

        private Dictionary<DomEventInstance, DomEventInstance.Registration> _eventHandlers;

        public DomNodeInstance(EngineInstance engine, Object value)
            : base(engine.Jint)
        {
            _instance = engine;
            _value = value;

            Prototype = engine.GetDomPrototype(value.GetType());
        }

        public Object Value => _value;

        public override object ToObject() => _value;

        /// <summary>
        /// Gets the handler assigned to this node for the given event, if any.
        /// The handler is per node - the <see cref="DomEventInstance"/> itself is
        /// shared by every node using the same prototype.
        /// </summary>
        public DomEventInstance.Registration GetEventHandler(DomEventInstance ev)
        {
            if (_eventHandlers != null && _eventHandlers.TryGetValue(ev, out var registration))
            {
                return registration;
            }

            return null;
        }

        /// <summary>
        /// Assigns the handler for the given event to this node.
        /// </summary>
        public void SetEventHandler(DomEventInstance ev, DomEventInstance.Registration registration)
        {
            _eventHandlers = _eventHandlers ?? new Dictionary<DomEventInstance, DomEventInstance.Registration>();
            _eventHandlers[ev] = registration;
        }

        /// <summary>
        /// Removes and returns the handler assigned to this node for the given event, if any.
        /// </summary>
        public DomEventInstance.Registration RemoveEventHandler(DomEventInstance ev)
        {
            if (_eventHandlers != null && _eventHandlers.TryGetValue(ev, out var registration))
            {
                _eventHandlers.Remove(ev);
                return registration;
            }

            return null;
        }

        public override PropertyDescriptor GetOwnProperty(JsValue property)
        {
            //  An indexer is the only thing that can turn into an own property of the node
            //  itself. The members of the DOM interface live on the prototype, so finding
            //  them is the engine's job - answering them here would make the node claim
            //  every inherited member as its own.
            if (Prototype is DomPrototypeInstance prototype &&
                prototype.TryGetFromIndex(_value, property.ToString(), out var descriptor))
            {
                return descriptor;
            }

            return base.GetOwnProperty(property);
        }

        protected override void SetOwnProperty(JsValue property, PropertyDescriptor desc)
        {
            if (Prototype is DomPrototypeInstance prototype)
            {
                var value = desc.Value;

                if (prototype.TrySetToIndex(_value, property.ToString(), value))
                {
                    return;
                }
            }

            base.SetOwnProperty(property, desc);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(property, desc);
            }
        }

        public new void FastSetProperty(string name, PropertyDescriptor value)
        {
            base.FastSetProperty(name, value);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(name, value);
            }
        }

        public new void FastSetProperty(JsValue property, PropertyDescriptor value)
        {
            base.FastSetProperty(property, value);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(property, value);
            }
        }

        public new void FastSetDataProperty(string name, JsValue value)
        {
            base.FastSetDataProperty(name, value);

            if (_value is IWindow)
            {
                _instance.Jint.Global.FastSetProperty(name, new PropertyDescriptor(value, true, true, true));
            }
        }
    }
}
