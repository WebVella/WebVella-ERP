using Irony.Parsing;
using System;

namespace WebVella.Erp.Eql
{
	[Language("EntityQL")]
	internal class EqlGrammar : Grammar
	{
		public EqlGrammar() : base(false)
		{
			//terminals
			NonGrammarTerminals.Add(new CommentTerminal("COMMENT", "/*", "*/"));
			NonGrammarTerminals.Add(new CommentTerminal("LINE_COMMENT", "--", "\n", "\r\n"));
			var NUMBER = new NumberLiteral("NUMBER");
			var STRING = new StringLiteral("STRING", "'", StringOptions.AllowsDoubledQuote);
			var ARGUMENT = new IdentifierTerminal("ARGUMENT");
			var IDENTIFIER = new IdentifierTerminal("IDENTIFIER");
			var COMMA = ToTerm(",");
			var DOT = ToTerm(".");
			var NULL = ToTerm("NULL");
			var TRUE = ToTerm("TRUE");
			var FALSE = ToTerm("FALSE");
			var SELECT = ToTerm("SELECT");
			var FROM = ToTerm("FROM");
			var WHERE = ToTerm("WHERE");
			var ORDER = ToTerm("ORDER");
			var BY = ToTerm("BY");
			var PAGE = ToTerm("PAGE");
			var PAGESIZE = ToTerm("PAGESIZE");
			var ASC = ToTerm("ASC");
			var DESC = ToTerm("DESC");

			//non-terminals
			var identifier = new NonTerminal("identifier");
			var expressionIdentifier = new NonTerminal("expression_identifier");
			var argument = new NonTerminal("argument");
			var statement = new NonTerminal("statement");
			var selectStmt = new NonTerminal("select_statement");
			var identifierList = new NonTerminal("identifier_list");
			var orderList = new NonTerminal("order_list");
			var orderMember = new NonTerminal("order_member");
			var orderDirOpt = new NonTerminal("order_direction_optional");
			var expression = new NonTerminal("expression");
			var exprList = new NonTerminal("expression_list");
			var selList = new NonTerminal("select_list");
			var countClause = new NonTerminal("count_clause");
			var fromClause = new NonTerminal("from_clause");
			var orderClauseOpt = new NonTerminal("order_clause_optional");
			var columnItemList = new NonTerminal("column_item_list");
			var columnItem = new NonTerminal("column_item");
			var columnSource = new NonTerminal("column_source");
			var columnRelation = new NonTerminal("column_relation");
			var columnRelationList = new NonTerminal("column_relation_list");
			var whereClauseOpt = new NonTerminal("where_clause_optional");
			var pageClauseOpt = new NonTerminal("page_clause_optional");
			var pageSizeClauseOpt = new NonTerminal("pagesize_clause_optional");
			var term = new NonTerminal("term");
			var tuple = new NonTerminal("tuple");
			var binExpr = new NonTerminal("binary_expression");
			var binOp = new NonTerminal("binary_operator");
			var stmtRoot = new NonTerminal("root");


			//BNF root
			this.Root = stmtRoot;
			stmtRoot.Rule = selectStmt;

			//identifier
			identifier.Rule = IDENTIFIER; //MakePlusRule(identifier, DOT, IDENTIFIER);
			statement.Rule = selectStmt;
			identifierList.Rule = MakePlusRule(identifierList, COMMA, identifier);

			//argument
			argument.Rule = ToTerm("@") + ARGUMENT;

			//order
			orderList.Rule = MakePlusRule(orderList, COMMA, orderMember);
			orderMember.Rule = identifier + orderDirOpt | argument + orderDirOpt;
			orderDirOpt.Rule = Empty | ASC | DESC | argument;

			//select statement
			selectStmt.Rule = SELECT + columnItemList + fromClause + whereClauseOpt + orderClauseOpt + pageClauseOpt + pageSizeClauseOpt;
			selList.Rule = columnItemList;

			columnItemList.Rule = MakePlusRule(columnItemList, COMMA, columnItem);
			columnItem.Rule = columnSource;
			columnSource.Rule = identifier | "*" | columnRelationList + DOT + identifier | columnRelationList + DOT + "*";
			columnRelation.Rule = "$" + identifier | "$$" + identifier;
			columnRelationList.Rule = MakePlusRule(columnRelationList, DOT, columnRelation);

			fromClause.Rule = FROM + identifier;
			whereClauseOpt.Rule = Empty | WHERE + expression;
			orderClauseOpt.Rule = Empty | ORDER + BY + orderList;
			pageClauseOpt.Rule = Empty | PAGE + NUMBER | PAGE + argument;
			pageSizeClauseOpt.Rule = Empty | PAGESIZE + NUMBER | PAGESIZE + argument;

			//expression
			exprList.Rule = MakePlusRule(exprList, COMMA, expression);
			expression.Rule = term | binExpr;
			term.Rule = expressionIdentifier | argument | tuple | NUMBER | STRING | NULL | TRUE | FALSE;
			expressionIdentifier.Rule = identifier | columnRelation + DOT + identifier;
			tuple.Rule = "(" + exprList + ")";
			binExpr.Rule = expression + binOp + expression;
			binOp.Rule = ToTerm("=") | ">" | "<" | ">=" | "<=" | "<>" | "!=" | //compare operators
					  "AND" | "OR" | //logical operators
					  "CONTAINS" | "STARTSWITH" | // text operators
					  "~" | "~*" | "!~" | "!~*" |  //regex operators
					  "@@"; // fts operator

			//operators precedence
			RegisterOperators(8, "=", ">", "<", ">=", "<=", "<>", "CONTAINS", "STARTSWITH", "~", "~*", "!~", "!~*", "@@");
			RegisterOperators(5, "AND");
			RegisterOperators(4, "OR");

			MarkPunctuation(",", "(", ")");

			base.MarkTransient(statement, expression, tuple);
			binOp.SetFlag(TermFlags.InheritPrecedence);
		}
	}
}
