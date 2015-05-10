using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Storage;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api;

namespace WebVella.ERP
{
    public class ERPService : IERPService
    {
        public static IERPService Current
        {
            get; set;
        }

        public IStorageService StorageService
        {
            get; set;
        }

        public ERPService(IStorageService storage)
        {
            if (Current == null)
                Current = this;

            StorageService = storage;
        }

        public void RunTests()
        {
           //EntityTests();
        }

        public void InitializeSystemEntities()
        {
            int version = 150508;

            int currentVersion = 0;

            Guid systemEntityId = new Guid("A5050AC8-5967-4CE1-95E7-A79B054F9D14");
            Guid userEntityId = new Guid("B9CEBC3B-6443-452A-8E34-B311A73DCC8B");
            Guid roleEntityId = new Guid("C4541FEE-FBB6-4661-929E-1724ADEC285A");

            EntityManager entityManager = new EntityManager(StorageService);

            //Get current version here

            if (currentVersion < 150508)
            {
                InputEntity systemEntity = new InputEntity();
                systemEntity.Id = systemEntityId;
                systemEntity.Name = "System";
                systemEntity.Label = "System";
                systemEntity.PluralLabel = "Systems";
                systemEntity.System = true;

                EntityResponse response = entityManager.CreateEntity(systemEntity);

                NumberField versionField = new NumberField();

                versionField.Id = Guid.NewGuid();
                versionField.Name = "Version";
                versionField.Label = "Version";
                versionField.PlaceholderText = "";
                versionField.Description = "this field hold database version";
                versionField.HelpText = "";
                versionField.Required = true;
                versionField.Unique = true;
                versionField.Searchable = false;
                versionField.Auditable = false;
                versionField.System = true;
                versionField.DefaultValue = 0;
                
                versionField.MinValue = 1;
                versionField.MaxValue = 100;
                versionField.DecimalPlaces = 3;

                FieldResponse fieldResponse = entityManager.CreateField(systemEntity.Id.Value, versionField);

                InputEntity userEntity = new InputEntity();
                userEntity.Id = userEntityId;
                userEntity.Name = "User";
                userEntity.Label = "User";
                userEntity.PluralLabel = "Users";
                userEntity.System = true;

                TextField firstName = new TextField();

                firstName.Id = Guid.NewGuid();
                firstName.Name = "firstName";
                firstName.Label = "First Name";
                firstName.PlaceholderText = "";
                firstName.Description = "First name of the user";
                firstName.HelpText = "";
                firstName.Required = true;
                firstName.Unique = false;
                firstName.Searchable = true;
                firstName.Auditable = false;
                firstName.System = true;
                firstName.DefaultValue = "";

                firstName.MaxLength = 200;

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, firstName);

                TextField lastName = new TextField();

                lastName.Id = Guid.NewGuid();
                lastName.Name = "lastName";
                lastName.Label = "Last Name";
                lastName.PlaceholderText = "";
                lastName.Description = "Last name of the user";
                lastName.HelpText = "";
                lastName.Required = true;
                lastName.Unique = false;
                lastName.Searchable = true;
                lastName.Auditable = false;
                lastName.System = true;
                lastName.DefaultValue = "";

                lastName.MaxLength = 200;

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, lastName);

                EmailField email = new EmailField();

                email.Id = Guid.NewGuid();
                email.Name = "Email";
                email.Label = "Email";
                email.PlaceholderText = "";
                email.Description = "Email address of the user";
                email.HelpText = "";
                email.Required = true;
                email.Unique = true;
                email.Searchable = true;
                email.Auditable = false;
                email.System = true;
                email.DefaultValue = "";

                email.MaxLength = 255;

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, email);

                PasswordField password = new PasswordField();

                password.Id = Guid.NewGuid();
                password.Name = "Password";
                password.Label = "Password";
                password.PlaceholderText = "";
                password.Description = "Password for the user account";
                password.HelpText = "";
                password.Required = true;
                password.Unique = true;
                password.Searchable = true;
                password.Auditable = false;
                password.System = true;

