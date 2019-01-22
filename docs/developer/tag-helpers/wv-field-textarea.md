<!--{"sort_order":10, "name": "wv-field-textarea", "label": "wv-field-textarea"}-->
# wv-field-textarea

## Purpose

`<wv-field-textarea/>`. Provides the ability to render the multiline-text field type of an Erp Entity. Can be used to render other longer text based form values.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `autogrow`                    | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | will increase the textarea height automatically based on the text content
+-------------------------------+-----------------------------------+
| `height`                      | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | will be set as a height style to the textarea input
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-textarea value="@value"></wv-field-textarea>
```

