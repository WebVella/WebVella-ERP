<!--{"sort_order":10, "name": "wv-button-group", "label": "wv-button-group"}-->
# wv-button-group

## Purpose

`<wv-button-group/>`. Used to wrap a list of `<wv-button/>` providing the ability to render them as toolbar - horizontal or vertical.

## Properties

+---------------------+-----------------------------------+
| name                | description                       |
+=====================+===================================+
| `class`             | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | CSS classes that you may need to add to the standard Bootstrap CSS
+---------------------+-----------------------------------+
| `id`                | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html ID you may need to set to the rendered element
+---------------------+-----------------------------------+
| `is-vertical`       | *object type*: `bool`                         
|                     |         
|                     | *default value*: `FALSE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If TRUE, will render the button list vertically.
+---------------------+-----------------------------------+
| `size`              | *object type*: `enum CssSize`                         
|                     |         
|                     | *default value*: `CssSize.Inherit`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Size of the element. Options are: Normal,Small,Large, Inherit
+---------------------+-----------------------------------+

## Example

```html
<wv-button-group size="@CssSize.Small">
	<wv-button text="Save" color="@ErpColor.Primary"></wv-button>
	<wv-button text="Cancel" color="@ErpColor.Secondary"></wv-button>
</wv-button-group>
```

