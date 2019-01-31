<!--{"sort_order":2, "name": "attach-to-api-hook", "label": "Attach to an API hook"}-->
# Attach to a WebVella ERP API Hook

To create an API Hook attachment, you need to create a class file in your plugin's Hooks folder. In order for the system to reference your hook attachment, it needs to fulfill several requirements.

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
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | The entity name that is targeted by the attribute
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
[HookAttachment("task")]
```

#### Requirement 2: Inherit the interfaces that you need

This hook can inherit the following interfaces: IErpPreCreateRecordHook, IErpPreUpdateRecordHook, IErpPreDeleteRecordHook, IErpPostCreateRecordHook, IErpPostUpdateRecordHook, IErpPostDeleteRecordHook

*Note*: All API hooks are executed in a transaction with the main operation and other hooks. If one fails, the whole transaction will be reversed.

```csharp
public class TaskHook : IErpPreCreateRecordHook, IErpPreUpdateRecordHook,IErpPreDeleteRecordHook,IErpPostCreateRecordHook,IErpPostUpdateRecordHook,IErpPostDeleteRecordHook
```

#### Requirement 3: Implement the interfaces methods

```csharp
public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
{
}
```

## Full Example

```csharp
using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Next.Services;

namespace WebVella.Erp.Plugins.Next.Hooks.Api
{
	[HookAttachment("task")]
	public class TaskHook : IErpPreCreateRecordHook, IErpPreUpdateRecordHook, IErpPreDeleteRecordHook,
							IErpPostCreateRecordHook, IErpPostUpdateRecordHook, IErpPostDeleteRecordHook
	{

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
		}

		public void OnPostDeleteRecord(string entityName, EntityRecord record)
		{
		}

	}
}

```