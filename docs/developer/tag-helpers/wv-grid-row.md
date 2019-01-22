<!--{"sort_order":10, "name": "wv-grid-row", "label": "wv-grid-row"}-->
# wv-grid-row

## Purpose

`<wv-grid-row/>`. This tag helper is used in conjunction with `<wv-grid/>` and `<wv-grid-column/>` to generate a grid.

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| does not have any specific properties                             | 
+-------------------------------------------------------------------+


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

