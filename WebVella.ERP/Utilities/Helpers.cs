using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Utilities
{
    public class Helpers
    {
		public static QueryResponse UpsertEntityAsAreaSubscription(EntityManager entMan, RecordManager recMan, Guid areaId, string entityName, string viewDetailsName, string viewCreateName, string listName)
		{
			#region << Init >>
			var result = new QueryResponse();
			result.Success = false;
			result.Message = "unknown error";
			var areaList = new List<EntityRecord>();
			var selectedArea = new EntityRecord();
			var areaSubscriptionsText = "";
			var selectedEntity = new Entity();
			var selectedDetailsView = new RecordView();
			var selectedCreateView = new RecordView();
			var selectedList = new RecordList();
			//Get areas
			EntityQuery query = new EntityQuery("area");
			QueryResponse response = recMan.Find(query);
			if (!response.Success || !response.Object.Data.Any())
			{
				result.Success = false;
				result.Message = response.Message;
				return result;
			}
			areaList = response.Object.Data;

			selectedArea = null;
			foreach (var area in areaList)
			{
				if ((Guid)area["id"] == areaId)
				{
					selectedArea = area;
				}
			}

			if (selectedArea == null)
			{
				result.Success = false;
				result.Message = "There is no area with id " + areaId;
				return result;
			}

			//Get entity
			EntityResponse responseEntity = entMan.ReadEntity(entityName);
			if (!responseEntity.Success || responseEntity.Object == null)
			{
				result.Success = false;
				result.Message = "There is problem getting the entity  " + entityName;
				return result;
			}
			selectedEntity = responseEntity.Object;
			//Get details view
			selectedDetailsView = null;
			foreach (var entityView in selectedEntity.RecordViews)
			{
				if (entityView.Name == viewDetailsName)
				{
					selectedDetailsView = entityView;
				}
			}
			if (selectedDetailsView == null)
			{
				result.Success = false;
				result.Message = "There is no view with name " + viewDetailsName;
				return result;
			}
			//Get create view
			selectedCreateView = null;
			foreach (var entityView in selectedEntity.RecordViews)
			{
				if (entityView.Name == viewCreateName)
				{
					selectedCreateView = entityView;
				}
			}
			if (selectedCreateView == null)
			{
				result.Success = false;
				result.Message = "There is no view with name " + viewCreateName;
				return result;
			}
			//Get list
			selectedList = null;
			foreach (var entityList in selectedEntity.RecordLists)
			{
				if (entityList.Name == listName)
				{
					selectedList = entityList;
				}
			}
			if (selectedList == null)
			{
				result.Success = false;
				result.Message = "There is no list with name " + listName;
				return result;
			}
			#endregion

			areaSubscriptionsText = (string)selectedArea["attachments"];
			var areaSubscriptionsJsonObject = new JArray();
			if(!String.IsNullOrWhiteSpace(areaSubscriptionsText)) {
				areaSubscriptionsJsonObject = JArray.Parse(areaSubscriptionsText);
			}
			var subscriptionToBeAdded = new JObject();
			//Check if there is already a subscription for this entity
			bool subscriptionFound = false;
			foreach (var areaSubscription in areaSubscriptionsJsonObject)
			{
				//Yes - updated the view and list with the supplied
				if ((string)areaSubscription["name"] == entityName)
				{
					subscriptionFound = true;
					//Update details view
					areaSubscription["view"] = new JObject();
					areaSubscription["view"]["name"] = selectedDetailsView.Name;
					areaSubscription["view"]["label"] = selectedDetailsView.Label;
					//Update create view
					areaSubscription["create"] = new JObject();
					areaSubscription["create"]["name"] = selectedCreateView.Name;
					areaSubscription["create"]["label"] = selectedCreateView.Label;
					//Update list
					areaSubscription["list"] = new JObject();
					areaSubscription["list"]["name"] = selectedList.Name;
					areaSubscription["list"]["label"] = selectedList.Label;
				}
			}
			//No - create new subscription and Add it to the list
			if (!subscriptionFound)
			{
				subscriptionToBeAdded["name"] = selectedEntity.Name;
				subscriptionToBeAdded["label"] = selectedEntity.Label;
				subscriptionToBeAdded["labelPlural"] = selectedEntity.LabelPlural;
				subscriptionToBeAdded["iconName"] = selectedList.IconName;
				subscriptionToBeAdded["weight"] = (int)selectedEntity.Weight;
				subscriptionToBeAdded["url"] = null;
				//Add details view
				subscriptionToBeAdded["view"] = new JObject();
				subscriptionToBeAdded["view"]["name"] = selectedDetailsView.Name;
				subscriptionToBeAdded["view"]["label"] = selectedDetailsView.Label;
				//Add create view
				subscriptionToBeAdded["create"] = new JObject();
				subscriptionToBeAdded["create"]["name"] = selectedCreateView.Name;
				subscriptionToBeAdded["create"]["label"] = selectedCreateView.Label;
				//Add list
				subscriptionToBeAdded["list"] = new JObject();
				subscriptionToBeAdded["list"]["name"] = selectedList.Name;
				subscriptionToBeAdded["list"]["label"] = selectedList.Label;
				areaSubscriptionsJsonObject.Add(subscriptionToBeAdded);
			}
			//Save area			
			selectedArea["attachments"] = JsonConvert.SerializeObject(areaSubscriptionsJsonObject);
			QueryResponse updateAreaResponse = recMan.UpdateRecord("area", selectedArea);
			if (!updateAreaResponse.Success)
			{
				result.Success = false;
				result.Message = "There is problem updating the area with id" + areaId;
				return result;
			}

			result.Success = true;
			result.Message = "Subscription successfully upserted";
			return result;
		}

		public static QueryResponse UpsertUrlAsAreaSubscription(EntityManager entMan, RecordManager recMan, Guid areaId, string url, string label, int weight, string iconName)
		{
			#region << Init >>
			var result = new QueryResponse();
			result.Success = false;
			result.Message = "unknown error";
			var areaList = new List<EntityRecord>();
			var selectedArea = new EntityRecord();
			var areaSubscriptionsText = "";
			var selectedEntity = new Entity();
			var selectedDetailsView = new RecordView();
			var selectedCreateView = new RecordView();
			var selectedList = new RecordList();
			//Get areas
			EntityQuery query = new EntityQuery("area");
			QueryResponse response = recMan.Find(query);
			if (!response.Success || !response.Object.Data.Any())
			{
				result.Success = false;
				result.Message = response.Message;
				return result;
			}
			areaList = response.Object.Data;

			selectedArea = null;
			foreach (var area in areaList)
			{
				if ((Guid)area["id"] == areaId)
				{
					selectedArea = area;
				}
			}

			if (selectedArea == null)
			{
				result.Success = false;
				result.Message = "There is no area with id " + areaId;
				return result;
			}

			#endregion

			areaSubscriptionsText = (string)selectedArea["attachments"];
			var areaSubscriptionsJsonObject = new JArray();
			if(!String.IsNullOrWhiteSpace(areaSubscriptionsText)) {
				areaSubscriptionsJsonObject = JArray.Parse(areaSubscriptionsText);
			}
			var subscriptionToBeAdded = new JObject();
			//Check if there is already a subscription for this entity
			bool subscriptionFound = false;
			foreach (var areaSubscription in areaSubscriptionsJsonObject)
			{
				//Yes - updated the view and list with the supplied
				if ((string)areaSubscription["url"] == url)
				{
					subscriptionFound = true;
					subscriptionToBeAdded["label"] = label;
					subscriptionToBeAdded["iconName"] = iconName;
					subscriptionToBeAdded["weight"] = (int)weight;
				}
			}
			//No - create new subscription and Add it to the list
			if (!subscriptionFound)
			{
				subscriptionToBeAdded["name"] = null;
				subscriptionToBeAdded["label"] = label;
				subscriptionToBeAdded["labelPlural"] = null;
				subscriptionToBeAdded["iconName"] = iconName;
				subscriptionToBeAdded["weight"] = (int)weight;
				subscriptionToBeAdded["url"] = url;
				//Add details view
				subscriptionToBeAdded["view"] = null;
				//Add create view
				subscriptionToBeAdded["create"] = null;
				//Add list
				subscriptionToBeAdded["list"] = null;
				areaSubscriptionsJsonObject.Add(subscriptionToBeAdded);
			}
			//Save area			
			selectedArea["attachments"] = JsonConvert.SerializeObject(areaSubscriptionsJsonObject);
			QueryResponse updateAreaResponse = recMan.UpdateRecord("area", selectedArea);
			if (!updateAreaResponse.Success)
			{
				result.Success = false;
				result.Message = "There is problem updating the area with id" + areaId;
				return result;
			}

			result.Success = true;
			result.Message = "Subscription successfully upserted";
			return result;
		}

		public static CurrencyType GetCurrencyTypeObject(string currencyCode) {
			var currencType = new CurrencyType();
			switch(currencyCode) {
				case "BGN":
					currencType.Symbol = "BGN";
					currencType.SymbolNative = "лв.";
					currencType.Name = "Bulgarian Lev";
					currencType.NamePlural = "Bulgarian leva";
					currencType.Code = "BGN";
					currencType.DecimalDigits = 2;
					currencType.Rounding = 0;
					currencType.SymbolPlacement = CurrencySymbolPlacement.After;
				break;
				case "USD":
					currencType.Symbol = "$";
					currencType.SymbolNative = "$";
					currencType.Name = "US Dollar";
					currencType.NamePlural = "US dollars";
					currencType.Code = "USD";
					currencType.DecimalDigits = 2;
					currencType.Rounding = 0;
					currencType.SymbolPlacement = CurrencySymbolPlacement.Before;
				break;
			}
			return currencType;
		}

		public static T DeepClone<T>(T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}

		public static List<ResponseTreeNode> GetTreeRecords(List<Entity> entities, List<EntityRelation> relationList, RecordTree tree)
		{
			EntityRelation relation = relationList.FirstOrDefault(r => r.Id == tree.RelationId);

			Guid treeEntityId = relation.OriginEntityId;
			Guid treeRelFieldId = relation.OriginFieldId;

			Entity treeEntity = entities.FirstOrDefault(e => e.Id == treeEntityId);
			Field treeIdField = treeEntity.Fields.FirstOrDefault(f => f.Id == treeRelFieldId);
			Field treeParrentField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeParentIdFieldId);
			Field nameField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeNameFieldId);
			Field labelField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeLabelFieldId);
			Field weightField = treeEntity.Fields.FirstOrDefault(f => f.Id == tree.NodeWeightFieldId);

			var relIdField = treeEntity.Fields.Single(x => x.Name == "id");

			List<Guid> fieldIdsToInclude = new List<Guid>();

			if (!fieldIdsToInclude.Contains(treeIdField.Id))
				fieldIdsToInclude.Add(treeIdField.Id);

			if (!fieldIdsToInclude.Contains(treeParrentField.Id))
				fieldIdsToInclude.Add(treeParrentField.Id);

			if (!fieldIdsToInclude.Contains(tree.NodeNameFieldId))
				fieldIdsToInclude.Add(tree.NodeNameFieldId);

			if (!fieldIdsToInclude.Contains(tree.NodeLabelFieldId))
				fieldIdsToInclude.Add(tree.NodeLabelFieldId);

			var weightFieldNonNullable = Guid.Empty;
			if (tree.NodeWeightFieldId.HasValue)
			{
				weightFieldNonNullable = tree.NodeWeightFieldId.Value;
			}
			if (weightField != null && !fieldIdsToInclude.Contains(weightFieldNonNullable))
				fieldIdsToInclude.Add(weightFieldNonNullable);

			string queryFields = string.Empty;
			//Add mandatory fields
			foreach (var fieldId in fieldIdsToInclude)
			{
				var f = treeEntity.Fields.SingleOrDefault(x => x.Id == fieldId);
				if (f != null)
				{
					if (!queryFields.Contains(f.Name))
						queryFields += (f.Name + ",");
				}
			}
			//Add object properties fields
			foreach (var fieldId in tree.NodeObjectProperties)
			{
				var f = treeEntity.Fields.SingleOrDefault(x => x.Id == fieldId);
				if (f != null)
				{
					if (!queryFields.Contains(f.Name))
						queryFields += (f.Name + ",");
				}
			}
			queryFields += "id";

			EntityQuery eq = new EntityQuery(treeEntity.Name, queryFields);
			RecordManager recMan = new RecordManager();
			var allRecords = recMan.Find(eq).Object.Data;

			List<ResponseTreeNode> rootNodes = new List<ResponseTreeNode>();
			foreach (var rootNode in tree.RootNodes.OrderBy(x => x.Name))
			{
				List<ResponseTreeNode> children = new List<ResponseTreeNode>();
				int? rootNodeWeight = null;
				if (weightField != null)
				{
					rootNodeWeight = rootNode.Weight;
					children = GetTreeNodeChildren(allRecords, treeIdField.Name,
									 treeParrentField.Name, nameField.Name, labelField.Name, rootNode.Id, weightField.Name, 1, tree.DepthLimit);
				}
				else
				{
					children = GetTreeNodeChildren(allRecords, treeIdField.Name,
									 treeParrentField.Name, nameField.Name, labelField.Name, rootNode.Id, "no-weight", 1, tree.DepthLimit);
				}
				rootNodes.Add(new ResponseTreeNode
				{
					RecordId = rootNode.RecordId,
					Id = rootNode.Id.Value,
					ParentId = rootNode.ParentId,
					Name = rootNode.Name,
					Label = rootNode.Label,
					Weight = rootNodeWeight,
					Nodes = children,
					Object = allRecords.SingleOrDefault(x => (Guid)x["id"] == rootNode.RecordId)
				});

			}

			return rootNodes;
		}

		public static List<ResponseTreeNode> GetTreeNodeChildren(string entityName, string fields, string idFieldName, string parentIdFieldName,
				string nameFieldName, string labelFieldName, Guid? nodeId, string weightFieldName = "no-weight", int depth = 1, int maxDepth = 20)
		{
			if (depth >= maxDepth)
				return new List<ResponseTreeNode>();

			var query = EntityQuery.QueryEQ(parentIdFieldName, nodeId);
			EntityQuery eq = new EntityQuery(entityName, fields, query);
			RecordManager recMan = new RecordManager();
			var records = recMan.Find(eq).Object.Data;
			List<ResponseTreeNode> nodes = new List<ResponseTreeNode>();
			depth++;
			foreach (var record in records)
			{
				if (weightFieldName == "no-weight")
				{
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = null,
						Nodes = GetTreeNodeChildren(entityName, fields, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth),
						Object = records.SingleOrDefault(x => (Guid)x["id"] == (Guid)record["id"])
					});
				}
				else
				{
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = (int?)((decimal?)record[weightFieldName]),
						Nodes = GetTreeNodeChildren(entityName, fields, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth),
						Object = records.SingleOrDefault(x => (Guid)x["id"] == (Guid)record["id"])
					});
				}
			}
			if (weightFieldName == "no-weight")
			{
				return nodes.OrderBy(x => x.Name).ToList();
			}
			else
			{
				return nodes.OrderBy(x => x.Weight).ThenBy(y => y.Name).ToList();
			}
		}

		public static List<ResponseTreeNode> GetTreeNodeChildren(List<EntityRecord> allRecords, string idFieldName, string parentIdFieldName,
				string nameFieldName, string labelFieldName, Guid? nodeId, string weightFieldName = "no-weight", int depth = 1, int maxDepth = 20)
		{
			if (depth >= maxDepth)
				return new List<ResponseTreeNode>();

			var records = allRecords.Where(x => (Guid?)x[parentIdFieldName] == nodeId).ToList();
			List<ResponseTreeNode> nodes = new List<ResponseTreeNode>();
			depth++;
			foreach (var record in records)
			{
				if (weightFieldName == "no-weight")
				{
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = null,
						Nodes = GetTreeNodeChildren(allRecords, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth),
						Object = allRecords.SingleOrDefault(x => (Guid)x["id"] == (Guid)record["id"])
					});
				}
				else
				{
					nodes.Add(new ResponseTreeNode
					{
						RecordId = (Guid)record["id"],
						Id = (Guid)record[idFieldName],
						ParentId = (Guid?)record[parentIdFieldName],
						Name = record[nameFieldName]?.ToString(),
						Label = record[labelFieldName]?.ToString(),
						Weight = (int?)((decimal?)record[weightFieldName]),
						Nodes = GetTreeNodeChildren(allRecords, idFieldName, parentIdFieldName, nameFieldName, labelFieldName, (Guid)record[idFieldName], weightFieldName, depth, maxDepth),
						Object = allRecords.SingleOrDefault(x => (Guid)x["id"] == (Guid)record["id"])
					});
				}
			}

			if (weightFieldName == "no-weight")
			{
				return nodes.OrderBy(x => x.Name).ToList();
			}
			else
			{
				return nodes.OrderBy(x => x.Weight).ThenBy(y => y.Name).ToList();
			}
		}

		public static EntityRecord FixDoubleDollarSignProblem(EntityRecord record) {
			var keysForRemoval = new List<string>();
			var recordKeyList = new List<string>();

			foreach(var property in record.Properties) {
				recordKeyList.Add(property.Key);
			}

			//in angular properties starting with $$ are not posted by the $http service, 
			foreach(var key in recordKeyList) {
				if(key.StartsWith("_$")) {
					var newKey = "$$" + key.Remove(0,2);
					record[newKey] = record[key];
					keysForRemoval.Add(key);
				}
			}

			foreach (var key in keysForRemoval)
			{
				record.Properties.Remove(key);
			}
			return record;
		}

		public static EntityRecord GetImageDimension(byte[] imageContent)
		{
			Stream stream = new MemoryStream(imageContent);
			System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
			var response = new EntityRecord();
			response["height"] = (decimal)image.Height;
			response["width"] = (decimal)image.Width;
			return response;
		}

	}
}
