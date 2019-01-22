<!--{"sort_order":10, "name": "wv-field-percent", "label": "wv-field-percent"}-->
# wv-field-percent

## Purpose

`<wv-field-percent/>`. Provides the ability to render the percent field type of an Erp Entity. Can be used to render other percent based form values.

## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
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
| `value`                       | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Value will be presented to the user as a percent (0 to 100), but will be submitted to the server and stored as a decimal (0 to 1)
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-percent value="@value"></wv-field-percent>
```

