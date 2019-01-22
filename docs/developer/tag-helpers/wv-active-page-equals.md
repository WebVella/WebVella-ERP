<!--{"sort_order":10, "name": "wv-active-page-equals", "label": "wv-active-page-equals"}-->
# wv-active-page-equals

## Purpose

This tag helper sets an `active` class to the element, if the current `ViewContext.RouteData.Values["page"].ToString().ToLowerInvariant()`, regexed equals the string from `asp-page` attribute(trimmed, lowercased). If no `asp-page` attribute present, `href` is similarly checked.

## Properties

+-----------------------------------+-----------------------------------+
| name                              | description                       |
+===================================+===================================+
|`wv-active-page-equals`            | *html target*: `attribute`        
|                                   |         
|                                   | *object type*: `has no value`
|                                   |         
|                                   | *default value*: `none`   
|                                   |
|                                   | *is required*: `TRUE`                      
|                                   |                                   
|                                   | Just the attribute is required. It has no value needed.
+-----------------------------------+-----------------------------------+
|`asp-page or href`                 | *html target*: `attribute`        
|                                   |         
|                                   | *object type*: `string`                               
|                                   |         
|                                   | *default value*: `String.Empty`                     
|                                   |
|                                   | *is required*: `TRUE`                      
|                                   |                                   
|                                   | This attribute is required to be present. If not <code>active</code> class will not be assigned.
+-----------------------------------+-----------------------------------+

## Example

```html
<a wv-active-page-equals asp-page='/dev/base-plugin/api/index'>Api Index Page</a>
<a wv-active-page-equals href='/dev/base-plugin/api/index'>Api Index Page</a>
```

