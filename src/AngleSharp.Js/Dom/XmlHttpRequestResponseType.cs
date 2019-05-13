namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;

    /// <summary>
    /// The various response type options.
    /// </summary>
    [DomLiterals]
    [DomName("XMLHttpRequestResponseType")]
    public enum XmlHttpRequestResponseType
    {
        /// <summary>
        /// No response given.
        /// </summary>
        [DomName("")]
        None,
        /// <summary>
        /// A plain array buffer.
        /// </summary>
        [DomName("arraybuffer")]
        ArrayBuffer,
        /// <summary>
        /// Some binary large object.
        /// </summary>
        [DomName("blob")]
        Blob,
        /// <summary>
        /// An (XML) document.
        /// </summary>
        [DomName("document")]
        Document,
        /// <summary>
        /// A JSON object.
        /// </summary>
        [DomName("json")]
        Json,
        /// <summary>
        /// Plain text.
        /// </summary>
        [DomName("text")]
        Text
    }
}
