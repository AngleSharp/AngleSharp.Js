---
title: "Getting Started"
section: "AngleSharp.Js"
---
# Getting Started

AngleSharp.Js runs JavaScript against an AngleSharp document. It integrates the
[Jint](https://github.com/sebastienros/jint) interpreter with AngleSharp, so scripts can
read and change the same DOM that your .NET code uses. This is useful when processing pages
whose behavior depends on script execution, evaluating a snippet in a document context, or
hosting JavaScript APIs alongside your application.

## Install and configure

Install the package:

```ps1
Install-Package AngleSharp.Js
```

Add `WithJs()` to the AngleSharp configuration. Add `WithEventLoop()` when scripts, events,
or resource callbacks must run in the browser-like task queue. Add a loader when the document
needs to fetch external scripts or other resources.

```cs
var configuration = Configuration.Default
    .WithDefaultLoader(new LoaderOptions
    {
        IsResourceLoadingEnabled = true,
    })
    .WithJs()
    .WithEventLoop();

var context = BrowsingContext.New(configuration);
var document = await context.OpenAsync("https://example.com");

await document.WaitUntilAvailable();
```

`WithJs()` registers the JavaScript scripting service, support for inline event attributes
such as `onclick`, a navigation handler for `javascript:` URLs, and a default `navigator`
when the configuration does not already provide one.

### Customize the event loop

`WithEventLoop()` creates a dedicated `JsEventLoop` for each browsing context. To select the
stack size of its worker thread, use the factory overload:

```cs
var configuration = Configuration.Default
    .WithJs()
    .WithEventLoop(_ => new JsEventLoop(32 * 1024 * 1024));
```

You can instead provide an `IEventLoop` implementation when the host application owns
scheduling. Use the factory overload when each browsing context needs an independent loop.

### Configure the execution stack

`JsScriptingOptions.MaxCallStackDepth` defaults to 10,000. It makes deep JavaScript recursion
fail with a JavaScript error rather than exhausting the process stack. Leave it positive unless
you explicitly accept the risk of an uncatchable `StackOverflowException`.

```cs
var configuration = Configuration.Default
    .WithJs(new JsScriptingOptions
    {
        MaxCallStackDepth = 5000,
    });
```

## Execute JavaScript

HTML `<script>` elements execute while AngleSharp processes the document. For a script you
want to run yourself, call `ExecuteScript` on the document. The return value is converted to
the corresponding .NET value where possible.

```cs
var document = await BrowsingContext.New(Configuration.Default.WithJs())
    .OpenAsync(request => request.Content("<p class=message>Hello</p>"));

var message = (String)document.ExecuteScript(
    "document.querySelector('.message').textContent");
```

`ExecuteScript(scriptCode, scriptType, sourceUrl)` accepts an optional MIME type and source
URL. JavaScript MIME types are evaluated as classic scripts. `module` runs an ES module and
`importmap` loads an import map. Other script types return `undefined`.

Each document window gets its own Jint engine and JavaScript global state. Reusing the same
configuration does not share variables between documents.

### Wait for queued work

With an event loop configured, use the document extensions to control when work runs:

| API | Use it when |
| --- | --- |
| `Then(Action<IDocument>)` | Queue .NET work after currently queued script work. |
| `Then(String)` | Queue a JavaScript snippet after currently queued script work. |
| `WhenStable()` | Wait until the work already in the event loop has completed. |
| `WaitUntilAvailable()` | Wait for document completion and then for the event loop to stabilize. |

These methods do not wait for asynchronous I/O that a script starts after the marker has been
queued. For example, await an event dispatched by the script after an `XMLHttpRequest` finishes.

For example, wait for scripts that change the document before reading the result:

```cs
var document = await context.OpenAsync("https://example.com")
    .WaitUntilAvailable();

var title = document.Title;
```

## Connect JavaScript and .NET

### Provide host values

Create `JsScriptingService` yourself when scripts need application-provided values. Add entries
to `External` before a document first executes JavaScript; these values are copied into every
new document engine.

```cs
var scripting = new JsScriptingService();
scripting.External["getGreeting"] = new Func<String>(() => "Hello from .NET");

var configuration = Configuration.Default
    .With(scripting)
    .WithEventLoop();

var document = await BrowsingContext.New(configuration)
    .OpenAsync(request => request.Content(
        "<script>document.body.textContent = getGreeting()</script>"));
```

JavaScript can call delegates and access the public members of objects exposed this way. For
advanced integration, `GetOrCreateJint(document)` returns the document's Jint `Engine`, which
lets host code inspect JavaScript values or invoke JavaScript functions directly.

Registering a `JsScriptingService` directly does not add the auxiliary integration that
`WithJs()` supplies: inline event-handler attributes and `javascript:` URL navigation.

### Capture `console.log`

`console.log` forwards its arguments to an `IConsoleLogger` registered for the browsing
context. Provide one with `WithConsoleLogger`:

```cs
var configuration = Configuration.Default
    .WithJs()
    .WithConsoleLogger(_ => new ApplicationConsoleLogger());
```

Implement `IConsoleLogger.Log(Object[] values)` to send the values to your application's
logging system. Without a logger, calls to `console.log` do not produce output.

For example, a minimal logger can forward values to `System.Diagnostics`:

```cs
sealed class ApplicationConsoleLogger : IConsoleLogger
{
    public void Log(Object[] values)
    {
        Debug.WriteLine(String.Join(" ", values));
    }
}
```

## DOM APIs and integration points

AngleSharp.Js exposes AngleSharp DOM interfaces to JavaScript dynamically. The available
surface therefore follows the AngleSharp services registered in the browsing context. For
example, adding CSS support also makes its AngleSharp DOM types available to scripts.

In addition to the DOM provided by AngleSharp, the package supplies:

- `console.log`, `atob`, `btoa`, `DOMParser`, `Image`, `screen`, and `XMLHttpRequest`.
- `javascript:` URL navigation.
- Inline event-handler attributes and DOM event callbacks.
- ES modules and import maps through Jint's module loader.

### Built-in browser facades

The additional browser-like APIs are deliberately small. Use this table when deciding whether
they meet a script's needs:

| API | Available behavior |
| --- | --- |
| `DOMParser` | `parseFromString` creates a document through the configured `IDocumentFactory`. The requested MIME type must be supported by that configuration. |
| `Image` | Creates an AngleSharp `<img>` element; optional width and height become its display dimensions. |
| `window.postMessage` | Queues a `message` event on the current window. It requires an event loop; it does not transfer objects, deliver to another browsing context, or enforce `targetOrigin`. |
| `XMLHttpRequest` | Supports `open`, `send`, request headers, status, text responses, and lifecycle events through the configured document loader. |
| `console` | Supports `console.log` only. |
| `screen` | Exposes fixed 1920-by-1080 dimensions and 24-bit color depth for compatibility. |

To replace the supplied behavior, register your own compatible AngleSharp service before
calling `WithJs()`. In particular, `WithJs()` preserves an existing `INavigator`, and the
`WithEventLoop` overloads accept either an existing `IEventLoop` or a factory for one.

## Supported behavior and limitations

AngleSharp.Js is a DOM and scripting integration, not a browser runtime. Script behavior
depends on the installed Jint and AngleSharp versions, plus the services that your
configuration supplies. Test the browser APIs your application relies on instead of assuming
complete browser parity.

Notable limitations include:

- The package does not calculate layout. Its `scroll*`, `client*`, and `offset*` element
  properties return `0`.
- The default `navigator` is intentionally minimal. Its platform is empty, registration
  methods are no-ops, and its user-agent value is a fixed compatibility string.
- Network-backed features such as external scripts and `XMLHttpRequest` require suitable
  AngleSharp requesters and resource loading configuration.
- `XMLHttpRequest` currently provides text responses only. Its `response`, `responseXML`, and
  `upload` properties are unavailable, `responseType` always has its empty value, and its
  `timeout` and `withCredentials` settings do not affect requests.
- The JavaScript engine executes application-provided or page-provided code in your process.
  Treat untrusted scripts as untrusted code and apply the constraints appropriate to your
  application.
