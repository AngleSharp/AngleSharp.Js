namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class PageTests
    {
        private static Task<IDocument> LoadPage(String url, out List<Exception> errors)
        {
            var configuration = Helpers.GetCssConfig()
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(configuration);
            errors = context.CollectErrors();
            return context.OpenAsync(url);
        }

        [Test]
        [Explicit]
        public async Task RunHtml5Test()
        {
            if (Helpers.IsNetworkAvailable())
            {
                var target = "https://html5test.com";
                var document = await LoadPage(target, out var errors).ConfigureAwait(false);
                var result = await document.WaitForSelectorAsync("#score > .pointsPanel > h2 > strong", TimeSpan.FromSeconds(30)).ConfigureAwait(false);

                if (result == null && HasEvalDestructuringError(errors))
                {
                    Assert.Inconclusive("Blocked by https://github.com/sebastienros/jint/issues/2825.");
                }

                Assert.IsNotNull(result, String.Join(Environment.NewLine, errors));
                var points = result?.TextContent ?? "0";
                Assert.AreNotEqual("0", points);
            }
        }

        private static Boolean HasEvalDestructuringError(IEnumerable<Exception> errors)
        {
            foreach (var error in errors)
            {
                if (error is InvalidCastException && error.Message.Contains("Acornima.Ast.ObjectPattern"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
