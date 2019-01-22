<!--{"sort_order":10, "name": "wv-field-number", "label": "wv-field-number"}-->
# wv-field-number

## Purpose

`<wv-field-number/>`. Provides the ability to render the number field type of an Erp Entity. Can be used to render other number based form values.

## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `decimal-digits`              | *object type*: `int`                         
|                               |         
|                               | *default value*: `2`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | On how many decimal digits the rounding should be done when displaying the value
+-------------------------------+-----------------------------------+
| `max`                         | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If present, will be set as a max attribute of the field's input
+-------------------------------+-----------------------------------+
| `min`                         | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If present, will be set as a min attribute of the field's input
+-------------------------------+-----------------------------------+
| `step`                        | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If present, will be set as a step attribute of the field's input
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-number value="@value"></wv-field-number>
```

