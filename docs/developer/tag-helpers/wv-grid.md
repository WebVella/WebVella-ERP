<!--{"sort_order":10, "name": "wv-grid", "label": "wv-grid"}-->
# wv-grid

## Purpose

`<wv-grid/>`. Generates a table / grid with integrated paging, sorting and more. The primary tool for presenting list of entity records. This tag helper is used in conjunction with `<wv-grid-row/>` and `<wv-grid-column/>`

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `bordered`                    | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Draws table borders, by applying Bootstrap styling
+-------------------------------+-----------------------------------+
| `borderless`                  | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Draws borderless table, by applying Bootstrap styling
+-------------------------------+-----------------------------------+
| `class`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Added to the grid generated classes
+-------------------------------+-----------------------------------+
| `columns`                     | *object type*: `List<GridColumn>`                         
|                               |         
|                               | *default value*: `new List<GridColumn>()`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Describes the columns of the grid. 
+-------------------------------+-----------------------------------+
| `culture`                     | *object type*: `CultureInfo`                         
|                               |         
|                               | *default value*: `new CultureInfo("en-US")`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used in data presentation. Could be inherited by other helpers wrapped in a <wv-grid/>
+-------------------------------+-----------------------------------+
| `has-tfoot`                   | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If FALSE, it will head the grid's tfoot element
+-------------------------------+-----------------------------------+
| `has-thead`                   | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If FALSE, it will head the grid's thead element
+-------------------------------+-----------------------------------+
| `hover`                       | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Changes the background of the hovered table row, by applying Bootstrap styling
+-------------------------------+-----------------------------------+
| `id`                          | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Html ID you may need to set to the rendered element
+-------------------------------+-----------------------------------+
| `name`                        | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used in grid's CSS class generation
+-------------------------------+-----------------------------------+
| `page`                        | *object type*: `int`                         
|                               |         
|                               | *default value*: `0`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the current page value
+-------------------------------+-----------------------------------+
| `page-size`                   | *object type*: `int`                         
|                               |         
|                               | *default value*: `0`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the expected page size 
+-------------------------------+-----------------------------------+
| `prefix`                      | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If you have two or more grids on a single page, each grid could apply a prefix to it query parameters it applies. 
+-------------------------------+-----------------------------------+
| `query-string-page`           | *object type*: `string`                         
|                               |         
|                               | *default value*: `page`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | This will override the default query string name for pagination. Used only in grid links generation
+-------------------------------+-----------------------------------+
| `query-string-sortby`         | *object type*: `string`                         
|                               |         
|                               | *default value*: `sortBy`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | This will override the default query string name for sorting. Used only in grid links generation
+-------------------------------+-----------------------------------+
| `query-string-sort-order`     | *object type*: `string`                         
|                               |         
|                               | *default value*: `sortOrder`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | This will override the default query string name for sorting order. Used only in grid links generation
+-------------------------------+-----------------------------------+
| `responsive-breakpoint`       | *object type*: `CssBreakpoint`                         
|                               |         
|                               | *default value*: `CssBreakpoint.None`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Across every breakpoint for horizontally scrolling tables.
+-------------------------------+-----------------------------------+
| `small`                       | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | less padding of the table cells, by applying Bootstrap styling
+-------------------------------+-----------------------------------+
| `striped`                     | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Alternates the background of each row of the table, by applying Bootstrap styling
+-------------------------------+-----------------------------------+
| `total-count`                 | *object type*: `int`                         
|                               |         
|                               | *default value*: `0`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the total count of the records
+-------------------------------+-----------------------------------+
| `vertical-align`              | *object type*: `VerticalAlignmentType`                         
|                               |         
|                               | *default value*: `VerticalAlignmentType.None`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | vertical alignment with the table. Options are: None, Top, Middle, Bottom
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

