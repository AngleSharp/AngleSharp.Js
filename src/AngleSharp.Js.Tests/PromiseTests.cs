namespace AngleSharp.Js.Tests
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Browser.Dom.Events;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [DomExposed("Window")]
    public static class DeferredWindowExtensions
    {
        [DomName("later")]
        public static async Task<String> Later(this IWindow window, String value)
        {
            await Task.Delay(20).ConfigureAwait(false);
            return value;
        }

        [DomName("laterNode")]
        public static async Task<IElement> LaterNode(this IWindow window, String id)
        {
            await Task.Delay(20).ConfigureAwait(false);
            return window.Document.GetElementById(id);
        }

        [DomName("laterJson")]
        public static async Task<Object> LaterJson(this IWindow window)
        {
            await Task.Delay(20).ConfigureAwait(false);
            return new Dictionary<String, Object>
            {
                ["token"] = "abc",
                ["user"] = null,
                ["tags"] = new Object[] { "a", 2.0, new Dictionary<String, Object> { ["x"] = true } },
            };
        }

        [DomName("pause")]
        public static Task Pause(this IWindow window) => Task.Delay(20);

        [DomName("fail")]
        public static async Task<String> Fail(this IWindow window, String message)
        {
            await Task.Delay(20).ConfigureAwait(false);
            throw new InvalidOperationException(message);
        }
    }

    /// <summary>
    /// Registered so that this assembly counts as a library the engine exposes.
    /// </summary>
    sealed class DeferredService
    {
    }

    /// <summary>
    /// DOM members returning a task (e.g. fetch) give a promise to script, which settles with
    /// the result of the task once it is done.
    /// </summary>
    [TestFixture]
    public class PromiseTests
    {
        [TestCase("later('done').then(function (v) { document.title = v; })", "done")]
        [TestCase("later('x').then(function (v) { return v + 'y'; }).then(function (v) { document.title = v; })", "xy")]
        [TestCase("(async function () { document.title = await later('awaited'); })()", "awaited")]
        [TestCase("document.title = later('a') instanceof Promise", "true")]
        [TestCase("laterNode('d').then(function (e) { document.title = e === document.getElementById('d'); })", "true")]
        [TestCase("laterJson().then(function (j) { document.title = [j.token, j.user, Array.isArray(j.tags), j.tags[1] + 1, j.tags[2].x, Object.keys(j).join('|')].join(); })", "abc,,true,3,true,token|user|tags")]
        [TestCase("pause().then(function (v) { document.title = 'paused:' + v; })", "paused:undefined")]
        [TestCase("fail('broken').catch(function (e) { document.title = (e instanceof Error) + ':' + e.message; })", "true:broken")]
        public async Task TaskBecomesAPromise(String script, String expected)
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop().With(new DeferredService());
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content($"<!doctype html><div id=d></div><script>{script}</script>"));

            for (var i = 0; i < 100 && document.Title != expected; i++)
            {
                await Task.Delay(20);
            }

            Assert.AreEqual(expected, document.Title);
        }

        [TestCase("Promise.reject(new Error('lost'))", "lost")]
        [TestCase("fail('broken').then(function () {})", "broken")]
        [TestCase("Promise.resolve().then(function () { throw new Error('thrown'); })", "thrown")]
        [TestCase("var p = Promise.reject(new Error('late')); p.catch(function () {})", null)]
        [TestCase("addEventListener('unhandledrejection', function (e) { document.title = e.reason.message; e.preventDefault(); }); Promise.reject(new Error('seen'))", null)]
        [TestCase("window.onunhandledrejection = function (e) { document.title = e.reason.message; return false; }; Promise.reject(new Error('seen'))", null)]
        public async Task UnhandledRejectionIsReported(String script, String expected)
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop().With(new DeferredService());
            var context = BrowsingContext.New(cfg);
            String reported = null;
            context.AddEventListener("error", (_, ev) =>
            {
                if (ev is TrackEvent { Error: Exception error })
                {
                    reported = error.Message;
                }
            });
            var document = await context.OpenAsync(m => m.Content($"<!doctype html><script>{script}</script>"));

            for (var i = 0; i < 25 && reported == null && document.Title.Length == 0; i++)
            {
                await Task.Delay(20);
            }

            await Task.Delay(100);
            Assert.AreEqual(expected, reported);
            Assert.AreEqual(expected == null && script.Contains("seen") ? "seen" : "", document.Title);
        }
    }
}
