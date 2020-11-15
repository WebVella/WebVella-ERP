<!--{"sort_order":3, "name": "entity-fields", "label": "Entity Fields"}-->
# Entity Fields

In order to achieve having as little code as possible, entity field definitions are extended from their traditional database sense. The guiding factor is not the type of the data, but how the end user will consume and alter this data. In other words, the entity fields are a symbioses between the data part and the UI control.

**Important**: All `<wv-field-*/>` support the entity field meta as defined below. When you use the PCField PageCompoment though you need to "map" the component by selecting the corresponding checkbox. This will automatically apply all field meta settings.

## Base Field

All field types inherit this class.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `Auditable`                   | *object type*: `bool`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Not implemented yet. Meant to enable history for the field data changes
+-------------------------------+-----------------------------------+
| `Description`                 | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used in UI Forms. A text that will appear after the input as a description
+-------------------------------+-----------------------------------+
| `EnableSecurity`              | *object type*: `bool`                         
|                               |         
|                               | *default value*: `FALSE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If set to TRUE, it will apply the permissions as defined in Permissions
+-------------------------------+-----------------------------------+
| `HelpText`                    | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used in UI Forms. It will appear as a help icon on the right of the field label and will show this text/html as a tooltip
+-------------------------------+-----------------------------------+
| `Id`                          | *object type*: `Guid`                         
|                               |         
|                               | *default value*: `Guid?`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Many processes use this as an identifier of the field. As an example a plugin can check not only if a field with a name exists but also if it matches the ID it requires.
+-------------------------------+-----------------------------------+
| `Label`                       | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Used in UI Forms. A text that will be displayed as a field identifier.
+-------------------------------+-----------------------------------+
| `Name`                        | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Human readable reference of the field a.k.a developer name
+-------------------------------+-----------------------------------+
| `Permissions`                 | *object type*: `FieldPermissions`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Defines which role has read or update permissions. Applied if EnableSecurity is TRUE
+-------------------------------+-----------------------------------+
| `PlaceholderText`             | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Used in UI Forms. The value of the placeholder attribute - the input's hint.
+-------------------------------+-----------------------------------+
| `Required`                    | *object type*: `bool`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Used in UI Forms and API. It will present a red asterix on the left of the field's label. Also if no value is provided on record creation, the API will automatically fill in the default value when the field is required.
+-------------------------------+-----------------------------------+
| `Searchable`                  | *object type*: `bool`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If set to TRUE, this will create a database index for quicker selects and sorts by this field. Have in mind that fields that participate in a relation will be indexed automatically
+-------------------------------+-----------------------------------+
| `System`                      | *object type*: `bool`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | When creating an application, you will need to define a schema that you need to be managed only by the corresponding plugin and not the administrator. By marking the field as System, the interface will lock certain changes.
+-------------------------------+-----------------------------------+
| `Unique`                      | *object type*: `bool`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | This will put the necessary database constrain so only unique values will be possible for this field
+-------------------------------+-----------------------------------+

## Autonumber

If you need a automatically incremented number with each new record, this is the field you need. You can customize the display format also. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DisplayFormat`               | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Influences how the field value is rendered. Use "{0}" where you need the value to be inserted in the template
+-------------------------------+-----------------------------------+
| `DefaultValue`                | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `StartingNumber`              | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | The starting number of the autonumber field in the database
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.AutoNumberField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Checkbox

The simple on and off switch. This field allows you to get a True (checked) or False (unchecked) value. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `bool?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.CheckboxField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Currency

A currency amount can be entered and will be represented in a suitable formatted way. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `Currency`                    | *object type*: `CurrencyType`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Localize and influence the field value rendering
+-------------------------------+-----------------------------------+
| `DefaultValue`                | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `MaxValue`                    | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as max html input attribute
+-------------------------------+-----------------------------------+
| `MinValue`                    | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as min html input attribute
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.CurrencyField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Date

A data pickup field, that can be later converting according to a provided pattern. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `DateTime?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `Format`                      | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Csharp date formatting string
+-------------------------------+-----------------------------------+
| `UseCurrentTimeAsDefaultValue`| *object type*: `bool?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Whether to use the record creation date as default
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.DateField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## DateTime

A date and time can be picked up and later presented according to a provided pattern. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `DateTime?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `Format`                      | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Csharp date formatting string
+-------------------------------+-----------------------------------+
| `UseCurrentTimeAsDefaultValue`| *object type*: `bool?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Whether to use the record creation date as default
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.DateTimeField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Email

An email can be entered by the user, which will be validated and presented accordingly. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `MaxLength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | set as maxlength attribute of the html input element
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.EmailField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## File

