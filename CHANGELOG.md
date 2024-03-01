# 1.0.0

Released on ?.

- Fixed usage of document ready state (#87) @Sebbs128
- Updated to use AngleSharp v1
- Updated for Jint v3 (#89) @tomvanenckevort

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
