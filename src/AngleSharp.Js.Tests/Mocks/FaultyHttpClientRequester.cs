namespace AngleSharp.Js.Tests.Mocks
{
    using AngleSharp.Io;
    using AngleSharp.Io.Network;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class FaultyHttpClientRequester : HttpClientRequester
    {
        protected override Task<IResponse> PerformRequestAsync(Request request, CancellationToken cancel) =>
            Task.FromException<IResponse>(new InvalidOperationException());
    }
}