File upload field. Files will be stored within the system. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.FileField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Geography

Stores geography information in either Well Known Text (WKT) or GeoJSON.  Requires postgis to be installed.

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `Format`                      | *object type*: `GeographyFormat`                         
|                               |         
|                               | *default value*: `GeoJSON`
|                               |
|                               | *is required*: `true`                      
|                               |                                   
|                               | The underlying data will be stored as either GeoJSON or WKT
+-------------------------------+-----------------------------------+
| `SRID`                        |*object type*: `integer`    
|                               |         
|                               | *default value*: `4236`
|                               |
|                               | *is required*: `true`                      
|                               |                                   
|                               | The spatial reference that will be used for the field
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.Geography`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Guid

Very important field for any entity to entity relation and required by it. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `Guid?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `GenerateNewId`               | *object type*: `bool?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | If TRUE, will generate a new Guid as a default value
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.GuidField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Html

Provides the ability of entering and presenting an HTML code. Supports multiple input languages. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `Guid?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.HtmlField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Image

Image upload field. Images will be stored within the system. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.ImageField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Textarea

A textarea for plain text input. Will be cleaned and stripped from any web unsafe content. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `MaxLength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | set as maxlength attribute of the html input element
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.MultiLineTextField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Multiselect

Multiple values can be selected from a provided list. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `List<string>`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `Options`                     | *object type*: `List<SelectOption>`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Options that will be presented to the user for selection.
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.MultiSelectField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Number

Only numbers are allowed. Leading zeros will be stripped. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DecimalPlaces`               | *object type*: `byte?`                         
|                               |         
|                               | *default value*: `2`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | To how many decimal places the presented value should be rounded.
+-------------------------------+-----------------------------------+
| `DefaultValue`                | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `MaxValue`                    | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as max html input attribute
+-------------------------------+-----------------------------------+
| `MinValue`                    | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as min html input attribute
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.NumberField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Password

This field is suitable for submitting passwords or other data that needs to stay encrypted in the database. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `MaxLength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Maximum length of the password allowed
+-------------------------------+-----------------------------------+
| `MinLength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Minimum length of the password allowed
+-------------------------------+-----------------------------------+
| `Encrypted`                   | *object type*: `List<SelectOption>`                         
|                               |         
|                               | *default value*: `TRUE`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Whether to store the password encrypted in the database
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.PasswordField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Percent

This will automatically present any number you enter as a percent value. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DecimalPlaces`               | *object type*: `byte?`                         
|                               |         
|                               | *default value*: `2`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | To how many decimal places the presented value should be rounded.
+-------------------------------+-----------------------------------+
| `DefaultValue`                | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `MaxValue`                    | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as max html input attribute
+-------------------------------+-----------------------------------+
| `MinValue`                    | *object type*: `decimal?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as min html input attribute
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.PercentField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Phone

Will allow only valid phone numbers to be submitted. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `MaxLength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Maximum length of the password allowed
+-------------------------------+-----------------------------------+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.PhoneField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Select

One value can be selected from a provided list. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `Options`                     | *object type*: `List<SelectOption>`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Options that will be presented to the user for selection.
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.SelectField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Text

A simple text field. One of the most needed field nevertheless. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `MaxLength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Minimum length of the password allowed
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.TextField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+

## Url

A simple text field. One of the most needed field nevertheless. Specific properties:

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `DefaultValue`                | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Set as field value when upon record creation no value is provided.
+-------------------------------+-----------------------------------+
| `MaxLength`                   | *object type*: `int?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Minimum length of the password allowed
+-------------------------------+-----------------------------------+
| `OpenTargetInNewWindow`       | *object type*: `bool?`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | Whether to open the link in a new tab
+-------------------------------+-----------------------------------+
| `FieldType`                   | *object type*: `FieldType`                         
|                               |         
|                               | *default value*: `FieldType.UrlField`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only property. Returns the field's type
+-------------------------------+-----------------------------------+
