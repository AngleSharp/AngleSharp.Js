namespace AngleSharp.Js.Tests
{
    using AngleSharp.Scripting;
    using NUnit.Framework;
    using System;

    /// <summary>
    /// Runs the whole suite against the registered DOM instead of the reflected one when the
    /// environment variable "registered" is "true", so that both paths are held to the same tests.
    /// </summary>
    [SetUpFixture]
    public sealed class RegisteredDomSetup
    {
        public static Boolean IsRegistered =>
            String.Equals(Environment.GetEnvironmentVariable("registered"), "true", StringComparison.Ordinal);

        [OneTimeSetUp]
        public void UseRegistration()
        {
            if (IsRegistered)
            {
                JsScriptingService.DefaultDomRegistration = DomRegistrations.Combine(AngleSharp.Css.CssDom.Register, AngleSharp.Io.IoDom.Register, AngleSharp.Xml.XmlDom.Register, TestDom.Register);
            }
        }

        [OneTimeTearDown]
        public void Reset() => JsScriptingService.DefaultDomRegistration = null;
    }
}
