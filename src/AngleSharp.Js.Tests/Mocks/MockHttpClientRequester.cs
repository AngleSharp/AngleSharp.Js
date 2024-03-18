namespace AngleSharp.Js.Tests.Mocks
{
    using AngleSharp.Io;
    using AngleSharp.Io.Network;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Mock HttpClientRequester which returns content for a specific request from a local dictionary.
    /// </summary>
    internal class MockHttpClientRequester : HttpClientRequester
    {
        private readonly Dictionary<string, string> _mockResponses;

        public MockHttpClientRequester(Dictionary<string, string> mockResponses) : base()
        {
            _mockResponses = mockResponses;
        }

        protected override async Task<IResponse> PerformRequestAsync(Request request, CancellationToken cancel)
        {
            var response = new DefaultResponse();

            if (_mockResponses.TryGetValue(request.Address.PathName, out var responseContent))
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(responseContent));
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
            }

            return response;
        }
    }
}
