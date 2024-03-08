namespace AngleSharp.Js.Tests
{
    using AngleSharp.Io;
    using AngleSharp.Js.Tests.Mocks;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
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

        [Test]
        public async Task ModuleScriptShouldRun()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/example-module.js", "import { $ } from '/jquery_4_0_0_esm.js'; $('#test').remove();" },
                                    { "/jquery_4_0_0_esm.js", Constants.Jquery4_0_0_ESM }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>Test</div><script type=module src=/example-module.js></script>";
            var document = await context.OpenAsync(r => r.Content(html));
            Assert.IsNull(document.GetElementById("test"));
        }

        [Test]
        public async Task InlineModuleScriptShouldRun()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/jquery_4_0_0_esm.js", Constants.Jquery4_0_0_ESM }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });
            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>Test</div><script type=module>import { $ } from '/jquery_4_0_0_esm.js'; $('#test').remove();</script>";
            var document = await context.OpenAsync(r => r.Content(html));
            Assert.IsNull(document.GetElementById("test"));
        }

        [Test]
        public async Task ModuleScriptWithImportMapShouldRun()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/jquery_4_0_0_esm.js", Constants.Jquery4_0_0_ESM }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>Test</div><script type=importmap>{ \"imports\": { \"jquery\": \"/jquery_4_0_0_esm.js\" } }</script><script type=module>import { $ } from 'jquery'; $('#test').remove();</script>";
            var document = await context.OpenAsync(r => r.Content(html));
            Assert.IsNull(document.GetElementById("test"));
        }

        [Test]
        public async Task ModuleScriptWithScopedImportMapShouldRunCorrectScript()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/example-module-1.js", "export function test() { document.getElementById('test1').remove(); }" },
                                    { "/example-module-2.js", "export function test() { document.getElementById('test2').remove(); }" },
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test1>Test</div><div id=test2>Test</div><script type=importmap>{ \"imports\": { \"example-module\": \"/example-module-1.js\" }, \"scopes\": { \"/test/\": { \"example-module\": \"/example-module-2.js\" } } }</script><script type=module>import { test } from 'example-module'; test();</script>";

            var document1 = await context.OpenAsync(r => r.Content(html));
            Assert.IsNull(document1.GetElementById("test1"));
            Assert.IsNotNull(document1.GetElementById("test2"));

            var document2 = await context.OpenAsync(r => r.Content(html).Address("http://localhost/test/"));
            Assert.IsNull(document2.GetElementById("test2"));
            Assert.IsNotNull(document2.GetElementById("test1"));
        }
    }
}
