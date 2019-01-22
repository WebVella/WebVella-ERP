<!--{"sort_order":6, "name": "create-entity-relation", "label": "Create entity relation"}-->
# Create entity relation

## Service Side API

The API class that is needed for entity, entity meta and field creation is `EntityRelationManager` with its `Create` method. To initiate this, you need to be in `Administrator` role. You can create an entity with a code similar to:

```csharp
EntityRelation newRelationObject = null; // <<< Replace with an initialized object

EntityRelationResponse response = new EntityRelationManager().Create(newRelationObject);
```

## Web API

##### Authorization

To initiate this web request, you need to be in `Administrator` role.

##### HTTP request
```http
POST https://<YOUR_DOMAIN>/api/v3/en_US/meta/relation
```

##### Query parameters

No query parameters are required with this method.

##### Request body

You need to post a `EntityRelation` object as a request body.

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

##### Step 4: Select the "Relations" tab

##### Step 5: Press the "Create Relation" button

![Field Create](/doc-images/sdk-entity-relation-create.png)

##### Step 6: Press the "Create Relation" button