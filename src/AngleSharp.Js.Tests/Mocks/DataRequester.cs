namespace AngleSharp.Js.Tests.Mocks
{
    using AngleSharp.Io;
    using AngleSharp.Text;
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    sealed class DataRequester : BaseRequester
    {
        private static readonly String Base64Section = ";base64";
        
        public override Boolean SupportsProtocol(String protocol)
        {
            return protocol.Is(ProtocolNames.Data);
        }
        
        protected override Task<IResponse> PerformRequestAsync(Request request, CancellationToken cancel)
        {
            var content = new MemoryStream();
            var data = request.Address.Data;

            if (data.StartsWith(","))
            {
                data = MimeTypeNames.Plain + data;
            }

            var parts = data.SplitCommas();
            var response = new DefaultResponse
            {
                Address = request.Address,
                Content = content,
                StatusCode = HttpStatusCode.BadRequest
            };

            if (parts.Length == 2)
            {
                var index = parts[0].IndexOf(Base64Section);
                var base64 = index >= 0;
                var mimeType = base64 ? parts[0].Remove(index, Base64Section.Length) : parts[0];

                try
                {
                    var raw = base64 ? Convert.FromBase64String(parts[1]) : parts[1].UrlDecode();
                    content.Write(raw, 0, raw.Length);
                    content.Position = 0;
                    response.Headers.Add(HeaderNames.ContentType, mimeType);
                    response.StatusCode = HttpStatusCode.OK;
                }
                catch (FormatException)
                {
                }
            }

            return Task.FromResult<IResponse>(response);
        }
    }
}
