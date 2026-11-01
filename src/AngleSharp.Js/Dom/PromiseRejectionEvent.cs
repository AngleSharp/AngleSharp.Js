namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom.Events;
    using System;

    /// <summary>
    /// The event fired at the window for a promise rejected without a handler
    /// (unhandledrejection), which pages use to report such errors.
    /// </summary>
    [DomName("PromiseRejectionEvent")]
    public sealed class PromiseRejectionEvent : Event
    {
        /// <summary>
        /// Creates a new event for the given promise.
        /// </summary>
        /// <param name="type">The type of the event, e.g. unhandledrejection.</param>
        /// <param name="promise">The rejected promise.</param>
        /// <param name="reason">The value the promise was rejected with.</param>
        public PromiseRejectionEvent(String type, Object promise, Object reason)
            : base(type, false, true)
        {
            Promise = promise;
            Reason = reason;
        }

        /// <summary>
        /// Gets the rejected promise.
        /// </summary>
        [DomName("promise")]
        public Object Promise { get; }

        /// <summary>
        /// Gets the value the promise was rejected with.
        /// </summary>
        [DomName("reason")]
        public Object Reason { get; }
    }
}
