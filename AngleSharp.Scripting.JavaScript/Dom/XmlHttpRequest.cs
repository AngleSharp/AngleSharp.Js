namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Network;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

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

        const Int32 BufferSize = 16384;

        readonly Dictionary<String, String> _headers;
        readonly IWindow _window;

        RequesterState _readyState;
        Int32 _timeout;
        Boolean _credentials;
        HttpMethod _method;
        Url _url;
        Boolean _async;
        String _mime;
        HttpStatusCode _responseStatus;
        String _responseUrl;
        String _responseText;
        IDownload _download;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new XHR.
        /// </summary>
        [DomConstructor]
        public XmlHttpRequest(IWindow window)
        {
            _window = window;
            _async = true;
            _method = HttpMethod.Get;
            _headers = new Dictionary<String, String>();
            _url = null;
            _mime = null;
            _responseUrl = String.Empty;
            _responseText = String.Empty;
            _readyState = RequesterState.Unsent;
            _responseStatus = (HttpStatusCode)0;
            _credentials = false;
            _timeout = 45000;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if response headers are accessible.
        /// </summary>
        public Boolean HasResponseHeaders
        {
            get { return _readyState == RequesterState.Loading || _readyState == RequesterState.Done; }
        }

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
            private set
            {
                _readyState = value;
                Fire(ReadyStateChangeEvent);
            }
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
            get { return _responseUrl; }
        }

        /// <summary>
        /// Gets the status code of the response.
        /// </summary>
        [DomName("status")]
        public Int32 StatusCode
        {
            get { return (Int32)_responseStatus; }
        }

        /// <summary>
        /// Gets the status text of the response.
        /// </summary>
        [DomName("statusText")]
        public String StatusText
        {
            get { return StatusCode != 0 ? _responseStatus.ToString() : String.Empty; }
        }

        /// <summary>
        /// Gets the resulting response object.
        /// </summary>
        [DomName("response")]
        public Object Response
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the body text of the response.
        /// </summary>
        [DomName("responseText")]
        public String ResponseText
        {
            get { return _responseText; }
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
            if (_readyState == RequesterState.Loading)
            {
                _download.Cancel();
                Fire(AbortEvent);
            }
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
        public void Open(String method, String url, Boolean async = true, String username = null, String password = null)
        {
            if (_readyState == RequesterState.Unsent)
            {
                ReadyState = RequesterState.Opened;

                if (!Enum.TryParse(method, true, out _method))
                {
                    _method = HttpMethod.Get;
                }

                _url = Url.Create(url);
                _async = async;
                _url.UserName = username;
                _url.Password = password;
            }
        }

        /// <summary>
        /// Sends the created request with the potentially provided object.
        /// </summary>
        /// <param name="body">The body to send (e.g., for forms POST).</param>
        [DomName("send")]
        public void Send(Object body = null)
        {
            if (_readyState == RequesterState.Opened)
            {
                var requestBody = Serialize(body);
                var mimeType = default(String);
                var loader = GetLoader();

                if (loader != null)
                {
                    var request = new DocumentRequest(_url)
                    {
                        Body = requestBody,
                        Method = _method,
                        MimeType = mimeType,
                        Referer = _window.Document.DocumentUri,
                    };

                    foreach (var header in _headers)
                    {
                        request.Headers[header.Key] = header.Value;
                    }

                    _headers.Clear();

                    Fire(LoadStartEvent);
                    ReadyState = RequesterState.HeadersReceived;
                    var connection = Receive(loader, request);

                    if (!_async)
                    {
                        connection.Wait();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the request header.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        [DomName("setRequestHeader")]
        public void SetRequestHeader(String name, String value)
        {
            if (_readyState == RequesterState.Opened)
            {
                _headers[name] = value;
            }
        }

        /// <summary>
        /// Gets the response header.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <returns>The value of the field.</returns>
        [DomName("getResponseHeader")]
        public String GetResponseHeader(String name)
        {
            var value = default(String);

            if (HasResponseHeaders && _headers.TryGetValue(name, out value))
            {
                return value;
            }

            return String.Empty;
        }

        /// <summary>
        /// Gets all response headers.
        /// </summary>
        /// <returns>The name and values.</returns>
        [DomName("getAllResponseHeaders")]
        public String GetAllResponseHeaders()
        {
            if (HasResponseHeaders)
            {
                var headers = _headers;
                var lines = new String[headers.Count];
                var index = 0;

                foreach (var header in headers)
                {
                    lines[index] = String.Concat(header.Key, ": ", header.Value);
                    index++;
                }

                return String.Join(Environment.NewLine, lines);
            }

            return String.Empty;
        }

        /// <summary>
        /// Overrides the returned mime-type to force a specific mode.
        /// </summary>
        /// <param name="mime">The mime-type to use.</param>
        [DomName("overrideMimeType")]
        public void OverrideMimeType(String mime)
        {
            if (_readyState == RequesterState.Opened)
            {
                _mime = mime;
            }
        }

        #endregion

        #region Helpers

        async Task Receive(IDocumentLoader loader, DocumentRequest request)
        {
            var eventName = ErrorEvent;

            try
            {
                _download = loader.DownloadAsync(request);

                using (var response = await _download.Task.ConfigureAwait(false))
                {
                    if (response != null)
                    {
                        eventName = LoadEvent;

                        foreach (var header in response.Headers)
                        {
                            _headers[header.Key] = header.Value;
                        }

                        _responseUrl = response.Address.Href;
                        _responseStatus = response.StatusCode;
                        ReadyState = RequesterState.Loading;

                        using (var ms = new MemoryStream())
                        {
                            await response.Content.CopyToAsync(ms, BufferSize).ConfigureAwait(false);
                            ms.Seek(0, SeekOrigin.Begin);

                            using (var reader = new StreamReader(ms))
                            {
                                _responseText = reader.ReadToEnd();
                            }
                        }

                        Fire(LoadEndEvent);
                    }

                    ReadyState = RequesterState.Done;
                    Fire(eventName);
                }
            }
            catch (TaskCanceledException)
            {
                ReadyState = RequesterState.Done;
                Fire(TimeoutEvent);
            }
        }

        IDocumentLoader GetLoader()
        {
            if (_window != null)
            {
                var document = _window.Document;

                if (document != null)
                {
                    var context = document.Context;

                    if (context != null)
                    {
                        return context.Loader;
                    }
                }
            }

            return null;

        }

        static Stream Serialize(Object body)
        {
            if (body != null)
            {
                //TODO Different Types?
                var content = body.ToString();
                var bytes = Encoding.UTF8.GetBytes(content);
                return new MemoryStream(bytes);
            }

            return Stream.Null;
        }

        void Fire(String eventName)
        {
            var evt = new Event(eventName);
            Dispatch(evt);
        }

        #endregion
    }
}
