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

        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

        [JsonProperty(PropertyName = "recordPermissions")]
        public RecordPermissions RecordPermissions { get; set; }
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

        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

        [JsonProperty(PropertyName = "recordPermissions")]
        public RecordPermissions RecordPermissions { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<Field> Fields { get; set; }

        [JsonProperty(PropertyName = "recordsLists")]
        public List<RecordsList> RecordsLists { get; set; }

        [JsonProperty(PropertyName = "recordViewLists")]
        public List<RecordView> RecordViewLists { get; set; }

        public Entity()
        {

        }

        public Entity(InputEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Label = entity.Label;
            PluralLabel = entity.PluralLabel;
            System = entity.System.Value;
            IconName = entity.IconName;
            Weight = entity.Weight;
            RecordPermissions = entity.RecordPermissions;
            if (RecordPermissions == null)
                RecordPermissions = new RecordPermissions();
        }

        public Entity(IStorageEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Label = entity.Label;
            PluralLabel = entity.PluralLabel;
            System = entity.System;
            IconName = entity.IconName;
            Weight = entity.Weight;
            RecordPermissions = new RecordPermissions();
            if (entity.RecordPermissions != null)
            {
                RecordPermissions.CanRead = entity.RecordPermissions.CanRead;
                RecordPermissions.CanCreate = entity.RecordPermissions.CanCreate;
                RecordPermissions.CanUpdate = entity.RecordPermissions.CanUpdate;
                RecordPermissions.CanDelete = entity.RecordPermissions.CanDelete;
            }

            Fields = new List<Field>();

            foreach (IStorageField storageField in entity.Fields)
            {
                Field field = Field.ConvertField(storageField);                

                Fields.Add(field);
            }

            RecordsLists = new List<RecordsList>();

            foreach (IStorageRecordsList storageRecordsList in entity.RecordsLists)
            {
                RecordsList recordsList = new RecordsList();
                recordsList.Id = storageRecordsList.Id;
                recordsList.Name = storageRecordsList.Name;
                recordsList.Label = storageRecordsList.Label;
                recordsList.Type = storageRecordsList.Type;

                recordsList.Filters = new List<RecordsListFilter>();

                foreach (IStorageRecordsListFilter storageFilter in storageRecordsList.Filters)
                {
                    RecordsListFilter filter = new RecordsListFilter();

                    filter.EntityId = storageFilter.EntityId;
                    filter.FieldId = storageFilter.FieldId;
                    filter.Operator = storageFilter.Operator;
                    filter.Value = storageFilter.Value;

                    recordsList.Filters.Add(filter);
                }

                recordsList.Fields = new List<RecordsListField>();

                foreach (IStorageRecordsListField storageField in storageRecordsList.Fields)
                {
                    RecordsListField field = new RecordsListField();

                    field.EntityId = storageField.EntityId;
                    field.Id = storageField.Id;
                    field.Position = storageField.Position;

                    recordsList.Fields.Add(field);
                }

                RecordsLists.Add(recordsList);
            }

            RecordViewLists = new List<RecordView>();

            foreach (IStorageRecordView storageRecordView in entity.RecordViewList)
            {
                RecordView recordView = new RecordView();
                recordView.Id = storageRecordView.Id;
                recordView.Name = storageRecordView.Name;
                recordView.Label = storageRecordView.Label;

                recordView.Fields = new List<RecordViewField>();

                foreach (IStorageRecordViewField storageField in storageRecordView.Fields)
                {
                    RecordViewField field = new RecordViewField();

                    field.EntityId = storageField.EntityId;
                    field.Id = storageField.Id;
                    field.Position = storageField.Position;

                    recordView.Fields.Add(field);
                }

                RecordViewLists.Add(recordView);
            }
        }
    }

    public class RecordPermissions
    {
        [JsonProperty(PropertyName = "canRead")]
        public List<Guid> CanRead { get; set; }

        [JsonProperty(PropertyName = "canCreate")]
        public List<Guid> CanCreate { get; set; }

        [JsonProperty(PropertyName = "canUpdate")]
        public List<Guid> CanUpdate { get; set; }

        [JsonProperty(PropertyName = "canDelete")]
        public List<Guid> CanDelete { get; set; }

        public RecordPermissions()
        {
            CanRead = new List<Guid>();
            CanCreate = new List<Guid>();
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