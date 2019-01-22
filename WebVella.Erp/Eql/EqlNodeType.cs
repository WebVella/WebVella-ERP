namespace WebVella.Erp.Eql
{
	internal enum EqlNodeType
	{
		Keyword,
		NumberValue,
		TextValue,
		ArgumentValue,
		Select,
		Field,
		RelationField,
		RelationWildcardField,
		WildcardField,
		From,
		Where,
		BinaryExpression,
		OrderBy,
		OrderByField,
		Page,
		PageSize,
	}
}
