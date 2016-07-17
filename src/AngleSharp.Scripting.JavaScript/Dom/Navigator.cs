namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Dom.Navigator;
    using System;
    using System.Net.NetworkInformation;

    sealed class Navigator : INavigator
    {
        public String Name
        {
            get { return "Netscape"; }
        }

        public String Platform
        {
            get { return String.Empty; }
        }

        public String UserAgent
        {
            get { return "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36 OPR/32.0.1948.19 (Edition beta)"; }
        }

        public String Version
        {
            get { return "1.0.0"; }
        }

        public Boolean IsContentHandlerRegistered(String mimeType, String url)
        {
            return false;
        }

        public Boolean IsProtocolHandlerRegistered(String scheme, String url)
        {
            return false;
        }

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

        public Boolean IsOnline
        {
            get { return NetworkInterface.GetIsNetworkAvailable(); }
        }
    }
}
