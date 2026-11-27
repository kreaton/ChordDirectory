# Third-party notices

**Chord Directory (ChordDirectory)** is © 2026 Per Hejndorf and is licensed under the [MIT License](LICENSE), except where stated below. This file lists the third-party components and assets it uses, each under its own licence. Licence texts and copyright notices are kept by the upstream projects at the *Source* links; where a licence requires its notice to travel with binaries, the upstream package already carries it.

_Last reviewed: 2026-11-26. Versions are the ones resolved in this repository at that date; Dependabot keeps them current, so check the project files for the exact version of a given release._

## Libraries included in the application

| Library | Version | Licence | Source | Modified |
|---|---|---|---|---|
| .NET runtime and Blazor WebAssembly (shipped in `_framework/`) | 10.0 | MIT | [dotnet/runtime](https://github.com/dotnet/runtime), [dotnet/aspnetcore](https://github.com/dotnet/aspnetcore) | No |
| Microsoft.AspNetCore.Components.WebAssembly | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly/10.0.15) | No |

<details>
<summary>Indirect (transitive) dependencies — 26 packages</summary>

These packages are pulled in by the libraries above and are shipped with the application. All are used unmodified.

| Library | Version | Licence | Source | Modified |
|---|---|---|---|---|
| Microsoft.AspNetCore.App.Internal.Assets | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.App.Internal.Assets/10.0.15) | No |
| Microsoft.AspNetCore.Authorization | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Authorization/10.0.15) | No |
| Microsoft.AspNetCore.Components | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Components/10.0.15) | No |
| Microsoft.AspNetCore.Components.Forms | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.Forms/10.0.15) | No |
| Microsoft.AspNetCore.Components.Web | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.Web/10.0.15) | No |
| Microsoft.AspNetCore.Metadata | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Metadata/10.0.15) | No |
| Microsoft.Extensions.Configuration | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/10.0.15) | No |
| Microsoft.Extensions.Configuration.Abstractions | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Abstractions/10.0.15) | No |
| Microsoft.Extensions.Configuration.Binder | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder/10.0.15) | No |
| Microsoft.Extensions.Configuration.FileExtensions | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.FileExtensions/10.0.15) | No |
| Microsoft.Extensions.Configuration.Json | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/10.0.15) | No |
| Microsoft.Extensions.DependencyInjection | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/10.0.15) | No |
| Microsoft.Extensions.DependencyInjection.Abstractions | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/10.0.15) | No |
| Microsoft.Extensions.Diagnostics | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics/10.0.15) | No |
| Microsoft.Extensions.Diagnostics.Abstractions | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Diagnostics.Abstractions/10.0.15) | No |
| Microsoft.Extensions.FileProviders.Abstractions | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.FileProviders.Abstractions/10.0.15) | No |
| Microsoft.Extensions.FileProviders.Physical | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.FileProviders.Physical/10.0.15) | No |
| Microsoft.Extensions.FileSystemGlobbing | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.FileSystemGlobbing/10.0.15) | No |
| Microsoft.Extensions.Logging | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Logging/10.0.15) | No |
| Microsoft.Extensions.Logging.Abstractions | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/10.0.15) | No |
| Microsoft.Extensions.Options | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Options/10.0.15) | No |
| Microsoft.Extensions.Options.ConfigurationExtensions | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Options.ConfigurationExtensions/10.0.15) | No |
| Microsoft.Extensions.Primitives | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Primitives/10.0.15) | No |
| Microsoft.Extensions.Validation | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Validation/10.0.15) | No |
| Microsoft.JSInterop | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.JSInterop/10.0.15) | No |
| Microsoft.JSInterop.WebAssembly | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.JSInterop.WebAssembly/10.0.15) | No |

</details>

## Build, test and tooling (not distributed)

Used to build, test or render promotional material only; not part of the shipped application. The last three are referenced automatically by the .NET SDK. The test project (`tests/ChordDirectory.Tests`) uses no packages.

| Library | Version | Licence | Source | Modified |
|---|---|---|---|---|
| Microsoft.AspNetCore.Components.WebAssembly.DevServer | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.DevServer/10.0.15) | No |
| Microsoft.AspNetCore.Components.Analyzers | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.Analyzers/10.0.15) | No |
| Microsoft.NET.ILLink.Tasks | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.NET.ILLink.Tasks/10.0.15) | No |
| Microsoft.NET.Sdk.WebAssembly.Pack | 10.0.15 | MIT | [NuGet](https://www.nuget.org/packages/Microsoft.NET.Sdk.WebAssembly.Pack/10.0.15) | No |

## Icons, images, audio and video

| Asset(s) | Origin | Licence |
|---|---|---|
| `art/icon.svg`, `src/ChordDirectory/wwwroot/icons/` (8 files: .png, rendered from `art/icon.svg` by `tools/make-icons.ps1`) | Drawn by the author as plain SVG shapes (piano keys and dots, written with an AI coding agent), not made with an image-generation tool. No fonts, emoji or third-party artwork are used | MIT (as part of this project) |
| `images/banner.png` | Screenshot of the app, taken by `tools/make-banner.ps1` | MIT (as part of this project) |
| Piano keyboard, key labels and chord highlights in the app | Drawn as SVG by the app's own code (`Components/PianoKeyboard.razor`) | MIT (as part of this project) |
| Instrument sounds (piano, electric piano, organ, synth pad) and reverb | Synthesised at run time with the browser's Web Audio API (`wwwroot/js/audio.js`); no audio files or samples are shipped | — |
| Symbols in the interface (♪ ♯ ♭ ▶ ■ and others) | Not shipped: drawn at run time by the fonts installed on the user's device | — |

No web fonts, data files or online services are used: the chord catalogue is the project's own, and the app makes no network requests beyond loading itself.

## Trademarks

Microsoft, Windows, .NET, Blazor and other Microsoft product names are trademarks of the Microsoft group of companies. All other product names, logos and brands mentioned in this repository are the property of their respective owners. They are used only to identify the services and tools this project works with, and their use does not imply endorsement.
