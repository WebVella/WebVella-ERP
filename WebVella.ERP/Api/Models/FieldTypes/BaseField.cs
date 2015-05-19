using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP
{
    public abstract class Field
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "placeholderText")]
        public string PlaceholderText { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "helpText")]
        public string HelpText { get; set; }

        [JsonProperty(PropertyName = "required")]
        public bool? Required { get; set; }

        [JsonProperty(PropertyName = "unique")]
        public bool? Unique { get; set; }

        [JsonProperty(PropertyName = "searchable")]
        public bool? Searchable { get; set; }

        [JsonProperty(PropertyName = "auditable")]
        public bool? Auditable { get; set; }

        [JsonProperty(PropertyName = "system")]
        public bool? System { get; set; }

        public Field()
        {
            Required = false;
            Unique = false;
            Searchable = false;
            Auditable = false;
            System = false;
        }

        public Field(Field field)
        {
            Id = field.Id;
            Name = field.Name;
            Label = field.Label;
            PlaceholderText = field.PlaceholderText;
            Description = field.Description;
            HelpText = field.HelpText;
            Required = field.Required;
            Unique = field.Unique;
            Searchable = field.Searchable;
            Auditable = field.Auditable;
            System = field.System;
        }

        public static Field ConvertField(JObject inputField)
        {
            Field field = null;

            var fieldTypeProp = inputField.Properties().SingleOrDefault(k => k.Name.ToLower() == "fieldtype");
            if (fieldTypeProp == null)
                return field;

            FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), fieldTypeProp.Value.ToObject<int>());

            switch (fieldType)
            {
                case FieldType.AutoNumberField:
                    field = inputField.ToObject<AutoNumberField>();
                    break;
                case FieldType.CheckboxField:
                    field = inputField.ToObject<CheckboxField>();
                    break;
                case FieldType.CurrencyField:
                    field = inputField.ToObject<CurrencyField>();
                    break;
                case FieldType.DateField:
                    field = inputField.ToObject<DateField>();
                    break;
                case FieldType.DateTimeField:
                    field = inputField.ToObject<DateTimeField>();
                    break;
                case FieldType.EmailField:
                    field = inputField.ToObject<EmailField>();
                    break;
                case FieldType.FileField:
                    field = inputField.ToObject<FileField>();
                    break;
                case FieldType.HtmlField:
                    field = inputField.ToObject<HtmlField>();
                    break;
                case FieldType.ImageField:
                    field = inputField.ToObject<ImageField>();
                    break;
                case FieldType.MultiLineTextField:
                    field = inputField.ToObject<MultiLineTextField>();
                    break;
                case FieldType.MultiSelectField:
                    field = inputField.ToObject<MultiSelectField>();
                    break;
                case FieldType.NumberField:
                    field = inputField.ToObject<NumberField>();
                    break;
                case FieldType.PasswordField:
                    field = inputField.ToObject<PasswordField>();
                    break;
                case FieldType.PercentField:
                    field = inputField.ToObject<PercentField>();
                    break;
                case FieldType.PhoneField:
                    field = inputField.ToObject<PhoneField>();
                    break;
                case FieldType.GuidField:
                    field = inputField.ToObject<GuidField>();
                    break;
                case FieldType.SelectField:
                    field = inputField.ToObject<SelectField>();
                    break;
                case FieldType.TextField:
                    field = inputField.ToObject<TextField>();
                    break;
                case FieldType.UrlField:
                    field = inputField.ToObject<UrlField>();
                    break;
            }

            return field;
        }
    }

    public class FieldList
    {
        [JsonProperty(PropertyName = "fields")]
        public List<Field> Fields { get; set; }

        public FieldList()
        {
            Fields = new List<Field>();
        }
    }

    public class FieldResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public Field Object { get; set; }
    }

    public class FieldListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public FieldList Object { get; set; }
    }
}