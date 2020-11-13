using System;
using NpgsqlTypes;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Database
{
	public static class DbTypeConverter
	{
		public static string ConvertToDatabaseSqlType(FieldType type)
		{
			string pgType = "";

			switch (type)
			{
				case FieldType.AutoNumberField:
					pgType = "serial";
					break;
				case FieldType.CheckboxField:
					pgType = "boolean";
					break;
				case FieldType.CurrencyField:
					pgType = "numeric";
					break;
				case FieldType.DateField:
					pgType = "date";
					break;
				case FieldType.DateTimeField:
					pgType = "timestamptz";
					break;
				case FieldType.EmailField:
					pgType = "varchar(500)";
					break;
				case FieldType.FileField:
					pgType = "varchar(1000)";
					break;
				case FieldType.GuidField:
					pgType = "uuid";
					break;
				case FieldType.HtmlField:
					pgType = "text";
					break;
				case FieldType.ImageField:
					pgType = "varchar(1000)";
					break;
				case FieldType.MultiLineTextField:
					pgType = "text";
					break;
				case FieldType.GeographyField:
					pgType = "geography";
					break;
				case FieldType.MultiSelectField:
					pgType = "text[]";
					break;
				case FieldType.NumberField:
					pgType = "numeric";
					break;
				case FieldType.PasswordField:
					pgType = "varchar(500)";
					break;
				case FieldType.PercentField:
					pgType = "numeric";
					break;
				case FieldType.PhoneField:
					pgType = "varchar(100)";
					break;
				case FieldType.SelectField:
					pgType = "varchar(200)";
					break;
				case FieldType.TextField:
					pgType = "text";
					break;
				case FieldType.UrlField:
					pgType = "varchar(1000)";
					break;
				default:
					throw new Exception("FieldType is not supported.");
			}

			return pgType;
		}

		public static NpgsqlDbType ConvertToDatabaseType(FieldType type)
		{
			NpgsqlDbType pgType = NpgsqlDbType.Numeric;

			switch (type)
			{
				case FieldType.AutoNumberField:
					pgType = NpgsqlDbType.Numeric;
					break;
				case FieldType.CheckboxField:
					pgType = NpgsqlDbType.Boolean;
					break;
				case FieldType.CurrencyField:
					pgType = NpgsqlDbType.Numeric;
					break;
				case FieldType.DateField:
					pgType = NpgsqlDbType.Date;
					break;
				case FieldType.DateTimeField:
					pgType = NpgsqlDbType.TimestampTz;
					break;
				case FieldType.EmailField:
					pgType = NpgsqlDbType.Varchar;
					break;
				case FieldType.FileField:
					pgType = NpgsqlDbType.Varchar;
					break;
				case FieldType.GuidField:
					pgType = NpgsqlDbType.Uuid;
					break;
				case FieldType.HtmlField:
					pgType = NpgsqlDbType.Text;
					break;
				case FieldType.ImageField:
					pgType = NpgsqlDbType.Varchar;
					break;
				case FieldType.MultiLineTextField:
					pgType = NpgsqlDbType.Text;
					break;
				case FieldType.GeographyField:
					pgType = NpgsqlDbType.Geography;
					break;
				case FieldType.MultiSelectField:
					pgType = NpgsqlDbType.Array | NpgsqlDbType.Text;
					break;
				case FieldType.NumberField:
					pgType = NpgsqlDbType.Numeric;
					break;
				case FieldType.PasswordField:
					pgType = NpgsqlDbType.Varchar;
					break;
				case FieldType.PercentField:
					pgType = NpgsqlDbType.Numeric;
					break;
				case FieldType.PhoneField:
					pgType = NpgsqlDbType.Varchar;
					break;
				case FieldType.SelectField:
					pgType = NpgsqlDbType.Varchar;
					break;
				case FieldType.TextField:
					pgType = NpgsqlDbType.Text;
					break;
				case FieldType.UrlField:
					pgType = NpgsqlDbType.Varchar;
					break;
				default:
					throw new Exception("FieldType is not supported.");
			}

			return pgType;
		}

		public static NpgsqlDbType GetDatabaseType(Field field)
		{
			FieldType type = field.GetFieldType();
			return ConvertToDatabaseType(type);
		}

		public static NpgsqlDbType GetDatabaseFieldType(DbBaseField field)
		{
			NpgsqlDbType pgType = NpgsqlDbType.Numeric;

			if (field is DbAutoNumberField)
				pgType = NpgsqlDbType.Numeric;
			else if (field is DbCheckboxField)
				pgType = NpgsqlDbType.Boolean;
			else if (field is DbCurrencyField)
				pgType = NpgsqlDbType.Numeric;
			else if (field is DbDateField)
				pgType = NpgsqlDbType.Date;
			else if (field is DbDateTimeField)
				pgType = NpgsqlDbType.TimestampTz;
			else if (field is DbEmailField)
				pgType = NpgsqlDbType.Varchar;
			else if (field is DbFileField)
				pgType = NpgsqlDbType.Varchar;
			else if (field is DbGuidField)
				pgType = NpgsqlDbType.Uuid;
			else if (field is DbHtmlField)
				pgType = NpgsqlDbType.Text;
			else if (field is DbImageField)
				pgType = NpgsqlDbType.Varchar;
			else if (field is DbMultiLineTextField)
				pgType = NpgsqlDbType.Text;
			else if (field is DbMultiSelectField)
				pgType = NpgsqlDbType.Array | NpgsqlDbType.Text;
			else if (field is DbNumberField)
				pgType = NpgsqlDbType.Numeric;
			else if (field is DbPasswordField)
				pgType = NpgsqlDbType.Varchar;
			else if (field is DbPercentField)
				pgType = NpgsqlDbType.Numeric;
			else if (field is DbPhoneField)
				pgType = NpgsqlDbType.Varchar;
			else if (field is DbSelectField)
				pgType = NpgsqlDbType.Varchar;
			else if (field is DbTextField)
				pgType = NpgsqlDbType.Text;
			else if (field is DbTreeSelectField)
				pgType = NpgsqlDbType.Array | NpgsqlDbType.Uuid;
			else if (field is DbUrlField)
				pgType = NpgsqlDbType.Varchar;
			else
				throw new Exception("FieldType is not supported.");

			return pgType;
		}
	}
}
