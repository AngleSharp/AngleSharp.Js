# 1.1.4

Released on Sunday, November 1 2026.

- Fixed callbacks getting their first parameter as `this` instead of as an argument (e.g. a `MutationObserver` callback got the observer instead of the records); only a leading window (timers, animation frames) is the receiver
- Fixed arrays (e.g. the records of a `MutationObserver`) not being JS arrays, so `for...of` failed with "The value is not iterable"
- Fixed members returning a task (e.g. `fetch`) giving an object without `then`; they return a promise settled on the event loop
- Fixed dictionaries (e.g. the parsed body of a JSON response) not being plain JS objects
- Added `unhandledrejection` (`PromiseRejectionEvent`, `window.onunhandledrejection`); a rejection nobody handles is reported as an error of the browsing context

# 1.1.3

Released on Friday, October 30 2026.

- Fixed prepared regular-expression literals ignoring the timeout configured during engine creation
- Fixed the console only having `log`; it now has all methods of the console standard (e.g. `error`, `warn`, `info`, `debug`, `assert`, `count`, `time`), as scripts calling or wrapping one of them failed
- Fixed reading the globals of scripts (e.g. `var x`, `x = 1`) and the built-ins (e.g. `Promise`, `JSON`) through `window`, which gave `undefined`
- Fixed named indexers (e.g. `item` of `NodeList` and `HTMLCollection`, `namedItem`) being exposed as properties instead of methods
- Fixed functions passed for delegates with value-type or no parameters failing to convert
- Fixed errors thrown by DOM members losing their message
- Fixed only the last of several extension methods of the same name (e.g. `scrollTo(x, y)` and `scrollTo(options)`) being callable; the one fitting the arguments is called
- Fixed the geometry stubs of this library (e.g. `offsetHeight` always 0) hiding the implementation of another library (e.g. AngleSharp.Renderer)
- Fixed plain objects not converting to dictionaries (classes with `DomName` members, e.g. `{ top: 100 }` to `ScrollToOptions`)
- Fixed `dataset` rejecting camel-cased names (e.g. `dataset.sideNavVisible` for `data-side-nav-visible`) with a syntax error
- Added `requestAnimationFrame`, `cancelAnimationFrame`, `queueMicrotask`, and `performance` (`now()`, `timeOrigin`) to the window

# 1.1.2

Released on Sunday, October 25 2026.

- Fixed globals named like a member of the window (e.g. `var item`, `var open`, `function close() {}`) keeping that member, as the members were copied to the global scope as read-only

# 1.1.1

Released on Wednesday, October 7 2026.

- Fixed background thread leaks when worker construction is rejected (#143) @PrzemyslawKlys
- Fixed canonical DOM node identity and concrete prototypes through `DomSameObject` getters (#141) @PrzemyslawKlys
- Added `JsScriptingOptions.EngineCreator` to create a configured Jint engine before any page script runs (#142) @PrzemyslawKlys

# 1.1.0

Released on Saturday, September 5 2026.

- Updated to require at least AngleSharp 1.8.0
- Added support for the `DomReturnType` attribute
- Added support for the `DomSameObject` attribute
- Added iterable integration for `IEnumerable<T>` instances

# 1.0.1

Released on Wednesday, August 5 2026.

- Updated to require at least AngleSharp 1.5.0
- Fixed `AmbiguousMatchException` in case of multiple `DomName` attributes (#136)
- Added support for aliases with multiple `DomName` attributes

# 1.0.0

Released on Friday, July 31 2026.

- Fixed `StackOverflowException` from deeply recursive scripts (#75) @lahma
- Fixed invoking JavaScript functions with DOM objects created in C#, which reached script as a plain CLR wrapper instead of as the DOM object (#76)
- Fixed usage of document ready state (#87) @Sebbs128
- Fixed `HasChildNodes` is now exposed as a method to DOM (#106) @arekdygas
- Fixed missing `parent` on Window objects (#68)
- Fixed missing `NodeFilter` (#101)
- Fixed dataset updates from C#-invoked JavaScript functions not reflected in DOM (#77)
- Fixed bare `location = '...'` assignment not updating navigation state (#98)
- Fixed XMLHttpRequest handling for relative URLs and failed requests
- Removed `Navigator` implementation (moved to *AngleSharp.Io*)
- Removed `XmlHttpRequester` implementation (moved to *AngleSharp.Io*)
- Updated to use AngleSharp v1
- Updated for Jint v4 (#89, #97) @tomvanenckevort @lahma
- Updated CreatorCache to be thread-safe (#110) @badnickname
- Updated DOM prototypes to be built from a member layout shared by the whole process, which resolves a type once instead of once per document and lets the engine cache warm member reads @lahma
- Added report for uncaught event-loop exceptions through the browsing context
- Added `JsScriptingOptions` to tune the engine via `WithJs` (#75) @lahma
- Added support for Web Workers (#78)

# 0.15.0

Released on Sunday, June 12 2021.

- Updated to use AngleSharp 0.15

# 0.14.0

Released on Tuesday, March 31 2020.

- Fixed deadlock with some versions of jQuery (#63)
- Enabled the instance creator factory (#11)
- Return `IDocument` from `WaitUntilAvailable()` (#61)

# 0.13.0

Released on Friday, September 6 2019.

- Fixed missing `btoa` and `atob` functions (#55)
- Added `javascript:` URL handler (#47)
- Added new `WithEventLoop` configuration extensions
- Added support for .NET Standard 1.3 (#58)
- Added constructors to `window` (#12)
- Added thread-based event loop implementation `JsEventLoop`

# 0.12.1

Released on Wednesday, May 15 2019.

- Binary version fix

# 0.12.0

Released on Tuesday, May 14 2019.

- Properly forward setting window.location (#31)
- Restored compatibility with AngleSharp v0.12 (#51)
- Renamed to `AngleSharp.Js` (focus only on JavaScript) (#51)
- Renamed the `WithJavaScript` extension method to `WithJs`
- Updated the namespace from `AngleSharp.Scripting.JavaScript` to `AngleSharp.Js`
- Added support for window.onload event (#42)
- Added support for more APIs to enable jQuery (#43)
- Added support for DOMContentLoaded event (#50)

# 0.5.1

Released on Sunday, May 7 2017.

- Updated to latest versions (#34, #37)

# 0.5.0

Released on Monday, October 3 2016.

- Several bug fixes and improvements
- Construct prototype chain (#7)
- Added better type casts and wrappers (#8)
- Allow strings instead of functions (#26)
- Include extension method `ExecuteScript` to `document` (#29)
- Simplified DOM context evaluation (#19)

# 0.4.2

Released on Sunday, September 4 2016.

- Updated to latest version of AngleSharp
- Allow getting the engine without a script (#28)

# 0.4.1

Released on Sunday, July 17 2016.

- Release as a portable library (profile 259)
- Adjustable console logger

# 0.4.0

Released on Friday, May 6 2016.

- Fixed some bugs (#24, #20, #17, #16)
- Added (pseudo) view extensions (#15)
- Added XHR object (#14)
- Provided event constructor (#5)

# 0.3.0

Released on Thursday, August 27 2015.

- Enhanced C# / JavaScript intergration (#9)
- Enabled extending of `IWindow` (#6)
- Editing of IHtmlInlineFrameElement content (#4)
- Extensibility of scripting engine (#3)

# 0.1.0

Released on Sunday, August 2 2015.

- Initial release
