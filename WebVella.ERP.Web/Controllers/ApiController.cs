using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using WebVella.ERP.Api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Storage;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Web.Security;


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

        [AllowAnonymous]
        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/login")]
        public IActionResult Login([FromBody]JObject submitObj)
        {
            string email = (string)submitObj["email"];
            string password = (string)submitObj["password"];
            bool rememberMe = (bool)submitObj["rememberMe"];

            SecurityManager secMan = new SecurityManager(service);
            var user = secMan.GetUser(email, password);
            var responseObj = new ResponseModel();

            if (user != null)
            {
                if (user.Enabled == false)
                {
                    responseObj.Success = false;
                    var errorMsg = new ErrorModel();
                    errorMsg.Key = "Email";
                    errorMsg.Value = email;
                    errorMsg.Message = "User account is disabled.";
                    responseObj.Errors.Add(errorMsg);
                    responseObj.Object = new { token = "" };
                }
                else
                {
                    responseObj.Object = null;
                    responseObj.Success = true;
                    responseObj.Timestamp = DateTime.UtcNow;
                    responseObj.Object = new { token = WebSecurityUtil.Login(Context, user.Id, user.ModifiedOn, rememberMe, service) };
                }

            }
            else
            {
                responseObj.Success = false;
				responseObj.Message = "Login failed";
                var errorMsg = new ErrorModel();
                errorMsg.Key = "Email";
                errorMsg.Value = email;
                errorMsg.Message = "Invalid email or password";
                responseObj.Errors.Add(errorMsg);
                responseObj.Object = new { token = "" };
            }

            return DoResponse(responseObj);
        }

        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/logout")]
        public IActionResult Logout()
        {
            WebSecurityUtil.Logout(Context);
            var responseObj = new ResponseModel();
            responseObj.Object = null;
            responseObj.Success = true;
            responseObj.Timestamp = DateTime.UtcNow;
            responseObj.Object = null;
            return DoResponse(responseObj);
        }

        #region << Entity Meta >>

            // Get all entity definitions
            // GET: api/v1/en_US/meta/entity/list/
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/list")]
		public IActionResult GetEntityMetaList()
		{
			var bo = entityManager.ReadEntities();
			return DoResponse(bo);
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
					Meta = field,
					EntityId = entity.Id,
					EntityName = entity.Name,
					EntityLabel = entity.Label,
					EntityLabelPlural = entity.LabelPlural,
					DataName = field.Name
				});

			}

			foreach (var view in entity.RecordViews)
			{
				itemList.Add(new RecordViewViewItem
				{
					ViewId = view.Id,
					ViewName = view.Name,
					Meta = view,
					EntityId = entity.Id,
					EntityName = entity.Name,
					EntityLabel = entity.Label,
					EntityLabelPlural = entity.LabelPlural,
					DataName = string.Format("view{0}", view.Name)
				});
			}

			foreach (var list in entity.RecordLists)
			{
				itemList.Add(new RecordViewListItem
				{
					ListId = list.Id,
					ListName = list.Name,
					Meta = list,
					EntityId = entity.Id,
					EntityName = entity.Name,
					EntityLabel = entity.Label,
					EntityLabelPlural = entity.LabelPlural,
					DataName = string.Format("$list${0}", list.Name)
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
						RelationName = relation.Name,
						EntityId = relatedEntity.Id,
						EntityName = relatedEntity.Name,
						EntityLabel = relatedEntity.Label,
						EntityLabelPlural = relatedEntity.LabelPlural,
						FieldId = field.Id,
						FieldName = field.Name,
						Meta = field,
						DataName = string.Format("$field${0}${1}", relation.Name, field.Name)
					});
				}

				foreach (var view in relatedEntity.RecordViews)
				{
					itemList.Add(new RecordViewRelationViewItem
					{
						RelationId = relation.Id,
                        RelationName = relation.Name,
                        EntityId = relatedEntity.Id,
						EntityName = relatedEntity.Name,
						EntityLabel = relatedEntity.Label,
						EntityLabelPlural = relatedEntity.LabelPlural,
						ViewId = view.Id,
						ViewName = view.Name,
						Meta = view,
						DataName = string.Format("$view${0}${1}", relation.Name, view.Name)
					});
				}

				foreach (var list in relatedEntity.RecordLists)
				{
					itemList.Add(new RecordViewRelationListItem
					{
						RelationId = relation.Id,
                        RelationName = relation.Name,
                        EntityId = relatedEntity.Id,
						EntityName = relatedEntity.Name,
						EntityLabel = relatedEntity.Label,
						EntityLabelPlural = relatedEntity.LabelPlural,
						ListId = list.Id,
						ListName = list.Name,
						Meta = list,
						DataName = string.Format("$list${0}${1}", relation.Name, list.Name)

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
					if (prop.Name.ToLower() == "name")
						view.Name = inputView.Name;
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

        // Update an entity record relation records
        // POST: api/v1/en_US/record/relation
        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/relation")]
        public IActionResult UpdateEntityRelationRecord([FromBody]InputEntityRelationRecordUpdateModel model)
        {
            var recMan = new RecordManager(service);
            var entMan = new EntityManager(service.StorageService);
            BaseResponseModel response = new BaseResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

            if (model == null)
            { 
                response.Errors.Add(new ErrorModel { Message = "Invalid model." });
                response.Success = false;
                return DoResponse(response);
            }

            EntityRelation relation = null;
            if (string.IsNullOrWhiteSpace(model.RelationName))
            {
                response.Errors.Add(new ErrorModel { Message = "Invalid relation name.", Key = "relationName" });
                response.Success = false;
                return DoResponse(response);
            }
            else
            {
                relation = new EntityRelationManager(service.StorageService).Read(model.RelationName).Object;
                if (relation == null)
                {
                    response.Errors.Add(new ErrorModel { Message = "Invalid relation name. No relation with that name.", Key = "relationName" });
                    response.Success = false;
                    return DoResponse(response);
                }
            }

            var originEntity = entMan.ReadEntity(relation.OriginEntityId).Object;
            var targetEntity = entMan.ReadEntity(relation.TargetEntityId).Object;
            var originField = originEntity.Fields.Single(x => x.Id == relation.OriginFieldId);
            var targetField = targetEntity.Fields.Single(x => x.Id == relation.TargetFieldId);

            if (model.DetachTargetFieldRecordIds != null && model.DetachTargetFieldRecordIds.Any() && targetField.Required && relation.RelationType != EntityRelationType.ManyToMany)
            {
                response.Errors.Add(new ErrorModel { Message = "Cannot detach records, when target field is required.", Key = "originFieldRecordId" });
                response.Success = false;
                return DoResponse(response);
            }

            EntityQuery query = new EntityQuery(originEntity.Name, "*", EntityQuery.QueryEQ("id", model.OriginFieldRecordId), null, null, null);
            QueryResponse result = recMan.Find(query);
            if (result.Object.Data.Count == 0)
            {
                response.Errors.Add(new ErrorModel { Message = "Origin record was not found. Id=[" + model.OriginFieldRecordId + "]", Key = "originFieldRecordId" });
                response.Success = false;
                return DoResponse(response);
            }

            var originRecord = result.Object.Data[0];
            object originValue = originRecord[originField.Name];

            List<EntityRecord> attachTargetRecords = new List<EntityRecord>();
            List<EntityRecord> detachTargetRecords = new List<EntityRecord>();

            foreach (var targetId in model.AttachTargetFieldRecordIds)
            {
                query = new EntityQuery(targetEntity.Name, "*", EntityQuery.QueryEQ("id", targetId), null, null, null);
                result = recMan.Find(query);
                if (result.Object.Data.Count == 0)
                {
                    response.Errors.Add(new ErrorModel { Message = "Attach target record was not found. Id=[" + targetEntity + "]", Key = "targetRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                else if (attachTargetRecords.Any(x => (Guid)x["id"] == targetId))
                {
                    response.Errors.Add(new ErrorModel { Message = "Attach target id was duplicated. Id=[" + targetEntity + "]", Key = "targetRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                attachTargetRecords.Add(result.Object.Data[0]);
            }

            foreach (var targetId in model.DetachTargetFieldRecordIds)
            {
                query = new EntityQuery(targetEntity.Name, "*", EntityQuery.QueryEQ("id", targetId), null, null, null);
                result = recMan.Find(query);
                if (result.Object.Data.Count == 0)
                {
                    response.Errors.Add(new ErrorModel { Message = "Detach target record was not found. Id=[" + targetEntity + "]", Key = "targetRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                else if (attachTargetRecords.Any(x => (Guid)x["id"] == targetId))
                {
                    response.Errors.Add(new ErrorModel { Message = "Detach target id was duplicated. Id=[" + targetEntity + "]", Key = "targetRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                detachTargetRecords.Add(result.Object.Data[0]);
            }

            var transaction = recMan.CreateTransaction();
            try
            {

                transaction.Begin();

                switch (relation.RelationType)
                {
                    case EntityRelationType.OneToOne:
                    case EntityRelationType.OneToMany:
                        {
                            foreach (var record in detachTargetRecords)
                            {
                                record[targetField.Name] = null;

                                var updResult = recMan.UpdateRecord(targetEntity, record);
                                if (!updResult.Success)
                                {
                                    response.Errors = updResult.Errors;
                                    response.Message = "Target record id=[" + record["id"] + "] detach operation failed.";
                                    response.Success = false;
                                    return DoResponse(response);
                                }
                            }

                            foreach (var record in attachTargetRecords)
                            {
                                record[targetField.Name] = originValue;

                                var updResult = recMan.UpdateRecord(targetEntity, record);
                                if (!updResult.Success)
                                {
                                    response.Errors = updResult.Errors;
                                    response.Message = "Target record id=[" + record["id"] + "] attach operation failed.";
                                    response.Success = false;
                                    return DoResponse(response);
                                }
                            }
                        }
                        break;
                    case EntityRelationType.ManyToMany:
                        {
                            foreach (var record in detachTargetRecords)
                            {
                                QueryResponse updResult = recMan.RemoveRelationManyToManyRecord(relation.Id, (Guid)originValue, (Guid)record[targetField.Name]);

                                if (!updResult.Success)
                                {
                                    response.Errors = updResult.Errors;
                                    response.Message = "Target record id=[" + record["id"] + "] detach operation failed.";
                                    response.Success = false;
                                    return DoResponse(response);
                                }
                            }

                            foreach (var record in attachTargetRecords)
                            {
                                QueryResponse updResult = recMan.CreateRelationManyToManyRecord(relation.Id, (Guid)originValue, (Guid)record[targetField.Name]);

                                if (!updResult.Success)
                                {
                                    response.Errors = updResult.Errors;
                                    response.Message = "Target record id=[" + record["id"] + "] attach  operation failed.";
                                    response.Success = false;
                                    return DoResponse(response);
                                }
                            }
                        }
                        break;
                    default:
                        throw new Exception("Not supported relation type");
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

                response.Success = false;
                response.Message = ex.Message;
                return DoResponse(response);
            }

            return DoResponse(response);
        }

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
		public IActionResult GetRecordsByEntityName(string entityName, string listName, int page, string filter = "all")
		{
			
			EntityListResponse entitiesResponse = entityManager.ReadEntities();
			List<Entity> entities = entitiesResponse.Object.Entities;

			RecordListRecordResponse response = new RecordListRecordResponse();
			response.Message = "Success";
			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Object = new RecordListRecord();

			Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

			if (entity == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
				return DoResponse(response);
			}

            bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Read, entity);
            if (!hasPermisstion)
            {
                response.Success = false;
                response.Message = "Trying to read records from entity '" + entity.Name + "' with no read access.";
                response.Errors.Add(new ErrorModel { Message = "Access denied." });
                return DoResponse(response);
            }

            response.Object.Data = GetListRecords(entities, entity, listName, page);

			RecordList list = entity.RecordLists.FirstOrDefault(l => l.Name == listName);
			if (list != null)
			{
				response.Object.Meta = list;
			}

			return DoResponse(response);
		}

		private List<EntityRecord> GetListRecords(List<Entity> entities, Entity entity, string listName, int? page = null, QueryObject queryObj = null)
		{
			EntityQuery resultQuery = new EntityQuery(entity.Name, "*", queryObj, null, null, null);

			EntityRelationManager relManager = new EntityRelationManager(Storage);
			EntityRelationListResponse relListResponse = relManager.Read();
			List<EntityRelation> relationList = new List<EntityRelation>();
			if (relListResponse.Object != null)
				relationList = relListResponse.Object;

			RecordList list = null;
			if (entity != null && entity.RecordLists != null)
				list = entity.RecordLists.FirstOrDefault(l => l.Name == listName);

			if (list != null)
			{
				List<QuerySortObject> sortList = new List<QuerySortObject>();
				if (list.Sorts != null && list.Sorts.Count > 0)
				{
					foreach (var sort in list.Sorts)
					{
                        QuerySortType sortType;
                        if (Enum.TryParse<QuerySortType>(sort.SortType, true, out sortType))
                            sortList.Add(new QuerySortObject(sort.FieldName, sortType));
                    }
					resultQuery.Sort = sortList.ToArray();
				}

				if (list.Query != null)
				{
					if (queryObj != null)
					{
						List<QueryObject> subQueries = new List<QueryObject>();
						subQueries.Add(RecordListQuery.ConvertQuery(list.Query));
						queryObj.SubQueries = subQueries;
					}
					else
						queryObj = RecordListQuery.ConvertQuery(list.Query);

					resultQuery.Query = queryObj;
				}

				string queryFields = "id,";
				if (list.Columns != null)
				{
					foreach (var column in list.Columns)
					{
						if (column is RecordListFieldItem)
						{
                            if (((RecordListFieldItem)column).Meta.Name != "id")
							    queryFields += ((RecordListFieldItem)column).Meta.Name + ", ";
						}
						else if (column is RecordListRelationFieldItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationFieldItem)column).RelationId);
							queryFields += string.Format("${0}.{1}, ", relation.Name, ((RecordListRelationFieldItem)column).Meta.Name);

                            //add ID field automatically if not added
                            if (!queryFields.Contains(string.Format("${0}.id", relation.Name)))
                                queryFields += string.Format("${0}.id,", relation.Name);

                            //always add origin field in query, its value may be required for relative view and list
                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            queryFields += field.Name + ", ";
                        }
						else if (column is RecordListListItem || column is RecordListViewItem)
						{
							if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
								queryFields += "id, ";
						}
						else if (column is RecordListRelationListItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationListItem)column).RelationId);

							string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							string queryFieldName = string.Format("{0}{1}, ", relName, relField.Name);

							if (!queryFields.Contains(queryFieldName))
								queryFields += queryFieldName;

                            //always add origin field in query, its value may be required for relative view and list
                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            queryFields += field.Name + ", ";
                        }
						else if (column is RecordListRelationViewItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationViewItem)column).RelationId);

							string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							string queryFieldName = string.Format("{0}{1}, ", relName, relField.Name);

							if (!queryFields.Contains(queryFieldName))
								queryFields += queryFieldName;

                            //always add origin field in query, its value may be required for relative view and list
                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            queryFields += field.Name + ", ";
                        }
					}

					if (queryFields.EndsWith(", "))
						queryFields = queryFields.Remove(queryFields.Length - 2);

					resultQuery.Fields = queryFields;

				}

                if (list.PageSize > 0)
                {
                    resultQuery.Limit = list.PageSize;
                    if (page != null && page > 0)
                        resultQuery.Skip = (page - 1) * resultQuery.Limit;
                }
                
			}

			List<EntityRecord> resultDataList = new List<EntityRecord>();

			QueryResponse result = recMan.Find(resultQuery);
			if (!result.Success)
				return resultDataList;

			if (list != null)
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
                    //always add id value
                    dataRecord["id"] = record["id"];

                    foreach (var column in list.Columns)
					{
						if (column is RecordListFieldItem)
						{
							dataRecord[column.DataName] = record[((RecordListFieldItem)column).FieldName];
                        }
						else if (column is RecordListRelationFieldItem)
						{
							string propName = string.Format("${0}", ((RecordListRelationFieldItem)column).RelationName);
                            List<EntityRecord> relFieldRecords = (List<EntityRecord>)record[propName];

                            string idDataName = "$field" + propName + "$id";
                            if (!dataRecord.Properties.ContainsKey(idDataName))
                            {
                                List<object> idFieldRecord = new List<object>();
                                if (relFieldRecords != null)
                                {
                                    foreach (var relFieldRecord in relFieldRecords)
                                        idFieldRecord.Add(relFieldRecord["id"]);
                                }
                                dataRecord[idDataName] = idFieldRecord;
                            }

                            List<object> resultFieldRecord = new List<object>();
							if (relFieldRecords != null)
							{
								foreach (var relFieldRecord in relFieldRecords)
								{
									resultFieldRecord.Add(relFieldRecord[((RecordListRelationFieldItem)column).FieldName]);
								}
							}
							dataRecord[column.DataName] = resultFieldRecord;

                          
                        }
						else if (column is RecordListListItem)
						{
							QueryObject subListQueryObj = new QueryObject();
							subListQueryObj.QueryType = QueryType.AND;
							subListQueryObj.SubQueries = new List<QueryObject>();
							subListQueryObj.SubQueries.Add(new QueryObject { FieldName = "id", FieldValue = record["id"], QueryType = QueryType.EQ });

							List<EntityRecord> subListResult = GetListRecords(entities, entity, ((RecordListListItem)column).ListName, queryObj: subListQueryObj);
							dataRecord[column.DataName] = subListResult;
						}
						else if (column is RecordListRelationListItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationListItem)column).RelationId);
							string relName = string.Format("${0}", relation.Name);

                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                            Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
                            Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                            Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                            QueryObject subListQueryObj = EntityQuery.QueryEQ(relField.Name, record[field.Name]);

                            List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordListRelationListItem)column).ListName, queryObj: subListQueryObj);
                            dataRecord[column.DataName] = subListResult;
                        }
						else if (column is RecordListViewItem)
						{
							List<EntityRecord> subViewResult = GetViewRecords(entities, entity, ((RecordListViewItem)column).ViewName, "id", record["id"]);
							dataRecord[column.DataName] = subViewResult;
						}
						else if (column is RecordListRelationViewItem)
						{
                            EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationViewItem)column).RelationId);
                            string relName = string.Format("${0}", relation.Name);

                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                            Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
                            Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                            Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                            List<EntityRecord> subViewResult = GetViewRecords(entities, relEntity, ((RecordListRelationViewItem)column).ViewName, relField.Name, record[field.Name]);
                            dataRecord[column.DataName] = subViewResult;

                        }
                    }

					resultDataList.Add(dataRecord);
				}
			}
			else
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
					foreach (var prop in record.Properties)
					{
						//string propName = "$field" + (prop.Key.StartsWith("$") ? prop.Key : "$" + prop.Key);
						string propName = prop.Key;
						dataRecord[propName] = record[prop.Key];
					}

					resultDataList.Add(dataRecord);
				}
			}

			return resultDataList;
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/view/{viewName}/{id}")]
		public IActionResult GetViewRecords(string entityName, string viewName, Guid id)
		{
			EntityListResponse entitiesResponse = entityManager.ReadEntities();
			List<Entity> entities = entitiesResponse.Object.Entities;

			RecordViewRecordResponse response = new RecordViewRecordResponse();
			response.Message = "Success";
			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Object = new RecordViewRecord();

			Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

			if (entity == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
				return DoResponse(response);
			}

			response.Object.Data = GetViewRecords(entities, entity, viewName, "id", id);

			RecordView view = entity.RecordViews.FirstOrDefault(v => v.Name == viewName);
			if (view != null)
			{
				response.Object.Meta = view;
			}

			return DoResponse(response);
		}

		private List<EntityRecord> GetViewRecords(List<Entity> entities, Entity entity, string viewName, string queryFieldName, object queryFieldValue)
		{
            EntityQuery resultQuery = new EntityQuery(entity.Name, "*", EntityQuery.QueryEQ(queryFieldName, queryFieldValue));

            EntityRelationManager relManager = new EntityRelationManager(Storage);
			EntityRelationListResponse relListResponse = relManager.Read();
			List<EntityRelation> relationList = new List<EntityRelation>();
			if (relListResponse.Object != null)
				relationList = relListResponse.Object;

			RecordView view = null;
			if (entity != null && entity.RecordViews != null)
				view = entity.RecordViews.FirstOrDefault(v => v.Name == viewName);

			List<EntityRecord> resultDataList = new List<EntityRecord>();

			string queryFields = "id,";

            //List<RecordViewItemBase> items = new List<RecordViewItemBase>();
            List<object> items = new List<object>();

			if (view != null)
			{

                if (view.Sidebar.Items.Any())
                    items.AddRange(view.Sidebar.Items);

				foreach (var region in view.Regions)
				{
					if (region.Sections == null)
						continue;

					foreach (var section in region.Sections)
					{
						if (section.Rows == null)
							continue;

						foreach (var row in section.Rows)
						{
							if (row.Columns == null)
								continue;

							foreach (var column in row.Columns)
							{
								if (column.Items != null && column.Items.Count > 0)
									items.AddRange(column.Items);
							}
						}
					}
				}

                foreach (var item in items)
				{
					if (item is RecordViewFieldItem)
					{
                        if (((RecordViewFieldItem)item).Meta.Name != "id")
						    queryFields += ((RecordViewFieldItem)item).Meta.Name;
					}
					else if (item is RecordViewRelationFieldItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationFieldItem)item).RelationId);
                                              
                        //add ID field automatically if not added
                        if (!queryFields.Contains(string.Format("${0}.id", relation.Name)))
                            queryFields += string.Format("${0}.id,", relation.Name);
                        
                        Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                        Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);

                        queryFields += field.Name + ", ";
                        queryFields += string.Format("${0}.{1}, ", relation.Name, ((RecordViewRelationFieldItem)item).Meta.Name);

                    }
                    else if (item is RecordViewListItem || item is RecordViewViewItem)
					{
						if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
							queryFields += "id";
					}
					else if (item is RecordViewRelationListItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationListItem)item).RelationId);

						string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

						Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
						Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                        string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

                        //always add origin field in query, its value may be required for relative view and list
                        Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                        Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                        queryFields += field.Name + ", ";

                    }
					else if (item is RecordViewRelationViewItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationViewItem)item).RelationId);

						string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

						Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
						Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                        string qFieldName = string.Format("{0}{1},", relName, relField.Name);

                        if (!queryFields.Contains(qFieldName))
                            queryFields += qFieldName;

                        //always add origin field in query, its value may be required for relative view and list
                        Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                        Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                        queryFields += field.Name + ", ";
                    }
                    else if (item is RecordViewSidebarViewItem)
                    {
                        //nothing to add, just check for record id
                        if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
                            queryFields += "id";
                    }
                    else if (item is RecordViewSidebarListItem)
                    {
                        //nothing to add, just check for record id
                        if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
                            queryFields += "id";
                    }
                    else if (item is RecordViewSidebarRelationListItem)
                    {
                        EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationListItem)item).RelationId);

                        string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

                        Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                        Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

                        Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                        Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                        string qFieldName = string.Format("{0}{1},", relName, relField.Name);

                        if (!queryFields.Contains(qFieldName))
                            queryFields += qFieldName;

                        //always add origin field in query, its value may be required for relative view and list
                        Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                        Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                        queryFields += field.Name + ", ";
                    }
                    else if (item is RecordViewSidebarRelationViewItem)
                    {
                        EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationViewItem)item).RelationId);

                        string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

                        Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                        Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

                        Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                        Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                        string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

                        //always add origin field in query, its value may be required for relative view and list
                        Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                        Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                        queryFields += field.Name + ", ";
                    }
                    
                    queryFields += ",";
                }

                queryFields = queryFields.Trim();
                if (queryFields.EndsWith(","))
					queryFields = queryFields.Remove(queryFields.Length - 1);

				resultQuery.Fields = queryFields;
			}

			QueryResponse result = recMan.Find(resultQuery);
			if (!result.Success)
				return resultDataList;

			if (view != null)
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
                    //always add id value
                    dataRecord["id"] = record["id"];

                    foreach (var item in items)
					{
						if (item is RecordViewFieldItem)
						{
                            dataRecord[((RecordViewFieldItem)item).DataName] = record[((RecordViewFieldItem)item).FieldName];
                        }
						else if (item is RecordViewListItem)
						{
                            var query = EntityQuery.QueryEQ("id", record["id"]);
                            List<EntityRecord> subListResult = GetListRecords(entities, entity, ((RecordViewListItem)item).ListName, queryObj: query);
                            dataRecord[((RecordViewListItem)item).DataName] = subListResult;
						}
						else if (item is RecordViewViewItem)
						{
							List<EntityRecord> subViewResult = GetViewRecords(entities, entity, ((RecordViewViewItem)item).ViewName, "id", record["id"]);
                            dataRecord[((RecordViewViewItem)item).DataName] = subViewResult;
						}
						else if (item is RecordViewRelationFieldItem)
						{
                            string propName = string.Format("${0}", ((RecordViewRelationFieldItem)item).RelationName);
                            List<EntityRecord> relFieldRecords = (List<EntityRecord>)record[propName];

                            string idDataName = "$field" + propName + "$id";
                            if (!dataRecord.Properties.ContainsKey(idDataName))
                            {
                                List<object> idFieldRecord = new List<object>();
                                if (relFieldRecords != null)
                                {
                                    foreach (var relFieldRecord in relFieldRecords)
                                        idFieldRecord.Add(relFieldRecord["id"]);
                                }
                                dataRecord[idDataName] = idFieldRecord;
                            }

							List<object> resultFieldRecord = new List<object>();
							if (relFieldRecords != null)
							{
								foreach (var relFieldRecord in relFieldRecords)
								{
									resultFieldRecord.Add(relFieldRecord[((RecordViewRelationFieldItem)item).FieldName]);
								}
							}
                            dataRecord[((RecordViewRelationFieldItem)item).DataName] = resultFieldRecord;
                        }
						else if (item is RecordViewRelationListItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationListItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                            Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
                            Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                            Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                            QueryObject subListQueryObj = EntityQuery.QueryEQ(relField.Name, record[field.Name]);

                            List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordViewRelationListItem)item).ListName, queryObj: subListQueryObj);
                            dataRecord[((RecordViewRelationListItem)item).DataName] = subListResult;
                        }
						else if (item is RecordViewRelationViewItem)
						{
                            EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationViewItem)item).RelationId);
                            string relName = string.Format("${0}", relation.Name);

                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                            Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
                            Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                            Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                            List<EntityRecord> subViewResult = GetViewRecords(entities, relEntity, ((RecordViewRelationViewItem)item).ViewName, relField.Name, record[field.Name]);
                            dataRecord[((RecordViewRelationViewItem)item).DataName] = subViewResult;
                        }
                        else if (item is RecordViewSidebarViewItem)
                        {
                            List<EntityRecord> subViewResult = GetViewRecords(entities, entity, ((RecordViewSidebarViewItem)item).ViewName, "id", record["id"]);
                            dataRecord[((RecordViewSidebarViewItem)item).DataName] = subViewResult;
                        }
                        else if (item is RecordViewSidebarListItem)
                        {
                            var query = EntityQuery.QueryEQ("id", record["id"]);
                            List<EntityRecord> subListResult = GetListRecords(entities, entity, ((RecordViewSidebarListItem)item).ListName, queryObj: query);
                            dataRecord[((RecordViewSidebarListItem)item).DataName] = subListResult;
                        }
                        else if (item is RecordViewSidebarRelationListItem)
                        {
                            EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationListItem)item).RelationId);
                            string relName = string.Format("${0}", relation.Name);

                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                            Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
                            Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                            Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                            QueryObject subListQueryObj = EntityQuery.QueryEQ(relField.Name, record[field.Name]);

                            List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordViewSidebarRelationListItem)item).ListName, queryObj: subListQueryObj);
                            dataRecord[((RecordViewSidebarRelationListItem)item).DataName] = subListResult;
                        }
                        else if (item is RecordViewSidebarRelationViewItem)
                        {
                            EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationViewItem)item).RelationId);
                            string relName = string.Format("${0}", relation.Name);

                            Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
                            Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
                            Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
                            Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
                            Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
                            Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

                            List<EntityRecord> subViewResult = GetViewRecords(entities, relEntity, ((RecordViewSidebarRelationViewItem)item).ViewName, relField.Name, record[field.Name]);
                            dataRecord[((RecordViewSidebarRelationViewItem)item).DataName] = subViewResult;
                        }
					}

					resultDataList.Add(dataRecord);
				}
			}
			else
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
					foreach (var prop in record.Properties)
					{
						//string propName = "$field" + (prop.Key.StartsWith("$") ? prop.Key : "$" + prop.Key);
						string propName = prop.Key;
						dataRecord[propName] = record[prop.Key];
					}

					resultDataList.Add(dataRecord);
				}
			}

			return resultDataList;
		}

        #endregion

        #region << Area Specific >>
        // Get area meta by name
        // GET: api/v1/en_US/area/{name}
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/area/{name}")]
        public IActionResult GetAreaByName(string name)
        {

            QueryObject areaFilterObj = EntityQuery.QueryEQ("name", name);

            EntityQuery query = new EntityQuery("area", "*", areaFilterObj, null, null, null);

            QueryResponse result = recMan.Find(query);
            if (!result.Success)
                return DoResponse(result);
            return Json(result);
        }


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

		// Get all entities that has relation to an area
		// GET: api/v1/en_US/area/entity/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/sitemap")]
		public IActionResult GetSitemap()
		{
            var columnsNeeded = "id,name,label,color,icon_name,weight,roles,subscriptions";
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
                    if ((string)area["subscriptions"] != "[]")
                    {
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

		#endregion

		#region << User specific >>

        // GET: api/v1/en_US/user/{userId}
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/user/{userId}")]
        public IActionResult GetUserById(Guid userId)
        {

            QueryObject areaFilterObj = EntityQuery.QueryEQ("id", userId);
			var userColumns = "$user_role.id,$user_role.name,id,email,first_name,last_name";

            EntityQuery query = new EntityQuery("user", userColumns, areaFilterObj, null, null, null);

            QueryResponse result = recMan.Find(query);
            if (!result.Success)
                return DoResponse(result);
            return Json(result);
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

