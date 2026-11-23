namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    [TestFixture]
    public class Html5TestTests
    {
        [Test]
        public async Task Html5TestShouldProduceScoreWithoutScriptErrors()
        {
            var configuration = Helpers.GetCssConfig()
                .With(new MockHttpClientRequester(Html5TestFixture.GetResponses()))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(configuration);
            var errors = context.CollectErrors();
            var document = await context.OpenAsync("https://html5test.com/").ConfigureAwait(false);
            var score = await document.WaitForSelectorAsync("#score > .pointsPanel > h2 > strong", TimeSpan.FromSeconds(30)).ConfigureAwait(false);
            var failureMessage = await GetFailureMessageAsync(document, errors).ConfigureAwait(false);

            Assert.IsNotNull(score, failureMessage);
            TestContext.WriteLine("HTML5test score: {0}", score.TextContent);
            Assert.Greater(Double.Parse(score.TextContent, CultureInfo.InvariantCulture), 0.0, failureMessage);
            //  AngleSharp.Io before 1.4.0 (the one of these tests) rejects the fetch('') of the page,
            //  instead of fetching the address of the document.
            errors.RemoveAll(error => error.Message.Contains("Value cannot be null. (Parameter 'url')"));
            Assert.IsEmpty(errors, failureMessage);
        }

        private static async Task<String> GetFailureMessageAsync(IDocument document, IEnumerable<Exception> errors)
        {
            String score = null;
            await document.Then(current => score = current.QuerySelector("#score")?.InnerHtml).ConfigureAwait(false);
            var messages = new List<String>();

            foreach (var error in errors)
            {
                messages.Add(error.ToString());
            }

            return String.Concat("#score: ", score, Environment.NewLine, "Errors: ", String.Join(Environment.NewLine, messages));
        }
    }
}
