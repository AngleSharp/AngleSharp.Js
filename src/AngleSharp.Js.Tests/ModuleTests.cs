namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using Jint.Runtime;
    using Jint.Runtime.Modules;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// What a module knows about where it comes from, and what it finds from there.
    /// </summary>
    [TestFixture]
    public class ModuleTests
    {
        private const String Page = "https://example.com/dir/page";
        private const String FilePage = "file:///dir/page.html";

        private static IBrowsingContext CreateContext(Dictionary<String, String> modules)
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new MockHttpClientRequester(modules))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            return BrowsingContext.New(config);
        }

        private static async Task<IDocument> OpenAsync(String html, Dictionary<String, String> modules = null)
        {
            var document = await CreateContext(modules ?? new Dictionary<String, String>()).OpenAsync(r => r.Address(Page).Content(html));
            await document.WhenStable();
            return document;
        }

        [Test]
        public async Task ExternalModuleKnowsItsResolvedAddress()
        {
            var document = await OpenAsync("<!doctype html><script type=module src=../js/app.js></script>", new Dictionary<String, String>
            {
                { "/js/app.js", "document.title = import.meta.url;" },
            });

            Assert.AreEqual("https://example.com/js/app.js", document.Title);
        }

        [Test]
        public async Task InlineModuleKnowsTheAddressOfItsDocument()
        {
            var document = await OpenAsync("<!doctype html><script type=module>document.title = import.meta.url;</script>");

            Assert.AreEqual(Page, document.Title);
        }

        [Test]
        public async Task InlineModuleKnowsTheBaseOfItsDocument()
        {
            var document = await OpenAsync("<!doctype html><base href=https://other.example/base/><script type=module>document.title = import.meta.url;</script>");

            Assert.AreEqual("https://other.example/base/", document.Title);
        }

        [Test]
        public async Task ImportedModuleKnowsItsAddress()
        {
            var document = await OpenAsync("<!doctype html><script type=module src=/js/app.js></script>", new Dictionary<String, String>
            {
                { "/js/app.js", "import './dep.js';" },
                { "/js/dep.js", "document.title = import.meta.url;" },
            });

            Assert.AreEqual("https://example.com/js/dep.js", document.Title);
        }

        [Test]
        public async Task ModuleFromAnImportMapKnowsItsAddress()
        {
            var document = await OpenAsync(@"<!doctype html><script type=importmap>{""imports"":{""dep"":""/js/dep.js""}}</script><script type=module>import 'dep';</script>", new Dictionary<String, String>
            {
                { "/js/dep.js", "document.title = import.meta.url;" },
            });

            Assert.AreEqual("https://example.com/js/dep.js", document.Title);
        }

        [TestCase("./sibling.js", "/js/sub/sibling.js")]
        [TestCase("../parent.js", "/js/parent.js")]
        [TestCase("/root.js", "/root.js")]
        [TestCase("https://example.com/absolute.js", "/absolute.js")]
        public async Task ImportsAreFoundFromTheirModule(String specifier, String path)
        {
            var document = await OpenAsync("<!doctype html><script type=module src=/js/sub/app.js></script>", new Dictionary<String, String>
            {
                { "/js/sub/app.js", $"import '{specifier}';" },
                { path, "document.title = 'found';" },
            });

            Assert.AreEqual("found", document.Title);
        }

        [TestCase("./dep.js", "/dir/dep.js")]
        [TestCase("/dep.js", "/dep.js")]
        public async Task ImportsOfAnInlineModuleAreFoundFromItsDocument(String specifier, String path)
        {
            var document = await OpenAsync($"<!doctype html><script type=module>import '{specifier}';</script>", new Dictionary<String, String>
            {
                { path, "document.title = 'found';" },
            });

            Assert.AreEqual("found", document.Title);
        }

        [Test]
        public async Task ImportsOfAnImportedModuleAreFoundFromThatModule()
        {
            var document = await OpenAsync("<!doctype html><script type=module src=/a/one.js></script>", new Dictionary<String, String>
            {
                { "/a/one.js", "import './b/two.js';" },
                { "/a/b/two.js", "import '../c/three.js';" },
                { "/a/c/three.js", "document.title = import.meta.url;" },
            });

            Assert.AreEqual("https://example.com/a/c/three.js", document.Title);
        }

        [Test]
        public async Task SyntaxErrorOfAnImportedModuleReachesTheWindow()
        {
            var document = await OpenAsync("<!doctype html><script>window.onerror = function () { document.title = 'caught'; };</script><script type=module src=/a.js></script>", new Dictionary<String, String>
            {
                { "/a.js", "import './broken.js';" },
                { "/broken.js", "function (" },
            });

            Assert.AreEqual("caught", document.Title);
        }

        [Test]
        public async Task ImportedJsonModuleIsStillFound()
        {
            var document = await OpenAsync("<!doctype html><script type=module>import data from './data.json' with { type: 'json' }; document.title = data.value;</script>", new Dictionary<String, String>
            {
                { "/dir/data.json", "{ \"value\": \"from json\" }" },
            });

            Assert.AreEqual("from json", document.Title);
        }

        [Test]
        public async Task ModuleRunsOnceHoweverItIsReached()
        {
            var document = await OpenAsync("<!doctype html><title>0</title><script type=module src=/a.js></script><script type=module src=/a.js></script><script type=module src=/b.js></script>", new Dictionary<String, String>
            {
                { "/a.js", "document.title = String(Number(document.title) + 1);" },
                { "/b.js", "import './a.js';" },
            });

            Assert.AreEqual("1", document.Title);
        }

        [Test]
        public async Task SeveralInlineModulesAreDifferentModules()
        {
            var document = await OpenAsync("<!doctype html><title>0</title><script type=module>document.title = String(Number(document.title) + 1);</script><script type=module>document.title = String(Number(document.title) + 1);</script>");

            Assert.AreEqual("2", document.Title);
        }

        [Test]
        public async Task ErrorInAnExternalModuleTellsItsAddress()
        {
            var context = CreateContext(new Dictionary<String, String>
            {
                { "/js/app.js", "throw new Error('from the module');" },
            });
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Address(Page).Content("<!doctype html><script type=module src=/js/app.js></script>"));
            await document.WhenStable();

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("https://example.com/js/app.js", ((JavaScriptException)errors[0]).Location.SourceFile);
        }

        [Test]
        public async Task ErrorInAnInlineModuleTellsItsPlaceInTheDocument()
        {
            var context = CreateContext(new Dictionary<String, String>());
            var errors = context.CollectErrors();
            var document = await context.OpenAsync(r => r.Address(Page).Content("<!doctype html>\n\n<script type=module>\nthrow new Error('from the module');\n</script>"));
            await document.WhenStable();

            Assert.AreEqual(1, errors.Count);
            var location = ((JavaScriptException)errors[0]).Location;
            StringAssert.StartsWith(Page, location.SourceFile);
            Assert.AreEqual(4, location.Start.Line);
        }

        [Test]
        public async Task SyntaxErrorOfAModuleReachesTheWindow()
        {
            var document = await OpenAsync("<!doctype html><script>window.onerror = function (message) { document.title = 'caught'; };</script><script type=module>function (</script>");

            Assert.AreEqual("caught", document.Title);
        }

        [Test]
        public async Task ImportsOfAModuleWithAFileAddressAreFoundFromThatModule()
        {
            var document = await OpenFileAsync("<!doctype html><script type=module src=sub/mod.js></script>", new Dictionary<String, String>
            {
                { "/dir/sub/mod.js", "import './dep.js';" },
                { "/dir/sub/dep.js", "document.title = 'found';" },
            });

            Assert.AreEqual("found", document.Title);
        }

        [Test]
        public async Task ModuleWithAFileAddressKnowsItsAddress()
        {
            var document = await OpenFileAsync("<!doctype html><script type=module src=sub/mod.js></script>", new Dictionary<String, String>
            {
                { "/dir/sub/mod.js", "document.title = import.meta.url;" },
            });

            Assert.AreEqual("file:///dir/sub/mod.js", document.Title);
        }

        [Test]
        public async Task InlineModuleOfAFileDocumentKnowsTheAddressOfItsDocument()
        {
            var document = await OpenFileAsync("<!doctype html><script type=module>document.title = import.meta.url;</script>", new Dictionary<String, String>());

            Assert.AreEqual(FilePage, document.Title);
        }

        [Test]
        public async Task FailedFetchOfAnImportReachesTheWindow()
        {
            var document = await OpenFaultyAsync("<script>window.onerror = function () { document.title = 'caught'; };</script><script type=module>import './a.js'; document.title = 'ran';</script>");

            Assert.AreEqual("caught", document.Title);
        }

        [Test]
        public async Task FailedFetchOfADynamicImportRejects()
        {
            var document = await OpenFaultyAsync("<script>import('./a.js').then(function () { document.title = 'loaded'; }, function () { document.title = 'rejected'; });</script>");

            Assert.AreEqual("rejected", document.Title);
        }

        //  Jint reports any failure of a loader as its own error, so script can't tell this: only
        //  the loader itself shows that the exception of the fetch isn't wrapped in another one.
        [Test]
        public async Task FailedFetchKeepsItsException()
        {
            var document = await OpenFaultyAsync(String.Empty);
            var loader = new ExposedModuleLoader(document);

            Assert.Throws<InvalidOperationException>(() => loader.Load("https://example.com/a.js"));
        }

        private sealed class ExposedModuleLoader : JsModuleLoader
        {
            public ExposedModuleLoader(IDocument document) : base(null, document)
            {
            }

            public String Load(String address)
            {
                var request = new ModuleRequest(address, Array.Empty<ModuleImportAttribute>());
                return LoadModuleContents(null, new ResolvedSpecifier(request, address, null, SpecifierType.RelativeOrAbsolute));
            }
        }

        private static async Task<IDocument> OpenFaultyAsync(String html)
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new FaultyHttpClientRequester())
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var document = await BrowsingContext.New(config).OpenAsync(r => r.Address(Page).Content("<!doctype html>" + html));
            await document.WhenStable();
            return document;
        }

        private static async Task<IDocument> OpenFileAsync(String html, Dictionary<String, String> files)
        {
            var config = Configuration.Default
                .WithJs()
                .WithEventLoop()
                .With(new MockFileRequester(files))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var document = await BrowsingContext.New(config).OpenAsync(r => r.Address(FilePage).Content(html));
            await document.WhenStable();
            return document;
        }
    
        // what a module does once something it awaits is settled comes after the page was loaded
        private static async Task<String> WaitForTitleAsync(IDocument document, String initial = "")
        {
            for (var i = 0; i < 100 && document.Title == initial; i++)
            {
                await Task.Delay(50);
            }

            return document.Title;
        }

        [Test]
        public async Task ImportMapPrefixFindsTheRestOfTheSpecifier()
        {
            var document = await OpenAsync(@"<!doctype html><script type=importmap>{""imports"":{""lib/"":""/js/lib/"",""lib/special/"":""/other/""}}</script><script type=module>import { a } from 'lib/a.js'; import { b } from 'lib/special/b.js'; document.title = a + b;</script>", new Dictionary<String, String>
            {
                { "/js/lib/a.js", "export const a = 'a';" },
                { "/other/b.js", "export const b = 'b';" },
            });

            Assert.AreEqual("ab", document.Title);
        }

        [Test]
        public async Task ModuleWithATopLevelAwaitGoesOnOnceItIsSettled()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var document = await OpenAsync("<!doctype html><script>window.onerror = function (e) { document.title = 'error ' + e; };</script><script type=module>import { value } from './later.js'; document.title = value;</script>", new Dictionary<String, String>
            {
                { "/dir/later.js", "export const value = await new Promise(resolve => setTimeout(() => resolve('settled'), 20));" },
            });

            Assert.AreEqual("settled", await WaitForTitleAsync(document));
            // the timer runs on the event loop, which the module doesn't wait for
            Assert.Less(watch.ElapsedMilliseconds, 5000);
        }

        [Test]
        public async Task ImportOfAModuleNotFoundRejectsWithATypeError()
        {
            var document = await OpenAsync("<!doctype html><script type=module>import('./missing.js').then(() => document.title = 'found', e => document.title = e instanceof TypeError ? 'TypeError' : 'other ' + e);</script>");

            Assert.AreEqual("TypeError", await WaitForTitleAsync(document));
        }

        [Test]
        public async Task CssModuleGivesAStyleSheet()
        {
            var config = Configuration.Default
                .WithCss()
                .WithJs()
                .WithEventLoop()
                .With(new MockHttpClientRequester(new Dictionary<String, String> { { "/dir/style.css", ".a { color: red } .b { color: blue }" } }))
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });
            var document = await BrowsingContext.New(config).OpenAsync(r => r.Address(Page).Content("<!doctype html><script>try { new CSSStyleSheet(); } catch (e) { window.notConstructible = true; }</script><script type=module>import('./style.css', { with: { type: 'css' } }).then(m => document.title = (m.default instanceof CSSStyleSheet) + ' ' + m.default.cssRules.length, e => document.title = window.notConstructible ? 'not constructible' : 'error ' + e);</script>"));
            await document.WhenStable();

            if (await WaitForTitleAsync(document) == "not constructible")
            {
                Assert.Ignore("The style sheets of the referenced AngleSharp.Css can't be constructed.");
            }

            Assert.AreEqual("true 2", await WaitForTitleAsync(document));
        }

        [Test]
        public async Task ImportMetaResolveGivesTheAddressOfASpecifier()
        {
            var document = await OpenAsync(@"<!doctype html><script type=importmap>{""imports"":{""dep"":""/js/dep.js""}}</script><script type=module>document.title = import.meta.resolve('./x.js') + ' ' + import.meta.resolve('dep');</script>");

            Assert.AreEqual("https://example.com/dir/x.js https://example.com/js/dep.js", document.Title);
        }
}
}
