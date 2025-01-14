using System.Collections.Generic;

namespace WebVella.Erp.Eql
{
	public class EqlAbstractTree
	{
		public EqlNode RootNode { get; set; }
	}

	public class EqlNode
	{
		public virtual EqlNodeType Type { get; }
	}

	public class EqlKeywordNode : EqlNode
	{
		public string Keyword { get; set; }

		public override EqlNodeType Type { get { return EqlNodeType.Keyword; } }
	}

	public class EqlNumberValueNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.NumberValue; } }

		public decimal Number { get; set; }
	}

	public class EqlTextValueNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.TextValue; } }

		public string Text { get; set; }
	}

	public class EqlArgumentValueNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.ArgumentValue; } }

		public string ArgumentName { get; set; }
	}

	public class EqlSelectNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Select; } }

		public List<EqlFieldNode> Fields { get; private set; } = new List<EqlFieldNode>();

		public EqlFromNode From { get; set; } = null;

		public EqlWhereNode Where { get; set; } = null;

		public EqlOrderByNode OrderBy { get; set; } = null;

		public EqlPageNode Page { get; set; } = null;

		public EqlPageSizeNode PageSize { get; set; } = null;
	}

	public class EqlFieldNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Field; } }

		public string FieldName { get; set; }
	}

	public class EqlRelationInfo
	{
		public string Name { get; set; }

		public EqlRelationDirectionType Direction { get; set; }
	}

	public class EqlRelationFieldNode : EqlFieldNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.RelationField; } }

		public List<EqlRelationInfo> Relations { get; private set; } = new List<EqlRelationInfo>();
	}

	public class EqlRelationWildcardFieldNode : EqlRelationFieldNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.RelationWildcardField; } }
	}

	public class EqlWildcardFieldNode : EqlFieldNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.WildcardField; } }
	}

	public class EqlFromNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.From; } }

		public string EntityName { get; set; }
	}

	public class EqlOrderByNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.OrderBy; } }

		public List<EqlOrderByFieldNode> Fields { get; private set; } = new List<EqlOrderByFieldNode>();
	}

	public class EqlOrderByFieldNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.OrderByField; } }

		public string FieldName { get; set; } = null;

		public string Direction { get; set; } = null;
	}

	public class EqlPageNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Page; } }

		public decimal? Number { get; set; } = null;
	}

	public class EqlPageSizeNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.PageSize; } }

		public decimal? Number { get; set; } = null;
	}

	public class EqlWhereNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.Where; } }

		//public string ArgumentName { get; set; } = null;

		//public decimal? Number { get; set; } = null;

		public EqlBinaryExpressionNode RootExpressionNode { get; set; }
	}

	public class EqlBinaryExpressionNode : EqlNode
	{
		public override EqlNodeType Type { get { return EqlNodeType.BinaryExpression; } }

		public string Operator { get; set; } = null;

		public EqlNode FirstOperand { get; set; }

		public EqlNode SecondOperand { get; set; }
	}
}
