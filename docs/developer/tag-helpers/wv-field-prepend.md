<!--{"sort_order":10, "name": "wv-field-prepend", "label": "wv-field-prepend"}-->
# wv-field-prepend

## Purpose

`<wv-field-prepend/>`. This is used only in a `<wv-field-*/>` field tag helpers. Provides the ability to render the field with an "input-group" as per Bootstrap CSS specifications. Prepends the provided string as html in its proper place. 

## Example

```html
<wv-field-date>
	<wv-field-prepend><span class='input-group-text'><i class='fa fa-fw fa-calendar-alt'></i></span></wv-field-prepend>
</wv-field-date>
```

