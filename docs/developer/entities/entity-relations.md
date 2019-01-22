<!--{"sort_order":5, "name": "entity-relations", "label": "Entity Relations"}-->
# Entity Relations

Multiple relations can exists for each entity. They can be either "1:N" (one to many) or "N:N" (many to many). By establishing such relations you can:

* select easy related data
* put database constrains to ensure proper relation

## Properties

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `Id`                          | *object type*: `Guid`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Unique key for identifying the relation
+-------------------------------+-----------------------------------+
| `Name`                        | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Human readable identification of the relation. Used in EQL selections by adding "$" prefix
+-------------------------------+-----------------------------------+
| `OriginEntityId`              | *object type*: `Guid`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | The Id of the entity which originates the relation
+-------------------------------+-----------------------------------+
| `OriginFieldId`               | *object type*: `Guid`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | The Id of the field in the entity which originates the relation
+-------------------------------+-----------------------------------+
| `OriginEntityName`            | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only. Convenience property. The name of the entity which originates the relation
+-------------------------------+-----------------------------------+
| `OriginFieldName`             | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only. Convenience property. The name of the field in the entity which originates the relation
+-------------------------------+-----------------------------------+
| `RelationType`                | *object type*: `EntityRelationType`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Sets the relation type. Options: OneToMany, ManyToMany
+-------------------------------+-----------------------------------+
| `System`                      | *object type*: `bool`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `FALSE`                      
|                               |                                   
|                               | When creating an application, you will need to define a schema that you need to be managed only by the corresponding plugin and not the administrator. By marking the relation as System, the interface will lock certain changes.
+-------------------------------+-----------------------------------+
| `TargetEntityId`              | *object type*: `Guid`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | The Id of the entity which is the relation target
+-------------------------------+-----------------------------------+
| `TargetFieldId`               | *object type*: `Guid`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | The Id of the field in the entity which is the relation target
+-------------------------------+-----------------------------------+
| `TargetEntityName`            | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only. Convenience property. The name of the entity which is the relation target
+-------------------------------+-----------------------------------+
| `TargetFieldName`             | *object type*: `string`                         
|                               |         
|                               | *default value*: `Null`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | GET only. Convenience property. The name of the field in the entity which is the relation target
+-------------------------------+-----------------------------------+