![logo](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Js/master/header.png)

# AngleSharp.Js

[![Build Status](https://img.shields.io/appveyor/ci/FlorianRappl/AngleSharp-Js.svg?style=flat-square)](https://ci.appveyor.com/project/FlorianRappl/AngleSharp-Js)
[![GitHub Tag](https://img.shields.io/github/tag/AngleSharp/AngleSharp.Js.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.Js/releases)
[![NuGet Count](https://img.shields.io/nuget/dt/AngleSharp.Js.svg?style=flat-square)](https://www.nuget.org/packages/AngleSharp.Js/)
[![Issues Open](https://img.shields.io/github/issues/AngleSharp/AngleSharp.Js.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.Js/issues)
[![StackOverflow Questions](https://img.shields.io/stackexchange/stackoverflow/t/anglesharp.svg?style=flat-square)](https://stackoverflow.com/tags/anglesharp)
[![CLA Assistant](https://cla-assistant.io/readme/badge/AngleSharp/AngleSharp.Js?style=flat-square)](https://cla-assistant.io/AngleSharp/AngleSharp.Js)

AngleSharp.Js extends the core AngleSharp library with a .NET-based JavaScript engine.

## Basic Configuration

(tbd)

## Vision and Status

The repository contains DOM bindings for the *Jint* JavaScript engine. *Jint* is fully ECMAScript 5 compatible and provides the basis for evaluating JavaScripts in the context of the AngleSharp DOM representation.

The library comes with a service that exposes `WithJs` to `IConfiguration`. This enables automatic evaluation of `script` elements that have a valid JavaScript type (or without any explicit type, since JavaScript is the default one). The DOM bindings are generated on the fly via reflection. Since *Jint* is interpreting JavaScript, the library can be published in compatibility with .NET Standard (2.0). The downside is that the performance is definitely worse than any compiled JavaScript engine would deliver. For most scripts that should not be a big issue.

## Features

(tbd)

## Participating

Participation in the project is highly welcome. For this project the same rules as for the AngleSharp core project may be applied.

If you have any question, concern, or spot an issue then please report it before opening a pull request. An initial discussion is appreciated regardless of the nature of the problem.

## License

The MIT License (MIT)

Copyright (c) 2015 - 2019 AngleSharp

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
