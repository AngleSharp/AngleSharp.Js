namespace AngleSharp.Scripting.JavaScript.Tests.Mocks
{
    using AngleSharp.Network;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    sealed class DelayedRequester : IRequester
    {
        readonly Int32 _delay;
        readonly String _message;
        Boolean _started;
        Boolean _finished;

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

        public async Task<IResponse> RequestAsync(IRequest request, CancellationToken cancel)
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

        class Response : IResponse
        {
            readonly MemoryStream _content;
            readonly Dictionary<String, String> _headers;
            readonly Url _address;

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
