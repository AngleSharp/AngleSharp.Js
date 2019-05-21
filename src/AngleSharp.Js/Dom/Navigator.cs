namespace AngleSharp.Js.Dom
{
    using AngleSharp.Browser.Dom;
    using System;
    using System.Net.NetworkInformation;

    sealed class Navigator : INavigator
    {
        public String Name => "Netscape";

        public String Platform => String.Empty;

        public String UserAgent => "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36 OPR/32.0.1948.19 (Edition beta)";

        public String Version => "1.0.0";

        public Boolean IsContentHandlerRegistered(String mimeType, String url) => false;

        public Boolean IsProtocolHandlerRegistered(String scheme, String url) => false;

        public void RegisterContentHandler(String mimeType, String url, String title)
        {
        }

        public void RegisterProtocolHandler(String scheme, String url, String title)
        {
        }

        public void UnregisterContentHandler(String mimeType, String url)
        {
        }

        public void UnregisterProtocolHandler(String scheme, String url)
        {
        }

        public void WaitForStorageUpdates()
        {
        }

        public Boolean IsOnline => NetworkInterface.GetIsNetworkAvailable();
    }
}
