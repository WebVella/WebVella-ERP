<!--{"sort_order":10, "name": "wv-modal", "label": "wv-modal"}-->
# wv-modal

## Purpose

`<wv-modal/>`. Presents a modal window. The element is operated with the help of JS Event it listens to. Used in conjunction with `<wv-modal-body/>` and `<wv-modal-footer/>`

## Properties

+---------------------+-----------------------------------+
| name                | description                       |
+=====================+===================================+
| `backdrop`          | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Sets the modal's backdrop property according to the Bootstrap reference. Options are: `true`, `false` or `static`. Includes a modal-backdrop element. Alternatively, specify static for a backdrop which doesn't close the modal on click.
+---------------------+-----------------------------------+
| `id`                | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html ID you may need to set to the rendered element
+---------------------+-----------------------------------+
| `position`          | *object type*: `ModalPosition` 
|                     |         
|                     | *default value*: `ModalPosition.Top`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Sets the modal position in the viewport. Options are: Top, VerticallyCentered
+---------------------+-----------------------------------+
| `size`              | *object type*: `ModalSize` 
|                     |         
|                     | *default value*: `ModalSize.Normal`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Sets the modal width size. Options are: Normal, Small, Large, ExtraLarge, Full
+---------------------+-----------------------------------+
| `title`             | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | The title of the modal.
+---------------------+-----------------------------------+


## Javascript Event Listeners
+----------------------+----------------------------------+
| action               | description                      |
+======================+==================================+
| `open` or `show`     | This action will open the modal. Example:
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal','open')
|                      | ```
|                      | 
|                      | If there are one or more modals on the page you need to set the correct `htmlId` of the modal's PageComponent  
|                      | 
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:HTML_ID,action:'open',payload:null})
|                      | ```
+----------------------+----------------------------------+
| `close` or `hide`    | This action will close the modal. Example:
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal','close')
|                      | ```
|                      | 
|                      | If there are one or more modals on the page you need to set the correct `htmlId` of the modal's PageComponent  
|                      | 
|                      | ```javascript
|                      | ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:HTML_ID,action:'close',payload:null})
|                      | ```
+----------------------+----------------------------------+

## Example

```html
<wv-modal title="SQL Result" id="modal-sql-result" size="Large">
	<wv-modal-body>
		...
	</wv-modal-body>
	<wv-modal-footer>
		...
	</wv-modal-footer>
</wv-modal>
```

