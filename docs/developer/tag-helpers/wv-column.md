<!--{"sort_order":10, "name": "wv-column", "label": "wv-column"}-->
# wv-column

## Purpose

`<wv-column/>`. Mostly used in a `<wv-row/>` tag helper. Helps in rendering a Bootstrap Layout Grid column and its options.

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
|                     | Additional CSS classes to be added to the element
+---------------------+-----------------------------------+
| `flex-order`        | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Sets the column flex order
+---------------------+-----------------------------------+
| `flex-self-align`   | *object type*: `FlexSelfAlignType`                         
|                     |         
|                     | *default value*: `FlexSelfAlignType.None`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Sets the flex alignment of the elements in the column. Options are: None, Start, Center, End
+---------------------+-----------------------------------+
| `id`                | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html ID you may need to set to the rendered element
+---------------------+-----------------------------------+
| `offset`            | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `offset` CSS class for the element. Null will not render the class
+---------------------+-----------------------------------+
| `offset-sm`         | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `offset-sm` CSS class for the element. Null will not render the class
+---------------------+-----------------------------------+
| `offset-md`         | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `offset-md` CSS class for the element. Null will not render the class
+---------------------+-----------------------------------+
| `offset-lg`         | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `offset-lg` CSS class for the element. Null will not render the class
+---------------------+-----------------------------------+
| `offset-xl`         | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `offset-xl` CSS class for the element. Null will not render the class
+---------------------+-----------------------------------+
| `span`              | *object type*: `int?`                         
|                     |         
|                     | *default value*: `0`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `span` CSS class for the element. 0 will render `col`, null will not render the class,-1 will render `col-auto`, any other value will render `col-` plus the value
+---------------------+-----------------------------------+
| `span-sm`           | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `span` CSS class for the element. 0 will render `col-sm`, null will not render the class,-1 will render `col-sm-auto`, any other value will render `col-sm-` plus the value
+---------------------+-----------------------------------+
| `span-md`           | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `span` CSS class for the element. 0 will render `col-md`, null will not render the class,-1 will render `col-md-auto`, any other value will render `col-md-` plus the value
+---------------------+-----------------------------------+
| `span-lg`           | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `span` CSS class for the element. 0 will render `col-lg`, null will not render the class,-1 will render `col-lg-auto`, any other value will render `col-lg-` plus the value
+---------------------+-----------------------------------+
| `span-xl`           | *object type*: `int?`                         
|                     |         
|                     | *default value*: `NULL`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Renders the proper `span` CSS class for the element. 0 will render `col-xl`, null will not render the class,-1 will render `col-xl-auto`, any other value will render `col-xl-` plus the value
+---------------------+-----------------------------------+



## Example

```html
<wv-row>
	<wv-column span="6">...<wv-column>
	<wv-column span="6">...<wv-column>
</wv-row>
```

