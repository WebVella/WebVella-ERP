# Third-Party Libraries

This file lists the third-party libraries used by the project. All libraries keep their own
license. The project itself is Apache-2.0.

## .NET packages (NuGet)

### Core

| Library | Version | Used for |
|---|---|---|
| Npgsql | 9.0.4 | PostgreSQL data provider |
| Newtonsoft.Json | 13.0.4 | JSON serialization |
| AutoMapper | 14.0.0 | Object-to-object mapping |
| CsvHelper | 33.1.0 | CSV import and export |
| Ical.Net | 5.2.3 | Calendar / ICS handling |
| Irony.NetCore | 1.1.11 | Parser generator (used by the EQL engine) |
| MimeMapping | 4.0.0 | MIME type mapping |
| Storage.Net | 9.3.0 | File storage abstraction |
| System.Drawing.Common | 10.0.10 | Image handling |

### Web and rendering

| Library | Version | Used for |
|---|---|---|
| Microsoft.CodeAnalysis.CSharp (+ Common, .Scripting, .Workspaces) | 5.3.0 | Roslyn: dynamic C# compilation for code hooks |
| CS-Script | 4.14.9 | C# scripting engine |
| HtmlAgilityPack | 1.12.4 | HTML parsing |
| WebVella.TagHelpers | 1.8.2 | The WebVella tag-helper library |
| Wangkanai.Detection | 8.20.0 | Device and browser detection |
| Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation | 10.0.8 | Runtime Razor compilation |
| System.IdentityModel.Tokens.Jwt | 8.18.0 | JWT creation and validation |
| Microsoft.Extensions.FileProviders.Embedded | 10.0.8 | Embedded resource file provider |

### Host and sites

| Library | Version | Used for |
|---|---|---|
| Microsoft.AspNetCore.Mvc.NewtonsoftJson | 10.0.8 | Newtonsoft.Json integration for MVC |
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.8 | JWT bearer authentication |
| Microsoft.Web.LibraryManager.Build | 3.0.71 | Client-side library restore at build |
| morelinq | 4.4.0 | Extra LINQ operators |

### Blazor WebAssembly client

| Library | Version | Used for |
|---|---|---|
| Microsoft.AspNetCore.Components.WebAssembly | 10.0.8 | Blazor WebAssembly framework |
| Microsoft.AspNetCore.Components.WebAssembly.Authentication | 10.0.8 | Client-side authentication |
| Microsoft.Extensions.Http | 10.0.8 | HttpClient factory |
| Blazored.LocalStorage | 4.5.0 | Browser local storage |

### Plugins

| Library | Version | Used for |
|---|---|---|
| MailKit | 4.16.0 | Email (Mail plugin) |

## Client-side (browser) libraries

The admin UI loads its browser libraries through LibraryManager
(`Microsoft.Web.LibraryManager.Build`) into `wwwroot` at build time. These are not NuGet
packages. They include the front-end bundle, Bootstrap, Angular, CKEditor, moment.js and
Chart.js, among others.

## Related repositories

The platform also depends on these separate repositories:

- [WebVella-TagHelpers](https://github.com/WebVella/TagHelpers) — the Razor tag-helper library,
  consumed as the NuGet package `WebVella.TagHelpers`.
- [WebVella-ERP-StencilJs](https://github.com/WebVella/WebVella-ERP-StencilJs) — the StencilJS
  web components (`wv-*`), consumed as prebuilt JS bundles under each plugin's `wwwroot/js/`.
- [WebVella-ERP-Seed](https://github.com/WebVella/WebVella-ERP-Seed) — a starter seed project.
