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
                .WithJs();

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
    }
}
