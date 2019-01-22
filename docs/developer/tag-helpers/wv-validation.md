<!--{"sort_order":10, "name": "wv-validation", "label": "wv-validation"}-->
# wv-validation

## Purpose

`<wv-validation/>`. Used when a validation message is needed outside of a the `<wv-form/>` element

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `show-errors`                 | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Whether to show a list of specific errors in the validation element
+-------------------------------+-----------------------------------+
| `validation`                  | *object type*: `ValidationException`                         
|                               |         
|                               | *default value*: `new ValidationException()`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The validation details to present
+-------------------------------+-----------------------------------+

## Example

```html
<wv-validation show-errors="false" validation="@validation"></wv-validation>
```

