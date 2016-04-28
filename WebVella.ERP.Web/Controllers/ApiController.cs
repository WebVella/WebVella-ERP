using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using WebVella.ERP.Api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Database;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Web.Security;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Authorization;
using CsvHelper;
using Microsoft.AspNet.StaticFiles;
using WebVella.ERP.Utilities;
using System.Dynamic;



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebVella.ERP.Web.Controllers
{
	public class ApiController : ApiControllerBase
	{
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

		//TODO - add created_by and modified_by fields where needed, when the login is done
		RecordManager recMan;
		EntityManager entityManager;
		EntityRelationManager entityRelationManager;
		SecurityManager secMan;

		public ApiController()
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entityManager = new EntityManager();
			entityRelationManager = new EntityRelationManager();
		}

		[AllowAnonymous]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/user/login")]
		public IActionResult Login([FromBody]JObject submitObj)
		{
			string email = (string)submitObj["email"];
			string password = (string)submitObj["password"];
			bool rememberMe = (bool)submitObj["rememberMe"];

			SecurityManager secMan = new SecurityManager();
			var user = secMan.GetUser(email, password);
			var responseObj = new ResponseModel();

			if (user != null)
			{
				if (user.Enabled == false)
				{
					responseObj.Success = false;
					responseObj.Message = "Error while user authentication.";

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
					responseObj.Object = new { token = WebSecurityUtil.Login(HttpContext, user.Id, user.ModifiedOn, rememberMe) };
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

		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/user/logout")]
		public IActionResult Logout()
		{
			WebSecurityUtil.Logout(HttpContext);
			var responseObj = new ResponseModel();
			responseObj.Object = null;
			responseObj.Success = true;
			responseObj.Timestamp = DateTime.UtcNow;
			return DoResponse(responseObj);
		}

		[AllowAnonymous]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/user/permissions")]
		public IActionResult CurrentUserPermissions()
		{
			var responseObj = new ResponseModel();
			responseObj.Object = WebSecurityUtil.GetCurrentUserPermissions(HttpContext);
			responseObj.Success = true;
			responseObj.Timestamp = DateTime.UtcNow;
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
		// GET: api/v1/en_US/meta/entity/id/{entityId}/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/id/{entityId}")]
		public IActionResult GetEntityMetaById(Guid entityId)
		{
			return DoResponse(entityManager.ReadEntity(entityId));
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

				DbEntity storageEntity = DbContext.Current.EntityRepository.Read(entityId);
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
				HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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

				DbEntity storageEntity = DbContext.Current.EntityRepository.Read(entityId);
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
				//DbEntity storageEntity = DbContext.Current.EntityRepository.Read(Name);
				//if (storageEntity == null)
				//{
				//	response.Timestamp = DateTime.UtcNow;
				//	response.Success = false;
				//	response.Message = "Entity with such Name does not exist!";
				//	return DoBadRequestResponse(response);
				//}
				//entity = storageEntity.MapTo<Entity>();

				var entResp = new EntityManager().ReadEntity(Name);
				if (entResp.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				entity = entResp.Object;

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
					if (prop.Name.ToLower() == "iconname")
						list.IconName = inputList.IconName;
					if (prop.Name.ToLower() == "visiblecolumnscount")
						list.VisibleColumnsCount = inputList.VisibleColumnsCount;
					if (prop.Name.ToLower() == "dynamichtmltemplate")
						list.DynamicHtmlTemplate = inputList.DynamicHtmlTemplate;
					if (prop.Name.ToLower() == "columnwidthscsv")
						list.ColumnWidthsCSV = inputList.ColumnWidthsCSV;
					if (prop.Name.ToLower() == "actionitems")
						list.ActionItems = inputList.ActionItems;
					if (prop.Name.ToLower() == "servicecode")
						list.ServiceCode = inputList.ServiceCode;
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

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}/service.js")]
		public IActionResult GetRecordListServiceJSByName(string Name, string ListName, bool defaultScript = false)
		{
			var list = entityManager.ReadRecordList(Name, ListName);
			if( list == null || list.Object == null || list.Success == false )
				return DoPageNotFoundResponse();

			var code = list.Object.ServiceCode;
			if (string.IsNullOrWhiteSpace(code) || defaultScript)
				return File("/plugins/webvella-core/providers/list_default_service_script.js", "text/javascript");

			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(code);
			return File(bytes, "text/javascript" );
		}

		#endregion

		#region << Record Views >>

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{entityName}/getEntityViewLibrary")]
		public IActionResult GetEntityLibrary(string entityName)
		{
			var result = new EntityLibraryItemsResponse() { Success = true, Timestamp = DateTime.UtcNow };
			var relMan = new EntityRelationManager();
			var relations = relMan.Read().Object;

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

			//itemList.Add(new { type = "html", tag = "HTML string", content = "" });

			foreach (var field in entity.Fields)
			{
				if (!(field is TreeSelectField))
				{
					itemList.Add(new RecordViewFieldItem
					{
						FieldId = field.Id,
						FieldName = field.Name,
						Meta = CleanupFieldForLibrary(field),
						EntityId = entity.Id,
						EntityName = entity.Name,
						EntityLabel = entity.Label,
						EntityLabelPlural = entity.LabelPlural,
						DataName = field.Name
					});
				}
				else
				{
					TreeSelectField treeField = field as TreeSelectField;
					var treeRelation = relations.SingleOrDefault(x => x.Id == treeField.RelationId);
					if (treeRelation == null) //skip if missing relation is used // simple protection
						continue;

					Entity relatedEntity = entityManager.ReadEntity(treeField.RelatedEntityId).Object;
					if (relatedEntity == null) //skip if missing related entity // simple protection
						continue;

					var tree = relatedEntity.RecordTrees.SingleOrDefault(t => t.Id == treeField.SelectedTreeId);
					if (tree == null) //skip if missing selected tree // simple protection
						continue;

					itemList.Add(new RelationTreeItem
					{
						RelationId = treeRelation.Id,
						RelationName = treeRelation.Name,
						EntityId = relatedEntity.Id,
						EntityName = relatedEntity.Name,
						EntityLabel = relatedEntity.Label,
						EntityLabelPlural = relatedEntity.LabelPlural,
						TreeId = tree.Id,
						TreeName = tree.Name,
						Meta = tree,
						DataName = string.Format("$tree${0}${1}", treeRelation.Name, tree.Name),
						FieldLabel = "",
						FieldPlaceholder = "",
						FieldHelpText = "",
						FieldRequired = false,
						FieldLookupList = "",
						FieldManageView = ""
					});
				}

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


			var entityRelations = relations.Where(x => x.OriginEntityId == entity.Id || x.TargetEntityId == entity.Id).ToList();

			foreach (var relation in entityRelations)
			{
				Guid relatedEntityId = relation.OriginEntityId == entity.Id ? relation.TargetEntityId : relation.OriginEntityId;
				Entity relatedEntity = entityManager.ReadEntity(relatedEntityId).Object;

				itemList.Add(new EntityRelationOptionsItem
				{
					RelationId = relation.Id,
					RelationName = relation.Name,
					Direction = "origin-target"
				});

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
						Meta = CleanupFieldForLibrary(field),
						DataName = string.Format("$field${0}${1}", relation.Name, field.Name),
						FieldLabel = "",
						FieldPlaceholder = "",
						FieldHelpText = "",
						FieldRequired = false,
						FieldLookupList = ""
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
						DataName = string.Format("$view${0}${1}", relation.Name, view.Name),
						FieldLabel = "",
						FieldPlaceholder = "",
						FieldHelpText = "",
						FieldRequired = false,
						FieldLookupList = "",
						FieldManageView = ""
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
						DataName = string.Format("$list${0}${1}", relation.Name, list.Name),
						FieldLabel = "",
						FieldPlaceholder = "",
						FieldHelpText = "",
						FieldRequired = false,
						FieldLookupList = "",
						FieldManageView = ""

					});
				}
			}

			result.Object = itemList;

			return DoResponse(result);
		}

		private Field CleanupFieldForLibrary(Field field)
		{
			//TODO remove default values and options and all not needed data
			if (field is SelectField)
				((SelectField)field).Options = new List<SelectFieldOption>();
			else if(field is MultiSelectField)
				((MultiSelectField)field).Options = new List<MultiSelectFieldOption>();

			return field;
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

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}/service.js")]
		public IActionResult GetRecordViewServiceJSByName(string Name, string ViewName, bool defaultScript = false)
		{
			var view = entityManager.ReadRecordView(Name, ViewName);
			if (view == null || view.Object == null || view.Success == false)
				return DoPageNotFoundResponse();

			var code = view.Object.ServiceCode;
			if (string.IsNullOrWhiteSpace(code) || defaultScript)
				return File("/plugins/webvella-core/providers/view_default_service_script.js", "text/javascript");

			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(code);
			return File(bytes, "text/javascript");
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
				DbEntity storageEntity = DbContext.Current.EntityRepository.Read(Name);
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
					if (prop.Name.ToLower() == "iconname")
						view.IconName = inputView.IconName;
					if (prop.Name.ToLower() == "dynamichtmltemplate")
						view.DynamicHtmlTemplate = inputView.DynamicHtmlTemplate;
					if (prop.Name.ToLower() == "actionitems")
						view.ActionItems = inputView.ActionItems;
					if (prop.Name.ToLower() == "servicecode")
						view.ServiceCode = inputView.ServiceCode;
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

		#region << Record Trees >>

		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree")]
		public IActionResult CreateRecordTreeByName(string entityName, [FromBody]JObject submitObj)
		{
			RecordListResponse response = new RecordListResponse();

			InputRecordTree tree = new InputRecordTree();
			try
			{
				tree = InputRecordTree.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.CreateRecordTree(entityName, tree));
		}

		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree/{treeName}")]
		public IActionResult UpdateRecordTreeByName(string entityName, string treeName, [FromBody]JObject submitObj)
		{
			RecordListResponse response = new RecordListResponse();

			InputRecordTree tree = new InputRecordTree();

			Type inputViewType = tree.GetType();

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
				tree = InputRecordTree.Convert(submitObj);
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entityManager.UpdateRecordTree(entityName, tree));
		}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree/{treeName}")]
		public IActionResult PatchRecordTreeByName(string entityName, string treeName, [FromBody]JObject submitObj)
		{
			RecordTreeResponse response = new RecordTreeResponse();
			Entity entity = new Entity();
			InputRecordTree tree = new InputRecordTree();

			try
			{
				DbEntity storageEntity = DbContext.Current.EntityRepository.Read(entityName);
				if (storageEntity == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				entity = storageEntity.MapTo<Entity>();

				RecordTree treeToUpdate = entity.RecordTrees.FirstOrDefault(l => l.Name == treeName);
				if (treeToUpdate == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "REcord tree with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				tree = treeToUpdate.MapTo<InputRecordTree>();

				Type inputListType = tree.GetType();

				foreach (var prop in submitObj.Properties())
				{
					int count = inputListType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
					if (count < 1)
						response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
				}

				if (response.Errors.Count > 0)
					return DoBadRequestResponse(response);

				InputRecordTree inputTree = InputRecordTree.Convert(submitObj);

				foreach (var prop in submitObj.Properties())
				{
					if (prop.Name.ToLower() == "label")
						tree.Label = inputTree.Label;
					if (prop.Name.ToLower() == "default")
						tree.Default = inputTree.Default;
					if (prop.Name.ToLower() == "system")
						tree.System = inputTree.System;
					if (prop.Name.ToLower() == "depthlimit")
						tree.DepthLimit = inputTree.DepthLimit;
					if (prop.Name.ToLower() == "cssclass")
						tree.CssClass = inputTree.CssClass;
					if (prop.Name.ToLower() == "iconname")
						tree.IconName = inputTree.IconName;
					if (prop.Name.ToLower() == "nodenamefieldid")
						tree.NodeNameFieldId = inputTree.NodeNameFieldId;
					if (prop.Name.ToLower() == "nodelabelfieldid")
						tree.NodeLabelFieldId = inputTree.NodeLabelFieldId;
					if (prop.Name.ToLower() == "nodeweightfieldid")
						tree.NodeWeightFieldId = inputTree.NodeWeightFieldId;
					if (prop.Name.ToLower() == "rootnodes")
						tree.RootNodes = inputTree.RootNodes;
					if (prop.Name.ToLower() == "nodeobjectproperties")
						tree.NodeObjectProperties = inputTree.NodeObjectProperties;
				}
			}
			catch (Exception e)
			{
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}
			var updateResponse = entityManager.UpdateRecordTree(entity, tree);
			return DoResponse(updateResponse);
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree/{treeName}")]
		public IActionResult DeleteRecordTreeByName(string entityName, string treeName)
		{
			return DoResponse(entityManager.DeleteRecordTree(entityName, treeName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree/{treeName}")]
		public IActionResult GetRecordTreeByName(string entityName, string treeName)
		{
			return DoResponse(entityManager.ReadRecordTree(entityName, treeName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree")]
		public IActionResult GetRecordTreesByEntityName(string entityName)
		{
			return DoResponse(entityManager.ReadRecordTrees(entityName));
		}

		#endregion

		#region << Relation Meta >>
		// Get all entity relation definitions
		// GET: api/v1/en_US/meta/relation/list/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/relation/list")]
		public IActionResult GetEntityRelationMetaList()
		{
			return DoResponse(new EntityRelationManager().Read());
		}

		// Get entity relation meta
		// GET: api/v1/en_US/meta/relation/{name}/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/relation/{name}")]
		public IActionResult GetEntityRelationMeta(string name)
		{
			return DoResponse(new EntityRelationManager().Read(name));
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
				return DoResponse(new EntityRelationManager().Create(relation));
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
				return DoResponse(new EntityRelationManager().Update(relation));
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
				return DoResponse(new EntityRelationManager().Delete(newGuid));
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
			var recMan = new RecordManager();
			var entMan = new EntityManager();
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
				relation = new EntityRelationManager().Read(model.RelationName).Object;
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

			using (var connection = DbContext.Current.CreateConnection())
			{
				connection.BeginTransaction();

				try
				{
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
										connection.RollbackTransaction();
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
										connection.RollbackTransaction();
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
										connection.RollbackTransaction();
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
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Target record id=[" + record["id"] + "] attach  operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}
							}
							break;
						default:
							{
								connection.RollbackTransaction();
								throw new Exception("Not supported relation type");
							}
					}

					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();

					response.Success = false;
					response.Message = ex.Message;
					return DoResponse(response);
				}
			}
			return DoResponse(response);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult GetRecord(Guid recordId, string entityName, string fields = "*")
		{

			QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);

			EntityQuery query = new EntityQuery(entityName, fields, filterObj, null, null, null);

			QueryResponse result = recMan.Find(query);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult DeleteRecord(Guid recordId, string entityName)
		{
			return DoResponse(recMan.DeleteRecord(entityName, recordId));
		}

		// Get an entity records by field and regex
		// GET: api/v1/en_US/record/{entityName}/regex
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/{entityName}/regex/{fieldName}")]
		public IActionResult GetRecordsByFieldAndRegex(string fieldName, string entityName, [FromBody]EntityRecord patternObj)
		{

			QueryObject filterObj = EntityQuery.QueryRegex(fieldName, patternObj["pattern"]);

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
			//TODO implement validation
			if (postObj == null)
				postObj = new EntityRecord();

			if (!postObj.GetProperties().Any(x => x.Key == "id"))
				postObj["id"] = Guid.NewGuid();
			else if (string.IsNullOrEmpty(postObj["id"] as string))
				postObj["id"] = Guid.NewGuid();

			QueryResponse result = recMan.CreateRecord(entityName, postObj);
			return DoResponse(result);
		}

		// Update an entity record
		// PUT: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult UpdateEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			//TODO implement validation
			QueryResponse result = recMan.UpdateRecord(entityName, postObj);
			return DoResponse(result);
		}

		// Patch an entity record
		// PATCH: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult PatchEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			//TODO implement validation
			postObj["id"] = recordId;
			QueryResponse result = recMan.UpdateRecord(entityName, postObj);
			return DoResponse(result);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/list/{listName}/{page}")]
		public IActionResult GetRecordListByEntityName(string entityName, string listName, int page, int? pageSize = null)
		{

			EntityListResponse entitiesResponse = entityManager.ReadEntities();
			List<Entity> entities = entitiesResponse.Object.Entities;

			var response = new RecordListRecordResponse();
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
			try
			{
				response.Object.Data = GetListRecords(entities, entity, listName, page, null, pageSize);
			}
			catch (Exception ex)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = ex.Message;
				return DoResponse(response);
			}

			RecordList list = entity.RecordLists.FirstOrDefault(l => l.Name == listName);
			if (list != null)
			{
				response.Object.Meta = list;
			}

			return DoResponse(response);
		}

		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/list")]
		public IActionResult GetRecordsByEntityName(string entityName, string ids = "", string fields = "")
		{
			var response = new QueryResponse();
			var recordIdList = new List<Guid>();
			var fieldList = new List<string>();

			if(!String.IsNullOrWhiteSpace(ids)) {
				var idStringList = ids.Split(',');
				var outGuid = Guid.Empty;
				foreach(var idString in idStringList) {
					if(Guid.TryParse(idString, out outGuid)){
						recordIdList.Add(outGuid);
					}
					else {
						response.Message = "One of the record ids is not a Guid";
						response.Timestamp = DateTime.UtcNow;
						response.Success = false;
						response.Object.Data = null;						
					}
				}
			}

			if(!String.IsNullOrWhiteSpace(fields)) {
				var fieldsArray = fields.Split(',');
				var hasId = false;
				foreach(var fieldName in fieldsArray) {
					if(fieldName == "id") {
						hasId = true;
					}
					fieldList.Add(fieldName);
				}
				if(!hasId) {
					fieldList.Add("id");
				}
			}

			var QueryList = new List<QueryObject>();
			foreach (var recordId in recordIdList)
			{
				QueryList.Add(EntityQuery.QueryEQ("id", recordId));
			}

			QueryObject recordsFilterObj = null;
			if(QueryList.Count > 0) {
				recordsFilterObj = EntityQuery.QueryOR(QueryList.ToArray());
			}
			
			if(!fieldList.Contains("id")) {
				fieldList.Add("id");
			}
			var columns = String.Join(",", fieldList.Select(x => x.ToString()).ToArray());

			EntityQuery query = new EntityQuery(entityName, columns, recordsFilterObj, null, null, null);
			var queryResponse = recMan.Find(query);
			if (!queryResponse.Success)
			{
				response.Message = queryResponse.Message;
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Object = null;
				return DoResponse(response);
			}


			response.Message = "Success";
			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Object.Data = queryResponse.Object.Data;


			return DoResponse(response);
		}

		private QueryObject CreateSearchQuery(string search, RecordList list, Entity entity)
		{
			if (string.IsNullOrWhiteSpace(search))
				return null;

			if (list == null)
				return null;

			search = search.Trim();

			var listFields = list.Columns.Where(c => c is RecordListFieldItem).Select(c => c as RecordListFieldItem).ToList();


			var firstSearchableField = listFields.FirstOrDefault(x => entity.Fields.Single(f => f.Id == x.FieldId).Searchable);
			if (firstSearchableField == null)
				throw new Exception("The list has no searchable fields.");

			var field = entity.Fields.SingleOrDefault(f => f.Id == firstSearchableField.FieldId);

			if (field is AutoNumberField || field is CurrencyField || field is NumberField || field is PercentField)
			{
				decimal value;
				if (!decimal.TryParse(search, out value))
					throw new Exception("Invalid search value. It should be a number.");

				return EntityQuery.QueryEQ(field.Name, value);
			}
			else if (field is GuidField)
			{
				Guid value;
				if (!Guid.TryParse(search, out value))
					throw new Exception("Invalid search value. It should be an unique identifier formated text.");

				return EntityQuery.QueryEQ(field.Name, value);
			}
			else if (field is DateTimeField || field is DateField)
			{
				DateTime value;
				if (!DateTime.TryParse(search, out value))
					throw new Exception("Invalid search value. Cannot be recognized as date.");

				value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
				return EntityQuery.QueryEQ(field.Name, value);
			}
			else if (field is MultiSelectField)
			{
				var option = (field as MultiSelectField).Options.FirstOrDefault(o => o.Value == search);
				if (option == null)
					return EntityQuery.QueryEQ(field.Name, Guid.NewGuid().ToString()); //this will always be not found
				else
					return EntityQuery.QueryEQ(field.Name, option.Key); //search in the keys
			}
			else
				return EntityQuery.QueryContains(field.Name, search);
		}

		private List<EntityRecord> GetListRecords(List<Entity> entities, Entity entity, string listName, int? page = null, QueryObject queryObj = null, int? pageSize = null, bool export = false)
		{
			if (entity == null)
				throw new Exception($"Entity '{entity.Name}' do not exist");

			RecordList list = null;
			if (entity != null && entity.RecordLists != null)
				list = entity.RecordLists.FirstOrDefault(l => l.Name == listName);
			
			if (list == null)
				throw new Exception($"Entity '{entity.Name}' do not have list named '{listName}'");
			
			List<KeyValuePair<string, string>> queryStringOverwriteParameters = new List<KeyValuePair<string, string>>();
			foreach (var key in Request.Query.Keys)
				queryStringOverwriteParameters.Add(new KeyValuePair<string, string>(key, Request.Query[key]));
			

			EntityQuery resultQuery = new EntityQuery(entity.Name, "*", queryObj, null, null, null, queryStringOverwriteParameters);
			EntityRelationManager relManager = new EntityRelationManager();
			EntityRelationListResponse relListResponse = relManager.Read();
			List<EntityRelation> relationList = new List<EntityRelation>();
			if (relListResponse.Object != null)
				relationList = relListResponse.Object;

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
					var listQuery = RecordListQuery.ConvertQuery(list.Query);

					if (queryObj != null)
					{
						//if (queryObj.SubQueries != null && queryObj.SubQueries.Any())
						//	queryObj.SubQueries.Add(listQuery);
						//else
							queryObj = EntityQuery.QueryAND(listQuery, queryObj);
					}
					else
						queryObj = listQuery;

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
						else if (column is RecordListRelationTreeItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationTreeItem)column).RelationId);

							string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

							Guid relEntityId = relation.OriginEntityId;
							Guid relFieldId = relation.OriginFieldId;

							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var treeId = (column as RecordListRelationTreeItem).TreeId;
							RecordTree tree = relEntity.RecordTrees.Single(x => x.Id == treeId);

							var relIdField = relEntity.Fields.Single(x => x.Name == "id");

							List<Guid> fieldIdsToInclude = new List<Guid>();
							fieldIdsToInclude.AddRange(tree.NodeObjectProperties);

							if (!fieldIdsToInclude.Contains(relIdField.Id))
								fieldIdsToInclude.Add(relIdField.Id);

							if (!fieldIdsToInclude.Contains(tree.NodeNameFieldId))
								fieldIdsToInclude.Add(tree.NodeNameFieldId);

							if (!fieldIdsToInclude.Contains(tree.NodeLabelFieldId))
								fieldIdsToInclude.Add(tree.NodeLabelFieldId);

							if (!fieldIdsToInclude.Contains(relField.Id))
								fieldIdsToInclude.Add(relField.Id);

							foreach (var fieldId in fieldIdsToInclude)
							{
								var f = relEntity.Fields.SingleOrDefault(x => x.Id == fieldId);
								if (f != null)
								{
									string qFieldName = string.Format("{0}{1},", relName, f.Name);
									if (!queryFields.Contains(qFieldName))
										queryFields += qFieldName;
								}
							}

							//always add target field in query, its value may be required for relative view and list
							Field field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
							queryFields += field.Name + ", ";
						}
						else if (column is RecordListRelationFieldItem)
						{
							string targetOriginPrefix = "";
							if (list.RelationOptions != null)
							{
								var options = list.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationFieldItem)column).RelationId);
								if (options != null && options.Direction == "target-origin")
									targetOriginPrefix = "$";
							}

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationFieldItem)column).RelationId);
							queryFields += string.Format(targetOriginPrefix + "${0}.{1}, ", relation.Name, ((RecordListRelationFieldItem)column).Meta.Name);

							//add ID field automatically if not added
							if (!queryFields.Contains(string.Format(targetOriginPrefix + "${0}.id", relation.Name)))
								queryFields += string.Format(targetOriginPrefix + "${0}.id,", relation.Name);

							//always add origin field in query, its value may be required for relative view and list
							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							queryFields += field.Name + ", ";
						}
						else if (column is RecordListListItem || column is RecordListViewItem)
						{
							if (export)
								continue;

							if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
								queryFields += "id, ";
						}
						else if (column is RecordListRelationListItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationListItem)column).RelationId);

							string targetOriginPrefix = "";
							if (list.RelationOptions != null)
							{
								var options = list.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationListItem)column).RelationId);
								if (options != null && options.Direction == "target-origin")
									targetOriginPrefix = "$";
							}

							string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

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
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationViewItem)column).RelationId);

							string targetOriginPrefix = "";
							if (list.RelationOptions != null)
							{
								var options = list.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationViewItem)column).RelationId);
								if (options != null && options.Direction == "target-origin")
									targetOriginPrefix = "$";
							}

							string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

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

				if (!pageSize.HasValue)
					pageSize = list.PageSize;

				if (pageSize.Value > 0)
				{
					resultQuery.Limit = pageSize.Value;
					if (page != null && page > 0)
						resultQuery.Skip = (page - 1) * resultQuery.Limit;
				}
			}

			List<EntityRecord> resultDataList = new List<EntityRecord>();

			QueryResponse result = recMan.Find(resultQuery);
			if (!result.Success)
				throw new Exception(result.Message);

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
							dataRecord[column.DataName] = GetListRecords(entities, entity, ((RecordListListItem)column).ListName);
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

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							List<QueryObject> queries = new List<QueryObject>();
							foreach (var relatedRecord in relatedRecords)
								queries.Add(EntityQuery.QueryEQ(relField.Name, relatedRecord[relField.Name]));

							if (queries.Count > 0)
							{
								QueryObject subListQueryObj = EntityQuery.QueryOR(queries.ToArray());
								List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordListRelationListItem)column).ListName, queryObj: subListQueryObj);
								dataRecord[((RecordListRelationListItem)column).DataName] = subListResult;
							}
							else
								dataRecord[((RecordListRelationListItem)column).DataName] = new List<object>();
						}
						else if (column is RecordListViewItem)
						{
							dataRecord[column.DataName] = GetViewRecords(entities, entity, ((RecordListViewItem)column).ViewName, "id", record["id"]);
						}
						else if (column is RecordListRelationTreeItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationTreeItem)column).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							dataRecord[((RecordListRelationTreeItem)column).DataName] = relatedRecords;
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

							List<EntityRecord> subViewResult = new List<EntityRecord>();
							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							foreach (var relatedRecord in relatedRecords)
							{
								subViewResult.AddRange(GetViewRecords(entities, relEntity, ((RecordListRelationViewItem)column).ViewName, relField.Name, relatedRecord[relField.Name]));
							}
							dataRecord[((RecordListRelationViewItem)column).DataName] = subViewResult;
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

			EntityRelationManager relManager = new EntityRelationManager();
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
						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationFieldItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";

						}
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationFieldItem)item).RelationId);

						//add ID field automatically if not added
						if (!queryFields.Contains(string.Format(targetOriginPrefix + "${0}.id", relation.Name)))
							queryFields += string.Format(targetOriginPrefix + "${0}.id,", relation.Name);

						Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
						Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);

						queryFields += field.Name + ", ";
						queryFields += string.Format(targetOriginPrefix + "${0}.{1}, ", relation.Name, ((RecordViewRelationFieldItem)item).Meta.Name);

					}
					else if (item is RecordViewListItem || item is RecordViewViewItem)
					{
						if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
							queryFields += "id";
					}
					else if (item is RecordViewRelationTreeItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationTreeItem)item).RelationId);

						string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

						Guid relEntityId = relation.OriginEntityId;
						Guid relFieldId = relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						var treeId = (item as RecordViewRelationTreeItem).TreeId;
						RecordTree tree = relEntity.RecordTrees.Single(x => x.Id == treeId);

						var relIdField = relEntity.Fields.Single(x => x.Name == "id");

						List<Guid> fieldIdsToInclude = new List<Guid>();
						fieldIdsToInclude.AddRange(tree.NodeObjectProperties);

						if (!fieldIdsToInclude.Contains(relIdField.Id))
							fieldIdsToInclude.Add(relIdField.Id);

						if (!fieldIdsToInclude.Contains(tree.NodeNameFieldId))
							fieldIdsToInclude.Add(tree.NodeNameFieldId);

						if (!fieldIdsToInclude.Contains(tree.NodeLabelFieldId))
							fieldIdsToInclude.Add(tree.NodeLabelFieldId);

						if (!fieldIdsToInclude.Contains(relField.Id))
							fieldIdsToInclude.Add(relField.Id);

						foreach (var fieldId in fieldIdsToInclude)
						{
							var f = relEntity.Fields.SingleOrDefault(x => x.Id == fieldId);
							if (f != null)
							{
								string qFieldName = string.Format("{0}{1},", relName, f.Name);
								if (!queryFields.Contains(qFieldName))
									queryFields += qFieldName;
							}
						}

						//always add target field in query, its value may be required for relative view and list
						Field field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
						queryFields += field.Name + ", ";
					}
					else if (item is RecordViewRelationListItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationListItem)item).RelationId);

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationListItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

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

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationViewItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

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
					else if (item is RecordViewSidebarRelationTreeItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationTreeItem)item).RelationId);

						string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

						Guid relEntityId = relation.OriginEntityId;
						Guid relFieldId = relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

						//always add target field in query, its value may be required for relative view and list
						Field field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
						queryFields += field.Name + ", ";
					}
					else if (item is RecordViewSidebarRelationListItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationListItem)item).RelationId);

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewSidebarRelationListItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

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

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewSidebarRelationViewItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

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
							dataRecord[((RecordViewListItem)item).DataName] = GetListRecords(entities, entity, ((RecordViewListItem)item).ListName);
						}
						else if (item is RecordViewViewItem)
						{
							dataRecord[((RecordViewViewItem)item).DataName] = GetViewRecords(entities, entity, ((RecordViewViewItem)item).ViewName, "id", record["id"]);
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
						else if (item is RecordViewRelationTreeItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationTreeItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							dataRecord[((RecordViewRelationTreeItem)item).DataName] = relatedRecords;
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

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							List<QueryObject> queries = new List<QueryObject>();
							foreach (var relatedRecord in relatedRecords)
								queries.Add(EntityQuery.QueryEQ(relField.Name, relatedRecord[relField.Name]));

							if (queries.Count > 0)
							{
								QueryObject subListQueryObj = EntityQuery.QueryOR(queries.ToArray());
								List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordViewRelationListItem)item).ListName, queryObj: subListQueryObj);
								dataRecord[((RecordViewRelationListItem)item).DataName] = subListResult;
							}
							else
								dataRecord[((RecordViewRelationListItem)item).DataName] = new List<object>();
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

							List<EntityRecord> subViewResult = new List<EntityRecord>();
							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							foreach (var relatedRecord in relatedRecords)
							{
								subViewResult.AddRange(GetViewRecords(entities, relEntity, ((RecordViewRelationViewItem)item).ViewName, "id", relatedRecord["id"]));
							}
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
						else if (item is RecordViewSidebarRelationTreeItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationTreeItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							dataRecord[((RecordViewSidebarRelationTreeItem)item).DataName] = relatedRecords;
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

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							List<QueryObject> queries = new List<QueryObject>();
							foreach (var relatedRecord in relatedRecords)
								queries.Add(EntityQuery.QueryEQ(relField.Name, relatedRecord[relField.Name]));

							if (queries.Count > 0)
							{
								//QueryObject subListQueryObj = EntityQuery.QueryEQ(relField.Name, record[field.Name]);
								QueryObject subListQueryObj = EntityQuery.QueryOR(queries.ToArray());
								List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordViewSidebarRelationListItem)item).ListName, queryObj: subListQueryObj);
								dataRecord[((RecordViewSidebarRelationListItem)item).DataName] = subListResult;
							}
							else
								dataRecord[((RecordViewSidebarRelationListItem)item).DataName] = new List<object>();
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

							List<EntityRecord> subViewResult = new List<EntityRecord>();
							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							foreach (var relatedRecord in relatedRecords)
							{
								subViewResult.AddRange(GetViewRecords(entities, relEntity, ((RecordViewSidebarRelationViewItem)item).ViewName, "id", relatedRecord["id"]));
							}
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

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/tree/{treeName}")]
		public IActionResult GetTreeRecords(string entityName, string treeName)
		{
			List<Entity> entities = entityManager.ReadEntities().Object.Entities;

			RecordTreeRecordResponse response = new RecordTreeRecordResponse();
			response.Message = "Success";
			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Object = new RecordTreeRecord();

			Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

			if (entity == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
				return DoResponse(response);
			}

			RecordTree tree = entity.RecordTrees.SingleOrDefault(x => x.Name == treeName);
			if (tree == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Tree with such name does not exist!";
				response.Errors.Add(new ErrorModel("treeName", treeName, "Tree with such name does not exist!"));
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
			try
			{
				List<EntityRelation> relationList = new EntityRelationManager().Read().Object ?? new List<EntityRelation>();
				response.Object.Data = GetTreeRecords(entities, relationList, tree);
				response.Object.Meta = tree;
			}
			catch (Exception ex)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = ex.Message;
				return DoResponse(response);
			}

			return DoResponse(response);
		}

		private List<ResponseTreeNode> GetTreeRecords(List<Entity> entities, List<EntityRelation> relationList, RecordTree tree)
		{
			EntityRelation relation = relationList.FirstOrDefault(r => r.Id == tree.RelationId);

			Guid treeEntityId = relation.OriginEntityId;
			Guid treeRelFieldId = relation.OriginFieldId;

			Entity treeEntity = entities.FirstOrDefault(e => e.Id == treeEntityId);
			Field treeIdField = treeEntity.Fields.FirstOrDefault(f => f.Id == treeRelFieldId);
			Field treeParrentField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeParentIdFieldId);
			Field nameField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeNameFieldId);
			Field labelField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeLabelFieldId);
			Field weightField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeWeightFieldId);

			var relIdField = treeEntity.Fields.Single(x => x.Name == "id");

			List<Guid> fieldIdsToInclude = new List<Guid>();

			if (!fieldIdsToInclude.Contains(treeIdField.Id))
				fieldIdsToInclude.Add(treeIdField.Id);

			if (!fieldIdsToInclude.Contains(treeParrentField.Id))
				fieldIdsToInclude.Add(treeParrentField.Id);

			if (!fieldIdsToInclude.Contains(tree.NodeNameFieldId))
				fieldIdsToInclude.Add(tree.NodeNameFieldId);

			if (!fieldIdsToInclude.Contains(tree.NodeLabelFieldId))
				fieldIdsToInclude.Add(tree.NodeLabelFieldId);

			var weightFieldNonNullable = Guid.Empty;
			if (tree.NodeWeightFieldId.HasValue)
			{
				weightFieldNonNullable = tree.NodeWeightFieldId.Value;
			}
			if (weightField != null && !fieldIdsToInclude.Contains(weightFieldNonNullable))
				fieldIdsToInclude.Add(weightFieldNonNullable);

			string queryFields = string.Empty;
			foreach (var fieldId in fieldIdsToInclude)
			{
				var f = treeEntity.Fields.SingleOrDefault(x => x.Id == fieldId);
				if (f != null)
				{
					if (!queryFields.Contains(f.Name))
						queryFields += (f.Name + ",");
				}
			}
			queryFields += "id";

			EntityQuery eq = new EntityQuery(treeEntity.Name, queryFields);
			RecordManager recMan = new RecordManager();
			var allRecords = recMan.Find(eq).Object.Data;

			List<ResponseTreeNode> rootNodes = new List<ResponseTreeNode>();
			foreach (var rootNode in tree.RootNodes.OrderBy(x => x.Name))
			{
				List<ResponseTreeNode> children = new List<ResponseTreeNode>();
				int? rootNodeWeight = null;
				if (weightField != null)
				{
					rootNodeWeight = rootNode.Weight;
					children = GetTreeNodeChildren(allRecords, treeIdField.Name,
									 treeParrentField.Name, nameField.Name, labelField.Name, rootNode.Id, weightField.Name, 1, tree.DepthLimit);
				}
				else {
					children = GetTreeNodeChildren(allRecords, treeIdField.Name,
									 treeParrentField.Name, nameField.Name, labelField.Name, rootNode.Id, "no-weight", 1, tree.DepthLimit);
				}
				rootNodes.Add(new ResponseTreeNode
				{
					RecordId = rootNode.RecordId,
					Id = rootNode.Id.Value,
					ParentId = rootNode.ParentId,
					Name = rootNode.Name,
					Label = rootNode.Label,
					Weight = rootNodeWeight,
					Nodes = children
				});

			}

			return rootNodes;
		}

		private List<ResponseTreeNode> GetTreeNodeChildren(string entityName, string fields, string idFieldName, string parentIdFieldName,
				string nameFieldName, string labelFieldName, Guid? nodeId, string weightFieldName = "no-weight", int depth = 1, int maxDepth = 20)
		{
			if (depth >= maxDepth)
				return new List<ResponseTreeNode>();

			var query = EntityQuery.QueryEQ(parentIdFieldName, nodeId);
			EntityQuery eq = new EntityQuery(entityName, fields, query);
			RecordManager recMan = new RecordManager();
			var records = recMan.Find(eq).Object.Data;
			List<ResponseTreeNode> nodes = new List<ResponseTreeNode>();
			depth++;
			foreach (var record in records)
			{
				if (weightFieldName == "no-weight")
				{
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = null,
						Nodes = GetTreeNodeChildren(entityName, fields, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth)
					});
				}
				else {
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = (int?)((decimal?)record[weightFieldName]),
						Nodes = GetTreeNodeChildren(entityName, fields, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth)
					});
				}
			}
			if (weightFieldName == "no-weight")
			{
				return nodes.OrderBy(x => x.Name).ToList();
			}
			else {
				return nodes.OrderBy(x => x.Weight).ThenBy(y => y.Name).ToList();
			}
		}

		private List<ResponseTreeNode> GetTreeNodeChildren(List<EntityRecord> allRecords, string idFieldName, string parentIdFieldName,
				string nameFieldName, string labelFieldName, Guid? nodeId, string weightFieldName = "no-weight", int depth = 1, int maxDepth = 20)
		{
			if (depth >= maxDepth)
				return new List<ResponseTreeNode>();

			var records = allRecords.Where(x => (Guid?)x[parentIdFieldName] == nodeId).ToList();
			List<ResponseTreeNode> nodes = new List<ResponseTreeNode>();
			depth++;
			foreach (var record in records)
			{
				if (weightFieldName == "no-weight")
				{
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = null,
						Nodes = GetTreeNodeChildren(allRecords, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth)
					});
				}
				else {
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = (int?)((decimal?)record[weightFieldName]),
						Nodes = GetTreeNodeChildren(allRecords, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth)
					});
				}
			}

			if (weightFieldName == "no-weight")
			{
				return nodes.OrderBy(x => x.Name).ToList();
			}
			else {
				return nodes.OrderBy(x => x.Weight).ThenBy(y => y.Name).ToList();
			}
		}


		// Export list records to csv
		// POST: api/v1/en_US/record/{entityName}/list/{listName}/export
		//[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/{entityName}/list/{listName}/export")]
		//public IActionResult ExportListRecordsToCsv(string entityName, string listName, int count = 10)
		//{
		//	ResponseModel response = new ResponseModel();
		//	response.Message = "Records successfully exported";
		//	response.Timestamp = DateTime.UtcNow;
		//	response.Success = true;
		//	response.Object = null;

		//	EntityListResponse entitiesResponse = entityManager.ReadEntities();
		//	List<Entity> entities = entitiesResponse.Object.Entities;
		//	Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

		//	if (entity == null)
		//	{
		//		response.Timestamp = DateTime.UtcNow;
		//		response.Success = false;
		//		response.Message = "Export failed! Entity with such name does not exist!";
		//		response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
		//		return DoResponse(response);
		//	}

		//	bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Read, entity);
		//	if (!hasPermisstion)
		//	{
		//		response.Success = false;
		//		response.Message = "Export failed! Trying to read records from entity '" + entity.Name + "' with no read access.";
		//		response.Errors.Add(new ErrorModel { Message = "Access denied." });
		//		return DoResponse(response);
		//	}

		//	var random = new Random().Next(10, 99);
		//	DateTime dt = DateTime.Now;
		//	string time = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();
		//	string fileName = $"{entity.Label.Replace(' ', '-').Trim().ToLowerInvariant()}-{time}{random}.csv"; //"goro-test-report.csv";

		//	return new FileGeneratingResult(fileName, "text/csv",
		//		stream => this.GenerateExport(entities, entity, listName, stream, count));
		//}

		//public void GenerateExport(List<Entity> entities, Entity entity, string listName, Stream stream, int count = 10)
		//{
		//	try
		//	{
		//		//var random = new Random().Next(10, 99);
		//		//DateTime dt = DateTime.Now;
		//		//string time = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();
		//		//string fileName = $"{entity.Label.Replace(' ', '-').Trim().ToLowerInvariant()}-{time}{random}.csv"; //"goro-test-report.csv";

		//		Response.ContentType = "application/octet-stream;charset=utf-8";
		//		//Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
		//		//Response.Headers.Add("Content-Length", fileResp.ContentLength.ToString());

		//		int page = 1;
		//		int pageSize = 100;
		//		int offset = 0;

		//		while (true)
		//		{
		//			//var stream = new MemoryStream();
		//			List<EntityRecord> records = new List<EntityRecord>();

		//			if (count > 0 && count < (pageSize * page))
		//			{
		//				pageSize = count < pageSize ? count : (count - (pageSize * (page - 1)));
		//			}

		//			records = GetListRecords(entities, entity, listName, page, null, null, null, pageSize, export: true);

		//			if (records.Count > 0)
		//			{
		//				RecordList listMeta = entity.RecordLists.FirstOrDefault(l => l.Name == listName);

		//				var textWriter = new StreamWriter(stream);
		//				var csv = new CsvWriter(textWriter);

		//				if (page == 1)
		//				{
		//					foreach (var prop in records[0].Properties)
		//					{
		//						string name = prop.Key;
		//						if (prop.Key.StartsWith("$field$"))
		//						{
		//							name = prop.Key.Remove(0, 7);
		//							name = "$" + name.Replace('$', '.');
		//						}
		//						csv.WriteField(name);
		//					}
		//					csv.NextRecord();
		//				}

		//				foreach (var record in records)
		//				{
		//					foreach (var prop in record.Properties)
		//					{
		//						if (prop.Value != null)
		//						{
		//							if (prop.Value is List<object>)
		//							{
		//								csv.WriteField(JsonConvert.SerializeObject(prop.Value).ToString());
		//							}
		//							else if (prop.Value is string)
		//							{
		//								csv.WriteField((string)prop.Value, true);
		//							}
		//							else
		//							{
		//								csv.WriteField(prop.Value);
		//							}
		//						}
		//						else
		//							csv.WriteField("");
		//					}
		//					csv.NextRecord();

		//					textWriter.Flush();
		//				}

		//				textWriter.Close();
		//			}

		//			//byte[] buffer = stream.GetBuffer();
		//			//Response.Body.Write(buffer, offset, buffer.Length);
		//			//offset += buffer.Length;
		//			//Response.Body.Flush();

		//			if (records.Count <= pageSize)
		//				break;

		//			page++;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		//response.Timestamp = DateTime.UtcNow;
		//		//response.Success = false;
		//		//response.Message = ex.Message;
		//		//return DoResponse(response);
		//	}

		//	//var random = new Random().Next(10, 99);
		//	//DateTime dt = DateTime.Now;
		//	//string time = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();
		//	//string fileName = $"{entity.Label.Replace(' ', '-').Trim().ToLowerInvariant()}-{time}{random}.csv"; //"goro-test-report.csv";

		//	//DbFileRepository fs = new DbFileRepository();
		//	//var createdFile = fs.CreateTempFile(fileName, stream.ToArray());

		//	//response.Object = "/fs" + createdFile.FilePath;
		//	//return DoResponse(response);

		//	//return File(stream.GetBuffer(), System.Net.Mime.MediaTypeNames.Application.Octet);

		//	//FileStreamResult result = new FileStreamResult(stream, "text/csv");
		//	//result.FileDownloadName = "testfile.csv";
		//	//return result;

		//	//Response.Body.Close();

		//	//return new EmptyResult();
		//}

		// Import list records to csv
		// POST: api/v1/en_US/record/{entityName}/list/{listName}/import
