namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The user timing of a window: performance.mark, measure and what they leave behind.
    /// </summary>
    [TestFixture]
    public class PerformanceTests
    {
        private static Task<String> Eval(String expression) => $"(function () {{ {expression} }})()".EvalScriptAsync();

        [Test]
        public async Task MarkReturnsAnEntry()
        {
            var result = await Eval(@"
                var m = performance.mark('a');
                return [m.name, m.entryType, typeof m.startTime, m.duration, m.detail, m instanceof PerformanceMark, m instanceof PerformanceEntry].join();");

            Assert.AreEqual("a,mark,number,0,,true,true", result);
        }

        [Test]
        public async Task MarkTakesItsStartTimeAndDetail()
        {
            var result = await Eval(@"
                var m = performance.mark('a', { startTime: 12.5, detail: { foo: 'bar' } });
                return [m.startTime, m.detail.foo].join();");

            Assert.AreEqual("12.5,bar", result);
        }

        [Test]
        public async Task MarkWithoutANameIsAnError()
        {
            var result = await Eval("try { performance.mark(); return 'no error'; } catch (e) { return e.name; }");

            Assert.AreEqual("TypeError", result);
        }

        [Test]
        public async Task EntriesAreRealArraysSortedByTheirStart()
        {
            var result = await Eval(@"
                performance.mark('late', { startTime: 20 });
                performance.mark('early', { startTime: 10 });
                performance.measure('both', 'early', 'late');
                var entries = performance.getEntries();
                return [Array.isArray(entries), entries.map(function (e) { return e.name; }).join('+')].join();");

            Assert.AreEqual("true,early+both+late", result);
        }

        [Test]
        public async Task EntriesAreFoundByNameAndType()
        {
            var result = await Eval(@"
                performance.mark('a', { startTime: 1 });
                performance.mark('b', { startTime: 2 });
                performance.mark('a', { startTime: 3 });
                performance.measure('a', { start: 1, end: 2 });
                return [
                    performance.getEntriesByName('a').length,
                    performance.getEntriesByName('a', 'mark').length,
                    performance.getEntriesByName('a', 'measure').length,
                    performance.getEntriesByType('mark').length,
                    performance.getEntriesByType('measure').length,
                    performance.getEntriesByType('nothing').length
                ].join();");

            Assert.AreEqual("3,2,1,3,1,0", result);
        }

        [Test]
        public async Task MeasureBetweenMarksUsesTheLatestOfAName()
        {
            var result = await Eval(@"
                performance.mark('start', { startTime: 1 });
                performance.mark('start', { startTime: 10 });
                performance.mark('end', { startTime: 25 });
                var m = performance.measure('m', 'start', 'end');
                return [m.name, m.entryType, m.startTime, m.duration, m instanceof PerformanceMeasure].join();");

            Assert.AreEqual("m,measure,10,15,true", result);
        }

        [Test]
        public async Task MeasureWithoutAnythingGoesFromTheOriginToNow()
        {
            var result = await Eval(@"
                var m = performance.measure('m');
                return [m.startTime, m.duration >= 0, m.duration <= performance.now()].join();");

            Assert.AreEqual("0,true,true", result);
        }

        [Test]
        public async Task MeasureTakesTheStartAndGoesOnUntilNow()
        {
            var result = await Eval(@"
                performance.mark('s', { startTime: 0 });
                var m = performance.measure('m', 's');
                return [m.startTime, m.duration >= 0].join();");

            Assert.AreEqual("0,true", result);
        }

        [TestCase("{ start: 5, end: 8 }", "5,3")]
        [TestCase("{ start: 5, duration: 2 }", "5,2")]
        [TestCase("{ end: 8, duration: 2 }", "6,2")]
        [TestCase("{ start: 'a', end: 'b' }", "2,5")]
        public async Task MeasureTakesItsOptions(String options, String expected)
        {
            var result = await Eval($@"
                performance.mark('a', {{ startTime: 2 }});
                performance.mark('b', {{ startTime: 7 }});
                var m = performance.measure('m', {options});
                return [m.startTime, m.duration].join();");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task MeasureTakesItsDetail()
        {
            var result = await Eval("return performance.measure('m', { start: 1, end: 2, detail: 'info' }).detail;");

            Assert.AreEqual("info", result);
        }

        [Test]
        public async Task MeasureWithAnUnknownMarkIsASyntaxError()
        {
            var result = await Eval("try { performance.measure('m', 'nothing'); return 'no error'; } catch (e) { return e.name; }");

            Assert.AreEqual("SyntaxError", result);
        }

        [Test]
        public async Task MeasureWithStartEndAndDurationIsATypeError()
        {
            var result = await Eval("try { performance.measure('m', { start: 1, end: 2, duration: 3 }); return 'no error'; } catch (e) { return e.name; }");

            Assert.AreEqual("TypeError", result);
        }

        [Test]
        public async Task EntriesAreCleared()
        {
            var result = await Eval(@"
                performance.mark('a');
                performance.mark('b');
                performance.measure('c');
                performance.measure('d');
                performance.clearMarks('a');
                performance.clearMeasures('c');
                var named = performance.getEntries().map(function (e) { return e.name; }).sort().join('+');
                performance.clearMarks();
                performance.clearMeasures();
                return [named, performance.getEntries().length].join();");

            Assert.AreEqual("b+d,0", result);
        }

        private static async Task<String> Observed(String script)
        {
            var config = Configuration.Default.WithJs().WithEventLoop();
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content($"<!doctype html><script>{script}</script>"));

            for (var i = 0; i < 200 && document.Title.Length == 0; i++)
            {
                await Task.Delay(10);
            }

            return document.Title;
        }

        [Test]
        public async Task ObserverGetsNewEntriesOfItsTypesLater()
        {
            var result = await Observed(@"
                var sync = true;
                new PerformanceObserver(function (list, observer) {
                    document.title = [sync, list.getEntries().map(function (e) { return e.name; }).join('+'), list.getEntriesByType('measure').length, list.getEntriesByName('a').length].join();
                }).observe({ entryTypes: ['mark'] });
                performance.mark('a');
                performance.measure('ignored');
                performance.mark('b');
                sync = false;");

            Assert.AreEqual("false,a+b,0,1", result);
        }

        [Test]
        public async Task ObserverWithBufferedGetsWhatIsThereAlready()
        {
            var result = await Observed(@"
                performance.mark('before');
                new PerformanceObserver(function (list) {
                    document.title = list.getEntries().map(function (e) { return e.name; }).join('+');
                }).observe({ type: 'mark', buffered: true });");

            Assert.AreEqual("before", result);
        }

        [Test]
        public async Task ObserverOfAnUnknownTypeIsFine()
        {
            var result = await Observed(@"
                var observer = new PerformanceObserver(function () { document.title = 'wrong'; });
                observer.observe({ type: 'longtask' });
                observer.observe({ entryTypes: ['paint', 'navigation'] });
                performance.mark('a');
                setTimeout(function () { document.title = 'fine'; }, 20);");

            Assert.AreEqual("fine", result);
        }

        [Test]
        public async Task DisconnectedObserverGetsNothing()
        {
            var result = await Observed(@"
                var observer = new PerformanceObserver(function () { document.title = 'wrong'; });
                observer.observe({ entryTypes: ['mark'] });
                performance.mark('a');
                observer.disconnect();
                performance.mark('b');
                setTimeout(function () { document.title = 'fine'; }, 20);");

            Assert.AreEqual("fine", result);
        }

        [Test]
        public async Task TakeRecordsTakesWhatTheCallbackWouldGet()
        {
            var result = await Observed(@"
                var observer = new PerformanceObserver(function () { document.title = 'wrong'; });
                observer.observe({ entryTypes: ['mark'] });
                performance.mark('a');
                var taken = observer.takeRecords().length;
                setTimeout(function () { document.title = String(taken); }, 20);");

            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task EntryConvertsToJson()
        {
            var result = await Eval(@"
                var json = performance.mark('a', { startTime: 3 }).toJSON();
                return [json.name, json.entryType, json.startTime, json.duration].join();");

            Assert.AreEqual("a,mark,3,0", result);
        }
    }
}
