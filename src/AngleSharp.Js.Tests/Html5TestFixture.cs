namespace AngleSharp.Js.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    internal static class Html5TestFixture
    {
        private const String ResourcePrefix = "AngleSharp.Js.Tests.Fixtures.Html5Test.";

        public static Dictionary<String, String> GetResponses()
        {
            return new Dictionary<String, String>
            {
                { "/", GetContent("index.html") },
                { "/index.html", GetContent("index.html") },
                { "/scripts/base.js", GetContent("scripts.base.js") },
                { "/scripts/8/engine.js", GetContent("scripts._8.engine.js") },
                { "/scripts/8/data.js", GetContent("scripts._8.data.js") },
                { "/rel/detect.js", GetContent("whichbrowser-stub.js") },
                { "/assets/detect.html", GetContent("assets.detect.html") },
                { "/assets/csp.html", GetContent("assets.csp.html") }
            };
        }

        private static String GetContent(String name)
        {
            var assembly = typeof(Html5TestFixture).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream(ResourcePrefix + name))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
