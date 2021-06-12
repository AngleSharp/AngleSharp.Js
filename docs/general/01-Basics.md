---
title: "Getting Started"
section: "AngleSharp.Js"
---
# Getting Started

## Requirements

AngleSharp.Js comes currently in two flavors: on Windows for .NET 4.6 and in general targetting .NET Standard 2.0 platforms.

Most of the features of the library do not require .NET 4.6, which means you could create your own fork and modify it to work with previous versions of the .NET-Framework.

You need to have AngleSharp installed already. This could be done via NuGet:

```ps1
Install-Package AngleSharp
```

## Getting AngleSharp.Js over NuGet

The simplest way of integrating AngleSharp.Js to your project is by using NuGet. You can install AngleSharp.Js by opening the package manager console (PM) and typing in the following statement:

```ps1
Install-Package AngleSharp.Js
```

You can also use the graphical library package manager ("Manage NuGet Packages for Solution"). Searching for "AngleSharp.Js" in the official NuGet online feed will find this library.

## Setting up AngleSharp.Js

To use AngleSharp.Js you need to add it to your `Configuration` coming from AngleSharp itself.

If you just want a configuration *that works* you should use the following code:

```cs
var config = Configuration.Default
    .WithJs(); // from AngleSharp.Js
```

This will register a scripting engine for JS files. The JS parsing options and more could be set with parameters of the `WithJs` method.

You can also use this part with a console for logging. The call for this is `WithConsoleLogger`, e.g.,

```cs
var config = Configuration.Default
    .WithJs()
    .WithConsoleLogger(ctx => new MyConsoleLogger(ctx));
```

in the previous example `MyConsoleLogger` refers to a class implementing the `IConsoleLogger` interface. Examples of classes implementing this interface are available in our [samples repository](https://github.com/AngleSharp/AngleSharp.Samples).
