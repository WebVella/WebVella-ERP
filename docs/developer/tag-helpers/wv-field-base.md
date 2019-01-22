<!--{"sort_order":10, "name": "wv-field-base", "label": "wv-field-*"}-->
# wv-field-* base properties

## Purpose

All field tag helpers inherit this base field's properties. Some of them can be overrided or not used by the specific field though. Check the relevant tag helper document page for more information.

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `access`                      | *object type*: `FieldAccess`                         
|                               |         
|                               | *default value*: `FieldAccess.Full`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets whether the user can interact or view that value of the field. Options are: Undefined, Full, FullAndCreate, ReadOnly, Forbidden
+-------------------------------+-----------------------------------+
| `access-denied-message`       | *object type*: `string`                         
|                               |         
|                               | *default value*: `access denied`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Overrides the default access denied message, presented to the user when he/she doesn't have access to the field value
+-------------------------------+-----------------------------------+
| `api-url`                     | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Overrides the default API URL call, when InlineEdit a field
+-------------------------------+-----------------------------------+
| `autocomplete`                | *object type*: `bool`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Ability to set the autocomplete attribute of the field's input, used by the browsers to prefill form data.
+-------------------------------+-----------------------------------+
| `class`                       | *object type*: `string` 
|                               |         
|                               | *default value*: `String.Empty`                    
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | A CSS class to be added to the general classes of the field.
+-------------------------------+-----------------------------------+
| `field-id`                    | *object type*: `Guid?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Field Id is used when initializing scripts
+-------------------------------+-----------------------------------+
| `description`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Presents a description text after the field's input
+-------------------------------+-----------------------------------+
| `default-value`               | *object type*: `dynamic`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Depends on the field type.
+-------------------------------+-----------------------------------+
| `empty-value-message`         | *object type*: `string`                         
|                               |         
|                               | *default value*: `no data`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Overrides the default message, when the field value is null
+-------------------------------+-----------------------------------+
| `entity-name`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used in InlineEdit mode in order to set the entity of the altered record Id
+-------------------------------+-----------------------------------+
| `id`                          | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Html ID you may need to set to the rendered field
+-------------------------------+-----------------------------------+
| `init-errors`                 | *object type*: `List<string>`                         
|                               |         
|                               | *default value*: `new List<string>()`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If any init errors are set, the field will render the label and an error message. Usually used for showing non validation system errors
+-------------------------------+-----------------------------------+
| `label-error-text`            | *object type*: `string` 
|                               |         
|                               | *default value*: `String.Empty`                    
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Will render an error icon next to the label text, with the provided text as a tooltip 
+-------------------------------+-----------------------------------+
| `label-help-text`             | *object type*: `string` 
|                               |         
|                               | *default value*: `String.Empty`                    
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Will render a help icon next to the label text, with the provided text as a tooltip 
+-------------------------------+-----------------------------------+
| `label-mode`                  | *object type*: `LabelRenderMode` 
|                               |         
|                               | *default value*: `LabelRenderMode.Undefined`                    
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the rendering mode of the label. Options: Undefined, Stacked, Horizontal, Hidden
+-------------------------------+-----------------------------------+
| `label-text`                  | *object type*: `string` 
|                               |         
|                               | *default value*: `String.Empty`                    
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The text rendered as a field label
+-------------------------------+-----------------------------------+
| `label-warning-text`          | *object type*: `string` 
|                               |         
|                               | *default value*: `String.Empty`                    
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Will render a warning icon next to the label text, with the provided text as a tooltip 
+-------------------------------+-----------------------------------+
| `locale`                      | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If set, this will initialize the `new CultureInfo(Locale)` object which will be used in the fields value presentation and localization
+-------------------------------+-----------------------------------+
| `mode`                        | *object type*: `FieldRenderMode`                         
|                               |         
|                               | *default value*: `FieldRenderMode.Undefined`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Defines how the field will be rendered. Options are: Undefined, Form, Display, InlineEdit, Simple
+-------------------------------+-----------------------------------+
| `name`                        | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Will set the name attribute of the html element
+-------------------------------+-----------------------------------+
| `placeholder`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `String.Empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Sets the placeholder attribute of the field's input
+-------------------------------+-----------------------------------+
| `record-id`                   | *object type*: `Guid?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used in InlineEdit mode and send to the API handler which looks for the field name as record property and alters it 
+-------------------------------+-----------------------------------+
| `required`                    | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Present a red asterix sign before the field's label
+-------------------------------+-----------------------------------+
| `validation-errors`           | *object type*: `List<ValidationError>`                         
|                               |         
|                               | *default value*: `new List<ValidationError>()`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If any validation errors are set, the field will render them at its bottom. Used for form validation messages towards the end user.
+-------------------------------+-----------------------------------+
| `value`                       | *object type*: `dynamic`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Depends on the field type.
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-autonumber id="my-drawer" title="This is a drawer">...</wv-field-autonumber>
```

