<!--{"sort_order":10, "name": "wv-active-page-regex", "label": "wv-active-page-regex"}-->
# wv-active-page-regex

## Purpose

This tag helper sets an `active` class to the element, if the current page path (trimmed and lowercased) matches the provided regex pattern. If no active page present, `href` is similarly checked.

## Properties

+-----------------------------------+-----------------------------------+
| name                              | description                       |
+===================================+===================================+
|`wv-active-page-regex`             | *html target*: `attribute`        
|                                   |         
|                                   | *object type*: `Regex pattern`
|                                   |         
|                                   | *default value*: `none`                    
|                                   |
|                                   | *is required*: `TRUE`                      
|                                   |                                   
|                                   | A valid regex pattern to be matched.
+-----------------------------------+-----------------------------------+


## Example

```html
<a wv-active-page-regex='/dev/base-plugin/api/index'>Api Index Page</a>
```

