using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
    public class InputEntity
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string PluralLabel { get; set; }

        public bool? System { get; set; }
    }

    public class Entity
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string PluralLabel { get; set; }

        public bool? System { get; set; }

        public List<Field> Fields { get; set; }

        public List<View> Views { get; set; }

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
        }

        public Entity(IStorageEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            entity.Label = entity.Label;
            entity.PluralLabel = entity.PluralLabel;
            System = entity.System;

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

    public class EntityList
    {
        public Guid Offset { get; set; }

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
        public Entity Object { get; set; }
    }

    public class EntityListResponse : BaseResponseModel
    {
        public EntityList Object { get; set; }
    }
}