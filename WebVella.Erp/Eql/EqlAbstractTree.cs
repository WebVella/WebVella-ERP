using System.Collections.Generic;

namespace WebVella.Erp.Eql
{
	internal class EqlAbstractTree
	{
		public EqlNode RootNode { get; set; }
	}

	internal class EqlNode
	{
		public virtual EqlNodeType Type { get; }
	}

	internal class EqlKeywordNode : EqlNode
	{
		public string Keyword { get; set; }

		public override EqlNodeType Type { get { return EqlNodeType.Keyword; } }
	}

	internal class EqlNumberValueNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.NumberValue; } }

		public decimal Number { get; set; }
	}

	internal class EqlTextValueNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.TextValue; } }

		public string Text { get; set; }
	}

	internal class EqlArgumentValueNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.ArgumentValue; } }

		public string ArgumentName { get; set; }
	}

	internal class EqlSelectNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Select; } }

		public List<EqlFieldNode> Fields { get; private set; } = new List<EqlFieldNode>();

		public EqlFromNode From { get; set; } = null;

		public EqlWhereNode Where { get; set; } = null;

		public EqlOrderByNode OrderBy { get; set; } = null;

		public EqlPageNode Page { get; set; } = null;

		public EqlPageSizeNode PageSize { get; set; } = null;
	}

	internal class EqlFieldNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Field; } }

		public string FieldName { get; set; }
	}

	internal class EqlRelationInfo
	{
		public string Name { get; set; }

		public EqlRelationDirectionType Direction { get; set; }
	}

	internal class EqlRelationFieldNode : EqlFieldNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.RelationField; } }

		public List<EqlRelationInfo> Relations { get; private set; } = new List<EqlRelationInfo>();
	}

	internal class EqlRelationWildcardFieldNode : EqlRelationFieldNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.RelationWildcardField; } }
	}

	internal class EqlWildcardFieldNode : EqlFieldNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.WildcardField; } }
	}

	internal class EqlFromNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.From; } }

		public string EntityName { get; set; }
	}

	internal class EqlOrderByNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.OrderBy; } }

		public List<EqlOrderByFieldNode> Fields { get; private set; } = new List<EqlOrderByFieldNode>();
	}

	internal class EqlOrderByFieldNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.OrderByField; } }

		public string FieldName { get; set; } = null;

		public string Direction { get; set; } = null;
	}

	internal class EqlPageNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Page; } }

		public decimal? Number { get; set; } = null;
	}

	internal class EqlPageSizeNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.PageSize; } }

		public decimal? Number { get; set; } = null;
	}

	internal class EqlWhereNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Where; } }

		//public string ArgumentName { get; set; } = null;

		//public decimal? Number { get; set; } = null;

		public EqlBinaryExpressionNode RootExpressionNode { get; set; }
	}

	internal class EqlBinaryExpressionNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.BinaryExpression; } }

		public string Operator { get; set; } = null;

		public EqlNode FirstOperand { get; set; }

		public EqlNode SecondOperand { get; set; }
	}
}
