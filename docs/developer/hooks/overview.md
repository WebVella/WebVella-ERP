<!--{"sort_order":1, "name": "overview", "label": "Overview and Types"}-->
# What is a WebVella ERP Hook

Hooks' purpose is to achieve better modularity in the system, by providing a managed way for an outside code to be executed, each time a specific system process is run. There are several types of hooks that you can attach to.
Each of them has a specific requirement to be discovered and referenced by the system.

Hooks are three general type - API Hooks, Page Hooks and Render Hooks

## API Hooks

API Hooks are triggered when the system creates, updates or deletes an entity record by using its. Each hook can target only one entity (for performance reasons) and implements different
methods for the different stages of the create, update or delete procedure.
Hooks attachments have two attributes: key (targeted entity name) and priority(more will be executed first). Attachments to an API hook is not expected to return result. 
They alter the process by changing or not the referenced parameters. The available positions to hook are:

1. Record Creation
	- PreCreate - used mainly for extending validation checks. Executed within the process database transaction.
	- PostCreate - used mainly for follow up alerts or notifications. Executed outside the process database transaction.
2. Record Update
	- PreUpdate - used mainly for extending validation checks. Executed within the process database transaction.
	- PostUpdate - used mainly for follow up alerts or notifications. Executed outside the process database transaction.
3. Record Delete
	- PreDelete - used mainly for extending validation checks. Executed within the process database transaction.
	- PostDelete - used mainly for follow up alerts or notifications. Executed outside the process database transaction.

The available parameters for each of the positions differs.

- Pre Positions - available REFERENCED parameters are `(string entityName, EntityRecord data, List<ErrorModel> errors)`
- POST Positions - available REFERENCED parameters are `(string entityName, EntityRecord data)`

**Important:** The hook attachment needs to implements all of the attached interfaces' methods.

**Important 2:** Sometimes to prevent a loop you need to initialize the API with `new RecordManager(executeHooks: false)`

If there are several hook attachments targeting the same entity and position they will be executed by taking into consideration their priority attribute. 
As the parameters are references, they will be also shared within all the hook attachments. In this way, the end result is a compound of all validation errors in example, not only the ones from the last attachment.

Example API Hook attachment is:

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;

namespace WebVella.Erp.Plugins.SDK.Hooks
{
	[HookAttachment("user")]
	public class UserHook : IErpPreCreateRecordHook, IErpPreUpdateRecordHook,IErpPreDeleteRecordHook,	
							IErpPostCreateRecordHook,IErpPostUpdateRecordHook,IErpPostDeleteRecordHook
	{
		public void OnPreCreateRecord(string entityName, EntityRecord data, List<ErrorModel> errors)
		{
			Debug.WriteLine("Test pre-create executed");
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord data, List<ErrorModel> errors)
		{
			Debug.WriteLine("Test pre-update executed");
		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			Debug.WriteLine("Test pre-delete executed");
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			Debug.WriteLine("Test post-create executed");
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			Debug.WriteLine("Test post-update executed");
		}

		public void OnPostDeleteRecord(string entityName, EntityRecord record)
		{
			Debug.WriteLine("Test post-delete executed");
		}

	}
}

```

## Page Hooks

Page hooks provide a way for plugin code to be executed during the page model generation. This can either complements the normal execution process or bypass it. 
There is also an ability to GET or POST the page with a custom hook handler key, which will execute only the page hook attachments that are specifically targeting the said page and the said hook key.
A result is expected to be returned by page hook attachments. It can be either `IActionResult` or `null`. If the result is different than `null`, the process will be interrupted and the attachment result returned.

Each hook attachment, via its `HookAttachment` attribute has two properties - key (the name of the custom hook handler), priority (more will be executed first).

### General Page Hook

This hook is provided by the `IPageHook` interface and has two methods `OnGet` and `OnPost`. Both receive `BaseErpPageModel pageModel` as a parameter. All pages provided by the base platform implement this hook.

**Important**: The general Page Hook attachments is executed before any page type hooks.

#### Example without custom hook handler

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment]
	public class AllPagesHook : IPageHook
	{
		public IActionResult OnGet(BaseErpPageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(BaseErpPageModel pageModel)
		{
			return null;
		}
	}
}
```
#### Example with custom hook handler

The custom hook handler will be defined by requesting the page with a query parameter `hookKey`. If this is the case, the page will execute only hooks that have this key set in their `HookAttachment` attribute.

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment(key:"webvella-sdk")]  // <<<<< The key is the name of the custom hook handler
	public class AllPagesHook : IPageHook
	{
		public IActionResult OnGet(BaseErpPageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(BaseErpPageModel pageModel)
		{
			return null;
		}
	}
}
```

### Page Type Hooks

These hooks target only pages from a specific type, and have different positions based on the page type. They are as follows:

#### ILoginPageHook

Targets the `login` page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : ILoginPageHook
	{
		public IActionResult OnPostPreLogin(LoginModel pageModel)
		{
			return null;
		}

		public IActionResult OnPostAfterLogin(ErpUser user, LoginModel pageModel)
		{
			return null;
		}
	}
}
```
#### ILogoutPageHook

