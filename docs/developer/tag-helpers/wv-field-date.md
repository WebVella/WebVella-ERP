<!--{"sort_order":10, "name": "wv-field-date", "label": "wv-field-date"}-->
# wv-field-date

## Purpose

`<wv-field-date/>`. Provides the ability to render the date field type of an Erp Entity. Can be used to render other date based form values.


## Properties
**Important**: All `<wv-field-*>` helpers inherit a ["base tag helper" properties](docs/developer/tag-helpers/wv-field-base). In the following list are presented only the properties that this tag helper adds or alters. Not all base tag helper properties can be implemented by this tag helper too.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `value`                       | *object type*: `DateTime?`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | All date's in the database are stored as a date, without a timezone or a time component. Have in mind that for the date, the server timezone is considered always as the beginning of the date
+-------------------------------+-----------------------------------+

## Example

```html
<wv-field-date value="@value"></wv-field-date>
```

