<!--{"sort_order":10, "name": "wv-button", "label": "wv-button"}-->
# wv-button

## Purpose

`<wv-button/>`. Used to render button and links styled as buttons with added features for styling, sizing, form submission and click behavior. 

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
| `color`             | *object type*: `enum ErpColor`
|                     |         
|                     | *default value*: `ErpColor.White`                        
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Select from 32 color options
+---------------------+-----------------------------------+
| `form`              | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Awaits a form HTML ID, which will be submitted when the button is pressed. Available only for `ButtonType.Submit`
+---------------------+-----------------------------------+
| `formaction`        | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If submitted, will add a `formaction` attribute with this value. Available only for `ButtonType.Submit`
+---------------------+-----------------------------------+
| `href`              | *object type*: `string`
|                     |         
|                     | *default value*: `String.Empty`                        
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Used when the tag renders an HTML link element only.
+---------------------+-----------------------------------+
| `icon-class`        | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If submitted an additional `<i/>` HTML element will be rendered before the button text with the set CSS class using [FontAwesome icon library](https://fontawesome.com/icons)
+---------------------+-----------------------------------+
| `id`                | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html ID you may need to set to the rendered element
+---------------------+-----------------------------------+
| `is-active`         | *object type*: `bool`                         
|                     |         
|                     | *default value*: `FALSE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Additional `active` class will be added to the button
+---------------------+-----------------------------------+
| `is-block`          | *object type*: `bool`                         
|                     |         
|                     | *default value*: `FALSE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | The button width will expand based on the container it is wrapped in
+---------------------+-----------------------------------+
| `is-disabled`       | *object type*: `bool`                         
|                     |         
|                     | *default value*: `FALSE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If the tag renders an HTML button, an additional `disabled` attribute will be added. If the tag renders a link, additional `disabled` class will be addded.
+---------------------+-----------------------------------+
| `is-outline`        | *object type*: `bool`                         
|                     |         
|                     | *default value*: `FALSE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Render only the button outlines and no background under the text
+---------------------+-----------------------------------+
| `new-tab`           | *object type*: `string`
|                     |         
|                     | *default value*: `String.Empty`                        
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Used when the tag renders an HTML link element only. Will add an attribute `target="_blank"`
+---------------------+-----------------------------------+
| `onclick`           | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If submitted, an `onclick` attribute will be added to the element.
+---------------------+-----------------------------------+
| `size`              | *object type*: `enum CssSize`                         
|                     |         
|                     | *default value*: `CssSize.Inherit`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Size of the element. Options are: Normal,Small,Large, Inherit
+---------------------+-----------------------------------+
| `text`              | *object type*: `string`
|                     |         
|                     | *default value*: `String.Empty`                         
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | The button text displayed to the user
+---------------------+-----------------------------------+
| `type`              | *object type*: `enum ButtonType`
|                     |         
|                     | *default value*: `ButtonType.Button`          
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | The type of the rendered element. Available options are: `Button` (button type="button"), `Submit` (button type="submit"), `LinkAsButton` (link that mimics a button), `ButtonLink` (button that mimics a link)                   
+---------------------+-----------------------------------+

## Example

```html
<wv-button type="@ButtonType.Button" text="Save" color="@ErpColor.Primary"></wv-button>
```

