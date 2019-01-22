<!--{"sort_order":10, "name": "wv-field-file", "label": "wv-field-file"}-->
# wv-field-file

## Purpose

`<wv-field-file/>`. Provides the ability to render the file field type of an Erp Entity. It uploads files to the server based on the configuration - in the database or on local storage. Have in mind that server file and media paths require a file controller to be access, usually you need to add "/fs" controller before the path.

## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `accept`                      | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | string of the accepted file extensions. For reference you can check out the [html attribute definition page](https://www.w3schools.com/tags/att_input_accept.asp).
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-file value="@value"></wv-field-file>
```

