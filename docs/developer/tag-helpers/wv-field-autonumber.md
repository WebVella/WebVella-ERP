<!--{"sort_order":10, "name": "wv-field-autonumber", "label": "wv-field-autonumber"}-->
# wv-field-autonumber

## Purpose

`<wv-field-autonumber/>`. Provides the ability to render the autonumber field type of an Erp Entity. Can be used to render other template based text too.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `template`                    | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | This template will be used in a string format operation, which will replace any `{0}` used in the template with the field's value
+-------------------------------+-----------------------------------+

## Example

```html
@{
	var template = "Order-{0}"
}
<wv-field-autonumber value="123" template="@template"></wv-field-autonumber>
```