                password.MaxLength = 1;
                password.MaskType = Api.PasswordFieldMaskTypes.MaskAllCharacters;
                password.MaskCharacter = '*';
                password.Encrypted = true;

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, password);

                DateTimeField lastLoggedIn = new DateTimeField();

                lastLoggedIn.Id = Guid.NewGuid();
                lastLoggedIn.Name = "lastLoggedIn";
                lastLoggedIn.Label = "Last Logged In";
                lastLoggedIn.PlaceholderText = "";
                lastLoggedIn.Description = "";
                lastLoggedIn.HelpText = "";
                lastLoggedIn.Required = true;
                lastLoggedIn.Unique = true;
                lastLoggedIn.Searchable = true;
                lastLoggedIn.Auditable = true;
                lastLoggedIn.System = true;
                lastLoggedIn.DefaultValue = DateTime.MinValue;

                lastLoggedIn.Format = "MM/dd/YYYY";

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, lastLoggedIn);

                CheckboxField enabledField = new CheckboxField();

                enabledField.Id = Guid.NewGuid();
                enabledField.Name = "Enabled";
                enabledField.Label = "Enabled";
                enabledField.PlaceholderText = "";
                enabledField.Description = "Shous if the user account is enabled";
                enabledField.HelpText = "";
                enabledField.Required = true;
                enabledField.Unique = false;
                enabledField.Searchable = true;
                enabledField.Auditable = false;
                enabledField.System = true;
                enabledField.DefaultValue = false;

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, enabledField);

                CheckboxField verifiedUserField = new CheckboxField();

                verifiedUserField.Id = Guid.NewGuid();
                verifiedUserField.Name = "Verified";
                verifiedUserField.Label = "Verified";
                verifiedUserField.PlaceholderText = "";
                verifiedUserField.Description = "Shows if the user email is verified";
                verifiedUserField.HelpText = "";
                verifiedUserField.Required = true;
                verifiedUserField.Unique = false;
                verifiedUserField.Searchable = true;
                verifiedUserField.Auditable = false;
                verifiedUserField.System = true;
                verifiedUserField.DefaultValue = false;

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, verifiedUserField);

                Entity roleEntity = new Entity();

                InputEntity inputRoleEntity = new InputEntity();
                inputRoleEntity.Id = roleEntityId;
                inputRoleEntity.Name = "Role";
                inputRoleEntity.Label = "Role";
                inputRoleEntity.PluralLabel = "Roles";
                inputRoleEntity.System = true;


                TextField nameRoleField = new TextField();

                nameRoleField.Id = Guid.NewGuid();
                nameRoleField.Name = "Name";
                nameRoleField.Label = "Name";
                nameRoleField.PlaceholderText = "";
                nameRoleField.Description = "The name of the role";
                nameRoleField.HelpText = "";
                nameRoleField.Required = true;
                nameRoleField.Unique = false;
                nameRoleField.Searchable = true;
                nameRoleField.Auditable = false;
                nameRoleField.System = true;
                nameRoleField.DefaultValue = "";

                firstName.MaxLength = 200;

                fieldResponse = entityManager.CreateField(userEntity.Id.Value, firstName);

                TextField descriptionRoleField = new TextField();

                descriptionRoleField.Id = Guid.NewGuid();
                descriptionRoleField.Name = "Description";
                descriptionRoleField.Label = "Description";
                descriptionRoleField.PlaceholderText = "";
                descriptionRoleField.Description = "";
                descriptionRoleField.HelpText = "";
                descriptionRoleField.Required = true;
                descriptionRoleField.Unique = false;
                descriptionRoleField.Searchable = true;
                descriptionRoleField.Auditable = false;
                descriptionRoleField.System = true;
                descriptionRoleField.DefaultValue = "";
                
                descriptionRoleField.MaxLength = 200;

                fieldResponse = entityManager.CreateField(roleEntity.Id.Value, descriptionRoleField);
            }

            if (currentVersion <= 150510)
            {

            }
        }

        private void EntityTests()
        {
            Debug.WriteLine("==== START ENTITY TESTS====");


            var entityManager = new EntityManager(StorageService);

            InputEntity inputEntity = new InputEntity();
            //entity.Id = new Guid("C5050AC8-5967-4CE1-95E7-A79B054F9D14");
            inputEntity.Id = Guid.NewGuid();
            inputEntity.Name = "GoroTest";
            inputEntity.Label = "Goro Test";
            inputEntity.PluralLabel = "Goro Tests";
            inputEntity.System = true;

            try
            {
                Entity entity = new Entity(inputEntity);

                EntityResponse response = entityManager.CreateEntity(inputEntity);

                TextField field = new TextField();
                field.Id = Guid.NewGuid();
                field.Name = "TextField";
                field.Label = "Text field";
                field.PlaceholderText = "Text field placeholder text";
                field.Description = "Text field description";
                field.HelpText = "Text field help text";
                field.Required = true;
                field.Unique = true;
                field.Searchable = true;
                field.Auditable = true;
                field.System = true;
                field.DefaultValue = "";

                field.MaxLength = 200;

                FieldResponse fieldResponse = entityManager.CreateField(entity.Id.Value, field);

                inputEntity.Label = "GoroTest_edited";
                inputEntity.PluralLabel = "Goro Tests - edited";

                response = entityManager.UpdateEntity(inputEntity);

                field.Label = "TextField_edited";

                fieldResponse = entityManager.UpdateField(entity.Id.Value, field);

                fieldResponse = entityManager.DeleteField(entity.Id.Value, field.Id.Value);

                List<Field> fields = CreateTestFieldCollection(entity);
                //FieldResponse fieldResponse = entityManager.CreateField(entity.Id.Value, fields[0]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[1]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[2]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[3]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[4]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[5]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[6]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[7]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[8]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[9]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[10]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[11]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[12]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[13]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[14]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[15]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[16]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[17]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[18]);
                fieldResponse = entityManager.CreateField(entity.Id.Value, fields[19]);

                EntityResponse entityResponse = entityManager.ReadEntity(entity.Id.Value);
                entity = entityResponse.Object;

                List<View> views = CreateTestViewCollection(entity);

                ViewResponse viewResponse = entityManager.CreateView(entity.Id.Value, views[0]);

                views[0].Label = "Edited View";

                viewResponse = entityManager.UpdateView(entity.Id.Value, views[0]);

                List<Form> forms = CreateTestFormCollection(entity);

                FormResponse formResponse = entityManager.CreateForm(entity.Id.Value, forms[0]);

                forms[0].Label = "Edited Form";

                formResponse = entityManager.CreateForm(entity.Id.Value, forms[0]);

                entityManager.ReadEntities();

                EntityResponse resultEntity = entityManager.ReadEntity(entity.Id.Value);

                response = entityManager.DeleteEntity(entity.Id.Value);

            }
            catch (StorageException e)
            {
                Debug.WriteLine(e);
            }

            Debug.WriteLine("==== END ENTITY TESTS====");
        }

        private List<Field> CreateTestFieldCollection(Entity entity)
        {
            List<Field> fields = new List<Field>();

            AutoNumberField autoNumberField = new AutoNumberField();

            autoNumberField.Id = Guid.NewGuid();
            autoNumberField.Name = "AutoNumberField";
            autoNumberField.Label = "AutoNumber field";
            autoNumberField.PlaceholderText = "AutoNumber field placeholder text";
            autoNumberField.Description = "AutoNumber field description";
            autoNumberField.HelpText = "AutoNumber field help text";
            autoNumberField.Required = true;
            autoNumberField.Unique = true;
            autoNumberField.Searchable = true;
            autoNumberField.Auditable = true;
            autoNumberField.System = true;
            autoNumberField.DefaultValue = 0;

            autoNumberField.DisplayFormat = "A{0000}";
            autoNumberField.StartingNumber = 10;

            fields.Add(autoNumberField);

            CheckboxField checkboxField = new CheckboxField();

            checkboxField.Id = Guid.NewGuid();
            checkboxField.Name = "CheckboxField";
            checkboxField.Label = "Checkbox field";
            checkboxField.PlaceholderText = "Checkbox field placeholder text";
            checkboxField.Description = "Checkbox field description";
            checkboxField.HelpText = "Checkbox field help text";
            checkboxField.Required = true;
            checkboxField.Unique = true;
            checkboxField.Searchable = true;
            checkboxField.Auditable = true;
            checkboxField.System = true;
            checkboxField.DefaultValue = false;

            fields.Add(checkboxField);

            CurrencyField currencyField = new CurrencyField();

            currencyField.Id = Guid.NewGuid();
            currencyField.Name = "CurrencyField";
            currencyField.Label = "Currency field";
            currencyField.PlaceholderText = "Currency field placeholder text";
            currencyField.Description = "Currency field description";
            currencyField.HelpText = "Currency field help text";
            currencyField.Required = true;
            currencyField.Unique = true;
            currencyField.Searchable = true;
            currencyField.Auditable = true;
            currencyField.System = true;
            currencyField.DefaultValue = 0;

            currencyField.MinValue = 1;
            currencyField.MaxValue = 35;
            currencyField.Currency = new CurrencyTypes();
            currencyField.Currency.CurrencyName = "USD";
            currencyField.Currency.CurrencySymbol = "$";
            currencyField.Currency.Position = CurrencyPosition.AfterTheNumber;

            fields.Add(currencyField);

            DateField dateField = new DateField();

            dateField.Id = Guid.NewGuid();
            dateField.Name = "DateField";
            dateField.Label = "Date field";
            dateField.PlaceholderText = "Date field placeholder text";
            dateField.Description = "Date field description";
            dateField.HelpText = "Date field help text";
            dateField.Required = true;
            dateField.Unique = true;
            dateField.Searchable = true;
            dateField.Auditable = true;
            dateField.System = true;
            dateField.DefaultValue = DateTime.MinValue;

            dateField.Format = "dd.MM.YYYY";

            fields.Add(dateField);

            DateTimeField dateTimeField = new DateTimeField();

            dateTimeField.Id = Guid.NewGuid();
            dateTimeField.Name = "DateTimeField";
            dateTimeField.Label = "DateTime field";
            dateTimeField.PlaceholderText = "DateTime field placeholder text";
            dateTimeField.Description = "DateTime field description";
            dateTimeField.HelpText = "DateTime field help text";
            dateTimeField.Required = true;
            dateTimeField.Unique = true;
            dateTimeField.Searchable = true;
            dateTimeField.Auditable = true;
            dateTimeField.System = true;
            dateTimeField.DefaultValue = DateTime.MinValue;

            dateTimeField.Format = "dd.MM.YYYY";

            fields.Add(dateTimeField);

            EmailField emailField = new EmailField();

            emailField.Id = Guid.NewGuid();
            emailField.Name = "EmailField";
            emailField.Label = "Email field";
            emailField.PlaceholderText = "Email field placeholder text";
            emailField.Description = "Email field description";
            emailField.HelpText = "Email field help text";
            emailField.Required = true;
            emailField.Unique = true;
            emailField.Searchable = true;
            emailField.Auditable = true;
            emailField.System = true;
            emailField.DefaultValue = "";

            emailField.MaxLength = 255;

            fields.Add(emailField);

            FileField fileField = new FileField();

            fileField.Id = Guid.NewGuid();
            fileField.Name = "FileField";
            fileField.Label = "File field";
            fileField.PlaceholderText = "File field placeholder text";
            fileField.Description = "File field description";
            fileField.HelpText = "File field help text";
            fileField.Required = true;
            fileField.Unique = true;
            fileField.Searchable = true;
            fileField.Auditable = true;
            fileField.System = true;
            fileField.DefaultValue = "";

            fields.Add(fileField);

            //FormulaField formulaField = new FormulaField();

            //formulaField.Id = Guid.NewGuid();
            //formulaField.Name = "Formula field";
            //formulaField.Label = "Formula field";
            //formulaField.PlaceholderText = "Formula field placeholder text";
            //formulaField.Description = "Formula field description";
            //formulaField.HelpText = "Formula field help text";
            //formulaField.Required = true;
            //formulaField.Unique = true;
            //formulaField.Searchable = true;
            //formulaField.Auditable = true;
            //formulaField.System = true;

            //formulaField.ReturnType = Api.FormulaFieldReturnType.Number;
            //formulaField.FormulaText = "2 + 5";
            //formulaField.DecimalPlaces = 2;

            //fields.Add(formulaField);

            HtmlField htmlField = new HtmlField();

            htmlField.Id = Guid.NewGuid();
            htmlField.Name = "HtmlField";
            htmlField.Label = "Html field";
            htmlField.PlaceholderText = "Html field placeholder text";
            htmlField.Description = "Html field description";
            htmlField.HelpText = "Html field help text";
            htmlField.Required = true;
            htmlField.Unique = true;
            htmlField.Searchable = true;
            htmlField.Auditable = true;
            htmlField.System = true;
            htmlField.DefaultValue = "";

            fields.Add(htmlField);

            ImageField imageField = new ImageField();

            imageField.Id = Guid.NewGuid();
            imageField.Name = "ImageField";
            imageField.Label = "Image field";
            imageField.PlaceholderText = "Image field placeholder text";
            imageField.Description = "Image field description";
            imageField.HelpText = "Image field help text";
            imageField.Required = true;
            imageField.Unique = true;
            imageField.Searchable = true;
            imageField.Auditable = true;
            imageField.System = true;
            imageField.DefaultValue = "";
            fields.Add(imageField);

            LookupRelationField lookupRelationField = new LookupRelationField();

            lookupRelationField.Id = Guid.NewGuid();
            lookupRelationField.Name = "LookupRelationField";
            lookupRelationField.Label = "LookupRelation field";
            lookupRelationField.PlaceholderText = "LookupRelation field placeholder text";
            lookupRelationField.Description = "LookupRelation field description";
            lookupRelationField.HelpText = "LookupRelation field help text";
            lookupRelationField.Required = true;
            lookupRelationField.Unique = true;
            lookupRelationField.Searchable = true;
            lookupRelationField.Auditable = true;
            lookupRelationField.System = true;

            lookupRelationField.RelatedEntityId = Guid.Empty;

            fields.Add(lookupRelationField);

            MasterDetailsRelationshipField masterDetailsRelationshipField = new MasterDetailsRelationshipField();

            masterDetailsRelationshipField.Id = Guid.NewGuid();
            masterDetailsRelationshipField.Name = "MasterDetailsRelationship";
            masterDetailsRelationshipField.Label = "Master Details Relationship";
            masterDetailsRelationshipField.PlaceholderText = "MasterDetailsRelationship field placeholder text";
            masterDetailsRelationshipField.Description = "MasterDetailsRelationship field description";
            masterDetailsRelationshipField.HelpText = "MasterDetailsRelationship field help text";
            masterDetailsRelationshipField.Required = true;
            masterDetailsRelationshipField.Unique = true;
            masterDetailsRelationshipField.Searchable = true;
            masterDetailsRelationshipField.Auditable = true;
            masterDetailsRelationshipField.System = true;

            masterDetailsRelationshipField.RelatedEntityId = Guid.Empty;

            fields.Add(masterDetailsRelationshipField);

            MultiLineTextField multiLineTextField = new MultiLineTextField();

            multiLineTextField.Id = Guid.NewGuid();
            multiLineTextField.Name = "MultiLineTextField";
            multiLineTextField.Label = "MultiLineText field";
            multiLineTextField.PlaceholderText = "MultiLineText field placeholder text";
            multiLineTextField.Description = "MultiLineText field description";
            multiLineTextField.HelpText = "MultiLineText field help text";
            multiLineTextField.Required = true;
            multiLineTextField.Unique = true;
            multiLineTextField.Searchable = true;
            multiLineTextField.Auditable = true;
            multiLineTextField.System = true;
            multiLineTextField.DefaultValue = "";

            multiLineTextField.MaxLength = 500;
            multiLineTextField.VisibleLineNumber = 10;

            fields.Add(multiLineTextField);

            MultiSelectField multiSelectField = new MultiSelectField();

            multiSelectField.Id = Guid.NewGuid();
            multiSelectField.Name = "MultiSelectField";
            multiSelectField.Label = "MultiSelect field";
            multiSelectField.PlaceholderText = "MultiSelect field placeholder text";
            multiSelectField.Description = "MultiSelect field description";
            multiSelectField.HelpText = "MultiSelect field help text";
            multiSelectField.Required = true;
            multiSelectField.Unique = true;
            multiSelectField.Searchable = true;
            multiSelectField.Auditable = true;
            multiSelectField.System = true;
            multiSelectField.DefaultValue = new string[] { "itemKey1", "itemKey4" };

            multiSelectField.Options = new Dictionary<string, string>();
            multiSelectField.Options.Add("itemKey1", "itemValue1");
            multiSelectField.Options.Add("itemKey2", "itemValue2");
            multiSelectField.Options.Add("itemKey3", "itemValue3");
            multiSelectField.Options.Add("itemKey4", "itemValue4");
            multiSelectField.Options.Add("itemKey5", "itemValue5");
            multiSelectField.Options.Add("itemKey6", "itemValue6");

            fields.Add(multiSelectField);

            NumberField numberField = new NumberField();

            numberField.Id = Guid.NewGuid();
            numberField.Name = "NumberField";
            numberField.Label = "Number field";
            numberField.PlaceholderText = "Number field placeholder text";
            numberField.Description = "Number field description";
            numberField.HelpText = "Number field help text";
            numberField.Required = true;
            numberField.Unique = true;
            numberField.Searchable = true;
            numberField.Auditable = true;
            numberField.System = true;
            numberField.DefaultValue = 0;

            numberField.MinValue = 1;
            numberField.MaxValue = 100;
            numberField.DecimalPlaces = 3;

            fields.Add(numberField);

            PasswordField passwordField = new PasswordField();

            passwordField.Id = Guid.NewGuid();
            passwordField.Name = "PasswordField";
            passwordField.Label = "Password field";
            passwordField.PlaceholderText = "Password field placeholder text";
            passwordField.Description = "Password field description";
            passwordField.HelpText = "Password field help text";
            passwordField.Required = true;
            passwordField.Unique = true;
            passwordField.Searchable = true;
            passwordField.Auditable = true;
            passwordField.System = true;

            passwordField.MaxLength = 1;
            passwordField.MaskType = Api.PasswordFieldMaskTypes.MaskAllCharacters;
            passwordField.MaskCharacter = '*';

            fields.Add(passwordField);

            PercentField percentField = new PercentField();

            percentField.Id = Guid.NewGuid();
            percentField.Name = "PercentField";
            percentField.Label = "Percent field";
            percentField.PlaceholderText = "Percent field";
            percentField.Description = "Percent field description";
            percentField.HelpText = "Percent field help text";
            percentField.Required = true;
            percentField.Unique = true;
            percentField.Searchable = true;
            percentField.Auditable = true;
            percentField.System = true;
            percentField.DefaultValue = 0;

            percentField.MinValue = 1;
            percentField.MaxValue = 100;
            percentField.DecimalPlaces = 3;

            fields.Add(percentField);

            PhoneField phoneField = new PhoneField();

            phoneField.Id = Guid.NewGuid();
            phoneField.Name = "PhoneField";
            phoneField.Label = "Phone field";
            phoneField.PlaceholderText = "Phone field";
            phoneField.Description = "Phone field description";
            phoneField.HelpText = "Phone field help text";
            phoneField.Required = true;
            phoneField.Unique = true;
            phoneField.Searchable = true;
            phoneField.Auditable = true;
            phoneField.System = true;
            phoneField.DefaultValue = "";

            phoneField.Format = "{0000}-{000}-{000}";
            phoneField.MaxLength = 10;

            fields.Add(phoneField);

            PrimaryKeyField primaryKeyField = new PrimaryKeyField();

            primaryKeyField.Id = Guid.NewGuid();
            primaryKeyField.Name = "PrimaryKeyField";
            primaryKeyField.Label = "PrimaryKey field";
            primaryKeyField.PlaceholderText = "PrimaryKey field placeholder text";
            primaryKeyField.Description = "PrimaryKey field description";
            primaryKeyField.HelpText = "PrimaryKey field help text";
            primaryKeyField.Required = true;
            primaryKeyField.Unique = true;
            primaryKeyField.Searchable = true;
            primaryKeyField.Auditable = true;
            primaryKeyField.System = true;
            primaryKeyField.DefaultValue = Guid.Empty;

            fields.Add(primaryKeyField);

            SelectField selectField = new SelectField();

            selectField.Id = Guid.NewGuid();
            selectField.Name = "SelectField";
            selectField.Label = "Select field";
            selectField.PlaceholderText = "Select field placeholder text";
            selectField.Description = "Select field description";
            selectField.HelpText = "Select field help text";
            selectField.Required = true;
            selectField.Unique = true;
            selectField.Searchable = true;
            selectField.Auditable = true;
            selectField.System = true;
            selectField.DefaultValue = "itemKey2";

            selectField.Options = new Dictionary<string, string>();
            selectField.Options.Add("itemKey1", "itemValue1");
            selectField.Options.Add("itemKey2", "itemValue2");
            selectField.Options.Add("itemKey3", "itemValue3");
            selectField.Options.Add("itemKey4", "itemValue4");
            selectField.Options.Add("itemKey5", "itemValue5");
            selectField.Options.Add("itemKey6", "itemValue6");

            fields.Add(selectField);

            TextField textField = new TextField();

            textField.Id = Guid.NewGuid();
            textField.Name = "TextField";
            textField.Label = "Text field";
            textField.PlaceholderText = "Text field placeholder text";
            textField.Description = "Text field description";
            textField.HelpText = "Text field help text";
            textField.Required = true;
            textField.Unique = true;
            textField.Searchable = true;
            textField.Auditable = true;
            textField.System = true;
            textField.DefaultValue = "";

            textField.MaxLength = 200;

            UrlField urlField = new UrlField();

            urlField.Id = Guid.NewGuid();
            urlField.Name = "UrlField";
            urlField.Label = "Url field";
            urlField.PlaceholderText = "Url field placeholder text";
            urlField.Description = "Url field description";
            urlField.HelpText = "Url field help text";
            urlField.Required = true;
            urlField.Unique = true;
            urlField.Searchable = true;
            urlField.Auditable = true;
            urlField.System = true;
            urlField.DefaultValue = "";

            urlField.MaxLength = 200;
            urlField.OpenTargetInNewWindow = true;

            fields.Add(urlField);

            return fields;
        }

        private List<View> CreateTestViewCollection(Entity entity)
        {
            List<View> views = new List<View>();

            View firstView = new View();

            firstView.Id = Guid.NewGuid();
            firstView.Name = "SearchPopupviewname";
            firstView.Label = "Search Popup view label";
            firstView.Type = Api.ViewTypes.SearchPopup;

            firstView.Filters = new List<ViewFilter>();

            ViewFilter filter = new ViewFilter();
            filter.EntityId = entity.Id;
            filter.FieldId = entity.Fields[1].Id.Value;
            filter.Operator = Api.FilterOperatorTypes.Equals;
            filter.Value = "false";

            firstView.Filters.Add(filter);

            firstView.Fields = new List<ViewField>();

            ViewField field1 = new ViewField();
            field1.EntityId = entity.Id;
            field1.Id = entity.Fields[3].Id.Value;
            field1.Position = 1;

            firstView.Fields.Add(field1);

            ViewField field2 = new ViewField();
            field2.EntityId = entity.Id;
            field2.Id = entity.Fields[10].Id.Value;
            field2.Position = 2;

            firstView.Fields.Add(field2);

            views.Add(firstView);
            return views;
        }

        private List<Form> CreateTestFormCollection(Entity entity)
        {
            List<Form> forms = new List<Form>();

            Form form = new Form();

            form.Id = Guid.NewGuid();
            form.Name = "FormName";
            form.Label = "Form label";

            form.Fields = new List<FormField>();

            FormField field1 = new FormField();

            field1.Id = entity.Fields[1].Id.Value;
            field1.EntityId = entity.Id;
            field1.Column = Api.FormColumns.Left;
            field1.Position = 1;

            form.Fields.Add(field1);

            FormField field2 = new FormField();

            field2.Id = entity.Fields[5].Id.Value;
            field2.EntityId = entity.Id;
            field2.Column = Api.FormColumns.Right;
            field2.Position = 2;

            form.Fields.Add(field2);

            forms.Add(form);

            return forms;
        }
    }
}