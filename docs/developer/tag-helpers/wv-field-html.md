<!--{"sort_order":10, "name": "wv-field-html", "label": "wv-field-html"}-->
# wv-field-html

## Purpose

`<wv-field-html/>`. Provides the ability to render the html field type of an Erp Entity. Can be used to render other html based form values.

## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `upload-mode`                 | *object type*: `HtmlUploadMode`                         
|                               |         
|                               | *default value*: `HtmlUploadMode.None`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Editor allows user's to upload images or files that they use during the HTML creating. This property manages their storage. Options are: None, SiteRepository. "None" will not allow files to be uploaded from the editor.
+-------------------------------+-----------------------------------+
| `toolbar-mode`                | *object type*: `HtmlToolbarMode`                         
|                               |         
|                               | *default value*: `HtmlToolbarMode.Basic`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Manages the editor's toolbar button configurations. Options are: Basic, Standard, Full
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-html value="@value"></wv-field-html>
```

