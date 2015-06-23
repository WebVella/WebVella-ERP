using MongoDB.Bson.Serialization.Attributes;
using System;
using WebVella.ERP.Storage;


namespace WebVella.ERP.Storage.Mongo
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
    public abstract class MongoBaseField : IStorageField
    {
		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("label")]
		public string Label { get; set; }

		[BsonElement("placeholderText")]
		public string PlaceholderText { get; set; }

		[BsonElement("description")]
		public string Description { get; set; }

		[BsonElement("helpText")]
		public string HelpText { get; set; }

		[BsonElement("required")]
		public bool Required { get; set; }

		[BsonElement("unique")]
		public bool Unique { get; set; }

		[BsonElement("searchable")]
		public bool Searchable { get; set; }

		[BsonElement("auditable")]
		public bool Auditable { get; set; }

		[BsonElement("system")]
		public bool System { get; set; }
    }
}