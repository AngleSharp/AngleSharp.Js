namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using System;

    /// <summary>
    /// Represents the basis for the XHR.
    /// </summary>
    [DomName("XMLHttpRequestEventTarget")]
    [DomExposed("Window")]
    [DomExposed("DedicatedWorker")]
    [DomExposed("SharedWorker")]
    public class XmlHttpRequestEventTarget : EventTarget
    {
        #region Event Names

        internal static readonly String LoadStartEvent = "loadstart";
        internal static readonly String LoadEndEvent = "loadend";
        internal static readonly String ProgressEvent = "progress";
        internal static readonly String AbortEvent = "abort";
        internal static readonly String ErrorEvent = "error";
        internal static readonly String LoadEvent = "load";
        internal static readonly String TimeoutEvent = "timeout";
        internal static readonly String ReadyStateChangeEvent = "readystatechange";

        #endregion

        #region Event Handlers

        /// <summary>
        /// Adds or removes the handler for the loadstart event.
        /// </summary>
        [DomName("onloadstart")]
        public event DomEventHandler Start
        {
            add { AddEventListener(LoadStartEvent, value, false); }
            remove { RemoveEventListener(LoadStartEvent, value, false); }
        }

        /// <summary>
        /// Adds or removes the handler for the progress event.
        /// </summary>
        [DomName("onprogress")]
        public event DomEventHandler Progress
        {
            add { AddEventListener(ProgressEvent, value, false); }
            remove { RemoveEventListener(ProgressEvent, value, false); }
        }

        /// <summary>
        /// Adds or removes the handler for the abort event.
        /// </summary>
        [DomName("onabort")]
        public event DomEventHandler Aborted
        {
            add { AddEventListener(AbortEvent, value, false); }
            remove { RemoveEventListener(AbortEvent, value, false); }
        }

        /// <summary>
        /// Adds or removes the handler for the error event.
        /// </summary>
        [DomName("onerror")]
        public event DomEventHandler Error
        {
            add { AddEventListener(ErrorEvent, value, false); }
            remove { RemoveEventListener(ErrorEvent, value, false); }
        }

        /// <summary>
        /// Adds or removes the handler for the load event.
        /// </summary>
        [DomName("onload")]
        public event DomEventHandler Loaded
        {
            add { AddEventListener(LoadEvent, value, false); }
            remove { RemoveEventListener(LoadEvent, value, false); }
        }

        /// <summary>
        /// Adds or removes the handler for the timeout event.
        /// </summary>
        [DomName("ontimeout")]
        public event DomEventHandler Timedout
        {
            add { AddEventListener(TimeoutEvent, value, false); }
            remove { RemoveEventListener(TimeoutEvent, value, false); }
        }

        /// <summary>
        /// Adds or removes the handler for the loadend event.
        /// </summary>
        [DomName("onloadend")]
        public event DomEventHandler End
        {
            add { AddEventListener(LoadEndEvent, value, false); }
            remove { RemoveEventListener(LoadEndEvent, value, false); }
        }

        #endregion
    }
}
