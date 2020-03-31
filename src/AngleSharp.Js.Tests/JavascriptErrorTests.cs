namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class JavascriptErrorTests
    {
        [Test]
        public async Task JavascriptErrorInListenerShouldNotThrowJavascriptException()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop();

            var context = BrowsingContext.New(config);

            const string content = @"
                <html>
                    <body>
                        <script type='text/javascript'>
                            document.addEventListener('load', function() {
                                undefinedVariable.invalidMethod();
                            });
                        </script>
                        
                    </body>
                </html>
            ";

            await context.OpenAsync(r => r.Content(content));
        }

        [Test]
        public async Task CanComputeAndHoistLocalVariablesInForLoop()
        {
            var source = @"
(function (cl) {
          for (
            var n = !!document.getElementsByClassName,
              ret = [],
              els = n
                ? document.getElementsByClassName(cl)
                : document.getElementsByTagName(""*""),
              p = n ? false : new RegExp(""(^|\\s)"" + cl + ""(\\s|$)""),
              i = 0;
            i < els.length;
            i++
          )
            if (!p || p.test(els[i].className)) ret.push(els[i]);
          return ret;
})('abc').length";
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop();
            var document = await BrowsingContext.New(config).OpenNewAsync();
            var result = document.ExecuteScript(source);
            Assert.AreEqual(0.0, result);
        }
    }
}
