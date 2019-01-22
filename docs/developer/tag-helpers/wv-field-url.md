<!--{"sort_order":10, "name": "wv-field-url", "label": "wv-field-url"}-->
# wv-field-url

## Purpose

`<wv-field-url/>`. Provides the ability to render the url field type of an Erp Entity. Can be used to render other url based form values.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `target-blank`                | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If TRUE, will open the corresponding link in a new browser tab
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-url value="@value"></wv-field-url>
```

