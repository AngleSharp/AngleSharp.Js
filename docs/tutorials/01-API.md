---
title: "API Documentation"
section: "AngleSharp.Js"
---
# API Documentation

This page lists the public .NET API of AngleSharp.Js and explains how .NET types become JavaScript objects. For a guided introduction see [Getting Started](../general/01-Basics.md).

The package targets `netstandard2.0`, `net462`, `net472`, `net8.0` and `net10.0`, and depends only on AngleSharp and [Jint](https://github.com/sebastienros/jint).

## Overview

| Type / member | Namespace | Purpose |
| --- | --- | --- |
| `WithJs()`, `WithJs(JsScriptingOptions)` | `AngleSharp` | Registers the JavaScript engine and its auxiliary services. |
| `WithEventLoop()` and overloads | `AngleSharp` | Registers the event loop that runs scripts, timers and callbacks. |
| `WithConsoleLogger(Func<IBrowsingContext, IConsoleLogger>)` | `AngleSharp` | Routes `console.*` output to your code. |
| `ExecuteScript(document, code, type, sourceUrl)` | `AngleSharp.Js` | Runs JavaScript in the document's engine and returns the result. |
| `Then`, `WhenStable`, `WaitUntilAvailable` | `AngleSharp.Dom` | Synchronize .NET code with the event loop. |
| `JsScriptingService` | `AngleSharp.Scripting` | The `IScriptingService`; holds host values and gives access to the Jint  engine. |
| `JsScriptingOptions` | `AngleSharp.Js` | Call stack depth and function source retention. |
| `EngineCreator` | `AngleSharp.Js` | Delegate service to build the Jint `Engine` yourself (limits, timeouts). |
| `JsEventLoop` | `AngleSharp.Js` | The default `IEventLoop`, a dedicated background thread. |
| `IConsoleLogger` | `AngleSharp.Js` | Receives the values passed to `console.*`. |
| `JsNavigationHandler` | `AngleSharp.Js` | Handles `javascript:` URLs. |
| `DomConstructorFunctionAttribute` | `AngleSharp.Js.Attributes` | Marks a method as a factory-style constructor function (like `Image`). |

Typical `using` directives:

```cs
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Js;
using AngleSharp.Scripting; // only for JsScriptingService
```

## Configuration extensions

All of these extend `IConfiguration` and live in `AngleSharp.JsConfigurationExtensions`.

### `WithJs()` / `WithJs(JsScriptingOptions options)`

Registers:

- a `JsScriptingService` (the engine) configured with a copy of `options`,
- an `EventAttributeObserver`, which turns `onclick="…"` and similar attributes into handlers,
- a `JsNavigationHandler` for `javascript:` URLs,
- a structured clone service (used by `postMessage`, `structuredClone`, workers),
- a custom element host, so `customElements.define` works with JavaScript classes.

Calling `WithJs` does **not** register an event loop or a loader. Most documents that run scripts need both (see `WithEventLoop` and AngleSharp's `WithDefaultLoader`).

### `WithEventLoop()`

```cs
IConfiguration WithEventLoop(this IConfiguration configuration);
IConfiguration WithEventLoop(this IConfiguration configuration, IEventLoop loop);
IConfiguration WithEventLoop(this IConfiguration configuration, Func<IBrowsingContext, IEventLoop> creator);
```

The parameterless overload creates one `JsEventLoop` per browsing context. Pass an instance to share one loop, or a factory to customize the loop per context. Each overload replaces a previously registered `IEventLoop` (`WithOnly`).

Without an event loop, scripts still run, but nothing is queued: timers, `postMessage` to the window, and the `Then`/`WhenStable` synchronization have nothing to wait for.

### `WithConsoleLogger(Func<IBrowsingContext, IConsoleLogger> createLogger)`

Registers a factory for an `IConsoleLogger`. It is ignored if a logger is already registered.

## Running scripts from .NET

### `ExecuteScript`

```cs
public static Object ExecuteScript(this IDocument document, String scriptCode,
    String scriptType = null, String sourceUrl = null);
```

Defined in `AngleSharp.Js.JsApiExtensions`. Runs `scriptCode` synchronously in the engine of the document's window and returns the completion value of the script (the value of the last expression statement), converted to .NET:

| JavaScript value | .NET value |
| --- | --- |
| `undefined` (also a script ending in a declaration such as `var x = 1;`) | the **string** `"undefined"` |
| `null` | `null` |
| boolean | `Boolean` |
| number | `Double` (always, also for integers) |
| string | `String` |
| BigInt | `System.Numerics.BigInteger` |
| DOM object (`document.body`, …) | the AngleSharp object (`IHtmlElement`, …) |
| other objects, arrays, dates, functions | the Jint object (`Jint.Native.JsObject`, `JsArray`, `JsDate`, function), a `JsValue` |

When you need structured data, return `JSON.stringify(…)` and deserialize it in .NET.

Remarks:

- `scriptType` defaults to `text/javascript`. Any JavaScript MIME type runs a classic script, `module` runs an ES module, `importmap` registers an import map; other types return `null`.
- `sourceUrl` is used in error messages and stack traces, and as the base address for imports.
- Errors are thrown to the caller: a JavaScript error as `Jint.Runtime.JavaScriptException` (also for syntax errors), a violated Jint constraint as the respective Jint exception (e.g. `StatementsCountOverflowException`).
- It returns `null` when the configuration has no `JsScriptingService` or the document is in a sandboxed frame that may not run scripts.
- It does **not** go through the event loop. If page scripts are still loading, wait for them first (`WaitUntilAvailable`), or queue the call with `Then(String)`.

### Synchronizing with the event loop

Defined in `AngleSharp.Dom.JsDocumentExtensions`; every method exists for `IDocument` and for `Task<IDocument>`, so they can be chained directly after `OpenAsync`.

| Method | Behavior |
| --- | --- |
| `Then(Action<IDocument> action)` | Queues `action` on the event loop and completes after it ran. Runs immediately if there is no event loop. |
| `Then(String jsSource)` | Same, but runs JavaScript via `ExecuteScript`. |
| `WhenStable()` | Completes once every task queued so far has run. |
| `WaitUntilAvailable(CancellationToken cancellation = default)` | Waits for the `load` event of the document (if not yet complete), then for `WhenStable()`. The token aborts the wait for `load`. |

```cs
var document = await context.OpenAsync(address)
    .WaitUntilAvailable()
    .Then("document.title = 'changed'");
```

The returned tasks only cover work already in the queue. Work started later, for example a `setTimeout` callback or an `XMLHttpRequest` response, is not awaited; see the [FAQ](03-Questions.md#how-do-i-wait-for-a-timer-or-a-request-started-by-a-script).

## `JsScriptingService`

Namespace `AngleSharp.Scripting`. `WithJs()` creates one for you; create it yourself when you want to use `External` or `GetOrCreateJint`, and register it with `.With(service)`.

| Member | Description |
| --- | --- |
| `JsScriptingService()` / `JsScriptingService(JsScriptingOptions options)` | Creates the service. The options are copied. |
| `IDictionary<String, Object> External` | Values set as globals in every engine created afterwards (one engine per window). |
| `Engine GetOrCreateJint(IDocument document)` | The Jint engine of the document's window, created on first use. |
| `Object EvaluateScript(IDocument document, String source, String type, String sourceUrl)` | What `ExecuteScript` calls. |
| `String Type` | `text/javascript`. |

To keep the inline-event and `javascript:` URL support, register the service *and* call `WithJs()`, or prefer the `EngineCreator` service for host globals:

```cs
var scripting = new JsScriptingService();
scripting.External["appName"] = "MyApp";

var config = Configuration.Default
    .With(scripting)
    .WithEventLoop();
```

Each window has its own engine: `GetOrCreateJint(frameDocument)` returns a different engine than for the top-level document, and global variables are not shared between documents.

The page's scripts run in a window scope layered over the Jint global. `engine.Evaluate`, `engine.Execute`, `engine.GetValue("name")` and `engine.Invoke("name", …)` do not see the variables and functions the page declared. Use `ExecuteScript` to run code or to obtain a function, and `engine.Invoke(JsValue function, params Object[] arguments)` to call it.

## `JsScriptingOptions`

Namespace `AngleSharp.Js`. Copied when the service is created.

| Property | Default | Description |
| --- | --- | --- |
| `Int32 MaxCallStackDepth` | `10000` | Recursion depth before the script gets a `RangeError` ("Maximum call stack size exceeded"). Zero or less removes the guard; deep recursion then crashes the process with a `StackOverflowException`. |
| `Boolean RetainFunctionSourceText` | `true` | Keeps the source text so `Function.prototype.toString()` returns it. Turn off to save memory; functions then print as `function name() { [native code] }`. |

## `EngineCreator`

```cs
public delegate Jint.Engine EngineCreator(IWindow window, Jint.Options options);
```

Register an `EngineCreator` (or a `Func<IBrowsingContext, EngineCreator>`) as a service to create the Jint engine yourself. It is called once per window, before any page script runs.

- Always construct the engine with the given `options`: they carry the DOM object wrapper, the module loader and the call stack guard. Add to them, do not replace them.
- Return a new `Engine` each time; returning `null` throws `InvalidOperationException`.
- The DOM is installed after the delegate returns, so do not touch DOM objects in it. Setting host globals with `engine.SetValue` is fine.
- Workers created by the page also use the creator.

```cs
using Jint;

var config = Configuration.Default
    .With(new EngineCreator((window, options) => new Engine(options
        .MaxStatements(1_000_000)
        .TimeoutInterval(TimeSpan.FromSeconds(5))
        .LimitMemory(64 * 1024 * 1024)
        .RegexTimeoutInterval(TimeSpan.FromSeconds(1)))))
    .WithJs()
    .WithEventLoop();
```

An inline page script violating a constraint is reported to the browsing context (see [error handling](02-Examples.md#collect-script-errors)); `ExecuteScript` throws the Jint exception.

## `JsEventLoop`

Namespace `AngleSharp.Js`, implements `IEventLoop` and `IDisposable`.

| Constructor | Description |
| --- | --- |
| `JsEventLoop()` | Loop with the default worker thread stack size. |
| `JsEventLoop(IBrowsingContext context)` | Same, used by `WithEventLoop()`. |
| `JsEventLoop(Int32 maxStackSize)` | Uses the given stack size (bytes) for the worker thread. Raise it together with `MaxCallStackDepth`. |

All tasks of the loop run on one dedicated thread, one after another, ordered by priority (scripts are queued as critical). After each task the pending promise jobs (microtasks) run. Exceptions thrown by a task are reported as `error` events of the browsing context.

## `IConsoleLogger`

```cs
public interface IConsoleLogger
{
    void Log(Object[] values);
}
```

Every `console` method ends up in `Log`. The values are the converted arguments (strings, numbers, AngleSharp objects, …).

| Script call | What `Log` receives |
| --- | --- |
| `log`, `info`, `warn`, `error`, `debug`, `trace`, `dir`, `dirxml`, `table` | the arguments |
| `assert(condition, …)` | the arguments after `condition`, only if `condition` is falsy |
| `group(…)`, `groupCollapsed(…)` | the arguments, if any |
| `count(label)` | `"label: n"` |
| `time(label)` / `timeLog(label, …)` / `timeEnd(label)` | the elapsed time in milliseconds |
| `groupEnd()`, `clear()`, `countReset()` | nothing |

The severity is not passed on: `console.error` and `console.log` look the same to the logger.

## `JsNavigationHandler`

Registered by `WithJs()`. Navigating to a `javascript:` URL (e.g. clicking `<a href="javascript:doSomething()">`) runs the code in the current document instead of loading a new one.

## `DomConstructorFunctionAttribute`

Namespace `AngleSharp.Js.Attributes`. Placed on a static extension method of `IWindow` to expose it as a constructor function under the given name, the way `Image(width, height)` and `Audio(src)` are built:

```cs
[DomConstructorFunction("Image")]
public static IHtmlImageElement Image(this IWindow window, Int32? width = null, Int32? height = null)
```

## How .NET types become JavaScript

There is no generated binding code. When an engine is created, AngleSharp.Js reflects over the exported types of every assembly whose name starts with `AngleSharp` and that provides a service registered in the browsing context (AngleSharp itself, AngleSharp.Js, AngleSharp.Css, AngleSharp.Io, …). The attributes from `AngleSharp.Attributes` drive what scripts see:

| Attribute | Effect |
| --- | --- |
| `[DomName("X")]` on an interface or class | The JavaScript interface name (`HTMLDivElement`). Its instances get `X.prototype`. |
| `[DomName("x")]` on a member | Exposes the property, method or event under that name. Methods of the same name are overloads. |
| `[DomConstructor]` on a constructor | `new X(…)` is possible from script. |
| `[DomExposed("Window")]` on a type | Publishes the interface on the window (or on another interface or namespace, e.g. `[DomExposed("Worker")]`). On a static class, its `[DomName]` extension methods are added to the type they extend. |
| `[DomNoInterfaceObject]` | Members are available, but no global constructor is published. |
| `[DomInitDict]` on a parameter | The parameter is filled from the members of an options object. |
| `[DomLiterals]` on an enum | The enum values are strings (`"loading"`), otherwise numbers. |
| `[DomPutForwards("x")]` | Assigning the property assigns its member `x` (e.g. `location = "…"`). |

Value conversions follow WebIDL:

- **Into .NET:** JS numbers to any numeric type; functions to delegates (also bound functions and callable proxies); objects with `handleEvent`/`acceptNode` to the respective listener or filter; any iterable to arrays and the generic interfaces arrays implement; BufferSources to `Byte[]` when the parameter is `Object`; promises to `Task`; BigInt to `BigInteger`.
- **Out of .NET:** `[DomLiterals]` enums to strings, other enums to numbers (the enum type is exposed as an object of constants); `Byte[]` to a new `ArrayBuffer`; `ArraySegment<Byte>` to a new  `Uint8Array`; `Memory<Byte>` to an `ArrayBuffer` sharing the memory; other arrays to JS arrays; `IDictionary<String, Object>` to a plain object; `Task` to a promise; `DateTime` to `Date`; delegates to functions; DOM exceptions to errors of their own class (`instanceof Error`).
- Identity is preserved: the same AngleSharp node is always the same JavaScript object, so `document.body === document.body` holds and expandos stay attached.
- Members live on the prototype of the interface declaring them and are writable, so polyfills can patch them (`Element.prototype.querySelector = …`).
