![logo](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Js/master/header.png)

# AngleSharp.Js

[![CI](https://github.com/AngleSharp/AngleSharp.Js/actions/workflows/ci.yml/badge.svg)](https://github.com/AngleSharp/AngleSharp.Js/actions/workflows/ci.yml)
[![GitHub Tag](https://img.shields.io/github/tag/AngleSharp/AngleSharp.Js.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.Js/releases)
[![NuGet Count](https://img.shields.io/nuget/dt/AngleSharp.Js.svg?style=flat-square)](https://www.nuget.org/packages/AngleSharp.Js/)
[![Issues Open](https://img.shields.io/github/issues/AngleSharp/AngleSharp.Js.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.Js/issues)
[![Gitter Chat](http://img.shields.io/badge/gitter-AngleSharp/AngleSharp-blue.svg?style=flat-square)](https://gitter.im/AngleSharp/AngleSharp)
[![StackOverflow Questions](https://img.shields.io/stackexchange/stackoverflow/t/anglesharp.svg?style=flat-square)](https://stackoverflow.com/tags/anglesharp)
[![CLA Assistant](https://cla-assistant.io/readme/badge/AngleSharp/AngleSharp.Js?style=flat-square)](https://cla-assistant.io/AngleSharp/AngleSharp.Js)

AngleSharp.Js extends the core AngleSharp library with a .NET-based JavaScript engine.

## Basic Configuration

If you just want a configuration *that works* you should use the following code:

```cs
var config = Configuration.Default
    .WithJs(); // from AngleSharp.Js
```

This will register a scripting engine for JS files. The engine can be tuned by passing a `JsScriptingOptions` instance to `WithJs`:

```cs
var config = Configuration.Default
    .WithJs(new JsScriptingOptions
    {
        // how deep a script may recurse before the engine reports
        // "Maximum call stack size exceeded" (10000 by default)
        MaxCallStackDepth = 5000,
    });
```

You can also use this part with a console for logging. The call for this is `WithConsoleLogger`, e.g.,

```cs
var config = Configuration.Default
    .WithJs()
    .WithConsoleLogger(ctx => new MyConsoleLogger(ctx));
```

in the previous example `MyConsoleLogger` refers to a class implementing the `IConsoleLogger` interface. Examples of classes implementing this interface are available in our [samples repository](https://github.com/AngleSharp/AngleSharp.Samples).

## Extension Methods

This plugin also delivers some extension methods to be used together with elements from AngleSharp, e.g., `IDocument` or `IElement`.

For instance, the following waits until a stable point has been reached (and the document is fully available):

```cs
var context = BrowsingContext.New(config);
var document = await context.OpenAsync(address)
    .WaitUntilAvailable();
```

Scripts can also be run in the context of the document, where the result of the last expression is returned:

```cs
var numEntries = document.ExecuteScript("document.querySelectorAll('div').length");
```

## Vision and Status

The repository contains DOM bindings for the *Jint* JavaScript engine. *Jint* implements ECMAScript 2015 (ES6) through ECMAScript 2025 — classes and private fields, modules, generators, `async`/`await`, `Promise`, `Proxy`/`Reflect`, `Symbol`, typed arrays, `BigInt`, optional chaining, and iterator helpers among them — and provides the basis for evaluating JavaScripts in the context of the AngleSharp DOM representation. See the [Jint feature list](https://github.com/sebastienros/jint#supported-features) for the authoritative matrix.

The library comes with a service that exposes `WithJs` to `IConfiguration`. This enables automatic evaluation of `script` elements that have a valid JavaScript type (or without any explicit type, since JavaScript is the default one), including `type="module"` and `type="importmap"`. The DOM bindings are generated on the fly via reflection. Since *Jint* is interpreting JavaScript, the library can be published in compatibility with .NET Standard (2.0). The downside is that the performance is definitely worse than any compiled JavaScript engine would deliver. For most scripts that should not be a big issue.

## Features

- Support of modern JavaScript (ES2015 through ES2025) through Jint
- Connection to the DOM
- ES modules and import maps (`<script type="module">`, `<script type="importmap">`)
- Web Workers
- Evaluation of real-world libraries (the test suite runs jQuery 1 through 4, React 16 and Bootstrap 5)

## Participating

Participation in the project is highly welcome. For this project the same rules as for the AngleSharp core project may be applied.

If you have any question, concern, or spot an issue then please report it before opening a pull request. An initial discussion is appreciated regardless of the nature of the problem.

Live discussions can take place in our [Gitter chat](https://gitter.im/AngleSharp/AngleSharp), which supports using GitHub accounts.

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.

For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

## License

AngleSharp.Js is released using the MIT license. For more information see the [license file](./LICENSE).
