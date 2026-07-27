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

    sealed class CaptureRequester : EventTarget, IRequester
    {
        private String _body = String.Empty;
        private Dictionary<String, String> _headers = new Dictionary<String, String>();

#pragma warning disable CS0067
        public event DomEventHandler Requesting;
        public event DomEventHandler Requested;
#pragma warning restore CS0067

        public String Body => _body;

        public IDictionary<String, String> Headers => _headers;

        public async Task<IResponse> RequestAsync(Request request, CancellationToken cancel)
        {
            var body = request.Content;

            if (body != null)
            {
                if (body.CanSeek)
                {
                    body.Seek(0, SeekOrigin.Begin);
                }

                using (var reader = new StreamReader(body, Encoding.UTF8, true, 1024, true))
                {
                    _body = await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
            else
            {
                _body = String.Empty;
            }

            _headers = new Dictionary<String, String>(request.Headers);

            return new DefaultResponse
            {
                Address = request.Address,
                StatusCode = HttpStatusCode.OK,
                Content = new MemoryStream(Encoding.UTF8.GetBytes(String.Empty)),
            };
        }

        public Boolean SupportsProtocol(String protocol) => true;
    }
}
