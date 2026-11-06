namespace AngleSharp.Js.Tests.Mocks
{
    using AngleSharp.Io;
    using AngleSharp.Text;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Serves file addresses from a dictionary, so nothing of the actual disk is involved.
    /// </summary>
    sealed class MockFileRequester : BaseRequester
    {
        private readonly Dictionary<String, String> _files;

        public MockFileRequester(Dictionary<String, String> files)
        {
            _files = files;
        }

        public override Boolean SupportsProtocol(String protocol) => protocol.Is(ProtocolNames.File);

        protected override Task<IResponse> PerformRequestAsync(Request request, CancellationToken cancel)
        {
            var found = _files.TryGetValue(request.Address.PathName, out var content);

            return Task.FromResult<IResponse>(new DefaultResponse
            {
                Address = request.Address,
                StatusCode = found ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Content = new MemoryStream(Encoding.UTF8.GetBytes(content ?? String.Empty)),
            });
        }
    }
}
