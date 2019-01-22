<!--{"sort_order":10, "name": "wv-button-toolbar", "label": "wv-button-toolbar"}-->
# wv-button-toolbar

## Purpose

`<wv-button-toolbar/>`. Used to wrap multiple `<wv-button-group/>` and render them horizontal or vertical.

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
<wv-button-toolbar  size="@CssSize.Small">
	<wv-button-group>
		<wv-button text="Prev"></wv-button>
	</wv-button-group>
	<wv-button-group>
		<wv-button text="1"></wv-button>
		<wv-button text="2"></wv-button>
		<wv-button text="3"></wv-button>
	</wv-button-group>
	<wv-button-group>
		<wv-button text="Next"></wv-button>
	</wv-button-group>
</wv-button-toolbar>
```

