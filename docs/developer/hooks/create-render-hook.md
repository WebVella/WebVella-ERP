<!--{"sort_order":5, "name": "create-render-hook", "label": "Create Render Hook"}-->
# Create a new Render Hook

It is very easy to create a render hook, so other plugins can attach and inject html in its placeholder. To do that just paste a code in a Razor view, very similarly to including a view component via a tag helper.

## Properties
The hook supports the following properties, which will be also provided as parameters to all attached methods:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `model`                       | *object type*: `dynamic`                         
|                               |         
|                               | *default value*: `null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Any specific model you want to provide to the hook attachments. Useful when you loop through items in a list and want to provide the current item
+-------------------------------+-----------------------------------+
| `page-model`                  | *object type*: `BaseErpPageModel`                         
|                               |         
|                               | *default value*: `new BaseErpPageModel()`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | The current page model.
+-------------------------------+-----------------------------------+
| `placeholder`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `string.Empty`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | An unique name defining the position
+-------------------------------+-----------------------------------+

IMPORTANT: In our application master page there are already render hooks included for: `head-top`, `head-bottom`,`body-toop`, `body-bottom`.

## Example 
Paste the following code in a Razor view:

```html
<vc:render-hook placeholder="head-top" page-model="@Model"  model="null"></vc:render-hook>
```

