<!--{"sort_order":2, "name": "create-your-own", "label": "Create your own"}-->
# Create a Page Component for WebVella Erp

To create a Page Component you need to add a ViewComponent to your project that has specific structure and requirements. 

## Page Component name

The page component should have an unique name withing the solution. This is why you should use a vendor prefix. We use "Pc" in all our components, so you can select any other combinations.

## Folder Structure

Page components usually reside in the `Components` folder of your project. There, they should have their own subfolder with a name matching the component name.

In the folder, the following files are expected:

<i class="fa fa-fw fa-file-code go-blue"></i> YourComponentName.cs<br/>
<i class="fa fa-fw fa-file-code go-blue"></i> Design.cshtml<br/>
<i class="fa fa-fw fa-file-code go-blue"></i> Display.cshtml<br/>
<i class="fa fa-fw fa-file-code go-blue"></i> Error.cshtml<br/>
<i class="fa fa-fw fa-file-code go-blue"></i> Help.cshtml<br/>
<i class="fa fa-fw fa-file-code go-blue"></i> Options.cshtml<br/>
<i class="fa fa-fw fa-file-code go-blue"></i> service.js

You can review more information about the files in the next sections

## YourComponentName.cs

This is the file that defines the view component and turns it into a page component. There are several key requirements for a view component to be recognized by the system as a page component:

#### Requirement 1: `PageComponent` class attribute

