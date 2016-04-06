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

	service.$inject = ['$log', '$http', '$rootScope','$filter', 'wvAppConstants', 'Upload', 'ngToast'];



	/* @ngInject */
	function service($log, $http, $rootScope,$filter, wvAppConstants, Upload, ngToast) {
		var serviceInstance = this;

		//create a plug point in the rootScope
		$rootScope.webvellaAdmin = {};

		//#region << Include functions >>
		serviceInstance.getMetaEntityList = getMetaEntityList;
		serviceInstance.initEntity = initEntity;
		serviceInstance.createEntity = createEntity;
		serviceInstance.patchEntity = patchEntity;
		serviceInstance.getEntityMeta = getEntityMeta;
		serviceInstance.getEntityMetaById = getEntityMetaById;
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
		serviceInstance.getRowColumnCountVariationsArray = getRowColumnCountVariationsArray;
		serviceInstance.getRowColumnCountVariationKey = getRowColumnCountVariationKey;
		serviceInstance.convertRowColumnCountVariationKeyToArray = convertRowColumnCountVariationKeyToArray;
		serviceInstance.calculateViewFieldColsFromGridColSize = calculateViewFieldColsFromGridColSize;
 
		//List
		serviceInstance.initList = initList;
		serviceInstance.getEntityLists = getEntityLists;
		serviceInstance.getEntityList = getEntityList;
		serviceInstance.createEntityList = createEntityList;
		serviceInstance.patchEntityList = patchEntityList;
		serviceInstance.updateEntityList = updateEntityList;
		serviceInstance.deleteEntityList = deleteEntityList;
		//Tree
		serviceInstance.initTree = initTree;
		serviceInstance.getEntityTreesMeta = getEntityTreesMeta;
		serviceInstance.getEntityTreeMeta = getEntityTreeMeta;
		serviceInstance.createEntityTree = createEntityTree;
		serviceInstance.patchEntityTree = patchEntityTree;
		serviceInstance.updateEntityTree = updateEntityTree;
		serviceInstance.deleteEntityTree = deleteEntityTree;
		serviceInstance.getRecordsByTreeName = getRecordsByTreeName;
		//Record
		serviceInstance.getRecordsByEntityName = getRecordsByEntityName;
		serviceInstance.getRecord = getRecord;
		serviceInstance.createRecord = createRecord;
		serviceInstance.updateRecord = updateRecord;
		serviceInstance.patchRecord = patchRecord;
		serviceInstance.deleteRecord = deleteRecord;
		serviceInstance.patchRecordDefault = patchRecordDefault;
		serviceInstance.uploadFileToTemp = uploadFileToTemp;
		serviceInstance.moveFileFromTempToFS = moveFileFromTempToFS;
		serviceInstance.deleteFileFromFS = deleteFileFromFS;
		serviceInstance.manageRecordsRelation = manageRecordsRelation;
		serviceInstance.getRecordsByFieldRegex = getRecordsByFieldRegex;
		//Area
		serviceInstance.initArea = initArea;
		serviceInstance.getAreaByName = getAreaByName;
		serviceInstance.deleteArea = deleteArea;
		//serviceInstance.getAreaRelationByEntityId = getAreaRelationByEntityId;
		//serviceInstance.createAreaEntityRelation = createAreaEntityRelation;
		//serviceInstance.removeAreaEntityRelation = removeAreaEntityRelation;
		serviceInstance.regenerateAllAreaSubscriptions = regenerateAllAreaSubscriptions;
		//Function
		serviceInstance.getItemsFromRegion = getItemsFromRegion;
		//User
		serviceInstance.getUserById = getUserById;
		serviceInstance.getAllUsers = getAllUsers;
		serviceInstance.createUser = createUser;
		serviceInstance.updateUser = updateUser;
		serviceInstance.initUser = initUser;
		//#endregion

		//#region << Aux methods >>

		// Global functions for result handling for all methods of this service
		function handleErrorResult(data, status, errorCallback) {
			switch (status) {
				case 401: {
					//handled globally by http observer
					break;
				}
				case 403: {
					//handled globally by http observer
					break;
				}
				case 400:
					if (errorCallback === undefined || typeof (errorCallback) != "function") {
						$log.debug('webvellaAdmin>providers>admin.service> result failure: errorCallback not a function or missing ' + moment().format('HH:mm:ss SSSS'));
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
						content: messageString,
						timeout: 7000
					});
					errorCallback(data);
					break;
				default:
					$log.debug('webvellaAdmin>providers>admin.service> result failure: API call finished with error: ' + status + '  ' + moment().format('HH:mm:ss SSSS'));
					alert("An API call finished with error: " + status);
					break;
			}
		}

		function handleSuccessResult(data, status, successCallback, errorCallback) {
			if (successCallback === undefined || typeof (successCallback) != "function") {
				$log.debug('webvellaAdmin>providers>admin.service> result failure: successCallback not a function or missing  ' + moment().format('HH:mm:ss SSSS'));
				alert("The successCallback argument is not a function or missing");
				return;
			}

			if (!data.success) {
				//when the validation errors occurred
				if (errorCallback === undefined || typeof (errorCallback) != "function") {
					$log.debug('webvellaAdmin>providers>admin.service> result failure: errorCallback not a function or missing  ' + moment().format('HH:mm:ss SSSS'));
					alert("The errorCallback argument in handleSuccessResult is not a function or missing");
					return;
				}
				errorCallback(data);
			}
			else {
				$log.debug('webvellaAdmin>providers>admin.service> result success: get object  ' + moment().format('HH:mm:ss SSSS'));
				successCallback(data);
			}

		}
		//#endregion

		//#region << Entity >>

		///////////////////////
		function getMetaEntityList(successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function initEntity() {
			$log.debug('webvellaAdmin>providers>admin.service>initEntity> function called ' + moment().format('HH:mm:ss SSSS'));
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
			$log.debug('webvellaAdmin>providers>admin.service>createEntity> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getEntityMeta(name, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityMeta> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + name }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getEntityMetaById(entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityMetaById> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/id/' + entityId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function patchEntity(entityId, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchEntity> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId, data: patchObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteEntity(entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteEntity> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion << Entity >>

		//#region << Field >>
		///////////////////////
		function initField(typeId) {
			$log.debug('webvellaAdmin>providers>admin.service>initField> function called ' + moment().format('HH:mm:ss SSSS'));
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
				enableSecurity: false,
				permissions: {
					canRead: [],
					canUpdate: []
				}

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
				case 20: //relation field not used in ui
					break
				case 21:
					field.relatedEntityId = null;
					field.relationId = null;
					field.selectedTreeId = null;
					field.selectionType = "single-select";
					field.selectionTarget = "all";
					break;
				default:
					break;
			}

			return field;
		}

		///////////////////////
		function createField(postObject, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createField> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function updateField(putObject, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateField> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + putObject.id, data: putObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteField(fieldId, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateField> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + fieldId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#region << Relations  >>
		///////////////////////
		function initRelation() {
			$log.debug('webvellaAdmin>providers>admin.service>initRelation> function called ' + moment().format('HH:mm:ss SSSS'));
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
			$log.debug('webvellaAdmin>providers>admin.service>getRelationByName> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + name }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getRelationsList(successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getRelationsList> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function createRelation(postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createRelation> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/relation', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function updateRelation(postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateRelation> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + postObject.id, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteRelation(relationId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateRelation> function called');
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + relationId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#region << Entity Views >>

		///////////////////////
		function initView() {
			$log.debug('webvellaAdmin>providers>admin.service>initView> function called ' + moment().format('HH:mm:ss SSSS'));
			var view = {
				"id": null,
				"name": "",
				"label": "",
				"default": false,
				"system": false,
				"weight": 1,
				"cssClass": "",
				"type": "general",
				"iconName": "file-text-o",
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
				"label": "Section name",
				"cssClass": "",
				"showLabel": true,
				"collapsed": false,
				"weight": 1,
				"tabOrder": "left-right",
				"rows": [initViewRow(1)]
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

			var rowColumnCountArray = convertRowColumnCountVariationKeyToArray(columnCount);

			for (var i = 0; i < rowColumnCountArray.length; i++) {
				var column = {
					"gridColCount": rowColumnCountArray[i],
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
			$log.debug('webvellaAdmin>providers>admin.service>getEntityViewLibrary> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/getEntityViewLibrary' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
			$log.debug('webvellaAdmin>providers>admin.service>getEntityView> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function createEntityView(viewObj, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createRelation> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/view", data: viewObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function updateEntityView(viewObj, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateEntityView> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewObj.name, data: viewObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function patchEntityView(viewObj, viewName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchEntityView> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName, data: viewObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteEntityView(viewName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteEntityView> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
			var resultArray = [];
			//If the place is empty or null give it a very high number which will be made correct later
			if (updateObject.weight === "" || updateObject.weight === null) {
				updateObject.weight = 99999;
			}
			//Check if the element is new, or already existing, by matching the ID.
			//If new, than the other sections with the same or more weight should be moved back with one place
			//If existing, than only the section with matching weight should be move one place ahead so the updated could take its place. All the rest should preserve their places
			var alreadyExistingElement = false;
			var originalWeight = -1;
			if (updateObject.id && updateObject.id != null) {
				for (var i = 0; i < array.length; i++) {
					if (updateObject.id === array[i].id) {
						alreadyExistingElement = true;
						originalWeight = array[i].weight;
					}
				}
			}

			//There is a change in the place
			if (parseInt(originalWeight) != parseInt(updateObject.weight)) {
				for (var i = 0; i < array.length; i++) {
					var currentElement = array[i];
					//Case 1: New element is pushed
					if (!alreadyExistingElement) {
						//If this is an element that has the same place as the newly updated one increment its place
						if (parseInt(currentElement.weight) >= parseInt(updateObject.weight) && currentElement.id != updateObject.id) {
							currentElement.weight = parseInt(currentElement.weight) + 1;
							resultArray.push(currentElement);
						}
						//Find the element and update it
						if (array[i].id == updateObject.id) {
							resultArray.push(updateObject);
						}
					}
						//Case 2: existing element is moved
					else {
						//case 2.1 - elements with smaller weight from  both the old and new weight should preserve theirs. elements with weight bigger than both the old and new weight should preserve weight
						if (currentElement.id != updateObject.id && (parseInt(currentElement.weight) < parseInt(updateObject.weight) && parseInt(currentElement.weight) < parseInt(originalWeight)) ||
							(parseInt(currentElement.weight) > parseInt(updateObject.weight) && parseInt(currentElement.weight) > parseInt(originalWeight))) {
							resultArray.push(currentElement);
						}
							//case 2.2 - this is the same element
						else if (currentElement.id === updateObject.id) {
							resultArray.push(updateObject);
						}
							//case 2.3 - elements with weight between the new and the old one should either gain or lose weight
						else {
							//case 2.3.1 - if the new weight is biger the the old -> element is pushed back, the between elements should lose weight
							if (parseInt(updateObject.weight) > parseInt(originalWeight)) {
								currentElement.weight = parseInt(currentElement.weight) - 1;
								resultArray.push(currentElement);
							}
								//case 2.3.1 - if the new weight is smaller the the old -> element is pushed forward, the between elements should gain weight
							else {
								currentElement.weight = parseInt(currentElement.weight) + 1;
								resultArray.push(currentElement);
							}

						}

					}

				}
				//Sort again
				resultArray.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
				//Recalculate the places to remove gaps
				for (var i = 0; i < array.length; i++) {
					resultArray[i].weight = i + 1;
				}
			}
				//There is no place change
			else {
				for (var i = 0; i < array.length; i++) {
					var currentElement = array[i];
					if (currentElement.id === updateObject.id) {
						resultArray.push(updateObject);
					}
					else {
						resultArray.push(currentElement);
					}
				}
			}
			return resultArray;
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

		function getRowColumnCountVariationsArray() {
			var rowColCountVariantions = [
					  {
	  					key: 1,
						columns:1,
	  					value: "One column"
					  },
					  {
	  					key: 2,
						columns:2,
	  					value: "Two columns"
					  },
					  {
	  					key: 3,
						columns:3,
	  					value: "Three columns"
					  },
					  {
	  					key: 4,
						columns:4,
	  					value: "Four columns"
					  },
					  {
	  					key: 12,
						columns:2,
	  					value: "1-2 columns"
					  },
					  {
	  					key: 13,
						columns:2,
	  					value: "1-3 columns"
					  },
					  {
	  					key: 15,
						columns:2,
	  					value: "1-5 columns"
					  },
					  {
	  					key: 21,
						columns:2,
	  					value: "2-1 columns"
					  },
					  {
	  					key: 31,
						columns:2,
	  					value: "3-1 columns"
					  },
					  {
	  					key: 51,
						columns:2,
	  					value: "5-1 columns"
					  }
			];

			return rowColCountVariantions;
		}

		function getRowColumnCountVariationKey(row){
			var gridColCountArray = [];
			for (var j = 0; j < row.columns.length; j++) {
				 gridColCountArray.push(row.columns[j].gridColCount);
			}

			if(arraysEqual(gridColCountArray,[12])){
				return 1;
			}
			else if(arraysEqual(gridColCountArray,[6,6])){
				return 2;
			}
			else if(arraysEqual(gridColCountArray,[4,4,4])){
				return 3;
			}
			else if(arraysEqual(gridColCountArray,[3,3,3,3])){
				return 4;
			}
			else if(arraysEqual(gridColCountArray,[4,8])){
				return 12;
			}
			else if(arraysEqual(gridColCountArray,[3,9])){
				return 13;
			}
			else if(arraysEqual(gridColCountArray,[2,10])){
				return 15;
			}
			else if(arraysEqual(gridColCountArray,[8,4])){
				return 21;
			}
			else if(arraysEqual(gridColCountArray,[9,3])){
				return 31;
			}
			else if(arraysEqual(gridColCountArray,[10,2])){
				return 51;
			}
		}

		function convertRowColumnCountVariationKeyToArray(key){
			switch(key){
				case 1:
					return [12];
				case 2:
					return [6,6];
				case 3:
					return [4,4,4];
				case 4:
					return [3,3,3,3];
				case 12:
					return [4,8];
				case 13:
					return [3,9];
				case 15:
					return [2,10];		
				case 21:
					return [8,4];
				case 31:
					return [9,3];
				case 51:
					return [10,2];					
			}
		}

		function calculateViewFieldColsFromGridColSize(elementType,gridColSize){

			if(12 % gridColSize != 0){
				return "erp";
			}
			else {
				if(elementType == "label"){
				  return 12/gridColSize;
				}
				else if(elementType == "field"){
					return 12 - 12/gridColSize;
				}
				else{
					return 1;
				}
			}

		}

		//#endregion

		//#region << Records List >>
		///////////////////////
		function initList() {
			$log.debug('webvellaAdmin>providers>admin.service>initList> function called ' + moment().format('HH:mm:ss SSSS'));
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
					"visibleColumnsCount": 7,
					"viewNameOverride": null,
					"columns": [],
					"query": null,
					"sorts": null
				}
			return list;
		}

		function sampleList() {
			$log.debug('webvellaAdmin>providers>admin.service>initList> function called ' + moment().format('HH:mm:ss SSSS'));
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
			$log.debug('webvellaAdmin>providers>admin.service>getEntityLists> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityList(listName, entityName, successCallback, errorCallback) {
			//api/v1/en_US/meta/entity/{Name}/list/{ListName}"
			$log.debug('webvellaAdmin>providers>admin.service>getEntityList> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function createEntityList(submitObj, entityName, successCallback, errorCallback) {
			//"api/v1/en_US/meta/entity/{Name}/list") -> submitObj
			$log.debug('webvellaAdmin>providers>admin.service>createRelation> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list", data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function patchEntityList(submitObj, listName, entityName, successCallback, errorCallback) {
			//"api/v1/en_US/meta/entity/{Name}/list/{ListName}") -> submitObj
			$log.debug('webvellaAdmin>providers>admin.service>patchEntityList> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list/" + listName, data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//////////////////////
		function updateEntityList(listObj, entityName, successCallback, errorCallback) {
			//"api/v1/en_US/meta/entity/{Name}/list/{ListName}") -> submitObj
			$log.debug('webvellaAdmin>providers>admin.service>patchEntityList> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list/" + listObj.name, data: listObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteEntityList(listName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteEntityList> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#region <<Records Tree>>
		///////////////////////

		//Additionally in the field is set
		//SelectionType - single-select,multi-select,single-branch-select
		//SelectionTarget - all,leaves

		function initTree() {
			$log.debug('webvellaAdmin>providers>admin.service>initTree> function called ' + moment().format('HH:mm:ss SSSS'));
			var tree =
            {
            	"id": null,
            	"name": "",
            	"label": "",
            	"default": false,
            	"system": false,
            	"cssClass": "",
            	"iconName": "",
            	"relationId": null, // Only relations in which both origin and target are the current entity
            	"depthLimit": 5,
            	"nodeParentIdFieldId": null, //Inherited from the relation Target field
            	"nodeIdFieldId": null, //Inherited from the relation Origin field
            	"nodeNameFieldId": null, //Only certain types should be allowed here - used for URL generation
            	"nodeLabelFieldId": null, //Only certain types should be allowed here - human readable node label
            	"rootNodes": [],
            	"nodeObjectProperties": []
            }
			return tree;
		}

		function sampleTree() {
			$log.debug('webvellaAdmin>providers>admin.service>sampleTree> function called ' + moment().format('HH:mm:ss SSSS'));
			var tree =
            {
            	"id": "be607564-8424-4df2-b6ff-16a7a9529c30",
            	"name": "categories",
            	"label": "Categories",
            	"default": false,
            	"system": false,
            	"cssClass": "some-css",
            	"iconName": "sitemap",
            	"relationId": "460e699c-6624-4238-bb17-12243cf5d56b", // Only relations in which both origin and target are the current entity
            	"depthLimit": 5,
            	"nodeParentIdFieldId": "16672229-1694-468e-a363-c80effffe5d1", //Inherited from the relation Target field
            	"nodeIdFieldId": "5df6bba4-061b-41ce-bf39-8f6b50fd023d", //Inherited from the relation Origin field
            	"nodeNameFieldId": "c80e1e20-71e2-4de1-8b3b-5a63c6740cea", //Only certain types should be allowed here - used for URL generation
            	"nodeLabelFieldId": "664f2b4a-dd96-4e27-aabd-898e255d9c8e", //Only certain types should be allowed here - human readable node label
            	"rootNodes": [
					{
						"id": "5548bbc7-eda3-45e7-b0ee-253f4eaf2785",
						"recordId": "5548bbc7-eda3-45e7-b0ee-253f4eaf2785",
						"name": "clothes",
						"label": "Clothes",
						"parentId": "b6add018-f9eb-4b60-a724-7d1e2597449c"
					}
            	],
            	"nodeObjectProperties": ["16672229-1694-468e-a363-c80effffe5d1", "5df6bba4-061b-41ce-bf39-8f6b50fd023d", "c80e1e20-71e2-4de1-8b3b-5a63c6740cea", "664f2b4a-dd96-4e27-aabd-898e255d9c8e"]
            }
			return tree;
		}

		///////////////////////
		function getEntityTreesMeta(entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityTreesMeta> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/tree' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityTreeMeta(treeName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityTreeMeta> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/tree/' + treeName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function createEntityTree(submitObj, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createEntityTree> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/tree", data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function patchEntityTree(submitObj, treeName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchEntityTree> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/tree/" + treeName, data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//////////////////////
		function updateEntityTree(treeObj, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateEntityTree> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/tree/" + treeObj.name, data: treeObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteEntityTree(treeName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteEntityTree> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/tree/' + treeName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getRecordsByTreeName(treeName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityTreeData> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/tree/' + treeName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
			$log.debug('webvellaAdmin>providers>admin.service>getRecordsByEntityName> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + filter + '/' + page }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getRecord(recordId, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAreas>providers>areas.service>getEntityRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function createRecord(entityName, postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function updateRecord(recordId, entityName, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId, data: patchObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////

		function patchRecordDefault(recordId, entityName, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			//Make the service method pluggable
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId, data: patchObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		function patchRecord(recordId, entityName, patchObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			//Make the service method pluggable
			if ($rootScope.webvellaAdmin.patchRecord && typeof ($rootScope.webvellaAdmin.patchRecord) == "function") {
				$rootScope.webvellaAdmin.patchRecord(recordId, entityName, patchObject, successCallback, errorCallback);
			}
			else {
				patchRecordDefault(recordId, entityName, patchObject, successCallback, errorCallback);
			}

		}

		function deleteRecord(recordId, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			//Make the service method pluggable
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}


		///////////////////////
		function getRecordsByFieldRegex(fieldName, entityName, pattern, successCallback, errorCallback) {
			var patternObject = {};
			patternObject.pattern = pattern;
			$log.debug('webvellaAdmin>providers>admin.service>getRecordsByFieldRegex> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/regex/' + fieldName, data: patternObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}


		///////////////////////
		function uploadFileToTemp(file, fieldName, progressCallback, successCallback, errorCallback) {
			//"/fs/upload/" file
			$log.debug('webvellaAdmin>providers>admin.service>uploadFileToTemp> function called ' + moment().format('HH:mm:ss SSSS'));
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
			$log.debug('webvellaAdmin>providers>admin.service>moveFileFromTempToFS> function called ' + moment().format('HH:mm:ss SSSS'));
			var postObject = {
				source: source,
				target: target,
				overwrite: overwrite
			}
			$http({ method: 'POST', url: '/fs/move', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteFileFromFS(filepath, successCallback, errorCallback) {
			///fs/delete/{*filepath}"
			$log.debug('webvellaAdmin>providers>admin.service>deleteFileFromFS> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: filepath }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}


		/////////////////////////
		function manageRecordsRelation(relationName, originFieldRecordId, attachTargetFieldRecordIds, detachTargetFieldRecordIds, successCallback, errorCallback) {
			var postObject = {
				relationName: relationName,//string
				originFieldRecordId: originFieldRecordId, //guid
				attachTargetFieldRecordIds: attachTargetFieldRecordIds, //guid array - list of recordIds that needs to be attached to the new origin
				detachTargetFieldRecordIds: detachTargetFieldRecordIds  //guid array - list of recordIds that needs to be detached to the new origin - should be empty array when the target field is required
			}


			$log.debug('webvellaAdmin>providers>query.service>execute manageRecordsRelation> function called');
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/relation', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
			$log.debug('webvellaAdmin>providers>admin.service>getAreaByName> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'area/' + areaName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteArea(recordId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>patchRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'area/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getAreaRelationByEntityId(entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getEntityRelatedAreas> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'area/relations/entity/' + entityId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function createAreaEntityRelation(areaId, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createAreaEntityRelation> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'area/' + areaId + '/entity/' + entityId + '/relation' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function removeAreaEntityRelation(areaId, entityId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>removeAreaEntityRelation> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'area/' + areaId + '/entity/' + entityId + '/relation' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function regenerateAllAreaSubscriptions() {
			$log.debug('webvellaAdmin>providers>admin.service>regenerateAllAreaSubscriptions> function called ' + moment().format('HH:mm:ss SSSS'));
			var response = {};
			response.success = true;
			response.message = "All area subscriptions regenerated";

			var entities = [];
			var areas = [];

			//#region << Get data >>
			function rasErrorCallback(data, status) {
				$log.warn("Area subscriptions were not regenerated due to:  " + response.message);
			}

			function rasGetEntityMetaListSuccessCallback(data, status) {
				entities = data.object.entities;
				//Get all areas
				getRecordsByEntityName("null", "area", "null", "null", rasGetAreasListSuccessCallback, rasErrorCallback);
			}

			function rasGetAreasListSuccessCallback(data, status) {
				areas = data.object.data;
				executeRegeneration();
			}

			//Get all entities meta
			getMetaEntityList(rasGetEntityMetaListSuccessCallback, rasErrorCallback)

			//#endregion

			//#region << Process >>

			function executeRegeneration() {
				//Cycle entities and generate array of valid subscription for each
				var validSubscriptionsArray = [];
				entities.forEach(function (entity) {
					var validSubscriptionObj = {
						name: null,
						label: null,
						labelPlural: null,
						iconName: null,
						weight: null
					};
					validSubscriptionObj.view = {
						name: null,
						label: null
					};
					validSubscriptionObj.list = {
						name: null,
						label: null
					};
					//Entity
					validSubscriptionObj.name = entity.name;
					validSubscriptionObj.label = entity.label;
					validSubscriptionObj.labelPlural = entity.labelPlural;
					validSubscriptionObj.iconName = entity.iconName;
					validSubscriptionObj.weight = entity.weight;

					//Views

					entity.recordViews.sort(function (a, b) {
						if (a.weight < b.weight) return -1;
						if (a.weight > b.weight) return 1;
						return 0;
					});
					for (var k = 0; k < entity.recordViews.length; k++) {
						var view = entity.recordViews[k];
						if (view.default && view.type == "general") {
							validSubscriptionObj.view.name = view.name;
							validSubscriptionObj.view.label = view.label;
							break;
						}
					}
					//List
					entity.recordLists.sort(function (a, b) {
						if (a.weight < b.weight) return -1;
						if (a.weight > b.weight) return 1;
						return 0;
					});
					for (var m = 0; m < entity.recordLists.length; m++) {
						var list = entity.recordLists[m];
						if (list.default && list.type == "general") {
							validSubscriptionObj.list.name = list.name;
							validSubscriptionObj.list.label = list.label;
							break;
						}
					}

					if (validSubscriptionObj.view.name && validSubscriptionObj.list.name) {
						validSubscriptionsArray.push(validSubscriptionObj);
					}
				});

				function rasAreaUpdateSuccessCallback(response) { }

				function rasAreaUpdateErrorCallback(response) {
					$log.warn("Area subscriptions were not regenerated due to:  " + response.message);
				}

				//Cycle through areas and substitute each entity subscription with its new valid subscription
				function checkIfEntityViewListExists(entityName, viewName, listName) {
					var isEntityViewListExist = {};
					isEntityViewListExist.view = false;
					isEntityViewListExist.list = false;
					for (var i = 0; i < entities.length; i++) {
						if (entities[i].name == entityName) {
							for (var j = 0; j < entities[i].recordViews.length; j++) {
								if (entities[i].recordViews[j].name == viewName) {
									isEntityViewListExist.view = true;
									break;
								}
							}
							for (var m = 0; m < entities[i].recordLists.length; m++) {
								if (entities[i].recordLists[m].name == listName) {
									isEntityViewListExist.list = true;
									break;
								}
							}
						}
					}
					return isEntityViewListExist;
				}

				areas.forEach(function (area) {
					var subscriptions = angular.fromJson(area.subscriptions);
					var newSubscriptions = [];
					for (var n = 0; n < subscriptions.length; n++) {
						//if subscription view or list exists do not change it. This will enable the manual selections not to be overwritten
						var isEntityViewListExist = {};
						isEntityViewListExist.view = false;
						isEntityViewListExist.list = false;
						isEntityViewListExist = checkIfEntityViewListExists(subscriptions[n].name, subscriptions[n].view.name, subscriptions[n].list.name);
						if (isEntityViewListExist.view || isEntityViewListExist.list) {
							for (var j = 0; j < validSubscriptionsArray.length; j++) {
								if (subscriptions[n].name === validSubscriptionsArray[j].name) {
									var newSubscriptionObject = validSubscriptionsArray[j];
									if (isEntityViewListExist.view) {
										newSubscriptionObject.view = subscriptions[n].view;
									}
									if (isEntityViewListExist.list) {
										newSubscriptionObject.list = subscriptions[n].list;
									}
									newSubscriptions.push(newSubscriptionObject);
									break;
								}
							}
						}
					}
					area.subscriptions = angular.toJson(newSubscriptions);
					updateRecord(area.id, "area", area, rasAreaUpdateSuccessCallback, rasAreaUpdateErrorCallback);
				});

			}

			//#endregion
		}

		//#endregion

		//#region << User specific >>
		///////////////////////
		function initUser() {
			var user = {
				"id": null,
				"email": null,
				"enabled": true,
				"first_name": null,
				"image": null,
				"last_logged_in": null,
				"last_name": null,
				"password": null,
				"verified": true
			}
			return user;
		}

		///////////////////////
		function getUserById(userId, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getUserById> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'user/' + userId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getAllUsers(successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getAllUsers> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'user/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function createUser(userObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createUser> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'user', data: userObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function updateUser(userId, userObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>updateUser> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'user/' + userId, data: userObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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