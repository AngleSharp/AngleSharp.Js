namespace AngleSharp.Js.Tests
{
    using AngleSharp.Dom;
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
                                    { "/test.js", "import { test } from 'example-module'; test();" },
                                    { "/test/test.js", "import { test } from 'example-module'; test();" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);

            var html1 = "<!doctype html><div id=test1>Test</div><div id=test2>Test</div><script type=importmap>{ \"imports\": { \"example-module\": \"/example-module-1.js\" }, \"scopes\": { \"/test/\": { \"example-module\": \"/example-module-2.js\" } } }</script><script type=module src=/test.js></script>";
            var document1 = await context.OpenAsync(r => r.Content(html1));
            Assert.IsNull(document1.GetElementById("test1"));
            Assert.IsNotNull(document1.GetElementById("test2"));

            var html2 = "<!doctype html><div id=test1>Test</div><div id=test2>Test</div><script type=importmap>{ \"imports\": { \"example-module\": \"/example-module-1.js\" }, \"scopes\": { \"/test/\": { \"example-module\": \"/example-module-2.js\" } } }</script><script type=module src=/test/test.js></script>";
            var document2 = await context.OpenAsync(r => r.Content(html2));
            Assert.IsNull(document2.GetElementById("test2"));
            Assert.IsNotNull(document2.GetElementById("test1"));
        }

        [Test]
        public async Task ModuleScriptWithQuoteInImportMapShouldRun()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/example-module.js", "export function test() { document.getElementById('test').remove(); }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>Test</div><script type=importmap>{ \"imports\": { \"o'clock\": \"/example-module.js\" } }</script><script type=module>import { test } from \"o'clock\"; test();</script>";
            var document = await context.OpenAsync(r => r.Content(html));
            Assert.IsNull(document.GetElementById("test"));
        }

        [Test]
        public async Task ImportMapContentIsNotEvaluatedAsScript()
        {
            var config = Configuration.Default.WithJs();
            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>Test</div><script type=importmap>{ \"imports\": {} }'); document.getElementById('test').remove(); ('</script>";
            var document = await context.OpenAsync(r => r.Content(html));
            Assert.IsNotNull(document.GetElementById("test"));
        }

        [Test]
        public async Task ModuleScriptWithAbsoluteUrlImportMapShouldRun()
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
            var html = "<!doctype html><div id=test>Test</div><script type=importmap>{ \"imports\": { \"https://example.com/jquery.js\": \"/jquery_4_0_0_esm.js\" } }</script><script type=module>import { $ } from 'https://example.com/jquery.js'; $('#test').remove();</script>";
            var document = await context.OpenAsync(r => r.Content(html));
            Assert.IsNull(document.GetElementById("test"));
        }

        [Test]
        public async Task ModuleScriptCanUseDynamicImportForFeatureSplit()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/app.js", "import('/feature.js').then(m => m.mount());" },
                                    { "/feature.js", "export function mount() { document.getElementById('test').textContent = 'mounted'; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>pending</div><script type=module src=/app.js></script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("mounted", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleGraphCanShareSingletonStateAcrossImports()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/store.js", "let count = 0; export function inc() { count++; } export function read() { return count; }" },
                                    { "/worker.js", "import { inc } from '/store.js'; inc();" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>0</div><script type=module>import { inc, read } from '/store.js'; import '/worker.js'; inc(); document.getElementById('test').textContent = read().toString();</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("2", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleScriptCanUseReExportedBindings()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/core.js", "export const version = '1.2.3';" },
                                    { "/api.js", "export { version } from '/core.js';" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import { version } from '/api.js'; document.getElementById('test').textContent = version;</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("1.2.3", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleScriptCanUseDefaultAndNamedExportsTogether()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/math.js", "export default function sum(a, b) { return a + b; } export const mul = (a, b) => a * b;" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import sum, { mul } from '/math.js'; document.getElementById('test').textContent = sum(2, 3).toString() + ':' + mul(2, 3).toString();</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("5:6", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleEvaluationRunsOnlyOnceForSharedDependency()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/counter.js", "let runs = 0; runs++; export function getRuns() { return runs; }" },
                                    { "/a.js", "import { getRuns } from '/counter.js'; export const a = getRuns();" },
                                    { "/b.js", "import { getRuns } from '/counter.js'; export const b = getRuns();" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import { a } from '/a.js'; import { b } from '/b.js'; document.getElementById('test').textContent = a.toString() + ':' + b.toString();</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("1:1", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleScriptCanRecoverFromDynamicImportFailure()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/app.js", "import('/broken.js').catch(() => import('/fallback.js')).then(m => m.start());" },
                                    { "/broken.js", "throw new Error('broken'); export const x = 1;" },
                                    { "/fallback.js", "export function start() { document.getElementById('test').textContent = 'fallback'; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>booting</div><script type=module src=/app.js></script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("fallback", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleScriptCanPerformRetryLikeWorkflow()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/task.js", "let attempts = 0; export async function runWithRetry() { attempts++; if (attempts < 2) { throw new Error('transient'); } return 'ok:' + attempts.toString(); }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>init</div><script type=module>import { runWithRetry } from '/task.js'; async function main() { try { await runWithRetry(); } catch (e) { const value = await runWithRetry(); document.getElementById('test').textContent = value; } } main();</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("ok:2", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleScriptCanComposeParallelAsyncDependencies()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/a.js", "export async function readA() { return 'A'; }" },
                                    { "/b.js", "export async function readB() { return 'B'; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>x</div><script type=module>import { readA } from '/a.js'; import { readB } from '/b.js'; Promise.all([readA(), readB()]).then(values => { document.getElementById('test').textContent = values.join(''); });</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("AB", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ModuleCycleMaintainsLiveBindings()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/a.js", "import { bValue } from '/b.js'; export let aValue = 'a0'; export function hydrateA() { aValue = 'a1:' + bValue; }" },
                                    { "/b.js", "import { aValue } from '/a.js'; export let bValue = 'b0'; export function hydrateB() { bValue = 'b1:' + aValue; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import { aValue, hydrateA } from '/a.js'; import { bValue, hydrateB } from '/b.js'; hydrateB(); hydrateA(); document.getElementById('test').textContent = aValue + '|' + bValue;</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("a1:b1:a0|b1:a0", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task DynamicImportUsesModuleCacheAcrossCalls()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/counter.js", "let count = 0; count++; export function value() { return count; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>Promise.all([import('/counter.js'), import('/counter.js')]).then(all => { const first = all[0].value(); const second = all[1].value(); document.getElementById('test').textContent = first.toString() + ':' + second.toString(); });</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("1:1", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ImportMapCanProvideVersionedAliasing()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/lib-v2.js", "export const version = '2.0.0';" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=importmap>{ \"imports\": { \"app-lib\": \"/lib-v2.js\" } }</script><script type=module>import { version } from 'app-lib'; document.getElementById('test').textContent = version;</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("2.0.0", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task ApplicationBootstrapCanExposeUnhandledErrorsToUi()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/boot.js", "export async function start() { throw new Error('fatal'); }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>idle</div><script type=module>import { start } from '/boot.js'; start().catch(e => { document.getElementById('test').textContent = e.message; });</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("fatal", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task AppFlowCanIgnoreStaleRequestResults()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/api.js", "const resolvers = []; export function request(value) { return new Promise(resolve => resolvers.push(() => resolve(value))); } export function flush(index) { resolvers[index](); }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test>none</div><script type=module>import { request, flush } from '/api.js'; let latest = 0; function load(value) { const token = ++latest; request(value).then(v => { if (token !== latest) { return; } document.getElementById('test').textContent = v; }); } load('old'); load('new'); flush(1); flush(0);</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("new", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task AppBootstrapGuardCanEnsureIdempotentStartup()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/startup.js", "let started = false; let calls = 0; export function start() { if (started) { return; } started = true; calls++; } export function getCalls() { return calls; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import { start, getCalls } from '/startup.js'; start(); start(); start(); document.getElementById('test').textContent = getCalls().toString();</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("1", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task AppFlowCanShareInFlightInitializationAcrossConsumers()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/init.js", "let initPromise; let runs = 0; export function ensureReady() { if (!initPromise) { runs++; initPromise = Promise.resolve('ready'); } return initPromise; } export function getRuns() { return runs; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import { ensureReady, getRuns } from '/init.js'; Promise.all([ensureReady(), ensureReady(), ensureReady()]).then(values => { document.getElementById('test').textContent = values[0] + ':' + getRuns().toString(); });</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("ready:1", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task AppFlowCanPropagateCancellationTokenState()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/cancel.js", "export function createToken() { return { cancelled: false }; } export function cancel(token) { token.cancelled = true; } export function run(token) { return Promise.resolve().then(() => { if (token.cancelled) { throw new Error('cancelled'); } return 'done'; }); }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import { createToken, cancel, run } from '/cancel.js'; const token = createToken(); const pending = run(token).then(v => { document.getElementById('test').textContent = v; }).catch(e => { document.getElementById('test').textContent = e.message; }); cancel(token); pending;</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("cancelled", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task AppFlowCanDeDuplicateConcurrentRequestsByKey()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .With(new MockHttpClientRequester(new Dictionary<string, string>()
                                {
                                    { "/users.js", "const inflight = new Map(); let calls = 0; export function fetchUser(id) { if (!inflight.has(id)) { calls++; inflight.set(id, Promise.resolve('user-' + id).finally(() => inflight.delete(id))); } return inflight.get(id); } export function getCalls() { return calls; }" }
                                }))
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>import { fetchUser, getCalls } from '/users.js'; Promise.all([fetchUser('42'), fetchUser('42')]).then(values => { document.getElementById('test').textContent = values[0] + ':' + values[1] + ':' + getCalls().toString(); });</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("user-42:user-42:1", document.GetElementById("test")?.TextContent);
        }

        [Test]
        public async Task MicrotasksRunAfterSynchronousWorkInOrder()
        {
            var config =
                Configuration.Default
                                .WithJs()
                                .WithDefaultLoader(new LoaderOptions() { IsResourceLoadingEnabled = true });

            var context = BrowsingContext.New(config);
            var html = "<!doctype html><div id=test></div><script type=module>const order = []; Promise.resolve().then(() => order.push('m1')); order.push('sync'); Promise.resolve().then(() => { order.push('m2'); document.getElementById('test').textContent = order.join(','); });</script>";
            var document = await context.OpenAsync(r => r.Content(html)).WhenStable().ConfigureAwait(false);
            Assert.AreEqual("sync,m1,m2", document.GetElementById("test")?.TextContent);
        }
    }
}
