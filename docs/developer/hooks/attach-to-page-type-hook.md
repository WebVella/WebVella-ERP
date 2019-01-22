<!--{"sort_order":4, "name": "attach-to-page-type-hook", "label": "Attach to a Page-Type Hook"}-->
# Attach to a Page-Type Hook

To create an "Page-Type Hook" attachment, you need to create a class file in your plugin's Hooks folder. In order for the system to reference a hook attachment, it needs to fulfill several requirements.

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
|                               | If you need to target a custom hook handler, here you need to provide its name
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

#### Requirement 2: Inherit the proper interface of the hook based on the targeted page type

Available options are: ILoginPageHook, ILogoutPageHook, IHomePageHook, ISitePageHook, IApplicationHomePageHook, IApplicationNodePageHook, IRecordCreatePageHook, IRecordDetailsPageHook, 
IRecordListPageHook, IRecordManagePageHook, IRecordRelatedRecordCreatePageHook, IRecordRelatedRecordDetailsPageHook, IRecordRelatedRecordsListPageHook, IRecordRelatedRecordManagePageHook

```csharp
public class SomePageHook : ISitePageHook
```

#### Requirement 3: Implement the interface methods

You need to check what methods the hook interface provides and implement them.

## Example
This is an example of attaching to a site page type hook

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