using System;
using Microsoft.AspNet.Mvc;
using WebVella.ERP.Api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Storage;
using System.Collections.Generic;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebVella.ERP.Web.Controllers
{
	public class ApiController : ApiControllerBase
	{

		//TODO - add created_by and modified_by fields where needed, when the login is done
		RecordManager recMan;
		EntityManager entityManager;

		public IStorageService Storage { get; set; }
		public ApiController(IErpService service, IStorageService storage) : base(service)
		{
			Storage = storage;
			recMan = new RecordManager(service);
			entityManager = new EntityManager(storage);
		}


		#region << Entity Meta >>

		// Get all entity definitions
		// GET: api/v1/en_US/meta/entity/list/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/list")]
		public IActionResult GetEntityMetaList()
		{
			return DoResponse(new EntityManager(service.StorageService).ReadEntities());
		}

		// Get entity meta
		// GET: api/v1/en_US/meta/entity/{name}/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}")]
		public IActionResult GetEntityMeta(string Name)
		{
			return DoResponse(new EntityManager(service.StorageService).ReadEntity(Name));
		}


		// Create an entity
		// POST: api/v1/en_US/meta/entity
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
		public IActionResult CreateEntity([FromBody]InputEntity submitObj)
		{
			return DoResponse(new EntityManager(service.StorageService).CreateEntity(submitObj));
		}

		// Create an entity
		// POST: api/v1/en_US/meta/entity
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{StringId}")]
		public IActionResult PatchEntity(string StringId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			Guid entityId;
			if (!Guid.TryParse(StringId, out entityId))
			{
				response.Errors.Add(new ErrorModel("id", StringId, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			InputEntity inputEntity = new InputEntity();

			Type inputEntityType = inputEntity.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputEntityType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				inputEntity = submitObj.ToObject<InputEntity>();
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).PartialUpdateEntity(entityId, inputEntity));
		}


		// Delete an entity
		// DELETE: api/v1/en_US/meta/entity/{id}
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{StringId}")]
		public IActionResult DeleteEntity(string StringId)
		{
			EntityManager manager = new EntityManager(service.StorageService);
			EntityResponse response = new EntityResponse();

			// Parse each string representation.
			Guid newGuid;
			Guid id = Guid.Empty;
			if (Guid.TryParse(StringId, out newGuid))
			{
				response = manager.DeleteEntity(newGuid);
			}
			else
			{
				response.Success = false;
				response.Message = "The entity Id should be a valid Guid";
				Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			return DoResponse(response);
		}

		#endregion

		#region << Entity Fields >>

		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity/{Id}/field")]
		public IActionResult CreateField(string Id, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			Guid entityId;
			if (!Guid.TryParse(Id, out entityId))
			{
				response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			InputField field = new InputGuidField();
			try
			{
				field = InputField.ConvertField(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).CreateField(entityId, field));
		}

		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{Id}/field/{FieldId}")]
		public IActionResult UpdateField(string Id, string FieldId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			Guid entityId;
			if (!Guid.TryParse(Id, out entityId))
			{
				response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			Guid fieldId;
			if (!Guid.TryParse(FieldId, out fieldId))
			{
				response.Errors.Add(new ErrorModel("id", FieldId, "FieldId parameter is not valid Guid value"));
				return DoResponse(response);
			}

			InputField field = new InputGuidField();

			Type inputFieldType = field.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputFieldType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				field = InputField.ConvertField(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).UpdateField(entityId, field));
		}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Id}/field/{FieldId}")]
		public IActionResult PatchField(string Id, string FieldId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			Guid entityId;
			if (!Guid.TryParse(Id, out entityId))
			{
				response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			Guid fieldId;
			if (!Guid.TryParse(FieldId, out fieldId))
			{
				response.Errors.Add(new ErrorModel("id", FieldId, "FieldId parameter is not valid Guid value"));
				return DoResponse(response);
			}

			InputField field = new InputGuidField();

			Type inputFieldType = field.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputFieldType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				field = InputField.ConvertField(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).PartialUpdateField(entityId, fieldId, field));
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Id}/field/{FieldId}")]
		public IActionResult DeleteField(string Id, string FieldId)
		{
			FieldResponse response = new FieldResponse();

			Guid entityId;
			if (!Guid.TryParse(Id, out entityId))
			{
				response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			Guid fieldId;
			if (!Guid.TryParse(FieldId, out fieldId))
			{
				response.Errors.Add(new ErrorModel("id", FieldId, "FieldId parameter is not valid Guid value"));
				return DoResponse(response);
			}

			return DoResponse(new EntityManager(service.StorageService).DeleteField(entityId, fieldId));
		}

		#endregion

		#region << Record Lists >>

		#endregion

		#region << Record Views >>

		//[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity/{Id}/view")]
		//public IActionResult CreateRecordView(Guid Id, [FromBody]JObject submitObj)
		//{
		//	RecordViewResponse response = new RecordViewResponse();

		//	InputRecordView view = new InputRecordView();
		//	try
		//	{
		//		view = InputRecordView.Convert(submitObj);
		//	}
		//	catch (Exception e)
		//	{
		//		return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
		//	}

		//	return DoResponse(new EntityManager(service.StorageService).CreateRecordView(Id, view));
		//}

		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity/{Name}/view")]
		public IActionResult CreateRecordViewByName(string Name, [FromBody]JObject submitObj)
		{
			RecordViewResponse response = new RecordViewResponse();

			InputRecordView view = new InputRecordView();
			try
			{
				view = InputRecordView.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).CreateRecordView(Name, view));
		}

		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		public IActionResult UpdateRecordView(Guid Id, Guid ViewId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			InputRecordView view = new InputRecordView();

			Type inputViewType = view.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputViewType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				view = InputRecordView.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).UpdateRecordView(Id, view));
		}

		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult UpdateRecordViewByName(string Name, string ViewName, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			InputRecordView view = new InputRecordView();

			Type inputViewType = view.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputViewType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				view = InputRecordView.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).UpdateRecordView(Name, view));
		}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		public IActionResult PatchRecordView(Guid Id, Guid ViewId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			InputRecordView view = new InputRecordView();

			Type inputViewType = view.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputViewType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				view = InputRecordView.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).PartialUpdateRecordView(Id, ViewId, view));
		}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult PatchRecordViewByName(string Name, string ViewName, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			InputRecordView view = new InputRecordView();

			Type inputViewType = view.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputViewType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				view = InputRecordView.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(new EntityManager(service.StorageService).PartialUpdateRecordView(Name, ViewName, view));
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		public IActionResult DeleteRecordView(Guid Id, Guid ViewId)
		{
			return DoResponse(new EntityManager(service.StorageService).DeleteRecordView(Id, ViewId));
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult DeleteRecordViewByName(string Name, string ViewName)
		{
			return DoResponse(new EntityManager(service.StorageService).DeleteRecordView(Name, ViewName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		public IActionResult GetRecordView(Guid Id, Guid ViewId)
		{
			return DoResponse(new EntityManager(service.StorageService).ReadRecordView(Id, ViewId));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult GetRecordViewByName(string Name, string ViewName)
		{
			return DoResponse(new EntityManager(service.StorageService).ReadRecordView(Name, ViewName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Id}/view")]
		public IActionResult GetRecordViews(Guid Id)
		{
			return DoResponse(new EntityManager(service.StorageService).ReadRecordViews(Id));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view")]
		public IActionResult GetRecordViewsByName(string Name)
		{
			return DoResponse(new EntityManager(service.StorageService).ReadRecordViews(Name));
		}

		#endregion

		#region << Relation Meta >>
		// Get all entity relation definitions
		// GET: api/v1/en_US/meta/relation/list/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/relation/list")]
		public IActionResult GetEntityRelationMetaList()
		{
			return DoResponse(new EntityRelationManager(service.StorageService).Read());
		}

		// Get entity relation meta
		// GET: api/v1/en_US/meta/relation/{name}/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/relation/{name}")]
		public IActionResult GetEntityRelationMeta(string name)
		{
			return DoResponse(new EntityRelationManager(service.StorageService).Read(name));
		}


		// Create an entity relation
		// POST: api/v1/en_US/meta/relation
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/relation")]
		public IActionResult CreateEntityRelation([FromBody]JObject submitObj)
		{
			try
			{
				if (submitObj["id"].IsNullOrEmpty())
					submitObj["id"] = Guid.Empty;
				var relation = submitObj.ToObject<EntityRelation>();
				return DoResponse(new EntityRelationManager(service.StorageService).Create(relation));
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(new EntityRelationResponse(), null, e);
			}
		}

		// Update an entity relation
		// PUT: api/v1/en_US/meta/relation/id
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/relation/{RelationIdString}")]
		public IActionResult UpdateEntityRelation(string RelationIdString, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			Guid relationId;
			if (!Guid.TryParse(RelationIdString, out relationId))
			{
				response.Errors.Add(new ErrorModel("id", RelationIdString, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			try
			{
				var relation = submitObj.ToObject<EntityRelation>();
				return DoResponse(new EntityRelationManager(service.StorageService).Update(relation));
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(new EntityRelationResponse(), null, e);
			}
		}

		// Delete an entity relation
		// DELETE: api/v1/en_US/meta/relation/{idToken}
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/relation/{idToken}")]
		public IActionResult DeleteEntityRelation(string idToken)
		{
			Guid newGuid;
			Guid id = Guid.Empty;
			if (Guid.TryParse(idToken, out newGuid))
			{
				return DoResponse(new EntityRelationManager(service.StorageService).Delete(newGuid));
			}
			else
			{
				return DoBadRequestResponse(new EntityRelationResponse(), "The entity relation Id should be a valid Guid", null);
			}

		}

		#endregion

		#region << Records >>
		// Create an entity record
		// POST: api/v1/en_US/record/{entityName}
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/{entityName}")]
		public IActionResult CreateEntityRecord(string entityName, [FromBody]EntityRecord postObj)
		{
			QueryResponse result = recMan.CreateRecord(entityName, postObj);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}

		// Update an entity record
		// PUT: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult UpdateEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			QueryResponse result = recMan.UpdateRecord(entityName, postObj);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}

		// Patch an entity record
		// PATCH: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult PatchEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			QueryResponse result = recMan.UpdateRecord(entityName, postObj);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/list")]
		public IActionResult GetRecordsByEntityName(string entityName)
		{

			QuerySortObject sortObj = new QuerySortObject("label", QuerySortType.Ascending);

			EntityQuery query = new EntityQuery(entityName, "*", null, new[] { sortObj }, null, null);

			QueryResponse result = recMan.Find(query);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}



		#endregion

		//#region << Area Specific >>

		// Delete an area record
		// DELETE: api/v1/en_US/area/{recordId}
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/area/{recordId}")]
		public IActionResult DeleteAreaRecord(Guid recordId)
		{
			QueryResponse response = new QueryResponse();
			//Begin transaction
			var recRep = Storage.GetRecordRepository();
			var transaction = recRep.CreateTransaction();
			try
			{
				transaction.Begin();
				//Delete all relations in the areas_entities collection/entity
				List<EntityRecord> areasEntititesRelations = new List<EntityRecord>();
				QueryObject areasEntititesRelationsFilterObj = EntityQuery.QueryEQ("area_id", recordId);
				var areasEntititesRelationsQuery = new EntityQuery("areas_entities", "*", areasEntititesRelationsFilterObj, null, null, null);
				var areasEntititesRelationsResult = recMan.Find(areasEntititesRelationsQuery);
				if (!areasEntititesRelationsResult.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = areasEntititesRelationsResult.Message;
					transaction.Rollback();
					return Json(response);
				}
				if (areasEntititesRelationsResult.Object.Data != null && areasEntititesRelationsResult.Object.Data.Any())
				{
					areasEntititesRelations = areasEntititesRelationsResult.Object.Data;
				}
				foreach (var relation in areasEntititesRelations)
				{
					var relationDeleteResult = recMan.DeleteRecord("areas_entities", (Guid)relation["id"]);
					if (!relationDeleteResult.Success)
					{
						response.Timestamp = DateTime.UtcNow;
						response.Success = false;
						response.Message = relationDeleteResult.Message;
						transaction.Rollback();
						return Json(response);
					}
				}

				//Delete the area
				var areaDeleteResult = recMan.DeleteRecord("area", recordId);
				if (!areaDeleteResult.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = areaDeleteResult.Message;
					transaction.Rollback();
					return Json(response);
				}

				transaction.Commit();
			}
			catch
			{
				transaction.Rollback();
				throw;
			}

			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Message = "Area successfully deleted";
			return DoResponse(response);
		}

		// Get all relations between area and entity by entity name
		// GET: api/v1/en_US/area/relations/entity/{entityName}
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/area/relations/entity/{entityId}")]
		public IActionResult GetAreaRelationsByEntityId(Guid entityId)
		{

			QueryObject areasRelationsFilterObj = EntityQuery.QueryEQ("entity_id", entityId);

			EntityQuery query = new EntityQuery("areas_entities", "*", areasRelationsFilterObj, null, null, null);

			QueryResponse result = recMan.Find(query);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}

		// Get all entities that has relation to an area
		// GET: api/v1/en_US/area/entity/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/sitemap")]
		public IActionResult GetSitemap()
		{
			var columnsNeeded = "id,name,label,color,icon_name,weight,roles,"
				+ "$areas_area_relation.entity_id";
			EntityQuery queryAreas = new EntityQuery("area", columnsNeeded, null, null, null, null);
			QueryResponse resultAreas = recMan.Find(queryAreas);
			if (!resultAreas.Success)
				return DoResponse(resultAreas);

			List<EntityRecord> areas = new List<EntityRecord>();
			List<EntityRecord> responseAreas = new List<EntityRecord>();
			if (resultAreas.Object.Data != null && resultAreas.Object.Data.Any())
			{
				areas = resultAreas.Object.Data;

				foreach (EntityRecord area in areas)
				{
					List<EntityRecord> areaEntities = new List<EntityRecord>();
					//area["entities"] = null;
					if (area["$areas_area_relation"] != null && ((List<EntityRecord>)area["$areas_area_relation"]).Any()) // Just in case
					{
						List<EntityRecord> areaEntityIds = (List<EntityRecord>)area["$areas_area_relation"];
						var entityColumnsNeeded = "id,name,label,icon_name,weight,recordLists,recordViews";
						foreach (var entityId in areaEntityIds)
						{
							EntityResponse entityResult = entityManager.ReadEntity((Guid)entityId["entity_id"]);
							if (!entityResult.Success)
								throw new Exception(entityResult.Message);

							EntityRecord entityObj = new EntityRecord();
							entityObj["id"] = entityResult.Object.Id;
							entityObj["name"] = entityResult.Object.Name;
							entityObj["label"] = entityResult.Object.Label;
							entityObj["label_plural"] = entityResult.Object.LabelPlural;
							entityObj["icon_name"] = entityResult.Object.IconName;
							entityObj["weight"] = entityResult.Object.Weight;
							entityObj["recordLists"] = entityResult.Object.RecordLists;
							entityObj["recordViews"] = entityResult.Object.RecordViews;
							areaEntities.Add(entityObj);
						}
						area["entities"] = areaEntities;
						responseAreas.Add(area);
					}

				}
			}

			var response = new QueryResponse();
			response.Success = true;
			response.Message = "Query successfully executed";
			if (responseAreas == new List<EntityRecord>())
			{
				response.Object.Data = null;
			}
			else
			{
				response.Object.Data = responseAreas;
			}
			return Json(response);
		}


		// Create an area entity relation
		// POST: api/v1/en_US/area/{areaId}/entity/{entityId}/relation
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/area/{areaId}/entity/{entityId}/relation")]
		public IActionResult CreateAreaEntityRelation(Guid areaId, Guid entityId)
		{
			EntityRecord record = new EntityRecord();
			record["id"] = null;
			record["area_id"] = areaId;
			record["entity_id"] = entityId;
			//TODO - created and modified by when we have the functionality
			QueryResponse result = recMan.CreateRecord("areas_entities", record);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}

		// Delete an area entity relation
		// DELETE: api/v1/en_US/area/{areaId}/entity/{entityId}/relation
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/area/{areaId}/entity/{entityId}/relation")]
		public IActionResult DeleteAreaEntityRelation(Guid areaId, Guid entityId)
		{

			QueryObject filter = EntityQuery.QueryAND(EntityQuery.QueryEQ("area_id", areaId),EntityQuery.QueryEQ("entity_id", entityId));
			EntityQuery queryRelations = new EntityQuery("areas_entities", "*", filter, null, null, null);
			QueryResponse resultRelations = recMan.Find(queryRelations);
			if (!resultRelations.Success)
				return DoResponse(resultRelations);
			if(resultRelations.Object.Data != null && resultRelations.Object.Data.Any()) {
				EntityRecord recordForDeletion = resultRelations.Object.Data.First();
			QueryResponse result = recMan.DeleteRecord("areas_entities", (Guid)recordForDeletion["id"]);
				if (!result.Success) {
					return DoResponse(result);
					}
				else {
					return Json(result);
				}

			}
			else {
				QueryResponse responseNotFound = new QueryResponse();
				responseNotFound.Success = false;
				responseNotFound.Message = "No relation was found between areaId: " + areaId + " and entity Id: " + entityId;
				return Json(responseNotFound);
				}
		}

		//#endregion


	}
}

