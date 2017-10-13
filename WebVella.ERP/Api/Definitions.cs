using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api
{
	public class SystemIds
	{
		public static Guid SystemEntityId { get { return new Guid("a5050ac8-5967-4ce1-95e7-a79b054f9d14"); } }
		public static Guid UserEntityId { get { return new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"); } }
		public static Guid RoleEntityId { get { return new Guid("c4541fee-fbb6-4661-929e-1724adec285a"); } }
		public static Guid AreaEntityId = new Guid("cb434298-8583-4a96-bdbb-97b2c1764192");

		public static Guid UserRoleRelationId { get { return new Guid("0C4B119E-1D7B-4B40-8D2C-9E447CC656AB"); } }

		public static Guid AdministratorRoleId { get { return new Guid("BDC56420-CAF0-4030-8A0E-D264938E0CDA"); } }
		public static Guid RegularRoleId { get { return new Guid("F16EC6DB-626D-4C27-8DE0-3E7CE542C55F"); } }
		public static Guid GuestRoleId { get { return new Guid("987148B1-AFA8-4B33-8616-55861E5FD065"); } }

		public static Guid SystemUserId { get { return new Guid("10000000-0000-0000-0000-000000000000"); } }
		public static Guid FirstUserId { get { return new Guid("EABD66FD-8DE1-4D79-9674-447EE89921C2"); } }
	}


	public class SystemWebHookNames
	{
		public const string CreateRecordAction = "create_record_success_action";
		public const string CreateRecordInput = "create_record_input_filter";
		public const string CreateRecordValidationErrors = "create_record_validation_errors_filter";
		public const string CreateRecordPreSave = "create_record_pre_save_filter";

		public const string GetRecordAction = "get_record_success_action";
		public const string GetRecordInput = "get_record_input_filter";
		public const string GetRecordOutput = "get_record_output_filter";

		public const string UpdateRecordAction = "update_record_success_action";
		public const string UpdateRecordInput = "update_record_input_filter";
		public const string UpdateRecordValidationErrors = "update_record_validation_errors_filter";
		public const string UpdateRecordPreSave = "update_record_pre_save_filter";

		//Intentionally now patch events are equal to update events
		public const string PatchRecordAction = "update_record_success_action";
		public const string PatchRecordInput = "update_record_input_filter";
		public const string PatchRecordValidationErrors = "update_record_validation_errors_filter";
		public const string PatchRecordPreSave = "update_record_pre_save_filter";

		public const string DeleteRecordAction = "delete_record_success_action";
		public const string DeleteRecordInput = "delete_record_input_filter";
		public const string DeleteRecordValidationErrors = "delete_record_validation_errors_filter";
		public const string DeleteRecordPreSave = "delete_record_pre_save_filter";

		public const string ManageRelationAction = "manage_relation_success_action";
		public const string ManageRelationInput = "manage_relation_input_filter";
		public const string ManageRelationPreSave = "manage_relation_pre_save_filter";
	}

	public enum RecordsListTypes
	{
		SearchPopup = 1,
		List,
		Custom
	}

	public enum FilterOperatorTypes
	{
		Equals = 1,
		NotEqualTo,
		StartsWith,
		Contains,
		DoesNotContain,
		LessThan,
		GreaterThan,
		LessOrEqual,
		GreaterOrEqual,
		Includes,
		Excludes,
		Within
	}

	public enum RecordViewLayouts
	{
		OneColumn = 1,
		TwoColumns
	}

	public enum RecordViewColumns
	{
		Left = 1,
		Right
	}

	public enum CurrencySymbolPlacement
	{
		Before = 1,
		After
	}

	[Serializable]
	public class CurrencyType
	{
		[JsonProperty(PropertyName = "symbol")]
		public string Symbol { get; set; }

		[JsonProperty(PropertyName = "symbolNative")]
		public string SymbolNative { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "namePlural")]
		public string NamePlural { get; set; }

		[JsonProperty(PropertyName = "code")]
		public string Code { get; set; }

		[JsonProperty(PropertyName = "decimalDigits")]
		public int DecimalDigits { get; set; }

		[JsonProperty(PropertyName = "rounding")]
		public int Rounding { get; set; }

		[JsonProperty(PropertyName = "symbolPlacement")]
		public CurrencySymbolPlacement SymbolPlacement { get; set; }
	}

	public enum FormulaFieldReturnType
	{
		Checkbox = 1,
		Currency,
		Date,
		DateTime,
		Number,
		Percent,
		Text
	}

	public enum EntityPermission
	{
		Read,
		Create,
		Update,
		Delete
	}

}