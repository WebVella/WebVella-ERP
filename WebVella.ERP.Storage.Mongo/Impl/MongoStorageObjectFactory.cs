using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageObjectFactory : IStorageObjectFactory
    {
        public IStorageEntity CreateEmptyEntityObject()
        {
            return new MongoEntity();
        }

        public IStorageEntityRelation CreateEmptyEntityRelationObject()
        {
            return new MongoEntityRelation();
        }

        public IStorageField CreateEmptyFieldObject(Type type)
        {
            if (type == typeof(AutoNumberField))
                return new MongoAutoNumberField();
            if (type == typeof(CheckboxField))
                return new MongoCheckboxField();
            if (type == typeof(CurrencyField))
                return new MongoCurrencyField();
            if (type == typeof(DateTimeField))
                return new MongoDateField();
            if (type == typeof(DateField))
                return new MongoDateField();
            if (type == typeof(EmailField))
                return new MongoEmailField();
            if (type == typeof(FileField))
                return new MongoFileField();
            if (type == typeof(GuidField))
                return new MongoGuidField();
            if (type == typeof(HtmlField))
                return new MongoHtmlField();
            if (type == typeof(ImageField))
                return new MongoImageField();
            if (type == typeof(MultiLineTextField))
                return new MongoMultiLineTextField();
            if (type == typeof(MultiSelectField))
                return new MongoMultiSelectField();
            if (type == typeof(NumberField))
                return new MongoNumberField();
            if (type == typeof(PasswordField))
                return new MongoPasswordField();
            if (type == typeof(PercentField))
                return new MongoPercentField();
            if (type == typeof(PhoneField))
                return new MongoPhoneField();
            if (type == typeof(SelectField))
                return new MongoSelectField();
            if (type == typeof(TextField))
                return new MongoTextField();
            if (type == typeof(UrlField))
                return new MongoUrlField();

            throw new Exception("The provided field type is not supported by current storage implementation");
        }

        public IStorageRecordPermissions CreateEmptyRecordPermissionsObject()
        {
            return new MongoRecordPermissions();
        }

        public IStorageRecordsList CreateEmptyRecordsListObject()
        {
            return new MongoRecordsList();
        }

        public IStorageRecordView CreateEmptyRecordViewObject()
        {
            return new MongoRecordView();
        }

        public IStorageMultiSelectFieldOption CreateEmptyMultiSelectFieldOptionObject()
        {
            return new MongoMultiSelectFieldOption();
        }

        public IStorageSelectFieldOption CreateEmptySelectFieldOptionObject()
        {
            return new MongoSelectFieldOption();
        }

        public IStorageRecordsListFilter CreateEmptyRecordsListFilterObject()
        {
            return new MongoRecordsListFilter();
        }

        public IStorageRecordsListField CreateEmptyRecordsListFieldObject()
        {
            return new MongoRecordsListField();
        }

        public IStorageRecordViewField CreateEmptyRecordViewFieldObject()
        {
            return new MongoRecordViewField();
        }
    }
}
