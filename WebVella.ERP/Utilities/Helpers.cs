using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
				subscriptionToBeAdded["weight"] = selectedEntity.Weight;
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
					subscriptionToBeAdded["weight"] = weight;
				}
			}
			//No - create new subscription and Add it to the list
			if (!subscriptionFound)
			{
				subscriptionToBeAdded["name"] = null;
				subscriptionToBeAdded["label"] = label;
				subscriptionToBeAdded["labelPlural"] = null;
				subscriptionToBeAdded["iconName"] = iconName;
				subscriptionToBeAdded["weight"] = weight;
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
    }
}
