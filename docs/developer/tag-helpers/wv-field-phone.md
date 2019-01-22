<!--{"sort_order":10, "name": "wv-field-phone", "label": "wv-field-phone"}-->
# wv-field-phone

## Purpose

`<wv-field-phone/>`. Provides the ability to render the phone field type of an Erp Entity. Can be used to render other phone based form values.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `maxlength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If present, will be set as a maxlength attribute of the field's input
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-phone value="@value"></wv-field-phone>
```

