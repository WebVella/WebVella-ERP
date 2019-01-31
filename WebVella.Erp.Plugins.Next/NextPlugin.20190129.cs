using Newtonsoft.Json;
using System;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Next
{
	public partial class NextPlugin : ErpPlugin
	{
		private static void Patch20190129(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Create relation*** Relation name: user_nn_task_watchers >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("879b49cc-6af6-4b34-a554-761ec992534d");
				relation.Name = "user_nn_task_watchers";
				relation.Label = "user_nn_task_watchers";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.ManyToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: user_nn_task_watchers Create. Message:" + response.Message);
				}
			}
			#endregion
		}
	}
}
