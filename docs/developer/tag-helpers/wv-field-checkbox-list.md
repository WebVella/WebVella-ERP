<!--{"sort_order":10, "name": "wv-field-checkbox-list", "label": "wv-field-checkbox-list"}-->
# wv-field-checkbox-list

## Purpose

`<wv-field-checkbox-list/>`. Provides the ability to render the checkbox field type of an Erp Entity. Can be used to render other boolean based form values.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `options`                     | *object type*: `List<SelectOption>`                         
|                               |         
|                               | *default value*: `new List<SelectOption>()`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The text presented as checkbox label in forms and also as checkbox value in Simple mode, when checked.
+-------------------------------+-----------------------------------+
| `value`                       | *object type*: `dynamic`                         
|                               |         
|                               | *default value*: `null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Expects the value to be parsed as `List<string>` or `List<SelectOption>`
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-checkbox-list value="@value" options="@options"></wv-field-checkbox-list>
```

