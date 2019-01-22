<!--{"sort_order":10, "name": "wv-form", "label": "wv-form"}-->
# wv-form

## Purpose

`<wv-form/>`. Presents a form with an ability to autogenerate a proper antiforgery token.

## Properties

+---------------------+-----------------------------------+
| name                | description                       |
+=====================+===================================+
| `accept-charset`    | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `accept-charset` attribute you may need to set to the rendered element
+---------------------+-----------------------------------+
| `action`            | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `action` attribute you may need to set to the rendered element
+---------------------+-----------------------------------+
| `antiforgery`       | *object type*: `bool`                         
|                     |         
|                     | *default value*: `true`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If true adds a antiforgery hidden input with its proper value so this form can be submitted towards a Razor Page
+---------------------+-----------------------------------+
| `autocomplete`      | *object type*: `bool`                         
|                     |         
|                     | *default value*: `TRUE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `autocomplete` attribute you may need to set to the rendered element. 
+---------------------+-----------------------------------+
| `enctype`           | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `enctype` attribute you may need to set to the rendered element. Specifies how the form-data should be encoded when submitting it to the server (only for method="post"). Options are: application/x-www-form-urlencoded, multipart/form-data, text/plain
+---------------------+-----------------------------------+
| `id`                | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html ID you may need to set to the rendered element
+---------------------+-----------------------------------+
| `label-mode`        | *object type*: `LabelRenderMode`                         
|                     |         
|                     | *default value*: `LabelRenderMode.Undefined`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | How the labels of any wrapped fields should be presented. Useful in order to set this option only on when place. Inherited by the wrapped fields.
+---------------------+-----------------------------------+
| `method`            | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `method` attribute you may need to set to the rendered element. Options are: get or post
+---------------------+-----------------------------------+
| `mode`              | *object type*: `FieldRenderMode`                         
|                     |         
|                     | *default value*: `FieldRenderMode.Undefined`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | How any wrapped fields should be presented. Useful in order to set this option only on when place. Inherited by the wrapped fields.
+---------------------+-----------------------------------+
| `name`              | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `name` attribute you may need to set to the rendered element
+---------------------+-----------------------------------+
| `novalidate`        | *object type*: `bool`                         
|                     |         
|                     | *default value*: `TRUE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `novalidate` attribute you may need to set to the rendered element. 
+---------------------+-----------------------------------+
| `target`            | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html `target` attribute you may need to set to the rendered element. Specifies where to display the response that is received after submitting the form. Options are: _blank, _self, _parent, _top
+---------------------+-----------------------------------+
| `validation`        | *object type*: `ValidationException`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Helps to present any validation messages within the form to the end user
+---------------------+-----------------------------------+

## Example

```html
<wv-form name="ManageRecord" validation="Model.Validation" label-mode="Stacked" mode="Form" autocomplete="false">
	<wv-row>
		<wv-column span="6">
			<wv-field-checkbox label-text="Enabled" value="@Model.Enabled" name="Enabled" text-true="enable this schedule plan"></wv-field-checkbox>
		</wv-column>
		<wv-column span="6">
			<wv-field-guid label-text="Id" value="@Model.Id" access="ReadOnly" name="Id"></wv-field-guid>
		</wv-column>
	</wv-row>
</wv-form>
```

