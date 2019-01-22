<!--{"sort_order":10, "name": "wv-field-image", "label": "wv-field-image"}-->
# wv-field-image

## Purpose

`<wv-field-image/>`. Provides the ability to render the image field type of an Erp Entity. It uploads files to the server based on the configuration - in the database or on local storage. Have in mind that server file and media paths require a file controller to be access, usually you need to add "/fs" controller before the path.

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
| `height`                      | *object type*: `int?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | the requested image height. The server will automatically resize and cache the new copy of the image
+-------------------------------+-----------------------------------+
| `resize-action`               | *object type*: `ImageResizeMode`                         
|                               |         
|                               | *default value*: `ImageResizeMode.Pad`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | the resize action type that needs to be taken when both width and height are set. Options are: Pad, BoxPad, Crop, Min, Max, Stretch. For more information please check the [ImageProcessor reference page](http://imageprocessor.org/imageprocessor-web/imageprocessingmodule/resize/)
+-------------------------------+-----------------------------------+
| `text-remove`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `remove`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | the text for the remove image link
+-------------------------------+-----------------------------------+
| `text-select`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `select`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | the text for the select image link
+-------------------------------+-----------------------------------+
| `width`                       | *object type*: `int?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | the requested image width. The server will automatically resize and cache the new copy of the image
+-------------------------------+-----------------------------------+


## Example

```html
<wv-field-image value="@value"></wv-field-image>
```

