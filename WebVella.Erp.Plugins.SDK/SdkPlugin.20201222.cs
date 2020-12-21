using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.SDK
{
    public partial class SdkPlugin : ErpPlugin
    {
        private static void Patch20201222(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
        {

            #region << ***Update field***  Entity: case Field Name: created_on >>
            {
                var currentEntity = entMan.ReadEntity(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c")).Object;
                InputDateTimeField datetimeField = new InputDateTimeField();
                datetimeField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "created_on").Id;
                datetimeField.Name = "created_on";
                datetimeField.Label = "Created on";
                datetimeField.PlaceholderText = null;
                datetimeField.Description = null;
                datetimeField.HelpText = null;
                datetimeField.Required = true;
                datetimeField.Unique = false;
                datetimeField.Searchable = false;
                datetimeField.Auditable = false;
                datetimeField.System = true;
                datetimeField.DefaultValue = null;
                datetimeField.Format = "yyyy-MMM-dd HH:mm";
                datetimeField.UseCurrentTimeAsDefaultValue = true;
                datetimeField.EnableSecurity = false;
                datetimeField.Permissions = new FieldPermissions();
                datetimeField.Permissions.CanRead = new List<Guid>();
                datetimeField.Permissions.CanUpdate = new List<Guid>();
                //READ
                //UPDATE
                {
                    var response = entMan.UpdateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), datetimeField);
                    if (!response.Success)
                        throw new Exception("System error 10060. Entity: case Field: created_on Message:" + response.Message);
                }
            }
            #endregion



        }
    }
}
