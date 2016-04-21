/* core.service.js */

/**
* @desc Javascript API core service
*/

(function () {
	'use strict';

	angular
        .module('webvellaCore')
        .service('webvellaCoreService', service);

	service.$inject = ['$cookies', '$http', '$log', 'wvAppConstants', '$rootScope', '$anchorScroll', 'ngToast', '$timeout', 'Upload', ];


	function service($cookies, $http, $log, wvAppConstants, $rootScope, $anchorScroll, ngToast, $timeout, Upload) {
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
		serviceInstance.initViewSection = initViewSection;
		serviceInstance.initViewRow = initViewRow;
		serviceInstance.initViewRowColumn = initViewRowColumn;
		serviceInstance.initViewActionItem = initViewActionItem;
		//Create
		serviceInstance.createEntityView = createEntityView;
		//Read
		serviceInstance.getEntityView = getEntityView;
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
		serviceInstance.getViewMenuOptions = getViewMenuOptions
		serviceInstance.getItemsFromRegion = getItemsFromRegion;

		//#endregion

		//#region << List >>

		//Init
		serviceInstance.initList = initList;
		serviceInstance.initListActionItem = initListActionItem;
		//Create
		serviceInstance.createEntityList = createEntityList;
		//Read
		serviceInstance.getEntityLists = getEntityLists;
		serviceInstance.getEntityList = getEntityList;
		//Update
		serviceInstance.patchEntityList = patchEntityList;
		serviceInstance.updateEntityList = updateEntityList;
		//Delete
		serviceInstance.deleteEntityList = deleteEntityList;
		//Helpers
		serviceInstance.getListMenuOptions = getListMenuOptions

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

		//#endregion

		//#region << Record >>

		//Create
		serviceInstance.createRecord = createRecord;
		serviceInstance.importEntityRecords = importEntityRecords;
		serviceInstance.exportListRecords = exportListRecords;
		//Read
		serviceInstance.getRecord = getRecord;
		serviceInstance.getRecordByViewName = getRecordByViewName;
		serviceInstance.getRecordsWithLimitations = getRecordsWithLimitations;
		serviceInstance.getRecordsByListName = getRecordsByListName;
		serviceInstance.getRecordsByTreeName = getRecordsByTreeName;
		serviceInstance.getRecordsByFieldAndRegex = getRecordsByFieldAndRegex;
		//Update
		serviceInstance.updateRecord = updateRecord;
		serviceInstance.patchRecord = patchRecord;
		serviceInstance.updateRecordRelation = updateRecordRelation;
		//Delete
		serviceInstance.deleteRecord = deleteRecord;
		//Helpers
		serviceInstance.uploadFileToTemp = uploadFileToTemp;
		serviceInstance.moveFileFromTempToFS = moveFileFromTempToFS;
		serviceInstance.deleteFileFromFS = deleteFileFromFS;

		//#endregion

		//#region << Site >>
		serviceInstance.registerHookListener = registerHookListener;
		serviceInstance.launchHook = launchHook;
		serviceInstance.setPageTitle = setPageTitle;
		serviceInstance.setBodyColorClass = setBodyColorClass;
		serviceInstance.generateValidationMessages = generateValidationMessages;
		serviceInstance.GoToState = GoToState;
		//#endregion

		//#region << Area >>
		serviceInstance.initArea = initArea;
		serviceInstance.regenerateAllAreaAttachments = regenerateAllAreaAttachments;
		serviceInstance.getCurrentAreaFromAreaList = getCurrentAreaFromAreaList;
		//#endregion

		//#region << User >>
		serviceInstance.initUser = initUser;
		serviceInstance.login = login;
		serviceInstance.logout = logout;
		serviceInstance.getCurrentUser = getCurrentUser;
		serviceInstance.getCurrentUserPermissions = getCurrentUserPermissions;
		serviceInstance.applyAreaAccessPolicy = applyAreaAccessPolicy;
		serviceInstance.userHasEntityPermissions = userHasEntityPermissions;
		//#endregion

		//#region << Default list actions >>
		serviceInstance.listAction_getRecordDetailsUrl = listAction_getRecordDetailsUrl;

		//#endregion

		//#region << Helpers >>
		serviceInstance.getFileContent = getFileContent;

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
					ngToast.create({
						className: 'error',
						content: messageString,
						timeout: 7000
					});
					errorCallback(data);
					break;
				default:
					alert("An API call finished with error: " + status);
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
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function patchEntity(entityId, patchObject, successCallback, errorCallback) {
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId, data: patchObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteEntity(entityId, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return getAutoIncrementString(data[0], fieldMeta);
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getAutoIncrementString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
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
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
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
			else if (fieldMeta.currency != null && fieldMeta.currency !== {} && fieldMeta.currency.symbol) {
				if (fieldMeta.currency.symbolPlacement === 1) {
					return fieldMeta.currency.symbol + " " + data;
				}
				else {
					return data + " " + fieldMeta.currency.symbol;
				}
			}
			else {
				return data;
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
				return moment(data).format("DD MMM YYYY");;
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
				return data;
			}

		}
		//7.File
		function getFileString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
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
				return "<a href='" + data + "' target='_blank' class='link-icon'>" + fileName + "</a>";
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
					return htmlString;
				}
			}
			else {
				return data;
			}
		}
		//9.Image
		function getImageString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
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
				return "<a target='_blank' href='" + data + "'><img src='" + data + "' class='table-image'/></a>";
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
				return data;
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
						generatedStringArray.push((data[i] && selected.length) ? selected[0].value : 'empty');
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
				return data * 100 + "%";
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
				return phoneUtils.formatInternational(data);
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
				return (data && selected.length) ? selected[0].value : 'empty';
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
				return data;
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
		function initView() {
			var view = {
				"id": null,
				"name": "",
				"label": "",
				"default": false,
				"system": false,
				"weight": 1,
				"cssClass": "",
				"dynamicHtmlTemplate": null,
				"relationOptions": [],
				"serviceCode": null,
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
				},
				"actionItems": []
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
		function getViewMenuOptions() {
			var menuOptions = [
				{
					key: "hidden",
					value: "hidden",
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
					key: "create-bottom",
					value: "create-bottom",
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
		//////////////////////
		function getEntityViewLibrary(entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/getEntityViewLibrary' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function getEntityView(viewName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
					"actionItems": [],
					"columnWidthsCSV": null,
					"relationOptions": [],
					"serviceCode": null
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
		///////////////////////
		function getEntityLists(entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getEntityList(listName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function createEntityList(submitObj, entityName, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list", data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function patchEntityList(submitObj, listName, entityName, successCallback, errorCallback) {
			$http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list/" + listName, data: submitObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////////
		function updateEntityList(listObj, entityName, successCallback, errorCallback) {
			$http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + "/list/" + listObj.name, data: listObj }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function deleteEntityList(listName, entityName, successCallback, errorCallback) {
			$http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/list/' + listName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/list' }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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

		//#endregion

		//#region << Records >>

		///////////////////////
		function createRecord(entityName, postObject, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function importEntityRecords(entityName, fileTempPath, successCallback, errorCallback) {
			var postObject = {};
			postObject.fileTempPath = fileTempPath;
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/import', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
 		///////////////////////
		function exportListRecords(entityName, listName, count, successCallback, errorCallback) {
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + "/export?count=" + count }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecord(recordId, fields, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId + "?fields=" + fields }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecordByViewName(recordId, viewName, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/view/' + viewName + '/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		/////////////////////
		function getRecordsWithLimitations(recordIds, fieldNames, entityName, successCallback, errorCallback) {
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list?ids=' + recordIds + "&fields=" + fieldNames }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////////
		function getRecordsByListName(listName, entityName, page, successCallback, errorCallback) {
			//submit listName = "null" to get unconfigured records list
			//submit page = "null" to get all records
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + page }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
		function uploadFileToTemp(file, fieldName, progressCallback, successCallback, errorCallback) {
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

		//#endregion

		//#region << Site >>

		///////////////////////
		function registerHookListener(eventHookName, currentScope, executeOnHookFunction) {
			if (executeOnHookFunction === undefined || typeof (executeOnHookFunction) != "function") {
				alert("The executeOnHookFunction argument is not a function or missing ");
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
		///////////////////
		function generateValidationMessages(response, scopeObj, formObject, location) {
			//Fill in validationError boolean and message for each field according to the template
			// scopeDate.fieldNameError => boolean; scopeDate.fieldNameMessage => the error from the api; 
			scopeObj.validation = {};
			for (var i = 0; i < response.errors.length; i++) {
				scopeObj.validation[response.errors[i].key] = {};
				scopeObj.validation[response.errors[i].key]["message"] = response.errors[i].message;
				scopeObj.validation[response.errors[i].key]["state"] = true;
			}
			//Rebind the form with the data returned from the server
			formObject = response.object;
			//Notify with a toast about the error and show the server response.message
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
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
				entities = data.object.entities;
				//Get all areas
				getRecordsByEntityName("area", "null", rasGetAreasListSuccessCallback, rasErrorCallback);
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
		function userHasEntityPermissions(entityMeta, permissionsCsv) {
			var requestedPermissionsArray = permissionsCsv.split(',');
			var permissionChecks = {};
			var createRolesArray = fastCopy(entityMeta.recordPermissions.canCreate);
			if(getCurrentUser() == null){
				return false;
			}

			var userRoles = fastCopy(getCurrentUser().roles);

			for (var i = 0; i < requestedPermissionsArray.length; i++) {
				var permissionName = requestedPermissionsArray[i];
				permissionChecks[permissionName] = false;
				var entityAllowedRoles = fastCopy(entityMeta.recordPermissions[permissionName]);
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

		//#endregion

		//#region << Default action services >>

		function listAction_getRecordDetailsUrl(record, ngCtrl) {
			
			//#region << Init >>
			var currentRecord = fastCopy(record);
			var siteAreas = fastCopy(ngCtrl.areas);
			var currentEntity = fastCopy(ngCtrl.entity);
			var currentAreaName = fastCopy(ngCtrl.stateParams.areaName);
			var currentEntityName = fastCopy(ngCtrl.stateParams.entityName);
			var currentArea = null;
			var targetViewName = null;
			var targetViewExists = false;
			//#endregion

			//#region << Get the selected view in the area >>
			for (var i = 0; i < siteAreas.length; i++) {
			   if(siteAreas[i].name == currentAreaName){
				  currentArea = siteAreas[i];
				  break;
			   }
			}
			if(currentArea == null){
				alert("Error: No area with such name found");
			}
			currentArea.attachments = angular.fromJson(currentArea.attachments);
			for (var i = 0; i < currentArea.attachments.length; i++) {
				if(currentArea.attachments[i].name == currentEntityName){
					targetViewName =  currentArea.attachments[i].view.name;
					break;
				}
			}
			if(targetViewName == null){
				alert("Error: The current entity is either not attached to the area or the view name is missing");
			}
			//#endregion

			//#region << Check if it target view exists >>
			for (var i = 0; i < currentEntity.recordViews.length; i++) {
			   if(currentEntity.recordViews[i].name === targetViewName){
			   	   targetViewExists = true;
				   break;
			   }
			}

			//#endregion
			
			//#region << Calculate what the view name should be and return >>
			if(targetViewExists){
				return "#/areas/" + currentAreaName + "/" + currentEntityName + "/" + currentRecord.id + "/" + targetViewName + "/*/*";
			}
			//The target name does not exist. Fallback to default
			else {
				targetViewName = null;				
				//If not sort and get the first default and general
				currentEntity.recordViews.sort(sort_by({name:'weight', primer: parseInt, reverse:false}));
				for (var i = 0; i < currentEntity.recordViews.length; i++) {
				   if(currentEntity.recordViews[i].default && currentEntity.recordViews[i].type == "general"){
				   		targetViewName = currentEntity.recordViews[i].name;
						break;
				   }
				}
				if(targetViewName != null){
					//there is a default and general view fallback option available
					return "#/areas/" + currentAreaName + "/" + currentEntityName + "/" + currentRecord.id + "/" + targetViewName + "/*/*";
				}
				else {
					//If there is default general take the first general
					for (var i = 0; i < currentEntity.recordViews.length; i++) {
					   if(currentEntity.recordViews[i].type == "general"){
				   			targetViewName = currentEntity.recordViews[i].name;
							break;
					   }
					}
					if(targetViewName != null){
						 return "#/areas/" + currentAreaName + "/" + currentEntityName + "/" + currentRecord.id + "/" + targetViewName + "/*/*";
					}
					else {
						alert("Error: Cannot find suitable details view for this entity records");
					}
			   }
			}
			//#endregion
		}

		//#endregion

		//#region << Helpers >>
		function getFileContent(url,successCallback, errorCallback){
			$http({ method: 'GET', url: url }).then(function getSuccessCallback(response) { successCallback(response); }, function getErrorCallback(response) { errorCallback(response); });
		}

		//#endregion

		//#endregion
	}
})();
