<!--{"sort_order":10, "name": "wv-field-currency", "label": "wv-field-currency"}-->
# wv-field-currency

## Purpose

`<wv-field-currency/>`. Provides the ability to render the currency field type of an Erp Entity. Can be used to render other currency based form values.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `currency-code`               | *object type*: `string`                         
|                               |         
|                               | *default value*: `USD`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Localization settings and display is generated based on the selected Currency Code. 
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
<wv-field-currency value="@value" currency-code="GBP"></wv-field-currency>
```

