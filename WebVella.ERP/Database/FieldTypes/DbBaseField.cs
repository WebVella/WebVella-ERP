using Newtonsoft.Json;
using System;


namespace WebVella.ERP.Database
{
	//strange it doesn't work so we use classmap registration in staticcontext
	//[BsonDiscriminator(Required = true)]
	//[BsonKnownTypes(
	//    typeof(MongoAutoNumberField), 
	//    typeof(MongoCheckboxField),
	//    typeof(MongoCurrencyField),
	//    typeof(MongoDateField),
	//    typeof(MongoDateTimeField),
	//    typeof(MongoEmailField),
	//    typeof(MongoFileField),
	//    typeof(MongoHtmlField),
	//    typeof(MongoImageField),
	//    typeof(MongoLookupRelationField),
	//    typeof(MongoMultiLineTextField),
	//    typeof(MongoMultiSelectField),
	//    typeof(MongoNumberField),
	//    typeof(MongoPasswordField),
	//    typeof(MongoPercentField),
	//    typeof(MongoPhoneField),
	//    typeof(MongoPrimaryKeyField),
	//    typeof(MongoSelectField),
	//    typeof(MongoTextField),
	//    typeof(MongoUrlField)
	//    )]
	public abstract class DbBaseField
    {
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "placeholder_text")]
		public string PlaceholderText { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "help_text")]
		public string HelpText { get; set; }

		[JsonProperty(PropertyName = "required")]
		public bool Required { get; set; }

		[JsonProperty(PropertyName = "unique")]
		public bool Unique { get; set; }

		[JsonProperty(PropertyName = "searchable")]
		public bool Searchable { get; set; }

		[JsonProperty(PropertyName = "auditable")]
		public bool Auditable { get; set; }

		[JsonProperty(PropertyName = "system")]
		public bool System { get; set; }

        [JsonProperty(PropertyName = "permissions")]
        public DbFieldPermissions Permissions { get; set; }

        [JsonProperty(PropertyName = "enable_security")]
        public bool EnableSecurity { get; set; }
    }
}