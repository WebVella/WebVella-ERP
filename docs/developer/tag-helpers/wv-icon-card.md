<!--{"sort_order":10, "name": "wv-icon-card", "label": "wv-icon-card"}-->
# wv-icon-card

## Purpose

`<wv-icon-card/>`. Generates a styles icon card.

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
|                               | Added to the generated card classes
+-------------------------------+-----------------------------------+
| `description`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Description of the card 
+-------------------------------+-----------------------------------+
| `icon-class`                  | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used to generate the icon of the card using [FontAwesome icon library](https://fontawesome.com/icons)
+-------------------------------+-----------------------------------+
| `icon-color`                  | *object type*: `ErpColor`                         
|                               |         
|                               | *default value*: `ErpColor.Default`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Select from 32 color options
+-------------------------------+-----------------------------------+
| `is-card`                     | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If TRUE generates the card wrapping lines according to the Bootstrap styling
+-------------------------------+-----------------------------------+
| `is-clickable`                | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If TRUE adds a "clickable" class that will change the cursor as pointer on hovering the card
+-------------------------------+-----------------------------------+
| `has-shadow`                  | *object type*: `bool`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If TRUE generates a shadow below the card
+-------------------------------+-----------------------------------+
| `title`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.empty`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Title of the card 
+-------------------------------+-----------------------------------+



## Example

```html
<wv-icon-card title="Database" class="mb-4" description="SQL Select" icon-class="fas fa-fw fa-database" icon-color="Purple"></wv-icon-card>
```

