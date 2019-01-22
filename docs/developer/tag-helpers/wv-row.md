<!--{"sort_order":10, "name": "wv-row", "label": "wv-row"}-->
# wv-row

## Purpose

`<wv-row/>`. Helps in rendering a Bootstrap Layout Grid column and its options.

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `class`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Additional CSS classes to be added to the element
+-------------------------------+-----------------------------------+
| `flex-horizontal-alignment`   | *object type*: `FlexHorizontalAlignmentType`                         
|                               |         
|                               | *default value*: `FlexHorizontalAlignmentType.None`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the horizontal positioning of the nested columns as per flex standard. Options are: None, Start, Center, End, Around, Between
+-------------------------------+-----------------------------------+
| `flex-vertical-alignment`     | *object type*: `FlexVerticalAlignmentType`                         
|                               |         
|                               | *default value*: `FlexVerticalAlignmentType.None`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the vertical positioning of the nested columns as per flex standard. Options are: None, Start, Center, End
+-------------------------------+-----------------------------------+
| `no-gutters`                  | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Removes the nested column padding as per Bootstrap styling
+-------------------------------+-----------------------------------+


## Example

```html
<wv-row>
	<wv-column span="6">...<wv-column>
	<wv-column span="6">...<wv-column>
</wv-row>
```

