---
title: "Examples"
section: "AngleSharp.Js"
---
# Example Code

This is a (growing) list of examples for every-day usage of AngleSharp.Js. All examples assume these `using` directives:

```cs
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Js;
```

## Evaluate an expression against HTML

No event loop or loader is needed when you only want to run your own code against a document. The value of the last expression is returned.

```cs
var context = BrowsingContext.New(Configuration.Default.WithJs());
var document = await context.OpenAsync(req => req.Content(@"
    <ul>
      <li>One</li>
      <li>Two</li>
      <li>Three</li>
    </ul>"));

var count = (Double)document.ExecuteScript("document.querySelectorAll('li').length");
var text = (String)document.ExecuteScript(
    "Array.from(document.querySelectorAll('li'), li => li.textContent).join(', ')");
// count == 3, text == "One, Two, Three"
```

Numbers always come back as `Double`, `undefined` as the string `"undefined"`, and DOM objects as the AngleSharp objects (an `IElement`, …). Return a string (e.g. via `JSON.stringify`) when you need a structured result:

```cs
var json = (String)document.ExecuteScript(
    "JSON.stringify([...document.querySelectorAll('li')].map(li => ({ text: li.textContent })))");
```

## Load a page and read the DOM after its scripts ran

```cs
var config = Configuration.Default
    .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
    .WithCss()
    .WithJs()
    .WithEventLoop();

var context = BrowsingContext.New(config);
var document = await context.OpenAsync("https://example.com")
    .WaitUntilAvailable();

Console.WriteLine(document.Title);
Console.WriteLine(document.Body.InnerHtml);
```

`IsResourceLoadingEnabled` is what makes AngleSharp download `<script src="…">`. Without it only inline scripts run.

## Run scripts on a document built from a string

Scripts in the source run while the document is parsed. Use `WaitUntilAvailable` before reading the result, because the event loop runs them on its own thread.

```cs
var config = Configuration.Default.WithJs().WithEventLoop();
var document = await BrowsingContext.New(config).OpenAsync(req => req.Content(@"
    <div id=out></div>
    <script>
      document.getElementById('out').textContent = [1, 2, 3].map(x => x * 2).join();
    </script>"))
    .WaitUntilAvailable();

var output = document.GetElementById("out").TextContent; // "2,4,6"
```

## Interact with the page: click, then inspect

Events dispatched from .NET reach JavaScript listeners, and the other way round.

```cs
var config = Configuration.Default.WithJs().WithEventLoop();
var document = await BrowsingContext.New(config).OpenAsync(req => req.Content(@"
    <button id=btn>Count</button><span id=n>0</span>
    <script>
      let n = 0;
      document.getElementById('btn').addEventListener('click', () => {
        document.getElementById('n').textContent = ++n;
      });
    </script>"))
    .WaitUntilAvailable();

var button = (IHtmlElement)document.GetElementById("btn"); // AngleSharp.Html.Dom
button.DoClick();
button.DoClick();

await document.WhenStable();
var clicks = document.GetElementById("n").TextContent; // "2"
```

Inline handlers such as `<button onclick="…">` work as well, as long as the configuration uses `WithJs()`.

## Queue work after the page's scripts

`Then` runs .NET code or JavaScript on the event loop, after everything queued so far:

```cs
var document = await context.OpenAsync(address)
    .WaitUntilAvailable()
    .Then("window.scrollTo(0, 0); document.title += ' (processed)'")
    .Then(doc => Console.WriteLine(doc.Title));
```

## Wait for something a script does later

`WhenStable` and `WaitUntilAvailable` do not wait for timers or network requests started by a script. Wait for an observable effect instead; with a `TaskCompletionSource` and a host function:

```cs
using AngleSharp.Scripting;

var ready = new TaskCompletionSource<String>();
var scripting = new JsScriptingService();
scripting.External["notifyHost"] = new Action<String>(value => ready.TrySetResult(value));

var config = Configuration.Default
    .With(scripting)
    .WithEventLoop();

var document = await BrowsingContext.New(config).OpenAsync(req => req.Content(@"
    <script>
      setTimeout(() => notifyHost('done after timer'), 50);
    </script>"));

var result = await ready.Task.WaitAsync(TimeSpan.FromSeconds(5)); // .NET 6+
```

Alternatively, poll for a selector until a timeout:

```cs
static async Task<IElement> WaitForSelectorAsync(IDocument document, String selector, TimeSpan timeout)
{
    var end = DateTime.UtcNow + timeout;

    while (DateTime.UtcNow < end)
    {
        await document.WhenStable();
        var element = document.QuerySelector(selector);

        if (element != null)
        {
            return element;
        }

        await Task.Delay(25);
    }

    return null;
}
```

## Expose .NET functions and objects to scripts

Values in `JsScriptingService.External` become globals of every document. Delegates become functions, other objects are exposed with their public members (Jint also accepts camel-cased names, so `calc.Add` and `calc.add` both work).

```cs
using AngleSharp.Scripting;

public sealed class Calculator
{
    public Int32 Add(Int32 a, Int32 b) => a + b;
}

var scripting = new JsScriptingService();
scripting.External["calc"] = new Calculator();
scripting.External["log"] = new Action<String>(message => Console.WriteLine(message));

var config = Configuration.Default
    .With(scripting)
    .WithJs()   // keeps inline event attributes and javascript: URLs working
    .WithEventLoop();

var document = await BrowsingContext.New(config).OpenNewAsync();
var sum = document.ExecuteScript("calc.add(2, 3)"); // 5.0
```

The same can be done per engine in an `EngineCreator`, which also lets you set Jint options:

