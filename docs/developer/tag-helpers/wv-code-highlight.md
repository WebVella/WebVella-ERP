<!--{"sort_order":10, "name": "wv-code-highlight", "label": "wv-code-highlight"}-->
# wv-code-highlight

## Purpose

Helper for implementing [prismjs.com](http://prismjs.com) code highlighter. Install this JS library and its CSS classes before using this Tag helper. 
The library is already installed in this developer's section.

## Properties

+-----------------------------------+-----------------------------------+
| name                              | description                       |
+===================================+===================================+
|`wv-code-highlight`                | *html target*: `attribute`        
|                                   |         
|                                   | *object type*: `string`
|                                   |         
|                                   | *default value*: `language-html`                     
|                                   |
|                                   | *is required*: `TRUE`                      
|                                   |                                   
|                                   | Sets the highlighting language based on the install plugins for prism.js and according to the [supported languages](http://prismjs.com/index.html#languages-list)
+-----------------------------------+-----------------------------------+
|`wv-code-string`                   | *html target*: `attribute`        
|                                   |         
|                                   | *object type*: `string` 
|                                   |         
|                                   | *default value*: `sample html`                   
|                                   |
|                                   | *is required*: `TRUE`                      
|                                   |                                   
|                                   | A string variable that provides the source code / html to be rendered. The implementation is done this way, as otherwise MVC will clear any used non standard attributes in the HTML case
+-----------------------------------+-----------------------------------+

## Example

```html
    <div wv-code-highlight="language-html" wv-code-string="@example1Code"></div>
```

