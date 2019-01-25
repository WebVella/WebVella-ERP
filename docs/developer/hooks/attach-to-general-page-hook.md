<!--{"sort_order":3, "name": "attach-to-general-page-hook", "label": "Attach to a General Page Hook"}-->
# Attach to a WebVella ERP General Page Hook

To create an "General Page Hook" attachment, you need to create a class file in your plugin's Hooks folder. In order for the system to reference a hook attachment, it needs to fulfill several requirements.

## Requirements

#### Requirement 1: `HookAttachment` class attribute

You need to decorate the class as an API hook by prepending an attribute. It has two properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `key`                         | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The name of a custom hook handler -> if the page has a queryParam `hookKey`, only hooks that match it with their `key` will be executed
+-------------------------------+-----------------------------------+
| `priority`                    | *object type*: `int`                         
|                               |         
|                               | *default value*: `10`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The plugin order priority. More executes first.
+-------------------------------+-----------------------------------+

```csharp
[HookAttachment]
```

#### Requirement 2: Inherit the `IPageHook` interface

```csharp
public class AllPagesHook : IPageHook
```

#### Requirement 3: Implement the interface methods

There are two methods that need to be implemented. Checkout the example that follows for more info

## Full Example

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