Targets the `logout` page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : ILogoutPageHook
	{
		public IActionResult OnGet(LogoutModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(LogoutModel pageModel)
		{
			return null;
		}
	}
}
```

#### IHomePageHook

Targets the `home` page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IHomePageHook
	{
		public IActionResult OnGet(HomePageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(HomePageModel pageModel)
		{
			return null;
		}
	}
}
```

#### ISitePageHook

Targets a `site` type page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : ISitePageHook
	{
		public IActionResult OnGet(SitePageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(SitePageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IApplicationHomePageHook

Targets a `application` type page (not attached to a sitemap node) with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IApplicationHomePageHook
	{
		public IActionResult OnGet(ApplicationHomePageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(ApplicationHomePageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IApplicationNodePageHook

Targets a `application` type page (attached to a sitemap node) with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IApplicationNodePageHook
	{
		public IActionResult OnGet(ApplicationNodePageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(ApplicationNodePageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordCreatePageHook

Targets a `record create` type page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordCreatePageHook
	{
		public IActionResult OnPreCreateRecord(EntityRecord record, Entity entity, RecordCreatePageModel pageModel, List<ValidationError> validationErrors )
		{
			return null;
		}

		public IActionResult OnPostCreateRecord(EntityRecord record, Entity entity, RecordCreatePageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordDetailsPageHook

Targets a `record details` type page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordDetailsPageHook
	{
		public IActionResult OnPost(RecordDetailsPageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordListPageHook

Targets a `record list` type page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordListPageHook
	{
		public IActionResult OnGet(RecordListPageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(RecordListPageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordManagePageHook

Targets a `record manage` type page with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordManagePageHook
	{
		public IActionResult OnPreManageRecord(EntityRecord record, Entity entity, RecordManagePageModel pageModel, List<ValidationError> validationErrors )
		{
			return null;
		}

		public IActionResult OnPostManageRecord(EntityRecord record, Entity entity, RecordManagePageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordRelatedRecordCreatePageHook

Targets a `record create` type page when in relation with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordRelatedRecordCreatePageHook
	{
		public IActionResult OnPreCreateRecord(EntityRecord record, Entity entity, RecordRelatedRecordCreatePageModel pageModel, List<ValidationError> validationErrors )
		{
			return null;
		}

		public IActionResult OnPostCreateRecord(EntityRecord record, Entity entity, RecordRelatedRecordCreatePageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordRelatedRecordDetailsPageHook

Targets a `record details` type page when in relation with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordRelatedRecordDetailsPageHook
	{
		public IActionResult OnPost(RecordRelatedRecordDetailsPageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordRelatedRecordsListPageHook

Targets a `record list` type page when in relation with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordRelatedRecordsListPageHook
	{
		public IActionResult OnGet(RecordRelatedRecordsListPageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(RecordRelatedRecordsListPageModel pageModel)
		{
			return null;
		}
	}
}
```

#### IRecordRelatedRecordManagePageHook

Targets a `record manage` type page when in relation with the following positions:

```csharp
using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment] // <<<<< Add key parameter to target a custom hook handler
	public class SomePageHook : IRecordRelatedRecordManagePageHook
	{
		public IActionResult OnPreManageRecord(EntityRecord record, Entity entity, RecordRelatedRecordManagePageModel pageModel, List<ValidationError> validationErrors )
		{
			return null;
		}

		public IActionResult OnPostManageRecord(EntityRecord record, Entity entity, RecordRelatedRecordManagePageModel pageModel)
		{
			return null;
		}
	}
}
```

## Render Hooks

Render hooks are rather different then the previous ones. They provide the ability to attach but only during the page view rendering process and are implemented similarly to a view component. 
Unlike the rest, such hooks are easily created by everyone by including the following code in your page view:

```html
<vc:render-hook placeholder="head-top" page-model="@Model"  model="null"></vc:render-hook>
```

To attach to such a hook you need to create your own ViewComponent and decorate it's class with the following attribute:

```csharp
[RenderHookAttachment("head-top", 10)]
	public class HeadTopIncludes : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel, dynamic model, string placeholder)
        {
		}
	}
```

IMPORTANT: In our application master page there are already render hooks included for: `head-top`, `head-bottom`,`body-toop`, `body-bottom`.