//		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/{entityName}/import")]
//		public IActionResult ImportEntityRecordsFromCsv(string entityName, [FromBody]JObject postObject)
//		{
//			//The import CSV should have column names matching the names of the imported fields. The first column should be "id" matching the id of the record to be updated. 
//			//If the 'id' of a record equals 'null', a new record will be created with the provided columns and default values for the missing ones.

//			ResponseModel response = new ResponseModel();
//			response.Message = "Records successfully imported";
//			response.Timestamp = DateTime.UtcNow;
//			response.Success = true;
//			response.Object = null;

//			string fileTempPath = "/fs/tmp/02cf04fc81f747938929b81c55559a04/import_1.csv";
//			//if (!postObject.IsNullOrEmpty() && postObject.Properties().Any(p => p.Name == "fileTempPath"))
//			//{
//			//	fileTempPath = postObject["fileTempPath"].ToString();
//			//}

//			//if (string.IsNullOrWhiteSpace(fileTempPath))
//			//{
//			//	response.Timestamp = DateTime.UtcNow;
//			//	response.Success = false;
//			//	response.Message = "Import failed! fileTempPath parameter cannot be empty or null!";
//			//	response.Errors.Add(new ErrorModel("fileTempPath", fileTempPath, "Import failed! File does not exist!"));
//			//	return DoItemNotFoundResponse(response);
//			//}

