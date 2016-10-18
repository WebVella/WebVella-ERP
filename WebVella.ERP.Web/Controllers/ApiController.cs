using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebVella.ERP.Api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Database;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Web.Security;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using CsvHelper;
using Microsoft.AspNetCore.StaticFiles;
using WebVella.ERP.Utilities;
using System.Dynamic;
using WebVella.ERP.Plugins;
using WebVella.ERP.WebHooks;
using System.Diagnostics;
using Npgsql;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using ImageProcessor;
using Microsoft.Extensions.Primitives;
using ImageProcessor.Imaging;
using System.Drawing;
using Newtonsoft.Json.Converters;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebVella.ERP.Web.Controllers
{
	public partial class ApiController : ApiControllerBase
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
		public IActionResult GetEntityMetaList(string hash = null)
		{
			var bo = entMan.ReadEntities();

			//check hash and clear data if hash match
			if (bo.Success && bo.Object != null && !string.IsNullOrWhiteSpace(hash) && bo.Hash == hash)
				bo.Object = null;

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
			var entity = new InputEntity();
			entity.Name = submitObj.Name;
			entity.Label = submitObj.Label;
			entity.LabelPlural = submitObj.LabelPlural;
			entity.System = submitObj.System;
			entity.IconName = submitObj.IconName;
			entity.Weight = submitObj.Weight;
			entity.RecordPermissions = submitObj.RecordPermissions;

			return DoResponse(entMan.CreateEntity(entity, submitObj.CreateViews, submitObj.CreateLists));
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
		public IActionResult GetEntityRelationMetaList(string hash = null)
		{
			var response = new EntityRelationManager().Read();

			//check hash and clear data if hash match
			if (response.Success && response.Object != null && !string.IsNullOrWhiteSpace(hash) && response.Hash == hash)
				response.Object = null;

			return DoResponse(response);
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

		// Update an entity record relation records for origin record
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
                hookFilterObj.direction = "origin-target";
                hookFilterObj.relation = relation;
				hookFilterObj.originEntity = originEntity;
				hookFilterObj.targetEntity = targetEntity;
				hookFilterObj.controller = this;
				hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationInput, originEntity.Name, hookFilterObj);
				model = hookFilterObj.record;

				//Hook for the target entity
				hookFilterObj = new ExpandoObject();
				hookFilterObj.record = model;
                hookFilterObj.direction = "origin-target";
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
					response.Errors.Add(new ErrorModel { Message = "Attach target record was not found. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				else if (attachTargetRecords.Any(x => (Guid)x["id"] == targetId))
				{
					response.Errors.Add(new ErrorModel { Message = "Attach target id was duplicated. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
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
					response.Errors.Add(new ErrorModel { Message = "Detach target record was not found. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				else if (detachTargetRecords.Any(x => (Guid)x["id"] == targetId))
				{
					response.Errors.Add(new ErrorModel { Message = "Detach target id was duplicated. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
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
                    hookFilterObj.direction = "origin-target";
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
                    hookFilterObj.direction = "origin-target";
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
				dynamic hookActionObj = new ExpandoObject();
				hookActionObj.record = model;
				hookActionObj.result = result;
				hookActionObj.relation = relation;
				hookActionObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.ManageRelationAction, originEntity.Name, hookActionObj);
				hookActionObj = new ExpandoObject();
				hookActionObj.record = model;
				hookActionObj.result = result;
				hookActionObj.relation = relation;
				hookActionObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.ManageRelationAction, targetEntity.Name, hookActionObj);
			}
			catch (Exception ex)
			{
				return Json(CreateErrorResponse("Plugin error in web hook create_record_success_action: " + ex.Message));
			}// <<<		

			return DoResponse(response);
		}


        // Update an entity record relation records for target record
        // POST: api/v1/en_US/record/relation/reverse
        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/relation/reverse")]
        public IActionResult UpdateEntityRelationRecordReverse([FromBody]InputEntityRelationRecordReverseUpdateModel model)
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

            if (model.DetachOriginFieldRecordIds != null && model.DetachOriginFieldRecordIds.Any() && originField.Required && relation.RelationType != EntityRelationType.ManyToMany)
            {
                response.Errors.Add(new ErrorModel { Message = "Cannot detach records, when origin field is required.", Key = "originFieldRecordId" });
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
                hookFilterObj.direction = "target-origin";
                hookFilterObj.relation = relation;
                hookFilterObj.originEntity = originEntity;
                hookFilterObj.targetEntity = targetEntity;
                hookFilterObj.controller = this;
                hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationInput, originEntity.Name, hookFilterObj);
                model = hookFilterObj.record;

                //Hook for the target entity
                hookFilterObj = new ExpandoObject();
                hookFilterObj.record = model;
                hookFilterObj.direction = "target-origin";
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


            EntityQuery query = new EntityQuery(originEntity.Name, "id," + targetField.Name, EntityQuery.QueryEQ("id", model.TargetFieldRecordId), null, null, null);
            QueryResponse result = recMan.Find(query);
            if (result.Object.Data.Count == 0)
            {
                response.Errors.Add(new ErrorModel { Message = "Target record was not found. Id=[" + model.TargetFieldRecordId + "]", Key = "targetFieldRecordId" });
                response.Success = false;
                return DoResponse(response);
            }

            var targetRecord = result.Object.Data[0];
            object targetValue = targetRecord[targetField.Name];

            var attachOriginRecords = new List<EntityRecord>();
            var detachOriginRecords = new List<EntityRecord>();

            foreach (var originId in model.AttachOriginFieldRecordIds)
            {
                query = new EntityQuery(originEntity.Name, "id," + originField.Name, EntityQuery.QueryEQ("id", originId), null, null, null);
                result = recMan.Find(query);
                if (result.Object.Data.Count == 0)
                {
                    response.Errors.Add(new ErrorModel { Message = "Attach origin record was not found. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                else if (attachOriginRecords.Any(x => (Guid)x["id"] == originId))
                {
                    response.Errors.Add(new ErrorModel { Message = "Attach origin id was duplicated. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                attachOriginRecords.Add(result.Object.Data[0]);
            }

            foreach (var originId in model.DetachOriginFieldRecordIds)
            {
                query = new EntityQuery(originEntity.Name, "id," + originField.Name, EntityQuery.QueryEQ("id", originId), null, null, null);
                result = recMan.Find(query);
                if (result.Object.Data.Count == 0)
                {
                    response.Errors.Add(new ErrorModel { Message = "Detach origin record was not found. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                else if (detachOriginRecords.Any(x => (Guid)x["id"] == originId))
                {
                    response.Errors.Add(new ErrorModel { Message = "Detach origin id was duplicated. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
                    response.Success = false;
                    return DoResponse(response);
                }
                detachOriginRecords.Add(result.Object.Data[0]);
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
                    hookFilterObj.direction = "target-origin";
                    hookFilterObj.attachOriginRecords = attachOriginRecords;
                    hookFilterObj.detachOriginRecords = detachOriginRecords;
                    hookFilterObj.targetRecord = targetRecord;
                    hookFilterObj.originEntity = originEntity;
                    hookFilterObj.targetEntity = targetEntity;
                    hookFilterObj.relation = relation;
                    hookFilterObj.controller = this;
                    hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationPreSave, originEntity.Name, hookFilterObj);
                    attachOriginRecords = hookFilterObj.attachOriginRecords;
                    detachOriginRecords = hookFilterObj.detachOriginRecords;

                    //Hook for the target entity
                    hookFilterObj = new ExpandoObject();
                    hookFilterObj.direction = "target-origin";
                    hookFilterObj.attachOriginRecords = attachOriginRecords;
                    hookFilterObj.detachOriginRecords = detachOriginRecords;
                    hookFilterObj.targetRecord = targetRecord;
                    hookFilterObj.originEntity = originEntity;
                    hookFilterObj.targetEntity = targetEntity;
                    hookFilterObj.relation = relation;
                    hookFilterObj.controller = this;
                    hookFilterObj = hooksService.ProcessFilters(SystemWebHookNames.ManageRelationPreSave, targetEntity.Name, hookFilterObj);
                    attachOriginRecords = hookFilterObj.attachOriginRecords;
                    detachOriginRecords = hookFilterObj.detachOriginRecords;
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
                                foreach (var record in detachOriginRecords)
                                {
                                    record[originField.Name] = null;

                                    var updResult = recMan.UpdateRecord(originEntity, record);
                                    if (!updResult.Success)
                                    {
                                        connection.RollbackTransaction();
                                        response.Errors = updResult.Errors;
                                        response.Message = "Origin record id=[" + record["id"] + "] detach operation failed.";
                                        response.Success = false;
                                        return DoResponse(response);
                                    }
                                }

                                foreach (var record in attachOriginRecords)
                                {
                                    var patchObject = new EntityRecord();
                                    patchObject["id"] = (Guid)record["id"];
                                    patchObject[originField.Name] = targetValue;

                                    var updResult = recMan.UpdateRecord(originEntity, patchObject);
                                    if (!updResult.Success)
                                    {
                                        connection.RollbackTransaction();
                                        response.Errors = updResult.Errors;
                                        response.Message = "Origin record id=[" + record["id"] + "] attach operation failed.";
                                        response.Success = false;
                                        return DoResponse(response);
                                    }
                                }
                            }
                            break;
                        case EntityRelationType.ManyToMany:
                            {
                                foreach (var record in detachOriginRecords)
                                {
                                    QueryResponse updResult = recMan.RemoveRelationManyToManyRecord(relation.Id, (Guid)record[originField.Name], (Guid)targetValue );

                                    if (!updResult.Success)
                                    {
                                        connection.RollbackTransaction();
                                        response.Errors = updResult.Errors;
                                        response.Message = "Origin record id=[" + record["id"] + "] detach operation failed.";
                                        response.Success = false;
                                        return DoResponse(response);
                                    }
                                }

                                foreach (var record in attachOriginRecords)
                                {
                                    QueryResponse updResult = recMan.CreateRelationManyToManyRecord(relation.Id, (Guid)record[originField.Name], (Guid)targetValue);

                                    if (!updResult.Success)
                                    {
                                        connection.RollbackTransaction();
                                        response.Errors = updResult.Errors;
                                        response.Message = "Origin record id=[" + record["id"] + "] attach  operation failed.";
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
                dynamic hookActionObj = new ExpandoObject();
                hookActionObj.record = model;
                hookActionObj.result = result;
                hookActionObj.relation = relation;
                hookActionObj.controller = this;
                hooksService.ProcessActions(SystemWebHookNames.ManageRelationAction, originEntity.Name, hookActionObj);
                hookActionObj = new ExpandoObject();
                hookActionObj.record = model;
                hookActionObj.result = result;
                hookActionObj.relation = relation;
                hookActionObj.controller = this;
                hooksService.ProcessActions(SystemWebHookNames.ManageRelationAction, targetEntity.Name, hookActionObj);
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
				dynamic hookActionObj = new ExpandoObject();
				hookActionObj.recordId = recordId;
				hookActionObj.result = result;
				hookActionObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.GetRecordAction, entityName, hookActionObj);
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
				dynamic hookActionObj = new ExpandoObject();
				hookActionObj.recordId = recordId;
				hookActionObj.result = result;
				hookActionObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.DeleteRecordAction, entityName, hookActionObj);
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
						hookFilterObj.recordId = (Guid)postObj["id"];
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
				dynamic hookActionObj = new ExpandoObject();
				hookActionObj.record = postObj;
				hookActionObj.result = result;
				hookActionObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.CreateRecordAction, entityName, hookActionObj);
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
			if (!postObj.Properties.ContainsKey("id"))
			{
				postObj["id"] = recordId;
			}

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
						hookFilterObj.recordId = (Guid)postObj["id"];
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
				dynamic hookActionObj = new ExpandoObject();
				hookActionObj.record = postObj;
				hookActionObj.oldRecord = postObj;
				hookActionObj.result = result;
				hookActionObj.recordId = recordId;
				hookActionObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.UpdateRecordAction, entityName, hookActionObj);
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
				dynamic hookActionObj = new ExpandoObject();
				hookActionObj.record = postObj;
				hookActionObj.result = result;
				hookActionObj.recordId = recordId;
				hookActionObj.controller = this;
				hooksService.ProcessActions(SystemWebHookNames.PatchRecordAction, entityName, hookActionObj);
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
		public IActionResult GetRecordListByEntityName(string entityName, string listName, int page, int? pageSize = null,
				Guid? relationId = null, Guid? relatedRecordId = null, string direction = "origin-target")
		{

			EntityListResponse entitiesResponse = entMan.ReadEntities();
			List<Entity> entities = entitiesResponse.Object;

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

			EntityRelation relation = null;
			if (relationId != null)
			{
				relation = relMan.Read().Object.SingleOrDefault(r => r.Id == relationId);
				if (relation == null)
				{
					response.Success = false;
					response.Message = "The provided relationId is not of any existing relation";
					return DoResponse(response);
				}
				if (relation != null && relatedRecordId == null)
				{
					response.Success = false;
					response.Message = "The Id of the relation record is required when a relation is submitted";
					return DoResponse(response);
				}
			}

			try
			{
				QueryObject queryObj = null;
				/*if (Request.Query.Count > 0)
				{
					List<QueryObject> queryObjList = new List<QueryObject>();

					RecordList listMeta = entity.RecordLists.FirstOrDefault(l => l.Name == listName);
					if (listMeta != null)
					{
						foreach (var query in Request.Query)
						{
							if (listMeta.Columns.Any(c => c.DataName == query.Key))
							{
								queryObjList.Add(EntityQuery.QueryContains(query.Key, query.Value));
							}
						}
					}

					if (queryObjList.Count == 1)
						queryObj = queryObjList[0];
					else if (queryObjList.Count > 1)
						queryObj = EntityQuery.QueryAND(queryObjList.ToArray());
				}*/

				if (relation == null)
				{
					response.Object = GetListRecords(entities, entity, listName, page, queryObj, pageSize);
				}
				else
				{
					response.Object = GetListRecords(entities, entity, listName, page, queryObj, pageSize, false, relation, relatedRecordId, direction);
				}

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

		private List<EntityRecord> GetListRecords(List<Entity> entities, Entity entity, string listName, int? page = null, QueryObject queryObj = null,
					int? pageSize = null, bool export = false, EntityRelation auxRelation = null, Guid? auxRelatedRecordId = null,
					string auxRelationDirection = "origin-target")
		{
			List<KeyValuePair<string, string>> queryStringOverwriteParameters = new List<KeyValuePair<string, string>>();
			foreach (var key in Request.Query.Keys)
				queryStringOverwriteParameters.Add(new KeyValuePair<string, string>(key, Request.Query[key]));

			return recMan.GetListRecords(entities, entity, listName, page, queryObj, pageSize, export, auxRelation,
				auxRelatedRecordId, auxRelationDirection, queryStringOverwriteParameters);
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/view/{viewName}/{id}")]
		public IActionResult GetRecordWithView(string entityName, string viewName, Guid id)
		{
			EntityListResponse entitiesResponse = entMan.ReadEntities();
			List<Entity> entities = entitiesResponse.Object;

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

			response.Object = recMan.GetViewRecords(entities, entity, viewName, "id", id);
			return DoResponse(response);
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/record/{entityName}/tree/{treeName}")]
		public IActionResult GetTreeRecords(string entityName, string treeName)
		{
			List<Entity> entities = entMan.ReadEntities().Object;

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
				response.Object.Data = Helpers.GetTreeRecords(entities, relationList, tree);
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
		[AcceptVerbs(new[] { "GET", "POST" }, Route = "api/v1/en_US/record/{entityName}/list/{listName}/export")]
		public IActionResult ExportListRecordsToCsv(string entityName, string listName, int count = 10)
		{
			var random = new Random().Next(10, 99);
			DateTime dt = DateTime.Now;
			string time = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();
			string fileName = $"{entityName.Replace('_', '-').Trim().ToLowerInvariant()}-{time}{random}.csv"; //"goro-test-report.csv";

			Response.ContentType = "application/octet-stream;charset=utf-8";
			Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
			//Response.Headers.Add("Content-Length", fileResp.ContentLength.ToString());

			Dictionary<string, string> urlQueries = new Dictionary<string, string>();
			if (Request.Query.Count > 0)
			{
				foreach (var query in Request.Query)
				{
					if (query.Key != "count")
						urlQueries.Add(query.Key, query.Value);
				}
			}

			ImportExportManager ieManager = new ImportExportManager(this.hooksService);
			ResponseModel response = ieManager.ExportListRecordsToCsv(entityName, listName, Response.Body, urlQueries, count);

			Response.Body.Close();

			if (!response.Success)
				return DoResponse(response);

			return new EmptyResult();
		}

		// Import list records to csv
		// POST: api/v1/en_US/record/{entityName}/list/{listName}/import
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/{entityName}/import")]
		public IActionResult ImportEntityRecordsFromCsv(string entityName, [FromBody]JObject postObject)
		{
			string fileTempPath = "";

			if (!postObject.IsNullOrEmpty() && postObject.Properties().Any(p => p.Name == "fileTempPath"))
			{
				fileTempPath = postObject["fileTempPath"].ToString();
			}

			ImportExportManager ieManager = new ImportExportManager(this.hooksService);
			ResponseModel response = ieManager.ImportEntityRecordsFromCsv(entityName, fileTempPath);

			return DoResponse(response);

		}


		// Import list records to csv
		// POST: api/v1/en_US/record/{entityName}/list/{listName}/import
		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/record/{entityName}/import-evaluate")]
		public IActionResult EvaluateImportEntityRecordsFromCsv(string entityName, [FromBody]JObject postObject)
		{
			ImportExportManager ieManager = new ImportExportManager(this.hooksService);
			ResponseModel response = ieManager.EvaluateImportEntityRecordsFromCsv(entityName, postObject, controller: this);

			return DoResponse(response);
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

			DbFileRepository fsRepository = new DbFileRepository();
			var file = fsRepository.Find(filepath);

			if (file == null)
				return DoPageNotFoundResponse();

			//check for modification
			string headerModifiedSince = Request.Headers["If-Modified-Since"];
			if (headerModifiedSince != null)
			{
				DateTime isModifiedSince;
				if (DateTime.TryParse(headerModifiedSince, out isModifiedSince))
				{
					if (isModifiedSince <= file.LastModificationDate)
					{
						Response.StatusCode = 304;
						return new EmptyResult();
					}
				}
			}

			HttpContext.Response.Headers.Add("last-modified", file.LastModificationDate.ToString());


			string mimeType;
			var extension = Path.GetExtension(filepath).ToLowerInvariant();
			new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out mimeType);


			IDictionary<string, StringValues> queryCollection = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(HttpContext.Request.QueryString.ToString());
			string action = queryCollection.Keys.Any(x => x == "action") ? ((string)queryCollection["action"]).ToLowerInvariant() : "";
			string requestedMode = queryCollection.Keys.Any(x => x == "mode") ? ((string)queryCollection["mode"]).ToLowerInvariant() : "";
			string width = queryCollection.Keys.Any(x => x == "width") ? ((string)queryCollection["width"]).ToLowerInvariant() : "";
			string height = queryCollection.Keys.Any(x => x == "height") ? ((string)queryCollection["height"]).ToLowerInvariant() : "";
			bool isImage = extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
			if (isImage && (!string.IsNullOrWhiteSpace(action) || !string.IsNullOrWhiteSpace(requestedMode) || !string.IsNullOrWhiteSpace(width) || !string.IsNullOrWhiteSpace(height)))
			{
				var fileContent = file.GetBytes();
				using (ImageFactory imageFactory = new ImageFactory())
				{
					using (Stream inStream = new MemoryStream(fileContent))
					{

						MemoryStream outStream = new MemoryStream();
						imageFactory.Load(inStream);

						//sets background
						System.Drawing.Color backgroundColor = System.Drawing.Color.White;
						switch (imageFactory.CurrentImageFormat.MimeType)
						{
							case "image/gif":
							case "image/png":
								backgroundColor = System.Drawing.Color.Transparent;
								break;
							default:
								break;
						}

						switch (action)
						{
							default:
							case "resize":
								{
									ResizeMode mode;
									switch (requestedMode)
									{
										case "boxpad":
											mode = ResizeMode.BoxPad;
											break;
										case "crop":
											mode = ResizeMode.Crop;
											break;
										case "min":
											mode = ResizeMode.Min;
											break;
										case "max":
											mode = ResizeMode.Max;
											break;
										case "stretch":
											mode = ResizeMode.Stretch;
											break;
										default:
											mode = ResizeMode.Pad;
											break;
									}

									Size size = ParseSize(queryCollection);
									ResizeLayer rl = new ResizeLayer(size, mode);
									imageFactory.Resize(rl).BackgroundColor(backgroundColor).Save(outStream);
								}
								break;
						}

						outStream.Seek(0, SeekOrigin.Begin);
						return File(outStream, mimeType);
					}
				}
			}

			return File(file.GetBytes(), mimeType);
		}

		/// <summary>
		/// Parse width and height parameters from query string
		/// </summary>
		/// <param name="queryCollection"></param>
		/// <returns></returns>
		private Size ParseSize(IDictionary<string, StringValues> queryCollection)
		{
			string width = queryCollection.Keys.Any(x => x == "width") ? (string)queryCollection["width"] : "";
			string height = queryCollection.Keys.Any(x => x == "height") ? (string)queryCollection["height"] : "";
			Size size = new Size();

			// We round up so that single pixel lines are not produced.
			const MidpointRounding Rounding = MidpointRounding.AwayFromZero;

			// First cater for single dimensions.
			if (width != null && height == null)
			{

				width = width.Replace("px", string.Empty);
				size = new Size((int)Math.Round(ImageProcessor.Web.Helpers.QueryParamParser.Instance.ParseValue<float>(width), Rounding), 0);
			}

			if (width == null && height != null)
			{
				height = height.Replace("px", string.Empty);
				size = new Size(0, (int)Math.Round(ImageProcessor.Web.Helpers.QueryParamParser.Instance.ParseValue<float>(height), Rounding));
			}

			// Both supplied
			if (width != null && height != null)
			{
				width = width.Replace("px", string.Empty);
				height = height.Replace("px", string.Empty);
				size = new Size(
					(int)Math.Round(ImageProcessor.Web.Helpers.QueryParamParser.Instance.ParseValue<float>(width), Rounding),
					(int)Math.Round(ImageProcessor.Web.Helpers.QueryParamParser.Instance.ParseValue<float>(height), Rounding));
			}

			return size;
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

	}
}

