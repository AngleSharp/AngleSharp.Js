namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Network;
    using System;

    /// <summary>
    /// Defines the XHR. For more information see:
    /// https://xhr.spec.whatwg.org/#interface-xmlhttprequest
    /// </summary>
    [DomName("XMLHttpRequest")]
    [DomExposed("Window")]
    [DomExposed("DedicatedWorker")]
    [DomExposed("SharedWorker")]
    public sealed class XmlHttpRequest : XmlHttpRequestEventTarget
    {
        #region Fields

        RequesterState _readyState;
        Int32 _timeout;
        Boolean _credentials;
        IResponse _response;
        HttpMethod _method;
        Url _url;
        Boolean _async;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new XHR.
        /// </summary>
        [DomConstructor]
        public XmlHttpRequest()
        {
            _async = true;
            _method = HttpMethod.Get;
            _url = null;
            _response = null;
            _readyState = RequesterState.Unsent;
            _credentials = false;
            _timeout = 45000;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Adds or removes the handler for the readystatechange event.
        /// </summary>
        [DomName("onreadystatechange")]
        public event DomEventHandler ReadyStateChanged
        {
            add { AddEventListener(ReadyStateChangeEvent, value, false); }
            remove { RemoveEventListener(ReadyStateChangeEvent, value, false); }
        }

        /// <summary>
        /// Gets the current ready state.
        /// </summary>
        [DomName("readyState")]
        public RequesterState ReadyState
        {
            get { return _readyState; }
        }

        /// <summary>
        /// Gets or sets the timeout of the request in milliseconds.
        /// </summary>
        [DomName("timeout")]
        public Int32 Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// Gets the associated upload process, if any.
        /// </summary>
        [DomName("upload")]
        public XmlHttpRequestUpload Upload
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets if credentials should be used for the request.
        /// </summary>
        [DomName("withCredentials")]
        public Boolean WithCredentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        /// <summary>
        /// Gets the determined response type.
        /// </summary>
        [DomName("responseType")]
        public XmlHttpRequestResponseType ResponseType
        {
            get { return XmlHttpRequestResponseType.None; }
        }

        /// <summary>
        /// Gets the url of the response.
        /// </summary>
        [DomName("responseURL")]
        public String ResponseUrl
        {
            get { return _response != null ? _response.Address.Href : String.Empty; }
        }

        /// <summary>
        /// Gets the status code of the response.
        /// </summary>
        [DomName("status")]
        public Int32 StatusCode
        {
            get { return _response != null ? (Int32)_response.StatusCode : 0; }
        }

        /// <summary>
        /// Gets the status text of the response.
        /// </summary>
        [DomName("statusText")]
        public String StatusText
        {
            get { return _response != null ? _response.StatusCode.ToString() : String.Empty; }
        }

        /// <summary>
        /// Gets the resulting response object.
        /// </summary>
        [DomName("response")]
        public Object Response
        {
            get { return _response != null ? null : String.Empty; }
        }

        /// <summary>
        /// Gets the body text of the response.
        /// </summary>
        [DomName("responseText")]
        public String ResponseText
        {
            get { return _response != null ? null : String.Empty; }
        }

        /// <summary>
        /// Gets the XML document of the response, if any.
        /// </summary>
        [DomName("responseXML")]
        public IDocument ResponseXml
        {
            get { return null; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Aborts the request.
        /// </summary>
        [DomName("abort")]
        public void Abort()
        {
            //TODO
        }

        /// <summary>
        /// Opens a new request with the provided method and URL.
        /// </summary>
        /// <param name="method">The method to use.</param>
        /// <param name="url">The URL to send to request to.</param>
        [DomName("open")]
        public void Open(String method, String url)
        {
            if (Enum.TryParse(method, true, out _method) == false)
                _method = HttpMethod.Get;

            _url = Url.Create(url);
        }

        /// <summary>
        /// Opens a new request with the provided method and URL.
        /// </summary>
        /// <param name="method">The method to use.</param>
        /// <param name="url">The URL to send to request to.</param>
        /// <param name="async">Should the request be send async?</param>
        /// <param name="username">Should a username be used?</param>
        /// <param name="password">Should a password be used?</param>
        [DomName("open")]
        public void Open(String method, String url, Boolean async, String username = null, String password = null)
        {
            Open(method, url);

            _async = async;
            _url.UserName = username;
            _url.Password = password;
        }

        /// <summary>
        /// Sends the created request with the potentially provided object.
        /// </summary>
        /// <param name="body">The body to send (e.g., for forms POST).</param>
        [DomName("send")]
        public void Send(Object body = null)
        {
            //TODO
        }

        /// <summary>
        /// Sets the request header.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        [DomName("setRequestHeader")]
        public void SetRequestHeader(String name, String value)
        {
            //TODO
        }

        /// <summary>
        /// Gets the response header.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <returns>The value of the field.</returns>
        [DomName("getResponseHeader")]
        public String GetResponseHeader(String name)
        {
            //TODO
            return String.Empty;
        }

        /// <summary>
        /// Gets all response headers.
        /// </summary>
        /// <returns>The name and values.</returns>
        [DomName("getAllResponseHeaders")]
        public String GetAllResponseHeaders()
        {
            //TODO
            return String.Empty;
        }

        /// <summary>
        /// Overrides the returned mime-type to force a specific mode.
        /// </summary>
        /// <param name="mime">The mime-type to use.</param>
        [DomName("overrideMimeType")]
        public void OverrideMimeType(String mime)
        {
            //TODO
        }

        #endregion
    }
}