//			if (!fileTempPath.StartsWith("/"))
//				fileTempPath = "/" + fileTempPath;

//			fileTempPath = fileTempPath.ToLowerInvariant();

//			using (DbConnection connection = DbContext.Current.CreateConnection())
//			{
//				List<EntityRelation> relations = entityRelationManager.Read().Object;
//				EntityListResponse entitiesResponse = entityManager.ReadEntities();
//				List<Entity> entities = entitiesResponse.Object.Entities;
//				Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

//				if (entity == null)
//				{
//					response.Timestamp = DateTime.UtcNow;
//					response.Success = false;
//					response.Message = "Import failed! Entity with such name does not exist!";
//					response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
//					return DoResponse(response);
//				}

//				DbFileRepository fs = new DbFileRepository();
//				DbFile file = fs.Find(fileTempPath);

//				if (file == null)
//				{
//					response.Timestamp = DateTime.UtcNow;
//					response.Success = false;
//					response.Message = "Import failed! File does not exist!";
//					response.Errors.Add(new ErrorModel("fileTempPath", fileTempPath, "Import failed! File does not exist!"));
//					return DoItemNotFoundResponse(response);
//				}

//				byte[] fileBytes = file.GetBytes();
//				Stream fileStream = new MemoryStream();
//				fileStream.Write(fileBytes, 0, fileBytes.Length);
//				TextReader reader = new StreamReader(fileStream);

