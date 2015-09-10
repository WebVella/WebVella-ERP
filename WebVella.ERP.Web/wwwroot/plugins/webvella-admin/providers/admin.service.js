/* admin.service.js */

/**
* @desc all actions with site area
*/

function guid() {
	function s4() {
		return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
	}
	return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}


(function () {
	'use strict';

	angular
        .module('webvellaAdmin')
        .service('webvellaAdminService', service);

	service.$inject = ['$log', '$http', '$rootScope', 'wvAppConstants', 'Upload', 'ngToast'];



	/* @ngInject */
	function service($log, $http, $rootScope, wvAppConstants, Upload, ngToast) {
		var serviceInstance = this;

		//create a plug point in the rootScope
		$rootScope.webvellaAdmin = {};

		//#region << Include functions >>
		serviceInstance.getMetaEntityList = getMetaEntityList;
		serviceInstance.initEntity = initEntity;
		serviceInstance.createEntity = createEntity;
		serviceInstance.patchEntity = patchEntity;
		serviceInstance.getEntityMeta = getEntityMeta;
		serviceInstance.deleteEntity = deleteEntity;
		serviceInstance.initField = initField;
		serviceInstance.createField = createField;
		serviceInstance.updateField = updateField;
		serviceInstance.deleteField = deleteField;
		serviceInstance.initRelation = initRelation;
		serviceInstance.getRelationByName = getRelationByName;
		serviceInstance.getRelationsList = getRelationsList;
		serviceInstance.createRelation = createRelation;
		serviceInstance.updateRelation = updateRelation;
		serviceInstance.deleteRelation = deleteRelation;
		//View
		serviceInstance.initView = initView;
		serviceInstance.initViewSection = initViewSection;
		serviceInstance.initViewRow = initViewRow;
		serviceInstance.initViewRowColumn = initViewRowColumn;
		serviceInstance.getEntityView = getEntityView;
		serviceInstance.createEntityView = createEntityView;
		serviceInstance.updateEntityView = updateEntityView;
		serviceInstance.deleteEntityView = deleteEntityView;
		serviceInstance.patchEntityView = patchEntityView;
		serviceInstance.safeAddArrayPlace = safeAddArrayPlace;
		serviceInstance.safeUpdateArrayPlace = safeUpdateArrayPlace;
		serviceInstance.safeRemoveArrayPlace = safeRemoveArrayPlace;
		serviceInstance.getEntityViewList = getEntityViewList;
		serviceInstance.getEntityViewLibrary = getEntityViewLibrary;
		//List
		serviceInstance.initList = initList;
		serviceInstance.getEntityLists = getEntityLists;
		serviceInstance.getEntityList = getEntityList;
		serviceInstance.createEntityList = createEntityList;
		serviceInstance.patchEntityList = patchEntityList;
		serviceInstance.deleteEntityList = deleteEntityList;
		//Record
		serviceInstance.getRecordsByEntityName = getRecordsByEntityName;
		serviceInstance.createRecord = createRecord;
		serviceInstance.updateRecord = updateRecord;
		serviceInstance.patchRecord = patchRecord;
		serviceInstance.patchRecordDefault = patchRecordDefault;
		serviceInstance.uploadFileToTemp = uploadFileToTemp;
		serviceInstance.moveFileFromTempToFS = moveFileFromTempToFS;
		serviceInstance.deleteFileFromFS = deleteFileFromFS;
		serviceInstance.manageRecordsRelation = manageRecordsRelation;
		//Area
		serviceInstance.initArea = initArea;
		serviceInstance.getAreaByName = getAreaByName;
		serviceInstance.deleteArea = deleteArea;
		serviceInstance.getAreaRelationByEntityId = getAreaRelationByEntityId;
		serviceInstance.createAreaEntityRelation = createAreaEntityRelation;
		serviceInstance.removeAreaEntityRelation = removeAreaEntityRelation;
		//Function
		serviceInstance.getItemsFromRegion = getItemsFromRegion;

		//#endregion

		//#region << Aux methods >>

		// Global functions for result handling for all methods of this service
		function handleErrorResult(data, status, errorCallback) {
			switch (status) {
				case 400:
					if (errorCallback === undefined || typeof (errorCallback) != "function") {
						$log.debug('webvellaAdmin>providers>admin.service> result failure: errorCallback not a function or missing ');
						alert("The errorCallback argument is not a function or missing");
						return;
					}
					data.success = false;
					var messageString = '<div><span class="go-red">Error:</span> ' + data.message + '</div>';
					if (data.errors.length > 0) {
						messageString += '<ul>';
						for (var i = 0; i < data.errors.length; i++) {
							messageString += '<li>' + data.errors[i].message + '</li>';
						}
						messageString += '</ul>';
					}
					ngToast.create({
						className: 'error',
						content: messageString
					});
					errorCallback(data);
					break;
				default:
					$log.debug('webvellaAdmin>providers>admin.service> result failure: API call finished with error: ' + status);
					alert("An API call finished with error: " + status);
					break;
			}
		}

		function handleSuccessResult(data, status, successCallback, errorCallback) {
			if (successCallback === undefined || typeof (successCallback) != "function") {
				$log.debug('webvellaAdmin>providers>admin.service> result failure: successCallback not a function or missing ');
				alert("The successCallback argument is not a function or missing");
				return;
			}

			if (!data.success) {
				//when the validation errors occurred
				if (errorCallback === undefined || typeof (errorCallback) != "function") {
					$log.debug('webvellaAdmin>providers>admin.service> result failure: errorCallback not a function or missing ');
					alert("The errorCallback argument in handleSuccessResult is not a function or missing");
					return;
				}
				errorCallback(data);
			}
			else {
				$log.debug('webvellaAdmin>providers>admin.service> result success: get object ');
				successCallback(data);
			}
		}
		//#endregion

		//#region << Entity >>

		///////////////////////
		function getMetaEntityList(successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function initEntity() {
			$log.debug('webvellaAdmin>providers>admin.service>initEntity> function called');
			var entity = {
				id: null,
				name: "",
				label: "",
				labelPlural: "",
				system: false,
				iconName: "database",
				weight: 1.0,
				recordPermissions: {
					canRead: [],
					canCreate: [],
					canUpdate: [],
					canDelete: []
				}
			};
			return entity;
		}


		///////////////////////
		function createEntity(postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createEntity> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function getEntityMeta(name, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityMeta> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + name }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function patchEntity(entityId, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchEntity> function called');
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId, data: patchObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function deleteEntity(entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteEntity> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		//#endregion << Entity >>

		//#region << Field >>
		///////////////////////
		function initField(typeId) {
			$log.debug('webvellaAdmin>providers>admin.service>initField> function called');
			var field = {
				id: null,
				name: "",
				label: "",
				placeholderText: "",
				description: "",
				helpText: "",
				required: false,
				unique: false,
				searchable: false,
				auditable: false,
				system: false,
				fieldType: typeId,
			};

			switch (typeId) {
				case 1:
					field.defaultValue = null;
					field.startingNumber = 1;
					field.displayFormat = null;
					break;
				case 2:
					field.defaultValue = false;
					break;
				case 3:
					field.defaultValue = null;
					field.minValue = null;
					field.maxValue = null;
					field.currency = {
						currencySymbol: null,
						currencyName: null,
						position: 0
					};
					break;
				case 4:
					field.defaultValue = null;
					field.format = "dd MMM yyyy";
					field.useCurrentTimeAsDefaultValue = false;
					break;
				case 5:
					field.defaultValue = null;
					field.format = "dd MMM yyyy HH:mm";
					field.useCurrentTimeAsDefaultValue = false;
					break;
				case 6:
					field.defaultValue = null;
					field.maxLength = null;
					break;
				case 7:
					field.defaultValue = null;
					break;
				case 8:
					field.defaultValue = null;
					break;
				case 9:
					field.defaultValue = null;
					break;
				case 10:
					field.defaultValue = null;
					field.maxLength = null;
					//field.visibleLineNumber = false;
					break;
				case 11:
					field.defaultValue = null;
					field.options = null;
					break;
				case 12:
					field.defaultValue = null;
					field.minValue = null;
					field.maxValue = null;
					field.decimalPlaces = 2;
					break;
				case 13:
					field.maxLength = null;
					field.encrypted = true;
					field.maskType = 1;
					break;
				case 14:
					field.defaultValue = null;
					field.minValue = null;
					field.maxValue = null;
					field.decimalPlaces = 2;
					break;
				case 15:
					field.defaultValue = null;
					field.format = null;
					field.maxLength = null;
					break;
				case 16:
					field.defaultValue = null;
					field.generateNewId = false;
					break;
				case 17:
					field.defaultValue = null;
					field.options = null;
					break;
				case 18:
					field.defaultValue = null;
					field.maxLength = null;
					break;
				case 19:
					field.defaultValue = null;
					field.maxLength = null;
					field.openTargetInNewWindow = false;
					break;
				default:
					break;
			}

			return field;
		}

		///////////////////////
		function createField(postObject, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createField> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function updateField(putObject, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateField> function called');
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + putObject.id, data: putObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function deleteField(fieldId, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateField> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + fieldId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		//#endregion

		//#region << Relations  >>
		///////////////////////
		function initRelation() {
			$log.debug('webvellaAdmin>providers>admin.service>initRelation> function called');
			var relation = {
				id: null,
				name: "",
				label: "",
				description: "",
				system: false,
				relationType: 1,
				originEntityId: null,
				originFieldId: null,
				targetEntityId: null,
				targetFieldId: null,

			};
			return relation;
		}


		///////////////////////
		function getRelationByName(name, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getRelationByName> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + name }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function getRelationsList(successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getRelationsList> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function createRelation(postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createRelation> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/relation', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function updateRelation(postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateRelation> function called');
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + postObject.id, data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function deleteRelation(relationId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateRelation> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + relationId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		//#endregion

		//#region << Entity Views >>

		///////////////////////
		function initView() {
			$log.debug('webvellaAdmin>providers>admin.service>initView> function called');
			var view = {
				"id": null,
				"name": "",
				"label": "",
				"default": false,
				"system": false,
				"weight": 1,
				"cssClass": "",
				"type": "general",
				"regions": [
                    {
                    	"name": "content",
                    	"render": false,
                    	"cssClass": "",
                    	"sections": []
                    }
				],
				"sidebar": {
					"render": false,
					"cssClass": "",
					"items": []
				}
			};
			return view;
		}
		////////////////////////
		function initViewSection() {
			var section = {
				"id": guid(),
				"name": "section",
				"label": "Section",
				"cssClass": "go-vertical",
				"showLabel": true,
				"collapsed": false,
				"weight": 1,
				"tabOrder": "left-right",
				"rows": []
			}
			return section;
		}
		///////////////////////
		function initViewRow(columnCount) {
			var row = {
				"id": guid(),
				"weight": 1,
				"columns": []
			}


			if (columnCount == 0 || columnCount == 5 || columnCount > 12 || (7 <= columnCount && columnCount <= 11)) {
				columnCount = 1;
			}

			for (var i = 0; i < columnCount; i++) {
				var column = {
					"gridColCount": 12 / columnCount,
					"items": []
				}
				row.columns.push(column);
			}


			return row;
		}
		///////////////////////
		function initViewRowColumn(columnCount) {

			var column = {
				"gridColCount": 12 / parseInt(columnCount),
				"items": []
			}

			return column;
		}
		//////////////////////
		function getEntityViewLibrary(entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityViewLibrary> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/getEntityViewLibrary' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		///////////////////////
		function getEntityViewList(entityName, successCallback, errorCallback) {
			var list = [];
			var view = initView();
			//Test data
			view.id = "7937a4a3-e074-4e2f-aca2-1467a29bb433";
			view.name = "details";
			view.label = "Details";
			view.default = true;
			view.system = true;
			view.type = "general";

			list.push(view);
			var response = {};
			response.success = true;
			response.object = {};
			response.object.views = list;
			successCallback(response);
		}
		//////////////////////
		function getEntityView(viewName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityView> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		//////////////////////
		function createEntityView(viewObj, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createRelation> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/view", data: viewObj }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		//////////////////////
		function updateEntityView(viewObj, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateEntityView> function called');
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewObj.name, data: viewObj }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		//////////////////////
		function patchEntityView(viewObj, viewName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchEntityView> function called');
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName, data: viewObj }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		///////////////////////
		function deleteEntityView(viewName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteEntityView> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		////////////////////
		function safeAddArrayPlace(newObject, array) {
			//If the place is empty or null give it a very high number which will be made correct later
			if (newObject.weight === "" || newObject.weight === null) {
				newObject.weight = 99999;
			}

			//Sort and Free a place
			array.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			for (var i = 0; i < array.length; i++) {
				if (parseInt(array[i].weight) >= parseInt(newObject.weight)) {
					array[i].weight = parseInt(array[i].weight) + 1;
				}
			}

			//Insert the element on its desired position
			array.splice(parseInt(newObject.weight) - 1, 0, newObject);

			//Fix possible gaps
			array.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			for (var i = 0; i < array.length; i++) {
				array[i].weight = i + 1;
			}

			return array;
		}
		/////////////////////
		function safeUpdateArrayPlace(updateObject, array) {
			//If the place is empty or null give it a very high number which will be made correct later
			if (updateObject.weight === "" || updateObject.weight === null) {
				updateObject.weight = 99999;
			}
			for (var i = 0; i < array.length; i++) {
				//If this is an element that has the same place as the newly updated one increment its place
				if (parseInt(array[i].weight) >= parseInt(updateObject.weight) && array[i].id != updateObject.id) {
					array[i].weight = parseInt(array[i].weight) + 1;
				}
				//Find the element and update it
				if (array[i].id == updateObject.id) {
					array[i] = updateObject;
				}

			}
			//Sort again
			array.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			//Recalculate the places to remove gaps
			for (var i = 0; i < array.length; i++) {
				array[i].weight = i + 1;
			}

			return array;
		}
		/////////////////////
		function safeRemoveArrayPlace(array, id) {
			var elementIndex = -1;
			for (var i = 0; i < array.length; i++) {
				if (array[i].id === id) {
					elementIndex = i;
				}
			}
			if (elementIndex != -1) {
				array.splice(elementIndex, 1);
			}
			//Sort again
			array.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			//Recalculate the places to remove gaps
			for (var i = 0; i < array.length; i++) {
				array[i].weight = i + 1;
			}
			return array;
		}

		//#endregion

		//#region <<Records List>>
		///////////////////////
		function initList() {
			$log.debug('webvellaAdmin>providers>admin.service>initList> function called');
			var list =
				{
					"id": null,
					"name": "",
					"label": "",
					"default": false,
					"system": false,
					"weight": 1,
					"type": "general",
					"cssClass": "",
					"pageSize": 10,
					"columns": [],
					"query": null,
					"sorts": null
				}
			return list;
		}

		function sampleList() {
			$log.debug('webvellaAdmin>providers>admin.service>initList> function called');
			var list =
            {
            	"id": "7937a4a3-e074-4e2f-aca2-1467a29bb433",
            	"name": "recent_orders",
            	"label": "Recent Orders",
            	"default": true,
            	"system": true,
            	"weight": 1,
            	"type": "general",
            	"cssClass": "",
            	"pageSize": 10,
            	"columns": [
			{
				"_t": "RecordViewFieldItem",
				"type": "field",
				"fieldId": "48818fa7-77b4-cedd-71e4-80e106038333",
				"fieldName": "id",
				"fieldLabel": "Id",
				"fieldTypeId": 18
			},
				{
					"_t": "RecordViewFieldItem",
					"type": "field",
					"fieldId": "48818fa7-77b4-cedd-71e4-80e106038abf",
					"fieldName": "username",
					"fieldLabel": "Username",
					"fieldTypeId": 18
				},
							{
								"_t": "RecordViewRelationFieldItem",
								"type": "fieldFromRelation",
								"relationId": "48818fa7-77b4-cedd-71e4-80e106038ab1",
								"entityId": "48818fa7-77b4-cedd-71e4-80e106038ab2",
								"entityName": "account",
								"entityLabel": "Account",
								"fieldId": "48818fa7-77b4-cedd-71e4-80e106038ab3",
								"fieldName": "email",
								"fieldLabel": "Email",
								"fieldTypeId": 18
							}],
            	"query": {
            		"queryType": "AND",
            		"fieldName": "",
            		"fieldValue": "",
            		"subQueries": [
						{
							"queryType": "EQ",
							"fieldName": "username",
							"fieldValue": "mozart",
							"subQueries": []
						},
						{
							"queryType": "CONTAINS",
							"fieldName": "email",
							"fieldValue": "domain.com",
							"subQueries": []
						}
            		]
            	},
            	"sorts": [
					{
						"fieldName": "username",
						"sortType": "Descending"
					}
            	]
            }
			return list;
		}

		///////////////////////
		function getEntityLists(entityName, successCallback, errorCallback) {
			//api/v1/en_US/meta/entity/{Name}/list
			$log.debug('webvellaAdmin>providers>admin.service>getEntityLists> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		///////////////////////
		function getEntityList(listName, entityName, successCallback, errorCallback) {
			//api/v1/en_US/meta/entity/{Name}/list/{ListName}"
			$log.debug('webvellaAdmin>providers>admin.service>getEntityList> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		//////////////////////
		function createEntityList(submitObj, entityName, successCallback, errorCallback) {
			//"api/v1/en_US/meta/entity/{Name}/list") -> submitObj
			$log.debug('webvellaAdmin>providers>admin.service>createRelation> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list", data: submitObj }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}
		//////////////////////
		function patchEntityList(submitObj, listName, entityName, successCallback, errorCallback) {
			//"api/v1/en_US/meta/entity/{Name}/list/{ListName}") -> submitObj
			$log.debug('webvellaAdmin>providers>admin.service>patchEntityList> function called');
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list/" + listName, data: submitObj }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function deleteEntityList(listName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteEntityList> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		//#endregion


		//#region << Records >>
		/////////////////////
		function initRecord() {
			var record = {};
			var meta = initView();
			record.meta = meta;
			record.data = {
				"fieldsMeta": null,
				"fieldsData": null
			};
			return record;
		}

		///////////////////////
		function getRecordsByEntityName(listName, entityName, filter, page, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getRecordsByEntityName> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + filter + '/' + page }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function createRecord(entityName, postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createRecord> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function updateRecord(recordId, entityName, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateRecord> function called');
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId, data: patchObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////

		function patchRecordDefault(recordId, entityName, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchRecord> function called');
			//Make the service method pluggable
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId, data: patchObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		function patchRecord(recordId, entityName, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchRecord> function called');
			//Make the service method pluggable
			if ($rootScope.webvellaAdmin.patchRecord && typeof ($rootScope.webvellaAdmin.patchRecord) == "function") {
				$rootScope.webvellaAdmin.patchRecord(recordId, entityName, patchObject, successCallback, errorCallback);
			}
			else {
				patchRecordDefault(recordId, entityName, patchObject, successCallback, errorCallback);
			}

		}

		///////////////////////
		function uploadFileToTemp(file, fieldName, progressCallback, successCallback, errorCallback) {
			//"/fs/upload/" file
			$log.debug('webvellaAdmin>providers>admin.service>uploadFileToTemp> function called');
			//$log.info(file);
			Upload.upload({
				url: '/fs/upload/',
				fields: {},
				file: file
			}).progress(function (evt) {
				progressCallback(evt);
			}).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); })
			.error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function moveFileFromTempToFS(source, target, overwrite, successCallback, errorCallback) {
			//"/fs/move/"
			$log.debug('webvellaAdmin>providers>admin.service>moveFileFromTempToFS> function called');
			var postObject = {
				source: source,
				target: target,
				overwrite: overwrite
			}
			$http({ method: 'POST', url: '/fs/move', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function deleteFileFromFS(filepath, successCallback, errorCallback) {
			///fs/delete/{*filepath}"
			$log.debug('webvellaAdmin>providers>admin.service>deleteFileFromFS> function called');
			$http({ method: 'DELETE', url: filepath }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}


		/////////////////////////
		function manageRecordsRelation(relationName, originFieldRecordId, attachTargetFieldRecordIds, detachTargetFieldRecordIds, successCallback, errorCallback) {
			var postObject = {
				relationName: relationName,//string
				originFieldRecordId: originFieldRecordId, //guid
				attachTargetFieldRecordIds: attachTargetFieldRecordIds, //guid array - list of recordIds that needs to be attached to the new origin
				detachTargetFieldRecordIds: detachTargetFieldRecordIds  //guid array - list of recordIds that needs to be dettached to the new origin - should be empty array when the target field is required
			}


			$log.info('webvellaAdmin>providers>query.service>execute manageRecordsRelation> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/relation', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}


		//#endregion

		//#region << Area specific >>

		/////////////////////
		function initArea() {
			var area = {
				"id": null,
				"color": "blue",
				"label": null,
				"icon_name": "th-large",
				"weight": 10,
				"name": null,
				"roles": [],
				"subscriptions": []
			};

			return area;
		}


		///////////////////////
		function getAreaByName(areaName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getAreaByName> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'area/' + areaName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function deleteArea(recordId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchRecord> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'area/' + recordId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function getAreaRelationByEntityId(entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityRelatedAreas> function called');
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'area/relations/entity/' + entityId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function createAreaEntityRelation(areaId, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createAreaEntityRelation> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'area/' + areaId + '/entity/' + entityId + '/relation' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		///////////////////////
		function removeAreaEntityRelation(areaId, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>removeAreaEntityRelation> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'area/' + areaId + '/entity/' + entityId + '/relation' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		//#endregion

		//#region << Functions >>
		///////////////////////
		function getItemsFromRegion(region) {
			var usedItemsArray = [];
			for (var j = 0; j < region.sections.length; j++) {
				for (var k = 0; k < region.sections[j].rows.length; k++) {
					for (var l = 0; l < region.sections[j].rows[k].columns.length; l++) {
						for (var m = 0; m < region.sections[j].rows[k].columns[l].items.length; m++) {
							usedItemsArray.push(region.sections[j].rows[k].columns[l].items[m]);
						}
					}
				}
			}

			return usedItemsArray;
		}
		//#endregion
	}
})();