```cs
using Jint;

var config = Configuration.Default
    .With(new EngineCreator((window, options) =>
    {
        var engine = new Engine(options);
        engine.SetValue("hostVersion", "1.0");
        return engine;
    }))
    .WithJs();
```

## Call a JavaScript function from .NET

For simple cases build the call as source:

```cs
var greeting = (String)document.ExecuteScript("greet('World')");
```

To pass .NET values without string concatenation, read the function with `ExecuteScript` and invoke it on the document's Jint engine:

```cs
using AngleSharp.Scripting;
using Jint.Native;

var scripting = new JsScriptingService();
var config = Configuration.Default.With(scripting).WithJs().WithEventLoop();
var document = await BrowsingContext.New(config).OpenAsync(req => req.Content(
    "<script>function greet(name) { return 'Hello ' + name; }</script>"))
    .WaitUntilAvailable();

var engine = scripting.GetOrCreateJint(document);
var greet = (JsValue)document.ExecuteScript("greet");
var greeting = engine.Invoke(greet, "World").AsString(); // "Hello World"
```

Do not use `engine.Evaluate(…)`, `engine.Execute(…)` or `engine.Invoke("greet", …)` for this: they bypass the window scope the page's scripts run in, so the page's globals are not found ("greet is not defined"). Always go through `ExecuteScript`.

## Limit untrusted or runaway scripts

```cs
using Jint;

var config = Configuration.Default
    .With(new EngineCreator((window, options) => new Engine(options
        .MaxStatements(1_000_000)
        .TimeoutInterval(TimeSpan.FromSeconds(2))
        .LimitMemory(32 * 1024 * 1024))))
    .WithJs(new JsScriptingOptions { MaxCallStackDepth = 2000 })
    .WithEventLoop();
```

`ExecuteScript` throws the corresponding Jint exception (e.g. `StatementsCountOverflowException`, `TimeoutException`, `MemoryLimitExceededException`) or a `JavaScriptException` for "Maximum call stack size exceeded". Page scripts that hit a limit stop and report the error to the browsing context (next example). Note that the limits apply per script run, not to the whole page lifetime.

## Collect script errors

Uncaught errors of page scripts, timers and event listeners do not throw into your code; they are dispatched as `error` events of the browsing context (after the page's own `window.onerror`).

```cs
using AngleSharp.Browser.Dom.Events; // TrackEvent

var errors = new List<Exception>();
var context = BrowsingContext.New(Configuration.Default.WithJs().WithEventLoop());
context.AddEventListener("error", (sender, ev) =>
{
    if (ev is TrackEvent track)
    {
        errors.Add(track.Error); // usually a Jint.Runtime.JavaScriptException
    }
});

var document = await context.OpenAsync(req => req.Content(
    "<script>undefinedFunction()</script>"))
    .WaitUntilAvailable();
// errors[0].Message == "undefinedFunction is not defined"
```

Errors of `ExecuteScript` are thrown directly:

```cs
try
{
    document.ExecuteScript("JSON.parse('{')");
}
catch (Jint.Runtime.JavaScriptException ex)
{
    Console.WriteLine($"{ex.Message} at {ex.Location.Start.Line}:{ex.Location.Start.Column}");
}
```

## Forward `console` output

```cs
public sealed class ConsoleForwarder : IConsoleLogger
{
    public void Log(Object[] values) =>
        System.Console.WriteLine("[js] " + String.Join(" ", values));
}

var config = Configuration.Default
    .WithJs()
    .WithConsoleLogger(_ => new ConsoleForwarder());
```

All `console` methods (`log`, `warn`, `error`, `info`, `debug`, `count`, `time`, …) arrive here. Use one logger per context (the factory receives the `IBrowsingContext`) when you need to tell the pages apart. Note that `System.Console` must be fully qualified inside code that also imports `AngleSharp.Js.Dom`, which has a `Console` class.

## ES modules and import maps

Module scripts are fetched through the configured loader, so give the context a requester for the addresses involved:

```cs
var config = Configuration.Default
    .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
    .WithJs()
    .WithEventLoop();

var document = await BrowsingContext.New(config).OpenAsync(req => req
    .Address("https://example.com/app/")
    .Content(@"
        <script type=importmap>
          { ""imports"": { ""lodash/"": ""https://cdn.example.com/lodash-es/"" } }
        </script>
        <script type=module>
          import chunk from 'lodash/chunk.js';
          document.body.dataset.chunks = JSON.stringify(chunk([1, 2, 3, 4], 2));
        </script>"))
    .WaitUntilAvailable();
```

From .NET a module can also be run directly; relative imports resolve against `sourceUrl`:

```cs
document.ExecuteScript("import { x } from './lib.js'; globalThis.result = x;",
    "module", "https://example.com/app/main.js");
```

Modules support top-level `await`, dynamic `import()`, `import.meta.url`/`import.meta.resolve()`, JSON modules and CSS module scripts (`import sheet from './a.css' with { type: 'css' }`, needs `WithCss()`).

## Workers and message channels

`Worker`, `SharedWorker` and `MessageChannel` are available to scripts. Worker scripts are loaded through the loader, so resource loading must be enabled:

```cs
var config = Configuration.Default
    .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
    .WithJs()
    .WithEventLoop();
```

```js
const worker = new Worker('worker.js');
worker.onmessage = e => document.title = e.data;
worker.postMessage({ numbers: [1, 2, 3] });
```

Messages are copied with the structured clone algorithm;  `MessagePort`s can be transferred.

## Use a different engine per page or per context

Register a factory to get a fresh `EngineCreator` (and thus separate host state) per browsing context:

```cs
var config = Configuration.Default
    .With<EngineCreator>(context => (window, options) => new Engine(options.MaxStatements(10_000)))
    .WithJs()
    .WithEventLoop();
```