This attribute is used to define the component's meta, that will be used by the system, when presenting it to the end user. It has the following properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `Category`                    | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Under which category the component should be presented
+-------------------------------+-----------------------------------+
| `Color`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Icon's color code
+-------------------------------+-----------------------------------+
| `Description`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Short text describing the component
+-------------------------------+-----------------------------------+
| `IconClass`                   | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The CSS class for generating the icon using [FontAwesome icon library](https://fontawesome.com/icons).
+-------------------------------+-----------------------------------+
| `IsInline`                    | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Whether the design mode component presentation should be inline or block.
+-------------------------------+-----------------------------------+
| `Label`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Will be presented as the component's name to the end user
+-------------------------------+-----------------------------------+
| `Library`                     | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Part of which library or vendor name
+-------------------------------+-----------------------------------+
| `Tags`                        | *object type*: `List<string>`                         
|                               |         
|                               | *default value*: `new List<string>()`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Keywords that describe the component
+-------------------------------+-----------------------------------+
| `Version`                     | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The component's current version
+-------------------------------+-----------------------------------+

```csharp
[PageComponent(Label = "Chart", Library = "WebVella", Description = "Line,area,pie, doughnut, bar, horizontal bar", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
```

#### Requirement 2: Inherit the `PageComponent` class

This inheritance will provide ability to inject the necessary services.

```csharp
public class PcChart : PageComponent
```

#### Requirement 3: Add `ErpRequestContext` class property

This property will grant the component an access to the page and application context.

```csharp
protected ErpRequestContext ErpRequestContext { get; set; }
```

#### Requirement 4: Inject the `ErpRequestContext` in the components constructor

In order to initialize the ErpRequestContext property you need to inject it during the component's creation

```csharp
public PcChart([FromServices]ErpRequestContext coreReqCtx)
{
	ErpRequestContext = coreReqCtx;
}
```

#### Requirement 5: Inject the component instance context in the `InvokeAsync` method of your component

This variable will provide details about the component's instance, its settings, environment and requested render mode

```csharp
public override async Task<IViewComponentResult> InvokeAsync(PageComponentContext context)
```

#### Requirement 6: Define components options

Often a component needs to have its own options, that the user needs to set. Each instance of your component will store a json version of the component's options object in the database, which you will need to restore when needed.

The convention of the options model naming that we advise is `ComponentName + "Options"`. Here is a definition example:

```csharp
public class PcChartOptions
{
	[JsonProperty(PropertyName = "height")]
	public string Height { get; set; } = null;

	[JsonProperty(PropertyName = "width")]
	public string Width { get; set; } = null;
}
```

Here is how it can be later restored:

```csharp
options = JsonConvert.DeserializeObject<PcChartOptions>(context.Options.ToString());
```
#### Requirement 7: Implement the rendering modes

A component in WebVella ERP should support the following rendering modes as defined by the `enum ComponentMode`: Display, Design, Options, Help, with the addition of an Error view.

#### Requirement 8: Component namespace convention

In order for the system to be able to find any possible embedded resource, as `service.js`, the namespace should exactly correspond to the folder structure where the component is located.

### Complete sample file contents

```csharp
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Chart", Library = "WebVella", Description = "Line,area,pie, doughnut, bar, horizontal bar", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcChart : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcChart([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcChartOptions
		{
			[JsonProperty(PropertyName = "height")]
			public string Height { get; set; } = null;

			[JsonProperty(PropertyName = "width")]
			public string Width { get; set; } = null;
		}

		public override async Task<IViewComponentResult> InvokeAsync(PageComponentContext context)
		{
			ErpPage currentPage = null;
			try
			{
				#region << Init >>
				if (context.Node == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: The node Id is required to be set as query parameter 'nid', when requesting this component"));
				}

				var pageFromModel = context.DataModel.GetProperty("Page");
				if (pageFromModel == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel cannot be null"));
				}
				else if (pageFromModel is ErpPage)
				{
					currentPage = (ErpPage)pageFromModel;
				}
				else
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel does not have Page property or it is not from ErpPage Type"));
				}

				var options = new PcChartOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcChartOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = options;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;

				ViewBag.Height = options.Height;
				ViewBag.Width = options.Width;

				switch (context.Mode)
				{
					case ComponentMode.Display:
						return await Task.FromResult<IViewComponentResult>(View("Display"));
					case ComponentMode.Design:
						return await Task.FromResult<IViewComponentResult>(View("Design"));
					case ComponentMode.Options:
						return await Task.FromResult<IViewComponentResult>(View("Options"));
					case ComponentMode.Help:
						return await Task.FromResult<IViewComponentResult>(View("Help"));
					default:
						ViewBag.ExceptionMessage = "Unknown component mode";
						ViewBag.Errors = new List<ValidationError>();
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}
			}
			catch (ValidationException ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				ViewBag.Errors = new List<ValidationError>();
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				ViewBag.Errors = new List<ValidationError>();
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}

```

## Design.cshtml

This view is requested by your component when the "Design" mode is requested. It's purpose is the present the component in the PageBody Manager - the place where the user will arrange page components for a page. 

It should mimic the appearance of the "Display" mode, either by presenting live data or by showing just an example. Many times the component does not have anything to present or cannot present it, in this case a simple text describing what the user should expect in "Display" mode is necessary.

**Javascript**: You can initialize any javascript by implementing the events as presented in `service.js` file.

### Example without nesting 

```html
@addTagHelper *, WebVella.Erp.Plugins.Core
@addTagHelper *, WebVella.Erp.Web
@using WebVella.Erp.Web.Components;
@using WebVella.Erp.Web.Models;
@using WebVella.Erp.Web;
@{
	var options = (PcChart.PcChartOptions)ViewBag.Options;
	var node = (PageBodyNode)ViewBag.Node;
	var erpRequest = (ErpRequestContext)ViewBag.RequestContext;
	var componentContext = (PageComponentContext)ViewBag.ComponentContext;
	var height = (string)ViewBag.Height;
	var width = (string)ViewBag.Width;
}
<div class="p-1">
	<wv-chart height="@height" width="@width"></wv-chart>
</div>
```

### Example with nesting 

Sometimes you need to provide the option of other components to be nested in yours. This is achieved by defining a nesting container in your component's Design view. Each of such containers should have unique name within the component, so you can later get the components for each container.

```html
@addTagHelper *, WebVella.Erp.Plugins.Core
@addTagHelper *, WebVella.Erp.Web
@using WebVella.Erp.Web.Components;
@using WebVella.Erp.Web.Models;
@using WebVella.Erp.Web;
@{
	var options = (PcChart.PcChartOptions)ViewBag.Options;
	var node = (PageBodyNode)ViewBag.Node;
	var erpRequest = (ErpRequestContext)ViewBag.RequestContext;
	var componentContext = (PageComponentContext)ViewBag.ComponentContext;
	var height = (string)ViewBag.Height;
	var width = (string)ViewBag.Width;
}
<wv-section>
	<wv-pb-node-container parent-node-id="@node.Id" container-id="body"></wv-pb-node-container>
</wv-section>
```

## Display.cshtml

This view is presented when the component operates in its main purpose - displaying data.

**Javascript**: You can initialize any javascript by implementing the events as presented in `service.js` file.

### Example without nesting

```html
@addTagHelper *, WebVella.Erp.Plugins.Core
@addTagHelper *, WebVella.Erp.Web
@using WebVella.Erp.Web.Components;
@using WebVella.Erp.Web.Models;
@using WebVella.Erp.Web;
@{
	var options = (PcChart.PcChartOptions)ViewBag.Options;
	var node = (PageBodyNode)ViewBag.Node;
	var erpRequest = (ErpRequestContext)ViewBag.RequestContext;
	var componentContext = (PageComponentContext)ViewBag.ComponentContext;
	var height = (string)ViewBag.Height;
	var width = (string)ViewBag.Width;
}
<wv-chart height="@height" width="@width"></wv-chart>
```

### Example with nesting

```html
@addTagHelper *, WebVella.Erp.Plugins.Core
@addTagHelper *, WebVella.Erp.Web
@using WebVella.Erp.Web.Components;
@using WebVella.Erp.Web.Models;
@using WebVella.Erp.Web;
@{
	var options = (PcChart.PcChartOptions)ViewBag.Options;
	var node = (PageBodyNode)ViewBag.Node;
	var erpRequest = (ErpRequestContext)ViewBag.RequestContext;
	var componentContext = (PageComponentContext)ViewBag.ComponentContext;
	var height = (string)ViewBag.Height;
	var width = (string)ViewBag.Width;
}
<wv-section>
	@foreach (var childNode in node.Nodes)
	{
		var nodeComponentName = "";
		if (childNode != null)
		{
			var nameArray = childNode.ComponentName.Split('.');
			nodeComponentName = nameArray[nameArray.Length - 1];
		}
		if (!String.IsNullOrWhiteSpace(nodeComponentName))
		{
			var childOptions = PageUtils.ConvertStringToJObject(childNode.Options.ToString());
			var pcContext = new PageComponentContext(childNode, componentContext.DataModel, ComponentMode.Display, childOptions, componentContext.Items);
			@await Component.InvokeAsync(nodeComponentName, new { context = pcContext })
		}
	}
</wv-section>
```

## Options.cshtml

This view is presented when in during the PageBody management, the user selects your component and presses the "Options" button. It is rendered within a `<form/>` automatically generated by the system. When the "Save" button is pressed, this form is automatically posted to the server with an AJAX call.

As the saving is automatically done by the system, you need to follow a rule when creating the options form contents:

**Important**: The field names should match exactly the JsonProperty Names of the option.

**Javascript**: You can initialize any javascript by implementing the events as presented in `service.js` file.

```html
@addTagHelper *, WebVella.Erp.Plugins.Core
@addTagHelper *, WebVella.Erp.Web
@using WebVella.Erp.Web.Components;
@using WebVella.Erp.Web.Models;
@using WebVella.Erp.Web;
@{
	var options = (PcChart.PcChartOptions)ViewBag.Options;
	var node = (PageBodyNode)ViewBag.Node;
	var erpRequest = (ErpRequestContext)ViewBag.RequestContext;
	var componentContext = (PageComponentContext)ViewBag.ComponentContext;
	var height = (string)ViewBag.Height;
	var width = (string)ViewBag.Width;
}
<wv-row>
	<wv-column span="4">
		<wv-field-text name="width" value="@options.Width" label-text="Width style value"></wv-field-text>
	</wv-column>
	<wv-column span="4">
		<wv-field-text name="height" value="@options.Height" label-text="Height style value"></wv-field-text>
	</wv-column>
</wv-row>
```

## Help.cshtml

This view is presented when in during the PageBody management, the user selects your component and presses the "Help" button. It is meant to have some helping text how your component should be setup or used.

```html
<div>No component documentation at this time</div>
```

## Error.cshtml

Utility view for rendering any errors that may occur

```html
<div>No component documentation at this time</div<div class="go-red p-3"><i class="fas fa-exclamation-circle"></i> @ViewBag.ExceptionMessage</div>
```

## service.js

This file will be automatically included and executed by the system during PageBody management. There are several rules in order for it to work correctly:

**Rule 1:** It should be set as `embedded resource`

**Rule 2:** It should work with the predefined events that are emitted by the system, based on the component name

**Rule 3:** In order for it to be discovered, your component's namespace should exactly correspond to the folder structure of the component

```javascript
"use strict";
(function (window, $) {

	/// Your code goes below
	///////////////////////////////////////////////////////////////////////////////////

	$(function () {
		document.addEventListener("WvPbManager_Design_Loaded", function (event) {
			if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcChart"){
				console.log("WebVella.Erp.Web.Components.PcChart Design loaded");
			}
		});
	});

	$(function () {
		document.addEventListener("WvPbManager_Design_Unloaded", function (event) {
			if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcChart"){
				console.log("WebVella.Erp.Web.Components.PcChart Design unloaded");
			}
		});
	});


	$(function () {
		document.addEventListener("WvPbManager_Options_Loaded", function (event) {
			if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcChart") {
				console.log("WebVella.Erp.Web.Components.PcChart Options loaded");
			}
		});
	});

	$(function () {
		document.addEventListener("WvPbManager_Options_Unloaded", function (event) {
			if (event && event.payload && event.payload.component_name === "WebVella.Erp.Web.Components.PcChart"){
				console.log("WebVella.Erp.Web.Components.PcChart Options unloaded");
			}
		});
	});

	//////////////////////////////////////////////////////////////////////////////////
	/// You code is above

})(window, jQuery);
```