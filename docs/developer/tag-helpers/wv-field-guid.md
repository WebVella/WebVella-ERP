<!--{"sort_order":10, "name": "wv-field-guid", "label": "wv-field-guid"}-->
# wv-field-guid

## Purpose

`<wv-field-guid/>`. Provides the ability to render the guid field type of an Erp Entity. Can be used to render other guid based form values.

## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `value`                       | *object type*: `GUID or string`                         
|                               |         
|                               | *default value*: `null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | string value will be parsed to GUID
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-guid value="@value"></wv-field-guid>
```

