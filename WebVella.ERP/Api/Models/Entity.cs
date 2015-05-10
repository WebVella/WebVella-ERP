using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
    public class InputEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "pluralLabel")]
        public string PluralLabel { get; set; }

        [JsonProperty(PropertyName = "system")]
        public bool? System { get; set; }

        [JsonProperty(PropertyName = "permissions")]
        public EntityPermissions Permissions { get; set; }
    }

    public class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "pluralLabel")]
        public string PluralLabel { get; set; }

        [JsonProperty(PropertyName = "system")]
        public bool? System { get; set; }

        [JsonProperty(PropertyName = "permissions")]
        public EntityPermissions Permissions { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<Field> Fields { get; set; }

        [JsonProperty(PropertyName = "views")]
        public List<View> Views { get; set; }

        [JsonProperty(PropertyName = "forms")]
        public List<Form> Forms { get; set; }

        public Entity()
        {

        }

        public Entity(InputEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Label = entity.Label;
            PluralLabel = entity.PluralLabel;
            System = entity.System;
            Permissions = entity.Permissions;
        }

        public Entity(IStorageEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Label = entity.Label;
            PluralLabel = entity.PluralLabel;
            System = entity.System;
            Permissions = new EntityPermissions();
            if (entity.Permissions != null)
            {
                Permissions.CanRead = entity.Permissions.CanRead;
                Permissions.CanUpdate = entity.Permissions.CanUpdate;
                Permissions.CanDelete = entity.Permissions.CanDelete;
            }

            Fields = new List<Field>();

            foreach (IStorageField storageField in entity.Fields)
            {
                Field field = Field.Convert(storageField);                

                Fields.Add(field);
            }

            Views = new List<View>();

            foreach (IStorageView storageView in entity.Views)
            {
                View view = new View();
                view.Id = storageView.Id;
                view.Name = storageView.Name;
                view.Label = storageView.Label;
                view.Type = storageView.Type;

                view.Filters = new List<ViewFilter>();

                foreach (IStorageViewFilter storageFilter in storageView.Filters)
                {
                    ViewFilter filter = new ViewFilter();

                    filter.EntityId = storageFilter.EntityId;
                    filter.FieldId = storageFilter.FieldId;
                    filter.Operator = storageFilter.Operator;
                    filter.Value = storageFilter.Value;

                    view.Filters.Add(filter);
                }

                view.Fields = new List<ViewField>();

                foreach (IStorageViewField storageField in storageView.Fields)
                {
                    ViewField field = new ViewField();

                    field.EntityId = storageField.EntityId;
                    field.Id = storageField.Id;
                    field.Position = storageField.Position;

                    view.Fields.Add(field);
                }

                Views.Add(view);
            }

            Forms = new List<Form>();

            foreach (IStorageForm storageForm in entity.Forms)
            {
                Form form = new Form();
                form.Id = storageForm.Id;
                form.Name = storageForm.Name;
                form.Label = storageForm.Label;

                form.Fields = new List<FormField>();

                foreach (IStorageFormField storageField in storageForm.Fields)
                {
                    FormField field = new FormField();

                    field.EntityId = storageField.EntityId;
                    field.Id = storageField.Id;
                    field.Position = storageField.Position;

                    form.Fields.Add(field);
                }

                Forms.Add(form);
            }
        }
    }

    public class EntityPermissions
    {
        [JsonProperty(PropertyName = "canRead")]
        public List<Guid> CanRead { get; set; }

        [JsonProperty(PropertyName = "canUpdate")]
        public List<Guid> CanUpdate { get; set; }

        [JsonProperty(PropertyName = "canDelete")]
        public List<Guid> CanDelete { get; set; }

        public EntityPermissions()
        {
            CanRead = new List<Guid>();
            CanUpdate = new List<Guid>();
            CanDelete = new List<Guid>();
        }
    }

    public class EntityList
    {
        [JsonProperty(PropertyName = "offset")]
        public Guid Offset { get; set; }

        [JsonProperty(PropertyName = "entities")]
        public List<Entity> Entities { get; set; }

        public EntityList()
        {
            Entities = new List<Entity>();
        }

        public EntityList(List<IStorageEntity> entities)
        {
            Entities = new List<Entity>();

            foreach (IStorageEntity storageEntity in entities)
            {
                Entities.Add(new Entity(storageEntity));
            }
        }
    }

    public class EntityResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public Entity Object { get; set; }
    }

    public class EntityListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public EntityList Object { get; set; }
    }
}