//				CsvReader csvReader = new CsvReader(reader);
//				csvReader.Configuration.HasHeaderRecord = false;
//				csvReader.Configuration.IsHeaderCaseSensitive = false;

//				csvReader.Read();
//				List<string> columns = csvReader.FieldHeaders.ToList();
//				List<dynamic> fieldMetaList = new List<dynamic>();

//				foreach (var column in columns)
//				{
//					Field field;
//					if (column.Contains(RELATION_SEPARATOR))
//					{
//						var relationData = column.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
//						if (relationData.Count > 2)
//							throw new Exception(string.Format("The specified field name '{0}' is incorrect. Only first level relation can be specified.", column));

//						string relationName = relationData[0];
//						string relationFieldName = relationData[1];

//						if (string.IsNullOrWhiteSpace(relationName) || relationName == "$" || relationName == "$$")
//							throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not specified.", column));
//						else if (!relationName.StartsWith("$"))
//							throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not correct.", column));
//						else
//							relationName = relationName.Substring(1);

//						//check for target priority mark $$
//						if (relationName.StartsWith("$"))
//						{
//							relationName = relationName.Substring(1);
//						}

//						if (string.IsNullOrWhiteSpace(relationFieldName))
//							throw new Exception(string.Format("Invalid relation '{0}'. The relation field name is not specified.", column));

