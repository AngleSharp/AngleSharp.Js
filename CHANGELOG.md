# 1.2.0

Released on Friday, November 6 2026.

- Fixed `window.origin` being `null` for a frame without an address of its own instead of the origin of its creator
- Fixed text given to `append`, `prepend` and the like failing instead of becoming a text node
- Fixed a class deriving from a type of the DOM not having its instances on its prototype chain
- Fixed an object with `handleEvent` not being accepted as an event listener, or one with `acceptNode` as a filter of a `TreeWalker` or `NodeIterator`
- Fixed a function not being accepted as a callback whose result is waited for, e.g. of `navigator.locks.request`: it runs on the event loop and is followed to its promise
- Fixed promise jobs not running after a timer, a listener or any other callback of the host: they waited for something else to run them, so `await` could hang
- Fixed a `DOMException` with a name having no code (e.g. `NotAllowedError`) being named `Error` for script
- Fixed a date from the DOM not being a `Date` for script, and bytes not surviving a structured clone
- Fixed a function not being accepted as a callback with a result, e.g. as a filter of a `TreeWalker`
- Fixed a worker that failed to start throwing from `postMessage` instead of firing an `error` event
- Fixed collections from attributes, e.g. `document.links` or `element.children`, not being iterable
- Fixed `classList` not being iterable and having no `forEach`, `entries`, `keys` and `values`
- Fixed a member of an options dictionary without a default becoming `undefined` in text
- Fixed members of the DOM being read-only and copied onto every prototype, so polyfills could neither replace them nor reach derived types (e.g. patching `Element.prototype.querySelector`): each is now on the prototype of the interface declaring it, as WebIDL says (writable, enumerable and configurable)
- Fixed interface objects of the window being read-only and enumerable: `window.Node = ...` now also changes `Node`, as WebIDL says
- Fixed bound functions and callable proxies not being accepted as callbacks (e.g. `setTimeout(f.bind(x))`, `addEventListener('click', f.bind(x))`, `requestAnimationFrame(f.bind(x))`, `onclick`), failing with `Could not find corresponding cast target`
- Fixed `removeEventListener` not removing listeners, as a function was turned into another listener each time
- Fixed errors of event listeners not reaching the `error` listeners of the window; an error thrown by such a listener goes to the browsing context
- Fixed the address of a script getting lost, so errors and stack traces named `<anonymous>`: they tell the resolved address of the script, or of the document for one written into it
- Fixed scripts written into a document telling lines and columns of their own in errors and stack traces instead of the ones in the document
- Fixed errors of scripts and of callbacks (e.g. timers, animation frames) not reaching the `error` listeners of the window; what they don't handle by `preventDefault` goes to the browsing context
- Fixed `onerror` of the window getting the event instead of the message, source, line, column and error, and returning `true` not handling the error
- Fixed `error` of an `ErrorEvent` being a .NET exception instead of what script threw
- Fixed an `onclick` and the like throwing taking down the process and skipping the other listeners, as it was not reported like a listener
- Fixed a `TypeError` or the like thrown for script by a member of the DOM becoming a plain `Error`
- Fixed modules being read from the local disk; every module is loaded through the configured requesters (#153) @michaelthyregod
- Fixed modules imported by a relative address being requested from the wrong address (e.g. `./a.js` of `/dir/page.html` from `/dir/page.html/a.js`, and the imports of a module of another origin from the own origin); they are resolved against what imports them (#153) @michaelthyregod
- Fixed modules being known by the path of their address, so errors didn't tell the whole address, and imports of an imported module were looked for from the wrong place
- Fixed a module written into a document telling lines and columns of its own, and importing from somewhere other than the base address of the document
- Fixed an error of a module being reported twice
- Fixed an event handler (e.g. `onclick`) returning `false` not cancelling the event
- Fixed an event handler attribute being added on top of the previous one when it changed, staying when it was removed, and throwing a syntax error at whoever set the attribute instead of reporting it
- Fixed only the first of several methods or constructors of the same name being callable (e.g. `new URLSearchParams('a=1')`, `register(url, options)`); the one fitting the arguments is called
- Fixed static members not being reachable (e.g. `DOMRect.fromRect`); they are on the constructor and may take the window as first parameter
- Fixed members of classes marked with `DomExposed` ending up on what they are exposed to (e.g. the members of a constructor on `window`); only extension methods do
- Fixed arrays and other iterables not converting to sequences (arrays and the interfaces they implement)
- Fixed `ArrayBuffer`, typed arrays, and `DataView` not converting to `Byte[]`, `ArraySegment<Byte>`, and `Object` (e.g. a request body being sent as `[object Uint8Array]`)
- Fixed numbers not converting from and to `Int64`, `UInt64`, `Int16`, `Byte`, `SByte`, `Single` (only from), and `Decimal`, as well as nullable forms of all value types
- Fixed values of `DomLiterals` enums being numbers instead of strings (e.g. `shadowRoot.mode`), and such enums being exposed as objects
- Fixed `Byte[]` being given as an array of numbers instead of an `ArrayBuffer`
- Fixed errors thrown by constructors losing their message
- Fixed WebIDL stringifiers (`URL`, `URLSearchParams`, `Location`, `Range`, `DOMTokenList`, `DOMMatrix`) answering `String(x)` with `[object X]`; a type overriding `ToString` has a `toString`
- Fixed sandboxed browsing contexts without `allow-scripts` running scripts and event handler attributes
- Fixed `postMessage` ignoring `targetOrigin`, giving it as the origin of the event, turning objects into strings, and not having a `source`; messages are copied by the structured clone algorithm, and the options form is supported
- Added `clearResourceTimings` and `setResourceTimingBufferSize` to `performance`
- Added `{ type: 'module' }` and `onerror` to `Worker`, with imports being loaded by the worker
- Added `MessageChannel` with queued message delivery, structured cloning, and transferable ports for windows and workers
- Added `IScriptValueCloner`, so that values of script can be kept, e.g. by IndexedDB
- Added `structuredClone`, `origin`, `isSecureContext` and `length` to the window
- Added `DOMException`, which is what the DOM now throws for scripts: an `Error` with the `name` and `code` of the standard, also available as a constructor and with its constants
- Added `TextEncoder` and `TextDecoder`
- Added the iterator of a type giving `entries` (WebIDL `iterable<K, V>`), so that e.g. `for (const [k, v] of new URLSearchParams(s))` works
- Added overloads brought by extensions for the methods of the DOM, as `addEventListener` with options has them
- Added `crypto` with `getRandomValues` and `randomUUID`, but without `subtle`
- Added `requestIdleCallback` and `cancelIdleCallback`
- Fixed `DOMParser` replacing the document of the page with what it parsed, and running its scripts
- Added `PerformanceObserver`, for the marks and measures of `performance`
- Added custom elements, run for the registry of AngleSharp: classes deriving from `HTMLElement` or another element, with their callbacks running when the call of a script returns (requires AngleSharp 1.10)
- Fixed classes deriving from a DOM type taking every object of that type for an instance of them
- Added what a promise rejects with being a `DOMException` or what was thrown, if the task behind it failed with one
- Added `import.meta.url` to modules; without it, e.g., webpack stopped with `Automatic publicPath is not supported in this browser`
- Added `ArraySegment<Byte>` being given as a `Uint8Array`
- Added classes with implicit conversions standing for a WebIDL union, e.g. of a string and a sequence of strings, which fixes such conversions failing; an implicit conversion from such a class is what a script receives for it
- Added the same-origin policy: script gets `null` for a node of another origin, and for its window a `CrossOriginWindow` with only `window`, `self`, `frames`, `parent`, `top`, `closed`, `length`, `location` (only to be set, and only for frames inside the own document), `postMessage`, `focus`, `blur` and `close`
- Added `performance.mark`, `measure`, `getEntries`, `getEntriesByName`, `getEntriesByType`, `clearMarks`, `clearMeasures`, along with `PerformanceEntry`, `PerformanceMark` and `PerformanceMeasure`

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
