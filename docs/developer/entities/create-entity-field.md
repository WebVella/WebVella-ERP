<!--{"sort_order":4, "name": "create-entity-field", "label": "Create entity field"}-->
# Create entity field

## Service Side API

The API class that is needed for entity, entity meta and field creation is `EntityManager` with its `CreateField` method. To initiate this, you need to be in `Administrator` role. You can create an entity with a code similar to:

```csharp
Guid entityId = Guid.Empty; // <<< Replace with the target entity Id
InputField newFieldObject = null; // <<< Replace with an initialized object

FieldResponse response = new EntityManager().CreateField(entityId, newFieldObject);
```

## Web API

##### Authorization

To initiate this web request, you need to be in `Administrator` role.

##### HTTP request
```http
POST https://<YOUR_DOMAIN>/api/v3/en_US/meta/entity/{Id}/field
```

##### Query parameters

+-------------------------------+-----------------------------------+
| name                          | description                       |
+===============================+===================================+
| `Id`                          | *object type*: `Guid`                         
|                               |         
|                               | *default value*: `NULL`
|                               |
|                               | *is required*: `TRUE`                      
|                               |                                   
|                               | Id of the target entity
+-------------------------------+-----------------------------------+

##### Request body

You need to post a `InputField` object as a request body.

##### Request response

If successful, this method returns a response JSON with the following structure:

```json
{
  "timestamp": "2014-03-03T23:20:23Z",
  "success": false,
  "message": "Aliqua anim consequat amet cupidatat proident amet amet.",
  "errors": [
    {
      "key": "fieldName",
      "value": "evaluated value",
      "message": "Error message"
    }
  ],
  "object": {
	/// The newly created entity
  }
}
```

## Web interface

**Important**: This page describes a process of creating an entity using the `WebVella SDK Plugin` web interface

##### Step 1: Navigate to the SDK Application

##### Step 2: Select from the Objects top menu -> Entities

##### Step 3: Select the target entity from the list

##### Step 4: Select the "Fields" tab

##### Step 5: Press the "Create Field" button

##### Step 6: Select the field type you need to create

![Field Create](/doc-images/sdk-entity-field-create.png)

##### Step 6: Press the "Create Field" button