//						var relation = relations.SingleOrDefault(x => x.Name == relationName);
//						if (relation == null)
//							throw new Exception(string.Format("Invalid relation '{0}'. The relation does not exist.", column));

//						if (relation.TargetEntityId != entity.Id && relation.OriginEntityId != entity.Id)
//							throw new Exception(string.Format("Invalid relation '{0}'. The relation field belongs to entity that does not relate to current entity.", column));

//						Entity relationEntity = null;

//						if (relation.OriginEntityId == entity.Id)
//						{
//							relationEntity = entities.FirstOrDefault(e => e.Id == relation.TargetEntityId);
//							field = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
//						}
//						else
//						{
//							relationEntity = entities.FirstOrDefault(e => e.Id == relation.OriginEntityId);
//							field = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
//						}
//					}
//					else
//					{
//						field = entity.Fields.FirstOrDefault(f => f.Name == column);
//					}

//					dynamic fieldMeta = new ExpandoObject();
//					fieldMeta.ColumnName = column;
//					fieldMeta.FieldType = field.GetFieldType();

//					fieldMetaList.Add(fieldMeta);
//				}

//				connection.BeginTransaction();

//				try
//				{
//					while (csvReader.Read())
//					{
//						EntityRecord newRecord = new EntityRecord();
//						foreach (var fieldMeta in fieldMetaList)
//						{
//							switch ((FieldType)fieldMeta.FieldType)
//							{
//								case FieldType.AutoNumberField:
//								case FieldType.CurrencyField:
//								case FieldType.NumberField:
//								case FieldType.PercentField:
//									{
//										newRecord[fieldMeta.ColumnName] = csvReader.GetField<decimal>(fieldMeta.ColumnName);
//									}
//									break;
//								case FieldType.CheckboxField:
//									{
//										newRecord[fieldMeta.ColumnName] = csvReader.GetField<bool>(fieldMeta.ColumnName);
//									}
//									break;
//								case FieldType.DateField:
//								case FieldType.DateTimeField:
//									{
//										newRecord[fieldMeta.ColumnName] = csvReader.GetField<DateTime>(fieldMeta.ColumnName);
//									}
//									break;
//								case FieldType.MultiSelectField:
//									{
//										newRecord[fieldMeta.ColumnName] = csvReader.GetField<string[]>(fieldMeta.ColumnName);
//									}
//									break;
//								case FieldType.TreeSelectField:
//									{
//										newRecord[fieldMeta.ColumnName] = csvReader.GetField<Guid[]>(fieldMeta.ColumnName);
//									}
//									break;
//								default:
//									{
//										newRecord[fieldMeta.ColumnName] = csvReader.GetField<string>(fieldMeta.ColumnName);
//									}
//									break;
//							}
//						}
//						if (!newRecord.GetProperties().Any(x => x.Key == "id") || string.IsNullOrEmpty(newRecord["id"] as string))
//						{
//							newRecord["id"] = Guid.NewGuid();
//							QueryResponse result = recMan.CreateRecord(entityName, newRecord);
//						}
//						else
//						{
//							QueryResponse result = recMan.UpdateRecord(entityName, newRecord);
//						}
//					}

