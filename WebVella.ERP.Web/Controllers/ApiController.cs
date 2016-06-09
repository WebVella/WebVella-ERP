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
using WebVella.ERP.Plugins;
using WebVella.ERP.WebHooks;
using System.Diagnostics;
using Npgsql;
using System.Data;



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebVella.ERP.Web.Controllers
{
	public class ApiController : ApiControllerBase
	{
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

		//TODO - add created_by and modified_by fields where needed, when the login is done
		RecordManager recMan;
		EntityManager entMan;
		EntityRelationManager relMan;
		SecurityManager secMan;
		IWebHookService hooksService;

		public ApiController(IWebHookService hooksService)
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entMan = new EntityManager();
			relMan = new EntityRelationManager();
			this.hooksService = hooksService;
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
			var bo = entMan.ReadEntities();
			return DoResponse(bo);
		}

		// Get entity meta
		// GET: api/v1/en_US/meta/entity/id/{entityId}/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/id/{entityId}")]
		public IActionResult GetEntityMetaById(Guid entityId)
		{
			return DoResponse(entMan.ReadEntity(entityId));
		}

		// Get entity meta
		// GET: api/v1/en_US/meta/entity/{name}/
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}")]
		public IActionResult GetEntityMeta(string Name)
		{
			return DoResponse(entMan.ReadEntity(Name));
		}


		// Create an entity
		// POST: api/v1/en_US/meta/entity
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
		public IActionResult CreateEntity([FromBody]InputEntity submitObj)
		{
			return DoResponse(entMan.CreateEntity(submitObj));
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

			return DoResponse(entMan.UpdateEntity(entity));
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
				response = entMan.DeleteEntity(newGuid);
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

			return DoResponse(entMan.CreateField(entityId, field));
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

			return DoResponse(entMan.UpdateField(entityId, field));
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

			return DoResponse(entMan.UpdateField(entity, field));
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

			return DoResponse(entMan.DeleteField(entityId, fieldId));
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

			return DoResponse(entMan.CreateRecordList(Name, list));
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

			return DoResponse(entMan.UpdateRecordList(Name, list));
		}

		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}")]
		public IActionResult PatchRecordListByName(string Name, string ListName, [FromBody]JObject submitObj)
		{
			RecordListResponse response = new RecordListResponse();
			Entity entity = new Entity();
			InputRecordList list = new InputRecordList();

			try
			{
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
					if (prop.Name.ToLower() == "title")
						list.Title = inputList.Title;
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
					if (prop.Name.ToLower() == "datasourceurl")
						list.DataSourceUrl = inputList.DataSourceUrl;
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

			return DoResponse(entMan.UpdateRecordList(entity, list));
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}")]
		public IActionResult DeleteRecordListByName(string Name, string ListName)
		{
			return DoResponse(entMan.DeleteRecordList(Name, ListName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}")]
		public IActionResult GetRecordListByName(string Name, string ListName)
		{
			return DoResponse(entMan.ReadRecordList(Name, ListName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/list")]
		public IActionResult GetRecordListsByName(string Name)
		{
			return DoResponse(entMan.ReadRecordLists(Name));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/list/{ListName}/service.js")]
		public IActionResult GetRecordListServiceJSByName(string Name, string ListName, bool defaultScript = false)
		{

			var list = entMan.ReadRecordList(Name, ListName);
			if (list == null || list.Object == null || list.Success == false)
				return DoPageNotFoundResponse();

			var code = list.Object.ServiceCode;
			if (string.IsNullOrWhiteSpace(code) || defaultScript)
				return File("/plugins/webvella-core/providers/list_default_service_script.js", "text/javascript");
			else if (code.StartsWith("/plugins/") || code.StartsWith("http://") || code.StartsWith("https://"))
			{
				return File(code, "text/javascript");
			}
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(code);
			return File(bytes, "text/javascript");
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


			var entity = entMan.ReadEntity(entityName).Object;
			if (entity == null)
			{
				result.Errors.Add(new ErrorModel { Message = "Entity not found." });
				result.Success = false;
				return DoResponse(result);
			}

			entity = Helpers.DeepClone(entMan.ReadEntity(entityName).Object);
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

					Entity relatedEntity = Helpers.DeepClone(entMan.ReadEntity(treeField.RelatedEntityId).Object);
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
				Entity relatedEntity = Helpers.DeepClone(entMan.ReadEntity(relatedEntityId).Object);

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
			else if (field is MultiSelectField)
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

			return DoResponse(entMan.CreateRecordView(Name, view));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}/service.js")]
		public IActionResult GetRecordViewServiceJSByName(string Name, string ViewName, bool defaultScript = false)
		{
			var view = entMan.ReadRecordView(Name, ViewName);
			if (view == null || view.Object == null || view.Success == false)
				return DoPageNotFoundResponse();

			var code = view.Object.ServiceCode;
			if (string.IsNullOrWhiteSpace(code) || defaultScript)
				return File("/plugins/webvella-core/providers/view_default_service_script.js", "text/javascript");
			else if (code.StartsWith("/plugins/") || code.StartsWith("http://") || code.StartsWith("https://"))
			{
				return File(code, "text/javascript");
			}
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(code);
			return File(bytes, "text/javascript");
		}

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

			return DoResponse(entMan.UpdateRecordView(Name, view));
		}


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
					if (prop.Name.ToLower() == "title")
						view.Title = inputView.Title;
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
					if (prop.Name.ToLower() == "datasourceurl")
						view.DataSourceUrl = inputView.DataSourceUrl;
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

			return DoResponse(entMan.UpdateRecordView(entity, view));
		}

		//[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Id}/view/{ViewId}")]
		//public IActionResult DeleteRecordView(Guid Id, Guid ViewId)
		//{
		//    return DoResponse(entityManager.DeleteRecordView(Id, ViewId));
		//}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult DeleteRecordViewByName(string Name, string ViewName)
		{
			return DoResponse(entMan.DeleteRecordView(Name, ViewName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view/{ViewName}")]
		public IActionResult GetRecordViewByName(string Name, string ViewName)
		{
			return DoResponse(entMan.ReadRecordView(Name, ViewName));
		}

		//[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Id}/view")]
		//      public IActionResult GetRecordViews(Guid Id)
		//      {
		//          return DoResponse(entityManager.ReadRecordViews(Id));
		//      }

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}/view")]
		public IActionResult GetRecordViewsByName(string Name)
		{
			return DoResponse(entMan.ReadRecordViews(Name));
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

			return DoResponse(entMan.CreateRecordTree(entityName, tree));
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

			return DoResponse(entMan.UpdateRecordTree(entityName, tree));
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
			var updateResponse = entMan.UpdateRecordTree(entity, tree);
			return DoResponse(updateResponse);
		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree/{treeName}")]
		public IActionResult DeleteRecordTreeByName(string entityName, string treeName)
		{
			return DoResponse(entMan.DeleteRecordTree(entityName, treeName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree/{treeName}")]
		public IActionResult GetRecordTreeByName(string entityName, string treeName)
		{
			return DoResponse(entMan.ReadRecordTree(entityName, treeName));
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{entityName}/tree")]
		public IActionResult GetRecordTreesByEntityName(string entityName)
		{
			return DoResponse(entMan.ReadRecordTrees(entityName));
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

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << manage_relation_input_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				//Hook for the origin entity
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = model;
				hookFilterObj.relation = relation;
				hookFilterObj.originEntity = originEntity;
				hookFilterObj.targetEntity = targetEntity;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationInput, originEntity.Name, hookFilterObj);
				model = hookFilterObj.record;

				//Hook for the target entity
				hookFilterObj = new ExpandoObject();
				hookFilterObj.record = model;
				hookFilterObj.relation = relation;
				hookFilterObj.originEntity = originEntity;
				hookFilterObj.targetEntity = targetEntity;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationInput, targetEntity.Name, hookFilterObj);
				model = hookFilterObj.record;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook manage_relation_input_filter: " + ex.Message));
			}// <<<		


			EntityQuery query = new EntityQuery(originEntity.Name, "id," + originField.Name, EntityQuery.QueryEQ("id", model.OriginFieldRecordId), null, null, null);
			QueryResponse result = recMan.Find(query);
			if (result.Object.Data.Count == 0)
			{
				response.Errors.Add(new ErrorModel { Message = "Origin record was not found. Id=[" + model.OriginFieldRecordId + "]", Key = "originFieldRecordId" });
				response.Success = false;
				return DoResponse(response);
			}

			var originRecord = result.Object.Data[0];
			object originValue = originRecord[originField.Name];

			var attachTargetRecords = new List<EntityRecord>();
			var detachTargetRecords = new List<EntityRecord>();

			foreach (var targetId in model.AttachTargetFieldRecordIds)
			{
				query = new EntityQuery(targetEntity.Name, "id," + targetField.Name, EntityQuery.QueryEQ("id", targetId), null, null, null);
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
				query = new EntityQuery(targetEntity.Name, "id," + targetField.Name, EntityQuery.QueryEQ("id", targetId), null, null, null);
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

				//////////////////////////////////////////////////////////////////////////////////////
				//WEBHOOK FILTER << manage_relation_pre_save_filter >>
				//////////////////////////////////////////////////////////////////////////////////////
				try
				{
					//Hook for the origin entity
					dynamic hookFilterObj = new ExpandoObject();
					hookFilterObj.attachTargetRecords = attachTargetRecords;
					hookFilterObj.detachTargetRecords = detachTargetRecords;
					hookFilterObj.originRecord = originRecord;
					hookFilterObj.originEntity = originEntity;
					hookFilterObj.targetEntity = targetEntity;
					hookFilterObj.relation = relation;
					hookFilterObj.controller = this;
					hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationPreSave, originEntity.Name, hookFilterObj);
					attachTargetRecords = hookFilterObj.attachTargetRecords;
					detachTargetRecords = hookFilterObj.detachTargetRecords;

					//Hook for the target entity
					hookFilterObj = new ExpandoObject();
					hookFilterObj.attachTargetRecords = attachTargetRecords;
					hookFilterObj.detachTargetRecords = detachTargetRecords;
					hookFilterObj.originRecord = originRecord;
					hookFilterObj.originEntity = originEntity;
					hookFilterObj.targetEntity = targetEntity;
					hookFilterObj.relation = relation;
					hookFilterObj.controller = this;
					hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationPreSave, targetEntity.Name, hookFilterObj);
					attachTargetRecords = hookFilterObj.attachTargetRecords;
					detachTargetRecords = hookFilterObj.detachTargetRecords;
				}
				catch (Exception ex)
				{
					return Json(CreateErrorResponse("Plugin error in web hook manage_relation_pre_save_filter: " + ex.Message));
				}// <<<	


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
									var patchObject = new EntityRecord();
									patchObject["id"] = (Guid)record["id"];
									patchObject[targetField.Name] = originValue;

									var updResult = recMan.UpdateRecord(targetEntity, patchObject);
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

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK ACTION << manage_relation_success_action >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = model;
				hookFilterObj.result = result;
				hookFilterObj.relation = relation;
				hookFilterObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.ManageRelationAction, originEntity.Name, hookFilterObj);
				hookFilterObj = new ExpandoObject();
				hookFilterObj.record = model;
				hookFilterObj.result = result;
				hookFilterObj.relation = relation;
				hookFilterObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.ManageRelationAction, targetEntity.Name, hookFilterObj);
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook create_record_success_action: " + ex.Message));
			}// <<<		

			return DoResponse(response);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult GetRecord(Guid recordId, string entityName, string fields = "*")
		{
			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << get_record_input_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.GetRecordInput, entityName, hookFilterObj);
				recordId = hookFilterObj.recordId;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook get_record_input_filter: " + ex.Message));
			}// <<<

			QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);

			EntityQuery query = new EntityQuery(entityName, fields, filterObj, null, null, null);

			QueryResponse result = recMan.Find(query);
			if (!result.Success)
				return DoResponse(result);

			EntityRecord record = result.Object.Data[0];
			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << get_record_output_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = record;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.GetRecordOutput, entityName, hookFilterObj);
				record = hookFilterObj.record;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook get_record_output_filter: " + ex.Message));
			}// <<<

			result.Object.Data[0] = record;

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK ACTION << get_record_success_action >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.recordId = recordId;
				hookFilterObj.result = result;
				hookFilterObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.GetRecordAction, entityName, hookFilterObj);
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook get_record_success_action: " + ex.Message));
			}// <<<

			return Json(result);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult DeleteRecord(Guid recordId, string entityName)
		{
			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << delete_record_input_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.DeleteRecordInput, entityName, hookFilterObj);
				recordId = hookFilterObj.recordId;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook delete_record_input_filter: " + ex.Message));
			}// <<<

			var validationErrors = new List<ErrorModel>();

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << delete_record_validation_errors_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.errors = validationErrors;
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.DeleteRecordValidationErrors, entityName, hookFilterObj);
				validationErrors = hookFilterObj.errors;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook delete_record_validation_errors_filter: " + ex.Message));
			}// <<<

			if (validationErrors.Count > 0)
			{
				var response = new ResponseModel();
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Errors = validationErrors;
				response.Message = "Validation error occurred!";
				response.Object = null;
				return Json(response);
			}

			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();

					//////////////////////////////////////////////////////////////////////////////////////
					//WEBHOOK FILTER << delete_record_pre_save_filter >>
					//////////////////////////////////////////////////////////////////////////////////////
					try
					{
						dynamic hookFilterObj = new ExpandoObject();
						hookFilterObj.recordId = recordId;
						hookFilterObj.controller = this;
						hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.DeleteRecordPreSave, entityName, hookFilterObj);
						recordId = hookFilterObj.recordId;
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						return Json(CreateErrorResponse("Plugin error in web hook delete_record_pre_save_filter: " + ex.Message));
					}// <<<

					result = recMan.DeleteRecord(entityName, recordId);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					var response = new ResponseModel();
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error while delete the record: " + ex.Message;
					response.Object = null;
					return Json(response);
				}
			}

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK ACTION << delete_record_success_action >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.recordId = recordId;
				hookFilterObj.result = result;
				hookFilterObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.DeleteRecordAction, entityName, hookFilterObj);
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook delete_record_success_action: " + ex.Message));
			}// <<<

			return DoResponse(result);
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
			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << create_record_input_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = postObj;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.CreateRecordInput, entityName, hookFilterObj);
				postObj = hookFilterObj.record;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook create_record_input_filter: " + ex.Message));
			}// <<<

			var validationErrors = new List<ErrorModel>();
			//TODO implement validation
			if (postObj == null)
				postObj = new EntityRecord();

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << create_record_validation_errors_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.errors = validationErrors;
				hookFilterObj.record = postObj;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.CreateRecordValidationErrors, entityName, hookFilterObj);
				validationErrors = hookFilterObj.errors;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook create_record_validation_errors_filter: " + ex.Message));
			}// <<<

			if (validationErrors.Count > 0)
			{
				var response = new ResponseModel();
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Errors = validationErrors;
				response.Message = "Validation error occurred!";
				response.Object = null;
				return Json(response);
			}

			if (!postObj.GetProperties().Any(x => x.Key == "id"))
				postObj["id"] = Guid.NewGuid();
			else if (string.IsNullOrEmpty(postObj["id"] as string))
				postObj["id"] = Guid.NewGuid();


			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					//////////////////////////////////////////////////////////////////////////////////////
					//WEBHOOK FILTER << create_record_pre_save_filter >>
					//////////////////////////////////////////////////////////////////////////////////////
					try
					{
						dynamic hookFilterObj = new ExpandoObject();
						hookFilterObj.record = postObj;
						hookFilterObj.controller = this;
						hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.CreateRecordPreSave, entityName, hookFilterObj);
						postObj = hookFilterObj.record;
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						return Json(CreateErrorResponse("Plugin error in web hook create_record_pre_save_filter: " + ex.Message));
					}// <<<

					result = recMan.CreateRecord(entityName, postObj);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					var response = new ResponseModel();
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error while saving the record: " + ex.Message;
					response.Object = null;
					return Json(response);
				}
			}

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK ACTION << create_record >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = postObj;
				hookFilterObj.result = result;
				hookFilterObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.CreateRecordAction, entityName, hookFilterObj);
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook create_record_success_action: " + ex.Message));
			}// <<<						

			return DoResponse(result);
		}

		// Update an entity record
		// PUT: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult UpdateEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << update_record_input_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			#region
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = postObj;
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.UpdateRecordInput, entityName, hookFilterObj);
				postObj = hookFilterObj.record;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook update_record_input_filter: " + ex.Message));
			}// <<<	
			#endregion

			var validationErrors = new List<ErrorModel>();
			//TODO implement validation

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << update_record_validation_errors_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			#region
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.errors = validationErrors;
				hookFilterObj.record = postObj;
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.UpdateRecordValidationErrors, entityName, hookFilterObj);
				validationErrors = hookFilterObj.errors;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook update_record_validation_errors_filter: " + ex.Message));
			}// <<<
			#endregion

			if (validationErrors.Count > 0)
			{
				var response = new ResponseModel();
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Errors = validationErrors;
				response.Message = "Validation error occurred!";
				response.Object = null;
				return Json(response);
			}

			//clear authentication cache
			if (entityName == "user")
				WebSecurityUtil.RemoveIdentityFromCache(recordId);

			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					//////////////////////////////////////////////////////////////////////////////////////
					//WEBHOOK FILTER << update_record_pre_save_filter >>
					//////////////////////////////////////////////////////////////////////////////////////
					#region
					try
					{
						dynamic hookFilterObj = new ExpandoObject();
						hookFilterObj.record = postObj;
						hookFilterObj.controller = this;
						hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.UpdateRecordPreSave, entityName, hookFilterObj);
						postObj = hookFilterObj.record;
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						return Json(CreateErrorResponse("Plugin error in web hook update_record_pre_save_filter: " + ex.Message));
					}// <<<
					#endregion

					result = recMan.UpdateRecord(entityName, postObj);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					var response = new ResponseModel();
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error while saving the record: " + ex.Message;
					response.Object = null;
					return Json(response);
				}
			}

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK ACTION << update_record_success_action >>
			//////////////////////////////////////////////////////////////////////////////////////
			#region
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = postObj;
				hookFilterObj.oldRecord = postObj;
				hookFilterObj.result = result;
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.UpdateRecordAction, entityName, hookFilterObj);
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook update_record_success_action: " + ex.Message));
			}// <<<
			#endregion

			return DoResponse(result);
		}

		// Patch an entity record
		// PATCH: api/v1/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v1/en_US/record/{entityName}/{recordId}")]
		public IActionResult PatchEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << patch_record_input_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = postObj;
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.PatchRecordInput, entityName, hookFilterObj);
				postObj = hookFilterObj.record;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook patch_record_input_filter: " + ex.Message));
			}// <<<

			var validationErrors = new List<ErrorModel>();
			//TODO implement validation
			if (postObj == null)
				postObj = new EntityRecord();

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK FILTER << patch_record_validation_errors_filter >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.errors = validationErrors;
				hookFilterObj.record = postObj;
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.PatchRecordValidationErrors, entityName, hookFilterObj);
				validationErrors = hookFilterObj.errors;
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook patch_record_validation_errors_filter: " + ex.Message));
			}// <<<

			if (validationErrors.Count > 0)
			{
				var response = new ResponseModel();
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Errors = validationErrors;
				response.Message = "Validation error occurred!";
				response.Object = null;
				return Json(response);
			}


			//clear authentication cache
			if (entityName == "user")
				WebSecurityUtil.RemoveIdentityFromCache(recordId);
			postObj["id"] = recordId;

			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					//////////////////////////////////////////////////////////////////////////////////////
					//WEBHOOK FILTER << patch_record_pre_save_filter >>
					//////////////////////////////////////////////////////////////////////////////////////
					try
					{
						dynamic hookFilterObj = new ExpandoObject();
						hookFilterObj.record = postObj;
						hookFilterObj.recordId = recordId;
						hookFilterObj.controller = this;
						hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.PatchRecordPreSave, entityName, hookFilterObj);
						postObj = hookFilterObj.record;
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						return Json(CreateErrorResponse("Plugin error in web hook patch_record_pre_save_filter: " + ex.Message));
					}// <<<
					result = recMan.UpdateRecord(entityName, postObj);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					var response = new ResponseModel();
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error while saving the record: " + ex.Message;
					response.Object = null;
					return Json(response);
				}
			}

			//////////////////////////////////////////////////////////////////////////////////////
			//WEBHOOK ACTION << patch_record_success_action >>
			//////////////////////////////////////////////////////////////////////////////////////
			try
			{
				dynamic hookFilterObj = new ExpandoObject();
				hookFilterObj.record = postObj;
				hookFilterObj.result = result;
				hookFilterObj.recordId = recordId;
				hookFilterObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.PatchRecordAction, entityName, hookFilterObj);
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook patch_record_success_action: " + ex.Message));
			}// <<<	

			return DoResponse(result);
		}

		// Get an entity record list
		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/list/{listName}/{page}")]
		public IActionResult GetRecordListByEntityName(string entityName, string listName, int page, int? pageSize = null)
		{

			EntityListResponse entitiesResponse = entMan.ReadEntities();
			List<Entity> entities = entitiesResponse.Object.Entities;

			var response = new RecordListRecordResponse();
			response.Message = "Success";
			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Object = null;

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
				response.Object = GetListRecords(entities, entity, listName, page, null, pageSize);
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

		// GET: api/v1/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/list")]
		public IActionResult GetRecordsByEntityName(string entityName, string ids = "", string fields = "", int? limit = null)
		{
			var response = new QueryResponse();
			var recordIdList = new List<Guid>();
			var fieldList = new List<string>();

			if (!String.IsNullOrWhiteSpace(ids) && ids != "null")
			{
				var idStringList = ids.Split(',');
				var outGuid = Guid.Empty;
				foreach (var idString in idStringList)
				{
					if (Guid.TryParse(idString, out outGuid))
					{
						recordIdList.Add(outGuid);
					}
					else
					{
						response.Message = "One of the record ids is not a Guid";
						response.Timestamp = DateTime.UtcNow;
						response.Success = false;
						response.Object.Data = null;
					}
				}
			}

			if (!String.IsNullOrWhiteSpace(fields) && fields != "null")
			{
				var fieldsArray = fields.Split(',');
				var hasId = false;
				foreach (var fieldName in fieldsArray)
				{
					if (fieldName == "id")
					{
						hasId = true;
					}
					fieldList.Add(fieldName);
				}
				if (!hasId)
				{
					fieldList.Add("id");
				}
			}

			var QueryList = new List<QueryObject>();
			foreach (var recordId in recordIdList)
			{
				QueryList.Add(EntityQuery.QueryEQ("id", recordId));
			}

			QueryObject recordsFilterObj = null;
			if (QueryList.Count > 0)
			{
				recordsFilterObj = EntityQuery.QueryOR(QueryList.ToArray());
			}

			var columns = "*";
			if (fieldList.Count > 0)
			{
				if (!fieldList.Contains("id"))
				{
					fieldList.Add("id");
				}
				columns = String.Join(",", fieldList.Select(x => x.ToString()).ToArray());
			}

			//var sortRulesList = new List<QuerySortObject>();
			//var sortRule = new QuerySortObject("id",QuerySortType.Descending);
			//sortRulesList.Add(sortRule);
			//EntityQuery query = new EntityQuery(entityName, columns, recordsFilterObj, sortRulesList.ToArray(), null, null);

			EntityQuery query = new EntityQuery(entityName, columns, recordsFilterObj, null, null, null);
			if (limit != null && limit > 0)
			{
				query = new EntityQuery(entityName, columns, recordsFilterObj, null, null, limit);
			}

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
			EntityListResponse entitiesResponse = entMan.ReadEntities();
			List<Entity> entities = entitiesResponse.Object.Entities;

			RecordViewRecordResponse response = new RecordViewRecordResponse();
			response.Message = "Success";
			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Object = null;

			Entity entity = entities.FirstOrDefault(e => e.Name == entityName);

			if (entity == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				response.Errors.Add(new ErrorModel("entityName", entityName, "Entity with such name does not exist!"));
				return DoResponse(response);
			}

			response.Object = GetViewRecords(entities, entity, viewName, "id", id);
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
			List<Entity> entities = entMan.ReadEntities().Object.Entities;

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
				else
				{
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
				else
				{
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
			else
			{
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
				else
				{
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
			else
			{
				return nodes.OrderBy(x => x.Weight).ThenBy(y => y.Name).ToList();
			}
		}


		private QueryResponse CreateErrorResponse(string message)
		{
			var response = new QueryResponse();
			response.Success = false;
			response.Timestamp = DateTime.UtcNow;
			response.Message = message;
			response.Object = null;
			return response;
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

		#region << Plugins >>
		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/plugin/list")]
		public IActionResult GetPlugins()
		{
			var responseObj = new ResponseModel();
			responseObj.Object = new PluginService().Plugins;
			responseObj.Success = true;
			responseObj.Timestamp = DateTime.UtcNow;
			return DoResponse(responseObj);
		}
		#endregion

		#region << Admin Tools >>
		[AcceptVerbs(new[] { "POST" }, Route = "/admin/tools/evaluate-changes")]
		public IActionResult Index([FromBody]JObject connectionStringObject)
		{

			var connectionString = (string)connectionStringObject["connectionString"];
			var includeEntityMeta = (bool)connectionStringObject["includeEntityMeta"];
			var includeEntityRelations = (bool)connectionStringObject["includeEntityRelations"];
			var includeAreas = (bool)connectionStringObject["includeAreas"];
			var includeRoles = (bool)connectionStringObject["includeRoles"];

			var response = new MetaChangeResponseModel();
			if(string.IsNullOrEmpty(connectionString)){
				response.Success = false;
				response.Message = "Connection string is required";
				return Json(response);
			}
			
			string OLD_DB_CONNECTION_STRING = connectionString;
			
			try
			{

				var changeRow = new MetaChangeModel();
				//Entity
				var currentEntityList = new List<DbEntity>();
				var oldEntityList = new List<DbEntity>();
				var oldEntityDictionary = new Dictionary<Guid, DbEntity>();
				var oldEntityProcessedDictionary = new Dictionary<Guid, bool>();
				//Field
				var oldEntityFieldsList = new List<DbBaseField>();
				var oldEntityFieldsDictionary = new Dictionary<Guid, DbBaseField>();
				var oldEntityFieldsProcessedDictionary = new Dictionary<Guid, bool>();
				//View
				var oldEntityRecordViewList = new List<DbRecordView>();
				var oldEntityRecordViewDictionary = new Dictionary<Guid, DbRecordView>();
				var oldEntityRecordViewProcessedDictionary = new Dictionary<Guid, bool>();
				//List
				var oldEntityRecordListList = new List<DbRecordList>();
				var oldEntityRecordListDictionary = new Dictionary<Guid, DbRecordList>();
				var oldEntityRecordListProcessedDictionary = new Dictionary<Guid, bool>();
				//Tree
				var oldEntityRecordTreeList = new List<DbRecordTree>();
				var oldEntityRecordTreeDictionary = new Dictionary<Guid, DbRecordTree>();
				var oldEntityRecordTreeProcessedDictionary = new Dictionary<Guid, bool>();

				//Relations
				var currentRelationsList = new List<DbEntityRelation>();
				var oldRelationsList = new List<DbEntityRelation>();
				var oldRelationsDictionary = new Dictionary<Guid, DbEntityRelation>();
				var oldRelationsProcessedDictionary = new Dictionary<Guid, bool>();

				//Area
				var currentAreaList = new List<EntityRecord>();
				var oldAreaList = new List<EntityRecord>();
				var oldAreasDictionary = new Dictionary<Guid, EntityRecord>();
				var oldAreasProcessedDictionary = new Dictionary<Guid, bool>();

				//Roles
				var currentRoleList = new List<EntityRecord>();
				var oldRoleList = new List<EntityRecord>();
				var oldRolesDictionary = new Dictionary<Guid, EntityRecord>();
				var oldRolesProcessedDictionary = new Dictionary<Guid, bool>();

				var query = new EntityQuery("area");
				var queryRole = new EntityQuery("role");

				#region << Get elements >>
				currentEntityList = DbContext.Current.EntityRepository.Read();
				currentAreaList = DbContext.Current.RecordRepository.Find(query);
				currentRelationsList = DbContext.Current.RelationRepository.Read();
				currentRoleList = DbContext.Current.RecordRepository.Find(queryRole);

				oldEntityList = ReadOldEntities(OLD_DB_CONNECTION_STRING);
				oldAreaList = ReadOldAreas(OLD_DB_CONNECTION_STRING);
				oldRelationsList = ReadOldRelations(OLD_DB_CONNECTION_STRING);
				oldRoleList = ReadOldRoles(OLD_DB_CONNECTION_STRING);
				#endregion

				if(includeAreas) {
				#region << Process areas >>

				#region << Init >>
				foreach (var area in oldAreaList)
				{
					oldAreasDictionary[(Guid)area["id"]] = area;
				}
				#endregion

				#region << Logic >>
				foreach (var area in currentAreaList)
				{
					if (!oldAreasDictionary.ContainsKey((Guid)area["id"]))
					{
						//// CREATED
						/////////////////////////////////////////////////////
						changeRow = new MetaChangeModel();
						changeRow.Element = "area";
						changeRow.Type = "created";
						changeRow.Name = (string)area["name"];
						response.Changes.Add(changeRow);
						response.Code += CreateAreaCode(area);
					}
					else
					{
						//// POSSIBLE UPDATE
						/////////////////////////////////////////////////////
						var changeCheckResponse = UpdateAreaCode(area, oldAreasDictionary[(Guid)area["id"]]);
						if (changeCheckResponse.HasUpdate)
						{
							//1.1 Updated
							changeRow = new MetaChangeModel();
							changeRow.Element = "area";
							changeRow.Type = "updated";
							changeRow.Name = (string)area["name"];
							changeRow.ChangeList = changeCheckResponse.ChangeList;
							response.Changes.Add(changeRow);
							response.Code += changeCheckResponse.Code;
						}

						// MARK ID AS PROCESSED
						/////////////////////////////////////////////////////
						oldAreasProcessedDictionary[(Guid)area["id"]] = true;
					}

				}

				foreach (var area in oldAreaList)
				{
					if (!oldAreasProcessedDictionary.ContainsKey((Guid)area["id"]))
					{
						//// DELETED
						/////////////////////////////////////////////////////
						changeRow = new MetaChangeModel();
						changeRow.Element = "area";
						changeRow.Type = "deleted";
						changeRow.Name = (string)area["name"];
						response.Changes.Add(changeRow);
						response.Code += DeleteAreaCode(area);
					}
				}

				#endregion

				#endregion
				}
				if(includeEntityMeta) {
				#region << Process entity >>

				#region << Init >>
				foreach (var entity in oldEntityList)
				{
					oldEntityDictionary[entity.Id] = entity;
					foreach (var field in entity.Fields)
					{
						oldEntityFieldsList.Add(field);
						oldEntityFieldsDictionary[field.Id] = field;
					}
					foreach (var view in entity.RecordViews)
					{
						oldEntityRecordViewList.Add(view);
						oldEntityRecordViewDictionary[view.Id] = view;
					}
					foreach (var list in entity.RecordLists)
					{
						oldEntityRecordListList.Add(list);
						oldEntityRecordListDictionary[list.Id] = list;
					}
					foreach (var tree in entity.RecordTrees)
					{
						oldEntityRecordTreeList.Add(tree);
						oldEntityRecordTreeDictionary[tree.Id] = tree;
					}
				}
				#endregion

				#region << Logic >>
				foreach (var entity in currentEntityList)
				{
					if (!oldEntityDictionary.ContainsKey(entity.Id))
					{
						//// CREATED
						/////////////////////////////////////////////////////
						changeRow = new MetaChangeModel();
						changeRow.Element = "entity";
						changeRow.Type = "created";
						changeRow.Name = entity.Name;
						response.Changes.Add(changeRow);
						response.Code += CreateEntityCode(entity);
					}
					else
					{
						//// POSSIBLE UPDATE
						/////////////////////////////////////////////////////
						var changeCheckResponse = UpdateEntityCode(entity, oldEntityDictionary[entity.Id]);
						if (changeCheckResponse.HasUpdate)
						{
							//1.1 Updated
							changeRow = new MetaChangeModel();
							changeRow.Element = "entity";
							changeRow.Type = "updated";
							changeRow.Name = entity.Name;
							changeRow.ChangeList = changeCheckResponse.ChangeList;
							response.Changes.Add(changeRow);
							response.Code += changeCheckResponse.Code;
						}


						// MARK ID AS PROCESSED
						/////////////////////////////////////////////////////
						oldEntityProcessedDictionary[entity.Id] = true;
					}

				}

				foreach (var entity in oldEntityList)
				{
					if (!oldEntityProcessedDictionary.ContainsKey(entity.Id))
					{
						//// DELETED
						/////////////////////////////////////////////////////
						changeRow = new MetaChangeModel();
						changeRow.Element = "entity";
						changeRow.Type = "deleted";
						changeRow.Name = entity.Name;
						response.Changes.Add(changeRow);
						response.Code += DeleteEntityCode(entity);
					}
				}
				#endregion

				#endregion
				}
				if(includeEntityRelations) {
				#region << Process relations >>

				#region << Init >>
				foreach (var relation in oldRelationsList)
				{
					if(relation.Name == "user_role_created_by") {
						var boz = 0;
					}
					oldRelationsDictionary[relation.Id] = relation;
				}
				#endregion

				#region << Logic >>
				foreach (var relation in currentRelationsList)
				{
					if(relation.Name == "user_role_created_by") {
						var boz = 0;
					}
					if (!oldRelationsDictionary.ContainsKey(relation.Id))
					{
						//// CREATED
						/////////////////////////////////////////////////////
						var changeCode = CreateRelationCode(relation);
						changeRow = new MetaChangeModel();
						changeRow.Element = "relation";
						changeRow.Type = "created";
						changeRow.Name = relation.Name;
						changeRow.ChangeList = new List<string>();
						if (changeCode == string.Empty)
						{
							changeRow.ChangeList.Add(@"<span class='go-gray'>No code will be generated. It is automatically created, in the entity creation process</span>");
						}
						response.Changes.Add(changeRow);
						response.Code += changeCode;
					}
					else
					{
						//// POSSIBLE UPDATE
						/////////////////////////////////////////////////////
						var changeCheckResponse = UpdateRelationCode(relation, oldRelationsDictionary[relation.Id]);
						if (changeCheckResponse.HasUpdate)
						{
							//1.1 Updated
							changeRow = new MetaChangeModel();
							changeRow.Element = "relation";
							changeRow.Type = "updated";
							changeRow.Name = relation.Name;
							changeRow.ChangeList = changeCheckResponse.ChangeList;
							response.Changes.Add(changeRow);
							response.Code += changeCheckResponse.Code;
						}

						// MARK ID AS PROCESSED
						/////////////////////////////////////////////////////
						oldRelationsProcessedDictionary[relation.Id] = true;
					}

				}

				foreach (var relation in oldRelationsList)
				{
					if (!oldRelationsProcessedDictionary.ContainsKey(relation.Id))
					{
						//// DELETED
						/////////////////////////////////////////////////////
						changeRow = new MetaChangeModel();
						changeRow.Element = "relation";
						changeRow.Type = "deleted";
						changeRow.Name = relation.Name;
						response.Changes.Add(changeRow);
						response.Code += DeleteRelationCode(relation);
					}
				}

				#endregion

				#endregion
				}
				if(includeRoles) {
				#region << Process roles >>

				#region << Init >>
				foreach (var role in oldRoleList)
				{
					oldRolesDictionary[(Guid)role["id"]] = role;
				}
				#endregion

				#region << Logic >>
				foreach (var role in currentRoleList)
				{
					if (!oldRolesDictionary.ContainsKey((Guid)role["id"]))
					{
						//// CREATED
						/////////////////////////////////////////////////////
						changeRow = new MetaChangeModel();
						changeRow.Element = "role";
						changeRow.Type = "created";
						changeRow.Name = (string)role["name"];
						response.Changes.Add(changeRow);
						response.Code += CreateRoleCode(role);
					}
					else
					{
						//// POSSIBLE UPDATE
						/////////////////////////////////////////////////////
						var changeCheckResponse = UpdateRoleCode(role, oldRolesDictionary[(Guid)role["id"]]);
						if (changeCheckResponse.HasUpdate)
						{
							//1.1 Updated
							changeRow = new MetaChangeModel();
							changeRow.Element = "role";
							changeRow.Type = "updated";
							changeRow.Name = (string)role["name"];
							changeRow.ChangeList = changeCheckResponse.ChangeList;
							response.Changes.Add(changeRow);
							response.Code += changeCheckResponse.Code;
						}

						// MARK ID AS PROCESSED
						/////////////////////////////////////////////////////
						oldRolesProcessedDictionary[(Guid)role["id"]] = true;
					}

				}

				foreach (var role in oldRoleList)
				{
					if (!oldRolesProcessedDictionary.ContainsKey((Guid)role["id"]))
					{
						//// DELETED
						/////////////////////////////////////////////////////
						changeRow = new MetaChangeModel();
						changeRow.Element = "role";
						changeRow.Type = "deleted";
						changeRow.Name = (string)role["name"];
						response.Changes.Add(changeRow);
						response.Code += DeleteRoleCode(role);
					}
				}

				#endregion

				#endregion
				}
				return Json(response);

			}
			catch (Exception ex)
			{
				var jsonReponse = new MetaChangeResponseModel();
				jsonReponse.Success = false;
				jsonReponse.Message = ex.Message;
				return Json(jsonReponse);
			}
		}

		#region << Read OLD data >>

		private List<DbEntity> ReadOldEntities(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT json FROM entities;", con);

					using (NpgsqlDataReader reader = command.ExecuteReader())
					{

						JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
						List<DbEntity> entities = new List<DbEntity>();
						while (reader.Read())
						{
							DbEntity entity = JsonConvert.DeserializeObject<DbEntity>(reader[0].ToString(), settings);
							entities.Add(entity);
						}



						reader.Close();
						return entities;
					}
				}
				finally
				{
					con.Close();
				}
			}
		}

		private List<EntityRecord> ReadOldAreas(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM rec_area;", con);
					NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
					DataTable table = new DataTable();
					adapter.Fill(table);
					return table.AsRecordList();
				}
				finally
				{
					con.Close();
				}
			}
		}

		private List<DbEntityRelation> ReadOldRelations(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT json FROM entity_relations;", con);
					using (NpgsqlDataReader reader = command.ExecuteReader())
					{

						JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
						List<DbEntityRelation> relations = new List<DbEntityRelation>();
						while (reader.Read())
						{
							DbEntityRelation relation = JsonConvert.DeserializeObject<DbEntityRelation>(reader[0].ToString(), settings);
							relations.Add(relation);
						}



						reader.Close();
						return relations;
					}
				}
				finally
				{
					con.Close();
				}
			}
		}

		private List<EntityRecord> ReadOldRoles(string OLD_DB_CONNECTION_STRING)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(OLD_DB_CONNECTION_STRING))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM rec_role;", con);
					NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
					DataTable table = new DataTable();
					adapter.Fill(table);
					return table.AsRecordList();
				}
				finally
				{
					con.Close();
				}
			}
		}

		#endregion

		#region << Area >>
		private string CreateAreaCode(EntityRecord area)
		{

			var response = "" +
$"#region << Create area: {(string)area["name"]} >>\n" +
"{\n" +
	"\tvar area = new EntityRecord();\n" +
	$"\tarea[\"id\"] = new Guid(\"{(Guid)area["id"]}\");\n" +
	$"\tarea[\"name\"] = \"{(string)area["name"]}\";\n" +
	$"\tarea[\"label\"] = \"{(string)area["label"]}\";\n" +
	$"\tarea[\"icon_name\"] = \"{(string)area["icon_name"]}\";\n" +
	$"\tarea[\"color\"] = \"{(string)area["color"]}\";\n" +
	$"\tarea[\"folder\"] = \"{(string)area["folder"]}\";\n" +
	$"\tarea[\"weight\"] = {(decimal)area["weight"]};\n" +
	$"\tarea[\"roles\"] = \"{((string)area["roles"]).Replace("\"", "\\\"")}\";\n" +
	$"\tarea[\"attachments\"] = \"{((string)area["attachments"]).Replace("\"", "\\\"")}\";\n" +
	"\tvar createAreaResult = recMan.CreateRecord(\"area\", area);\n" +
	"\tif (!createAreaResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Area create with name : {(string)area["name"]}. Message:\" + createAreaResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private string DeleteAreaCode(EntityRecord area)
		{
			var response = "" +
$"#region << Delete area: {(string)area["name"]} >>\n" +
"{\n" +
	$"\tvar deleteAreaResult = recMan.DeleteRecord(\"area\", new Guid(\"{(Guid)area["id"]}\"));\n" +
	"\tif (!deleteAreaResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Area delete with name : {(string)area["name"]}. Message:\" + deleteAreaResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateAreaCode(EntityRecord currentArea, EntityRecord oldArea)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			code =
$"#region << Update area: {(string)currentArea["name"]} >>\n" +
"{\n" +
	"\tvar patchObject = new EntityRecord();\n" +
	$"\tpatchObject[\"id\"] = new Guid(\"{(Guid)currentArea["id"]}\");\n";

			//name
			if ((string)currentArea["name"] != (string)oldArea["name"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"name\"] = \"{(string)currentArea["name"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{(string)oldArea["name"]}</span> to <span class='go-red'>{(string)currentArea["name"]}</span>");
			}
			//label	
			if ((string)currentArea["label"] != (string)oldArea["label"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"label\"] = \"{(string)currentArea["label"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>label</span> from <span class='go-red'>{(string)oldArea["label"]}</span> to <span class='go-red'>{(string)currentArea["label"]}</span>");
			}
			//color	
			if ((string)currentArea["color"] != (string)oldArea["color"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"color\"] = \"{(string)currentArea["color"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>color</span> from <span class='go-red'>{(string)oldArea["color"]}</span> to <span class='go-red'>{(string)currentArea["color"]}</span>");
			}
			//icon_name	
			if ((string)currentArea["icon_name"] != (string)oldArea["icon_name"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"icon_name\"] = \"{(string)currentArea["icon_name"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>icon_name</span> from <span class='go-red'>{(string)oldArea["icon_name"]}</span> to <span class='go-red'>{(string)currentArea["icon_name"]}</span>");
			}
			//weight	
			if ((decimal)currentArea["weight"] != (decimal)oldArea["weight"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"weight\"] = \"{(decimal)currentArea["weight"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>weight</span> from <span class='go-red'>{(decimal)oldArea["weight"]}</span> to <span class='go-red'>{(decimal)currentArea["weight"]}</span>");
			}
			//attachments	
			if ((string)currentArea["attachments"] != (string)oldArea["attachments"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"attachments\"] = \"{((string)currentArea["attachments"]).Replace("\"", "\\\"")}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>attachments</span> attachments code was changed</span>");
			}
			//roles	
			if ((string)currentArea["roles"] != (string)oldArea["roles"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"roles\"] = \"{((string)currentArea["roles"]).Replace("\"", "\\\"")}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>roles</span> from <span class='go-red'>{(string)oldArea["roles"]}</span> to <span class='go-red'>{(string)currentArea["roles"]}</span>");
			}
			//folder	
			if ((string)currentArea["folder"] != (string)oldArea["folder"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"folder\"] = \"{(string)currentArea["folder"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>folder</span> from <span class='go-red'>{(string)oldArea["folder"]}</span> to <span class='go-red'>{(string)currentArea["folder"]}</span>");
			}

			code +=
				"\tvar updateAreaResult = recMan.UpdateRecord(\"area\", patchObject);\n" +
				"\tif (!updateAreaResult.Success)\n" +
				"\t{\n" +
					$"\t\tthrow new Exception(\"System error 10060. Area update with name : {(string)currentArea["name"]}. Message:\" + updateAreaResult.Message);\n" +
				"\t}\n" +
			"}\n" +
			"#endregion\n\n";


			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;

		}
		#endregion

		#region << Entity >>


		private string CreateEntityCode(DbEntity entity)
		{

			//escape some possible quotes
			if (entity.Label != null)
				entity.Label = entity.Label.Replace("\"", "\\\"");

			var response = "" +
$"#region << Create entity: {entity.Name} >>\n" +
"{\n" +
	"\t#region << entity >>\n" +
	"\t{\n" +
		"\t\tvar entity = new InputEntity();\n" +
		"\t\tvar systemFieldIdDictionary = new Dictionary<string,Guid>();\n" +
		$"\t\tsystemFieldIdDictionary[\"id\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "id").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"created_on\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "created_on").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"created_by\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "created_by").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"last_modified_on\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "last_modified_on").Id}\");\n" +
		$"\t\tsystemFieldIdDictionary[\"last_modified_by\"] = new Guid(\"{entity.Fields.Single(x => x.Name == "last_modified_by").Id}\");\n" +
		$"\t\tentity.Id = new Guid(\"{entity.Id}\");\n" +
		$"\t\tentity.Name = \"{entity.Name}\";\n" +
		$"\t\tentity.Label = \"{entity.Label}\";\n" +
		$"\t\tentity.LabelPlural = \"{entity.LabelPlural}\";\n" +
		$"\t\tentity.System = {(entity.System).ToString().ToLowerInvariant()};\n" +
		$"\t\tentity.IconName = \"{entity.IconName}\";\n" +
		$"\t\tentity.Weight = (decimal){entity.Weight};\n" +
		"\t\tentity.RecordPermissions = new RecordPermissions();\n" +
		"\t\tentity.RecordPermissions.CanCreate = new List<Guid>();\n" +
		"\t\tentity.RecordPermissions.CanRead = new List<Guid>();\n" +
		"\t\tentity.RecordPermissions.CanUpdate = new List<Guid>();\n" +
		"\t\tentity.RecordPermissions.CanDelete = new List<Guid>();\n" +
		"\t\t//Create\n";
			foreach (var permId in entity.RecordPermissions.CanCreate)
			{
				response += $"\t\tentity.RecordPermissions.CanCreate.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t\t//READ\n";
			foreach (var permId in entity.RecordPermissions.CanRead)
			{
				response += $"\t\tentity.RecordPermissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t\t//UPDATE\n";
			foreach (var permId in entity.RecordPermissions.CanUpdate)
			{
				response += $"\t\tentity.RecordPermissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t\t//DELETE\n";
			foreach (var permId in entity.RecordPermissions.CanDelete)
			{
				response += $"\t\tentity.RecordPermissions.CanDelete.Add(new Guid(\"{permId}\"));\n";
			}

			response += "\t\t{\n" +
				"\t\t\tvar response = entMan.CreateEntity(entity, false, false,systemFieldIdDictionary);\n" +
				"\t\t\tif (!response.Success)\n" +
					$"\t\t\t\tthrow new Exception(\"System error 10050. Entity: {entity.Name} creation Message: \" + response.Message);\n" +
			"\t\t}\n" +
		"\t}\n" +
		"\t#endregion\n" +
	"}\n" +
	"#endregion\n\n";

			//foreach field generate createField and add

			foreach (var field in entity.Fields)
			{
				response += CreateFieldCode(field, entity.Id, entity.Name);
			}

			//foreach view generate createview and add
			foreach (var view in entity.RecordViews)
			{
				response += CreateViewCode(view, entity.Id, entity.Name);
			}

			//foreach list generate createList and add
			foreach (var list in entity.RecordLists)
			{
				response += CreateListCode(list, entity.Id, entity.Name);
			}

			//foreach tree generate createTree and add
			foreach (var tree in entity.RecordTrees)
			{
				response += CreateTreeCode(tree, entity.Id, entity.Name);
			}


			return response;
		}

		private UpdateCheckResponse UpdateEntityCode(DbEntity currentEntity, DbEntity oldEntity)
		{
			var response = new UpdateCheckResponse();

			#region << General >>
			var changeGeneralResponse = UpdateGeneralEntityCode(currentEntity, oldEntity);
			if (changeGeneralResponse.HasUpdate)
			{
				response.HasUpdate = true;
				foreach (var change in changeGeneralResponse.ChangeList)
				{
					response.ChangeList.Add(change);
				}
				response.Code += changeGeneralResponse.Code;
			}
			#endregion

			#region << fields >>
			//prepare the old fields dictionary
			var entityOldFieldsDictionary = new Dictionary<Guid, DbBaseField>();
			var entityProcessedFieldsDictionary = new Dictionary<Guid, bool>();
			foreach (var field in oldEntity.Fields)
			{
				entityOldFieldsDictionary[field.Id] = field;
			}

			foreach (var field in currentEntity.Fields)
			{
				if (!entityOldFieldsDictionary.ContainsKey(field.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>field</span>  new field <span class='go-red'>{field.Name}</span> was created.</span>");
					response.Code += CreateFieldCode(field, currentEntity.Id, currentEntity.Name);
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateFieldCode(field, entityOldFieldsDictionary[field.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						response.Code += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedFieldsDictionary[field.Id] = true;
				}
			}
			foreach (var field in oldEntity.Fields)
			{
				if (!entityProcessedFieldsDictionary.ContainsKey(field.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>field</span>  field <span class='go-red'>{field.Name}</span> was deleted.</span>");
					response.Code += DeleteFieldCode(field, currentEntity.Id, currentEntity.Name);
				}
			}
			#endregion

			#region << RecordViews >>
			//prepare the old fields dictionary
			var entityOldViewsDictionary = new Dictionary<Guid, DbRecordView>();
			var entityProcessedViewsDictionary = new Dictionary<Guid, bool>();
			foreach (var view in oldEntity.RecordViews)
			{
				entityOldViewsDictionary[view.Id] = view;
			}

			foreach (var view in currentEntity.RecordViews)
			{
				if (!entityOldViewsDictionary.ContainsKey(view.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>view</span>  new view <span class='go-red'>{view.Name}</span> was created.</span>");
					response.Code += CreateViewCode(view, currentEntity.Id, currentEntity.Name);
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateViewCode(view, entityOldViewsDictionary[view.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						response.Code += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedViewsDictionary[view.Id] = true;
				}

			}
			foreach (var view in oldEntity.RecordViews)
			{
				if (!entityProcessedViewsDictionary.ContainsKey(view.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>view</span>  view <span class='go-red'>{view.Name}</span> was deleted.</span>");
					response.Code += DeleteViewCode(view, currentEntity.Id, currentEntity.Name);
				}
			}

			#endregion

			#region << RecordLists >>
			//prepare the old fields dictionary
			var entityOldListsDictionary = new Dictionary<Guid, DbRecordList>();
			var entityProcessedListsDictionary = new Dictionary<Guid, bool>();
			foreach (var list in oldEntity.RecordLists)
			{
				entityOldListsDictionary[list.Id] = list;
			}

			foreach (var list in currentEntity.RecordLists)
			{
				if (!entityOldListsDictionary.ContainsKey(list.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>list</span>  new list <span class='go-red'>{list.Name}</span> was created.</span>");
					response.Code += CreateListCode(list, currentEntity.Id, currentEntity.Name);
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateListCode(list, entityOldListsDictionary[list.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						response.Code += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedListsDictionary[list.Id] = true;
				}

			}
			foreach (var list in oldEntity.RecordLists)
			{
				if (!entityProcessedListsDictionary.ContainsKey(list.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>list</span>  list <span class='go-red'>{list.Name}</span> was deleted.</span>");
					response.Code += DeleteListCode(list, currentEntity.Id, currentEntity.Name);
				}
			}

			#endregion

			#region << RecordTrees >>
			//prepare the old fields dictionary
			var entityOldTreesDictionary = new Dictionary<Guid, DbRecordTree>();
			var entityProcessedTreesDictionary = new Dictionary<Guid, bool>();
			foreach (var tree in oldEntity.RecordTrees)
			{
				entityOldTreesDictionary[tree.Id] = tree;
			}

			foreach (var tree in currentEntity.RecordTrees)
			{
				if (!entityOldTreesDictionary.ContainsKey(tree.Id))
				{
					//// CREATED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>tree</span>  new tree <span class='go-red'>{tree.Name}</span> was created.</span>");
					response.Code += CreateTreeCode(tree, currentEntity.Id, currentEntity.Name);
				}
				else
				{
					//// POSSIBLE UPDATE
					/////////////////////////////////////////////////////		
					var changeCheckResponse = UpdateTreeCode(tree, entityOldTreesDictionary[tree.Id], currentEntity);
					if (changeCheckResponse.HasUpdate)
					{
						response.HasUpdate = true;
						foreach (var change in changeCheckResponse.ChangeList)
						{
							response.ChangeList.Add(change);
						}
						response.Code += changeCheckResponse.Code;
					}

					// MARK ID AS PROCESSED
					/////////////////////////////////////////////////////
					entityProcessedTreesDictionary[tree.Id] = true;
				}

			}
			foreach (var tree in oldEntity.RecordTrees)
			{
				if (!entityProcessedTreesDictionary.ContainsKey(tree.Id))
				{
					//// DELETED
					/////////////////////////////////////////////////////
					response.HasUpdate = true;
					response.ChangeList.Add($"<span class='go-green label-block'>tree</span>  tree <span class='go-red'>{tree.Name}</span> was deleted.</span>");
					response.Code += DeleteTreeCode(tree, currentEntity.Id, currentEntity.Name);
				}
			}

			#endregion

			return response;
		}

		private string DeleteEntityCode(DbEntity entity)
		{
			var response =
		$"#region << Delete entity: {entity.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteEntity(new Guid(\"{entity.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entity.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateGeneralEntityCode(DbEntity currentEntity, DbEntity oldEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentEntity.Label != null)
				currentEntity.Label = currentEntity.Label.Replace("\"", "\\\"");

			//escape the old entity the same way so the update check is correct
			if (oldEntity.Label != null)
				oldEntity.Label = oldEntity.Label.Replace("\"", "\\\"");

			#region << General details >>
			code =
			$"#region << Update entity: {currentEntity.Name} >>\n" +
			"{\n" +
				"\tvar updateObject = new InputEntity();\n" +
				$"\tupdateObject.Id = new Guid(\"{currentEntity.Id}\");\n";
			//name
			if (currentEntity.Name != oldEntity.Name)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldEntity.Name}</span> to <span class='go-red'>{currentEntity.Name}</span>");
			}
			code += $"\tupdateObject.Name = \"{currentEntity.Name}\";\n";

			//label
			if (currentEntity.Label != oldEntity.Label)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldEntity.Label}</span> to <span class='go-red'>{currentEntity.Label}</span>");
			}
			code += $"\tupdateObject.Label = \"{currentEntity.Label}\";\n";

			//LabelPlural
			if (currentEntity.LabelPlural != oldEntity.LabelPlural)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>LabelPlural</span>  from <span class='go-red'>{oldEntity.LabelPlural}</span> to <span class='go-red'>{currentEntity.LabelPlural}</span>");
			}
			code += $"\tupdateObject.LabelPlural = \"{currentEntity.LabelPlural}\";\n";

			//System
			if (currentEntity.System != oldEntity.System)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>System</span>  from <span class='go-red'>{oldEntity.System}</span> to <span class='go-red'>{currentEntity.System}</span>");
			}
			code += $"\tupdateObject.System = {(currentEntity.System).ToString().ToLowerInvariant()};\n";

			//IconName
			if (currentEntity.IconName != oldEntity.IconName)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>IconName</span>  from <span class='go-red'>{oldEntity.IconName}</span> to <span class='go-red'>{currentEntity.IconName}</span>");
			}
			code += $"\tupdateObject.IconName = \"{currentEntity.IconName}\";\n";

			//Weight
			if (currentEntity.Weight != oldEntity.Weight)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>Weight</span>  from <span class='go-red'>{oldEntity.Weight}</span> to <span class='go-red'>{currentEntity.Weight}</span>");
			}
			code += $"\tupdateObject.Weight = string.IsNullOrEmpty(\"{currentEntity.Weight}\") ? (decimal?)null : Decimal.Parse(\"{currentEntity.Weight}\");\n";

			//RecordPermissions
			var recordPermissionsChanged = false;
			var oldCanReadDictionary = new Dictionary<Guid, bool>();
			var oldCanCreateDictionary = new Dictionary<Guid, bool>();
			var oldCanUpdateDictionary = new Dictionary<Guid, bool>();
			var oldCanDeleteDictionary = new Dictionary<Guid, bool>();

			#region << prepare old status dictionaries >>
			foreach (var permission in oldEntity.RecordPermissions.CanRead)
			{
				oldCanReadDictionary[permission] = true;
			}

			foreach (var permission in oldEntity.RecordPermissions.CanCreate)
			{
				oldCanCreateDictionary[permission] = true;
			}

			foreach (var permission in oldEntity.RecordPermissions.CanUpdate)
			{
				oldCanUpdateDictionary[permission] = true;
			}

			foreach (var permission in oldEntity.RecordPermissions.CanDelete)
			{
				oldCanDeleteDictionary[permission] = true;
			}
			#endregion

			#region << Check for change and generate permissions code >>
			code += $"\tupdateObject.RecordPermissions = new RecordPermissions();\n" +
			$"\tupdateObject.RecordPermissions.CanRead = new List<Guid>();\n" +
			$"\tupdateObject.RecordPermissions.CanCreate = new List<Guid>();\n" +
			$"\tupdateObject.RecordPermissions.CanUpdate = new List<Guid>();\n" +
			$"\tupdateObject.RecordPermissions.CanDelete = new List<Guid>();\n";

			foreach (var permission in currentEntity.RecordPermissions.CanRead)
			{
				if (!oldCanReadDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanRead.Add(new Guid(\"{permission}\"));\n";
			}

			foreach (var permission in currentEntity.RecordPermissions.CanCreate)
			{
				if (!oldCanCreateDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanCreate.Add(new Guid(\"{permission}\"));\n";
			}

			foreach (var permission in currentEntity.RecordPermissions.CanUpdate)
			{
				if (!oldCanUpdateDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanUpdate.Add(new Guid(\"{permission}\"));\n";
			}

			foreach (var permission in currentEntity.RecordPermissions.CanDelete)
			{
				if (!oldCanDeleteDictionary.ContainsKey(permission))
				{
					recordPermissionsChanged = true;
				}
				code += $"\tupdateObject.RecordPermissions.CanDelete.Add(new Guid(\"{permission}\"));\n";
			}
			if (recordPermissionsChanged)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>permissions</span>  record permissions were changes</span>");
			}

			#endregion

			code +=
				"\tvar updateEntityResult = entMan.UpdateEntity(updateObject);\n" +
				"\tif (!updateEntityResult.Success)\n" +
				"\t{\n" +
					$"\t\tthrow new Exception(\"System error 10060. Entity update with name : {oldEntity.Name}. Message:\" + updateEntityResult.Message);\n" +
				"\t}\n" +
			"}\n" +
			"#endregion\n\n";

			#endregion

			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;
		}
		#endregion

		#region << Field >>

		private string CreateFieldCode(DbBaseField field, Guid entityId, string entityName)
		{
			var response = "";
			//Skip system generated fields
			if (field.Name == "id" || field.Name == "created_on" || field.Name == "created_by" || field.Name == "last_modified_on" || field.Name == "last_modified_by")
			{
				return string.Empty;
			}
			//escape quotes where they can possible be
			if (field.HelpText != null)
				field.HelpText = field.HelpText.Replace("\"", "\\\"");
			if (field.PlaceholderText != null)
				field.PlaceholderText = field.PlaceholderText.Replace("\"", "\\\"");
			if (field.Label != null)
				field.Label = field.Label.Replace("\"", "\\\"");
			if (field.Description != null)
				field.Description = field.Description.Replace("\"", "\\\"");
			switch (field.GetFieldType())
			{
				case FieldType.AutoNumberField:
					response += CreateAutoNumberFieldCode(field as DbAutoNumberField, entityId, entityName);
					break;
				case FieldType.CheckboxField:
					response += CreateCheckboxFieldCode(field as DbCheckboxField, entityId, entityName);
					break;
				case FieldType.CurrencyField:
					response += CreateCurrencyFieldCode(field as DbCurrencyField, entityId, entityName);
					break;
				case FieldType.DateField:
					response += CreateDateFieldCode(field as DbDateField, entityId, entityName);
					break;
				case FieldType.DateTimeField:
					response += CreateDateTimeFieldCode(field as DbDateTimeField, entityId, entityName);
					break;
				case FieldType.EmailField:
					response += CreateEmailFieldCode(field as DbEmailField, entityId, entityName);
					break;
				case FieldType.FileField:
					response += CreateFileFieldCode(field as DbFileField, entityId, entityName);
					break;
				case FieldType.ImageField:
					response += CreateImageFieldCode(field as DbImageField, entityId, entityName);
					break;
				case FieldType.HtmlField:
					response += CreateHtmlFieldCode(field as DbHtmlField, entityId, entityName);
					break;
				case FieldType.MultiLineTextField:
					response += CreateMultiLineTextFieldCode(field as DbMultiLineTextField, entityId, entityName);
					break;
				case FieldType.MultiSelectField:
					response += CreateMultiSelectFieldCode(field as DbMultiSelectField, entityId, entityName);
					break;
				case FieldType.NumberField:
					response += CreateNumberFieldCode(field as DbNumberField, entityId, entityName);
					break;
				case FieldType.PasswordField:
					response += CreatePasswordFieldCode(field as DbPasswordField, entityId, entityName);
					break;
				case FieldType.PercentField:
					response += CreatePercentFieldCode(field as DbPercentField, entityId, entityName);
					break;
				case FieldType.PhoneField:
					response += CreatePhoneFieldCode(field as DbPhoneField, entityId, entityName);
					break;
				case FieldType.GuidField:
					response += CreateGuidFieldCode(field as DbGuidField, entityId, entityName);
					break;
				case FieldType.SelectField:
					response += CreateSelectFieldCode(field as DbSelectField, entityId, entityName);
					break;
				case FieldType.TextField:
					response += CreateTextFieldCode(field as DbTextField, entityId, entityName);
					break;
				case FieldType.UrlField:
					response += CreateUrlFieldCode(field as DbUrlField, entityId, entityName);
					break;
			}

			return response;
		}

		private string CreateAutoNumberFieldCode(DbAutoNumberField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	"\tInputAutoNumberField autonumberField = new InputAutoNumberField();\n" +
	$"\tautonumberField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tautonumberField.Name = \"{field.Name}\";\n" +
	$"\tautonumberField.Label = \"{field.Label}\";\n" +
	$"\tautonumberField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tautonumberField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tautonumberField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tautonumberField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tautonumberField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tautonumberField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tautonumberField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tautonumberField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tautonumberField.DefaultValue = (decimal?){field.DefaultValue};\n" +
	$"\tautonumberField.DisplayFormat = \"{field.DisplayFormat}\";\n" +
	$"\tautonumberField.StartingNumber = (decimal?){field.StartingNumber};\n" +
	$"\tautonumberField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	"\tautonumberField.Permissions = new FieldPermissions();\n" +
	"\tautonumberField.Permissions.CanRead = new List<Guid>();\n" +
	"\tautonumberField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";

			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tautonumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tautonumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), autonumberField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateCheckboxFieldCode(DbCheckboxField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	"\tInputCheckboxField checkboxField = new InputCheckboxField();\n" +
	$"\tcheckboxField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tcheckboxField.Name = \"{field.Name}\";\n" +
	$"\tcheckboxField.Label = \"{field.Label}\";\n" +
	$"\tcheckboxField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tcheckboxField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tcheckboxField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tcheckboxField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tcheckboxField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tcheckboxField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tcheckboxField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tcheckboxField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tcheckboxField.DefaultValue = {(field.DefaultValue).ToString().ToLowerInvariant()};\n" +
	$"\tcheckboxField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	"\tcheckboxField.Permissions = new FieldPermissions();\n" +
	"\tcheckboxField.Permissions.CanRead = new List<Guid>();\n" +
	"\tcheckboxField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tcheckboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tcheckboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), checkboxField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateCurrencyFieldCode(DbCurrencyField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	"\tInputCurrencyField currencyField = new InputCurrencyField();\n" +
	$"\tcurrencyField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tcurrencyField.Name = \"{field.Name}\";\n" +
	$"\tcurrencyField.Label =  \"{field.Label}\";\n" +
	$"\tcurrencyField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tcurrencyField.Description =string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tcurrencyField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tcurrencyField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tcurrencyField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tcurrencyField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tcurrencyField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tcurrencyField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tcurrencyField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? (decimal?)null : Decimal.Parse(\"{field.DefaultValue}\");\n" +
	$"\tcurrencyField.MinValue = string.IsNullOrEmpty(\"{field.MinValue}\") ? (decimal?)null : Decimal.Parse(\"{field.MinValue}\");\n" +
	$"\tcurrencyField.MaxValue = string.IsNullOrEmpty(\"{field.MaxValue}\") ? (decimal?)null : Decimal.Parse(\"{field.MaxValue}\");\n" +
	$"\tcurrencyField.Currency = WebVella.ERP.Utilities.Helpers.GetCurrencyTypeObject(\"{field.Currency.Code}\");\n" +
	$"\tcurrencyField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	"\tcurrencyField.Permissions = new FieldPermissions();\n" +
	"\tcurrencyField.Permissions.CanRead = new List<Guid>();\n" +
	"\tcurrencyField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tcurrencyField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tcurrencyField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), currencyField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateDateFieldCode(DbDateField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	"\tInputDateField dateField = new InputDateField();\n" +
	$"\tdateField.Id =  new Guid(\"{field.Id}\");\n" +
	$"\tdateField.Name = \"{field.Name}\";\n" +
	$"\tdateField.Label = \"{field.Label}\";\n" +
	$"\tdateField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tdateField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tdateField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tdateField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tdateField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tdateField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tdateField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tdateField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tdateField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? (DateTime?)null : DateTime.Parse(\"{field.DefaultValue}\");\n" +
	$"\tdateField.Format = string.IsNullOrEmpty(\"{field.Format}\") ? null : \"{field.Format}\";\n" +
	$"\tdateField.UseCurrentTimeAsDefaultValue = {(field.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
	$"\tdateField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	"\tdateField.Permissions = new FieldPermissions();\n" +
	"\tdateField.Permissions.CanRead = new List<Guid>();\n" +
	"\tdateField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";

			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tdateField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tdateField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), dateField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateDateTimeFieldCode(DbDateTimeField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	"\tInputDateTimeField datetimeField = new InputDateTimeField();\n" +
	$"\tdatetimeField.Id =  new Guid(\"{field.Id}\");\n" +
	$"\tdatetimeField.Name = \"{field.Name}\";\n" +
	$"\tdatetimeField.Label = \"{field.Label}\";\n" +
	$"\tdatetimeField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tdatetimeField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tdatetimeField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tdatetimeField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tdatetimeField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tdatetimeField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tdatetimeField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tdatetimeField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tdatetimeField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? (DateTime?)null : DateTime.Parse(\"{field.DefaultValue}\");\n" +
	$"\tdatetimeField.Format = string.IsNullOrEmpty(\"{field.Format}\") ? null : \"{field.Format}\";\n" +
	$"\tdatetimeField.UseCurrentTimeAsDefaultValue = {(field.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
	$"\tdatetimeField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	"\tdatetimeField.Permissions = new FieldPermissions();\n" +
	"\tdatetimeField.Permissions.CanRead = new List<Guid>();\n" +
	"\tdatetimeField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";

			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tdatetimeField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tdatetimeField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), datetimeField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateEmailFieldCode(DbEmailField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	"\tInputEmailField emailField = new InputEmailField();\n" +
	$"\temailField.Id = new Guid(\"{field.Id}\");\n" +
	$"\temailField.Name = \"{field.Name}\";\n" +
	$"\temailField.Label = \"{field.Label}\";\n" +
	$"\temailField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\temailField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\temailField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\temailField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\temailField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\temailField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\temailField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\temailField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\temailField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
	$"\temailField.MaxLength = string.IsNullOrEmpty(\"{field.MaxLength}\") ? (int?)null : Int32.Parse(\"{field.MaxLength}\");\n" +
	$"\temailField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\temailField.Permissions = new FieldPermissions();\n" +
	$"\temailField.Permissions.CanRead = new List<Guid>();\n" +
	$"\temailField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\temailField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\temailField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), emailField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreateFileFieldCode(DbFileField field, Guid entityId, string entityName)
		{
			var response = "";

			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputFileField fileField = new InputFileField();\n" +
	$"\tfileField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tfileField.Name = \"{field.Name}\";\n" +
	$"\tfileField.Label = \"{field.Label}\";\n" +
	$"\tfileField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tfileField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tfileField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tfileField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tfileField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tfileField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tfileField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tfileField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tfileField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
	$"\tfileField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tfileField.Permissions = new FieldPermissions();\n" +
	$"\tfileField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tfileField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tfileField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tfileField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), fileField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateHtmlFieldCode(DbHtmlField field, Guid entityId, string entityName)
		{
			var response = "";

			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputHtmlField htmlField = new InputHtmlField();\n" +
	$"\thtmlField.Id = new Guid(\"{field.Id}\");\n" +
	$"\thtmlField.Name = \"{field.Name}\";\n" +
	$"\thtmlField.Label = \"{field.Label}\";\n" +
	$"\thtmlField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\thtmlField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\thtmlField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\thtmlField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\thtmlField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\thtmlField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\thtmlField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\thtmlField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\thtmlField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
	$"\thtmlField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\thtmlField.Permissions = new FieldPermissions();\n" +
	$"\thtmlField.Permissions.CanRead = new List<Guid>();\n" +
	$"\thtmlField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\thtmlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\thtmlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), htmlField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			return response;
		}

		private string CreateImageFieldCode(DbImageField field, Guid entityId, string entityName)
		{
			var response = "";
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputImageField imageField = new InputImageField();\n" +
	$"\timageField.Id = new Guid(\"{field.Id}\");\n" +
	$"\timageField.Name = \"{field.Name}\";\n" +
	$"\timageField.Label = \"{field.Label}\";\n" +
	$"\timageField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\timageField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\timageField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\timageField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\timageField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\timageField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\timageField.Auditable =  {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\timageField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\timageField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
	$"\timageField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\timageField.Permissions = new FieldPermissions();\n" +
	$"\timageField.Permissions.CanRead = new List<Guid>();\n" +
	$"\timageField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\timageField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\timageField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), imageField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			return response;
		}

		private string CreateMultiLineTextFieldCode(DbMultiLineTextField field, Guid entityId, string entityName)
		{
			var response = "";
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputMultiLineTextField textareaField = new InputMultiLineTextField();\n" +
	$"\ttextareaField.Id = new Guid(\"{field.Id}\");\n" +
	$"\ttextareaField.Name = \"{field.Name}\";\n" +
	$"\ttextareaField.Label = \"{field.Label}\";\n" +
	$"\ttextareaField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\ttextareaField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\ttextareaField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\ttextareaField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\ttextareaField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\ttextareaField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\ttextareaField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\ttextareaField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\ttextareaField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
	$"\ttextareaField.MaxLength = string.IsNullOrEmpty(\"{field.MaxLength}\") ? (int?)null : Int32.Parse(\"{field.MaxLength}\");\n" +
	$"\ttextareaField.VisibleLineNumber = string.IsNullOrEmpty(\"{field.VisibleLineNumber}\") ? (int?)null : Int32.Parse(\"{field.VisibleLineNumber}\");\n" +
	$"\ttextareaField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\ttextareaField.Permissions = new FieldPermissions();\n" +
	$"\ttextareaField.Permissions.CanRead = new List<Guid>();\n" +
	$"\ttextareaField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\ttextareaField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\ttextareaField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), textareaField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateMultiSelectFieldCode(DbMultiSelectField field, Guid entityId, string entityName)
		{
			var response = "";
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputMultiSelectField multiSelectField = new InputMultiSelectField();\n" +
	$"\tmultiSelectField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tmultiSelectField.Name = \"{field.Name}\";\n" +
	$"\tmultiSelectField.Label = \"{field.Label}\";\n" +
	$"\tmultiSelectField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tmultiSelectField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tmultiSelectField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tmultiSelectField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tmultiSelectField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	"\tmultiSelectField.DefaultValue = ";
			var defaultValues = field.DefaultValue.ToList();
			if (defaultValues.Count > 0)
			{
				response += "new List<string>() {";

				for (int i = 0; i < defaultValues.Count; i++)
				{
					response += "\"" + defaultValues[i] + "\"";
					if (i < defaultValues.Count - 1)
					{
						response += ",";
					}
				}
				response += "};\n";
			}
			else
			{
				response += "null;\n";
			}
			response += "\tmultiSelectField.Options = ";
			var fieldOptions = field.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<MultiSelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew MultiSelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=

			$"\tmultiSelectField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tmultiSelectField.Permissions = new FieldPermissions();\n" +
			$"\tmultiSelectField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tmultiSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tmultiSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tmultiSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), multiSelectField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateNumberFieldCode(DbNumberField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputNumberField numberField = new InputNumberField();\n" +
	$"\tnumberField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tnumberField.Name = \"{field.Name}\";\n" +
	$"\tnumberField.Label = \"{field.Label}\";\n" +
	$"\tnumberField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tnumberField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tnumberField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tnumberField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? (decimal?)null : Decimal.Parse(\"{field.DefaultValue}\");\n" +
	$"\tnumberField.MinValue = string.IsNullOrEmpty(\"{field.MinValue}\") ? (decimal?)null : Decimal.Parse(\"{field.MinValue}\");\n" +
	$"\tnumberField.MaxValue = string.IsNullOrEmpty(\"{field.MaxValue}\") ? (decimal?)null : Decimal.Parse(\"{field.MaxValue}\");\n" +
	$"\tnumberField.DecimalPlaces = string.IsNullOrEmpty(\"{field.DecimalPlaces}\") ? (byte?)null : byte.Parse(\"{field.DecimalPlaces}\");\n" +
	$"\tnumberField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Permissions = new FieldPermissions();\n" +
	$"\tnumberField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tnumberField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tnumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tnumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), numberField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreatePasswordFieldCode(DbPasswordField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputPasswordField passwordField = new InputPasswordField();\n" +
	$"\tpasswordField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tpasswordField.Name = \"{field.Name}\";\n" +
	$"\tpasswordField.Label = \"{field.Label}\";\n" +
	$"\tpasswordField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tpasswordField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tpasswordField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tpasswordField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.MinLength = string.IsNullOrEmpty(\"{field.MinLength}\") ? (int?)null : Int32.Parse(\"{field.MinLength}\");\n" +
	$"\tpasswordField.MaxLength = string.IsNullOrEmpty(\"{field.MaxLength}\") ? (int?)null : Int32.Parse(\"{field.MaxLength}\");\n" +
	$"\tpasswordField.Encrypted = {(field.Encrypted).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Permissions = new FieldPermissions();\n" +
	$"\tpasswordField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tpasswordField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tpasswordField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tpasswordField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), passwordField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreatePercentFieldCode(DbPercentField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputPercentField percentField = new InputPercentField();\n" +
	$"\tpercentField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tpercentField.Name = \"{field.Name}\";\n" +
	$"\tpercentField.Label = \"{field.Label}\";\n" +
	$"\tpercentField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tpercentField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tpercentField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tpercentField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? (decimal?)null : Decimal.Parse(\"{field.DefaultValue}\");\n" +
	$"\tpercentField.MinValue = string.IsNullOrEmpty(\"{field.MinValue}\") ? (decimal?)null : Decimal.Parse(\"{field.MinValue}\");\n" +
	$"\tpercentField.MaxValue = string.IsNullOrEmpty(\"{field.MaxValue}\") ? (decimal?)null : Decimal.Parse(\"{field.MaxValue}\");\n" +
	$"\tpercentField.DecimalPlaces = string.IsNullOrEmpty(\"{field.DecimalPlaces}\") ? (byte?)null : byte.Parse(\"{field.DecimalPlaces}\");\n" +
	$"\tpercentField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Permissions = new FieldPermissions();\n" +
	$"\tpercentField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tpercentField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tpercentField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tpercentField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), percentField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}

		private string CreatePhoneFieldCode(DbPhoneField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputPhoneField phoneField = new InputPhoneField();\n" +
	$"\tphoneField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tphoneField.Name = \"{field.Name}\";\n" +
	$"\tphoneField.Label =  \"{field.Label}\";\n" +
	$"\tphoneField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tphoneField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tphoneField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tphoneField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
	$"\tphoneField.MaxLength = string.IsNullOrEmpty(\"{field.MaxLength}\") ? (int?)null : Int32.Parse(\"{field.MaxLength}\");\n" +
	$"\tphoneField.Format = string.IsNullOrEmpty(\"{field.Format}\") ? null : \"{field.Format}\";\n" +
	$"\tphoneField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Permissions = new FieldPermissions();\n" +
	$"\tphoneField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tphoneField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tphoneField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tphoneField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), phoneField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateGuidFieldCode(DbGuidField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputGuidField guidField = new InputGuidField();\n" +
	$"\tguidField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tguidField.Name = \"{field.Name}\";\n" +
	$"\tguidField.Label = \"{field.Label}\";\n" +
	$"\tguidField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tguidField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tguidField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tguidField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? (Guid?)null : Guid.Parse(\"{field.DefaultValue}\");\n" +
	$"\tguidField.GenerateNewId = {(field.GenerateNewId).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Permissions = new FieldPermissions();\n" +
	$"\tguidField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tguidField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tguidField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tguidField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), guidField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateSelectFieldCode(DbSelectField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputSelectField dropdownField = new InputSelectField();\n" +
	$"\tdropdownField.Id = new Guid(\"{field.Id}\");\n" +
	$"\tdropdownField.Name = \"{field.Name}\";\n" +
	$"\tdropdownField.Label = \"{field.Label}\";\n" +
	$"\tdropdownField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
	$"\tdropdownField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
	$"\tdropdownField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
	$"\tdropdownField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n";
			response += "\tdropdownField.Options = ";
			var fieldOptions = field.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<SelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew SelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=
			$"\tdropdownField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Permissions = new FieldPermissions();\n" +
			$"\tdropdownField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tdropdownField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\tdropdownField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\tdropdownField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), dropdownField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string CreateTextFieldCode(DbTextField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
"{\n" +
	$"\tInputTextField textboxField = new InputTextField();\n" +
			$"\ttextboxField.Id = new Guid(\"{field.Id}\");\n" +
			$"\ttextboxField.Name = \"{field.Name}\";\n" +
			$"\ttextboxField.Label = \"{field.Label}\";\n" +
			$"\ttextboxField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
			$"\ttextboxField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
			$"\ttextboxField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
			$"\ttextboxField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
			$"\ttextboxField.MaxLength = string.IsNullOrEmpty(\"{field.MaxLength}\") ? (int?)null : Int32.Parse(\"{field.MaxLength}\");\n" +
			$"\ttextboxField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Permissions = new FieldPermissions();\n" +
			$"\ttextboxField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttextboxField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\ttextboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\ttextboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), textboxField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			return response;
		}

		private string CreateUrlFieldCode(DbUrlField field, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =

		$"#region << Create  Enity: {entityName} field: {field.Name} >>\n" +
		"{\n" +
			$"\tInputUrlField urlField = new InputUrlField();\n" +
			$"\turlField.Id = new Guid(\"{field.Id}\");\n" +
			$"\turlField.Name = \"{field.Name}\";\n" +
			$"\turlField.Label = \"{field.Label}\";\n" +
			$"\turlField.PlaceholderText = string.IsNullOrEmpty(\"{field.PlaceholderText}\") ? null : \"{field.PlaceholderText}\";\n" +
			$"\turlField.Description = string.IsNullOrEmpty(\"{field.Description}\") ? string.Empty : \"{field.Description}\";\n" +
			$"\turlField.HelpText = string.IsNullOrEmpty(\"{field.HelpText}\") ? null : \"{field.HelpText}\";\n" +
			$"\turlField.Required = {(field.Required).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Unique = {(field.Unique).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Searchable = {(field.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Auditable = {(field.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.System = {(field.System).ToString().ToLowerInvariant()};\n" +
			$"\turlField.DefaultValue = string.IsNullOrEmpty(\"{field.DefaultValue}\") ? string.Empty : \"{field.DefaultValue}\";\n" +
			$"\turlField.MaxLength = string.IsNullOrEmpty(\"{field.MaxLength}\") ? (int?)null : Int32.Parse(\"{field.MaxLength}\");\n" +
			$"\turlField.OpenTargetInNewWindow = {(field.OpenTargetInNewWindow).ToString().ToLowerInvariant()};\n" +
			$"\turlField.EnableSecurity = {(field.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Permissions = new FieldPermissions();\n" +
			$"\turlField.Permissions.CanRead = new List<Guid>();\n" +
			$"\turlField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in field.Permissions.CanRead)
			{
				response += $"\turlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in field.Permissions.CanUpdate)
			{
				response += $"\turlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateField(new Guid(\"{entityId}\"), urlField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {field.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private string DeleteFieldCode(DbBaseField field, Guid entityId, string entityName)
		{
			var response =

		$"#region << Delete field: {field.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteField(new Guid(\"{entityId}\"),new Guid(\"{field.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Delete field failed for Field: {field.Name} Entity: {entityName}. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateFieldCode(DbBaseField currentField, DbBaseField oldField, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;

			//escape possible double quotes
			if (currentField.HelpText != null)
				currentField.HelpText = currentField.HelpText.Replace("\"", "\\\"");
			if (currentField.PlaceholderText != null)
				currentField.PlaceholderText = currentField.PlaceholderText.Replace("\"", "\\\"");
			if (currentField.Label != null)
				currentField.Label = currentField.Label.Replace("\"", "\\\"");
			if (currentField.Description != null)
				currentField.Description = currentField.Description.Replace("\"", "\\\"");
			//escape the old field values the same way so the update check is correct later
			if (oldField.HelpText != null)
				oldField.HelpText = oldField.HelpText.Replace("\"", "\\\"");
			if (oldField.PlaceholderText != null)
				oldField.PlaceholderText = oldField.PlaceholderText.Replace("\"", "\\\"");
			if (oldField.Label != null)
				oldField.Label = oldField.Label.Replace("\"", "\\\"");
			if (oldField.Description != null)
				oldField.Description = oldField.Description.Replace("\"", "\\\"");

			switch (currentField.GetFieldType())
			{
				case FieldType.AutoNumberField:
					{
						var responseCode = UpdateAutoNumberFieldCode(currentField as DbAutoNumberField, oldField as DbAutoNumberField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.CheckboxField:
					{
						var responseCode = UpdateCheckboxFieldCode(currentField as DbCheckboxField, oldField as DbCheckboxField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.CurrencyField:
					{
						var responseCode = UpdateCurrencyFieldCode(currentField as DbCurrencyField, oldField as DbCurrencyField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.DateField:
					{
						var responseCode = UpdateDateFieldCode(currentField as DbDateField, oldField as DbDateField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.DateTimeField:
					{
						var responseCode = UpdateDateTimeFieldCode(currentField as DbDateTimeField, oldField as DbDateTimeField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.EmailField:
					{
						var responseCode = UpdateEmailFieldCode(currentField as DbEmailField, oldField as DbEmailField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.FileField:
					{
						var responseCode = UpdateFileFieldCode(currentField as DbFileField, oldField as DbFileField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.ImageField:
					{
						var responseCode = UpdateImageFieldCode(currentField as DbImageField, oldField as DbImageField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.HtmlField:
					{
						var responseCode = UpdateHtmlFieldCode(currentField as DbHtmlField, oldField as DbHtmlField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.MultiLineTextField:
					{
						var responseCode = UpdateMultiLineTextFieldCode(currentField as DbMultiLineTextField, oldField as DbMultiLineTextField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.MultiSelectField:
					{
						var responseCode = UpdateMultiSelectFieldCode(currentField as DbMultiSelectField, oldField as DbMultiSelectField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.NumberField:
					{
						var responseCode = UpdateNumberFieldCode(currentField as DbNumberField, oldField as DbNumberField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.PasswordField:
					{
						var responseCode = UpdatePasswordFieldCode(currentField as DbPasswordField, oldField as DbPasswordField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.PercentField:
					{
						var responseCode = UpdatePercentFieldCode(currentField as DbPercentField, oldField as DbPercentField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.PhoneField:
					{
						var responseCode = UpdatePhoneFieldCode(currentField as DbPhoneField, oldField as DbPhoneField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.GuidField:
					{
						var responseCode = UpdateGuidFieldCode(currentField as DbGuidField, oldField as DbGuidField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.SelectField:
					{
						var responseCode = UpdateSelectFieldCode(currentField as DbSelectField, oldField as DbSelectField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.TextField:
					{
						var responseCode = UpdateTextFieldCode(currentField as DbTextField, oldField as DbTextField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
				case FieldType.UrlField:
					{
						var responseCode = UpdateUrlFieldCode(currentField as DbUrlField, oldField as DbUrlField, currentEntity.Id, currentEntity.Name);
						if (responseCode != string.Empty)
						{
							code = responseCode;
							hasUpdate = true;
						}
					}
					break;
			}

			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>field</span>  with name <span class='go-red'>{oldField.Name}</span> was updated");
			return response;
		}

		private string UpdateAutoNumberFieldCode(DbAutoNumberField currentField, DbAutoNumberField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputAutoNumberField autonumberField = new InputAutoNumberField();\n" +
				$"\tautonumberField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tautonumberField.Name = \"{currentField.Name}\";\n" +
				$"\tautonumberField.Label = \"{currentField.Label}\";\n" +
				$"\tautonumberField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\tautonumberField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\tautonumberField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\tautonumberField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tautonumberField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tautonumberField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tautonumberField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tautonumberField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\tautonumberField.DefaultValue = (decimal?){currentField.DefaultValue};\n" +
				$"\tautonumberField.DisplayFormat = \"{currentField.DisplayFormat}\";\n" +
				$"\tautonumberField.StartingNumber = (decimal?){currentField.StartingNumber};\n" +
				$"\tautonumberField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				"\tautonumberField.Permissions = new FieldPermissions();\n" +
				"\tautonumberField.Permissions.CanRead = new List<Guid>();\n" +
				"\tautonumberField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";

			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tautonumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tautonumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), autonumberField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion
			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.DisplayFormat != oldField.DisplayFormat)
			{
				hasUpdate = true;
			}
			if (currentField.StartingNumber != oldField.StartingNumber)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateCheckboxFieldCode(DbCheckboxField currentField, DbCheckboxField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputCheckboxField checkboxField = new InputCheckboxField();\n" +
				$"\tcheckboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tcheckboxField.Name = \"{currentField.Name}\";\n" +
				$"\tcheckboxField.Label = \"{currentField.Label}\";\n" +
				$"\tcheckboxField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\tcheckboxField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\tcheckboxField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\tcheckboxField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tcheckboxField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tcheckboxField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tcheckboxField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tcheckboxField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\tcheckboxField.DefaultValue = {(currentField.DefaultValue).ToString().ToLowerInvariant()};\n" +
				$"\tcheckboxField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				"\tcheckboxField.Permissions = new FieldPermissions();\n" +
				"\tcheckboxField.Permissions.CanRead = new List<Guid>();\n" +
				"\tcheckboxField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tcheckboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tcheckboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), checkboxField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdateCurrencyFieldCode(DbCurrencyField currentField, DbCurrencyField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputCurrencyField currencyField = new InputCurrencyField();\n" +
				$"\tcurrencyField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tcurrencyField.Name = \"{currentField.Name}\";\n" +
				$"\tcurrencyField.Label =  \"{currentField.Label}\";\n" +
				$"\tcurrencyField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\tcurrencyField.Description =string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\tcurrencyField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\tcurrencyField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tcurrencyField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tcurrencyField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tcurrencyField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tcurrencyField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\tcurrencyField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.DefaultValue}\");\n" +
				$"\tcurrencyField.MinValue = string.IsNullOrEmpty(\"{currentField.MinValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.MinValue}\");\n" +
				$"\tcurrencyField.MaxValue = string.IsNullOrEmpty(\"{currentField.MaxValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.MaxValue}\");\n" +
				$"\tcurrencyField.Currency = WebVella.ERP.Utilities.Helpers.GetCurrencyTypeObject(\"{currentField.Currency.Code}\");\n" +
				$"\tcurrencyField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				"\tcurrencyField.Permissions = new FieldPermissions();\n" +
				"\tcurrencyField.Permissions.CanRead = new List<Guid>();\n" +
				"\tcurrencyField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tcurrencyField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tcurrencyField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), currencyField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MinValue != oldField.MinValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxValue != oldField.MaxValue)
			{
				hasUpdate = true;
			}
			if (currentField.Currency.Code != oldField.Currency.Code)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}



			return response;
		}

		private string UpdateDateFieldCode(DbDateField currentField, DbDateField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputDateField dateField = new InputDateField();\n" +
				$"\tdateField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tdateField.Name = \"{currentField.Name}\";\n" +
				$"\tdateField.Label = \"{currentField.Label}\";\n" +
				$"\tdateField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\tdateField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\tdateField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\tdateField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? (DateTime?)null : DateTime.Parse(\"{currentField.DefaultValue}\");\n" +
				$"\tdateField.Format = string.IsNullOrEmpty(\"{currentField.Format}\") ? null : \"{currentField.Format}\";\n" +
				$"\tdateField.UseCurrentTimeAsDefaultValue = {(currentField.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
				$"\tdateField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				"\tdateField.Permissions = new FieldPermissions();\n" +
				"\tdateField.Permissions.CanRead = new List<Guid>();\n" +
				"\tdateField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";

			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tdateField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tdateField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), dateField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.Format != oldField.Format)
			{
				hasUpdate = true;
			}
			if (currentField.UseCurrentTimeAsDefaultValue != oldField.UseCurrentTimeAsDefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateDateTimeFieldCode(DbDateTimeField currentField, DbDateTimeField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =

			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputDateTimeField datetimeField = new InputDateTimeField();\n" +
				$"\tdatetimeField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tdatetimeField.Name = \"{currentField.Name}\";\n" +
				$"\tdatetimeField.Label = \"{currentField.Label}\";\n" +
				$"\tdatetimeField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\tdatetimeField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\tdatetimeField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\tdatetimeField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? (DateTime?)null : DateTime.Parse(\"{currentField.DefaultValue}\");\n" +
				$"\tdatetimeField.Format = string.IsNullOrEmpty(\"{currentField.Format}\") ? null : \"{currentField.Format}\";\n" +
				$"\tdatetimeField.UseCurrentTimeAsDefaultValue = {(currentField.UseCurrentTimeAsDefaultValue).ToString().ToLowerInvariant()};\n" +
				$"\tdatetimeField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				"\tdatetimeField.Permissions = new FieldPermissions();\n" +
				"\tdatetimeField.Permissions.CanRead = new List<Guid>();\n" +
				"\tdatetimeField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";

			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tdatetimeField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tdatetimeField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), datetimeField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.Format != oldField.Format)
			{
				hasUpdate = true;
			}
			if (currentField.UseCurrentTimeAsDefaultValue != oldField.UseCurrentTimeAsDefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateEmailFieldCode(DbEmailField currentField, DbEmailField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			response =

			#region << Code >>
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				"\tInputEmailField emailField = new InputEmailField();\n" +
				$"\temailField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\temailField.Name = \"{currentField.Name}\";\n" +
				$"\temailField.Label = \"{currentField.Label}\";\n" +
				$"\temailField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\temailField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\temailField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\temailField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\temailField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\temailField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\temailField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\temailField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\temailField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
				$"\temailField.MaxLength = string.IsNullOrEmpty(\"{currentField.MaxLength}\") ? (int?)null : Int32.Parse(\"{currentField.MaxLength}\");\n" +
				$"\temailField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				$"\temailField.Permissions = new FieldPermissions();\n" +
				$"\temailField.Permissions.CanRead = new List<Guid>();\n" +
				$"\temailField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\temailField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\temailField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), emailField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateFileFieldCode(DbFileField currentField, DbFileField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputFileField fileField = new InputFileField();\n" +
				$"\tfileField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tfileField.Name = \"{currentField.Name}\";\n" +
				$"\tfileField.Label = \"{currentField.Label}\";\n" +
				$"\tfileField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\tfileField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\tfileField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\tfileField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
				$"\tfileField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				$"\tfileField.Permissions = new FieldPermissions();\n" +
				$"\tfileField.Permissions.CanRead = new List<Guid>();\n" +
				$"\tfileField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tfileField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tfileField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), fileField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateImageFieldCode(DbImageField currentField, DbImageField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;

			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputImageField imageField = new InputImageField();\n" +
				$"\timageField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\timageField.Name = \"{currentField.Name}\";\n" +
				$"\timageField.Label = \"{currentField.Label}\";\n" +
				$"\timageField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\timageField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\timageField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\timageField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\timageField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\timageField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\timageField.Auditable =  {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\timageField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\timageField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
				$"\timageField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				$"\timageField.Permissions = new FieldPermissions();\n" +
				$"\timageField.Permissions.CanRead = new List<Guid>();\n" +
				$"\timageField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\timageField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\timageField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), imageField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateHtmlFieldCode(DbHtmlField currentField, DbHtmlField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputHtmlField htmlField = new InputHtmlField();\n" +
				$"\thtmlField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\thtmlField.Name = \"{currentField.Name}\";\n" +
				$"\thtmlField.Label = \"{currentField.Label}\";\n" +
				$"\thtmlField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\thtmlField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\thtmlField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\thtmlField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
				$"\thtmlField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				$"\thtmlField.Permissions = new FieldPermissions();\n" +
				$"\thtmlField.Permissions.CanRead = new List<Guid>();\n" +
				$"\thtmlField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\thtmlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\thtmlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), htmlField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdateMultiLineTextFieldCode(DbMultiLineTextField currentField, DbMultiLineTextField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;

			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputMultiLineTextField textareaField = new InputMultiLineTextField();\n" +
				$"\ttextareaField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\ttextareaField.Name = \"{currentField.Name}\";\n" +
				$"\ttextareaField.Label = \"{currentField.Label}\";\n" +
				$"\ttextareaField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\ttextareaField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\ttextareaField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\ttextareaField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\ttextareaField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\ttextareaField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\ttextareaField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\ttextareaField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				$"\ttextareaField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
				$"\ttextareaField.MaxLength = string.IsNullOrEmpty(\"{currentField.MaxLength}\") ? (int?)null : Int32.Parse(\"{currentField.MaxLength}\");\n" +
				$"\ttextareaField.VisibleLineNumber = string.IsNullOrEmpty(\"{currentField.VisibleLineNumber}\") ? (int?)null : Int32.Parse(\"{currentField.VisibleLineNumber}\");\n" +
				$"\ttextareaField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
				$"\ttextareaField.Permissions = new FieldPermissions();\n" +
				$"\ttextareaField.Permissions.CanRead = new List<Guid>();\n" +
				$"\ttextareaField.Permissions.CanUpdate = new List<Guid>();\n" +
				"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\ttextareaField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\ttextareaField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), textareaField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			if (currentField.VisibleLineNumber != oldField.VisibleLineNumber)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateMultiSelectFieldCode(DbMultiSelectField currentField, DbMultiSelectField oldField, Guid entityId, string entityName)
		{
			var response = "";
			var hasUpdate = false;
			#region << Code >>
			response =
			$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
			"{\n" +
				$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
				$"\tInputMultiSelectField multiSelectField = new InputMultiSelectField();\n" +
				$"\tmultiSelectField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
				$"\tmultiSelectField.Name = \"{currentField.Name}\";\n" +
				$"\tmultiSelectField.Label = \"{currentField.Label}\";\n" +
				$"\tmultiSelectField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
				$"\tmultiSelectField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
				$"\tmultiSelectField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
				$"\tmultiSelectField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
				$"\tmultiSelectField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
				"\tmultiSelectField.DefaultValue = ";
			var defaultValues = currentField.DefaultValue.ToList();
			if (defaultValues.Count > 0)
			{
				response += "new List<string>() {";

				for (int i = 0; i < defaultValues.Count; i++)
				{
					response += "\"" + defaultValues[i] + "\"";
					if (i < defaultValues.Count - 1)
					{
						response += ",";
					}
				}
				response += "};\n";
			}
			else
			{
				response += "null;\n";
			}
			response += "\tmultiSelectField.Options = ";
			var fieldOptions = currentField.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<MultiSelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew MultiSelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=

			$"\tmultiSelectField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tmultiSelectField.Permissions = new FieldPermissions();\n" +
			$"\tmultiSelectField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tmultiSelectField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tmultiSelectField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tmultiSelectField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), multiSelectField, false);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}

			var oldDefaultValuesDictionary = new Dictionary<string, bool>();
			//create dictionary
			foreach (var value in oldField.DefaultValue.ToList())
			{
				oldDefaultValuesDictionary[value] = true;
			}
			foreach (var value in currentField.DefaultValue.ToList())
			{
				if (!oldDefaultValuesDictionary.ContainsKey(value))
				{
					hasUpdate = true;
				}
			}

			var oldOptionsDictionary = new Dictionary<string, bool>();
			//create dictionary
			foreach (var value in oldField.Options.ToList())
			{
				oldOptionsDictionary[value.Key] = true;
			}
			foreach (var value in currentField.Options.ToList())
			{
				if (!oldOptionsDictionary.ContainsKey(value.Key))
				{
					hasUpdate = true;
				}
			}

			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateNumberFieldCode(DbNumberField currentField, DbNumberField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;
			#region << Code >>
			response =
$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputNumberField numberField = new InputNumberField();\n" +
	$"\tnumberField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tnumberField.Name = \"{currentField.Name}\";\n" +
	$"\tnumberField.Label = \"{currentField.Label}\";\n" +
	$"\tnumberField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
	$"\tnumberField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
	$"\tnumberField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
	$"\tnumberField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.DefaultValue}\");\n" +
	$"\tnumberField.MinValue = string.IsNullOrEmpty(\"{currentField.MinValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.MinValue}\");\n" +
	$"\tnumberField.MaxValue = string.IsNullOrEmpty(\"{currentField.MaxValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.MaxValue}\");\n" +
	$"\tnumberField.DecimalPlaces = string.IsNullOrEmpty(\"{currentField.DecimalPlaces}\") ? (byte?)null : byte.Parse(\"{currentField.DecimalPlaces}\");\n" +
	$"\tnumberField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tnumberField.Permissions = new FieldPermissions();\n" +
	$"\tnumberField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tnumberField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tnumberField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tnumberField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), numberField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MinValue != oldField.MinValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxValue != oldField.MaxValue)
			{
				hasUpdate = true;
			}
			if (currentField.DecimalPlaces != oldField.DecimalPlaces)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdatePasswordFieldCode(DbPasswordField currentField, DbPasswordField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputPasswordField passwordField = new InputPasswordField();\n" +
	$"\tpasswordField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tpasswordField.Name = \"{currentField.Name}\";\n" +
	$"\tpasswordField.Label = \"{currentField.Label}\";\n" +
	$"\tpasswordField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
	$"\tpasswordField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
	$"\tpasswordField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
	$"\tpasswordField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.MinLength = string.IsNullOrEmpty(\"{currentField.MinLength}\") ? (int?)null : Int32.Parse(\"{currentField.MinLength}\");\n" +
	$"\tpasswordField.MaxLength = string.IsNullOrEmpty(\"{currentField.MaxLength}\") ? (int?)null : Int32.Parse(\"{currentField.MaxLength}\");\n" +
	$"\tpasswordField.Encrypted = {(currentField.Encrypted).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tpasswordField.Permissions = new FieldPermissions();\n" +
	$"\tpasswordField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tpasswordField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tpasswordField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tpasswordField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), passwordField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.MinLength != oldField.MinLength)
			{
				hasUpdate = true;
			}
			if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			if (currentField.Encrypted != oldField.Encrypted)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}
			return response;
		}

		private string UpdatePercentFieldCode(DbPercentField currentField, DbPercentField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputPercentField percentField = new InputPercentField();\n" +
	$"\tpercentField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tpercentField.Name = \"{currentField.Name}\";\n" +
	$"\tpercentField.Label = \"{currentField.Label}\";\n" +
	$"\tpercentField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
	$"\tpercentField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
	$"\tpercentField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
	$"\tpercentField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.DefaultValue}\");\n" +
	$"\tpercentField.MinValue = string.IsNullOrEmpty(\"{currentField.MinValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.MinValue}\");\n" +
	$"\tpercentField.MaxValue = string.IsNullOrEmpty(\"{currentField.MaxValue}\") ? (decimal?)null : Decimal.Parse(\"{currentField.MaxValue}\");\n" +
	$"\tpercentField.DecimalPlaces = string.IsNullOrEmpty(\"{currentField.DecimalPlaces}\") ? (byte?)null : byte.Parse(\"{currentField.DecimalPlaces}\");\n" +
	$"\tpercentField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tpercentField.Permissions = new FieldPermissions();\n" +
	$"\tpercentField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tpercentField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tpercentField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tpercentField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), percentField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MinValue != oldField.MinValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxValue != oldField.MaxValue)
			{
				hasUpdate = true;
			}
			if (currentField.DecimalPlaces != oldField.DecimalPlaces)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdatePhoneFieldCode(DbPhoneField currentField, DbPhoneField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputPhoneField phoneField = new InputPhoneField();\n" +
	$"\tphoneField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tphoneField.Name = \"{currentField.Name}\";\n" +
	$"\tphoneField.Label =  \"{currentField.Label}\";\n" +
	$"\tphoneField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
	$"\tphoneField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
	$"\tphoneField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
	$"\tphoneField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
	$"\tphoneField.MaxLength = string.IsNullOrEmpty(\"{currentField.MaxLength}\") ? (int?)null : Int32.Parse(\"{currentField.MaxLength}\");\n" +
	$"\tphoneField.Format = string.IsNullOrEmpty(\"{currentField.Format}\") ? null : \"{currentField.Format}\";\n" +
	$"\tphoneField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tphoneField.Permissions = new FieldPermissions();\n" +
	$"\tphoneField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tphoneField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tphoneField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tphoneField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), phoneField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			if (currentField.Format != oldField.Format)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateGuidFieldCode(DbGuidField currentField, DbGuidField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =
$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputGuidField guidField = new InputGuidField();\n" +
	$"\tguidField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tguidField.Name = \"{currentField.Name}\";\n" +
	$"\tguidField.Label = \"{currentField.Label}\";\n" +
	$"\tguidField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
	$"\tguidField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
	$"\tguidField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
	$"\tguidField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? (Guid?)null : Guid.Parse(\"{currentField.DefaultValue}\");\n" +
	$"\tguidField.GenerateNewId = {(currentField.GenerateNewId).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
	$"\tguidField.Permissions = new FieldPermissions();\n" +
	$"\tguidField.Permissions.CanRead = new List<Guid>();\n" +
	$"\tguidField.Permissions.CanUpdate = new List<Guid>();\n" +
	"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tguidField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tguidField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), guidField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.GenerateNewId != oldField.GenerateNewId)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateSelectFieldCode(DbSelectField currentField, DbSelectField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =

$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
"{\n" +
	$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputSelectField dropdownField = new InputSelectField();\n" +
	$"\tdropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
	$"\tdropdownField.Name = \"{currentField.Name}\";\n" +
	$"\tdropdownField.Label = \"{currentField.Label}\";\n" +
	$"\tdropdownField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
	$"\tdropdownField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
	$"\tdropdownField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
	$"\tdropdownField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
	$"\tdropdownField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n";
			response += "\tdropdownField.Options = ";
			var fieldOptions = currentField.Options.ToList();
			if (fieldOptions.Count > 0)
			{
				response += "new List<SelectFieldOption>\n\t{\n";
				for (int i = 0; i < fieldOptions.Count; i++)
				{
					response += $"\t\tnew SelectFieldOption() {{ Key = \"{fieldOptions[i].Key}\", Value = \"{fieldOptions[i].Value}\"}}";
					if (i < fieldOptions.Count - 1)
					{
						response += ",\n";
					}
				}
				response += "\n\t};\n";
			}
			else
			{
				response += "null;\n";
			}
			response +=
			$"\tdropdownField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\tdropdownField.Permissions = new FieldPermissions();\n" +
			$"\tdropdownField.Permissions.CanRead = new List<Guid>();\n" +
			$"\tdropdownField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\tdropdownField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\tdropdownField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), dropdownField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}

			var oldOptionsDictionary = new Dictionary<string, bool>();
			//create dictionary
			foreach (var value in oldField.Options.ToList())
			{
				oldOptionsDictionary[value.Key] = true;
			}
			foreach (var value in currentField.Options.ToList())
			{
				if (!oldOptionsDictionary.ContainsKey(value.Key))
				{
					hasUpdate = true;
				}
			}

			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}


			return response;
		}

		private string UpdateTextFieldCode(DbTextField currentField, DbTextField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>

			response =
$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
"{\n" +
		$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
	$"\tInputTextField textboxField = new InputTextField();\n" +
			$"\ttextboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
			$"\ttextboxField.Name = \"{currentField.Name}\";\n" +
			$"\ttextboxField.Label = \"{currentField.Label}\";\n" +
			$"\ttextboxField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
			$"\ttextboxField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
			$"\ttextboxField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
			$"\ttextboxField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
			$"\ttextboxField.MaxLength = string.IsNullOrEmpty(\"{currentField.MaxLength}\") ? (int?)null : Int32.Parse(\"{currentField.MaxLength}\");\n" +
			$"\ttextboxField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\ttextboxField.Permissions = new FieldPermissions();\n" +
			$"\ttextboxField.Permissions.CanRead = new List<Guid>();\n" +
			$"\ttextboxField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\ttextboxField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\ttextboxField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), textboxField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";

			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		private string UpdateUrlFieldCode(DbUrlField currentField, DbUrlField oldField, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var hasUpdate = false;

			#region << Code >>
			response =

		$"#region << Update  Enity: {entityName} field: {currentField.Name} >>\n" +
		"{\n" +
			$"\tvar currentEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			$"\tInputUrlField urlField = new InputUrlField();\n" +
			$"\turlField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == \"{currentField.Name}\").Id;\n" +
			$"\turlField.Name = \"{currentField.Name}\";\n" +
			$"\turlField.Label = \"{currentField.Label}\";\n" +
			$"\turlField.PlaceholderText = string.IsNullOrEmpty(\"{currentField.PlaceholderText}\") ? null : \"{currentField.PlaceholderText}\";\n" +
			$"\turlField.Description = string.IsNullOrEmpty(\"{currentField.Description}\") ? string.Empty : \"{currentField.Description}\";\n" +
			$"\turlField.HelpText = string.IsNullOrEmpty(\"{currentField.HelpText}\") ? null : \"{currentField.HelpText}\";\n" +
			$"\turlField.Required = {(currentField.Required).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Unique = {(currentField.Unique).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Searchable = {(currentField.Searchable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Auditable = {(currentField.Auditable).ToString().ToLowerInvariant()};\n" +
			$"\turlField.System = {(currentField.System).ToString().ToLowerInvariant()};\n" +
			$"\turlField.DefaultValue = string.IsNullOrEmpty(\"{currentField.DefaultValue}\") ? string.Empty : \"{currentField.DefaultValue}\";\n" +
			$"\turlField.MaxLength = string.IsNullOrEmpty(\"{currentField.MaxLength}\") ? (int?)null : Int32.Parse(\"{currentField.MaxLength}\");\n" +
			$"\turlField.OpenTargetInNewWindow = {(currentField.OpenTargetInNewWindow).ToString().ToLowerInvariant()};\n" +
			$"\turlField.EnableSecurity = {(currentField.EnableSecurity).ToString().ToLowerInvariant()};\n" +
			$"\turlField.Permissions = new FieldPermissions();\n" +
			$"\turlField.Permissions.CanRead = new List<Guid>();\n" +
			$"\turlField.Permissions.CanUpdate = new List<Guid>();\n" +
			"\t//READ\n";
			foreach (var permId in currentField.Permissions.CanRead)
			{
				response += $"\turlField.Permissions.CanRead.Add(new Guid(\"{permId}\"));\n";
			}
			response += "\t//UPDATE\n";
			foreach (var permId in currentField.Permissions.CanUpdate)
			{
				response += $"\turlField.Permissions.CanUpdate.Add(new Guid(\"{permId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateField(new Guid(\"{entityId}\"), urlField);\n" +
				"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Field: {currentField.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Update check >>
			if (currentField.Name != oldField.Name)
			{
				hasUpdate = true;
			}
			if (currentField.Label != oldField.Label)
			{
				hasUpdate = true;
			}
			if (currentField.PlaceholderText != oldField.PlaceholderText)
			{
				hasUpdate = true;
			}
			if (currentField.Description != oldField.Description)
			{
				hasUpdate = true;
			}
			if (currentField.HelpText != oldField.HelpText)
			{
				hasUpdate = true;
			}
			if (currentField.Required != oldField.Required)
			{
				hasUpdate = true;
			}
			if (currentField.Unique != oldField.Unique)
			{
				hasUpdate = true;
			}
			if (currentField.Searchable != oldField.Searchable)
			{
				hasUpdate = true;
			}
			if (currentField.Auditable != oldField.Auditable)
			{
				hasUpdate = true;
			}
			if (currentField.System != oldField.System)
			{
				hasUpdate = true;
			}
			if (currentField.DefaultValue != oldField.DefaultValue)
			{
				hasUpdate = true;
			}
			if (currentField.MaxLength != oldField.MaxLength)
			{
				hasUpdate = true;
			}
			if (currentField.OpenTargetInNewWindow != oldField.OpenTargetInNewWindow)
			{
				hasUpdate = true;
			}
			if (currentField.EnableSecurity != oldField.EnableSecurity)
			{
				hasUpdate = true;
			}

			//Create old permissions Dictionaries
			var oldFieldCanReadPermissionDictionary = new Dictionary<Guid, bool>();
			var oldFieldCanUpdatePermissionDictionary = new Dictionary<Guid, bool>();
			foreach (var permission in oldField.Permissions.CanRead)
			{
				oldFieldCanReadPermissionDictionary[permission] = true;
			}
			foreach (var permission in oldField.Permissions.CanUpdate)
			{
				oldFieldCanUpdatePermissionDictionary[permission] = true;
			}

			foreach (var permission in currentField.Permissions.CanRead)
			{
				if (!oldFieldCanReadPermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			foreach (var permission in currentField.Permissions.CanUpdate)
			{
				if (!oldFieldCanUpdatePermissionDictionary.ContainsKey(permission))
				{
					hasUpdate = true;
				}
			}
			#endregion

			if (!hasUpdate)
			{
				return string.Empty;
			}

			return response;
		}

		#endregion // End field region

		#region << View >>
		private string CreateViewCode(DbRecordView view, Guid entityId, string entityName)
		{
			var response = string.Empty;
			//escape possible double quotes
			if (view.Label != null)
				view.Label = view.Label.Replace("\"", "\\\"");
			if (view.Title != null)
				view.Title = view.Title.Replace("\"", "\\\"");

			response +=
		   $"#region << View  Enity: {entityName} name: {view.Name} >>\n" +
		   "{\n" +
			   $"\tvar createViewEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			   $"\tvar createViewInput = new InputRecordView();\n\n" +
			   $"\t#region << details >>\n" +
			   $"\tcreateViewInput.Id = new Guid(\"{view.Id}\");\n" +
			   $"\tcreateViewInput.Type = \"{view.Type}\";\n" +
			   $"\tcreateViewInput.Name = \"{view.Name}\";\n" +
			   $"\tcreateViewInput.Label = \"{view.Label}\";\n" +
			   $"\tcreateViewInput.Title = \"{view.Title}\";\n" +
			   $"\tcreateViewInput.Default = {(view.Default).ToString().ToLowerInvariant()};\n" +
			   $"\tcreateViewInput.System = {(view.System).ToString().ToLowerInvariant()};\n" +
			   $"\tcreateViewInput.Weight = string.IsNullOrEmpty(\"{view.Weight}\") ? (decimal?)null : Decimal.Parse(\"{view.Weight}\");\n" +
			   $"\tcreateViewInput.CssClass = string.IsNullOrEmpty(\"{view.CssClass}\") ? null : \"{view.CssClass}\";\n" +
			   $"\tcreateViewInput.IconName = string.IsNullOrEmpty(\"{view.IconName}\") ? null : \"{view.IconName}\";\n" +
			   $"\tcreateViewInput.DynamicHtmlTemplate = string.IsNullOrEmpty(\"{view.DynamicHtmlTemplate}\") ? null : \"{view.DynamicHtmlTemplate}\";\n" +
			   $"\tcreateViewInput.DataSourceUrl = string.IsNullOrEmpty(\"{view.DataSourceUrl}\") ? null : \"{view.DataSourceUrl}\";\n" +
			   $"\tcreateViewInput.ServiceCode =string.IsNullOrEmpty(\"{view.ServiceCode}\") ? null : \"{view.ServiceCode}\";\n" +
			   $"\t#endregion\n\n" +
			   //Region
			   $"\t#region << regions >>\n" +
			   $"\tcreateViewInput.Regions = new List<InputRecordViewRegion>();\n\n";
			foreach (var region in view.Regions)
			{
				response += CreateViewRegionCode(region, entityId, entityName);
			}
			response += $"\t#endregion\n\n";

			//Relation options
			response +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tcreateViewInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in view.RelationOptions)
			{
				response += CreateRelationOptionCode(relationOption, entityId, entityName);
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			response +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tcreateViewInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in view.ActionItems)
			{
				response += CreateViewActionItemCode(actionItem, entityId, entityName);
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";
			//Sidebar
			response += CreateViewSidebarCode(view.Sidebar, entityId, entityName);
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateRecordView(new Guid(\"{entityId}\"), createViewInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Updated view: {view.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";


			return response;
		}
		//ViewRegion
		private string CreateViewRegionCode(DbRecordViewRegion region, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"\t\t#region << Region: {region.Name} >>\n" +
			"\t\t{\n" +
			$"\t\t\tvar viewRegion = new InputRecordViewRegion();\n" +
			$"\t\t\tviewRegion.Name = \"{region.Name}\";\n" +
			$"\t\t\tviewRegion.Label = \"{region.Label}\";\n" +
			$"\t\t\tviewRegion.Render = {(region.Render).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tviewRegion.Weight = string.IsNullOrEmpty(\"{region.Weight}\") ? (decimal?)null : Decimal.Parse(\"{region.Weight}\");\n" +
			$"\t\t\tviewRegion.CssClass = string.IsNullOrEmpty(\"{region.CssClass}\") ? null : \"{region.CssClass}\";\n" +
			$"\t\t\tviewRegion.Sections = new List<InputRecordViewSection>();\n\n";
			foreach (var section in region.Sections)
			{
				response += CreateViewSectionCode(section, entityId, entityName);
			}

			response +=
			$"\t\t\t//Save region\n" +
			$"\t\t\tcreateViewInput.Regions.Add(viewRegion);\n" +
			"\t\t}\n" +
			$"\t\t#endregion\n\n";

			return response;
		}

		//ViewSection
		private string CreateViewSectionCode(DbRecordViewSection section, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response =
			$"\t\t\t#region << Section: {section.Name} >>\n" +
			"\t\t\t{\n" +
			$"\t\t\tvar viewSection = new InputRecordViewSection();\n" +
			$"\t\t\tviewSection.Id = new Guid(\"{section.Id}\");\n" +
			$"\t\t\tviewSection.Name = \"{section.Name}\";\n" +
			$"\t\t\tviewSection.Label = \"{section.Label}\";\n" +
			$"\t\t\tviewSection.ShowLabel = {(section.ShowLabel).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tviewSection.CssClass = string.IsNullOrEmpty(\"{section.CssClass}\") ? null : \"{section.CssClass}\";\n" +
			$"\t\t\tviewSection.Collapsed = {(section.Collapsed).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tviewSection.TabOrder = string.IsNullOrEmpty(\"{section.TabOrder}\") ? null : \"{section.TabOrder}\";\n" +
			$"\t\t\tviewSection.Weight = string.IsNullOrEmpty(\"{section.Weight}\") ? (decimal?)null : Decimal.Parse(\"{section.Weight}\");\n" +
			$"\t\t\tviewSection.Rows = new List<InputRecordViewRow>();\n\n";
			var rowIndex = 1;
			foreach (var row in section.Rows)
			{
				response += CreateViewRowCode(row, entityId, entityName, rowIndex);
				rowIndex++;
			}
			response +=
			$"\t\t\t//Save section\n" +
			$"\t\t\tviewRegion.Sections.Add(viewSection);\n" +
			"\t\t\t}\n" +
			$"\t\t\t#endregion\n";

			return response;
		}

		//ViewRow
		private string CreateViewRowCode(DbRecordViewRow row, Guid entityId, string entityName, int rowIndex)
		{
			var response = string.Empty;
			response +=
			$"\t\t\t\t#region << Row {rowIndex}>>\n" +
			"\t\t\t\t{\n" +
			"\t\t\t\t\tvar viewRow = new InputRecordViewRow();\n" +
			$"\t\t\t\t\tviewRow.Id = new Guid(\"{row.Id}\");\n" +
			$"\t\t\t\t\tviewRow.Weight = string.IsNullOrEmpty(\"{row.Weight}\") ? (decimal?)null : Decimal.Parse(\"{row.Weight}\");\n" +
			$"\t\t\t\t\tviewRow.Columns = new List<InputRecordViewColumn>();\n\n";
			var colIndex = 1;
			foreach (var column in row.Columns)
			{
				response += CreateViewColumnCode(column, entityId, entityName, colIndex);
				colIndex++;
			}
			response +=
			"\t\t\t\t//Save row\n" +
			"\t\t\t\tviewSection.Rows.Add(viewRow);\n" +
			"\t\t\t\t}\n" +
			"\t\t\t\t#endregion\n";
			return response;
		}

		//ViewColumn
		private string CreateViewColumnCode(DbRecordViewColumn column, Guid entityId, string entityName, int colIndex)
		{
			var response = string.Empty;
			response +=
			$"\t\t\t\t\t#region << Column {colIndex} >>\n" +
			"\t\t\t\t\t{\n" +
			$"\t\t\t\t\tvar viewColumn = new InputRecordViewColumn();\n" +
			$"\t\t\t\t\tviewColumn.GridColCount = string.IsNullOrEmpty(\"{column.GridColCount}\") ? (int?)null : Int32.Parse(\"{column.GridColCount}\");\n" +
			$"\t\t\t\t\tviewColumn.Items = new List<InputRecordViewItemBase>();\n\n";
			foreach (var item in column.Items)
			{
				response += CreateViewItemCode(item, entityId, entityName);
			}
			response +=
			"\t\t\t\t\t//Save column\n" +
			"\t\t\t\t\tviewRow.Columns.Add(viewColumn);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";

			return response;
		}

		//field
		private string CreateViewItemCode(DbRecordViewItemBase item, Guid entityId, string entityName)
		{
			var response = string.Empty;
			if (item is DbRecordViewFieldItem)
			{
				response += CreateRecordViewFieldItemCode(item as DbRecordViewFieldItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationFieldItem)
			{
				response += CreateRecordViewRelationFieldItemCode(item as DbRecordViewRelationFieldItem, entityId, entityName);
			}
			else if (item is DbRecordViewViewItem)
			{
				response += CreateRecordViewViewItemCode(item as DbRecordViewViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationViewItem)
			{
				response += CreateRecordViewRelationViewItemCode(item as DbRecordViewRelationViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewListItem)
			{
				response += CreateRecordViewListItemCode(item as DbRecordViewListItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationListItem)
			{
				response += CreateRecordViewRelationListItemCode(item as DbRecordViewRelationListItem, entityId, entityName);
			}
			else if (item is DbRecordViewRelationTreeItem)
			{
				response += CreateRecordViewRelationTreeItemCode(item as DbRecordViewRelationTreeItem, entityId, entityName);
			}
			return response;
		}

		//field
		private string CreateRecordViewFieldItemCode(DbRecordViewFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			//var currentField = currentEntity.Fields.Single(x => x.Id == fieldItem.FieldId);
			Field currentField = null;
			try
			{
				currentField = currentEntity.Fields.Single(x => x.Id == fieldItem.FieldId);
			}
			catch (Exception ex)
			{
				var boz = ex;
			}
			response +=
			$"\t\t\t\t\t#region << {currentField.Name} >>\n" +
			"\t\t\t\t\t{\n" +
				"\t\t\t\t\t\tvar viewItem = new InputRecordViewFieldItem();\n" +
				$"\t\t\t\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
				$"\t\t\t\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
				$"\t\t\t\t\t\tviewItem.FieldId = new Guid(\"{fieldItem.FieldId}\");\n" +
				$"\t\t\t\t\t\tviewItem.FieldName = \"{currentField.Name}\";\n" +
				$"\t\t\t\t\t\tviewItem.Type = \"field\";\n" +
				$"\t\t\t\t\t\tviewColumn.Items.Add(viewItem);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//field from relation
		private string CreateRecordViewRelationFieldItemCode(DbRecordViewRelationFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(fieldItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(fieldItem.EntityId).Object;
			var relatedField = relatedEntity.Fields.Single(x => x.Id == fieldItem.FieldId);
			response +=
			$"\t\t\t\t\t#region << field from Relation: {relatedField.Name} >>\n" +
			"\t\t\t\t\t{\n" +
				"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationFieldItem();\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{fieldItem.EntityId}\");\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.Type = \"fieldFromRelation\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldId = new Guid(\"{relatedField.Id}\");\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldName = \"{relatedField.Name}\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{fieldItem.FieldLabel}\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{fieldItem.FieldPlaceholder}\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{fieldItem.FieldHelpText}\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(fieldItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = \"{fieldItem.FieldLookupList}\";\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{fieldItem.RelationId}\");\n" +
				$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
				$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//view
		private string CreateRecordViewViewItemCode(DbRecordViewViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentView = currentEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t\t\t\t#region << View: {currentView.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItem = new InputRecordViewViewItem();\n" +
			$"\t\t\t\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\t\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\t\t\t\tviewItem.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\t\t\t\tviewItem.ViewName = \"{currentView.Name}\";\n" +
			$"\t\t\t\t\t\tviewItem.Type = \"view\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItem);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//view from relation
		private string CreateRecordViewRelationViewItemCode(DbRecordViewRelationViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordViewItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordViewItem.EntityId).Object;
			var relatedView = relatedEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t\t\t\t#region << View from relation: {relatedView.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationViewItem();\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{recordViewItem.EntityId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ViewName = \"{relatedView.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{recordViewItem.FieldLabel}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{recordViewItem.FieldPlaceholder}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{recordViewItem.FieldHelpText}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(recordViewItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = \"{recordViewItem.FieldLookupList}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{recordViewItem.RelationId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.Type = \"viewFromRelation\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//list
		private string CreateRecordViewListItemCode(DbRecordViewListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentList = currentEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t\t\t\t#region << List: {currentList.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItem = new InputRecordViewListItem();\n" +
			$"\t\t\t\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\t\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\t\t\t\tviewItem.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\t\t\t\tviewItem.ListName = \"{currentList.Name}\";\n" +
			$"\t\t\t\t\t\tviewItem.Type = \"list\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItem);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";

			return response;
		}
		//list from relation
		private string CreateRecordViewRelationListItemCode(DbRecordViewRelationListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(listItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(listItem.EntityId).Object;
			var relatedList = relatedEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t\t\t\t#region << List from relation: {relatedList.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationListItem();\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{listItem.EntityId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.ListName = \"{relatedList.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{listItem.FieldLabel}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{listItem.FieldPlaceholder}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{listItem.FieldHelpText}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(listItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldLookupList = \"{listItem.FieldLookupList}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{listItem.RelationId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.Type = \"listFromRelation\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}
		//tree from relation
		private string CreateRecordViewRelationTreeItemCode(DbRecordViewRelationTreeItem treeItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(treeItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(treeItem.EntityId).Object;
			var relatedTree = relatedEntity.RecordTrees.Single(x => x.Id == treeItem.TreeId);
			response +=
			$"\t\t\t\t\t#region << Tree from relation: {relatedTree.Name} >>\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tvar viewItemFromRelation = new InputRecordViewRelationTreeItem();\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityId = new Guid(\"{treeItem.EntityId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.TreeId = new Guid(\"{treeItem.TreeId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.TreeName = \"{relatedTree.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldLabel = \"{treeItem.FieldLabel}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldPlaceholder = \"{treeItem.FieldPlaceholder}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldHelpText = \"{treeItem.FieldHelpText}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.FieldRequired = {(treeItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationId = new Guid(\"{treeItem.RelationId}\");\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\t\t\t\tviewItemFromRelation.Type = \"treeFromRelation\";\n" +
			$"\t\t\t\t\t\tviewColumn.Items.Add(viewItemFromRelation);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\t\t#endregion\n";
			return response;
		}

		//Sidebar
		private string CreateViewSidebarCode(DbRecordViewSidebar sidebar, Guid entityId, string entityName)
		{
			var response = string.Empty;

			if(sidebar == null) {
				response +=
				"\t#region << Sidebar >>\n" +	
				$"\tcreateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();\n\n" +		
				"\t#endregion\n";
				return response;	
			}
		
			response +=
			"\t#region << Sidebar >>\n" +
			"\tcreateViewInput.Sidebar = new InputRecordViewSidebar();\n" +
			$"\tcreateViewInput.Sidebar.CssClass = string.IsNullOrEmpty(\"{sidebar.CssClass}\") ? null : \"{sidebar.CssClass}\";\n" +
			$"\tcreateViewInput.Sidebar.Render = {(sidebar.Render).ToString().ToLowerInvariant()};\n" +
			$"\tcreateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();\n\n";
			foreach (var item in sidebar.Items)
			{
				response += CreateSidebarItemCode(item, entityId, entityName);
			}
			response +=
			"\t#endregion\n";
			return response;
		}

		private string CreateSidebarItemCode(DbRecordViewSidebarItemBase item, Guid entityId, string entityName)
		{
			var response = string.Empty;
			if (item is DbRecordViewSidebarViewItem)
			{
				response += CreateSidebarViewItemCode(item as DbRecordViewSidebarViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarRelationViewItem)
			{
				response += CreateSidebarRelationViewItemCode(item as DbRecordViewSidebarRelationViewItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarListItem)
			{
				response += CreateSidebarListItemCode(item as DbRecordViewSidebarListItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarRelationListItem)
			{
				response += CreateSidebarRelationListItemCode(item as DbRecordViewSidebarRelationListItem, entityId, entityName);
			}
			else if (item is DbRecordViewSidebarRelationTreeItem)
			{
				response += CreateSidebarRelationTreeItemCode(item as DbRecordViewSidebarRelationTreeItem, entityId, entityName);
			}
			return response;
		}

		//sidebar - view
		private string CreateSidebarViewItemCode(DbRecordViewSidebarViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentView = currentEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t#region << View: {currentView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItem = new InputRecordViewSidebarViewItem();\n" +
			$"\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tviewItem.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\tviewItem.ViewName = \"{currentView.Name}\";\n" +
			$"\t\t\tviewItem.Type = \"view\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";

			return response;
		}

		//sidebar - view from relation
		private string CreateSidebarRelationViewItemCode(DbRecordViewSidebarRelationViewItem recordViewItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordViewItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordViewItem.EntityId).Object;
			var relatedView = relatedEntity.RecordViews.Single(x => x.Id == recordViewItem.ViewId);
			response +=
			$"\t\t#region << view from relation: {relatedView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItemFromRelation = new InputRecordViewSidebarRelationViewItem();\n" +
			$"\t\t\tviewItemFromRelation.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tviewItemFromRelation.EntityName = \"{entityName}\";\n" +
			$"\t\t\tviewItemFromRelation.ViewId = new Guid(\"{recordViewItem.ViewId}\");\n" +
			$"\t\t\tviewItemFromRelation.ViewName =\"{relatedView.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldLabel = \"{recordViewItem.FieldLabel}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldPlaceholder = \"{recordViewItem.FieldPlaceholder}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldHelpText = \"{recordViewItem.FieldHelpText}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldRequired = {(recordViewItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tviewItemFromRelation.FieldLookupList = \"{recordViewItem.FieldLookupList}\";\n" +
			$"\t\t\tviewItemFromRelation.RelationId = new Guid(\"{recordViewItem.RelationId}\");\n" +
			$"\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.Type = \"viewFromRelation\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";

			return response;
		}

		//sidebar - list
		private string CreateSidebarListItemCode(DbRecordViewSidebarListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentList = currentEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t#region << List: {currentList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItem = new InputRecordViewSidebarListItem();\n" +
			$"\t\t\tviewItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tviewItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tviewItem.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\tviewItem.ListName = \"{currentList.Name}\";\n" +
			$"\t\t\tviewItem.Type = \"list\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//sidebar - list from relation
		private string CreateSidebarRelationListItemCode(DbRecordViewSidebarRelationListItem listItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(listItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(listItem.EntityId).Object;
			var relatedList = relatedEntity.RecordLists.Single(x => x.Id == listItem.ListId);
			response +=
			$"\t\t#region << list from relation: {relatedList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItemFromRelation = new InputRecordViewSidebarRelationListItem();\n" +
			$"\t\t\tviewItemFromRelation.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tviewItemFromRelation.EntityName = \"{entityName}\";\n" +
			$"\t\t\tviewItemFromRelation.ListId = new Guid(\"{listItem.ListId}\");\n" +
			$"\t\t\tviewItemFromRelation.ListName =\"{relatedList.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldLabel = \"{listItem.FieldLabel}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldPlaceholder = \"{listItem.FieldPlaceholder}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldHelpText = \"{listItem.FieldHelpText}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldRequired = {(listItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tviewItemFromRelation.FieldLookupList = \"{listItem.FieldLookupList}\";\n" +
			$"\t\t\tviewItemFromRelation.RelationId = new Guid(\"{listItem.RelationId}\");\n" +
			$"\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.Type = \"listFromRelation\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//sidebar - tree relation item
		private string CreateSidebarRelationTreeItemCode(DbRecordViewSidebarRelationTreeItem treeItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(treeItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(treeItem.EntityId).Object;
			var relatedTree = relatedEntity.RecordLists.Single(x => x.Id == treeItem.TreeId);
			response +=
			$"\t\t#region << list from relation: {relatedTree.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar viewItemFromRelation = new InputRecordViewSidebarRelationTreeItem();\n" +
			$"\t\t\tviewItemFromRelation.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tviewItemFromRelation.EntityName = \"{entityName}\";\n" +
			$"\t\t\tviewItemFromRelation.TreeId = new Guid(\"{treeItem.TreeId}\");\n" +
			$"\t\t\tviewItemFromRelation.TreeName =\"{relatedTree.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldLabel = \"{treeItem.FieldLabel}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldPlaceholder = \"{treeItem.FieldPlaceholder}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldHelpText = \"{treeItem.FieldHelpText}\";\n" +
			$"\t\t\tviewItemFromRelation.FieldRequired = {(treeItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tviewItemFromRelation.RelationId = new Guid(\"{treeItem.RelationId}\");\n" +
			$"\t\t\tviewItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tviewItemFromRelation.Type = \"treeFromRelation\";\n" +
			$"\t\t\tcreateViewInput.Sidebar.Items.Add(viewItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Action Item
		private string CreateViewActionItemCode(ActionItem actionItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			string lineSeparator = ((char)0x2028).ToString();
			string paragraphSeparator = ((char)0x2029).ToString();
			response +=
			$"\t\t#region << action item: {actionItem.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar actionItem = new ActionItem();\n" +
			$"\t\t\tactionItem.Name = \"{actionItem.Name}\";\n" +
			$"\t\t\tactionItem.Menu = \"{actionItem.Menu}\";\n" +
			$"\t\t\tactionItem.Weight = string.IsNullOrEmpty(\"{actionItem.Weight}\") ? (decimal?)null : Decimal.Parse(\"{actionItem.Weight}\");\n" +
			$"\t\t\tactionItem.Template =  @\"{actionItem.Template.Replace("\"", "\"\"")}\";\n" +
			$"\t\t\tcreateViewInput.ActionItems.Add(actionItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Relation option
		private string CreateRelationOptionCode(DbEntityRelationOptions relationOption, Guid entityId, string entityName)
		{
			var response = string.Empty;
			response +=
			$"#region << relation option name: {relationOption.RelationName} >>\n" +
			"{\n" +
			"\tvar relationOption = new EntityRelationOptionsItem();\n" +
			$"\trelationOption.RelationName = \"{relationOption.RelationName}\";\n" +
			$"\trelationOption.RelationId = \"{relationOption.RelationId}\";\n" +
			$"\trelationOption.Direction = \"{relationOption.Direction}\";\n" +
			$"\tcreateViewInput.RelationOptions.Add(relationOption);\n" +
			"}\n" +
			"#endregion\n";
			return response;
		}

		//Delete
		private string DeleteViewCode(DbRecordView view, Guid entityId, string entityName)
		{
			var response =
		$"#region << Delete  Enity: {entityName} view: {view.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteRecordView(new Guid(\"{entityId}\"),new Guid(\"{view.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. View: {view.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateViewCode(DbRecordView currentView, DbRecordView oldView, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;

			//escape possible double quotes
			if (currentView.Label != null)
				currentView.Label = currentView.Label.Replace("\"", "\\\"");
			if (currentView.Title != null)
				currentView.Title = currentView.Title.Replace("\"", "\\\"");
			//escape the old to so the update check is correct
			if (oldView.Label != null)
				oldView.Label = oldView.Label.Replace("\"", "\\\"");
			if (oldView.Title != null)
				oldView.Title = oldView.Title.Replace("\"", "\\\"");


			#region << Code >>
			code +=
		   $"#region << Update  Enity: {currentEntity.Name} View: {currentView.Name} >>\n" +
		   "{\n" +
			   $"\tvar updateViewEntity = entMan.ReadEntity(new Guid(\"{currentEntity.Id}\")).Object;\n" +
			   $"\tvar updateViewInput = new InputRecordView();\n\n" +
			   $"\t#region << details >>\n" +
			   $"\tupdateViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == \"{currentView.Name}\").Id;\n" +
			   $"\tupdateViewInput.Type = \"{currentView.Type}\";\n" +
			   $"\tupdateViewInput.Name = \"{currentView.Name}\";\n" +
			   $"\tupdateViewInput.Label = \"{currentView.Label}\";\n" +
			   $"\tupdateViewInput.Title = \"{currentView.Title}\";\n" +
			   $"\tupdateViewInput.Default = {(currentView.Default).ToString().ToLowerInvariant()};\n" +
			   $"\tupdateViewInput.System = {(currentView.System).ToString().ToLowerInvariant()};\n" +
			   $"\tupdateViewInput.Weight = string.IsNullOrEmpty(\"{currentView.Weight}\") ? (decimal?)null : Decimal.Parse(\"{currentView.Weight}\");\n" +
			   $"\tupdateViewInput.CssClass = string.IsNullOrEmpty(\"{currentView.CssClass}\") ? null : \"{currentView.CssClass}\";\n" +
			   $"\tupdateViewInput.IconName = string.IsNullOrEmpty(\"{currentView.IconName}\") ? null : \"{currentView.IconName}\";\n" +
			   $"\tupdateViewInput.DynamicHtmlTemplate = string.IsNullOrEmpty(\"{currentView.DynamicHtmlTemplate}\") ? null : \"{currentView.DynamicHtmlTemplate}\";\n" +
			   $"\tupdateViewInput.DataSourceUrl = string.IsNullOrEmpty(\"{currentView.DataSourceUrl}\") ? null : \"{currentView.DataSourceUrl}\";\n" +
			   $"\tupdateViewInput.ServiceCode =string.IsNullOrEmpty(\"{currentView.ServiceCode}\") ? null : \"{currentView.ServiceCode}\";\n" +
			   $"\t#endregion\n\n" +
			   //Region
			   $"\t#region << regions >>\n" +
			   $"\tupdateViewInput.Regions = new List<InputRecordViewRegion>();\n\n";
			foreach (var region in currentView.Regions)
			{
				code += CreateViewRegionCode(region, currentEntity.Id, currentEntity.Name);
			}
			code += $"\t#endregion\n\n";

			//Relation options
			code +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tupdateViewInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in currentView.RelationOptions)
			{
				code += CreateRelationOptionCode(relationOption, currentEntity.Id, currentEntity.Name);
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			code +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tupdateViewInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in currentView.ActionItems)
			{
				code += CreateViewActionItemCode(actionItem, currentEntity.Id, currentEntity.Name);
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";
			//Sidebar
			code += CreateViewSidebarCode(currentView.Sidebar, currentEntity.Id, currentEntity.Name);
			code +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateRecordView(new Guid(\"{currentEntity.Id}\"), updateViewInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {currentEntity.Name} Updated view: {oldView.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
		"}\n" +
		"#endregion\n\n";
			#endregion

			#region << Check change >>
			if (currentView.Type != oldView.Type)
			{
				hasUpdate = true;
			}
			if (currentView.Name != oldView.Name)
			{
				hasUpdate = true;
			}
			if (currentView.Label != oldView.Label)
			{
				hasUpdate = true;
			}
			if (currentView.Title != oldView.Title)
			{
				hasUpdate = true;
			}
			if (currentView.Default != oldView.Default)
			{
				hasUpdate = true;
			}
			if (currentView.System != oldView.System)
			{
				hasUpdate = true;
			}
			if (currentView.Weight != oldView.Weight)
			{
				hasUpdate = true;
			}
			if (currentView.CssClass != oldView.CssClass)
			{
				hasUpdate = true;
			}
			if (currentView.IconName != oldView.IconName)
			{
				hasUpdate = true;
			}
			if (currentView.DynamicHtmlTemplate != oldView.DynamicHtmlTemplate)
			{
				hasUpdate = true;
			}
			if (currentView.DataSourceUrl != oldView.DataSourceUrl)
			{
				hasUpdate = true;
			}
			if (currentView.ServiceCode != oldView.ServiceCode)
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentView.Regions) != JsonConvert.SerializeObject(oldView.Regions))
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentView.RelationOptions) != JsonConvert.SerializeObject(oldView.RelationOptions))
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentView.ActionItems) != JsonConvert.SerializeObject(oldView.ActionItems))
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentView.Sidebar) != JsonConvert.SerializeObject(oldView.Sidebar))
			{
				hasUpdate = true;
			}

			#endregion


			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>view</span>  with name <span class='go-red'>{oldView.Name}</span> was updated");
			return response;
		}

		#endregion

		#region << List >>
		private string CreateListCode(DbRecordList list, Guid entityId, string entityName)
		{
			var response = string.Empty;
			//escape possible double quotes
			if (list.Label != null)
				list.Label = list.Label.Replace("\"", "\\\"");

			response +=
			$"#region << List  Enity: {entityName} name: {list.Name} >>\n" +
			"{\n" +
			$"\tvar createListEntity = entMan.ReadEntity(new Guid(\"{entityId}\")).Object;\n" +
			$"\tvar createListInput = new InputRecordList();\n\n" +
			$"\t#region << details >>\n" +
			$"\tcreateListInput.Id = new Guid(\"{list.Id}\");\n" +
			$"\tcreateListInput.Type =  \"{list.Type}\";\n" +
			$"\tcreateListInput.Name = \"{list.Name}\";\n" +
			$"\tcreateListInput.Label = \"{list.Label}\";\n" +
			$"\tcreateListInput.Title = \"{list.Title}\";\n" +
			$"\tcreateListInput.Weight = string.IsNullOrEmpty(\"{list.Weight}\") ? (decimal?)null : Decimal.Parse(\"{list.Weight}\");\n" +
			$"\tcreateListInput.Default = {(list.Default).ToString().ToLowerInvariant()};\n" +
			$"\tcreateListInput.System = {(list.System).ToString().ToLowerInvariant()};\n" +
			$"\tcreateListInput.CssClass = string.IsNullOrEmpty(\"{list.CssClass}\") ? null : \"{list.CssClass}\";\n" +
			$"\tcreateListInput.IconName = string.IsNullOrEmpty(\"{list.IconName}\") ? null : \"{list.IconName}\";\n" +
			$"\tcreateListInput.VisibleColumnsCount = string.IsNullOrEmpty(\"{list.VisibleColumnsCount}\") ? (int?)null : Int32.Parse(\"{list.VisibleColumnsCount}\");\n" +
			$"\tcreateListInput.ColumnWidthsCSV = string.IsNullOrEmpty(\"{list.ColumnWidthsCSV}\") ? null : \"{list.ColumnWidthsCSV}\";\n" +
			$"\tcreateListInput.PageSize = string.IsNullOrEmpty(\"{list.PageSize}\") ? (int?)null : Int32.Parse(\"{list.PageSize}\");\n" +
			$"\tcreateListInput.DynamicHtmlTemplate = string.IsNullOrEmpty(\"{list.DynamicHtmlTemplate}\") ? null : \"{list.DynamicHtmlTemplate}\";\n" +
			$"\tcreateListInput.DataSourceUrl = string.IsNullOrEmpty(\"{list.DataSourceUrl}\") ? null : \"{list.DataSourceUrl}\";\n" +
			$"\tcreateListInput.ServiceCode = string.IsNullOrEmpty(\"{list.ServiceCode}\") ? null : \"{list.ServiceCode}\";\n" +
			$"\t#endregion\n\n";


			//Relation options
			response +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tcreateListInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in list.RelationOptions)
			{
				response += CreateRelationOptionCode(relationOption, entityId, entityName);
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			response +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tcreateListInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in list.ActionItems)
			{
				response += CreateListActionItemCode(actionItem, entityId, entityName);
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Columns
			response +=
			$"\t#region << Columns >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Columns = new List<InputRecordListItemBase>();\n\n";
			foreach (var column in list.Columns)
			{
				response += CreateListColumnCode(column, entityId, entityName, list.Name);
			}

			response += "\t}\n" +
			$"\t#endregion\n\n";



			//Query
			response +=
			$"\t#region << Query >>\n" +
			"\t{\n";
			if (list.Query == null)
			{
				response += $"\tcreateListInput.Query = null;\n";
			}
			else
			{
				response +=
				$"\tcreateListInput.Query = new InputRecordListQuery();\n" +
				$"\tvar queryDictionary = new Dictionary<Guid,InputRecordListQuery>();\n" +
				$"\tvar subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();\n" +
				$"\t//Main query rule\n" +
				$"\tcreateListInput.Query.FieldName = string.IsNullOrEmpty(\"{list.Query.FieldName}\") ? \"\" : \"{list.Query.FieldName}\";\n" +
				$"\tcreateListInput.Query.FieldValue =  string.IsNullOrEmpty(\"{list.Query.FieldValue}\") ? \"\" : \"{list.Query.FieldValue}\";\n" +
				$"\tcreateListInput.Query.QueryType = string.IsNullOrEmpty(\"{list.Query.QueryType}\") ? \"\" : \"{list.Query.QueryType}\";\n" +
				$"\tcreateListInput.Query.SubQueries = new List<InputRecordListQuery>();\n";
				var nodeId = Guid.NewGuid();
				foreach (var query in list.Query.SubQueries)
				{
					response += CreateListQueryCode(query, entityId, entityName, nodeId, 1);
				}
				response +=
				$"\tif(subQueryDictionary.ContainsKey(new Guid(\"{nodeId}\"))) {{createListInput.Query.SubQueries = subQueryDictionary[new Guid(\"{nodeId}\")];}}\n";
			}
			response += "\t}\n" +
			$"\t#endregion\n\n";

			//Sort
			response +=
			$"\t#region << Sorts >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Sorts = new List<InputRecordListSort>();\n\n";
			foreach (var sort in list.Sorts)
			{
				response += CreateListSortCode(sort, entityId, entityName);
			}

			response += "\t}\n" +
			$"\t#endregion\n\n";

			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateRecordList(new Guid(\"{entityId}\"), createListInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Created list: {list.Name} Message:\" + response.Message);\n" +
			"\t}\n";

			response +=
			"}\n" +
			"#endregion\n\n";
			return response;
		}

		//Action Item
		private string CreateListActionItemCode(ActionItem actionItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			string lineSeparator = ((char)0x2028).ToString();
			string paragraphSeparator = ((char)0x2029).ToString();
			response +=
			$"\t\t#region << action item: {actionItem.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar actionItem = new ActionItem();\n" +
			$"\t\t\tactionItem.Name = \"{actionItem.Name}\";\n" +
			$"\t\t\tactionItem.Menu = \"{actionItem.Menu}\";\n" +
			$"\t\t\tactionItem.Weight = string.IsNullOrEmpty(\"{actionItem.Weight}\") ? (decimal?)null : Decimal.Parse(\"{actionItem.Weight}\");\n" +
			$"\t\t\tactionItem.Template = @\"{actionItem.Template.Replace("\"", "\"\"")}\";\n" +
			$"\t\t\tcreateListInput.ActionItems.Add(actionItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Column Item
		private string CreateListColumnCode(DbRecordListItemBase item, Guid entityId, string entityName, string ListName)
		{
			var response = string.Empty;
			if (item is DbRecordListFieldItem)
			{
				response += CreateRecordListFieldItemCode(item as DbRecordListFieldItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationFieldItem)
			{
				response += CreateRecordListRelationFieldItemCode(item as DbRecordListRelationFieldItem, entityId, entityName);
			}
			else if (item is DbRecordListViewItem)
			{
				response += CreateRecordListViewItemCode(item as DbRecordListViewItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationViewItem)
			{
				response += CreateRecordListRelationViewItemCode(item as DbRecordListRelationViewItem, entityId, entityName);
			}
			else if (item is DbRecordListListItem)
			{
				response += CreateRecordListListItemCode(item as DbRecordListListItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationListItem)
			{
				response += CreateRecordListRelationListItemCode(item as DbRecordListRelationListItem, entityId, entityName);
			}
			else if (item is DbRecordListRelationTreeItem)
			{
				response += CreateRecordListRelationTreeItemCode(item as DbRecordListRelationTreeItem, entityId, entityName);
			}

			return response;
		}

		//field
		private string CreateRecordListFieldItemCode(DbRecordListFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentField = currentEntity.Fields.Single(x => x.Id == fieldItem.FieldId);
			response +=
			$"\t\t#region << {currentField.Name} >>\n" +
			"\t\t{\n" +
				"\t\t\tvar listField = new InputRecordListFieldItem();\n" +
				$"\t\t\tlistField.EntityId = new Guid(\"{entityId}\");\n" +
				$"\t\t\tlistField.EntityName = \"{entityName}\";\n" +
				$"\t\t\tlistField.FieldId = new Guid(\"{fieldItem.FieldId}\");\n" +
				$"\t\t\tlistField.FieldName = \"{currentField.Name}\";\n" +
				$"\t\t\tlistField.Type = \"field\";\n" +
				$"\t\t\tcreateListInput.Columns.Add(listField);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//field from relation
		private string CreateRecordListRelationFieldItemCode(DbRecordListRelationFieldItem fieldItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(fieldItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(fieldItem.EntityId).Object;
			var relatedField = relatedEntity.Fields.SingleOrDefault(x => x.Id == fieldItem.FieldId);
			if(relatedField == null) {
				response += "/////////////////////////////////////////////////////////////////////////////\n";
				response += "//WARNING: Field from relation was not found: fieldId: " + fieldItem.FieldId + " in entity: " + entityName + "\n";
				response += "/////////////////////////////////////////////////////////////////////////////\n";
			}
			else {
			response +=
			$"\t\t#region << field from Relation: {relatedField.Name} >>\n" +
			"\t\t{\n" +
				"\t\t\tvar listItemFromRelation = new InputRecordListRelationFieldItem();\n" +
				$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{fieldItem.EntityId}\");\n" +
				$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
				$"\t\t\tlistItemFromRelation.Type = \"fieldFromRelation\";\n" +
				$"\t\t\tlistItemFromRelation.FieldId = new Guid(\"{relatedField.Id}\");\n" +
				$"\t\t\tlistItemFromRelation.FieldName = \"{relatedField.Name}\";\n" +
				$"\t\t\tlistItemFromRelation.FieldLabel = \"{fieldItem.FieldLabel}\";\n" +
				$"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{fieldItem.FieldPlaceholder}\";\n" +
				$"\t\t\tlistItemFromRelation.FieldHelpText = \"{fieldItem.FieldHelpText}\";\n" +
				$"\t\t\tlistItemFromRelation.FieldRequired = {(fieldItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
				$"\t\t\tlistItemFromRelation.FieldLookupList = \"{fieldItem.FieldLookupList}\";\n" +
				$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{fieldItem.RelationId}\");\n" +
				$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
				$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			}
			return response;
		}

		//view
		private string CreateRecordListViewItemCode(DbRecordListViewItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentView = currentEntity.RecordViews.Single(x => x.Id == recordListItem.ViewId);
			response +=
			$"\t\t#region << View: {currentView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItem = new InputRecordListlistItem();\n" +
			$"\t\t\tlistItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tlistItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tlistItem.ViewId = new Guid(\"{recordListItem.ViewId}\");\n" +
			$"\t\t\tlistItem.ViewName = \"{currentView.Name}\";\n" +
			$"\t\t\tlistItem.Type = \"view\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//view from relation
		private string CreateRecordListRelationViewItemCode(DbRecordListRelationViewItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordListItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordListItem.EntityId).Object;
			var relatedView = relatedEntity.RecordViews.Single(x => x.Id == recordListItem.ViewId);
			response +=
			$"\t\t#region << View from relation: {relatedView.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItemFromRelation = new InputRecordListRelationViewItem();\n" +
			$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{recordListItem.EntityId}\");\n" +
			$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.ViewId = new Guid(\"{recordListItem.ViewId}\");\n" +
			$"\t\t\tlistItemFromRelation.ViewName = \"{relatedView.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldLabel = \"{recordListItem.FieldLabel}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{recordListItem.FieldPlaceholder}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldHelpText = \"{recordListItem.FieldHelpText}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldRequired = {(recordListItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tlistItemFromRelation.FieldLookupList = \"{recordListItem.FieldLookupList}\";\n" +
			$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{recordListItem.RelationId}\");\n" +
			$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.Type = \"viewFromRelation\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//list
		private string CreateRecordListListItemCode(DbRecordListListItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			var currentList = currentEntity.RecordLists.Single(x => x.Id == recordListItem.ListId);
			response +=
			$"\t\t#region << List: {currentList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItem = new InputRecordListListItem();\n" +
			$"\t\t\tlistItem.EntityId = new Guid(\"{entityId}\");\n" +
			$"\t\t\tlistItem.EntityName = \"{entityName}\";\n" +
			$"\t\t\tlistItem.ListId = new Guid(\"{recordListItem.ListId}\");\n" +
			$"\t\t\tlistItem.ListName = \"{currentList.Name}\";\n" +
			$"\t\t\tlistItem.Type = \"list\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItem);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";

			return response;
		}

		//list from relation
		private string CreateRecordListRelationListItemCode(DbRecordListRelationListItem recordListItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(recordListItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(recordListItem.EntityId).Object;
			var relatedList = relatedEntity.RecordViews.Single(x => x.Id == recordListItem.ListId);
			response +=
			$"\t\t#region << List from relation: {relatedList.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItemFromRelation = new InputRecordListRelationListItem();\n" +
			$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{recordListItem.EntityId}\");\n" +
			$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.ListId = new Guid(\"{recordListItem.ListId}\");\n" +
			$"\t\t\tlistItemFromRelation.ListName = \"{relatedList.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldLabel = \"{recordListItem.FieldLabel}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{recordListItem.FieldPlaceholder}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldHelpText = \"{recordListItem.FieldHelpText}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldRequired = {(recordListItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tlistItemFromRelation.FieldLookupList = \"{recordListItem.FieldLookupList}\";\n" +
			$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{recordListItem.RelationId}\");\n" +
			$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.Type = \"listFromRelation\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		//tree from relation
		private string CreateRecordListRelationTreeItemCode(DbRecordListRelationTreeItem treeItem, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentRelation = relMan.Read(treeItem.RelationId).Object;
			var relatedEntity = entMan.ReadEntity(treeItem.EntityId).Object;
			var relatedTree = relatedEntity.RecordTrees.Single(x => x.Id == treeItem.TreeId);
			response +=
			$"\t\t#region << Tree from relation: {relatedTree.Name} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar listItemFromRelation = new InputRecordListRelationTreeItem();\n" +
			$"\t\t\tlistItemFromRelation.EntityId = new Guid(\"{treeItem.EntityId}\");\n" +
			$"\t\t\tlistItemFromRelation.EntityName = \"{relatedEntity.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.TreeId = new Guid(\"{treeItem.TreeId}\");\n" +
			$"\t\t\tlistItemFromRelation.TreeName = \"{relatedTree.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldLabel = \"{treeItem.FieldLabel}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldPlaceholder = \"{treeItem.FieldPlaceholder}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldHelpText = \"{treeItem.FieldHelpText}\";\n" +
			$"\t\t\tlistItemFromRelation.FieldRequired = {(treeItem.FieldRequired).ToString().ToLowerInvariant()};\n" +
			$"\t\t\tlistItemFromRelation.RelationId = new Guid(\"{treeItem.RelationId}\");\n" +
			$"\t\t\tlistItemFromRelation.RelationName = \"{currentRelation.Name}\";\n" +
			$"\t\t\tlistItemFromRelation.Type = \"treeFromRelation\";\n" +
			$"\t\t\tcreateListInput.Columns.Add(listItemFromRelation);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n";
			return response;
		}

		private string CreateListQueryCode(DbRecordListQuery query, Guid entityId, string entityName, Guid nodeId, int subQueryLevel)
		{
			var response = string.Empty;
			var levelTabs = "";
			for (int i = 0; i < subQueryLevel + 1; i++)
			{
				levelTabs += "\t";
			}
			var encodedFieldValue = query.FieldValue;

			if (query.FieldValue != null && query.FieldValue.StartsWith("{"))
			{
				encodedFieldValue = query.FieldValue.Replace("\"", "\\\"");
			}

			var newNodeId = Guid.NewGuid();
			response +=
			levelTabs + "{\n" +
			levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")] = new InputRecordListQuery();\n" +
			levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].FieldName = string.IsNullOrEmpty(\"{query.FieldName}\") ? \"\" : \"{query.FieldName}\";\n" +
			levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].FieldValue =  string.IsNullOrEmpty(\"{encodedFieldValue}\") ? \"\" : \"{encodedFieldValue}\";\n" +
			levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].QueryType = string.IsNullOrEmpty(\"{query.QueryType}\") ? \"\" : \"{query.QueryType}\";\n" +
			levelTabs + $"queryDictionary[new Guid(\"{newNodeId}\")].SubQueries = new List<InputRecordListQuery>();\n";
			foreach (var subQuery in query.SubQueries)
			{
				response += CreateListQueryCode(subQuery, entityId, entityName, newNodeId, subQueryLevel + 1);
			}
			response +=
			levelTabs + $"if(subQueryDictionary.ContainsKey(new Guid(\"{newNodeId}\"))) {{queryDictionary[new Guid(\"{newNodeId}\")].SubQueries = subQueryDictionary[new Guid(\"{newNodeId}\")];}}\n" +
			levelTabs + $"if(!subQueryDictionary.ContainsKey(new Guid(\"{nodeId}\"))) {{subQueryDictionary[new Guid(\"{nodeId}\")] = new List<InputRecordListQuery>();}}\n" +
			levelTabs + $"subQueryDictionary[new Guid(\"{nodeId}\")].Add(queryDictionary[new Guid(\"{newNodeId}\")]);\n" +
			levelTabs + "}\n";
			return response;
		}

		//Sort
		private string CreateListSortCode(DbRecordListSort sort, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var encodedFieldName = sort.FieldName;
			if (sort.FieldName.StartsWith("{"))
			{
				encodedFieldName = sort.FieldName.Replace("\"", "\\\"");
			}

			response +=
			$"\t\t#region << sort >>\n" +
			"\t\t{\n" +
			"\t\t\tvar sort = new InputRecordListSort();\n" +
			$"\t\t\tsort.FieldName = \"{encodedFieldName}\";\n" +
			$"\t\t\tsort.SortType = \"{sort.SortType}\";\n" +
			$"\t\t\tcreateListInput.Sorts.Add(sort);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Delete
		private string DeleteListCode(DbRecordList list, Guid entityId, string entityName)
		{
			var response =
		$"#region << Delete  Enity: {entityName} list: {list.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteRecordList(new Guid(\"{entityId}\"),new Guid(\"{list.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. List: {list.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateListCode(DbRecordList currentList, DbRecordList oldList, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentList.Label != null)
				currentList.Label = currentList.Label.Replace("\"", "\\\"");

			//escape the old too
			if (oldList.Label != null)
				oldList.Label = oldList.Label.Replace("\"", "\\\"");

			#region << Code >>
			code +=
			$"#region << Update  Enity: {currentEntity.Name} name: {currentList.Name} >>\n" +
			"{\n" +
			$"\tvar createListEntity = entMan.ReadEntity(new Guid(\"{currentEntity.Id}\")).Object;\n" +
			$"\tvar createListInput = new InputRecordList();\n\n" +
			$"\t#region << details >>\n" +
			$"\tcreateListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == \"{currentList.Name}\").Id;\n" +
			$"\tcreateListInput.Type =  \"{currentList.Type}\";\n" +
			$"\tcreateListInput.Name = \"{currentList.Name}\";\n" +
			$"\tcreateListInput.Label = \"{currentList.Label}\";\n" +
			$"\tcreateListInput.Title = \"{currentList.Title}\";\n" +
			$"\tcreateListInput.Weight = string.IsNullOrEmpty(\"{currentList.Weight}\") ? (decimal?)null : Decimal.Parse(\"{currentList.Weight}\");\n" +
			$"\tcreateListInput.Default = {(currentList.Default).ToString().ToLowerInvariant()};\n" +
			$"\tcreateListInput.System = {(currentList.System).ToString().ToLowerInvariant()};\n" +
			$"\tcreateListInput.CssClass = string.IsNullOrEmpty(\"{currentList.CssClass}\") ? null : \"{currentList.CssClass}\";\n" +
			$"\tcreateListInput.IconName = string.IsNullOrEmpty(\"{currentList.IconName}\") ? null : \"{currentList.IconName}\";\n" +
			$"\tcreateListInput.VisibleColumnsCount = string.IsNullOrEmpty(\"{currentList.VisibleColumnsCount}\") ? (int?)null : Int32.Parse(\"{currentList.VisibleColumnsCount}\");\n" +
			$"\tcreateListInput.ColumnWidthsCSV = string.IsNullOrEmpty(\"{currentList.ColumnWidthsCSV}\") ? null : \"{currentList.ColumnWidthsCSV}\";\n" +
			$"\tcreateListInput.PageSize = string.IsNullOrEmpty(\"{currentList.PageSize}\") ? (int?)null : Int32.Parse(\"{currentList.PageSize}\");\n" +
			$"\tcreateListInput.DynamicHtmlTemplate = string.IsNullOrEmpty(\"{currentList.DynamicHtmlTemplate}\") ? null : \"{currentList.DynamicHtmlTemplate}\";\n" +
			$"\tcreateListInput.DataSourceUrl = string.IsNullOrEmpty(\"{currentList.DataSourceUrl}\") ? null : \"{currentList.DataSourceUrl}\";\n" +
			$"\tcreateListInput.ServiceCode = string.IsNullOrEmpty(\"{currentList.ServiceCode}\") ? null : \"{currentList.ServiceCode}\";\n" +
			$"\t#endregion\n\n";


			//Relation options
			code +=
			$"\t#region << Relation options >>\n" +
			"\t{\n" +
			$"\tcreateListInput.RelationOptions = new List<EntityRelationOptionsItem>();\n";
			foreach (var relationOption in currentList.RelationOptions)
			{
				code += CreateRelationOptionCode(relationOption, currentEntity.Id, currentEntity.Name);
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Action items
			code +=
			$"\t#region << Action items >>\n" +
			"\t{\n" +
			$"\tcreateListInput.ActionItems = new List<ActionItem>();\n\n";
			foreach (var actionItem in currentList.ActionItems)
			{
				code += CreateListActionItemCode(actionItem, currentEntity.Id, currentEntity.Name);
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Columns
			code +=
			$"\t#region << Columns >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Columns = new List<InputRecordListItemBase>();\n\n";
			foreach (var column in currentList.Columns)
			{
				code += CreateListColumnCode(column, currentEntity.Id, currentEntity.Name, currentList.Name);
			}

			code += "\t}\n" +
			$"\t#endregion\n\n";



			//Query
			code +=
			$"\t#region << Query >>\n" +
			"\t{\n";
			if (currentList.Query == null)
			{
				code += $"\tcreateListInput.Query = null;\n";
			}
			else
			{
				code +=
				$"\tcreateListInput.Query = new InputRecordListQuery();\n" +
				$"\tvar queryDictionary = new Dictionary<Guid,InputRecordListQuery>();\n" +
				$"\tvar subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();\n" +
				$"\t//Main query rule\n" +
				$"\tcreateListInput.Query.FieldName = string.IsNullOrEmpty(\"{currentList.Query.FieldName}\") ? \"\" : \"{currentList.Query.FieldName}\";\n" +
				$"\tcreateListInput.Query.FieldValue =  string.IsNullOrEmpty(\"{currentList.Query.FieldValue}\") ? \"\" : \"{currentList.Query.FieldValue}\";\n" +
				$"\tcreateListInput.Query.QueryType = string.IsNullOrEmpty(\"{currentList.Query.QueryType}\") ? \"\" : \"{currentList.Query.QueryType}\";\n" +
				$"\tcreateListInput.Query.SubQueries = new List<InputRecordListQuery>();\n";
				var nodeId = Guid.NewGuid();
				foreach (var query in currentList.Query.SubQueries)
				{
					code += CreateListQueryCode(query, currentEntity.Id, currentEntity.Name, nodeId, 1);
				}
				code +=
				$"\tif(subQueryDictionary.ContainsKey(new Guid(\"{nodeId}\"))) {{createListInput.Query.SubQueries = subQueryDictionary[new Guid(\"{nodeId}\")];}}\n";
			}
			code += "\t}\n" +
			$"\t#endregion\n\n";

			//Sort
			code +=
			$"\t#region << Sorts >>\n" +
			"\t{\n" +
			$"\tcreateListInput.Sorts = new List<InputRecordListSort>();\n\n";
			foreach (var sort in currentList.Sorts)
			{
				code += CreateListSortCode(sort, currentEntity.Id, currentEntity.Name);
			}

			code += "\t}\n" +
			$"\t#endregion\n\n";

			code +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateRecordList(new Guid(\"{currentEntity.Id}\"), createListInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {currentEntity.Name} Updated list: {currentList.Name} Message:\" + response.Message);\n" +
			"\t}\n";

			code +=
			"}\n" +
			"#endregion\n\n";
			#endregion

			#region << Check change >>
			if (currentList.Type != oldList.Type)
			{
				hasUpdate = true;
			}
			if (currentList.Name != oldList.Name)
			{
				hasUpdate = true;
			}
			if (currentList.Label != oldList.Label)
			{
				hasUpdate = true;
			}
			if (currentList.Title != oldList.Title)
			{
				hasUpdate = true;
			}
			if (currentList.Weight != oldList.Weight)
			{
				hasUpdate = true;
			}
			if (currentList.Default != oldList.Default)
			{
				hasUpdate = true;
			}
			if (currentList.System != oldList.System)
			{
				hasUpdate = true;
			}
			if (currentList.CssClass != oldList.CssClass)
			{
				hasUpdate = true;
			}
			if (currentList.IconName != oldList.IconName)
			{
				hasUpdate = true;
			}
			if (currentList.VisibleColumnsCount != oldList.VisibleColumnsCount)
			{
				hasUpdate = true;
			}
			if (currentList.ColumnWidthsCSV != oldList.ColumnWidthsCSV)
			{
				hasUpdate = true;
			}
			if (currentList.PageSize != oldList.PageSize)
			{
				hasUpdate = true;
			}
			if (currentList.DynamicHtmlTemplate != oldList.DynamicHtmlTemplate)
			{
				hasUpdate = true;
			}
			if (currentList.DataSourceUrl != oldList.DataSourceUrl)
			{
				hasUpdate = true;
			}
			if (currentList.ServiceCode != oldList.ServiceCode)
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.RelationOptions) != JsonConvert.SerializeObject(oldList.RelationOptions))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.ActionItems) != JsonConvert.SerializeObject(oldList.ActionItems))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.Columns) != JsonConvert.SerializeObject(oldList.Columns))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.Query) != JsonConvert.SerializeObject(oldList.Query))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentList.Sorts) != JsonConvert.SerializeObject(oldList.Sorts))
			{
				hasUpdate = true;
			}
			#endregion


			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>list</span>  with name <span class='go-red'>{oldList.Name}</span> was updated");
			return response;
		}

		#endregion

		#region << Tree >>
		private string CreateTreeCode(DbRecordTree tree, Guid entityId, string entityName)
		{
			var response = string.Empty;
			//escape possible double quotes
			if (tree.Label != null)
				tree.Label = tree.Label.Replace("\"", "\\\"");

			response +=
		   $"#region << Tree  Enity: {entityName} name: {tree.Name} >>\n" +
		   "{\n" +
			"\tvar createTreeInput = new InputRecordTree();\n" +
			$"\tcreateTreeInput.Id = new Guid(\"{tree.Id}\");\n" +
			$"\tcreateTreeInput.Name = \"{tree.Name}\";\n" +
			$"\tcreateTreeInput.Label = \"{tree.Label}\";\n" +
			$"\tcreateTreeInput.Default = {(tree.Default).ToString().ToLowerInvariant()};\n" +
			$"\tcreateTreeInput.System = {(tree.System).ToString().ToLowerInvariant()};\n" +
			$"\tcreateTreeInput.CssClass = string.IsNullOrEmpty(\"{tree.CssClass}\") ? null : \"{tree.CssClass}\";\n" +
			$"\tcreateTreeInput.IconName = string.IsNullOrEmpty(\"{tree.IconName}\") ? null : \"{tree.IconName}\";\n" +
			$"\tcreateTreeInput.RelationId = new Guid(\"{tree.RelationId}\");\n" +
			$"\tcreateTreeInput.DepthLimit = string.IsNullOrEmpty(\"{tree.DepthLimit}\") ? (int?)null : Int32.Parse(\"{tree.DepthLimit}\");\n" +
			$"\tcreateTreeInput.NodeIdFieldId = string.IsNullOrEmpty(\"{tree.NodeIdFieldId}\") ? (Guid?)null : Guid.Parse(\"{tree.NodeIdFieldId}\");\n" +
			$"\tcreateTreeInput.NodeParentIdFieldId = string.IsNullOrEmpty(\"{tree.NodeWeightFieldId}\") ? (Guid?)null : Guid.Parse(\"{tree.NodeWeightFieldId}\");\n" +
			$"\tcreateTreeInput.NodeNameFieldId = string.IsNullOrEmpty(\"{tree.NodeLabelFieldId}\") ? (Guid?)null : Guid.Parse(\"{tree.NodeLabelFieldId}\");\n" +
			$"\tcreateTreeInput.NodeLabelFieldId = string.IsNullOrEmpty(\"{tree.NodeLabelFieldId}\") ? (Guid?)null : Guid.Parse(\"{tree.NodeLabelFieldId}\");\n" +
			$"\tcreateTreeInput.NodeWeightFieldId = string.IsNullOrEmpty(\"{tree.NodeWeightFieldId}\") ? (Guid?)null : Guid.Parse(\"{tree.NodeWeightFieldId}\");\n" +
			$"\tcreateTreeInput.RootNodes = new List<RecordTreeNode>();\n";
			foreach (var recordId in tree.RootNodes)
			{
				response += CreateTreeRootNodeCode(recordId, tree, entityId, entityName);
			}
			response += $"\tcreateTreeInput.NodeObjectProperties = new List<Guid>();\n";
			foreach (var propertyId in tree.NodeObjectProperties)
			{
				response += $"\tcreateTreeInput.NodeObjectProperties.Add(new Guid(\"{propertyId}\"));\n";
			}
			response +=
			"\t{\n" +
				$"\t\tvar response = entMan.CreateRecordTree(new Guid(\"{entityId}\"), createTreeInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {entityName} Updated List: list_name Message:\" + response.Message);\n" +
			"\t}\n" +
			"}\n" +
			"#endregion\n\n";
			return response;
		}

		//RootNode
		private string CreateTreeRootNodeCode(Guid recordId, DbRecordTree tree, Guid entityId, string entityName)
		{
			var response = string.Empty;
			var currentEntity = entMan.ReadEntity(entityId).Object;
			EntityRecord record = null;
			EntityQuery query = new EntityQuery("entityName", "*", EntityQuery.QueryEQ("id", recordId), null, null, null);
			var result = recMan.Find(query);
			if (result.Success && result.Object.Data.Any())
			{
				record = result.Object.Data[0];
			}

			if (record == null)
			{
				return $"\t\t//Cannot find record for root node recordId {recordId} in entity: {entityName}. Skiping the addition of this root node.\n";
			}

			var nodeId = record[currentEntity.Fields.Single(x => x.Id == tree.NodeIdFieldId).Name];
			var nodeName = record[currentEntity.Fields.Single(x => x.Id == tree.NodeNameFieldId).Name];
			var nodeLabel = record[currentEntity.Fields.Single(x => x.Id == tree.NodeLabelFieldId).Name]; ;
			var nodeParentId = record[currentEntity.Fields.Single(x => x.Id == tree.NodeParentIdFieldId).Name];
			var nodeWeight = record[currentEntity.Fields.Single(x => x.Id == tree.NodeWeightFieldId).Name];
			var nodeRecordId = recordId;

			response +=
			$"\t\t#region << root node: {nodeName} >>\n" +
			"\t\t{\n" +
			"\t\t\tvar treeNode = new RecordTreeNode();\n" +
			$"\t\t\ttreeNode.Id = \"{nodeId}\";\n" +
			$"\t\t\ttreeNode.Name = \"{nodeName}\";\n" +
			$"\t\t\ttreeNode.Label = \"{nodeLabel}\";\n" +
			$"\t\t\ttreeNode.Weight = \"{nodeWeight}\";\n" +
			$"\t\t\ttreeNode.ParentId =  new Guid(\"{nodeParentId}\");\n" +
			$"\t\t\ttreeNode.RecordId =  new Guid(\"{nodeRecordId}\");\n" +
			$"\t\t\tcreateTreeInput.RootNodes.Add(treeNode);\n" +
			"\t\t}\n" +
			"\t\t#endregion\n\n";
			return response;
		}

		//Delete
		private string DeleteTreeCode(DbRecordTree tree, Guid entityId, string entityName)
		{
			var response =
		$"#region << Delete  Enity: {entityName} tree: {tree.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = entMan.DeleteRecordTree(new Guid(\"{entityId}\"),new Guid(\"{tree.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Tree: {tree.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		//Update
		private UpdateCheckResponse UpdateTreeCode(DbRecordTree currentTree, DbRecordTree oldTree, DbEntity currentEntity)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentTree.Label != null)
				currentTree.Label = currentTree.Label.Replace("\"", "\\\"");
			//escape the old for update check sake
			if (oldTree.Label != null)
				oldTree.Label = oldTree.Label.Replace("\"", "\\\"");
			#region << Code >>
			code +=
		   $"#region << Tree  Enity: {currentEntity.Name} name: {currentTree.Name} >>\n" +
		   "{\n" +
			$"\tvar createTreeEntity = entMan.ReadEntity(new Guid(\"{currentEntity.Id}\")).Object;\n" +
			"\tvar updateTreeInput = new InputRecordTree();\n" +
			$"\tupdateTreeInput.Id = createTreeEntity.RecordTrees.SingleOrDefault(x => x.Name == \"{currentTree.Name}\").Id;\n" +
			$"\tupdateTreeInput.Name = \"{currentTree.Name}\";\n" +
			$"\tupdateTreeInput.Label = \"{currentTree.Label}\";\n" +
			$"\tupdateTreeInput.Default = {(currentTree.Default).ToString().ToLowerInvariant()};\n" +
			$"\tupdateTreeInput.System = {(currentTree.System).ToString().ToLowerInvariant()};\n" +
			$"\tupdateTreeInput.CssClass = string.IsNullOrEmpty(\"{currentTree.CssClass}\") ? null : \"{currentTree.CssClass}\";\n" +
			$"\tupdateTreeInput.IconName = string.IsNullOrEmpty(\"{currentTree.IconName}\") ? null : \"{currentTree.IconName}\";\n" +
			$"\tupdateTreeInput.RelationId = new Guid(\"{currentTree.RelationId}\");\n" +
			$"\tupdateTreeInput.DepthLimit = string.IsNullOrEmpty(\"{currentTree.DepthLimit}\") ? (int?)null : Int32.Parse(\"{currentTree.DepthLimit}\");\n" +
			$"\tupdateTreeInput.NodeIdFieldId = string.IsNullOrEmpty(\"{currentTree.NodeIdFieldId}\") ? (Guid?)null : Guid.Parse(\"{currentTree.NodeIdFieldId}\");\n" +
			$"\tupdateTreeInput.NodeParentIdFieldId = string.IsNullOrEmpty(\"{currentTree.NodeWeightFieldId}\") ? (Guid?)null : Guid.Parse(\"{currentTree.NodeWeightFieldId}\");\n" +
			$"\tupdateTreeInput.NodeNameFieldId = string.IsNullOrEmpty(\"{currentTree.NodeLabelFieldId}\") ? (Guid?)null : Guid.Parse(\"{currentTree.NodeLabelFieldId}\");\n" +
			$"\tupdateTreeInput.NodeLabelFieldId = string.IsNullOrEmpty(\"{currentTree.NodeLabelFieldId}\") ? (Guid?)null : Guid.Parse(\"{currentTree.NodeLabelFieldId}\");\n" +
			$"\tupdateTreeInput.NodeWeightFieldId = string.IsNullOrEmpty(\"{currentTree.NodeWeightFieldId}\") ? (Guid?)null : Guid.Parse(\"{currentTree.NodeWeightFieldId}\");\n" +
			$"\tupdateTreeInput.RootNodes = new List<RecordTreeNode>();\n";
			foreach (var recordId in currentTree.RootNodes)
			{
				code += CreateTreeRootNodeCode(recordId, currentTree, currentEntity.Id, currentEntity.Name);
			}
			code += $"\tupdateTreeInput.NodeObjectProperties = new List<Guid>();\n";
			foreach (var propertyId in currentTree.NodeObjectProperties)
			{
				code += $"\tupdateTreeInput.NodeObjectProperties.Add(new Guid(\"{propertyId}\"));\n";
			}
			code +=
			"\t{\n" +
				$"\t\tvar response = entMan.UpdateRecordTree(new Guid(\"{currentEntity.Id}\"), updateTreeInput);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Entity: {currentEntity.Name} Updated Tree: {oldTree.Name} Message:\" + response.Message);\n" +
			"\t}\n" +
			"}\n" +
			"#endregion\n\n";

			#endregion


			#region << Changes >>
			if (currentTree.Name != oldTree.Name)
			{
				hasUpdate = true;
			}
			if (currentTree.Label != oldTree.Label)
			{
				hasUpdate = true;
			}
			if (currentTree.Default != oldTree.Default)
			{
				hasUpdate = true;
			}
			if (currentTree.System != oldTree.System)
			{
				hasUpdate = true;
			}
			if (currentTree.CssClass != oldTree.CssClass)
			{
				hasUpdate = true;
			}
			if (currentTree.IconName != oldTree.IconName)
			{
				hasUpdate = true;
			}
			if (currentTree.RelationId != oldTree.RelationId)
			{
				hasUpdate = true;
			}
			if (currentTree.DepthLimit != oldTree.DepthLimit)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeIdFieldId != oldTree.NodeIdFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeParentIdFieldId != oldTree.NodeParentIdFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeNameFieldId != oldTree.NodeNameFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeLabelFieldId != oldTree.NodeLabelFieldId)
			{
				hasUpdate = true;
			}
			if (currentTree.NodeWeightFieldId != oldTree.NodeWeightFieldId)
			{
				hasUpdate = true;
			}

			if (JsonConvert.SerializeObject(currentTree.RootNodes) != JsonConvert.SerializeObject(oldTree.RootNodes))
			{
				hasUpdate = true;
			}
			if (JsonConvert.SerializeObject(currentTree.NodeObjectProperties) != JsonConvert.SerializeObject(oldTree.NodeObjectProperties))
			{
				hasUpdate = true;
			}
			#endregion

			response.Code = code;
			response.HasUpdate = hasUpdate;
			response.ChangeList.Add($"<span class='go-green label-block'>tree</span>  with name <span class='go-red'>{oldTree.Name}</span> was updated");
			return response;
		}
		#endregion

		#region << Relations >>
		private string CreateRelationCode(DbEntityRelation relationRecord)
		{
			var originEntity = entMan.ReadEntity(relationRecord.OriginEntityId).Object;
			var originField = originEntity.Fields.Single(x => x.Id == relationRecord.OriginFieldId);
			var targetEntity = entMan.ReadEntity(relationRecord.TargetEntityId).Object;
			var targetField = targetEntity.Fields.Single(x => x.Id == relationRecord.TargetFieldId);

			//escape possible double quotes
			if (relationRecord.Label != null)
				relationRecord.Label = relationRecord.Label.Replace("\"", "\\\"");
			if (relationRecord.Description != null)
				relationRecord.Description = relationRecord.Description.Replace("\"", "\\\"");

			var response =
			$"#region << Create relation: {relationRecord.Name} >>\n" +
			"{\n" +
				"\tvar relation = new EntityRelation();\n" +
				$"\tvar originEntity = entMan.ReadEntity(new Guid(\"{originEntity.Id}\")).Object;\n" +
				$"\tvar originField = originEntity.Fields.SingleOrDefault(x => x.Name == \"originField.Name\");\n" +
				$"\tvar targetEntity = entMan.ReadEntity(new Guid(\"{targetEntity.Id}\")).Object;\n" +
				$"\tvar targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == \"targetField.Name\");\n" +
				$"\trelation.Id = new Guid(\"{relationRecord.Id}\");\n" +
				$"\trelation.Name =  \"{relationRecord.Name}\";\n" +
				$"\trelation.Label = \"{relationRecord.Label}\";\n" +
				$"\trelation.Description = \"{relationRecord.Description}\";\n" +
				$"\trelation.System =  {(relationRecord.System).ToString().ToLowerInvariant()};\n";
			if (relationRecord.RelationType == EntityRelationType.OneToOne)
			{
				response += $"\trelation.RelationType = EntityRelationType.OneToOne;\n";
			}
			else if (relationRecord.RelationType == EntityRelationType.OneToMany)
			{
				response += $"\trelation.RelationType = EntityRelationType.OneToMany;\n";
			}
			else if (relationRecord.RelationType == EntityRelationType.ManyToMany)
			{
				response += $"\trelation.RelationType = EntityRelationType.ManyToMany;\n";
			}
			response +=
			$"\trelation.OriginEntityId = originEntity.Id;\n" +
			$"\trelation.OriginEntityName = originEntity.Name;\n" +
			$"\trelation.OriginFieldId = originField.Id;\n" +
			$"\trelation.OriginFieldName = originField.Name;\n" +
			$"\trelation.TargetEntityId = targetEntity.Id;\n" +
			$"\trelation.TargetEntityName = targetEntity.Name;\n" +
			$"\trelation.TargetFieldId = targetField.Id;\n" +
			$"\trelation.TargetFieldName = targetField.Name;\n" +
			"\t{\n" +
				$"\t\tvar response = relMan.Create(relation);\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Relation: {relationRecord.Name} Create. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			if (targetField.Name == "created_by" || targetField.Name == "last_modified_by")
			{
				return string.Empty;
			}

			return response;
		}

		private string DeleteRelationCode(DbEntityRelation relationRecord)
		{
			var response =
		$"#region << Delete relation: {relationRecord.Name} >>\n" +
		"{\n" +
			"\t{\n" +
				$"\t\tvar response = relMan.Delete(new Guid(\"{relationRecord.Id}\"));\n" +
				$"\t\tif (!response.Success)\n" +
					$"\t\t\tthrow new Exception(\"System error 10060. Relation: {relationRecord.Name} Delete. Message:\" + response.Message);\n" +
			"\t}\n" +

		"}\n" +
		"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateRelationCode(DbEntityRelation currentRelation, DbEntityRelation oldRelation)
		{
			var response = new UpdateCheckResponse();
			var originEntity = entMan.ReadEntity(currentRelation.OriginEntityId).Object;
			var originField = originEntity.Fields.Single(x => x.Id == currentRelation.OriginFieldId);
			var targetEntity = entMan.ReadEntity(currentRelation.TargetEntityId).Object;
			var targetField = targetEntity.Fields.Single(x => x.Id == currentRelation.TargetFieldId);
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentRelation.Label != null)
				currentRelation.Label = currentRelation.Label.Replace("\"", "\\\"");
			if (currentRelation.Description != null)
				currentRelation.Description = currentRelation.Description.Replace("\"", "\\\"");
			//escape the old to for update check sake
			if (oldRelation.Label != null)
				oldRelation.Label = oldRelation.Label.Replace("\"", "\\\"");
			if (oldRelation.Description != null)
				oldRelation.Description = oldRelation.Description.Replace("\"", "\\\"");

			code = $"#region << Update relation: {currentRelation.Name} >>\n" +
			"{\n" +
				"\tvar relation = new EntityRelation();\n" +
				$"\tvar originEntity = entMan.ReadEntity(new Guid(\"{originEntity.Id}\")).Object;\n" +
				$"\tvar originField = originEntity.Fields.SingleOrDefault(x => x.Name == \"{originField.Name}\");\n" +
				$"\tvar targetEntity = entMan.ReadEntity(new Guid(\"{targetEntity.Id}\")).Object;\n" +
				$"\tvar targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == \"{targetField.Name}\");\n" +
				$"\trelation.Id = new Guid(\"{currentRelation.Id}\");\n";

			//name
			if (currentRelation.Name != oldRelation.Name)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{oldRelation.Name}</span> to <span class='go-red'>{currentRelation.Name}</span>");
			}
			code += $"\trelation.Name = \"{currentRelation.Name}\";\n";

			//label
			if (currentRelation.Label != oldRelation.Label)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>label</span>  from <span class='go-red'>{oldRelation.Label}</span> to <span class='go-red'>{currentRelation.Label}</span>");
			}

			code += $"\trelation.Label = \"{currentRelation.Label}\";\n";

			//description
			if (currentRelation.Description != oldRelation.Description)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>description</span>  from <span class='go-red'>{oldRelation.Description}</span> to <span class='go-red'>{currentRelation.Description}</span>");
			}
			code += $"\trelation.Description = \"{currentRelation.Description}\";\n";

			//system
			if (currentRelation.System != oldRelation.System)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>system</span>  from <span class='go-red'>{(oldRelation.System).ToString().ToLowerInvariant()}</span> to <span class='go-red'>{(currentRelation.System).ToString().ToLowerInvariant()}</span>");
			}
			code += $"\trelation.System = {(currentRelation.System).ToString().ToLowerInvariant()};\n";

			//relation type
			if (currentRelation.RelationType != oldRelation.RelationType)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>relationType</span>  from <span class='go-red'>{oldRelation.RelationType}</span> to <span class='go-red'>{currentRelation.RelationType}</span>");
			}
			if (currentRelation.RelationType == EntityRelationType.OneToOne)
			{
				code += $"\trelation.RelationType = EntityRelationType.OneToOne;\n";
			}
			else if (currentRelation.RelationType == EntityRelationType.OneToMany)
			{
				code += $"\trelation.RelationType = EntityRelationType.OneToMany;\n";
			}
			else if (currentRelation.RelationType == EntityRelationType.ManyToMany)
			{
				code += $"\trelation.RelationType = EntityRelationType.ManyToMany;\n";
			}


			//originEntityId
			if (currentRelation.OriginEntityId != oldRelation.OriginEntityId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>OriginEntityId</span>  from <span class='go-red'>{oldRelation.OriginEntityId}</span> to <span class='go-red'>{currentRelation.OriginEntityId}</span>");
			}
			code += $"\trelation.OriginEntityId = originEntity.Id;\n";

			//OriginFieldId
			if (currentRelation.OriginFieldId != oldRelation.OriginFieldId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>OriginFieldId</span>  from <span class='go-red'>{oldRelation.OriginFieldId}</span> to <span class='go-red'>{currentRelation.OriginFieldId}</span>");
			}
			code += $"\trelation.OriginFieldId = originField.Id;\n";

			//TargetEntityId
			if (currentRelation.TargetEntityId != oldRelation.TargetEntityId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>TargetEntityId</span>  from <span class='go-red'>{oldRelation.TargetEntityId}</span> to <span class='go-red'>{currentRelation.TargetEntityId}</span>");
			}
			code += $"\trelation.TargetEntityId = targetEntity.Id;\n";

			//TargetFieldId
			if (currentRelation.TargetFieldId != oldRelation.TargetFieldId)
			{
				hasUpdate = true;
				response.ChangeList.Add($"<span class='go-green label-block'>TargetFieldId</span>  from <span class='go-red'>{oldRelation.TargetFieldId}</span> to <span class='go-red'>{currentRelation.TargetFieldId}</span>");
			}
			code += $"\trelation.TargetFieldId = targetField.Id;\n";
			code +=
			"\t{\n" +
				$"\tvar response = relMan.Update(relation);\n" +
				$"\tif (!response.Success)\n" +
					$"\t\tthrow new Exception(\"System error 10060. Failed update relation: {currentRelation.Name}. Message:\" + response.Message);\n" +
			"\t}\n" +


			"}\n" +
			"#endregion\n\n";


			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;

		}
		#endregion

		#region << Role >>
		private string CreateRoleCode(EntityRecord role)
		{

			//escape possible double quotes
			if (role["description"] != null)
				role["description"] = ((string)role["description"]).Replace("\"", "\\\"");

			var response = "" +
$"#region << Create role: {(string)role["name"]} >>\n" +
"{\n" +
	"\tvar role = new EntityRecord();\n" +
	$"\trole[\"id\"] = new Guid(\"{(Guid)role["id"]}\");\n" +
	$"\trole[\"name\"] = \"{(string)role["name"]}\";\n" +
	$"\trole[\"description\"] = \"{(string)role["description"]}\";\n" +
	"\tvar createRoleResult = recMan.CreateRecord(\"role\", role);\n" +
	"\tif (!createRoleResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Role create with name : {(string)role["name"]}. Message:\" + createRoleResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private string DeleteRoleCode(EntityRecord role)
		{
			var response = "" +
$"#region << Delete role: {(string)role["name"]} >>\n" +
"{\n" +
	$"\tvar deleteRoleResult = recMan.DeleteRecord(\"role\", new Guid(\"{(Guid)role["id"]}\"));\n" +
	"\tif (!deleteRoleResult.Success)\n" +
	"\t{\n" +
		$"\t\tthrow new Exception(\"System error 10060. Role delete with name : {(string)role["name"]}. Message:\" + deleteRoleResult.Message);\n" +
	"\t}\n" +
"}\n" +
"#endregion\n\n";

			return response;
		}

		private UpdateCheckResponse UpdateRoleCode(EntityRecord currentRole, EntityRecord oldRole)
		{
			var response = new UpdateCheckResponse();
			string code = string.Empty;
			var hasUpdate = false;
			//escape possible double quotes
			if (currentRole["description"] != null)
				currentRole["description"] = ((string)currentRole["description"]).Replace("\"", "\\\"");
			//escape the for update check sake
			if (oldRole["description"] != null)
				oldRole["description"] = ((string)oldRole["description"]).Replace("\"", "\\\"");

			code =
$"#region << Update role: {(string)currentRole["name"]} >>\n" +
"{\n" +
	"\tvar patchObject = new EntityRecord();\n" +
	$"\tpatchObject[\"id\"] = new Guid(\"{(Guid)currentRole["id"]}\");\n";

			//name
			if ((string)currentRole["name"] != (string)oldRole["name"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"name\"] = \"{(string)currentRole["name"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>name</span>  from <span class='go-red'>{(string)oldRole["name"]}</span> to <span class='go-red'>{(string)currentRole["name"]}</span>");
			}
			//label	
			if ((string)currentRole["description"] != (string)oldRole["description"])
			{
				hasUpdate = true;
				code += $"\tpatchObject[\"description\"] = \"{(string)currentRole["description"]}\";\n";
				response.ChangeList.Add($"<span class='go-green label-block'>description</span> from <span class='go-red'>{(string)oldRole["description"]}</span> to <span class='go-red'>{(string)currentRole["description"]}</span>");
			}

			code +=
				"\tvar updateRoleResult = recMan.UpdateRecord(\"role\", patchObject);\n" +
				"\tif (!updateRoleResult.Success)\n" +
				"\t{\n" +
					$"\t\tthrow new Exception(\"System error 10060. Role update with name : {(string)currentRole["name"]}. Message:\" + updateRoleResult.Message);\n" +
				"\t}\n" +
			"}\n" +
			"#endregion\n\n";


			response.Code = code;
			response.HasUpdate = hasUpdate;
			return response;

		}
		#endregion
		#endregion

	}
}

