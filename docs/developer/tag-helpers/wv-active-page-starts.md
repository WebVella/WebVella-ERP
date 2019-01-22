<!--{"sort_order":10, "name": "wv-active-page-starts", "label": "wv-active-page-starts"}-->
# wv-active-page-starts

## Purpose

This tag helper sets an <code>active</code> class to the element in the following case:

1. if the current `ViewContext.RouteData.Values["page"].ToString().ToLowerInvariant()`, regexed starts with the string from `asp-page` attribute(trimmed, lowercased and page name removed);
2. if the current `ViewContext.HttpContext.Request.Path.ToString().ToLowerInvariant()`, regexed starts with the string from `href` attribute(trimmed, lowercased and last node removed)

## Properties

+-----------------------------------+-----------------------------------+
| name                              | description                       |
+===================================+===================================+
|`wv-active-page-starts`            | *html target*: `attribute`        
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
<a wv-active-page-starts asp-page='/dev/base-plugin/api/index'>Api Index Page</a>
<a wv-active-page-starts href='/dev/index'>Api Index Page</a>
```

