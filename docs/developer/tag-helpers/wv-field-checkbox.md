<!--{"sort_order":10, "name": "wv-field-checkbox", "label": "wv-field-checkbox"}-->
# wv-field-checkbox

## Purpose

`<wv-field-checkbox/>`. Provides the ability to render the checkbox field type of an Erp Entity. Can be used to render other boolean based form values.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `text-true`                   | *object type*: `string`                         
|                               |         
|                               | *default value*: `selected`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The text presented as checkbox label in forms and also as checkbox value in Simple mode, when checked.
+-------------------------------+-----------------------------------+
| `text-false`                  | *object type*: `string`                         
|                               |         
|                               | *default value*: `not selected`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The text presented as checkbox value in Simple mode, when checked.
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-checkbox value="@value" text-true="is for sale"></wv-field-checkbox>
```

