namespace AngleSharp.Scripting.JavaScript.Dom
{
    using AngleSharp.Dom.Navigator;
    using System;
    using System.Linq;
    using System.Net.NetworkInformation;

    sealed class Navigator : INavigator
    {
        public String Name
        {
            get { return "Netscape"; }
        }

        public String Platform
        {
            get { return Environment.OSVersion.VersionString; }
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
            get
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    var adapters = NetworkInterface.GetAllNetworkInterfaces().Where(m => m.OperationalStatus == OperationalStatus.Up);

                    foreach (var adapter in adapters)
                    {
                        if (adapter.NetworkInterfaceType != NetworkInterfaceType.Tunnel && 
                            adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        {
                            var statistic = adapter.GetIPv4Statistics();

                            if (statistic.BytesReceived > 0 && statistic.BytesSent > 0)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}
