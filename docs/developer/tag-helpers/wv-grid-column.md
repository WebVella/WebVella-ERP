<!--{"sort_order":10, "name": "wv-grid-column", "label": "wv-grid-column"}-->
# wv-grid-column

## Purpose

`<wv-grid-column/>`. This tag helper is used in conjunction with `<wv-grid/>` and `<wv-grid-row/>` to generate a grid.

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `class`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Added to the grid generated classes
+-------------------------------+-----------------------------------+
| `horizontal-align`            | *object type*: `HorizontalAlignmentType`                         
|                               |         
|                               | *default value*: `HorizontalAlignmentType.None`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | horizontal alignment with this column's table cells. Options are: None, Left, Center, Right
+-------------------------------+-----------------------------------+
| `text-wrap`                   | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | enable or disable the text-wrapping in this column's table cells
+-------------------------------+-----------------------------------+
| `vertical-align`              | *object type*: `VerticalAlignmentType`                         
|                               |         
|                               | *default value*: `VerticalAlignmentType.None`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | vertical alignment with this column's table cells. Options are: None, Top, Middle, Bottom
+-------------------------------+-----------------------------------+


## Example

```html
<wv-grid page="@pager" total-count="@totalCount" columns="@columns">
	@foreach(var record in records)
	{
		<wv-grid-row>
			<wv-grid-column>...</wv-grid-column>
			<wv-grid-column>...</wv-grid-column>
		</wv-grid-row>
	}
</wv-form>
```

