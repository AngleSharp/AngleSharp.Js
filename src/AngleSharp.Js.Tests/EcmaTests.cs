namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class EcmaTests
    {
        private static String SetResult(String eval) =>
            $"document.querySelector('#result').textContent = {eval};";

        [Test]
        public async Task BootstrapVersionFive()
        {
            var result = await (new[] { Constants.Bootstrap_5_3_3, SetResult("bootstrap.toString()") }).EvalScriptsAsync()
                .ConfigureAwait(false);
            Assert.AreNotEqual("", result);
        }
    }
}
