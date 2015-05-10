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
    //    typeof(MongoMasterDetailsRelationshipField),
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
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string PlaceholderText { get; set; }

        public string Description { get; set; }

        public string HelpText { get; set; }

        public bool Required { get; set; }

        public bool Unique { get; set; }

        public bool Searchable { get; set; }

        public bool Auditable { get; set; }

        public bool System { get; set; }
    }
}