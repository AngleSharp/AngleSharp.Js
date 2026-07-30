# AGENTS.md

Guidance for AI coding agents working in this repository. `CLAUDE.md` imports this file, so
keep it the single source of truth and every agent reads the same instructions.

AngleSharp.Js is an AngleSharp plugin that exposes the AngleSharp DOM to the
[Jint](https://github.com/sebastienros/jint) JavaScript engine. There is no code generation
and no hand-written binding layer: the whole JS surface is derived at runtime by reflecting
over AngleSharp's `[DomName]`-style attributes.

## Commands

The orchestrator is [Fallout](https://fallout.build) (`build/Build.cs`), the maintained hard fork
of NUKE. `build.ps1` / `build.sh` (`build.cmd` forwards to either) provision the SDK, then
`dotnet tool restore` + `dotnet fallout`; the CLI itself is pinned in
`.config/dotnet-tools.json` and resolves `build/_build.csproj` by convention. Default target is
`RunUnitTests`.

```powershell
.\build.ps1                        # restore, compile, run the full test suite
.\build.ps1 -Target Compile        # other targets: Clean Restore Compile RunUnitTests
.\build.ps1 -Target Package        #   CreatePackage Package PrePublish Publish
```

For the normal edit/test loop use the SDK directly — much faster than the Fallout bootstrap:

```powershell
dotnet build src/AngleSharp.Js.sln
dotnet test src/AngleSharp.Js.Tests/AngleSharp.Js.Tests.csproj -f net10.0
dotnet test src/AngleSharp.Js.Tests/AngleSharp.Js.Tests.csproj -f net10.0 --filter "FullyQualifiedName~InstanceOfTests"
dotnet test src/AngleSharp.Js.Tests/AngleSharp.Js.Tests.csproj -f net10.0 --filter "Name=WindowIsAnInstanceOfWindow"
```

- Always pass `-f net10.0` when iterating. On Windows the test project also targets `net462`
  and `net472`, so omitting it runs everything three times.
- `TreatWarningsAsErrors` is on (`src/Directory.Build.props`) — a warning breaks the build.
  There is no separate lint step; the compiler is it.
- Package versions are centralized in `src/Directory.Packages.props` (CPM is on), so a
  `PackageReference` in a csproj carries no `Version`. AngleSharp and Jint are deliberately
  stated as ranges: `dotnet pack` publishes them verbatim as the package's dependency ranges.
- The package version is parsed from the top entry of `CHANGELOG.md` (`ReleaseNotesParser`)
  and passed to the build as `-p:Version`. Release-worthy changes get a `CHANGELOG.md` line.
  `AssemblyVersion` is pinned to `1.0.0.0` so the strong-name identity survives that.
- `RunUnitTests` runs the suite twice, differing only in a `prefetched` environment variable
  that nothing in this repo currently reads — a single run is equivalent locally.
- There is deliberately no `global.json` — the repository does not restrict the SDK version. The
  bootstrap scripts use the STS channel, CI installs 10.0.x.

## Architecture

### Wiring into AngleSharp

`WithJs()` (`JsConfigurationExtensions`) registers four things: `JsScriptingService`, an
`EventAttributeObserver` (inline `onclick="…"` attributes), a `JsNavigationHandler`
(`javascript:` URLs), and a default `INavigator`. `WithEventLoop()` adds `JsEventLoop`, a
dedicated background thread that serializes script tasks — most DOM-mutating tests need it.

`JsScriptingService` is AngleSharp's `IScriptingService`. It keeps one `EngineInstance` per
`IWindow` in a `ConditionalWeakTable`, and accepts `text/javascript`, `module` and
`importmap`. **The set of assemblies whose types get exposed is derived from the services
registered in the browsing context** (every `AngleSharp*` assembly behind a registered
service), so adding `WithCss()` or `WithIo()` widens the JS DOM surface.

`EngineInstance` owns the Jint `Engine` and the per-engine caches. On construction it wraps
the window, walks each library's exported types to publish constructors, constructor
functions and instances, then copies the window's own properties onto the Jint global and
points the global's prototype at the window prototype. `RunScript` locks on the engine.

### The proxy objects (`src/AngleSharp.Js/Proxies`)

- **`DomNodeInstance`** — the JS object standing for one CLR DOM object. Identity is
  preserved by `ReferenceCache` (a `ConditionalWeakTable`), so the same node always maps to
  the same JS object. Only indexers become own properties; interface members live on the
  prototype. Writes to the window instance are mirrored onto the Jint global.
- **`DomPrototypeInstance`** — one per DOM type. Members are registered lazily in
  `Initialize()`: reflecting the full type tree is the expensive part and most prototypes
  are never touched. Also owns the numeric/string indexers and extension members pulled from
  `[DomExposed]` types.
- **`DomConstructorInstance`** — the exposed constructor (`HTMLDivElement`, …). The
  prototype holds it, so the object script reads off the window and the one an instance
  reports as its `constructor` are the same. It answers `Symbol.hasInstance` itself, because
  the prototype chain cannot cover mixins (`ParentNode`) or generic collections
  (`IHtmlCollection<T>`).
- **`DomConstructorDescriptor`** — the property a constructor is published under. Every
  exported type gets one, so the constructor object behind it is built only on first read.

### Type → prototype canonicalization

This is the subtlety most changes trip over. Instances are created from internal concrete
classes (`HtmlDivElement`), constructors are built from exported interfaces
(`IHtmlDivElement`), and many element classes carry no `[DomName]` of their own (a `b`
element is just an `HTMLElement`). `PrototypeTypeCache` folds all of these onto the topmost
class that defines a given DOM name, which is what makes `instanceof` and
`Object.getPrototypeOf(div) === HTMLDivElement.prototype` hold. Enums and generic types are
deliberately excluded from folding.

### Caching rules (`src/AngleSharp.Js/Cache`)

Split by what the cached value depends on:

- Process-wide statics — `CreatorCache`, `PrototypeTypeCache`, `MethodCache`, `ScriptCache`
  (capped at 32 prepared scripts) — hold values determined by a type, method or source alone.
- Per-engine — `PrototypeCache`, `ReferenceCache` — hold anything that is a `JsValue` or
  otherwise bound to one engine, plus anything depending on the engine's library set.

Anything shared across engines must be thread-safe; the existing caches all use
`Concurrent*` collections.

### Marshalling

`EngineExtensions.ToJsValue` and `JsValueExtensions.FromJsValue` / `.As(type, …)` convert
values. `EngineExtensions.BuildArgs` maps JS arguments onto a CLR signature and handles the
DOM-specific cases: an implicit leading `IWindow` parameter, optional parameters, `params`
arrays, and `[DomInitDict]` option objects expanded into positional arguments.

## Performance

**Performance is a primary concern in this repository, not an afterthought.** Jint is an
interpreter and every DOM access from script crosses this binding layer, so the binding must
never be the bottleneck. Reflection is the whole mechanism here, and reflection is slow — the
work of the last release cycle was largely making it happen once instead of once per call
(`perf/cache-interop-reflection`, `perf/cache-parsed-scripts`, `perf/lazy-dom-prototypes`,
`perf/lazy-dom-constructors`). Hold new code to that standard.

The two techniques that carry most of the win:

- **Cache anything derived from a type, method or source string.** It cannot change for the
  lifetime of the process. `MethodDescription.Of` resolves a signature once;
  `CreatorCache.GetConstructorDefinition` caches even the *null* answer, because most
  exported types are not constructors; `ScriptCache` keeps Jint's `Prepared<Script>`, which
  is documented as reusable and thread-safe, so a page loading jQuery parses it once for the
  whole process.
- **Do the work on first use, not up front.** A document touches a handful of the thousands
  of types an AngleSharp assembly exports. `DomPrototypeInstance.Initialize` defers the
  type-tree walk until Jint first serves a property; `DomConstructorDescriptor` publishes the
  property but builds the constructor object only when script reads it. When adding anything
  per-type, ask what it costs on a document that never touches that type.

Concrete .NET techniques that apply here:

- `ConcurrentDictionary.GetOrAdd` for caches shared across engines — lock-free reads on the
  hot path. Pass a cached delegate or a static lambda so the factory does not allocate a
  closure per call.
- `ConditionalWeakTable` when the cache key is a live object (`ReferenceCache` for node
  identity, `JsScriptingService` for the per-window engine). It gives identity semantics
  without keeping the DOM alive.
- `readonly struct` for small descriptors that are only read — `ParameterDescription` is one,
  and it keeps the parameter array allocation-free per element.
- Allocate lazily per instance: `DomNodeInstance._eventHandlers` and
  `DomPrototypeInstance._deferred` stay null until something is actually registered. Most
  nodes never need either, and there can be very many nodes.
- Prefer `for` over LINQ in anything reached per call or per property. LINQ allocates an
  enumerator and usually a closure; `BuildArgs` and `MethodDescription` are deliberately
  written as plain loops. LINQ is fine in the one-off setup paths that feed a cache.
- Size collections and arrays when the count is known, and avoid intermediate
  `ToArray()`/`ToList()` in code that runs more than once.
- `StringComparer.Ordinal` for name lookups — it is both faster and correct here
  (`XMLHttpRequest` and `XmlHttpRequest` are different DOM names). Use AngleSharp's
  `Is`/`Isi` extensions instead of `ToLower()` comparisons; they do not allocate.
- Jint-specific: `FastSetProperty` skips the property-definition protocol,
  `PropertyFlag.CustomJsValue` lets a descriptor stay lazy without breaking Jint's property
  caches, and `Engine.PrepareScript` moves parsing and static analysis off the run path.

Constraint worth knowing before reaching for a newer BCL API: the library targets
`netstandard2.0`, `net462`, `net472`, `net8.0` and `net10.0`, and takes no dependency beyond
AngleSharp and Jint. `Span<T>`, `MemoryExtensions`, `ArrayPool<T>` and friends are therefore **not**
available unconditionally — they would need a `System.Memory` package reference or a
`#if NET8_0_OR_GREATER` guard, so weigh that against the actual gain. `LangVersion` is
`latest`, so modern C# *syntax* is always fine.

There is no benchmark project in the repository. Measure a claimed improvement with a
profiler on a realistic page (the jQuery and React fixtures are good workloads) rather than
asserting it, and say in the PR what moved.

## Code conventions

From `.github/CONTRIBUTING.md` and `.editorconfig` — these differ from typical modern C#:

- `using` directives go **inside** the namespace declaration.
- Framework type names, not keywords: `String`, `Int32`, `Boolean`, `Object`.
- Prefer `var` on the left-hand side wherever possible.
- Always use statement blocks; blank line between two non-simple statements.
- `ConfigureAwait(false)` on every `await`.
- 4 spaces, LF, UTF-8, trimmed trailing whitespace; 2 spaces in `*.csproj`.
- Older files use `#region Fields / ctor / Properties / Methods / Helpers`; match the file
  you are editing rather than converting it.
- Comments in the newer code explain *why* a non-obvious construct exists (prototype cycles,
  laziness, Jint quirks). Keep that density — do not narrate what the code already says.

## Tests

NUnit 3 (classic model: `Assert.AreEqual`), fixtures in `src/AngleSharp.Js.Tests`.

The idiomatic test asserts on script behaviour rather than on internals:

```csharp
var result = await "document.createElement('div') instanceof HTMLDivElement".EvalScriptAsync();
Assert.AreEqual("True", result);
```

- `String.EvalScriptAsync()` returns what `console.log(<expr>)` printed.
- `IEnumerable<String>.EvalScriptsAsync()` runs several scripts against one document and
  returns the innerHTML of `#result` — use it for DOM-mutation tests.
- `Helpers.GetCssConfig()` for tests needing CSS/render device.
- If a timing-sensitive behavior works in real browsers but fails in this test runtime,
  treat it as a bug candidate in `AngleSharp.Js` or upstream `AngleSharp` and investigate
  root cause. Do **not** weaken or rewrite the test just to match the current behavior.
- The main assembly grants `InternalsVisibleTo` to the test assembly, but reaching into
  internals is rare; prefer a script-level regression test.
- `IntegrationTests` and `PageTests` hit the network and report `Inconclusive` when it is
  unavailable. `JqueryTests` / `ReactTests` run real library sources embedded in
  `Constants.cs` — that file is ~750 KB of minified JS, never read it wholesale.

## Repository notes

These files are synced from the `AngleSharp.GitBase` repository and should not be edited
here: `.editorconfig`, `.gitignore`, `.gitattributes`, `.github/*`, `build.ps1`, `build.sh`,
`tools/*`, `LICENSE`.

CI (`.github/workflows/ci.yml`) builds on Linux and Windows; on Windows it selects the NUKE
target from the branch (`main` → `Publish`, `devel` → `PrePublish`, otherwise the default).
`CONTRIBUTING.md` asks for feature branches (`feature/#777`) and pull requests against
`devel`.
