<!--{"sort_order":10, "name": "wv-chart", "label": "wv-chart"}-->
# wv-chart

## Purpose

`<wv-chart/>`. Used to render charts by using the [Chart JS](https://www.chartjs.org/) Javascript library. 

## Properties

+---------------------+-----------------------------------+
| name                | description                       |
+=====================+===================================+
| `datasets`          | *object type*: `List<ErpChartDataset>`                         
|                     |         
|                     | *default value*: `new List<ErpChartDataset>()`
|                     |
|                     | *is required*: `TRUE`                      
|                     |                                   
|                     | Data for the charts, background and border colors provided in a specific format needed by the library
+---------------------+-----------------------------------+
| `height`            | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If provided, it will be added as a CSS style height value of the chart's wrapper element.
+---------------------+-----------------------------------+
| `id`                | *object type*: `string`                         
|                     |         
|                     | *default value*: `String.Empty`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Html ID you may need to set to the rendered element
+---------------------+-----------------------------------+
| `labels`            | *object type*: `List<string>`                         
|                     |         
|                     | *default value*: `new List<string>()`
|                     |
|                     | *is required*: `TRUE`                      
|                     |                                   
|                     | Labels corresponding to the dataset values
+---------------------+-----------------------------------+
| `show-legend`       | *object type*: `bool`                         
|                     |         
|                     | *default value*: `FALSE`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | Whether to render the chart's legend
+---------------------+-----------------------------------+
| `type`              | *object type*: `enum ErpChartType`                         
|                     |         
|                     | *default value*: `ErpChartType.Line`
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | The type of the chart that needs to be rendered. Options are: Line, Bar, Pie, Doughnut, Area, HorizontalBar
+---------------------+-----------------------------------+
| `width`             | *object type*: `string` 
|                     |         
|                     | *default value*: `String.Empty`                    
|                     |
|                     | *is required*: `FALSE`                      
|                     |                                   
|                     | If provided, it will be added as a CSS style width value of the chart's wrapper element.
+---------------------+-----------------------------------+


## Example

```html
<wv-chart type="@ErpChartType.Line" datasets="@Datasets"></wv-chart>
```

