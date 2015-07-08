using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using WebVella.ERP.Api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Storage;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using WebVella.ERP.Api.Models.AutoMapper;


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
			return DoResponse(entityManager.ReadEntities());
		}

		// Get entity meta
		// GET: api/v1/en_US/meta/entity/{name}/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}")]
		public IActionResult GetEntityMeta(string Name)
		{
			return DoResponse(entityManager.ReadEntity(Name));
		}


		// Create an entity
		// POST: api/v1/en_US/meta/entity
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
		public IActionResult CreateEntity([FromBody]InputEntity submitObj)
		{
			return DoResponse(entityManager.CreateEntity(submitObj));
		}

		// Create an entity
		// POST: api/v1/en_US/meta/entity
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{StringId}")]
		public IActionResult PatchEntity(string StringId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();
			InputEntity entity = new InputEntity();

			try
			{
			Guid entityId;
			if (!Guid.TryParse(StringId, out entityId))
			{
				response.Errors.Add(new ErrorModel("id", StringId, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

				IStorageEntity storageEntity = Storage.GetEntityRepository().Read(entityId);
				if (storageEntity == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				entity = storageEntity.MapTo<Entity>().MapTo<InputEntity>();

				Type inputEntityType = entity.GetType();

			foreach (var prop in submitObj.Properties())
			{
				int count = inputEntityType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

				InputEntity inputEntity = submitObj.ToObject<InputEntity>();

				foreach (var prop in submitObj.Properties())
			{
					if (prop.Name.ToLower() == "label")
						entity.Label = inputEntity.Label;
					if (prop.Name.ToLower() == "labelplural")
						entity.LabelPlural = inputEntity.LabelPlural;
					if (prop.Name.ToLower() == "system")
						entity.System = inputEntity.System;
					if (prop.Name.ToLower() == "iconname")
						entity.IconName = inputEntity.IconName;
					if (prop.Name.ToLower() == "weight")
						entity.Weight = inputEntity.Weight;
					if (prop.Name.ToLower() == "recordpermissions")
						entity.RecordPermissions = inputEntity.RecordPermissions;
			}
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.UpdateEntity(entity));
		}


		// Delete an entity
		// DELETE: api/v1/en_US/meta/entity/{id}
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{StringId}")]
		public IActionResult DeleteEntity(string StringId)
		{
			EntityResponse response = new EntityResponse();

			// Parse each string representation.
			Guid newGuid;
			Guid id = Guid.Empty;
			if (Guid.TryParse(StringId, out newGuid))
			{
				response = entityManager.DeleteEntity(newGuid);
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

			return DoResponse(entityManager.CreateField(entityId, field));
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
			FieldType fieldType = FieldType.GuidField;

			var fieldTypeProp = submitObj.Properties().SingleOrDefault(k => k.Name.ToLower() == "fieldtype");
			if (fieldTypeProp != null)
			{
				fieldType = (FieldType)Enum.ToObject(typeof(FieldType), fieldTypeProp.Value.ToObject<int>());
			}

			Type inputFieldType = InputField.GetFieldType(fieldType);

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

			return DoResponse(entityManager.UpdateField(entityId, field));
		}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Id}/field/{FieldId}")]
		public IActionResult PatchField(string Id, string FieldId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();
			Entity entity = new Entity();
			InputField field = new InputGuidField();

			try
			{
			Guid entityId;
			if (!Guid.TryParse(Id, out entityId))
			{
					response.Errors.Add(new ErrorModel("Id", Id, "id parameter is not valid Guid value"));
					return DoBadRequestResponse(response, "Field was not updated!");
			}

			Guid fieldId;
			if (!Guid.TryParse(FieldId, out fieldId))
			{
					response.Errors.Add(new ErrorModel("FieldId", FieldId, "FieldId parameter is not valid Guid value"));
					return DoBadRequestResponse(response, "Field was not updated!");
			}

				IStorageEntity storageEntity = Storage.GetEntityRepository().Read(entityId);
				if (storageEntity == null)
				{
					response.Errors.Add(new ErrorModel("Id", Id, "Entity with such Id does not exist!"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}
				entity = storageEntity.MapTo<Entity>();

				Field updatedField = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
				if (updatedField == null)
				{
					response.Errors.Add(new ErrorModel("FieldId", FieldId, "Field with such Id does not exist!"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}

			FieldType fieldType = FieldType.GuidField;

			var fieldTypeProp = submitObj.Properties().SingleOrDefault(k => k.Name.ToLower() == "fieldtype");
			if (fieldTypeProp != null)
			{
				fieldType = (FieldType)Enum.ToObject(typeof(FieldType), fieldTypeProp.Value.ToObject<int>());
			}
				else
				{
					response.Errors.Add(new ErrorModel("fieldType", null, "fieldType is required!"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}

			Type inputFieldType = InputField.GetFieldType(fieldType);
			foreach (var prop in submitObj.Properties())
			{
				int count = inputFieldType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

				InputField inputField = InputField.ConvertField(submitObj);

				foreach (var prop in submitObj.Properties())
				{
					switch (fieldType)
					{
						case FieldType.AutoNumberField:
							{
								field = new InputAutoNumberField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputAutoNumberField)field).DefaultValue = ((InputAutoNumberField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "U")
									((InputAutoNumberField)field).DisplayFormat = ((InputAutoNumberField)inputField).DisplayFormat;
								if (prop.Name.ToLower() == "startingnumber")
									((InputAutoNumberField)field).StartingNumber = ((InputAutoNumberField)inputField).StartingNumber;
							}
							break;
						case FieldType.CheckboxField:
							{
								field = new InputCheckboxField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputCheckboxField)field).DefaultValue = ((InputCheckboxField)inputField).DefaultValue;
							}
							break;
						case FieldType.CurrencyField:
							{
								field = new InputCurrencyField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputCurrencyField)field).DefaultValue = ((InputCurrencyField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "minvalue")
									((InputCurrencyField)field).MinValue = ((InputCurrencyField)inputField).MinValue;
								if (prop.Name.ToLower() == "maxvalue")
									((InputCurrencyField)field).MaxValue = ((InputCurrencyField)inputField).MaxValue;
								if (prop.Name.ToLower() == "currency")
									((InputCurrencyField)field).Currency = ((InputCurrencyField)inputField).Currency;
							}
							break;
						case FieldType.DateField:
							{
								field = new InputDateField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputDateField)field).DefaultValue = ((InputDateField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "format")
									((InputDateField)field).Format = ((InputDateField)inputField).Format;
								if (prop.Name.ToLower() == "usecurrenttimeasdefaultvalue")
									((InputDateField)field).UseCurrentTimeAsDefaultValue = ((InputDateField)inputField).UseCurrentTimeAsDefaultValue;
							}
							break;
						case FieldType.DateTimeField:
							{
								field = new InputDateTimeField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputDateTimeField)field).DefaultValue = ((InputDateTimeField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "format")
									((InputDateTimeField)field).Format = ((InputDateTimeField)inputField).Format;
								if (prop.Name.ToLower() == "usecurrenttimeasdefaultvalue")
									((InputDateTimeField)field).UseCurrentTimeAsDefaultValue = ((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue;
							}
							break;
						case FieldType.EmailField:
							{
								field = new InputEmailField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputEmailField)field).DefaultValue = ((InputEmailField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputEmailField)field).MaxLength = ((InputEmailField)inputField).MaxLength;
							}
							break;
						case FieldType.FileField:
							{
								field = new InputFileField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputFileField)field).DefaultValue = ((InputFileField)inputField).DefaultValue;
							}
							break;
						case FieldType.HtmlField:
							{
								field = new InputHtmlField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputHtmlField)field).DefaultValue = ((InputHtmlField)inputField).DefaultValue;
							}
							break;
						case FieldType.ImageField:
							{
								field = new InputImageField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputImageField)field).DefaultValue = ((InputImageField)inputField).DefaultValue;
							}
							break;
						case FieldType.MultiLineTextField:
							{
								field = new InputMultiLineTextField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputMultiLineTextField)field).DefaultValue = ((InputMultiLineTextField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputMultiLineTextField)field).MaxLength = ((InputMultiLineTextField)inputField).MaxLength;
								if (prop.Name.ToLower() == "visiblelinenumber")
									((InputMultiLineTextField)field).VisibleLineNumber = ((InputMultiLineTextField)inputField).VisibleLineNumber;
							}
							break;
						case FieldType.MultiSelectField:
							{
								field = new InputMultiSelectField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputMultiSelectField)field).DefaultValue = ((InputMultiSelectField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "options")
									((InputMultiSelectField)field).Options = ((InputMultiSelectField)inputField).Options;
							}
							break;
						case FieldType.NumberField:
							{
								field = new InputNumberField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputNumberField)field).DefaultValue = ((InputNumberField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "minvalue")
									((InputNumberField)field).MinValue = ((InputNumberField)inputField).MinValue;
								if (prop.Name.ToLower() == "maxvalue")
									((InputNumberField)field).MaxValue = ((InputNumberField)inputField).MaxValue;
								if (prop.Name.ToLower() == "decimalplaces")
									((InputNumberField)field).DecimalPlaces = ((InputNumberField)inputField).DecimalPlaces;
							}
							break;
						case FieldType.PasswordField:
							{
								field = new InputPasswordField();
								if (prop.Name.ToLower() == "maxlength")
									((InputPasswordField)field).MaxLength = ((InputPasswordField)inputField).MaxLength;
								if (prop.Name.ToLower() == "minlength")
									((InputPasswordField)field).MinLength = ((InputPasswordField)inputField).MinLength;
								if (prop.Name.ToLower() == "encrypted")
									((InputPasswordField)field).Encrypted = ((InputPasswordField)inputField).Encrypted;
							}
							break;
						case FieldType.PercentField:
							{
								field = new InputPercentField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputPercentField)field).DefaultValue = ((InputPercentField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "minvalue")
									((InputPercentField)field).MinValue = ((InputPercentField)inputField).MinValue;
								if (prop.Name.ToLower() == "maxvalue")
									((InputPercentField)field).MaxValue = ((InputPercentField)inputField).MaxValue;
								if (prop.Name.ToLower() == "decimalplaces")
									((InputPercentField)field).DecimalPlaces = ((InputPercentField)inputField).DecimalPlaces;
							}
							break;
						case FieldType.PhoneField:
							{
								field = new InputPhoneField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputPhoneField)field).DefaultValue = ((InputPhoneField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "format")
									((InputPhoneField)field).Format = ((InputPhoneField)inputField).Format;
								if (prop.Name.ToLower() == "maxlength")
									((InputPhoneField)field).MaxLength = ((InputPhoneField)inputField).MaxLength;
							}
							break;
						case FieldType.GuidField:
							{
								field = new InputGuidField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputGuidField)field).DefaultValue = ((InputGuidField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "generatenewid")
									((InputGuidField)field).GenerateNewId = ((InputGuidField)inputField).GenerateNewId;
							}
							break;
						case FieldType.SelectField:
							{
								field = new InputSelectField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputSelectField)field).DefaultValue = ((InputSelectField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "options")
									((InputSelectField)field).Options = ((InputSelectField)inputField).Options;
							}
							break;
						case FieldType.TextField:
							{
								field = new InputTextField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputTextField)field).DefaultValue = ((InputTextField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputTextField)field).MaxLength = ((InputTextField)inputField).MaxLength;
							}
							break;
						case FieldType.UrlField:
			{
								field = new InputUrlField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputUrlField)field).DefaultValue = ((InputUrlField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputUrlField)field).MaxLength = ((InputUrlField)inputField).MaxLength;
								if (prop.Name.ToLower() == "opentargetinnewwindow")
									((InputUrlField)field).OpenTargetInNewWindow = ((InputUrlField)inputField).OpenTargetInNewWindow;
							}
							break;
					}

					if (prop.Name.ToLower() == "label")
						field.Label = inputField.Label;
					else if (prop.Name.ToLower() == "placeholdertext")
						field.PlaceholderText = inputField.PlaceholderText;
					else if (prop.Name.ToLower() == "description")
						field.Description = inputField.Description;
					else if (prop.Name.ToLower() == "helptext")
						field.HelpText = inputField.HelpText;
					else if (prop.Name.ToLower() == "required")
						field.Required = inputField.Required;
					else if (prop.Name.ToLower() == "unique")
						field.Unique = inputField.Unique;
					else if (prop.Name.ToLower() == "searchable")
						field.Searchable = inputField.Searchable;
					else if (prop.Name.ToLower() == "auditable")
						field.Auditable = inputField.Auditable;
					else if (prop.Name.ToLower() == "system")
						field.System = inputField.System;
				}
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.UpdateField(entity, field));
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

			return DoResponse(entityManager.DeleteField(entityId, fieldId));
		}

		#endregion

		#region << Record Lists >>

		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity/{Name}/list")]
		public IActionResult CreateRecordListByName(string Name, [FromBody]JObject submitObj)
		{
			RecordListResponse response = new RecordListResponse();

			InputRecordList list = new InputRecordList();
			try
			{
				list = InputRecordList.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.CreateRecordList(Name, list));
		}

		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}")]
		public IActionResult UpdateRecordListByName(string Name, string ListName, [FromBody]JObject submitObj)
		{
			RecordListResponse response = new RecordListResponse();

			InputRecordList list = new InputRecordList();

			Type inputViewType = list.GetType();

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
				list = InputRecordList.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.UpdateRecordList(Name, list));
		}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}")]
		public IActionResult PatchRecordListByName(string Name, string ListName, [FromBody]JObject submitObj)
		{
			RecordListResponse response = new RecordListResponse();
			Entity entity = new Entity();
            InputRecordList list = new InputRecordList();

			try
			{
				IStorageEntity storageEntity = Storage.GetEntityRepository().Read(Name);
				if (storageEntity == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				entity = storageEntity.MapTo<Entity>();

				RecordList updatedList = entity.RecordLists.FirstOrDefault(l => l.Name == ListName);
				if (updatedList == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "List with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				list = updatedList.MapTo<InputRecordList>();

				Type inputListType = list.GetType();

				foreach (var prop in submitObj.Properties())
				{
					int count = inputListType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
					if (count < 1)
						response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
				}

				if (response.Errors.Count > 0)
					return DoBadRequestResponse(response);

				InputRecordList inputList = InputRecordList.Convert(submitObj);

				foreach (var prop in submitObj.Properties())
					{

						if (prop.Name.ToLower() == "label")
							list.Label = inputList.Label;
						if (prop.Name.ToLower() == "default")
							list.Default = inputList.Default;
						if (prop.Name.ToLower() == "system")
							list.System = inputList.System;
						if (prop.Name.ToLower() == "weight")
							list.Weight = inputList.Weight;
						if (prop.Name.ToLower() == "cssclass")
							list.CssClass = inputList.CssClass;
						if (prop.Name.ToLower() == "type")
							list.Type = inputList.Type;
						if (prop.Name.ToLower() == "recordslimit")
							list.RecordsLimit = inputList.RecordsLimit;
						if (prop.Name.ToLower() == "pagesize")
							list.PageSize = inputList.PageSize;
						if (prop.Name.ToLower() == "columns")
							list.Columns = inputList.Columns;
						if (prop.Name.ToLower() == "query")
							list.Query = inputList.Query;
						if (prop.Name.ToLower() == "sorts")
							list.Sorts = inputList.Sorts;
					}
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.UpdateRecordList(entity, list));
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}")]
		public IActionResult DeleteRecordListByName(string Name, string ListName)
		{
			return DoResponse(entityManager.DeleteRecordList(Name, ListName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}")]
		public IActionResult GetRecordListByName(string Name, string ListName)
		{
			return DoResponse(entityManager.ReadRecordList(Name, ListName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/list")]
		public IActionResult GetRecordListsByName(string Name)
		{
			return DoResponse(entityManager.ReadRecordLists(Name));
		}

		#endregion

		#region << Record Views >>

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{entityName}/getEntityViewLibrary")]
		public IActionResult GetEntityLibrary(string entityName)
		{
			var result = new EntityLibraryItemsResponse() { Success = true, Timestamp = DateTime.UtcNow };
			var relMan = new EntityRelationManager(service.StorageService);

			if (string.IsNullOrWhiteSpace(entityName))
			{
				result.Errors.Add(new ErrorModel { Message = "Invalid entity name." });
				result.Success = false;
				return DoResponse(result);
			}

			var entity = entityManager.ReadEntity(entityName).Object;
			if (entity == null)
			{
				result.Errors.Add(new ErrorModel { Message = "Entity not found." });
				result.Success = false;
				return DoResponse(result);
			}

			List<object> itemList = new List<object>();

			itemList.Add(new { type = "html", tag = "", content = "" });

			foreach (var field in entity.Fields)
			{
                itemList.Add(new RecordViewFieldItem
				{
                    FieldId = field.Id,
                    FieldName = field.Name,
                    FieldLabel = field.Label,
                    FieldTypeId = field.GetFieldType()
				});

			}

			foreach (var view in entity.RecordViews)
			{
                itemList.Add(new RecordViewViewItem
				{
                    ViewId = view.Id,
                    ViewName = view.Name,
                    ViewLabel = view.Label,
                    EntityId = entity.Id,
                    EntityName = entity.Name,
                    EntityLabel = entity.Label
				});
			}

			foreach (var list in entity.RecordLists)
			{
                itemList.Add(new RecordViewListItem
				{
                    ListId = list.Id,
                    ListName = list.Name,
                    ListLabel = list.Label,
                    EntityId = entity.Id,
                    EntityName = entity.Name,
                    EntityLabel = entity.Label,
                    EntityLabelPlural = entity.LabelPlural
				});
			}

			var relations = relMan.Read().Object;
			var entityRelations = relations.Where(x => x.OriginEntityId == entity.Id || x.TargetEntityId == entity.Id).ToList();

			foreach (var relation in entityRelations)
			{
				Guid relatedEntityId = relation.OriginEntityId == entity.Id ? relation.TargetEntityId : relation.OriginEntityId;
				Entity relatedEntity = entityManager.ReadEntity(relatedEntityId).Object;

				//TODO validation
				if (relatedEntity == null)
					throw new Exception(string.Format("Invalid relation '{0}'. Related entity '{1}' do not exist.", relation.Name, relatedEntityId));

				foreach (var field in relatedEntity.Fields)
				{
                    itemList.Add(new RecordViewRelationFieldItem
                    {
                        RelationId = relation.Id,
                        EntityId = relatedEntity.Id,
                        EntityName = relatedEntity.Name,
                        EntityLabel = relatedEntity.Label,
                        FieldId = field.Id,
                        FieldName = field.Name,
                        FieldLabel = field.Label,
                        FieldTypeId = field.GetFieldType()
                    });
                }

                foreach (var view in relatedEntity.RecordViews )
                {
                    itemList.Add(new RecordViewRelationViewItem
                    {
                        RelationId = relation.Id,
                        EntityId = relatedEntity.Id,
                        EntityName = relatedEntity.Name,
                        EntityLabel = relatedEntity.Label,
                        ViewId = view.Id,
                        ViewName = view.Name,
                        ViewLabel = view.Label
                    });
                }

                foreach (var list in relatedEntity.RecordLists)
                {
                    itemList.Add(new RecordViewRelationListItem
					{
                        RelationId = relation.Id,
                        EntityId = relatedEntity.Id,
                        EntityName = relatedEntity.Name,
                        EntityLabel = relatedEntity.Label,
                        EntityLabelPlural = relatedEntity.LabelPlural,
                        ListId = list.Id,
                        ListName = list.Name,
                        ListLabel = list.Label,
                        
					});
				}
			}

			result.Object = itemList;

			return DoResponse(result);
		}

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

		//	return DoResponse(entityManager.CreateRecordView(Id, view));
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

			return DoResponse(entityManager.CreateRecordView(Name, view));
		}

		//[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		//public IActionResult UpdateRecordView(Guid Id, Guid ViewId, [FromBody]JObject submitObj)
		//{
		//    RecordViewResponse response = new RecordViewResponse();

		//    InputRecordView view = new InputRecordView();

		//    Type inputViewType = view.GetType();

		//    foreach (var prop in submitObj.Properties())
		//    {
		//        int count = inputViewType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
		//        if (count < 1)
		//            response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
		//    }

		//    if (response.Errors.Count > 0)
		//        return DoBadRequestResponse(response);

		//    try
		//    {
		//        view = InputRecordView.Convert(submitObj);
		//    }
		//    catch (Exception e)
		//    {
		//        return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
		//    }

		//    return DoResponse(entityManager.UpdateRecordView(Id, view));
		//}

		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult UpdateRecordViewByName(string Name, string ViewName, [FromBody]JObject submitObj)
		{
			RecordViewResponse response = new RecordViewResponse();

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

			return DoResponse(entityManager.UpdateRecordView(Name, view));
		}

		//[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		//public IActionResult PatchRecordView(Guid Id, Guid ViewId, [FromBody]JObject submitObj)
		//{
		//    RecordViewResponse response = new RecordViewResponse();

		//    InputRecordView view = new InputRecordView();

		//    Type inputViewType = view.GetType();

		//    foreach (var prop in submitObj.Properties())
		//    {
		//        int count = inputViewType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
		//        if (count < 1)
		//            response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
		//    }

		//    if (response.Errors.Count > 0)
		//        return DoBadRequestResponse(response);

		//    try
		//    {
		//        view = InputRecordView.Convert(submitObj);
		//    }
		//    catch (Exception e)
		//    {
		//        return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
		//    }

		//    return DoResponse(entityManager.PartialUpdateRecordView(Id, ViewId, view));
		//}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult PatchRecordViewByName(string Name, string ViewName, [FromBody]JObject submitObj)
		{
			RecordViewResponse response = new RecordViewResponse();
			Entity entity = new Entity();
			InputRecordView view = new InputRecordView();

			try
			{
				IStorageEntity storageEntity = Storage.GetEntityRepository().Read(Name);
				if (storageEntity == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				entity = storageEntity.MapTo<Entity>();

				RecordView updatedView = entity.RecordViews.FirstOrDefault(v => v.Name == ViewName);
				if (updatedView == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "View with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				view = updatedView.MapTo<InputRecordView>();

			Type inputViewType = view.GetType();
			foreach (var prop in submitObj.Properties())
			{
				int count = inputViewType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

				InputRecordView inputView = InputRecordView.Convert(submitObj);

				foreach (var prop in submitObj.Properties())
			{
					if (prop.Name.ToLower() == "label")
						view.Label = inputView.Label;
					if (prop.Name.ToLower() == "default")
						view.Default = inputView.Default;
					if (prop.Name.ToLower() == "system")
						view.System = inputView.System;
					if (prop.Name.ToLower() == "weight")
						view.Weight = inputView.Weight;
					if (prop.Name.ToLower() == "cssclass")
						view.CssClass = inputView.CssClass;
					if (prop.Name.ToLower() == "type")
						view.Type = inputView.Type;
					if (prop.Name.ToLower() == "regions")
						view.Regions = inputView.Regions;
					if (prop.Name.ToLower() == "sidebar")
						view.Sidebar = inputView.Sidebar;
				}
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.UpdateRecordView(entity, view));
		}

		//[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		//public IActionResult DeleteRecordView(Guid Id, Guid ViewId)
		//{
		//    return DoResponse(entityManager.DeleteRecordView(Id, ViewId));
		//}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult DeleteRecordViewByName(string Name, string ViewName)
		{
			return DoResponse(entityManager.DeleteRecordView(Name, ViewName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult GetRecordViewByName(string Name, string ViewName)
		{
			return DoResponse(entityManager.ReadRecordView(Name, ViewName));
		}

		//[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Id}/view")]
		//      public IActionResult GetRecordViews(Guid Id)
		//      {
		//          return DoResponse(entityManager.ReadRecordViews(Id));
		//      }

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view")]
		public IActionResult GetRecordViewsByName(string Name)
		{
			return DoResponse(entityManager.ReadRecordViews(Name));
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
					submitObj["id"] = Guid.NewGuid();
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
		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult GetRecord(Guid recordId, string entityName)
		{

			QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);

			EntityQuery query = new EntityQuery(entityName, "*", filterObj, null, null, null);

			QueryResponse result = recMan.Find(query);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}


		// Create an entity record
		// POST: api/v1/en_US/record/{entityName}
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/{entityName}")]
		public IActionResult CreateEntityRecord(string entityName, [FromBody]EntityRecord postObj)
		{
			if (string.IsNullOrEmpty((string)postObj["id"]))
				postObj["id"] = Guid.NewGuid();

			QueryResponse result = recMan.CreateRecord(entityName, postObj);
			return DoResponse(result);
		}

		// Update an entity record
		// PUT: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult UpdateEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			QueryResponse result = recMan.UpdateRecord(entityName, postObj);
			return DoResponse(result);
		}

		// Patch an entity record
		// PATCH: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult PatchEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			postObj["id"] = recordId;
			QueryResponse result = recMan.UpdateRecord(entityName, postObj);
			return DoResponse(result);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/list/{listName}/{filter}/{page}")]
		public IActionResult GetRecordsByEntityName(string entityName, string listName, string filter, int page)
		{
			if (page < 1)
				page = 1;
			int limit = 25;
			int skip = (page - 1) * limit;

			QuerySortObject sObj = new QuerySortObject("label", QuerySortType.Ascending);
			EntityQuery resultQuery = new EntityQuery(entityName, "*", null, new[] { sObj }, skip, limit);

			RecordListResponse listResponse = entityManager.ReadRecordList(entityName, listName);

			if (listResponse != null && listResponse.Object != null)
			{
				RecordList list = listResponse.Object;

				List<QuerySortObject> sortList = new List<QuerySortObject>();
				if (list.Sorts != null && list.Sorts.Count > 0)
				{
					foreach (var sort in list.Sorts)
					{
						QuerySortType sortType;
						if (Enum.TryParse<QuerySortType>(sort.SortType, out sortType))
						{
							QuerySortObject sortObj = new QuerySortObject(sort.FieldName, sortType);

							sortList.Add(sortObj);
						}
					}

					resultQuery.Sort = sortList.ToArray();
				}

				QueryObject queryObj = null;
				if (list.Query != null)
				{
					queryObj = GenerateQuery(list.Query);

					resultQuery.Query = queryObj;
				}

				string queryFields = null;
				if (list.Columns != null)
				{
					foreach (var column in list.Columns)
					{
						if (column is RecordListFieldItem)
						{
							queryFields += ((RecordListFieldItem)column).FieldName + ", ";
						}

						if (column is RecordListRelationFieldItem)
						{
							EntityRelationManager relManager = new EntityRelationManager(Storage);
							EntityRelationResponse relResponse = relManager.Read(((RecordListRelationFieldItem)column).RelationId);

							string relName = relResponse != null && relResponse.Object != null ? string.Format("${0}.", relResponse.Object.Name) : "";
							queryFields += string.Format("{0}{1}, ", relName, ((RecordListRelationFieldItem)column).FieldName);
						}
					}

					if (queryFields.EndsWith(", "))
						queryFields = queryFields.Remove(queryFields.Length - 2);

					resultQuery.Fields = queryFields;

				}

				if (list.RecordsLimit > 0)
					resultQuery.Limit = list.RecordsLimit;
			}

			QueryResponse result = recMan.Find(resultQuery);
			if (!result.Success)
				return DoResponse(result);

			return Json(result);
		}

		private QueryObject GenerateQuery(RecordListQuery query)
		{
			QueryObject queryObj = new QueryObject();

			QueryType type;
			if (Enum.TryParse<QueryType>(query.QueryType, out type))
			{
				queryObj.FieldName = query.FieldName;
				queryObj.FieldValue = query.FieldValue;
				queryObj.QueryType = type;

				if (query.SubQueries != null)
				{
					queryObj.SubQueries = new List<QueryObject>();
					foreach (var subQuery in query.SubQueries)
					{
						QueryObject subQueryObj = GenerateQuery(subQuery);
						queryObj.SubQueries.Add(subQueryObj);
					}
				}
			}
			return queryObj;
		}

		#endregion

		#region << Area Specific >>

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
			record["id"] = Guid.NewGuid();
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

			QueryObject filter = EntityQuery.QueryAND(EntityQuery.QueryEQ("area_id", areaId), EntityQuery.QueryEQ("entity_id", entityId));
			EntityQuery queryRelations = new EntityQuery("areas_entities", "*", filter, null, null, null);
			QueryResponse resultRelations = recMan.Find(queryRelations);
			if (!resultRelations.Success)
				return DoResponse(resultRelations);
			if (resultRelations.Object.Data != null && resultRelations.Object.Data.Any())
			{
				EntityRecord recordForDeletion = resultRelations.Object.Data.First();
				QueryResponse result = recMan.DeleteRecord("areas_entities", (Guid)recordForDeletion["id"]);
				if (!result.Success)
				{
					return DoResponse(result);
				}
				else
				{
					return Json(result);
				}

			}
			else
			{
				QueryResponse responseNotFound = new QueryResponse();
				responseNotFound.Success = false;
				responseNotFound.Message = "No relation was found between areaId: " + areaId + " and entity Id: " + entityId;
				return Json(responseNotFound);
			}
		}

		#endregion

		#region << Files >>

		[HttpGet]
		[Route("/fs/{*filepath}")]
		public IActionResult Download([FromRoute] string filepath)
		{
			//TODO  authorize
			if (string.IsNullOrWhiteSpace(filepath))
				return DoPageNotFoundResponse();

			if (!filepath.StartsWith("/"))
				filepath = "/" + filepath;

			filepath = filepath.ToLowerInvariant();

			var fs = service.StorageService.GetFS();
			var file = fs.Find(filepath);

			if (file == null)
				return DoPageNotFoundResponse();

			return File(file.GetBytes(), System.Net.Mime.MediaTypeNames.Application.Octet);
		}

		[AcceptVerbs(new[] { "POST" }, Route = "/fs/upload/")]
		public IActionResult UploadFile([FromForm] IFormFile file)
		{
			var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').ToLowerInvariant();
			var fs = service.StorageService.GetFS();
			var createdFile = fs.CreateTempFile(fileName, ReadFully(file.OpenReadStream()));

			return DoResponse(new FSResponse(new FSResult { Url = "/fs" + createdFile.FilePath, Filename = fileName }));
		}

		[AcceptVerbs(new[] { "POST" }, Route = "/fs/move/")]
		public IActionResult MoveFile([FromBody]JObject submitObj)
		{
			string source = submitObj["source"].Value<string>();
			string target = submitObj["target"].Value<string>();
			bool overwrite = false;
			if (submitObj["overwrite"] != null)
				overwrite = submitObj["overwrite"].Value<bool>();

			source = source.ToLowerInvariant();
			target = target.ToLowerInvariant();

			if (source.StartsWith("/fs/"))
				source = source.Substring(3);

			if (source.StartsWith("fs/"))
				source = source.Substring(2);

			if (target.StartsWith("/fs/"))
				target = target.Substring(3);

			if (target.StartsWith("fs/"))
				target = target.Substring(2);

			var fileName = target.Split(new char[] { '/' }).LastOrDefault();

			var fs = service.StorageService.GetFS();
			var sourceFile = fs.Find(source);

			var movedFile = fs.Move(source, target, overwrite);
			return DoResponse(new FSResponse(new FSResult { Url = "/fs" + movedFile.FilePath, Filename = fileName }));
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "{*filepath}")]
		public IActionResult DeleteFile([FromRoute] string filepath)
		{

			filepath = filepath.ToLowerInvariant();

			if (filepath.StartsWith("/fs/"))
				filepath = filepath.Substring(3);

			if (filepath.StartsWith("fs/"))
				filepath = filepath.Substring(2);

			var fileName = filepath.Split(new char[] { '/' }).LastOrDefault();

			var fs = service.StorageService.GetFS();
			var sourceFile = fs.Find(filepath);

			fs.Delete(filepath);
			return DoResponse(new FSResponse(new FSResult { Url = "/fs" + filepath, Filename = fileName }));
		}

		private static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		#endregion
	}
}

