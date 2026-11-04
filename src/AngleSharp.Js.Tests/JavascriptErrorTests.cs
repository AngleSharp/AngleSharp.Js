namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using Jint.Runtime;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
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
        public async Task JavascriptErrorInTimerIsTrackedByBrowsingContext()
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop();
            var context = BrowsingContext.New(config);
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Content("<script>setTimeout(function () { undefinedVariable.invalidMethod(); }, 0);</script>"));

            await Task.Delay(100).ConfigureAwait(false);
            await document.WhenStable().ConfigureAwait(false);

            Assert.AreEqual(1, errors.Count);
            Assert.IsInstanceOf<Exception>(errors[0]);
        }

        private static async Task Eventually(Func<Boolean> condition)
        {
            for (var i = 0; i < 300 && !condition(); i++)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }
        }

        private static async Task<(IDocument Document, List<Exception> Errors)> OpenWithOnError(String script, String address = "https://example.com/page")
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Address(address).Content($@"<!doctype html><button id=b></button><script>
                window.onerror = function (message, source, line, column, error) {{
                    document.title = [typeof message, message, source, line, column, error instanceof TypeError, error && error.message].join('|');
                    {(script.Contains("HANDLED") ? "return true;" : "")}
                }};
                </script><script>{script.Replace("HANDLED", "")}</script>"));
            return (document, errors);
        }

        [Test]
        public async Task OnErrorGetsTheDetailsOfAnErrorInAListener()
        {
            var (document, _) = await OpenWithOnError("document.getElementById('b').addEventListener('click', function () { throw new TypeError('broken'); });");

            ((IHtmlElement)document.GetElementById("b")).DoClick();
            await Eventually(() => document.Title.Length > 0);

            var parts = document.Title.Split('|');
            Assert.AreEqual("string", parts[0]);
            StringAssert.Contains("broken", parts[1]);
            Assert.AreEqual("https://example.com/page", parts[2]);
            Assert.AreEqual("true", parts[5]);
            Assert.AreEqual("broken", parts[6]);
        }

        [Test]
        public async Task OnErrorGetsAnErrorOfATimer()
        {
            var (document, errors) = await OpenWithOnError("setTimeout(function () { throw new TypeError('late'); }, 0);");

            await Eventually(() => document.Title.Length > 0);

            StringAssert.Contains("late", document.Title);
            await Eventually(() => errors.Count > 0);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public async Task OnErrorGetsAnErrorOfAScriptAndItsLoadEventStillFires()
        {
            var (document, errors) = await OpenWithOnError("throw new TypeError('at once');");

            StringAssert.Contains("at once", document.Title);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public async Task OnErrorReturningTrueHandlesTheError()
        {
            var (document, errors) = await OpenWithOnError("HANDLEDthrow new TypeError('handled');");

            StringAssert.Contains("handled", document.Title);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public async Task ErrorListenerPreventingTheDefaultHandlesTheError()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Content(@"<!doctype html><script>
                window.addEventListener('error', function (ev) {
                    ev.preventDefault();
                    document.title = String(ev.error instanceof RangeError) + ev.error.message;
                });
                </script><script>throw new RangeError('quiet');</script>"));

            Assert.AreEqual("truequiet", document.Title);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public async Task EventHandlerPropertyThrowingDoesNotStopOtherListeners()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Content(@"<!doctype html><button id=b></button><script>
                var button = document.getElementById('b');
                button.onclick = function () { throw new Error('from the handler'); };
                button.addEventListener('click', function () { document.title = 'next'; });
                </script>"));

            ((IHtmlElement)document.GetElementById("b")).DoClick();
            await Eventually(() => document.Title == "next");

            Assert.AreEqual("next", document.Title);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public async Task OnErrorThrowingDoesNotCallItselfAgain()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Content(@"<!doctype html><script>
                var calls = 0;
                window.onerror = function () { document.title = String(++calls); throw new Error('from onerror'); };
                </script><script>throw new Error('first');</script>"));

            Assert.AreEqual("1", document.Title);
            Assert.AreEqual(2, errors.Count);
        }

        [Test]
        public async Task OnErrorOfAnElementGetsTheEvent()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var document = await context.OpenAsync(r => r.Content(@"<!doctype html><img id=i><script>
                var image = document.getElementById('i');
                image.onerror = function (ev) { document.title = typeof ev + arguments.length; };
                image.dispatchEvent(new Event('error'));
                </script>"));

            Assert.AreEqual("object1", document.Title);
        }

        [Test]
        public async Task InlineScriptReportsItsPositionInTheDocument()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            await context.OpenAsync(r => r.Address("https://example.com/page").Content("<!doctype html>\n<p>one</p>\n<p>two</p>\n  <script>var a = 1; throw new Error('first line');</script>"));

            var location = ((JavaScriptException)errors[0]).Location;
            Assert.AreEqual(4, location.Start.Line);
            // of the new, as that made the error
            Assert.AreEqual(27, location.Start.Column);
            StringAssert.Contains("https://example.com/page:4:", ((JavaScriptException)errors[0]).JavaScriptStackTrace);
        }

        [Test]
        public async Task InlineScriptOverSeveralLinesReportsItsPositionInTheDocument()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            await context.OpenAsync(r => r.Content("<!doctype html>\n<script>\nvar a = 1;\n  throw new Error('third line');\n</script>"));

            var location = ((JavaScriptException)errors[0]).Location;
            Assert.AreEqual(4, location.Start.Line);
            Assert.AreEqual(8, location.Start.Column);
        }

        [Test]
        public async Task InlineScriptsWithTheSameSourceReportTheirOwnPositions()
        {
            const String script = "<script>throw new Error('again');</script>";
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            await context.OpenAsync(r => r.Content($"<!doctype html>\n{script}\n\n{script}"));

            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual(2, ((JavaScriptException)errors[0]).Location.Start.Line);
            Assert.AreEqual(4, ((JavaScriptException)errors[1]).Location.Start.Line);
        }

        [Test]
        public async Task InlineScriptOfADocumentWithWindowsLineEndsReportsItsPosition()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            await context.OpenAsync(r => r.Content("<!doctype html>\r\n<p>one</p>\r\n<script>\r\nvar a = 1;\r\nthrow new Error('x');\r\n</script>"));

            Assert.AreEqual(5, ((JavaScriptException)errors[0]).Location.Start.Line);
        }

        [Test]
        public async Task ScriptAddedByAScriptReportsPositionsOfItsOwn()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            await context.OpenAsync(r => r.Content("<!doctype html>\n\n\n<body><script>var s = document.createElement('script'); s.textContent = \"throw new Error('dynamic');\"; document.body.appendChild(s);</script>"));

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(1, ((JavaScriptException)errors[0]).Location.Start.Line);
        }

        [Test]
        public async Task ExternalScriptKeepsItsOwnPositions()
        {
            var requester = new MockHttpClientRequester(new Dictionary<String, String>
            {
                { "/app.js", "var a = 1;\nthrow new Error('second line');" },
            });
            var config = Configuration.Default.WithJs().WithEventLoop().With(requester)
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(config);
            var errors = context.CollectErrors();
            await context.OpenAsync(r => r.Address("https://example.com/").Content("<!doctype html>\n\n\n\n<script src=app.js></script>"));

            Assert.AreEqual(2, ((JavaScriptException)errors[0]).Location.Start.Line);
        }

        [Test]
        public async Task JavascriptErrorInListenerReachesErrorListenersOfTheWindow()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var document = await context.OpenAsync(r => r.Content(@"<!doctype html><button id=b></button><script>
                window.addEventListener('error', function (ev) {
                    document.title = ev.type + '|' + ev.message;
                });
                document.getElementById('b').addEventListener('click', function () { throw new TypeError('broken'); });
                </script>"));

            ((IHtmlElement)document.GetElementById("b")).DoClick();
            await Eventually(() => document.Title.Length > 0);

            StringAssert.StartsWith("error|", document.Title);
            StringAssert.Contains("broken", document.Title);
        }

        [Test]
        public async Task JavascriptErrorInListenerTellsItsScript()
        {
            var requester = new MockHttpClientRequester(new Dictionary<String, String>
            {
                { "/app.js", "document.getElementById('b').addEventListener('click', function () { throw new Error('x'); });" },
            });
            var config = Configuration.Default.WithJs().WithEventLoop().With(requester)
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(r => r.Address("https://example.com/").Content(@"<!doctype html><button id=b></button><script src=app.js></script>"));
            await document.WhenStable().ConfigureAwait(false);
            ErrorEvent received = null;
            document.DefaultView.AddEventListener("error", (_, ev) => received = ev as ErrorEvent);

            ((IHtmlElement)document.GetElementById("b")).DoClick();
            await Eventually(() => received != null);

            Assert.IsNotNull(received);
            Assert.AreEqual("https://example.com/app.js", received.FileName);
            Assert.AreEqual(1, received.Line);
        }

        [Test]
        public async Task ErrorListenerThrowingIsTrackedInsteadOfReportedToItself()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Content(@"<!doctype html><button id=b></button><script>
                var calls = 0;
                window.addEventListener('error', function () {
                    document.title = String(++calls);
                    throw new Error('from the error listener');
                });
                document.getElementById('b').addEventListener('click', function () { throw new Error('from the listener'); });
                </script>"));
            var button = (IHtmlElement)document.GetElementById("b");

            button.DoClick();
            await Eventually(() => errors.Count > 0);

            Assert.AreEqual("1", document.Title);
            await Eventually(() => errors.Count == 2);
            Assert.AreEqual(2, errors.Count);
            StringAssert.Contains("from the error listener", errors[0].Message);
            StringAssert.Contains("from the listener", errors[1].Message);

            // the next error is reported again
            button.DoClick();
            await Eventually(() => document.Title == "2");

            Assert.AreEqual("2", document.Title);
        }

        [Test]
        public void ThrownErrorTellsTheAddressGivenWithTheScript()
        {
            var document = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop()).OpenNewAsync().Result;

            try
            {
                document.ExecuteScript("function thrower() { throw new Error('x'); }\nthrower();", sourceUrl: "https://example.com/a.js");
                Assert.Fail("The script should have thrown.");
            }
            catch (JavaScriptException ex)
            {
                Assert.AreEqual("https://example.com/a.js", ex.Location.SourceFile);
                StringAssert.Contains("https://example.com/a.js", ex.JavaScriptStackTrace);
                StringAssert.Contains("thrower", ex.JavaScriptStackTrace);
            }
        }

        [Test]
        public void SameScriptTellsTheAddressItWasRunFrom()
        {
            const String source = "throw new Error('same source');";
            var document = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop()).OpenNewAsync().Result;

            foreach (var url in new[] { "https://example.com/first.js", "https://example.com/second.js" })
            {
                var ex = Assert.Throws<JavaScriptException>(() => document.ExecuteScript(source, sourceUrl: url));
                Assert.AreEqual(url, ex.Location.SourceFile);
            }
        }

        [Test]
        public void ScriptWithSyntaxErrorTellsTheAddress()
        {
            var document = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop()).OpenNewAsync().Result;

            var ex = Assert.Catch(() => document.ExecuteScript("function (", sourceUrl: "https://example.com/broken.js"));

            StringAssert.Contains("https://example.com/broken.js", ex.ToString());
        }

        [Test]
        public async Task ExternalScriptReportsItsResolvedAddress()
        {
            var requester = new MockHttpClientRequester(new Dictionary<String, String>
            {
                { "/js/app.js", "setTimeout(function () { throw new Error('late'); }, 0);" },
            });
            var config = Configuration.Default.WithJs().WithEventLoop().With(requester)
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(config);
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Address("https://example.com/page").Content("<!doctype html><script src=js/app.js></script>"));

            await Task.Delay(100).ConfigureAwait(false);
            await document.WhenStable().ConfigureAwait(false);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("https://example.com/js/app.js", ((JavaScriptException)errors[0]).Location.SourceFile);
        }

        [Test]
        public async Task InlineScriptReportsTheAddressOfItsDocument()
        {
            var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Address("https://example.com/page").Content("<!doctype html><script>setTimeout(function () { throw new Error('late'); }, 0);</script>"));

            await Task.Delay(100).ConfigureAwait(false);
            await document.WhenStable().ConfigureAwait(false);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("https://example.com/page", ((JavaScriptException)errors[0]).Location.SourceFile);
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