//					reader.Close();
//					fileStream.Close();

//					connection.CommitTransaction();
//				}
//				catch (Exception e)
//				{
//					connection.RollbackTransaction();

//					response.Success = false;
//					response.Object = null;
//					response.Timestamp = DateTime.UtcNow;
//#if DEBUG
//					response.Message = e.Message + e.StackTrace;
//#else
//					response.Message = "Import failed! An internal error occurred!";
//#endif
//				}

//				return DoResponse(response);
//			}
//		}

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

			DbFileRepository fsRepository = new DbFileRepository();
			var file = fsRepository.Find(filepath);

			if (file == null)
				return DoPageNotFoundResponse();

			string mimeType;
			var extension = Path.GetExtension(filepath);
			new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out mimeType);

			return File(file.GetBytes(), mimeType);
		}

		[AcceptVerbs(new[] { "POST" }, Route = "/fs/upload/")]
		public IActionResult UploadFile([FromForm] IFormFile file)
		{

			var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').ToLowerInvariant();
			DbFileRepository fsRepository = new DbFileRepository();
			var createdFile = fsRepository.CreateTempFile(fileName, ReadFully(file.OpenReadStream()));

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

			DbFileRepository fsRepository = new DbFileRepository();
			var sourceFile = fsRepository.Find(source);

			var movedFile = fsRepository.Move(source, target, overwrite);
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

			DbFileRepository fsRepository = new DbFileRepository();
			var sourceFile = fsRepository.Find(filepath);

			fsRepository.Delete(filepath);
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

