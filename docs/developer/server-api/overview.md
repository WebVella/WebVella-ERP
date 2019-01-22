<!--{"sort_order":1, "name": "overview", "label": "Overview"}-->
# Overview

The server API is implemented by the following classes

## EntityManager

Entity meta and entity field related operations. **Important:** Requires `Administration` role

```csharp
Entity entity = new EntityManager().ReadEntity("user").Object;
```

## EntityRelationManager

Entity relations operation. **Important:** Requires `Administration` role

```csharp
List<EntityRelation> relationList = new EntityRelationManager().Read(storageEntityList).Object;
```

## RecordManager

Operations with entity records. **Important:** The access depends on the preferences selected in the corresponding entity

```csharp
var createResponse = new RecordManager().CreateRecord("offer", PostObject);
```

## SecurityManager

```csharp
var user = new SecurityManager().GetUser(userId);
```

User and User role related operations