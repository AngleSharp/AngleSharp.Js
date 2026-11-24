---
title: "Questions"
section: "AngleSharp.Js"
---
# Frequently Asked Questions

## Why do the scripts of my page not run?

Check these, in order:

1. `WithJs()` is part of the configuration.
2. External scripts (`<script src="…">`) need a loader **with resource loading enabled**: `WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })`.
3. You read the DOM too early. Scripts run on the event loop thread; `await` the document with `WaitUntilAvailable()` before reading it.
4. The script has an error. Errors are not thrown to you; listen for `error` on the browsing context (see [Collect script errors](02-Examples.md#collect-script-errors)).
5. The page is inside a sandboxed `<iframe>` without `allow-scripts`, which never runs scripts.

## Why is `fetch` / `XMLHttpRequest` / `localStorage` / `getComputedStyle` undefined?

The JavaScript surface is derived from the AngleSharp services registered in the browsing context. AngleSharp.Js itself contributes only its own APIs (see below). Many common browser APIs live in other packages:

| Script API | Comes from | Register with |
| --- | --- | --- |
| `fetch`, `XMLHttpRequest`, `WebSocket`, `localStorage`, `sessionStorage`, cookies | AngleSharp.Io | `WithRequesters()` or `WithIo(new IoOptions())`, plus `WithDefaultLoader()` |
| `navigator.userAgent` and other navigator details | AngleSharp.Io | `WithNavigator()` |
| `getComputedStyle`, `matchMedia`, `CSSStyleSheet`, `element.style`, `CSS.*` | AngleSharp.Css | `WithCss()` |
| DOM, events, timers, `URL`, `Blob`, `MutationObserver`, `customElements` | AngleSharp | always |

If an API is missing, test with `typeof X` in `ExecuteScript` after adding the package that implements it.

## Which APIs does AngleSharp.Js add itself?

On top of what the registered AngleSharp libraries expose:

- `console` (all methods, forwarded to an `IConsoleLogger`)
- `atob`, `btoa`, `structuredClone`, `queueMicrotask`
- `requestAnimationFrame` / `cancelAnimationFrame`, `requestIdleCallback` / `cancelIdleCallback`
- `postMessage` on the window, `MessageChannel`, `MessagePort`
- `Worker`, `SharedWorker`
- `performance` (`now`, `mark`, `measure`, entries) and `PerformanceObserver`
- `crypto.getRandomValues`, `crypto.randomUUID`, and `crypto.subtle` (not on .NET Framework)
- `TextEncoder`, `TextDecoder`
- `DOMParser`, `Image`, `Audio`, `screen`, `DOMException`
- `window.origin`, `isSecureContext`, `parent`, `top`, `length`, `window.event`
- ES modules, import maps, JSON and CSS module scripts
- inline event attributes (`onclick="…"`) and `javascript:` URLs

The JavaScript language itself (ES2015–ES2025: classes, `async`/`await`, `Proxy`, `BigInt`, iterator helpers, …) is provided by Jint.

## Why are `offsetWidth`, `clientHeight`, `scrollTop` and friends zero?

AngleSharp does not lay out or render pages. The `offset*`, `client*` and `scroll*` properties exist so that scripts do not fail, but always return `0`. Scripts that make decisions based on sizes or visibility may therefore take a different path than in a browser. If you need layout, AngleSharp.Css's render tree gives an approximation, but it is not fed back into these properties.

## Why does `ExecuteScript` return `"undefined"` instead of `null`?

`undefined` is converted to the string `"undefined"`. A script whose last statement is a declaration (`var x = 1;`, `function f() {}`) has no completion value and therefore also returns it. End the script with an expression (`x`) to get a value. Numbers are always `Double`; objects and arrays come back as Jint values, so return `JSON.stringify(…)` when you want plain data.

## Why does `engine.Evaluate(…)` not see the page's functions?

The page's scripts run in a scope bound to the window, not directly on the Jint global object. `Engine.Evaluate`, `Execute`, `GetValue` and `Invoke(String, …)` work on the bare global and report `X is not defined`. Use `document.ExecuteScript(…)`; to call a function with .NET arguments, obtain it via `ExecuteScript("fnName")` and call `engine.Invoke(function, args)`.

## `WaitUntilAvailable()` returned, but the content is still not there

`WaitUntilAvailable` waits for the `load` event and for the tasks queued at that point. Content that a page produces later — after a `setTimeout`, an `XMLHttpRequest`/`fetch` response, or a promise chain waiting on the network — is not covered. Wait for the effect instead: poll for a selector with a timeout, or expose a callback through `JsScriptingService.External` that the page (or a script you inject with `ExecuteScript`) calls. See [Wait for something a script does later](02-Examples.md#wait-for-something-a-script-does-later).

## How do I wait for a timer or a request started by a script?

Same answer as above: there is no global "network idle" signal. Typical approaches are:

- `await document.WhenStable()` in a loop with `Task.Delay`, until a selector appears;
- inject a hook: `document.ExecuteScript("someLib.on('ready', () => notifyHost())")` with `notifyHost` registered in `External`;
- listen to a DOM event from .NET: `document.AddEventListener("my-event", …)`.

## Can I expose my own .NET classes as real DOM interfaces?

Two ways:

- **Host values (simplest).** Put objects or delegates into `JsScriptingService.External`, or call `engine.SetValue` in an `EngineCreator`. Scripts then use them through Jint's regular CLR interop (public members, also camel-cased).
- **DOM-style bindings.** Decorate the types with `[DomName]`, `[DomExposed("Window")]`, `[DomConstructor]` and friends, exactly like AngleSharp does. These get prototypes, `instanceof`, WebIDL-like conversions and so on. AngleSharp.Js only reflects over assemblies  whose name starts with `AngleSharp` **and** that contribute a service registered in the browsing context (e.g. `.WithOnly<MyService>(_ => new MyService())`). Types in an assembly with another name are not picked up, so use the host values approach for application code.

See [How .NET types become JavaScript](01-API.md#how-net-types-become-javascript) for the attribute reference.

## Are global variables shared between documents, frames or contexts?

No. Each window has its own Jint engine with its own globals. Frames get their own window and engine as well; same-origin frames can reach each other through `parent`, `top`, `frames` and `contentWindow` like in a browser, and cross-origin windows only expose the cross-origin-safe members (`postMessage`, `location`, `closed`, …).

## Is it safe to run untrusted scripts?

Scripts run in-process and can call everything that is exposed to them, including the values you put into `External` and whatever the registered AngleSharp services can do (e.g. network access via the loader). There is no isolation like in a browser sandbox. At minimum:

- set limits via an `EngineCreator` (`MaxStatements`, `TimeoutInterval`, `LimitMemory`, `RegexTimeoutInterval`) and keep `MaxCallStackDepth` positive;
- only register the services (loaders, requesters, storage) the script is allowed to use;
- do not expose sensitive .NET objects through `External`;
- consider running untrusted work in a separate process.

## Why is my process crashing with a `StackOverflowException`?

Either `JsScriptingOptions.MaxCallStackDepth` was set to zero or less, or the configured depth needs more native stack than the event loop thread has. Lower `MaxCallStackDepth`, or raise the thread's stack with `WithEventLoop(_ => new JsEventLoop(64 * 1024 * 1024))`.

## Does it work with jQuery, React, Bootstrap, …?

Yes, the test suite runs jQuery 1–4, React 16 and Bootstrap 5 against the DOM. Larger frameworks usually work as long as they do not depend on layout (sizes are always `0`) or on APIs from packages you have not registered. Remember that Jint is an interpreter: a big bundle can take noticeably longer than in a browser. Parsed scripts are cached process-wide, so loading the same library again is cheaper.

## Is it fast?

Jint interprets JavaScript, so CPU-heavy scripts are much slower than in a JIT-compiling browser engine. The DOM binding itself is built lazily and cached, so the per-document overhead is mostly proportional to what the scripts actually touch. Practical tips:

- reuse one `IConfiguration` (and `BrowsingContext` where suitable);
- leave `IsResourceLoadingEnabled` off when you only need inline scripts;
- do not register services whose APIs the scripts do not need;
- set `RetainFunctionSourceText = false` if no script relies on `Function.prototype.toString()`.

## Is `ExecuteScript` thread-safe?

The engine of a window is locked while a script runs, so calls from different threads are serialized. They do not, however, wait for the event loop: if page scripts are still being queued, prefer `document.Then("…")`, which runs your code in order with the page's own tasks.

## Which .NET versions are supported?

The package targets `netstandard2.0`, `net462`, `net472`, `net8.0` and `net10.0`. The Web Cryptography API (`crypto.subtle`) is not available on .NET Framework; `crypto.getRandomValues` and `crypto.randomUUID` are.
