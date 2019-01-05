namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Attributes;

    /// <summary>
    /// Defines the states of the requester.
    /// </summary>
    [DomName("XmlHttpRequest")]
    public enum RequesterState : ushort
    {
        /// <summary>
        /// Nothing has been sent yet.
        /// </summary>
        [DomName("UNSENT")]
        Unsent = 0,
        /// <summary>
        /// The communication channel is open.
        /// </summary>
        [DomName("OPENED")]
        Opened = 1,
        /// <summary>
        /// The response headers have been received.
        /// </summary>
        [DomName("HEADERS_RECEIVED")]
        HeadersReceived = 2,
        /// <summary>
        /// The body is still incoming.
        /// </summary>
        [DomName("LOADING")]
        Loading = 3,
        /// <summary>
        /// The response has been received.
        /// </summary>
        [DomName("DONE")]
        Done = 4
    }
}
