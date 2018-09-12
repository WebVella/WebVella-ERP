using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Project
{
	public partial class Startup
	{
		private static void Patch20180913(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{
		
#region << ***Update area***  Area name: projects >>
{
	var patchObject = new EntityRecord();
	patchObject["id"] = new Guid("205877a1-242c-41bf-a080-49ea01d4f519");
	patchObject["attachments"] = "[{\"name\":null,\"label\":\"My Dashboard\",\"labelPlural\":null,\"iconName\":\"tachometer\",\"weight\":1,\"url\":\"/#/areas/projects/wv_project/dashboard\",\"view\":null,\"create\":null,\"list\":null},{\"name\":null,\"label\":\"Search\",\"url\":\"/#/areas/projects/wv_project/search\",\"labelPlural\":null,\"iconName\":\"search\",\"weight\":\"2\",\"view\":null,\"list\":null,\"create\":null},{\"name\":\"wv_task\",\"label\":\"Task\",\"labelPlural\":\"Tasks\",\"iconName\":\"tasks\",\"weight\":4,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"Details\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_tasks\",\"label\":\"My open tasks\"}},{\"name\":\"wv_bug\",\"label\":\"Bug\",\"labelPlural\":\"Bugs\",\"iconName\":\"bug\",\"weight\":5,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"Details\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_bugs\",\"label\":\"My open bugs\"}},{\"name\":\"wv_project\",\"label\":\"Project\",\"labelPlural\":\"Projects\",\"iconName\":\"product-hunt\",\"weight\":22,\"url\":null,\"view\":{\"name\":\"dashboard\",\"label\":\"Dashboard\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_projects\",\"label\":\"My Projects\"}},{\"name\":null,\"label\":\"My Sprints\",\"url\":\"/#/areas/projects/wv_project/sprints\",\"labelPlural\":null,\"iconName\":\"fast-forward\",\"weight\":\"50\",\"view\":null,\"list\":null,\"create\":null}]";
	var updateAreaResult = recMan.UpdateRecord("area", patchObject);
	if (!updateAreaResult.Success)
	{
		throw new Exception("System error 10060. Area update with name : projects. Message:" + updateAreaResult.Message);
	}
}
#endregion

#region << ***Update field***  Entity: search Field Name: created_on >>
{
	var currentEntity = entMan.ReadEntity(new Guid("171659b7-79a3-457e-844b-6c954b59420f")).Object;
	InputDateTimeField datetimeField = new InputDateTimeField();
	datetimeField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == "created_on").Id;
	datetimeField.Name = "created_on";
	datetimeField.Label = "Created On";
	datetimeField.PlaceholderText = "";
	datetimeField.Description = "";
	datetimeField.HelpText = "";
	datetimeField.Required = false;
	datetimeField.Unique = false;
	datetimeField.Searchable = true;
	datetimeField.Auditable = false;
	datetimeField.System = true;
	datetimeField.DefaultValue = null;
	datetimeField.Format = "dd MMM yyyy HH:mm";
	datetimeField.UseCurrentTimeAsDefaultValue = true;
	datetimeField.EnableSecurity = false;
	datetimeField.Permissions = new FieldPermissions();
	datetimeField.Permissions.CanRead = new List<Guid>();
	datetimeField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.UpdateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), datetimeField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: created_on Message:" + response.Message);
	}
}
#endregion

#region << ***Update field***  Entity: search Field Name: index >>
{
	var currentEntity = entMan.ReadEntity(new Guid("171659b7-79a3-457e-844b-6c954b59420f")).Object;
	InputTextField textboxField = new InputTextField();
	textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "index").Id;
	textboxField.Name = "index";
	textboxField.Label = "Index";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = true;
	textboxField.Unique = false;
	textboxField.Searchable = false;
	textboxField.Auditable = false;
	textboxField.System = true;
	textboxField.DefaultValue = "ddddd";
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.UpdateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), textboxField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: index Message:" + response.Message);
	}
}
#endregion


		}


	}
}
