<!--{"sort_order":10, "name": "wv-page-header", "label": "wv-page-header"}-->
# wv-page-header

## Purpose

`<wv-page-header/>`. Generates the standard page header element. Used in conjunction with `<wv-page-header-actions/>` and `<wv-page-header-toolbar/>`

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `area-label`                  | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Area label string
+-------------------------------+-----------------------------------+
| `area-sublabel`               | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Area sublabel string
+-------------------------------+-----------------------------------+
| `color`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Color code that should be used in page header's area text
+-------------------------------+-----------------------------------+
| `description`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Page description
+-------------------------------+-----------------------------------+
| `icon-class`                  | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Icon class with which to generate the page icon using [FontAwesome icon library](https://fontawesome.com/icons)
+-------------------------------+-----------------------------------+
| `icon-color`                  | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Color code that should be used in page header's icon
+-------------------------------+-----------------------------------+
| `page-switch-items`           | *object type*: `List<PageSwitchItem>`                         
|                               |         
|                               | *default value*: `new List<PageSwitchItem>()`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Page switch dropdown, meant to present other pages on the same level or different list views.
+-------------------------------+-----------------------------------+
| `return-url`                  | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If set will present a "back button" in the left side of the element
+-------------------------------+-----------------------------------+
| `subtitle`                    | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Page subtitle
+-------------------------------+-----------------------------------+
| `title`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Page title
+-------------------------------+-----------------------------------+


## Example

```html
<wv-page-header color="@color" icon-color="@iconColor" area-label="@areaLabel" area-sublabel="@areaSubLabel" title="@title"
	subtitle="@subTitle" description="@description" icon-class="@iconClass" return-url="@returnUrl" page-switch-items="@pageSwitchItems">
	<wv-page-header-actions>
		...
	</wv-page-header-actions>
	<wv-page-header-toolbar>
		...
	</wv-page-header-toolbar>
</wv-page-header>
```

