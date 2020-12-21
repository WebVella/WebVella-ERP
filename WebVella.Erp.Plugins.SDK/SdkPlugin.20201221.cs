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
        private static void Patch20201221(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
        {
            #region << ***Update entity*** Entity name: role >>
            {
                var updateObject = new InputEntity();
                updateObject.Id = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
                updateObject.Name = "role";
                updateObject.Label = "Role";
                updateObject.LabelPlural = "Roles";
                updateObject.System = true;
                updateObject.IconName = "fa fa-key";
                updateObject.Color = "#f44336";
                updateObject.RecordScreenIdField = null;
                updateObject.RecordPermissions = new RecordPermissions();
                updateObject.RecordPermissions.CanRead = new List<Guid>();
                updateObject.RecordPermissions.CanCreate = new List<Guid>();
                updateObject.RecordPermissions.CanUpdate = new List<Guid>();
                updateObject.RecordPermissions.CanDelete = new List<Guid>();
                updateObject.RecordPermissions.CanRead.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
                updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
                updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanCreate.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
                updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                var updateEntityResult = entMan.UpdateEntity(updateObject);
                if (!updateEntityResult.Success)
                {
                    throw new Exception("System error 10060. Entity update with name : role. Message:" + updateEntityResult.Message);
                }
            }
            #endregion

            #region << ***Update entity*** Entity name: user >>
            {
                var updateObject = new InputEntity();
                updateObject.Id = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
                updateObject.Name = "user";
                updateObject.Label = "User";
                updateObject.LabelPlural = "Users";
                updateObject.System = true;
                updateObject.IconName = "fa fa-user";
                updateObject.Color = "#f44336";
                updateObject.RecordScreenIdField = null;
                updateObject.RecordPermissions = new RecordPermissions();
                updateObject.RecordPermissions.CanRead = new List<Guid>();
                updateObject.RecordPermissions.CanCreate = new List<Guid>();
                updateObject.RecordPermissions.CanUpdate = new List<Guid>();
                updateObject.RecordPermissions.CanDelete = new List<Guid>();
                updateObject.RecordPermissions.CanRead.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
                updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
                updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanCreate.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
                updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                var updateEntityResult = entMan.UpdateEntity(updateObject);
                if (!updateEntityResult.Success)
                {
                    throw new Exception("System error 10060. Entity update with name : user. Message:" + updateEntityResult.Message);
                }
            }
            #endregion

            #region << ***Update entity*** Entity name: user_file >>
            {
                var updateObject = new InputEntity();
                updateObject.Id = new Guid("5c666c54-9e76-4327-ac7a-55851037810c");
                updateObject.Name = "user_file";
                updateObject.Label = "User File";
                updateObject.LabelPlural = "User Files";
                updateObject.System = true;
                updateObject.IconName = "fa fa-file";
                updateObject.Color = "#f44336";
                updateObject.RecordScreenIdField = null;
                updateObject.RecordPermissions = new RecordPermissions();
                updateObject.RecordPermissions.CanRead = new List<Guid>();
                updateObject.RecordPermissions.CanCreate = new List<Guid>();
                updateObject.RecordPermissions.CanUpdate = new List<Guid>();
                updateObject.RecordPermissions.CanDelete = new List<Guid>();
                updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
                updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
                updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
                updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                updateObject.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
                updateObject.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
                var updateEntityResult = entMan.UpdateEntity(updateObject);
                if (!updateEntityResult.Success)
                {
                    throw new Exception("System error 10060. Entity update with name : user_file. Message:" + updateEntityResult.Message);
                }
            }
            #endregion

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
                try { datetimeField.DefaultValue = DateTime.Parse("12/31/1999 10:00:00 PM"); } catch { datetimeField.DefaultValue = DateTime.Parse("12/31/1999 10:00:00 PM", new CultureInfo("en-US")); }
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
