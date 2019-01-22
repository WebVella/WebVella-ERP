<!--{"sort_order":10, "name": "wv-section", "label": "wv-section"}-->
# wv-section

## Purpose

`<wv-section/>`. Groups fields in a section or a card, with the integrated option to collapse

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `body-class`                  | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Additional CSS classes to be added to the body of the element
+-------------------------------+-----------------------------------+
| `class`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Additional CSS classes to be added to the element
+-------------------------------+-----------------------------------+
| `field-mode`                  | *object type*: `FieldRenderMode`                         
|                               |         
|                               | *default value*: `FieldRenderMode.Undefined`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Does not have effect on the section element, but on the nested fields as it could be inherited by default. Options are: Undefined, Form, Display, InlineEdit, Simple
+-------------------------------+-----------------------------------+
| `id`                          | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | HTML Id of the generated element
+-------------------------------+-----------------------------------+
| `is-card`                     | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Whether to render the element as a card
+-------------------------------+-----------------------------------+
| `is-collapsable`              | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Add the option of the element's body to be collapsed by clicking on its title
+-------------------------------+-----------------------------------+
| `is-collapsed`                | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the initial collapse status
+-------------------------------+-----------------------------------+
| `label-mode`                  | *object type*: `LabelRenderMode` 
|                               |         
|                               | *default value*: `LabelRenderMode.Undefined`                    
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Does not have effect on the section element, but on the nested fields as it could be inherited by default. Options: Undefined, Stacked, Horizontal, Hidden
+-------------------------------+-----------------------------------+
| `title`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The section title
+-------------------------------+-----------------------------------+
| `title-tag`                   | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The HTML tag for wrapping the title text
+-------------------------------+-----------------------------------+



## Example

```html
<wv-section class="mt-4" label-mode="@LabelRenderMode.Hidden">
	<wv-row>
		<wv-column span="6">
			<wv-field-text label-text="Name" value="@Model.Name" name="Name" required="true"></wv-field-text>
		</wv-column>
		<wv-column span="6">
			<wv-field-text label-text="Label" value="@Model.Label" name="Label" required="true"></wv-field-text>
		</wv-column>
	</wv-row>
</wv-section>
```

