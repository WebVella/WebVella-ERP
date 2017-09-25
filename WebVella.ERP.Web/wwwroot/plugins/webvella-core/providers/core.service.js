/* core.service.js */

/**
* @desc Javascript API core service
*/

(function () {
	'use strict';

	angular
		.module('webvellaCore')
		.service('webvellaCoreService', service);

	service.$inject = ['$cookies', '$q', '$http', '$log', '$location', 'wvAppConstants', '$rootScope', '$anchorScroll', 'ngToast',
				'$timeout', 'Upload', '$translate', '$filter', '$state', '$localStorage', '$sce'];


	function service($cookies, $q, $http, $log, $location, wvAppConstants, $rootScope, $anchorScroll, ngToast,
				$timeout, Upload, $translate, $filter, $state, $localStorage, $sce) {
		var serviceInstance = this;

		//#region << Include functions >> ///////////////////////////////////////////////////////////////////////////////////

		//#region << Entity >>

		//Init
		serviceInstance.initEntity = initEntity;
		serviceInstance.initField = initField;
		//Create
		serviceInstance.createEntity = createEntity;
		//Read
		serviceInstance.getEntityMeta = getEntityMeta;
		serviceInstance.getEntityMetaById = getEntityMetaById;
		serviceInstance.getEntityMetaList = getEntityMetaList;
		//Update
		serviceInstance.patchEntity = patchEntity;
		//Delete
		serviceInstance.deleteEntity = deleteEntity;
		//Helpers
		serviceInstance.getEntityMetaFromEntityList = getEntityMetaFromEntityList;
		//#endregion

		//#region << Field >>

		//Create
		serviceInstance.createField = createField;
		//Read
		serviceInstance.renderFieldValue = renderFieldValue;
		//Update
		serviceInstance.updateField = updateField;
		//Delete
		serviceInstance.deleteField = deleteField;

		//#endregion

		//#region << View >>

		//Init
		serviceInstance.initView = initView;
		serviceInstance.initViewRegion = initViewRegion;
		serviceInstance.initViewSection = initViewSection;
		serviceInstance.initViewRow = initViewRow;
		serviceInstance.initViewRowColumn = initViewRowColumn;
		serviceInstance.initViewActionItem = initViewActionItem;
		serviceInstance.initViewActionItemTemplate = initViewActionItemTemplate;
		//Create
		serviceInstance.createEntityView = createEntityView;
		//Read
		serviceInstance.getEntityView = getEntityView;
		serviceInstance.getEntityRecordViewFromEntitiesMetaList = getEntityRecordViewFromEntitiesMetaList;
		//Update
		serviceInstance.updateEntityView = updateEntityView;
		serviceInstance.patchEntityView = patchEntityView;
		//Delete
		serviceInstance.deleteEntityView = deleteEntityView;
		//Helpers
		serviceInstance.safeAddArrayPlace = safeAddArrayPlace;
		serviceInstance.safeUpdateArrayPlace = safeUpdateArrayPlace;
		serviceInstance.safeRemoveArrayPlace = safeRemoveArrayPlace;
		serviceInstance.getEntityViewLibrary = getEntityViewLibrary;
		serviceInstance.getRowColumnCountVariationsArray = getRowColumnCountVariationsArray;
		serviceInstance.getRowColumnCountVariationKey = getRowColumnCountVariationKey;
		serviceInstance.convertRowColumnCountVariationKeyToArray = convertRowColumnCountVariationKeyToArray;
		serviceInstance.getViewMenuOptions = getViewMenuOptions;
		serviceInstance.getItemsFromRegion = getItemsFromRegion;
		serviceInstance.getSafeViewNameAndEntityName = getSafeViewNameAndEntityName;

		//#endregion

		//#region << List >>

		//Init
		serviceInstance.initList = initList;
		serviceInstance.initListActionItem = initListActionItem;
		serviceInstance.initListActionItemTemplate = initListActionItemTemplate;
		//Create
		serviceInstance.createEntityRecordList = createEntityRecordList;
		//Read
		serviceInstance.getEntityRecordLists = getEntityRecordLists;
		serviceInstance.getEntityRecordList = getEntityRecordList;
		serviceInstance.getEntityRecordListFromEntitiesMetaList = getEntityRecordListFromEntitiesMetaList;
		//Update
		serviceInstance.patchEntityRecordList = patchEntityRecordList;
		serviceInstance.updateEntityRecordList = updateEntityRecordList;
		//Delete
		serviceInstance.deleteEntityRecordList = deleteEntityRecordList;
		//Helpers
		serviceInstance.getListMenuOptions = getListMenuOptions;
		serviceInstance.extractSupportedFilterFields = extractSupportedFilterFields;
		serviceInstance.getSafeListNameAndEntityName = getSafeListNameAndEntityName;
		//#endregion

		//#region << Tree >>

		//Init
		serviceInstance.initTree = initTree;
		//Create
		serviceInstance.createEntityTree = createEntityTree;
		//Read
		serviceInstance.getEntityTreesMeta = getEntityTreesMeta;
		serviceInstance.getEntityTreeMeta = getEntityTreeMeta;
		//Update
		serviceInstance.patchEntityTree = patchEntityTree;
		serviceInstance.updateEntityTree = updateEntityTree;
		//Delete
		serviceInstance.deleteEntityTree = deleteEntityTree;

		//#endregion

		//#region << Relations >>

		//Init
		serviceInstance.initRelation = initRelation;
		//Create
		serviceInstance.createRelation = createRelation;
		//Read
		serviceInstance.getRelationByName = getRelationByName;
		serviceInstance.getRelationsList = getRelationsList;
		//Update
		serviceInstance.updateRelation = updateRelation;
		//Delete
		serviceInstance.deleteRelation = deleteRelation;
		//Helpers
		serviceInstance.getRelationFromRelationsList = getRelationFromRelationsList;
		//#endregion

		//#region << Record >>

		//Create
		serviceInstance.createRecord = createRecord;
		serviceInstance.createRecordWithRelation = createRecordWithRelation;
		serviceInstance.importEntityRecords = importEntityRecords;
		serviceInstance.evaluateImportEntityRecords = evaluateImportEntityRecords;
		serviceInstance.exportListRecords = exportListRecords;
		//Read
		serviceInstance.getRecord = getRecord;
		serviceInstance.getRecordByViewName = getRecordByViewName;
		serviceInstance.getRecordByViewMeta = getRecordByViewMeta;
		serviceInstance.getRecordsWithoutList = getRecordsWithoutList;
		serviceInstance.getRecordsByListMeta = getRecordsByListMeta;
		serviceInstance.getRecordsByListName = getRecordsByListName;
		serviceInstance.getRecordsByTreeName = getRecordsByTreeName;
		serviceInstance.getRecordsByFieldAndRegex = getRecordsByFieldAndRegex;
		//Update
		serviceInstance.updateRecord = updateRecord;
		serviceInstance.patchRecord = patchRecord;
		serviceInstance.updateRecordRelation = updateRecordRelation;
		serviceInstance.updateRecordRelationReverse = updateRecordRelationReverse;
		//Delete
		serviceInstance.deleteRecord = deleteRecord;
		//Helpers
		serviceInstance.uploadFileToTemp = uploadFileToTemp;
		serviceInstance.moveFileFromTempToFS = moveFileFromTempToFS;
		serviceInstance.deleteFileFromFS = deleteFileFromFS;
		serviceInstance.generateHighlightString = generateHighlightString;
		serviceInstance.quickSearch = quickSearch;


		//#endregion

		//#region << Site >>
		serviceInstance.registerHookListener = registerHookListener;
		serviceInstance.launchHook = launchHook;
		serviceInstance.setPageTitle = setPageTitle;
		serviceInstance.setBodyColorClass = setBodyColorClass;
		serviceInstance.initValidationObject = initValidationObject;
		serviceInstance.setValidationError = setValidationError;
		serviceInstance.generateValidationMessages = generateValidationMessages;
		serviceInstance.GoToState = GoToState;
		//#endregion

		//#region << Area >>
		serviceInstance.initArea = initArea;
		serviceInstance.regenerateAllAreaAttachments = regenerateAllAreaAttachments;
		serviceInstance.getCurrentAreaFromAreaList = getCurrentAreaFromAreaList;
		serviceInstance.getDefaultViewNameForAreaEntity = getDefaultViewNameForAreaEntity;
		//#endregion

		//#region << User >>
		serviceInstance.initUser = initUser;
		serviceInstance.login = login;
		serviceInstance.logout = logout;
		serviceInstance.getCurrentUser = getCurrentUser;
		serviceInstance.getCurrentUserPermissions = getCurrentUserPermissions;
		serviceInstance.applyAreaAccessPolicy = applyAreaAccessPolicy;
		serviceInstance.userHasRecordPermissions = userHasRecordPermissions;
		serviceInstance.userIsInRole = userIsInRole;
		//#endregion

		//#region << Default list actions >>
		serviceInstance.listAction_getRecordCreateUrl = listAction_getRecordCreateUrl;
		serviceInstance.listAction_getRecordDetailsUrl = listAction_getRecordDetailsUrl;

		serviceInstance.viewAction_fieldUpdate = viewAction_fieldUpdate;
		serviceInstance.viewAction_deleteRecord = viewAction_deleteRecord;

		//#endregion

		//#region << Helpers >>
		serviceInstance.getFileContent = getFileContent;
		serviceInstance.getFieldTypes = getFieldTypes;
		serviceInstance.currencyMetas = currencyMetas;
		serviceInstance.generateErrorMessage = generateErrorMessage;
		//#endregion

		//#region << Plugins >>
		serviceInstance.getPluginsList = getPluginsList;
		//#endregion

		//#region << Schedule plans >>
		serviceInstance.updateSchedulePlan = updateSchedulePlan;
		serviceInstance.triggerSchedulePlan = triggerSchedulePlan;
		serviceInstance.getSchedulePlanList = getSchedulePlanList;
		serviceInstance.getSchedulePlan = triggerSchedulePlan;
		serviceInstance.getJobsList = getJobsList;
		//#endregion

		//#region << System log >>
		serviceInstance.getSystemLog = getSystemLog;
		//#endregion


		//#region << User file >>
		serviceInstance.getUserFileList = getUserFileList;
		serviceInstance.uploadUserFile = uploadUserFile;
		//#endregion


		//#endregion


		//#region << Functions >> ///////////////////////////////////////////////////////////////////////////////////

		//#region << Global HTTP Error and Success Handlers >>

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
					//ngToast.create({
					//	className: 'error',
					//	content: messageString,
					//	timeout: 7000
					//});
					//Interface should create their own validation errors
					errorCallback(data);
					break;
				default:
					console.log("An API call finished with error: " + status);
					break;
			}
		}
		function handleSuccessResult(data, status, successCallback, errorCallback) {
			if (successCallback === undefined || typeof (successCallback) != "function") {
				alert("The successCallback argument is not a function or missing");
				return;
			}

			if (!data.success) {
				//when the validation errors occurred
				if (errorCallback === undefined || typeof (errorCallback) != "function") {
					alert("The errorCallback argument in handleSuccessResult is not a function or missing");
					return;
				}
				errorCallback(data);
			}
			else {
				successCallback(data);
			}

		}

		//#endregion

		//#region << Entity >>

		///////////////////////
		function initEntity() {
			var entity = {
				id: null,
				name: "",
				label: "",
				labelPlural: "",
				system: false,
				iconName: "database",
				fields: [],
				recordViews: [],
				recordLists: [],
				recordTrees: [],
				weight: 10,
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
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityMeta(name, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + name }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityMetaById(entityId, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/id/' + entityId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityMetaList(successCallback, errorCallback) {
			var $storage = $localStorage;
			if ($storage.entityMetaList && $storage.entityMetaList.hash) {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list?hash=' + $storage.entityMetaList.hash })
					.then(function getSuccessCallback(response) {
						if (response.data.object != null) {
							var localStorageEntityMeta = {};
							localStorageEntityMeta.hash = response.data.hash;
							localStorageEntityMeta.list = response.data.object;
							$storage.entityMetaList = localStorageEntityMeta;
						}
						else {
							response.data.object = $storage.entityMetaList.list;
						}
						handleSuccessResult(response.data, response.status, successCallback, errorCallback);
					}, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list' })
					.then(function getSuccessCallback(response) {
						var localStorageEntityMeta = {};
						localStorageEntityMeta.hash = response.data.hash;
						localStorageEntityMeta.list = response.data.object;
						$storage.entityMetaList = localStorageEntityMeta;
						handleSuccessResult(response.data, response.status, successCallback, errorCallback);
					}, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
		}
		///////////////////////
		function patchEntity(entityId, patchObject, successCallback, errorCallback) {
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId, data: patchObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteEntity(entityId, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		/////////////////////
		function getEntityMetaFromEntityList(entityName, entityMetaList) {
			for (var i = 0; i < entityMetaList.length; i++) {
				if (entityMetaList[i].name == entityName) {
					return entityMetaList[i];
				}
			}
			return null;
		}


		//#endregion << Entity >>

		//#region << Field >>

		//////////////////////////
		function renderFieldValue(data, fieldMeta) {
			switch (fieldMeta.fieldType) {
				case 1:
					return getAutoIncrementString(data, fieldMeta);
				case 2:
					return getCheckboxString(data, fieldMeta);
				case 3:
					return getCurrencyString(data, fieldMeta);
				case 4:
					return getDateString(data, fieldMeta);
				case 5:
					return getDateTimeString(data, fieldMeta);
				case 6:
					return getEmailString(data, fieldMeta);
				case 7:
					return getFileString(data, fieldMeta);
				case 8:
					return getHtmlString(data, fieldMeta);
				case 9:
					return getImageString(data, fieldMeta);
				case 10:
					return getTextareaString(data, fieldMeta);
				case 11:
					return getMultiselectString(data, fieldMeta);
				case 12:
					return getNumberString(data, fieldMeta);
				case 13:
					return getPasswordString(data, fieldMeta);
				case 14:
					return getPercentString(data, fieldMeta);
				case 15:
					return getPhoneString(data, fieldMeta);
				case 16:
					return getGuidString(data, fieldMeta);
				case 17:
					return getDropdownString(data, fieldMeta);
				case 18:
					return getTextString(data, fieldMeta);
				case 19:
					return getUrlString(data, fieldMeta);
			}
		}
		//#region << Field data presentation >>
		//1.Auto increment
		function getAutoIncrementString(data, fieldMeta) {
			if (!data && data != 0) {
				return "";
			}
			else {
				if (fieldMeta.displayFormat) {
					return fieldMeta.displayFormat.replace("{0}", data);
				}
				else {
					return data;
				}
			}
		}
		//2.Checkbox
		function getCheckboxString(data, fieldMeta) {
			if (data == undefined) {
				return "undefined";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "undefined";
				}
				else if (data.length == 1) {
					return getCheckboxString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getCheckboxString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				if (data) {
					return "true";
				}
				else {
					return "false";
				}

			}



		}
		//3.Currency
		function getCurrencyString(data, fieldMeta) {
			if (!data && data != 0) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getCurrencyString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getCurrencyString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else if (fieldMeta.currency != null && fieldMeta.currency !== {} && fieldMeta.currency.symbol_native) {
				if (fieldMeta.currency.symbolPlacement === 1) {
					return escapeHtml(fieldMeta.currency.symbol_native + " " + $filter('number')(data, fieldMeta.currency.decimal_digits));
				}
				else {
					return escapeHtml($filter('number')(data, fieldMeta.currency.decimal_digits) + " " + fieldMeta.currency.symbol_native);
				}
			}
			else {
				return escapeHtml($filter('number')(data, fieldMeta.currency.decimal_digits));
			}
		}
		//4.Date
		function getDateString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getDateString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getDateString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				if (fieldMeta.format) {
					return moment(data).formatWithADF(fieldMeta.format);
				}
				else {
					return moment(data).formatWithADF("DD MMM YYYY");
				}
			}
		}
		//5.Datetime
		function getDateTimeString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getDateTimeString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getDateTimeString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return moment(data).format("DD MMM YYYY HH:mm");
			}

		}
		//6.Email
		function getEmailString(data, fieldMeta) {
			//There is a problem in Angular when having in href -> the href is not rendered
			//return "<a href='mailto:" + fieldValue + "' data-rel='external'>" + fieldValue + "</a>";
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getEmailString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getEmailString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return escapeHtml(data);
			}

		}
		//7.File
		function getFileString(data, fieldMeta) {
			if (!data) {
				return "no file";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "no file";
				}
				else if (data.length == 1) {
					return getFileString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getFileString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				var lastSlashIndex = data.lastIndexOf("/") + 1;
				var fileName = data.slice(lastSlashIndex, data.length);
				return "<a href='/fs" + data + "' target='_blank' class='link-icon'>" + escapeHtml(fileName) + "</a>";
			}
		}
		//8.Html
		function getHtmlString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getHtmlString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getHtmlString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return $sce.trustAsHtml(htmlString);
				}
			}
			else {
				return $sce.trustAsHtml(data);
			}
		}
		//9.Image
		function getImageString(data, fieldMeta) {
			if (!data) {
				return "<i class='fa fa-picture-o list-missing-image'></i>";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "<i class='fa fa-picture-o list-missing-image'></i>";
				}
				else if (data.length == 1) {
					return getImageString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getImageString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return "<a target='_blank' href='/fs" + data + "'><img src='/fs" + escapeHtml(data) + "?width=36' class='table-image'/></a>";
			}
		}
		//10. Textarea
		function getTextareaString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getTextareaString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getTextareaString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				//return data.replace(/(?:\r\n|\r|\n)/g, '<br />');
				return escapeHtml(data);
			}
		}
		//11.Multiselect
		function getMultiselectString(data, fieldMeta) {
			var generatedStringArray = [];
			if (!data && data != 0) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getMultiselectString(data[0], fieldMeta);
				}
				else {
					for (var i = 0; i < data.length; i++) {
						var selected = $filter('filter')(fieldMeta.options, { key: data[i] });
						generatedStringArray.push((data[i] && selected.length) ? selected[0].value : '');
					}
					return generatedStringArray.join(', ');
				}
			}
			else {
				var selected = $filter('filter')(fieldMeta.options, { key: data });
				return selected[0].value;
			}
		}
		//12. Number
		function getNumberString(data, fieldMeta) {
			if (!data && data != 0) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getNumberString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getNumberString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return data;
			}
		}
		//13. Password
		function getPasswordString(data, fieldMeta) {
			if (!data) {
				return "******";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "******";
				}
				else if (data.length == 1) {
					return "******";
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getPasswordString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return "******";
			}
		}
		//14.Percent
		function getPercentString(data, fieldMeta) {
			if (!data && data != 0) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getPercentString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getPercentString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return data + " = " + data * 100 + "%";
			}

		}
		//15. Phone
		function getPhoneString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getPhoneString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getPhoneString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				try {
					return phoneUtils.formatInternational(data, fieldMeta.format);
				}
				catch (err) {
					return "wrong number or format error";
				}
			}

		}
		//16. Guid
		function getGuidString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getGuidString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getGuidString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return data;
			}
		}
		//17.Dropdown
		function getDropdownString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getDropdownString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getDropdownString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				var selected = $filter('filter')(fieldMeta.options, { key: data });
				return (data && selected.length) ? selected[0].value : '';
			}
		}
		//18. Text
		function getTextString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getTextString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getTextString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return escapeHtml(data);
			}
		}
		//19.Url
		function getUrlString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getUrlString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getUrlString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				return data;
			}
		}
		//#endregion

		///////////////////////
		function initField(typeId) {
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
				case 20: //relation field not used in UI
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
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function updateField(putObject, entityId, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + putObject.id, data: putObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteField(fieldId, entityId, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + fieldId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#region << Views >>

		///////////////////////
		function initView(type) {
			var deleteAction = {
				"name": "wv_record_delete",
				"menu": "page-title-dropdown",
				"weight": 1,
				"template": "<a href=\"javascript:void(0)\" confirmed-click=\"ngCtrl.deleteRecord(ngCtrl)\" ng-confirm-click=\"{{'DELETE_CONFIRMATION_ALERT_MESSAGE' | translate}}\" \n\t\t ng-if=\"::ngCtrl.userHasRecordPermissions('canDelete')\"> \n\t <i class=\"fa fa-trash go-red\"></i> Delete Record \n </a>"
			};
			var createListAction = {
				"name": "wv_create_and_list",
				"menu": "create-bottom",
				"weight": 1,
				"template": "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"ngCtrl.createViewRegion != null\">Create</a>"
			};
			var createDetailsAction = {
				"name": "wv_create_and_details",
				"menu": "create-bottom",
				"weight": 2,
				"template": "<a class=\"btn btn-default  btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"ngCtrl.createViewRegion != null\">Create & Details</a>"
			};
			var createCancelAction = {
				"name": "wv_create_cancel",
				"menu": "create-bottom",
				"weight": 3,
				"template": "<a class=\"btn btn-default  btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>"
			};
			var backButtonAction = {
				"name": "wv_back_button",
				"menu": "sidebar-top",
				"weight": 1,
				"template": "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>"
			};
			var rvAddNewAction = {
				"name": "wv_recursive_view_add_new",
				"menu": "sidebar-top",
				"weight": 1,
				"template": "<a href=\"javascript:void(0)\" class=\"btn btn-sm btn-outline\" ng-if=\"::canCreate\" ng-click=\"manageRelatedRecordItem(null)\"><i class=\"fa fa-plus\"></i> Create & Add</a>"
			};
			var rvAddExistingAction = {
				"name": "wv_recursive_view_add_existing",
				"menu": "recursive-view-title",
				"weight": 2,
				"template": "<a href=\"javascript:void(0)\" class=\"btn btn-sm btn-outline\" ng-if=\"::canAddExisting\" ng-click=\"addExistingItem()\"><i class=\"fa fa-download\"></i> Add existing</a>"
			};

			var rvEditAction = {
				"name": "wv_recursive_view_edit",
				"menu": "recursive-list-record-row",
				"weight": 1,
				"template": "<a href=\"javascript:void(0)\" title=\"quick edit\" class=\"btn btn-sm btn-outline\" ng-click=\"manageRelatedRecordItem(recordData)\" ng-if=\"::canUpdate\"><i class=\"fa fa-pencil\"></i></a>"
			};
			var rvUnrelateAction = {
				"name": "wv_recursive_view_unrelate",
				"menu": "recursive-list-record-row",
				"weight": 2,
				"template": "<a href=\"javascript:void(0)\" title=\"remove relation\" class=\"btn btn-sm btn-outline\" confirmed-click=\"instantDetachRecord(recordData)\" ng-confirm-click=\"Are you sure that you need this relation removed?\" ng-if=\"::canRemove\"><i class=\"fa fa-times go-red\"></i></a>"
			};


			var view = {
				"id": null,
				"name": "",
				"label": "",
				"default": false,
				"system": false,
				"weight": 10,
				"cssClass": "",
				"dynamicHtmlTemplate": null,
				"dataSourceUrl": null,
				"relationOptions": [],
				"type": "general",
				"iconName": "file-text-o",
				"regions": [
					{
						"name": "header",
						"label": "Header",
						"render": true,
						"cssClass": "",
						"weight": 1,
						"sections": []
					}
				],
				"sidebar": {
					"render": false,
					"cssClass": "",
					"items": []
				},
				"serviceCode": null,
				"actionItems": []
			};

			switch (type) {
				case "general":
					view.actionItems.push(backButtonAction);
					view.actionItems.push(deleteAction);
					view.actionItems.push(rvAddNewAction);
					view.actionItems.push(rvAddExistingAction);
					view.actionItems.push(rvEditAction);
					view.actionItems.push(rvUnrelateAction);
					break;
				case "create":
					view.actionItems.push(backButtonAction);
					view.actionItems.push(createListAction);
					view.actionItems.push(createDetailsAction);
					view.actionItems.push(createCancelAction);
					break;
				case "quick_view":
					view.actionItems.push(backButtonAction);
					view.actionItems.push(deleteAction);
					break;
				case "quick_create":
					view.actionItems.push(backButtonAction);
					view.actionItems.push(createListAction);
					view.actionItems.push(createDetailsAction);
					view.actionItems.push(createCancelAction);
					break;
				case "hidden":
					view.actionItems.push(backButtonAction);
					view.actionItems.push(createListAction);
					view.actionItems.push(createDetailsAction);
					view.actionItems.push(createCancelAction);
					view.actionItems.push(deleteAction);
					break;
				default:
					break;
			}

			return view;
		}
		////////////////////////
		function initViewRegion() {
			var region = {
				"name": "",
				"label": "",
				"render": true,
				"cssClass": "",
				"weight": 10,
				"sections": []
			}
			return region;
		}
		////////////////////////
		function initViewSection() {
			var section = {
				"id": newGuid(),
				"name": "general",
				"label": "General",
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
				"id": newGuid(),
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
		function getViewMenuOptions() {
			var menuOptions = [
				{
					key: "hidden",
					value: "hidden",
					description: ""
				},
				{
					key: "create-bottom",
					value: "create-bottom",
					description: ""
				},
				{
					key: "page-bottom",
					value: "page-bottom",
					description: ""
				},
				{
					key: "page-title",
					value: "page-title",
					description: ""
				},
				{
					key: "page-title-dropdown",
					value: "page-title-dropdown",
					description: ""
				},
				{
					key: "sidebar-top",
					value: "sidebar-top",
					description: ""
				},
				{
					key: "recursive-view-title",
					value: "recursive-view-title",
					description: ""
				},
				{
					key: "recursive-view-record-row",
					value: "recursive-view-record-row",
					description: ""
				}

			];

			return menuOptions;
		}
		/////////////////////
		function initViewActionItem() {
			var actionItem = {
				name: null,
				weight: 10,
				menu: "hidden",
				template: ""
			}
			return actionItem;
		}

		function initViewActionItemTemplate(actionName) {
			var actionItem = initViewActionItem();
			switch (actionName) {
				case "wv_record_delete":
					actionItem.name = "wv_record_delete";
					actionItem.menu = "page-title-dropdown";
					actionItem.weight = 1;
					actionItem.template = "<a href=\"javascript:void(0)\" confirmed-click=\"::ngCtrl.deleteRecord(ngCtrl)\" ng-confirm-click=\"Are you sure?\" ng-if=\"::ngCtrl.userHasRecordPermissions('canDelete')\"><i class=\"fa fa-trash go-red\"></i> Delete Record</a>";
					break;
				case "wv_manage_view":
					actionItem.name = "wv_manage_view";
					actionItem.menu = "page-title-dropdown";
					actionItem.weight = 101;
					actionItem.template = "<a target=\"_blank\" ng-href=\"{{ngCtrl.getViewManageUrl()}}\" ng-if=\"::ngCtrl.userIsAdmin()\">\n\t<i class=\"fa fa-fw fa-cog\"></i> Manage View\n</a>";
					break;
				case "wv_create_and_list":
					actionItem.name = "wv_create_and_list";
					actionItem.menu = "create-bottom";
					actionItem.weight = 1;
					actionItem.template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
					break;
				case "wv_create_and_details":
					actionItem.name = "wv_create_and_details";
					actionItem.menu = "create-bottom";
					actionItem.weight = 2;
					actionItem.template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
					break;
				case "wv_create_cancel":
					actionItem.name = "wv_create_cancel";
					actionItem.menu = "create-bottom";
					actionItem.weight = 3;
					actionItem.template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
					break;
				case "wv_back_button":
					actionItem.name = "wv_back_button";
					actionItem.menu = "sidebar-top";
					actionItem.weight = 1;
					actionItem.template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
					break;
				case "wv_recursive_view_add_existing":
					actionItem.name = "wv_recursive_view_add_existing";
					actionItem.menu = "recursive-view-title";
					actionItem.weight = 1;
					actionItem.template = "<a href=\"javascript:void(0)\" class=\"btn btn-sm btn-outline\" ng-if=\"::canAddExisting\" ng-click=\"addExistingItem()\"><i class=\"fa fa-download\"></i> Add existing</a>";
					break;
				case "wv_recursive_view_add_new":
					actionItem.name = "wv_recursive_view_add_new";
					actionItem.menu = "recursive-view-title";
					actionItem.weight = 2;
					actionItem.template = "<a href=\"javascript:void(0)\" class=\"btn btn-sm btn-outline\" ng-if=\"::canCreate\" ng-click=\"manageRelatedRecordItem(null)\"><i class=\"fa fa-plus\"></i> Create & Add</a>";
					break;
				case "wv_recursive_view_edit":
					actionItem.name = "wv_recursive_view_edit";
					actionItem.menu = "recursive-view-record-row";
					actionItem.weight = 2;
					actionItem.template = "<a href=\"javascript:void(0)\" title=\"quick edit\" class=\"btn btn-sm btn-outline\" ng-click=\"manageRelatedRecordItem(recordData)\" ng-if=\"::canUpdate\"><i class=\"fa fa-pencil\"></i></a>";
					break;
				case "wv_recursive_view_unrelate":
					actionItem.name = "wv_recursive_view_unrelate";
					actionItem.menu = "recursive-view-record-row";
					actionItem.weight = 3;
					actionItem.template = "<a href=\"javascript:void(0)\" title=\"remove relation\" class=\"btn btn-sm btn-outline\" confirmed-click=\"instantDetachRecord(recordData)\" ng-confirm-click=\"Are you sure that you need this relation broken?\" ng-if=\"::canRemove\"><i class=\"fa fa-times go-red\"></i></a>";
					break;
			}

			return actionItem;
		}

		//////////////////////
		function getEntityViewLibrary(entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/getEntityViewLibrary' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function getEntityView(viewName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getEntityRecordViewFromEntitiesMetaList(viewName, entityName, entitiesMetaList) {
			var itemMeta = null;
			for (var i = 0; i < entitiesMetaList.length; i++) {
				if (entitiesMetaList[i].name == entityName) {
					for (var j = 0; j < entitiesMetaList[i].recordViews.length; j++) {
						if (entitiesMetaList[i].recordViews[j].name == viewName) {
							itemMeta = entitiesMetaList[i].recordViews[j];
							break;
						}
					}
					break;
				}
			}
			return itemMeta;
		}
		//////////////////////
		function createEntityView(viewObj, entityName, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/view", data: viewObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function updateEntityView(viewObj, entityName, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewObj.name, data: viewObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function patchEntityView(viewObj, viewName, entityName, successCallback, errorCallback) {
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName, data: viewObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteEntityView(viewName, entityName, successCallback, errorCallback) {
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
							//case 2.3.1 - if the new weight is bigger then the old -> element is pushed back, the between elements should lose weight
							if (parseInt(updateObject.weight) > parseInt(originalWeight)) {
								currentElement.weight = parseInt(currentElement.weight) - 1;
								resultArray.push(currentElement);
							}
								//case 2.3.1 - if the new weight is smaller then the old -> element is pushed forward, the between elements should gain weight
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
		/////////////////////
		function getRowColumnCountVariationsArray() {
			var rowColCountVariantions = [
					  {
						key: 1,
						columns: 1,
						value: "One column"
					  },
					  {
						key: 2,
						columns: 2,
						value: "Two columns"
					  },
					  {
						key: 3,
						columns: 3,
						value: "Three columns"
					  },
					  {
						key: 4,
						columns: 4,
						value: "Four columns"
					  },
					  {
						key: 12,
						columns: 2,
						value: "1-2 columns"
					  },
					  {
						key: 13,
						columns: 2,
						value: "1-3 columns"
					  },
					  {
						key: 15,
						columns: 2,
						value: "1-5 columns"
					  },
					  {
						key: 21,
						columns: 2,
						value: "2-1 columns"
					  },
					  {
						key: 31,
						columns: 2,
						value: "3-1 columns"
					  },
					  {
						key: 51,
						columns: 2,
						value: "5-1 columns"
					  }
			];

			return rowColCountVariantions;
		}
		////////////////////
		function getRowColumnCountVariationKey(row) {
			var gridColCountArray = [];
			for (var j = 0; j < row.columns.length; j++) {
				gridColCountArray.push(row.columns[j].gridColCount);
			}

			if (arraysEqual(gridColCountArray, [12])) {
				return 1;
			}
			else if (arraysEqual(gridColCountArray, [6, 6])) {
				return 2;
			}
			else if (arraysEqual(gridColCountArray, [4, 4, 4])) {
				return 3;
			}
			else if (arraysEqual(gridColCountArray, [3, 3, 3, 3])) {
				return 4;
			}
			else if (arraysEqual(gridColCountArray, [4, 8])) {
				return 12;
			}
			else if (arraysEqual(gridColCountArray, [3, 9])) {
				return 13;
			}
			else if (arraysEqual(gridColCountArray, [2, 10])) {
				return 15;
			}
			else if (arraysEqual(gridColCountArray, [8, 4])) {
				return 21;
			}
			else if (arraysEqual(gridColCountArray, [9, 3])) {
				return 31;
			}
			else if (arraysEqual(gridColCountArray, [10, 2])) {
				return 51;
			}
		}
		///////////////////
		function convertRowColumnCountVariationKeyToArray(key) {
			switch (key) {
				case 1:
					return [12];
				case 2:
					return [6, 6];
				case 3:
					return [4, 4, 4];
				case 4:
					return [3, 3, 3, 3];
				case 12:
					return [4, 8];
				case 13:
					return [3, 9];
				case 15:
					return [2, 10];
				case 21:
					return [8, 4];
				case 31:
					return [9, 3];
				case 51:
					return [10, 2];
			}
		}
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
		/////////////////////
		function getSafeViewNameAndEntityName(paramsViewName, paramsEntityName, relationsList) {
			var data = {};
			//if the list is in a view, than the name should be processed as the entity name could differ from the current one as well as the list name is not in fact a dataName
			//e.g. $list$project_1_n_milestone$general	when from another entity or $list$lookup when from the current
			data.viewName = paramsViewName;
			data.entityName = paramsEntityName;
			var viewDataName = paramsViewName;
			var viewDataNameArray = viewDataName.split("$");
			if (viewDataNameArray.length == 3) {
				//this is a list from the current entity ($view$general), we just need to get the proper list name
				data.viewName = viewDataNameArray[2];
			}
			else if (viewDataNameArray.length == 4) {
				//this is a list from another entity ($view$project_1_n_milestone$general), we should get both list name and entity name from the relation
				var relationName = viewDataNameArray[2];
				var relation = getRelationFromRelationsList(relationName, relationsList);
				if (relation.originEntityName == paramsEntityName) {
					data.entityName = relation.targetEntityName;
				}
				else if (relation.targetEntityName == paramsEntityName) {
					data.entityName = relation.originEntityName;
				}
				data.viewName = viewDataNameArray[3];
			}

			return data;
		}


		//#endregion

		//#region << List >>

		///////////////////////
		function initList() {
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
					"columns": [],
					"query": null,
					"sorts": null,
					"dynamicHtmlTemplate": null,
					"dataSourceUrl": null,
					"columnWidthsCSV": null,
					"relationOptions": [],
					"serviceCode": null,
					"actionItems": [
						{
							"name": "wv_create_record",
							"menu": "page-title",
							"weight": 1,
							"template": "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\"\n ng-href=\"{{ngCtrl.getRecordCreateUrl(ngCtrl)}}\">Add New</a>"
						},
						{
							"name": "wv_import_records",
							"menu": "page-title-dropdown",
							"weight": 10,
							"template": "<a ng-click=\"ngCtrl.openImportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-upload\"></i> Import CSV\n</a>"
						},
						{
							"name": "wv_export_records",
							"menu": "page-title-dropdown",
							"weight": 11,
							"template": "<a ng-click=\"ngCtrl.openExportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-download\"></i> Export CSV\n</a>"
						},
						{
							"name": "wv_record_details",
							"menu": "record-row",
							"weight": 1,
							"template": "<a class=\"btn btn-default btn-outline\" ng-href=\"{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}\">\n\t<i class=\"fa fa-fw fa-eye\"></i>\n</a>"
						},
						{
							"name": "wv_recursive_list_add_existing",
							"menu": "recursive-list-title",
							"weight": 1,
							"template": "<a href=\"javascript:void(0)\" class=\"btn btn-outline btn-sm\" ng-if=\"::canAddExisting\" ng-click=\"addExistingItem()\"><i class=\"fa fa-download\"></i> Add existing</a>"
						},
						{
							"name": "wv_recursive_list_add_new",
							"menu": "recursive-list-title",
							"weight": 2,
							"template": "<a href=\"javascript:void(0)\" class=\"btn btn-outline btn-sm\" ng-if=\"::canCreate\" ng-click=\"manageRelatedRecordItem(null)\"><i class=\"fa fa-plus\"></i> Create & Add</a>"
						},
						{
							"name": "wv_recursive_list_view",
							"menu": "recursive-list-record-row",
							"weight": 1,
							"template": "<a href=\"javascript:void(0)\" title=\"quick view this record\" class=\"btn btn-sm btn-outline\" ng-click=\"viewRelatedRecordItem(record)\"><i class=\"fa fa-eye\"></i></a>"
						},
						{
							"name": "wv_recursive_list_edit",
							"menu": "recursive-list-record-row",
							"weight": 2,
							"template": "<a href=\"javascript:void(0)\" title=\"quick edit this record\" class=\"btn btn-sm btn-outline\" ng-click=\"manageRelatedRecordItem(record)\" ng-if=\"::canUpdate\"><i class=\"fa fa-pencil\"></i></a>"
						},
						{
							"name": "wv_recursive_list_unrelate",
							"menu": "recursive-list-record-row",
							"weight": 3,
							"template": "<a href=\"javascript:void(0)\" title=\"Detach records relation\" class=\"btn btn-sm btn-outline\" confirmed-click=\"instantDetachRecord(record)\" ng-confirm-click=\"Are you sure that you need this relation removed?\" ng-if=\"::canRemove\"><i class=\"fa fa-times go-red\"></i></a>"
						}
					],
				}
			return list;
		}
		//////////////////////
		function getListMenuOptions() {
			var menuOptions = [
				{
					key: "hidden",
					value: "hidden",
					description: ""
				},
				{
					key: "page-bottom",
					value: "page-bottom",
					description: ""
				},
				{
					key: "page-title",
					value: "page-title",
					description: ""
				},
				{
					key: "page-title-dropdown",
					value: "page-title-dropdown",
					description: ""
				},
				{
					key: "record-row",
					value: "record-row",
					description: ""
				},
				{
					key: "record-row-dropdown",
					value: "record-row-dropdown",
					description: ""
				},
				{
					key: "recursive-list-title",
					value: "recursive-list-title",
					description: ""
				},
				{
					key: "recursive-list-record-row",
					value: "recursive-list-record-row",
					description: ""
				}
			];

			return menuOptions;
		}
		/////////////////////
		function initListActionItem() {
			var actionItem = {
				name: null,
				weight: 10,
				menu: "hidden",
				template: ""
			}
			return actionItem;
		}

		function initListActionItemTemplate(actionName) {
			var actionItem = initListActionItem();
			switch (actionName) {
				case "wv_create_record":
					actionItem.name = "wv_create_record";
					actionItem.menu = "page-title";
					actionItem.weight = 1;
					actionItem.template = "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\"\n ng-href=\"{{ngCtrl.getRecordCreateUrl(ngCtrl)}}\">Add New</a>";
					break;
				case "wv_import_records":
					actionItem.name = "wv_import_records";
					actionItem.menu = "page-title-dropdown";
					actionItem.weight = 10;
					actionItem.template = "<a ng-click=\"ngCtrl.openImportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-upload\"></i> Import CSV\n</a>";
					break;
				case "wv_export_records":
					actionItem.name = "wv_export_records";
					actionItem.menu = "page-title-dropdown";
					actionItem.weight = 11;
					actionItem.template = "<a ng-click=\"ngCtrl.openExportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-download\"></i> Export CSV\n</a>";
					break;
				case "wv_manage_list":
					actionItem.name = "wv_manage_list";
					actionItem.menu = "page-title-dropdown";
					actionItem.weight = 101;
					actionItem.template = "<a target=\"_blank\" ng-href=\"{{ngCtrl.getListManageUrl()}}\" ng-if=\"::ngCtrl.userIsAdmin()\">\n\t<i class=\"fa fa-fw fa-cog\"></i> Manage List\n</a>";
					break;
				case "wv_record_details":
					actionItem.name = "wv_record_details";
					actionItem.menu = "record-row";
					actionItem.weight = 1;
					actionItem.template = "<a class=\"btn btn-default btn-outline\" ng-href=\"{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}\">\n\t<i class=\"fa fa-fw fa-eye\"></i>\n</a>";
					break;
				case "wv_recursive_list_add_existing":
					actionItem.name = "wv_recursive_list_add_existing";
					actionItem.menu = "recursive-list-title";
					actionItem.weight = 1;
					actionItem.template = "<a href=\"javascript:void(0)\" class=\"btn btn-outline btn-sm\" ng-if=\"::canAddExisting\" ng-click=\"addExistingItem()\"><i class=\"fa fa-download\"></i> Add existing</a>";
					break;
				case "wv_recursive_list_add_new":
					actionItem.name = "wv_recursive_list_add_new";
					actionItem.menu = "recursive-list-title";
					actionItem.weight = 2;
					actionItem.template = "<a href=\"javascript:void(0)\" class=\"btn btn-outline btn-sm\" ng-if=\"::canCreate\" ng-click=\"manageRelatedRecordItem(null)\"><i class=\"fa fa-plus\"></i> Create & Add</a>";
					break;
				case "wv_recursive_list_view":
					actionItem.name = "wv_recursive_list_view";
					actionItem.menu = "recursive-list-record-row";
					actionItem.weight = 1;
					actionItem.template = "<a href=\"javascript:void(0)\" title=\"quick view this record\" class=\"btn btn-sm btn-outline\" ng-click=\"viewRelatedRecordItem(record)\"><i class=\"fa fa-eye\"></i></a>";
					break;
				case "wv_recursive_list_edit":
					actionItem.name = "wv_recursive_list_edit";
					actionItem.menu = "recursive-list-record-row";
					actionItem.weight = 2;
					actionItem.template = "<a href=\"javascript:void(0)\" title=\"quick edit this record\" class=\"btn btn-sm btn-outline\" ng-click=\"manageRelatedRecordItem(record)\" ng-if=\"::canUpdate\"><i class=\"fa fa-pencil\"></i></a>";
					break;
				case "wv_recursive_list_unrelate":
					actionItem.name = "wv_recursive_list_unrelate";
					actionItem.menu = "recursive-list-record-row";
					actionItem.weight = 3;
					actionItem.template = "<a href=\"javascript:void(0)\" title=\"Detach records relation\" class=\"btn btn-sm btn-outline\" confirmed-click=\"instantDetachRecord(record)\" ng-confirm-click=\"Are you sure that you need this relation broken?\" ng-if=\"::canRemove\"><i class=\"fa fa-times go-red\"></i></a>";
					break;
			}

			return actionItem;
		}
		///////////////////////
		function getEntityRecordLists(entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityRecordList(listName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityRecordListFromEntitiesMetaList(listName, entityName, entitiesMetaList) {
			var itemMeta = null;
			for (var i = 0; i < entitiesMetaList.length; i++) {
				if (entitiesMetaList[i].name == entityName) {
					for (var j = 0; j < entitiesMetaList[i].recordLists.length; j++) {
						if (entitiesMetaList[i].recordLists[j].name == listName) {
							itemMeta = entitiesMetaList[i].recordLists[j];
							break;
						}
					}
					break;
				}
			}
			return itemMeta;
		}
		//////////////////////
		function createEntityRecordList(submitObj, entityName, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list", data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function patchEntityRecordList(submitObj, listName, entityName, successCallback, errorCallback) {
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list/" + listName, data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function updateEntityRecordList(listObj, entityName, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list/" + listObj.name, data: listObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteEntityRecordList(listName, entityName, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		/////////////////////
		function extractSupportedFilterFields(recordList) {
			var result = {};
			result.fieldNames = [];
			result.queryTypes = [];
			if (recordList.meta.query == null || recordList.meta.query.length == 0) {
				return result;
			}
			var supportedFields = extractFieldsFromQuery(recordList.meta.query, result);
			return supportedFields;
		}
		/////////////////////
		function extractFieldsFromQuery(query, result) {
			if (query.fieldValue != null && query.fieldValue.trim().startsWith("{")) {
				var queryObject = angular.fromJson(query.fieldValue);
				if (queryObject.name == "url_query" && queryObject.option) {
					//option should equal fieldName in order to preset field to work
					if (queryObject.option == query.fieldName) {
						result.fieldNames.push(query.fieldName);
						result.queryTypes.push(query.queryType);
					}
				}
			}
			if (query.subQueries.length > 0) {
				for (var i = 0; i < query.subQueries.length; i++) {
					extractFieldsFromQuery(query.subQueries[i], result);
				}
			}

			return result;
		}
		/////////////////////
		function getSafeListNameAndEntityName(paramsListName, paramsEntityName, relationsList) {
			var data = {};
			//if the list is in a view, than the name should be processed as the entity name could differ from the current one as well as the list name is not in fact a dataName
			//e.g. $list$project_1_n_milestone$general	when from another entity or $list$lookup when from the current
			data.listName = paramsListName;
			data.entityName = paramsEntityName;
			var listDataName = paramsListName;
			var listDataNameArray = listDataName.split("$");
			if (listDataNameArray.length == 3) {
				//this is a list from the current entity ($list$lookup), we just need to get the proper list name
				data.listName = listDataNameArray[2];
			}
			else if (listDataNameArray.length == 4) {
				//this is a list from another entity ($list$project_1_n_milestone$general), we should get both list name and entity name from the relation
				var relationName = listDataNameArray[2];
				var relation = getRelationFromRelationsList(relationName, relationsList);
				if (relation.originEntityName == paramsEntityName) {
					data.entityName = relation.targetEntityName;
				}
				else if (relation.targetEntityName == paramsEntityName) {
					data.entityName = relation.originEntityName;
				}
				data.listName = listDataNameArray[3];
			}

			return data;
		}

		//#endregion

		//#region << Tree >>

		//Additionally in the field is set
		//SelectionType - single-select,multi-select,single-branch-select
		//SelectionTarget - all,leaves

		function initTree() {
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
				"nodeWeightFieldId": null, //Only certain types should be allowed here - human readable node label
				"rootNodes": [],
				"nodeObjectProperties": []
			}
			return tree;
		}

		///////////////////////
		function getEntityTreesMeta(entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/tree' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityTreeMeta(treeName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/tree/' + treeName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function createEntityTree(submitObj, entityName, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/tree", data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function patchEntityTree(submitObj, treeName, entityName, successCallback, errorCallback) {
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/tree/" + treeName, data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//////////////////////
		function updateEntityTree(treeObj, entityName, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/tree/" + treeObj.name, data: treeObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function deleteEntityTree(treeName, entityName, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/tree/' + treeName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#region << Relations >>

		///////////////////////
		function initRelation() {
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
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + name }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRelationsList(successCallback, errorCallback) {
			var $storage = $localStorage;
			if ($storage.entityRelationsList && $storage.entityRelationsList.hash) {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/list?hash=' + $storage.entityRelationsList.hash })
					.then(function getSuccessCallback(response) {
						if (response.data.object != null) {
							var localStorageEntityRelations = {};
							localStorageEntityRelations.hash = response.data.hash;
							localStorageEntityRelations.list = response.data.object;
							$storage.entityRelationsList = localStorageEntityRelations;
						}
						else {
							response.data.object = $storage.entityRelationsList.list;
						}
						handleSuccessResult(response.data, response.status, successCallback, errorCallback);
					}, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/list' })
					.then(function getSuccessCallback(response) {
						var localStorageEntityRelations = {};
						localStorageEntityRelations.hash = response.data.hash;
						localStorageEntityRelations.list = response.data.object;
						$storage.entityRelationsList = localStorageEntityRelations;
						handleSuccessResult(response.data, response.status, successCallback, errorCallback);
					}, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
		}
		///////////////////////
		function createRelation(postObject, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/relation', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function updateRelation(postObject, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + postObject.id, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteRelation(relationId, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/relation/' + relationId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRelationFromRelationsList(relationName, relationList) {
			var itemMeta = null;
			for (var i = 0; i < relationList.length; i++) {
				if (relationList[i].name == relationName) {
					itemMeta = relationList[i];
					break;
				}
			}
			return itemMeta;
		}

		//#endregion

		//#region << Records >>

		///////////////////////
		function createRecord(entityName, postObject, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		function createRecordWithRelation(entityName, postObject, relationName, parentRecordId, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/with-relation/' + relationName + '/' + parentRecordId, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function importEntityRecords(entityName, fileTempPath, clipboard, successCallback, errorCallback) {
			var postObject = {};
			postObject.fileTempPath = fileTempPath;
			postObject.clipboard = clipboard;
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/import', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function evaluateImportEntityRecords(entityName, fileTempPath, general_command, clipboard, commands, successCallback, errorCallback) {
			var postObject = {};
			postObject.fileTempPath = fileTempPath;
			postObject.clipboard = clipboard;
			postObject.commands = commands;
			postObject.general_command = general_command;
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/import-evaluate', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function exportListRecords(entityName, listName, count, queryParams, successCallback, errorCallback) {
			var path = wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + "/export?count=" + count;

			if (!isEmpty(queryParams)) {
				for (var param in queryParams) {
					path += "&" + param + "=" + queryParams[param];
				}
			}
			window.location = path;
			//$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + "/export?count=" + count }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			var response = {};
			response.data = {};
			response.data.success = true;
			response.status = 200;
			handleSuccessResult(response.data, response.status, successCallback, errorCallback);
		}
		///////////////////////
		function getRecord(recordId, fields, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId + "?fields=" + fields }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecordByViewName(recordId, viewName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/view/' + viewName + '/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecordByViewMeta(recordId, viewMeta, entityName, stateParams, successCallback, errorCallback) {
			if (viewMeta.dataSourceUrl == null || viewMeta.dataSourceUrl == '') {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/view/' + viewMeta.name + '/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				var extraParamQueryString = "?";
				var queryParamAux = [];
				//if data source already contains ?
				if (viewMeta.dataSourceUrl.indexOf("?") !== -1) {
					extraParamQueryString = "&";
				}
				extraParamQueryString += "entityName=" + entityName + "&";
				extraParamQueryString += "listName=" + viewMeta.name + "&";
				extraParamQueryString += "recordId=" + recordId;
				//Add stateParams
				if (stateParams != null && !isEmpty(stateParams)) {
					for (var param in stateParams) {
						if (stateParams[param] && extraParamQueryString.indexOf(param) == -1) {
							queryParamAux.push(param + "=" + stateParams[param]);
						}
					}
				}
				//Add locationParams with override of stateParams. Can be added to the view URL
				//IMPORTANT: this is not possible at this state as $location returns the params of the old state. before the change and problems could occur
				//var locationParams = $location.search();
				//if(locationParams != null && !isEmpty(locationParams)){
				//    for (var param in locationParams) {
				//        if (locationParams[param]) {
				//			var auxParamFound = _.find(queryParamAux,function(record){
				//				var index = record.indexOf(param + "=");
				//				if(index == -1){
				//					return false;
				//				}
				//				else{
				//					return true;
				//				}
				//			});
				//			if(auxParamFound){
				//				//Remove the found elements
				//				_.remove(queryParamAux,function(record){
				//					var index = record.indexOf(param + "=");
				//					if(index == -1){
				//						return false;
				//					}
				//					else{
				//						return true;
				//					}
				//				});
				//			}
				//			queryParamAux.push(param + "=" + locationParams[param]);
				//        }
				//    }					
				//}
				if (queryParamAux.length > 0) {
					extraParamQueryString += "&";
					extraParamQueryString += queryParamAux.join("&");
				}


				$http({ method: 'GET', url: viewMeta.dataSourceUrl + extraParamQueryString }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
		}
		/////////////////////
		function getRecordsWithoutList(recordIds, fieldNames, limit, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list?ids=' + recordIds + "&fields=" + fieldNames + "&limit=" + limit }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecordsByListName(listName, entityName, page, extraParams, successCallback, errorCallback) {
			var extraParamQueryString = "";
			if (extraParams != null) {
				if (!isEmpty(extraParams)) {
					extraParamQueryString = "?";
					for (var param in extraParams) {
						extraParamQueryString += param + "=" + extraParams[param] + "&";
					}
					//remove the last &
					extraParamQueryString = extraParamQueryString.substring(0, extraParamQueryString.length - 1);
				}
			}
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + page + extraParamQueryString }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecordsByListMeta(listMeta, entityName, page, stateParams, extraParams, successCallback, errorCallback) {
			if (listMeta.dataSourceUrl == null || listMeta.dataSourceUrl == '') {
				var extraParamQueryString = "";
				if (extraParams != null) {
					if (!isEmpty(extraParams)) {
						extraParamQueryString = "?";
						for (var param in extraParams) {
							if (extraParamQueryString.indexOf(param + "=") == -1) {
								extraParamQueryString += param + "=" + extraParams[param] + "&";
							}
						}
						//remove the last &
						extraParamQueryString = extraParamQueryString.substring(0, extraParamQueryString.length - 1);
					}
				}
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listMeta.name + '/' + page + extraParamQueryString }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				var extraParamQueryString = "?";
				var queryParamAux = [];
				//if data source already contains ?
				if (listMeta.dataSourceUrl.indexOf("?") !== -1) {
					extraParamQueryString = "&";
				}
				extraParamQueryString += "entityName=" + entityName + "&";
				extraParamQueryString += "listName=" + listMeta.name + "&";
				extraParamQueryString += "page=" + page + "&";
				if (extraParams != null) {
					if (!isEmpty(extraParams)) {
						for (var param in extraParams) {
							if (extraParamQueryString.indexOf(param) == -1) {
								extraParamQueryString += param + "=" + extraParams[param] + "&";
							}
						}
					}
				}
				//Add stateParams
				if (stateParams != null && !isEmpty(stateParams)) {
					for (var param in stateParams) {
						if (stateParams[param] && extraParamQueryString.indexOf(param) == -1) {
							queryParamAux.push(param + "=" + stateParams[param]);
						}
					}
				}
				//Add locationParams with override of stateParams. Can be added to the view URL. 
				//IMPORTANT: this is not possible at this state as $location returns the params of the old state. before the change and problems could occur
				//var locationParams = $location.search();
				//if(locationParams != null && !isEmpty(locationParams)){
				//    for (var param in locationParams) {
				//        if (locationParams[param]) {
				//			var auxParamFound = _.find(queryParamAux,function(record){
				//				var index = record.indexOf(param + "=");
				//				if(index == -1){
				//					return false;
				//				}
				//				else{
				//					return true;
				//				}
				//			});
				//			if(auxParamFound){
				//				//Remove the found elements
				//				_.remove(queryParamAux,function(record){
				//					var index = record.indexOf(param + "=");
				//					if(index == -1){
				//						return false;
				//					}
				//					else{
				//						return true;
				//					}
				//				});
				//			}
				//			queryParamAux.push(param + "=" + locationParams[param]);
				//        }
				//    }					
				//}
				if (queryParamAux.length > 0) {
					extraParamQueryString += "&";
					extraParamQueryString += queryParamAux.join("&");
				}


				$http({ method: 'GET', url: listMeta.dataSourceUrl + extraParamQueryString }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
		}
		///////////////////////
		function getRecordsByTreeName(treeName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/tree/' + treeName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecordsByFieldAndRegex(entityName, fieldName, postObject, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/regex/' + fieldName, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function updateRecord(recordId, entityName, patchObject, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId, data: patchObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function patchRecord(recordId, entityName, patchObject, successCallback, errorCallback) {
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId, data: patchObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//////////////////////
		function deleteRecord(recordId, entityName, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function uploadFileToTemp(file, progressCallback, successCallback, errorCallback) {
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
			var postObject = {
				source: source,
				target: target,
				overwrite: overwrite
			}
			$http({ method: 'POST', url: '/fs/move', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteFileFromFS(filepath, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: filepath }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		/////////////////////////
		function updateRecordRelation(relationName, originFieldRecordId, attachTargetFieldRecordIds, detachTargetFieldRecordIds, successCallback, errorCallback) {
			var postObject = {
				relationName: relationName,//string
				originFieldRecordId: originFieldRecordId, //guid
				attachTargetFieldRecordIds: attachTargetFieldRecordIds, //guid array - list of recordIds that needs to be attached to the new origin
				detachTargetFieldRecordIds: detachTargetFieldRecordIds  //guid array - list of recordIds that needs to be detached to the new origin - should be empty array when the target field is required
			}
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/relation', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		/////////////////////////
		function updateRecordRelationReverse(relationName, targetFieldRecordId, attachOriginFieldRecordIds, detachOriginFieldRecordIds, successCallback, errorCallback) {
			var postObject = {
				relationName: relationName,//string
				targetFieldRecordId: targetFieldRecordId, //guid
				attachOriginFieldRecordIds: attachOriginFieldRecordIds, //guid array - list of recordIds that needs to be attached to the new origin
				detachOriginFieldRecordIds: detachOriginFieldRecordIds  //guid array - list of recordIds that needs to be detached to the new origin - should be empty array when the target field is required
			}
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/relation/reverse', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		////////////////////////
		function generateHighlightString(elementMeta, elementData, stateParams, targetType) {
			if (targetType == "title") {
				if (elementMeta.title != null && elementMeta.title != "") {
					return substituteTagsWithData(elementMeta.title, elementData, stateParams);
				}
				else if (elementMeta.label != null && elementMeta.label != "") {
					return substituteTagsWithData(elementMeta.label, elementData, stateParams);
				}
				else {
					return substituteTagsWithData(elementMeta.name, elementData, stateParams);
				}
			}
			else {
				if (elementMeta.label != null && elementMeta.label != "") {
					return substituteTagsWithData(elementMeta.label, elementData, stateParams);
				}
				else {
					return substituteTagsWithData(elementMeta.name, elementData, stateParams);
				}
			}
		}

		function substituteTagsWithData(processedString, elementData, stateParams) {
			if (processedString.indexOf('{') != -1 && processedString.indexOf('}') != -1) {
				var arrayOfTemplateKeys = processedString.match(/\{([\$\w\[\]]+)\}/g); //Include support for matching also data from relations which include $ symbol
				var resultStringStorage = processedString;

				for (var i = 0; i < arrayOfTemplateKeys.length; i++) {
					if (arrayOfTemplateKeys[i] === "{areaName}" || arrayOfTemplateKeys[i] === "{entityName}") {
						switch (arrayOfTemplateKeys[i]) {
							case "{areaName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], stateParams.areaName);
								break;
							case "{entityName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], stateParams.entityName);
								break;
						}
					}
					else if (elementData != null) {
						//Extract the dataName from string by removing the leading and the closing {}
						var dataName = arrayOfTemplateKeys[i].replace('{', '').replace('}', '');
						//If it is a relation it should be possible to have an index required to select the data - example $field$relation_name$field_name[0]
						var arrayIndices = dataName.match(/\[([\w]+)\]/g);
						//We support only one level of drill down so we will use only the first of any matched such indexes
						var arrayIndex = null;
						if (arrayIndices != null && arrayIndices.length > 0) {
							var indexString = arrayIndices[0];
							arrayIndex = _.toInteger(indexString.replace("[", "").replace("]", ""));
						}
						var dataNameFromRelation = "$" + dataName.substring(dataName.indexOf('['), 1);
						//Check template has corresponding list data value
						if (elementData[dataName]) {
							//YES -> check the value of this dataName and substitute with it in the string, even if it is null (toString)
							//Case 1 - data is not from relation (not starting with $)
							if (!dataName.startsWith('$')) {
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], elementData[dataName].toString());
							}
							else {
								//Case 2 - relation field
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], elementData[dataName][0].toString());
							}

						}
							//Could be a relation field
						else if (elementData[dataNameFromRelation]) {
							resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], elementData[dataNameFromRelation][arrayIndex].toString());
						}
						else {
							//Try to look for other views from this entity that may have this value
							var deepScanMatchFound = false;
							for (var dataObject in elementData) {
								if (dataObject.indexOf('$view$') != -1) {
									if (elementData[dataObject][0][dataName]) {
										resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], elementData[dataObject][0][dataName].toString());
										deepScanMatchFound = true;
										break;
									}
								}
							}
							//NO -> substitute the template key with the dataName only, as no value could be extracted
							if (!deepScanMatchFound) {
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], dataName);
							}
						}
					}

				}
				return resultStringStorage;
			}
			else {
				return processedString;
			}
		}


		function quickSearch(query, entityName, lookupFieldsCsv, sortField, sortType, returnFieldsCsv, matchMethod, matchAllFields, skipRecords, limitRecords, findType, forceFiltersCsv, successCallback, errorCallback) {
			var paramsObject = {};
			paramsObject.query = query;
			paramsObject.entityName = entityName;
			paramsObject.lookupFieldsCsv = lookupFieldsCsv;
			paramsObject.sortField = sortField;
			paramsObject.sortType = sortType;
			paramsObject.returnFieldsCsv = returnFieldsCsv;
			paramsObject.matchMethod = matchMethod;
			paramsObject.matchAllFields = matchAllFields;
			paramsObject.skipRecords = skipRecords;
			paramsObject.limitRecords = limitRecords;
			paramsObject.findType = findType;
			paramsObject.forceFiltersCsv = forceFiltersCsv;
			$http({ method: 'GET', url: efreaPluginUrlApi + "reserved_item/list", params: paramsObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#region << Site >>

		///////////////////////
		function registerHookListener(eventHookName, currentScope, executeOnHookFunction) {
			if (executeOnHookFunction === undefined || typeof executeOnHookFunction != "function") {
				$translate(['EVENT_HOOKS_REGISTER_ERROR']).then(function (translations) {
					alert(translations.EVENT_HOOKS_REGISTER_ERROR);
				});
				return;
			}
			//When registering listener with $on, it returns automatically a function that can remove this listener. We will use it later
			var unregisterFunc = $rootScope.$on(eventHookName, function (event, data) {
				executeOnHookFunction(event, data);
			});
			//The listener should be manually removed as the rootScope is never destroyed, and this will lead to duplication the next time the controller is loaded
			currentScope.$on("$destroy", function () {
				unregisterFunc();
			});
		}
		/////////////////////
		function launchHook(eventHookName, data) {
			$timeout(function () {
				$rootScope.$emit(eventHookName, data);
			}, 0);
		}
		///////////////////////
		function setPageTitle(pageTitle) {
			$timeout(function () {
				$rootScope.$emit("application-pageTitle-update", pageTitle);
			}, 0);
		}
		//////////////////////
		function setBodyColorClass(color) {
			$rootScope.$emit("application-body-color-update", color);
		}
		/////////////////////
		function initValidationObject() {
			var validation = {};
			validation.message = null;
			validation.isInvalid = false;
			validation.errors = {};
			return validation;
		}
		////////////////////
		function setValidationError(validationObj, generalMessage, fieldName, fieldMessage) {
			validationObj.isInvalid = true;
			if (generalMessage != "" && generalMessage != null) {
				validationObj.message = generalMessage;
			}
			if (fieldName != null) {
				validationObj.errors[fieldName] = {};
				validationObj.errors[fieldName].isInvalid = true;
				validationObj.errors[fieldName].key = fieldName;
				validationObj.errors[fieldName].message = fieldMessage;
			}

			return validationObj;
		}

		///////////////////
		function generateValidationMessages(response, scopeObj, formObject, location) {
			//Fill in validationError boolean and message for each field according to the template
			// scopeDate.fieldNameError => boolean; scopeDate.fieldNameMessage => the error from the api; 
			scopeObj.validation = {};
			scopeObj.validation["errors"] = [];
			for (var i = 0; i < response.errors.length; i++) {
				var error = {};
				error.field = response.errors[i].key;
				error.message = response.errors[i].message;
				scopeObj.validation[response.errors[i].key] = {};
				scopeObj.validation[response.errors[i].key]["message"] = response.errors[i].message;
				scopeObj.validation[response.errors[i].key]["state"] = true;
			}
			//Rebind the form with the data returned from the server
			formObject = response.object;
			//Notify with a toast about the error and show the server response.message
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
			//Scroll top
			// set the location.hash to the id of
			// the element you wish to scroll to.
			location.hash('modal-top');

			// call $anchorScroll()
			$anchorScroll();
		}
		//////////////////
		function GoToState(stateName, params) {
			var redirectObject = {};
			redirectObject.stateName = stateName;
			redirectObject.params = params;

			$timeout(function () {
				$rootScope.$emit("state-change-needed", redirectObject);
			}, 0);
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
				"attachments": []
			};

			return area;
		}
		///////////////////////
		function regenerateAllAreaAttachments() {
			var response = {};
			response.success = true;
			response.message = "All area attachments regenerated";

			var entities = [];
			var areas = [];

			//#region << Get data >>
			function rasErrorCallback(data, status) {
				$log.warn("Area attachments were not regenerated due to:  " + response.message);
			}

			function rasGetEntityMetaListSuccessCallback(data, status) {
				entities = data.object;
				//Get all areas
				getRecordsWithoutList(null, null, null, "area", rasGetAreasListSuccessCallback, rasErrorCallback);
			}

			function rasGetAreasListSuccessCallback(data, status) {
				areas = data.object.data;
				executeRegeneration();
			}

			//Wait for the next cycle before triggering the regeneration
			//Get all entities meta
			$timeout(function () {
				getEntityMetaList(rasGetEntityMetaListSuccessCallback, rasErrorCallback);
			}, 0);

			//#endregion

			//#region << Process >>

			function executeRegeneration() {
				//Cycle entities and generate array of valid subscription for each
				var validAttachmentsArray = [];
				var entityValidatedDictionary = {};
				entities.forEach(function (entity) {
					var validAttachmentObj = {
						name: null,
						label: null,
						url: null,
						labelPlural: null,
						iconName: null,
						weight: null
					};
					validAttachmentObj.view = {
						name: null,
						label: null
					};
					validAttachmentObj.list = {
						name: null,
						label: null
					};
					//Entity
					validAttachmentObj.name = entity.name;
					validAttachmentObj.label = entity.label;
					validAttachmentObj.labelPlural = entity.labelPlural;
					validAttachmentObj.iconName = entity.iconName;
					validAttachmentObj.weight = entity.weight;

					//Views

					entity.recordViews.sort(function (a, b) {
						if (a.weight < b.weight) return -1;
						if (a.weight > b.weight) return 1;
						return 0;
					});
					for (var k = 0; k < entity.recordViews.length; k++) {
						var view = entity.recordViews[k];
						if (view.default && view.type == "general") {
							validAttachmentObj.view.name = view.name;
							validAttachmentObj.view.label = view.label;
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
							validAttachmentObj.list.name = list.name;
							validAttachmentObj.list.label = list.label;
							break;
						}
					}

					if (validAttachmentObj.view.name && validAttachmentObj.list.name) {
						validAttachmentsArray.push(validAttachmentObj);
						entityValidatedDictionary[entity.name] = validAttachmentObj;
					}
				});

				function rasAreaUpdateSuccessCallback(response) { }

				function rasAreaUpdateErrorCallback(response) {
					$log.warn("Area attachments were not regenerated due to:  " + response.message);
				}

				//Cycle through areas and substitute each entity attachment with its new valid attachment
				function getAttachmentChangeObject(attachment) {
					var updatedAttachmentObject = attachment;		 // null - no change, 404 - entity not found, {} - the new object that needs to be uploaded
					var selectedEntity = null;
					var attachmentUpdateIsNeeded = false;
					for (var i = 0; i < entities.length; i++) {
						if (entities[i].name == attachment.name) {
							selectedEntity = entities[i];
						}
					}

					if (selectedEntity == null) {
						return 404;
					}

					//Check general attributes
					if (selectedEntity.label != attachment.label) {
						attachmentUpdateIsNeeded = true;
						updatedAttachmentObject.label = selectedEntity.label;
					}

					if (selectedEntity.labelPlural != attachment.labelPlural) {
						attachmentUpdateIsNeeded = true;
						updatedAttachmentObject.labelPlural = selectedEntity.labelPlural;
					}

					if (selectedEntity.iconName != attachment.iconName) {
						attachmentUpdateIsNeeded = true;
						updatedAttachmentObject.iconName = selectedEntity.iconName;
					}

					if (selectedEntity.weight != attachment.weight) {
						attachmentUpdateIsNeeded = true;
						updatedAttachmentObject.weight = selectedEntity.weight;
					}

					//Check selected view
					var selectedViewIndex = -1;
					for (var n = 0; n < selectedEntity.recordViews.length; n++) {
						if (selectedEntity.recordViews[n].name == attachment.view.name) {
							selectedViewIndex = n;
							if (selectedEntity.recordViews[n].label != attachment.view.label) {
								attachmentUpdateIsNeeded = true;
								updatedAttachmentObject.view.label = selectedEntity.recordViews[n].label;
							}
							break;
						}
					}

					if (selectedViewIndex == -1) {
						//Selected view exists no more
						if (entityValidatedDictionary[selectedEntity.name] != null) {
							attachmentUpdateIsNeeded = true;
							var eligibleView = entityValidatedDictionary[selectedEntity.name].view;

							if (eligibleView != null) {
								updatedAttachmentObject.view.name = eligibleView.name;
								updatedAttachmentObject.view.label = eligibleView.label;
							}
							else {
								//Entity needs to have default view to be in an area
								return 404;
							}
						}
						else {
							//Entity is not found in the dictionary
							return 404;
						}

					}

					//Check selected list
					var selectedListIndex = -1;
					for (var n = 0; n < selectedEntity.recordLists.length; n++) {
						if (selectedEntity.recordLists[n].name == attachment.list.name) {
							selectedListIndex = n;
							if (selectedEntity.recordLists[n].label != attachment.list.label) {
								attachmentUpdateIsNeeded = true;
								updatedAttachmentObject.list.label = selectedEntity.recordLists[n].label;
							}
							break;
						}
					}


					if (selectedListIndex == -1) {
						//Selected list exists no more
						if (entityValidatedDictionary[selectedEntity.name] != null) {
							attachmentUpdateIsNeeded = true;
							var eligibleList = entityValidatedDictionary[selectedEntity.name].list;

							if (eligibleList != null) {
								updatedAttachmentObject.list.name = eligibleList.name;
								updatedAttachmentObject.list.label = eligibleList.label;
							}
							else {
								//Entity needs to have default list to be in an area
								return 404;
							}
						}
						else {
							//Entity is not found in the dictionary
							return 404;
						}
					}

					if (attachmentUpdateIsNeeded) {
						return updatedAttachmentObject;
					}
					else {
						return null;
					}
				}

				areas.forEach(function (area) {
					var attachments = angular.fromJson(area.attachments);
					var newAttachments = [];
					for (var n = 0; n < attachments.length; n++) {

						if (attachments[n].url != null && attachments[n].name == null) {
							//If it is a url just add it
							newAttachments.push(attachments[n]);
						}
						else if (attachments[n].name != null) {
							//If this is an entity check if this is the most recent version
							var newAttachmentObject = getAttachmentChangeObject(attachments[n]);
							if (newAttachmentObject == null) {
								//This attachment is not changed and is existing
								newAttachments.push(attachments[n]);
							}
							else if (newAttachmentObject == 404) {
								//the entity does not exist any more and should be removed
							}
							else {
								//The attachment is change and the new one should be used
								newAttachments.push(newAttachmentObject);
							}

						}
					}
					if (area.attachments != angular.toJson(newAttachments)) {
						//Need to sort the newAttachments first
						newAttachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
						area.attachments = angular.toJson(newAttachments);
						updateRecord(area.id, "area", area, rasAreaUpdateSuccessCallback, rasAreaUpdateErrorCallback);
					}
				});

			}

			//#endregion
		}
		///////////////////////
		function getCurrentAreaFromAreaList(areaName, areaList) {
			var currentArea = {};

			for (var i = 0; i < areaList.length; i++) {
				if (areaList[i].name == areaName) {
					currentArea = areaList[i];
				}
			}

			//Serialize the JSON attachments object
			currentArea.attachments = angular.fromJson(currentArea.attachments);

			currentArea.attachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			return currentArea;
		}

		////////////////////////
		function getDefaultViewNameForAreaEntity(currentArea, currentEntity) {
			var targetViewName = null;
			if (!currentArea || !currentEntity) {
				return null;
			}
			currentArea.attachments = angular.fromJson(currentArea.attachments);
			for (var i = 0; i < currentArea.attachments.length; i++) {
				if (currentArea.attachments[i].name == currentEntity.name) {
					targetViewName = currentArea.attachments[i].view.name;
					break;
				}
			}
			if (targetViewName != null) {
				return targetViewName;
			}
			else {
				currentEntity.recordViews.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
				for (var i = 0; i < currentEntity.recordViews.length; i++) {
					if (currentEntity.recordViews[i].default && currentEntity.recordViews[i].type == "general") {
						targetViewName = currentEntity.recordViews[i].name;
						break;
					}
				}
				if (targetViewName != null) {
					return targetViewName;
				}
				else {
					for (var i = 0; i < currentEntity.recordViews.length; i++) {
						if (currentEntity.recordViews[i].type == "general") {
							targetViewName = currentEntity.recordViews[i].name;
							break;
						}
					}
					return targetViewName;
				}
			}
		}
		//#endregion

		//#region << User >>

		///////////////////////
		function initUser() {
			var user = {
				"id": null,
				"email": null,
				"username": null,
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
		////////////////////
		function login(postObject, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'user/login', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		////////////////////
		function logout(successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'user/logout', data: {} }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////
		function getCurrentUser() {
			var user = null;
			var cookieValue = $cookies.get("erp-auth");
			if (cookieValue) {
				var cookieValueDecoded = decodeURIComponent(cookieValue);
				user = angular.fromJson(cookieValueDecoded);
			}
			return user;
		}
		////////////////////
		function getCurrentUserPermissions(successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'user/permissions' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////////////////////////////////////////////////////
		function applyAreaAccessPolicy(areaName, currentUser, sitemap) {
			if (currentUser == null) {
				return false;
			}

			var currentAreaObject = null;
			for (var i = 0; i < sitemap.data.length; i++) {
				if (sitemap.data[i].name == areaName) {
					currentAreaObject = sitemap.data[i];
				}
			}
			if (currentAreaObject == null) {
				return false;
			}

			var areaRoles = angular.fromJson(currentAreaObject.roles);
			var userHasAreaRole = false;
			for (var j = 0; j < areaRoles.length; j++) {
				for (var k = 0; k < currentUser.roles.length; k++) {
					if (currentUser.roles[k] == areaRoles[j]) {
						userHasAreaRole = true;
						break;
					}

				}
			}
			if (userHasAreaRole) {
				return true;
			}
			else {
				return false;
			}
		}
		////////////////////////
		function userHasRecordPermissions(entityMeta, permissionsCsv) {
			var requestedPermissionsArray = permissionsCsv.split(',');
			var permissionChecks = {};
			var createRolesArray = entityMeta.recordPermissions.canCreate;
			if (getCurrentUser() == null) {
				return false;
			}

			var userRoles = getCurrentUser().roles;

			for (var i = 0; i < requestedPermissionsArray.length; i++) {
				var permissionName = requestedPermissionsArray[i];
				permissionChecks[permissionName] = false;
				var entityAllowedRoles = entityMeta.recordPermissions[permissionName];
				for (var j = 0; j < entityAllowedRoles.length; j++) {
					var roleId = entityAllowedRoles[j];
					if (userRoles.indexOf(roleId) > -1) {
						permissionChecks[permissionName] = true;
						break;
					}
				}
			}

			var userHasAllPermissions = true;
			for (var permission in permissionChecks) {
				if (!permissionChecks[permission]) {
					userHasAllPermissions = false;
					break;
				}
			}

			return userHasAllPermissions;
		}
		///////////////////////
		function userIsInRole(roleId) {
			var userRoles = getCurrentUser().roles;
			for (var i = 0; i < userRoles.length; i++) {
				if (userRoles[i] == roleId) {
					return true;
				}
			}
			return false;
		}
		//#endregion

		//#region << Default action services >>
		function listAction_getRecordCreateUrl(ngCtrl) {

			//#region << Init >>

			var siteAreas = ngCtrl.areas;
			var entityList = ngCtrl.entityList;
			var currentEntity = ngCtrl.entity;
			var currentParent = ngCtrl.parentView;
			var currentAreaName = ngCtrl.stateParams.areaName;
			var currentEntityName = ngCtrl.stateParams.entityName;
			var currentListDataName = ngCtrl.stateParams.listName;
			var currentListName = ngCtrl.stateParams.listName;
			var currentRelationName = null; // when the list is listFromRelation
			var currentRelationDirection = "null"; // when the list is listFromRelation
			var targetEntityName = null;
			var targetEntity = null;
			var currentArea = null;
			var targetCreateName = null;
			var targetCreateExists = false;
			//#endregion	


			//#region << Check if listFromRelation, get the relationName if so >>
			//Example listFromRelation name: "$list$project_1_n_ticket$general"
			if (currentListName.indexOf('$') > -1 && currentListName.startsWith("$list")) {
				var dataNameArray = currentListName.split('$');
				if (dataNameArray.length == 4) {
					//this is a proper listFromRelation format
					currentRelationName = dataNameArray[2];
					currentListName = dataNameArray[3];
					var relationExists = false;
					for (var j = 0; j < ngCtrl.entityRelations.length; j++) {
						if (ngCtrl.entityRelations[j].name == currentRelationName) {
							relationExists = true;
							var currentRelation = ngCtrl.entityRelations[j];
							if (currentRelation.originEntityName == currentEntityName) {
								targetEntityName = currentRelation.targetEntityName;
							}
							else if (currentRelation.targetEntityName == currentEntityName) {
								targetEntityName = currentRelation.originEntityName;
							}
						}
					}
					//if the relation name is not found in the existing one, null it.
					if (!relationExists || targetEntityName == null) {
						console.log("the relation name in the listFromRelation is not found or the current entity is not participating in this relation");
						currentRelationName = null;
					}
					else {
						//Get the target entity details
						for (var i = 0; i < entityList.length; i++) {
							if (entityList[i].name == targetEntityName) {
								targetEntity = entityList[i];
							}
						}
					}
				}
			}

			if (currentRelationName != null && targetEntity == null) {
				console.log("cannot find the target entity");
				return null;
			}

			//#endregion


			//#region << Get relation direction if relation exists >>
			if (currentRelationName != null && currentParent.meta != null) {
				for (var i = 0; i < currentParent.meta.sidebar.items.length; i++) {
					if (currentParent.meta.sidebar.items[i].dataName == currentListDataName) {
						currentRelationDirection = currentParent.meta.sidebar.items[i].relationDirection;
						break;
					}
				}
			}

			//#endregion

			//#region << Get the selected create view in the area >>
			for (var i = 0; i < siteAreas.length; i++) {
				if (siteAreas[i].name == currentAreaName) {
					currentArea = siteAreas[i];
					break;
				}
			}
			if (currentArea == null) {
				//The list is presented in an URL attachment - the target create name is the first default create for the entity
				//To select that we just leave the process below to do that
			}
			else {
				currentArea.attachments = angular.fromJson(currentArea.attachments);
				for (var i = 0; i < currentArea.attachments.length; i++) {
					//If it is not listFromRelation 
					if (currentRelationName == null) {
						if (currentArea.attachments[i].name == currentEntityName) {
							targetCreateName = currentArea.attachments[i].create.name;
							break;
						}
					}
					else {
						if (currentArea.attachments[i].name == targetEntityName) {
							targetCreateName = currentArea.attachments[i].create.name;
							break;
						}
					}
				}
				if (targetCreateName == null) {
					//The list is presented in an URL attachment - the target create name is the first default create for the entity
					//To select that we just leave the process below to do that
				}
			}
			//#endregion

			//#region << Check if it target view exists >>
			if (targetCreateName != null) {
				if (currentRelationName == null) {
					for (var i = 0; i < currentEntity.recordViews.length; i++) {
						if (currentEntity.recordViews[i].name === targetCreateName) {
							targetCreateExists = true;
							break;
						}
					}
				}
				else {
					for (var i = 0; i < targetEntity.recordViews.length; i++) {
						if (targetEntity.recordViews[i].name === targetCreateName) {
							targetCreateExists = true;
							break;
						}
					}
				}
			}
			//#endregion

			//#region << Calculate what the view name should be and return >>
			if (targetCreateExists) {
				//Case 1: - list is not from relation
				if (currentRelationName == null) {
					return "#/areas/" + currentAreaName + "/" + currentEntityName + "/view-create/" + targetCreateName + "?returnUrl=" + encodeURI($location.path());
				}
					//Case 2: - this is a list with relation
				else {
					return "#/areas/" + currentAreaName + "/" + targetEntityName + "/view-create/" + targetCreateName + "/relation/" + currentRelationName + "/" +
						currentRelationDirection + "/" + ngCtrl.stateParams.recordId + "?returnUrl=" + encodeURI($location.path());
				}

			}
				//The target name does not exist. Fallback to default
			else {
				targetCreateName = null;
				//If not sort and get the first default and general
				if (currentRelationName == null) {
					currentEntity.recordViews.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
					for (var i = 0; i < currentEntity.recordViews.length; i++) {
						if (currentEntity.recordViews[i].default && currentEntity.recordViews[i].type == "create") {
							targetCreateName = currentEntity.recordViews[i].name;
							break;
						}
					}
				}
				else {
					targetEntity.recordViews.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
					for (var i = 0; i < targetEntity.recordViews.length; i++) {
						if (targetEntity.recordViews[i].default && targetEntity.recordViews[i].type == "create") {
							targetCreateName = targetEntity.recordViews[i].name;
							break;
						}
					}
				}
				if (targetCreateName != null) {
					//there is a default and general view fallback option available
					//Case 1: - list is not from relation
					if (currentRelationName == null) {
						return "#/areas/" + currentAreaName + "/" + currentEntityName + "/view-create/" + targetCreateName + "?returnUrl=" + encodeURI($location.path());
					}
						//Case 2: - this is a list with relation
					else {
						return "#/areas/" + currentAreaName + "/" + targetEntityName + "/view-create/" + targetCreateName + "/relation/" + currentRelationName + "/" +
						currentRelationDirection + "/" + ngCtrl.stateParams.recordId + "?returnUrl=" + encodeURI($location.path());
					}

				}
				else {
					//If there is default general take the first general
					if (currentRelationName == null) {
						for (var i = 0; i < currentEntity.recordViews.length; i++) {
							if (currentEntity.recordViews[i].type == "create") {
								targetCreateName = currentEntity.recordViews[i].name;
								break;
							}
						}
					}
					else {
						for (var i = 0; i < targetEntity.recordViews.length; i++) {
							if (targetEntity.recordViews[i].type == "create") {
								targetCreateName = targetEntity.recordViews[i].name;
								break;
							}
						}
					}
					if (targetCreateName != null) {
						//Case 1: - list is not from relation
						if (currentRelationName == null) {
							return "#/areas/" + currentAreaName + "/" + currentEntityName + "/view-create/" + targetCreateName + "?returnUrl=" + encodeURI($location.path());
						}
							//Case 2: - this is a list with relation
						else {
							return "#/areas/" + currentAreaName + "/" + targetEntityName + "/view-create/" + targetCreateName + "/relation/" + currentRelationName + "/" +
							currentRelationDirection + "/" + ngCtrl.stateParams.recordId + "?returnUrl=" + encodeURI($location.path());
						}
					}
					else {
						console.log("Error: Cannot find suitable create view for this entity records");
						return null;
					}
				}
			}
			//#endregion

		}

		function listAction_getRecordDetailsUrl(record, ngCtrl) {

			//#region << Init >>
			var currentRecord = record;
			var siteAreas = ngCtrl.areas;
			var entityList = ngCtrl.entityList;
			var currentEntity = ngCtrl.entity;
			var currentAreaName = ngCtrl.stateParams.areaName;
			var currentEntityName = ngCtrl.stateParams.entityName;
			var currentListName = ngCtrl.stateParams.listName;
			var currentRelationName = null; // when the list is listFromRelation
			var targetEntityName = null;
			var targetEntity = null
			var currentArea = null;
			var targetViewName = null;
			var targetViewExists = false;
			//#endregion

			//#region << Check if viewFromRelation, get the relationName if so >>
			//Example listFromRelation name: "$list$project_1_n_ticket$general"
			if (currentListName.indexOf('$') > -1 && currentListName.startsWith("$list")) {
				var dataNameArray = currentListName.split('$');
				if (dataNameArray.length == 4) {
					//this is a proper listFromRelation format
					currentRelationName = dataNameArray[2];
					currentListName = dataNameArray[3];
					var relationExists = false;
					for (var j = 0; j < ngCtrl.entityRelations.length; j++) {
						if (ngCtrl.entityRelations[j].name == currentRelationName) {
							relationExists = true;
							var currentRelation = ngCtrl.entityRelations[j];
							if (currentRelation.originEntityName == currentEntityName) {
								targetEntityName = currentRelation.targetEntityName;
							}
							else if (currentRelation.targetEntityName == currentEntityName) {
								targetEntityName = currentRelation.originEntityName;
							}
						}
					}
					//if the relation name is not found in the existing one, null it.
					if (!relationExists || targetEntityName == null) {
						console.log("the relation name in the listFromRelation is not found or the current entity is not participating in this relation");
						currentRelationName = null;
					}
					else {
						//Get the target entity details
						for (var i = 0; i < entityList.length; i++) {
							if (entityList[i].name == targetEntityName) {
								targetEntity = entityList[i];
							}
						}
					}
				}
			}

			if (currentRelationName != null && targetEntity == null) {
				console.log("cannot find the target entity");
				return null;
			}

			//#endregion

			//#region << Get the selected view in the area >>
			for (var i = 0; i < siteAreas.length; i++) {
				if (siteAreas[i].name == currentAreaName) {
					currentArea = siteAreas[i];
					break;
				}
			}
			if (currentArea == null) {
				//The list is presented in an URL attachment - the target create name is the first default create for the entity
				//To select that we just leave the process below to do that
			}
			currentArea.attachments = angular.fromJson(currentArea.attachments);
			for (var i = 0; i < currentArea.attachments.length; i++) {
				//If it is not listFromRelation 
				if (currentRelationName == null) {
					if (currentArea.attachments[i].name == currentEntityName) {
						targetViewName = currentArea.attachments[i].view.name;
						break;
					}
				}
				else {
					if (currentArea.attachments[i].name == targetEntityName) {
						targetViewName = currentArea.attachments[i].view.name;
						break;
					}
				}
			}
			if (targetViewName == null) {
				//The list is presented in an URL attachment - the target create name is the first default create for the entity
				//To select that we just leave the process below to do that
			}
			//#endregion

			//#region << Check if it target view exists >>
			if (targetViewName != null) {
				if (currentRelationName == null) {
					for (var i = 0; i < currentEntity.recordViews.length; i++) {
						if (currentEntity.recordViews[i].name === targetViewName) {
							targetViewExists = true;
							break;
						}
					}
				}
				else {
					for (var i = 0; i < targetEntity.recordViews.length; i++) {
						if (targetEntity.recordViews[i].name === targetViewName) {
							targetViewExists = true;
							break;
						}
					}
				}
			}
			//#endregion

			//#region << Calculate what the view name should be and return >>
			if (targetViewExists) {
				//Case 1: - list is not from relation
				if (currentRelationName == null) {
					return "#/areas/" + currentAreaName + "/" + currentEntityName + "/view-general/sb/" + targetViewName + "/" + currentRecord.id;
				}
					//Case 2: - this is a list with relation
				else {
					return "#/areas/" + currentAreaName + "/" + targetEntityName + "/view-general/sb/" + targetViewName + "/" + currentRecord.id + "?returnUrl=" + encodeURI($location.path());
				}
			}
				//The target name does not exist. Fallback to default
			else {
				targetViewName = null;
				//If not sort and get the first default and general
				if (currentRelationName == null) {
					currentEntity.recordViews.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
					for (var i = 0; i < currentEntity.recordViews.length; i++) {
						if (currentEntity.recordViews[i].default && currentEntity.recordViews[i].type == "general") {
							targetViewName = currentEntity.recordViews[i].name;
							break;
						}
					}
				}
				else {
					targetEntity.recordViews.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
					for (var i = 0; i < targetEntity.recordViews.length; i++) {
						if (targetEntity.recordViews[i].default && targetEntity.recordViews[i].type == "general") {
							targetViewName = targetEntity.recordViews[i].name;
							break;
						}
					}
				}
				if (targetViewName != null) {
					//there is a default and general view fallback option available
					//Case 1: - list is not from relation
					if (currentRelationName == null) {
						return "#/areas/" + currentAreaName + "/" + currentEntityName + "/view-general/sb/" + targetViewName + "/" + currentRecord.id;
					}
					else {
						return "#/areas/" + currentAreaName + "/" + targetEntityName + "/view-general/sb/" + targetViewName + "/" + currentRecord.id + "?returnUrl=" + encodeURI($location.path());
					}
				}
				else {
					//If there is default general take the first general
					if (currentRelationName == null) {
						for (var i = 0; i < currentEntity.recordViews.length; i++) {
							if (currentEntity.recordViews[i].default && currentEntity.recordViews[i].type == "hidden") {
								targetViewName = currentEntity.recordViews[i].name;
								break;
							}
						}
					}
					else {
						for (var i = 0; i < targetEntity.recordViews.length; i++) {
							if (targetEntity.recordViews[i].default && targetEntity.recordViews[i].type == "hidden") {
								targetViewName = targetEntity.recordViews[i].name;
								break;
							}
						}
					}
					if (targetViewName != null) {
						//Case 1: - list is not from relation
						if (currentRelationName == null) {
							return "#/areas/" + currentAreaName + "/" + currentEntityName + "/view-general/sb/" + targetViewName + "/" + currentRecord.id;
						}
							//Case 2: - this is a list with relation
						else {
							return "#/areas/" + currentAreaName + "/" + targetEntityName + "/view-general/sb/" + targetViewName + "/" + currentRecord.id + "?returnUrl=" + encodeURI($location.path());
						}
					}
					else {
						console.log("Error: Cannot find suitable details view for this entity records");
						return null;
					}
				}
			}
			//#endregion
		}

		///////////////////////
		function viewAction_fieldUpdate(item, data, recordId, ngCtrl) {
			var patchObject = {};
			var validation = {
				success: true,
				message: "successful validation"
			};
			if (data !== '[object Array]' && (typeof data === 'string' || data instanceof String) && data != null) {
				data = data.toString().trim();
			}
			switch (item.meta.fieldType) {

				//Auto increment number
				case 1:
					//Readonly
					break;
					//Checkbox
				case 2:
					if (typeof data === 'string' || data instanceof String) {
						data = data === "true"; // convert string to boolean
					}
					break;
					//Auto increment number
				case 3: //Currency
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					validation = checkDecimal(data);
					if (!validation.success) {
						return validation.message;
					}
					if (decimalPlaces(data) > item.meta.currency.decimalDigits) {
						return "Decimal places should be " + item.meta.currency.decimalDigits + " or less";
					}
					break;
				case 4: //Date
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					if (data != null) {
						//Tue Feb 02 2016 02:00:00 GMT+0200 (FLE Standard Time)
						var convertedData = data;
						convertedData = moment(data, "ddd MMM DD YYYY HH:mm:ss [GMT]ZZ").utc().toISOString();
						if (convertedData == "Invalid date") {
							data = moment(data).utc().toISOString();
						}
						else {
							data = convertedData;
						}
					}
					break;
				case 5: //Datetime
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					if (data != null) {
						var convertedData = data;
						convertedData = moment(data, "ddd MMM DD YYYY HH:mm:ss [GMT]ZZ").utc().toISOString();
						if (convertedData == "Invalid date") {
							data = moment(data).utc().toISOString();
						}
						else {
							data = convertedData;
						}
					}
					break;
				case 6: //Email
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					validation = checkEmail(data);
					if (!validation.success) {
						return validation.message;
					}
					break;
				case 11: // Multiselect
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					break;
					//Number
				case 12:
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					validation = checkDecimal(data);
					if (!validation.success) {
						return validation.message;
					}
					if (!data) {
						data = null;
					}
					break;
					//Percent
				case 14:
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					validation = checkPercent(data);
					if (!validation.success) {
						return validation.message;
					}
					if (!data) {
						data = null;
					}
					break;
				case 15: //Phone
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					validation = checkPhone(data);
					if (!validation.success) {
						return validation.message;
					}
					break;
				case 17: // Dropdown
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					break;
				default: // Dropdown
					if (!data && item.meta.required) {
						return "This is a required field";
					}
					break;
			}

			patchObject[item.meta.name] = data;

			function patchSuccessCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> ' + response.message
				});

				//we need to add a cache breaker for the browser to get the new version of files and images
				switch (item.meta.fieldType) {
					case 7: //file
						if (response.object.data[0][item.dataName] != null && response.object.data[0][item.dataName] != "") {
							response.object.data[0][item.dataName] += "?cb=" + moment().toISOString();
						}
						break;
					case 9: //image
						if (response.object.data[0][item.dataName] != null && response.object.data[0][item.dataName] != "") {
							response.object.data[0][item.dataName] += "?cb=" + moment().toISOString();
						}
						break;
				}
				return true;
			}

			function patchFailedCallback(response) {
				ngToast.create({
					className: 'error',
					content: generateErrorMessage(response),
					timeout: 7000
				});
				$state.go($state.current, {}, { reload: true });
				return "error";
			}

			if (!item.entityName) {
				return "item.entityName is missing, fixed this";
			}
			else {
				patchRecord(recordId, item.entityName, patchObject, patchSuccessCallback, patchFailedCallback);
			}
		}

		function viewAction_deleteRecord(ngCtrl) {
			function successCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> ' + response.message
				});

				//#region << Select default list >>
				ngCtrl.defaultEntityAreaListName = "";
				//get the current area meta
				for (var j = 0; j < ngCtrl.areas.length; j++) {
					if (ngCtrl.areas[j].name === ngCtrl.stateParams.areaName) {
						var areaAttachments = angular.fromJson(ngCtrl.areas[j].attachments);
						for (var k = 0; k < areaAttachments.length; k++) {
							if (areaAttachments[k].name === ngCtrl.stateParams.entityName) {
								ngCtrl.defaultEntityAreaListName = areaAttachments[k].list.name;
							}
						}
					}
				}
				//#endregion

				GoToState("webvella-area-list-general", { listName: ngCtrl.defaultEntityAreaListName, page: 1 });
			}

			function errorCallback(response) {
				popupCtrl.hasError = true;
				popupCtrl.errorMessage = response.message;
			}
			deleteRecord(ngCtrl.stateParams.recordId, ngCtrl.stateParams.entityName, successCallback, errorCallback);

		}

		//#endregion

		//#region << Helpers >>
		function generateErrorMessage(response) {
			var message = '<span class="go-red">Error:</span> ' + response.message;

			if (response.errors.length > 0) {
				message += '<ul>';
				_.forEach(response.errors, function (error) {
					message += '<li>';
					message += '<b>' + error.key + '</b> - ' + error.message;
					message += '</li>';
				});
				message += '</ul>';
			}

			return message;
		}


		function getFileContent(url, successCallback, errorCallback) {
			$http({ method: 'GET', url: url }).then(function getSuccessCallback(response) { successCallback(response); }, function getErrorCallback(response) { errorCallback(response); });
		}

		function getFieldTypes(successCallback) {
			$translate(['FIELD_TYPE_AUTONUMBER_LABEL', 'FIELD_TYPE_AUTONUMBER_DESCRIPTION',
						'FIELD_TYPE_CHECKBOX_LABEL', 'FIELD_TYPE_CHECKBOX_DESCRIPTION',
						'FIELD_TYPE_CURRENCY_LABEL', 'FIELD_TYPE_CURRENCY_DESCRIPTION',
						'FIELD_TYPE_DATE_LABEL', 'FIELD_TYPE_DATE_DESCRIPTION',
						'FIELD_TYPE_DATETIME_LABEL', 'FIELD_TYPE_DATETIME_DESCRIPTION',
						'FIELD_TYPE_EMAIL_LABEL', 'FIELD_TYPE_EMAIL_DESCRIPTION',
						'FIELD_TYPE_FILE_LABEL', 'FIELD_TYPE_FILE_DESCRIPTION',
						'FIELD_TYPE_HTML_LABEL', 'FIELD_TYPE_HTML_DESCRIPTION',
						'FIELD_TYPE_IMAGE_LABEL', 'FIELD_TYPE_IMAGE_DESCRIPTION',
						'FIELD_TYPE_TEXTAREA_LABEL', 'FIELD_TYPE_TEXTAREA_DESCRIPTION',
						'FIELD_TYPE_MULTISELECT_LABEL', 'FIELD_TYPE_MULTISELECT_DESCRIPTION',
						'FIELD_TYPE_NUMBER_LABEL', 'FIELD_TYPE_NUMBER_DESCRIPTION',
						'FIELD_TYPE_PASSWORD_LABEL', 'FIELD_TYPE_PASSWORD_DESCRIPTION',
						'FIELD_TYPE_PERCENT_LABEL', 'FIELD_TYPE_PERCENT_DESCRIPTION',
						'FIELD_TYPE_PHONE_LABEL', 'FIELD_TYPE_PHONE_DESCRIPTION',
						'FIELD_TYPE_GUID_LABEL', 'FIELD_TYPE_GUID_DESCRIPTION',
						'FIELD_TYPE_DROPDOWN_LABEL', 'FIELD_TYPE_DROPDOWN_DESCRIPTION',
						'FIELD_TYPE_TEXT_LABEL', 'FIELD_TYPE_TEXT_DESCRIPTION',
						'FIELD_TYPE_URL_LABEL', 'FIELD_TYPE_URL_DESCRIPTION',
						'FIELD_TYPE_TREESELECT_LABEL', 'FIELD_TYPE_TREESELECT_DESCRIPTION']).then(function (translations) {
							var types = [
								{
									"id": 1,
									"name": "AutoNumberField",
									"label": translations.FIELD_TYPE_AUTONUMBER_LABEL,
									"description": translations.FIELD_TYPE_AUTONUMBER_DESCRIPTION
								},
								{
									"id": 2,
									"name": "CheckboxField",
									"label": translations.FIELD_TYPE_CHECKBOX_LABEL,
									"description": translations.FIELD_TYPE_CHECKBOX_DESCRIPTION
								},
								{
									"id": 3,
									"name": "CurrencyField",
									"label": translations.FIELD_TYPE_CURRENCY_LABEL,
									"description": translations.FIELD_TYPE_CURRENCY_DESCRIPTION
								},
								{
									"id": 4,
									"name": "DateField",
									"label": translations.FIELD_TYPE_DATE_LABEL,
									"description": translations.FIELD_TYPE_DATE_DESCRIPTION
								},
								{
									"id": 5,
									"name": "DateTimeField",
									"label": translations.FIELD_TYPE_DATETIME_LABEL,
									"description": translations.FIELD_TYPE_DATETIME_DESCRIPTION
								},
								{
									"id": 6,
									"name": "EmailField",
									"label": translations.FIELD_TYPE_EMAIL_LABEL,
									"description": translations.FIELD_TYPE_EMAIL_DESCRIPTION
								},
								{
									"id": 7,
									"name": "FileField",
									"label": translations.FIELD_TYPE_FILE_LABEL,
									"description": translations.FIELD_TYPE_FILE_DESCRIPTION
								},
								{
									"id": 8,
									"name": "HtmlField",
									"label": translations.FIELD_TYPE_HTML_LABEL,
									"description": translations.FIELD_TYPE_HTML_DESCRIPTION
								},
								{
									"id": 9,
									"name": "ImageField",
									"label": translations.FIELD_TYPE_IMAGE_LABEL,
									"description": translations.FIELD_TYPE_IMAGE_DESCRIPTION
								},
								{
									"id": 10,
									"name": "MultiLineTextField",
									"label": translations.FIELD_TYPE_TEXTAREA_LABEL,
									"description": translations.FIELD_TYPE_TEXTAREA_DESCRIPTION
								},
								{
									"id": 11,
									"name": "MultiSelectField",
									"label": translations.FIELD_TYPE_MULTISELECT_LABEL,
									"description": translations.FIELD_TYPE_MULTISELECT_DESCRIPTION
								},
								{
									"id": 12,
									"name": "NumberField",
									"label": translations.FIELD_TYPE_NUMBER_LABEL,
									"description": translations.FIELD_TYPE_NUMBER_DESCRIPTION
								},
								{
									"id": 13,
									"name": "PasswordField",
									"label": translations.FIELD_TYPE_PASSWORD_LABEL,
									"description": translations.FIELD_TYPE_PASSWORD_DESCRIPTION
								},
								{
									"id": 14,
									"name": "PercentField",
									"label": translations.FIELD_TYPE_PERCENT_LABEL,
									"description": translations.FIELD_TYPE_PERCENT_DESCRIPTION
								},
								{
									"id": 15,
									"name": "PhoneField",
									"label": translations.FIELD_TYPE_PHONE_LABEL,
									"description": translations.FIELD_TYPE_PHONE_DESCRIPTION
								},
								{
									"id": 16,
									"name": "GuidField",
									"label": translations.FIELD_TYPE_GUID_LABEL,
									"description": translations.FIELD_TYPE_GUID_DESCRIPTION
								},
								{
									"id": 17,
									"name": "SelectField",
									"label": translations.FIELD_TYPE_DROPDOWN_LABEL,
									"description": translations.FIELD_TYPE_DROPDOWN_DESCRIPTION
								},
								{
									"id": 18,
									"name": "TextField",
									"label": translations.FIELD_TYPE_TEXT_LABEL,
									"description": translations.FIELD_TYPE_TEXT_DESCRIPTION
								},
								{
									"id": 19,
									"name": "UrlField",
									"label": translations.FIELD_TYPE_URL_LABEL,
									"description": translations.FIELD_TYPE_URL_DESCRIPTION
								},
								//20 is missing because it is relation field 
								{
									"id": 21,
									"name": "TreeSelect",
									"label": translations.FIELD_TYPE_TREESELECT_LABEL,
									"description": translations.FIELD_TYPE_TREESELECT_DESCRIPTION
								}
							];
							successCallback(types);
						});
		}

		function currencyMetas() {
			var meta = [
  {
	"symbol": "$",
	"name": "US Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "USD",
	"name_plural": "US dollars"
  },
  {
	"symbol": "CA$",
	"name": "Canadian Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "CAD",
	"name_plural": "Canadian dollars"
  },
  {
	"symbol": "€",
	"name": "Euro",
	"symbol_native": "€",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "EUR",
	"name_plural": "euros"
  },
  {
	"symbol": "AED",
	"name": "United Arab Emirates Dirham",
	"symbol_native": "د.إ.‏",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "AED",
	"name_plural": "UAE dirhams"
  },
  {
	"symbol": "Af",
	"name": "Afghan Afghani",
	"symbol_native": "؋",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "AFN",
	"name_plural": "Afghan Afghanis"
  },
  {
	"symbol": "ALL",
	"name": "Albanian Lek",
	"symbol_native": "Lek",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "ALL",
	"name_plural": "Albanian lekë"
  },
  {
	"symbol": "AMD",
	"name": "Armenian Dram",
	"symbol_native": "դր.",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "AMD",
	"name_plural": "Armenian drams"
  },
  {
	"symbol": "AR$",
	"name": "Argentine Peso",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "ARS",
	"name_plural": "Argentine pesos"
  },
  {
	"symbol": "AU$",
	"name": "Australian Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "AUD",
	"name_plural": "Australian dollars"
  },
  {
	"symbol": "man.",
	"name": "Azerbaijani Manat",
	"symbol_native": "ман.",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "AZN",
	"name_plural": "Azerbaijani manats"
  },
  {
	"symbol": "KM",
	"name": "Bosnia-Herzegovina Convertible Mark",
	"symbol_native": "KM",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BAM",
	"name_plural": "Bosnia-Herzegovina convertible marks"
  },
  {
	"symbol": "Tk",
	"name": "Bangladeshi Taka",
	"symbol_native": "৳",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BDT",
	"name_plural": "Bangladeshi takas"
  },
  {
	"symbol": "BGN",
	"name": "Bulgarian Lev",
	"symbol_native": "лв.",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BGN",
	"name_plural": "Bulgarian leva"
  },
  {
	"symbol": "BD",
	"name": "Bahraini Dinar",
	"symbol_native": "د.ب.‏",
	"decimal_digits": 3,
	"rounding": 0,
	"code": "BHD",
	"name_plural": "Bahraini dinars"
  },
  {
	"symbol": "FBu",
	"name": "Burundian Franc",
	"symbol_native": "FBu",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "BIF",
	"name_plural": "Burundian francs"
  },
  {
	"symbol": "BN$",
	"name": "Brunei Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BND",
	"name_plural": "Brunei dollars"
  },
  {
	"symbol": "Bs",
	"name": "Bolivian Boliviano",
	"symbol_native": "Bs",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BOB",
	"name_plural": "Bolivian bolivianos"
  },
  {
	"symbol": "R$",
	"name": "Brazilian Real",
	"symbol_native": "R$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BRL",
	"name_plural": "Brazilian reals"
  },
  {
	"symbol": "BWP",
	"name": "Botswanan Pula",
	"symbol_native": "P",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BWP",
	"name_plural": "Botswanan pulas"
  },
  {
	"symbol": "BYR",
	"name": "Belarusian Ruble",
	"symbol_native": "BYR",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "BYR",
	"name_plural": "Belarusian rubles"
  },
  {
	"symbol": "BZ$",
	"name": "Belize Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "BZD",
	"name_plural": "Belize dollars"
  },
  {
	"symbol": "CDF",
	"name": "Congolese Franc",
	"symbol_native": "FrCD",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "CDF",
	"name_plural": "Congolese francs"
  },
  {
	"symbol": "CHF",
	"name": "Swiss Franc",
	"symbol_native": "CHF",
	"decimal_digits": 2,
	"rounding": 0.05,
	"code": "CHF",
	"name_plural": "Swiss francs"
  },
  {
	"symbol": "CL$",
	"name": "Chilean Peso",
	"symbol_native": "$",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "CLP",
	"name_plural": "Chilean pesos"
  },
  {
	"symbol": "CN¥",
	"name": "Chinese Yuan",
	"symbol_native": "CN¥",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "CNY",
	"name_plural": "Chinese yuan"
  },
  {
	"symbol": "CO$",
	"name": "Colombian Peso",
	"symbol_native": "$",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "COP",
	"name_plural": "Colombian pesos"
  },
  {
	"symbol": "₡",
	"name": "Costa Rican Colón",
	"symbol_native": "₡",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "CRC",
	"name_plural": "Costa Rican colóns"
  },
  {
	"symbol": "CV$",
	"name": "Cape Verdean Escudo",
	"symbol_native": "CV$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "CVE",
	"name_plural": "Cape Verdean escudos"
  },
  {
	"symbol": "Kč",
	"name": "Czech Republic Koruna",
	"symbol_native": "Kč",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "CZK",
	"name_plural": "Czech Republic korunas"
  },
  {
	"symbol": "Fdj",
	"name": "Djiboutian Franc",
	"symbol_native": "Fdj",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "DJF",
	"name_plural": "Djiboutian francs"
  },
  {
	"symbol": "Dkr",
	"name": "Danish Krone",
	"symbol_native": "kr",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "DKK",
	"name_plural": "Danish kroner"
  },
  {
	"symbol": "RD$",
	"name": "Dominican Peso",
	"symbol_native": "RD$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "DOP",
	"name_plural": "Dominican pesos"
  },
  {
	"symbol": "DA",
	"name": "Algerian Dinar",
	"symbol_native": "د.ج.‏",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "DZD",
	"name_plural": "Algerian dinars"
  },
  {
	"symbol": "Ekr",
	"name": "Estonian Kroon",
	"symbol_native": "kr",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "EEK",
	"name_plural": "Estonian kroons"
  },
  {
	"symbol": "EGP",
	"name": "Egyptian Pound",
	"symbol_native": "ج.م.‏",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "EGP",
	"name_plural": "Egyptian pounds"
  },
  {
	"symbol": "Nfk",
	"name": "Eritrean Nakfa",
	"symbol_native": "Nfk",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "ERN",
	"name_plural": "Eritrean nakfas"
  },
  {
	"symbol": "Br",
	"name": "Ethiopian Birr",
	"symbol_native": "Br",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "ETB",
	"name_plural": "Ethiopian birrs"
  },
  {
	"symbol": "£",
	"name": "British Pound Sterling",
	"symbol_native": "£",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "GBP",
	"name_plural": "British pounds sterling"
  },
  {
	"symbol": "GEL",
	"name": "Georgian Lari",
	"symbol_native": "GEL",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "GEL",
	"name_plural": "Georgian laris"
  },
  {
	"symbol": "GH₵",
	"name": "Ghanaian Cedi",
	"symbol_native": "GH₵",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "GHS",
	"name_plural": "Ghanaian cedis"
  },
  {
	"symbol": "FG",
	"name": "Guinean Franc",
	"symbol_native": "FG",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "GNF",
	"name_plural": "Guinean francs"
  },
  {
	"symbol": "GTQ",
	"name": "Guatemalan Quetzal",
	"symbol_native": "Q",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "GTQ",
	"name_plural": "Guatemalan quetzals"
  },
  {
	"symbol": "HK$",
	"name": "Hong Kong Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "HKD",
	"name_plural": "Hong Kong dollars"
  },
  {
	"symbol": "HNL",
	"name": "Honduran Lempira",
	"symbol_native": "L",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "HNL",
	"name_plural": "Honduran lempiras"
  },
  {
	"symbol": "kn",
	"name": "Croatian Kuna",
	"symbol_native": "kn",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "HRK",
	"name_plural": "Croatian kunas"
  },
  {
	"symbol": "Ft",
	"name": "Hungarian Forint",
	"symbol_native": "Ft",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "HUF",
	"name_plural": "Hungarian forints"
  },
  {
	"symbol": "Rp",
	"name": "Indonesian Rupiah",
	"symbol_native": "Rp",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "IDR",
	"name_plural": "Indonesian rupiahs"
  },
  {
	"symbol": "₪",
	"name": "Israeli New Sheqel",
	"symbol_native": "₪",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "ILS",
	"name_plural": "Israeli new sheqels"
  },
  {
	"symbol": "Rs",
	"name": "Indian Rupee",
	"symbol_native": "টকা",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "INR",
	"name_plural": "Indian rupees"
  },
  {
	"symbol": "IQD",
	"name": "Iraqi Dinar",
	"symbol_native": "د.ع.‏",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "IQD",
	"name_plural": "Iraqi dinars"
  },
  {
	"symbol": "IRR",
	"name": "Iranian Rial",
	"symbol_native": "﷼",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "IRR",
	"name_plural": "Iranian rials"
  },
  {
	"symbol": "Ikr",
	"name": "Icelandic Króna",
	"symbol_native": "kr",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "ISK",
	"name_plural": "Icelandic krónur"
  },
  {
	"symbol": "J$",
	"name": "Jamaican Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "JMD",
	"name_plural": "Jamaican dollars"
  },
  {
	"symbol": "JD",
	"name": "Jordanian Dinar",
	"symbol_native": "د.أ.‏",
	"decimal_digits": 3,
	"rounding": 0,
	"code": "JOD",
	"name_plural": "Jordanian dinars"
  },
  {
	"symbol": "¥",
	"name": "Japanese Yen",
	"symbol_native": "￥",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "JPY",
	"name_plural": "Japanese yen"
  },
  {
	"symbol": "Ksh",
	"name": "Kenyan Shilling",
	"symbol_native": "Ksh",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "KES",
	"name_plural": "Kenyan shillings"
  },
  {
	"symbol": "KHR",
	"name": "Cambodian Riel",
	"symbol_native": "៛",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "KHR",
	"name_plural": "Cambodian riels"
  },
  {
	"symbol": "CF",
	"name": "Comorian Franc",
	"symbol_native": "FC",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "KMF",
	"name_plural": "Comorian francs"
  },
  {
	"symbol": "₩",
	"name": "South Korean Won",
	"symbol_native": "₩",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "KRW",
	"name_plural": "South Korean won"
  },
  {
	"symbol": "KD",
	"name": "Kuwaiti Dinar",
	"symbol_native": "د.ك.‏",
	"decimal_digits": 3,
	"rounding": 0,
	"code": "KWD",
	"name_plural": "Kuwaiti dinars"
  },
  {
	"symbol": "KZT",
	"name": "Kazakhstani Tenge",
	"symbol_native": "тңг.",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "KZT",
	"name_plural": "Kazakhstani tenges"
  },
  {
	"symbol": "LB£",
	"name": "Lebanese Pound",
	"symbol_native": "ل.ل.‏",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "LBP",
	"name_plural": "Lebanese pounds"
  },
  {
	"symbol": "SLRs",
	"name": "Sri Lankan Rupee",
	"symbol_native": "SL Re",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "LKR",
	"name_plural": "Sri Lankan rupees"
  },
  {
	"symbol": "Lt",
	"name": "Lithuanian Litas",
	"symbol_native": "Lt",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "LTL",
	"name_plural": "Lithuanian litai"
  },
  {
	"symbol": "Ls",
	"name": "Latvian Lats",
	"symbol_native": "Ls",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "LVL",
	"name_plural": "Latvian lati"
  },
  {
	"symbol": "LD",
	"name": "Libyan Dinar",
	"symbol_native": "د.ل.‏",
	"decimal_digits": 3,
	"rounding": 0,
	"code": "LYD",
	"name_plural": "Libyan dinars"
  },
  {
	"symbol": "MAD",
	"name": "Moroccan Dirham",
	"symbol_native": "د.م.‏",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "MAD",
	"name_plural": "Moroccan dirhams"
  },
  {
	"symbol": "MDL",
	"name": "Moldovan Leu",
	"symbol_native": "MDL",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "MDL",
	"name_plural": "Moldovan lei"
  },
  {
	"symbol": "MGA",
	"name": "Malagasy Ariary",
	"symbol_native": "MGA",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "MGA",
	"name_plural": "Malagasy Ariaries"
  },
  {
	"symbol": "MKD",
	"name": "Macedonian Denar",
	"symbol_native": "MKD",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "MKD",
	"name_plural": "Macedonian denari"
  },
  {
	"symbol": "MMK",
	"name": "Myanma Kyat",
	"symbol_native": "K",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "MMK",
	"name_plural": "Myanma kyats"
  },
  {
	"symbol": "MOP$",
	"name": "Macanese Pataca",
	"symbol_native": "MOP$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "MOP",
	"name_plural": "Macanese patacas"
  },
  {
	"symbol": "MURs",
	"name": "Mauritian Rupee",
	"symbol_native": "MURs",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "MUR",
	"name_plural": "Mauritian rupees"
  },
  {
	"symbol": "MX$",
	"name": "Mexican Peso",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "MXN",
	"name_plural": "Mexican pesos"
  },
  {
	"symbol": "RM",
	"name": "Malaysian Ringgit",
	"symbol_native": "RM",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "MYR",
	"name_plural": "Malaysian ringgits"
  },
  {
	"symbol": "MTn",
	"name": "Mozambican Metical",
	"symbol_native": "MTn",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "MZN",
	"name_plural": "Mozambican meticals"
  },
  {
	"symbol": "N$",
	"name": "Namibian Dollar",
	"symbol_native": "N$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "NAD",
	"name_plural": "Namibian dollars"
  },
  {
	"symbol": "₦",
	"name": "Nigerian Naira",
	"symbol_native": "₦",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "NGN",
	"name_plural": "Nigerian nairas"
  },
  {
	"symbol": "C$",
	"name": "Nicaraguan Córdoba",
	"symbol_native": "C$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "NIO",
	"name_plural": "Nicaraguan córdobas"
  },
  {
	"symbol": "Nkr",
	"name": "Norwegian Krone",
	"symbol_native": "kr",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "NOK",
	"name_plural": "Norwegian kroner"
  },
  {
	"symbol": "NPRs",
	"name": "Nepalese Rupee",
	"symbol_native": "नेरू",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "NPR",
	"name_plural": "Nepalese rupees"
  },
  {
	"symbol": "NZ$",
	"name": "New Zealand Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "NZD",
	"name_plural": "New Zealand dollars"
  },
  {
	"symbol": "OMR",
	"name": "Omani Rial",
	"symbol_native": "ر.ع.‏",
	"decimal_digits": 3,
	"rounding": 0,
	"code": "OMR",
	"name_plural": "Omani rials"
  },
  {
	"symbol": "B/.",
	"name": "Panamanian Balboa",
	"symbol_native": "B/.",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "PAB",
	"name_plural": "Panamanian balboas"
  },
  {
	"symbol": "S/.",
	"name": "Peruvian Nuevo Sol",
	"symbol_native": "S/.",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "PEN",
	"name_plural": "Peruvian nuevos soles"
  },
  {
	"symbol": "₱",
	"name": "Philippine Peso",
	"symbol_native": "₱",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "PHP",
	"name_plural": "Philippine pesos"
  },
  {
	"symbol": "PKRs",
	"name": "Pakistani Rupee",
	"symbol_native": "₨",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "PKR",
	"name_plural": "Pakistani rupees"
  },
  {
	"symbol": "zł",
	"name": "Polish Zloty",
	"symbol_native": "zł",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "PLN",
	"name_plural": "Polish zlotys"
  },
  {
	"symbol": "₲",
	"name": "Paraguayan Guarani",
	"symbol_native": "₲",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "PYG",
	"name_plural": "Paraguayan guaranis"
  },
  {
	"symbol": "QR",
	"name": "Qatari Rial",
	"symbol_native": "ر.ق.‏",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "QAR",
	"name_plural": "Qatari rials"
  },
  {
	"symbol": "RON",
	"name": "Romanian Leu",
	"symbol_native": "RON",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "RON",
	"name_plural": "Romanian lei"
  },
  {
	"symbol": "din.",
	"name": "Serbian Dinar",
	"symbol_native": "дин.",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "RSD",
	"name_plural": "Serbian dinars"
  },
  {
	"symbol": "RUB",
	"name": "Russian Ruble",
	"symbol_native": "руб.",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "RUB",
	"name_plural": "Russian rubles"
  },
  {
	"symbol": "RWF",
	"name": "Rwandan Franc",
	"symbol_native": "FR",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "RWF",
	"name_plural": "Rwandan francs"
  },
  {
	"symbol": "SR",
	"name": "Saudi Riyal",
	"symbol_native": "ر.س.‏",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "SAR",
	"name_plural": "Saudi riyals"
  },
  {
	"symbol": "SDG",
	"name": "Sudanese Pound",
	"symbol_native": "SDG",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "SDG",
	"name_plural": "Sudanese pounds"
  },
  {
	"symbol": "Skr",
	"name": "Swedish Krona",
	"symbol_native": "kr",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "SEK",
	"name_plural": "Swedish kronor"
  },
  {
	"symbol": "S$",
	"name": "Singapore Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "SGD",
	"name_plural": "Singapore dollars"
  },
  {
	"symbol": "Ssh",
	"name": "Somali Shilling",
	"symbol_native": "Ssh",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "SOS",
	"name_plural": "Somali shillings"
  },
  {
	"symbol": "SY£",
	"name": "Syrian Pound",
	"symbol_native": "ل.س.‏",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "SYP",
	"name_plural": "Syrian pounds"
  },
  {
	"symbol": "฿",
	"name": "Thai Baht",
	"symbol_native": "฿",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "THB",
	"name_plural": "Thai baht"
  },
  {
	"symbol": "DT",
	"name": "Tunisian Dinar",
	"symbol_native": "د.ت.‏",
	"decimal_digits": 3,
	"rounding": 0,
	"code": "TND",
	"name_plural": "Tunisian dinars"
  },
  {
	"symbol": "T$",
	"name": "Tongan Paʻanga",
	"symbol_native": "T$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "TOP",
	"name_plural": "Tongan paʻanga"
  },
  {
	"symbol": "TL",
	"name": "Turkish Lira",
	"symbol_native": "TL",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "TRY",
	"name_plural": "Turkish Lira"
  },
  {
	"symbol": "TT$",
	"name": "Trinidad and Tobago Dollar",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "TTD",
	"name_plural": "Trinidad and Tobago dollars"
  },
  {
	"symbol": "NT$",
	"name": "New Taiwan Dollar",
	"symbol_native": "NT$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "TWD",
	"name_plural": "New Taiwan dollars"
  },
  {
	"symbol": "TSh",
	"name": "Tanzanian Shilling",
	"symbol_native": "TSh",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "TZS",
	"name_plural": "Tanzanian shillings"
  },
  {
	"symbol": "₴",
	"name": "Ukrainian Hryvnia",
	"symbol_native": "₴",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "UAH",
	"name_plural": "Ukrainian hryvnias"
  },
  {
	"symbol": "USh",
	"name": "Ugandan Shilling",
	"symbol_native": "USh",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "UGX",
	"name_plural": "Ugandan shillings"
  },
  {
	"symbol": "$U",
	"name": "Uruguayan Peso",
	"symbol_native": "$",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "UYU",
	"name_plural": "Uruguayan pesos"
  },
  {
	"symbol": "UZS",
	"name": "Uzbekistan Som",
	"symbol_native": "UZS",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "UZS",
	"name_plural": "Uzbekistan som"
  },
  {
	"symbol": "Bs.F.",
	"name": "Venezuelan Bolívar",
	"symbol_native": "Bs.F.",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "VEF",
	"name_plural": "Venezuelan bolívars"
  },
  {
	"symbol": "₫",
	"name": "Vietnamese Dong",
	"symbol_native": "₫",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "VND",
	"name_plural": "Vietnamese dong"
  },
  {
	"symbol": "FCFA",
	"name": "CFA Franc BEAC",
	"symbol_native": "FCFA",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "XAF",
	"name_plural": "CFA francs BEAC"
  },
  {
	"symbol": "CFA",
	"name": "CFA Franc BCEAO",
	"symbol_native": "CFA",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "XOF",
	"name_plural": "CFA francs BCEAO"
  },
  {
	"symbol": "YR",
	"name": "Yemeni Rial",
	"symbol_native": "ر.ي.‏",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "YER",
	"name_plural": "Yemeni rials"
  },
  {
	"symbol": "R",
	"name": "South African Rand",
	"symbol_native": "R",
	"decimal_digits": 2,
	"rounding": 0,
	"code": "ZAR",
	"name_plural": "South African rand"
  },
  {
	"symbol": "ZK",
	"name": "Zambian Kwacha",
	"symbol_native": "ZK",
	"decimal_digits": 0,
	"rounding": 0,
	"code": "ZMK",
	"name_plural": "Zambian kwachas"
  }
			];
			return meta;
		}
		//#endregion

		//#region << Plugins >>
		///////////////////////
		function getPluginsList(successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'plugin/list/' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//#endregion


		//#region << Schedule plan >>
		///////////////////////
		function updateSchedulePlan(planId, submitObject, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'scheduleplan/' + planId, data: submitObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		function triggerSchedulePlan(planId, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'scheduleplan/' + planId + '/trigger' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		function getSchedulePlanList(successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'scheduleplan/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		function getSchedulePlan(planId, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'scheduleplan/' + planId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		function getJobsList(startFromDate, startToDate, finishedFromDate, finishedToDate,
			typeName, status, priority, schedulePlanId, page, pageSize, successCallback, errorCallback) {
			var queryParams = [];
			if (startFromDate != null) {
				queryParams.push("startFromDate=" + startFromDate);
			}
			if (startToDate != null) {
				queryParams.push("startToDate=" + startToDate);
			}
			if (finishedFromDate != null) {
				queryParams.push("finishedFromDate=" + finishedFromDate);
			}
			if (finishedToDate != null) {
				queryParams.push("finishedToDate=" + finishedToDate);
			}
			if (typeName != null) {
				queryParams.push("typeName=" + typeName);
			}
			if (status != null) {
				queryParams.push("status=" + status);
			}
			if (priority != null) {
				queryParams.push("priority=" + priority);
			}
			if (schedulePlanId != null) {
				queryParams.push("schedulePlanId=" + schedulePlanId);
			}
			if (page != null) {
				queryParams.push("page=" + page);
			}
			else {
				queryParams.push("page=1");
			}
			if (pageSize != null) {
				queryParams.push("pageSize=" + pageSize);
			}
			else {
				queryParams.push("pageSize=10");
			}

			var queryString = "";
			if (queryParams.length == 0) { }
			else if (queryParams.length == 1) {
				queryString = "?" + queryParams[0];
			}
			else {
				queryString = "?" + queryParams.join('&');
			}


			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'jobs' + queryString }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//#endregion

		//#region << System log >>
		function getSystemLog(fromDate, untilDate, type, source, message, notificationStatus, page, pageSize, successCallback, errorCallback) {
			var queryParams = [];
			if (fromDate != null) {
				queryParams.push("fromDate=" + fromDate);
			}
			if (untilDate != null) {
				queryParams.push("untilDate=" + untilDate);
			}
			if (type != null) {
				queryParams.push("type=" + type);
			}
			if (source != null) {
				queryParams.push("source=" + source);
			}
			if (message != null) {
				queryParams.push("message=" + message);
			}
			if (notificationStatus != null) {
				queryParams.push("notificationStatus=" + notificationStatus);
			}
			if (page != null) {
				queryParams.push("page=" + page);
			}
			else {
				queryParams.push("page=" + 1);
			}
			if (pageSize != null) {
				queryParams.push("pageSize=" + pageSize);
			}
			else {
				queryParams.push("pageSize=" + 15);
			}


			var queryString = "";
			if (queryParams.length == 0) { }
			else if (queryParams.length == 1) {
				queryString = "?" + queryParams[0];
			}
			else {
				queryString = "?" + queryParams.join('&');
			}
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'system-log' + queryString }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//#endregion


		//#region << User File >>
		function getUserFileList(type,search,sort,page,pageSize, successCallback, errorCallback) {
			var paramsObj = {};
			if(!isStringNullOrEmptyOrWhiteSpace(type)){
				paramsObj.type = type; 
			}
			if(!isStringNullOrEmptyOrWhiteSpace(search)){
				paramsObj.search = search; 
			}
			var checkPageIntResult = checkInt(page);
			if(page && checkPageIntResult.success){
				paramsObj.page = page; 
			}
			var checkPageSizeIntResult = checkInt(pageSize);
			if(pageSize && checkPageSizeIntResult.success){
				paramsObj.pageSize = pageSize; 
			}

			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'user_file',params:paramsObj}).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		function uploadUserFile(submitObj,successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'user_file', data:submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion


		//#endregion
	}
})();
