using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;

namespace WebVella.Erp.ConsoleApp
{
	[HookAttachment("user")]
	public class UserRecordHooks : IErpPreSearchRecordHook
	{
		public void OnPreSearchRecord(string entityName, EqlSelectNode tree, List<EqlError> errors)
		{
			//this filter is only for user entity and should filter records only to current user
			if (entityName != "user")
				return;

			var currentUserId = WebVella.Erp.Api.SecurityContext.CurrentUser.Id;

			// id = currentUserId  
			var userFilterCondition = new EqlBinaryExpressionNode
			{
				Operator = "=",
				FirstOperand = new EqlFieldNode
				{
					FieldName = "id"
				},
				SecondOperand = new EqlTextValueNode
				{
					Text = currentUserId.ToString()
				}
			};

			try
			{
				var whereNode = tree.Where;

				if (whereNode == null)
				{
					tree.Where = new EqlWhereNode
					{
						RootExpressionNode = userFilterCondition
					};
				}
				else
				{
					if (IsConditionDuplicate(whereNode.RootExpressionNode, userFilterCondition))
					{
						return;
					}

					whereNode.RootExpressionNode = new EqlBinaryExpressionNode
					{
						Operator = "AND",
						FirstOperand = whereNode.RootExpressionNode,
						SecondOperand = userFilterCondition
					};
				}
			}
			catch (Exception ex)
			{
				errors.Add(new EqlError
				{
					Message = $"Error while adding filter condition: {ex.Message}",
					Line = 91,
				});
			}
		}
		private bool IsConditionDuplicate(EqlNode existingCondition, EqlBinaryExpressionNode newCondition)
		{
			if (existingCondition is EqlBinaryExpressionNode binaryNode)
			{
				if (binaryNode.Operator == newCondition.Operator &&
					binaryNode.FirstOperand is EqlFieldNode existingField &&
					newCondition.FirstOperand is EqlFieldNode newField &&
					existingField.FieldName == newField.FieldName &&
					binaryNode.SecondOperand is EqlTextValueNode existingValue &&
					newCondition.SecondOperand is EqlTextValueNode newValue &&
					existingValue.Text == newValue.Text)
				{
					return true;
				}

				if (binaryNode.Operator == "AND" || binaryNode.Operator == "OR")
				{
					return IsConditionDuplicate(binaryNode.FirstOperand, newCondition) ||
						   IsConditionDuplicate(binaryNode.SecondOperand, newCondition);
				}
			}

			return false;
		}
	}
}
