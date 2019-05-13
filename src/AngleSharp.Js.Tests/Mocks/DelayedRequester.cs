namespace AngleSharp.Js.Tests.Mocks
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    sealed class DelayedRequester : EventTarget, IRequester
    {
        private readonly Int32 _delay;
        private readonly String _message;
        private Boolean _started;
        private Boolean _finished;

#pragma warning disable CS0067
        public event DomEventHandler Requesting;
        public event DomEventHandler Requested;
#pragma warning restore CS0067

        public DelayedRequester(Int32 delay, String message)
        {
            _delay = delay;
            _message = message;
        }

        public Boolean IsStarted
        {
            get { return _started; }
        }

        public Boolean IsFinished
        {
            get { return _finished; }
        }

        public async Task<IResponse> RequestAsync(Request request, CancellationToken cancel)
        {
            _started = true;
            await Task.Delay(_delay, cancel).ConfigureAwait(false);
            _finished = true;
            return new Response(_message, request.Address);
        }

        public Boolean SupportsProtocol(String protocol)
        {
            return true;
        }

        private sealed class Response : IResponse
        {
            private readonly MemoryStream _content;
            private readonly Dictionary<String, String> _headers;
            private readonly Url _address;

            public Response(String message, Url address)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                _content = new MemoryStream(buffer);
                _headers = new Dictionary<String, String>();
                _address = address;
            }

            public Url Address
            {
                get { return _address; }
            }

            public Stream Content
            {
                get { return _content; }
            }

            public IDictionary<String, String> Headers
            {
                get { return _headers; }
            }

            public HttpStatusCode StatusCode
            {
                get { return HttpStatusCode.OK; }
            }

            public void Dispose()
            {
                _content.Dispose();
            }
        }
    }
}
