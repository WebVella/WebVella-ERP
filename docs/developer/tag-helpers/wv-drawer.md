<!--{"sort_order":10, "name": "wv-drawer", "label": "wv-drawer"}-->
# wv-drawer

## Purpose

`<wv-drawer/>`. Presents a sliding from the right container. The element is operated with the help of JS Event it listens to. 

## Properties

+---------------------+-----------------------------------+
| name                | description                       |
+=====================+===================================+
| `body-class`        | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | A CSS class to be added to the general classes of the element's body.
+---------------------+-----------------------------------+
| `class`             | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | A CSS class to be added to the general classes of the element.
+---------------------+-----------------------------------+
| `id`                | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html ID you may need to set to the rendered element
+---------------------+-----------------------------------+
| `title`             | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | The title of the drawer.
+---------------------+-----------------------------------+
| `title-action-html` | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html to be rendered on the right of the title
+---------------------+-----------------------------------+
| `width`             | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If provided, it will be added as a CSS style width value of the element.
+---------------------+-----------------------------------+

## Javascript Event Listeners
+----------------------+----------------------------------+
| action               | description                      |
+======================+==================================+
| `open` or `show`     | This action will open the drawer. Example:
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')
|                      | ```
|                      | 
|                      | If there are one or more drawers on the page you need to set the correct `htmlId` of the drawer's PageComponent  
|                      | 
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer',{htmlId:HTML_ID,action:'open',payload:null})
|                      | ```
+----------------------+----------------------------------+
| `close` or `hide`    | This action will close the drawer. Example:
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','close')
|                      | ```
|                      | 
|                      | If there are one or more drawers on the page you need to set the correct `htmlId` of the drawer's PageComponent  
|                      | 
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer',{htmlId:HTML_ID,action:'close',payload:null})
|                      | ```
+----------------------+----------------------------------+

## Example

```html
<wv-drawer id="my-drawer" title="This is a drawer">...</wv-drawer>
```

