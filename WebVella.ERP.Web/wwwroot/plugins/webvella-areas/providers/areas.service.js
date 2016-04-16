/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .service('webvellaAreasService', service);

	service.$inject = ['$log', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter'];

	/* @ngInject */
	function service($log, $http, wvAppConstants, $timeout, ngToast, $filter) {
		var serviceInstance = this;

		serviceInstance.getAreaByName = getAreaByName;
		serviceInstance.getCurrentAreaFromSitemap = getCurrentAreaFromSitemap;
		serviceInstance.getCurrentEntityFromArea = getCurrentEntityFromArea;
		serviceInstance.getViewByName = getViewByName;
		serviceInstance.createEntityRecord = createEntityRecord;
		serviceInstance.getListRecords = getListRecords;
		serviceInstance.getRecordsByFieldAndRegex = getRecordsByFieldAndRegex;
		serviceInstance.getViewRecord = getViewRecord;
		//Import & Export
		serviceInstance.importEntityRecords = importEntityRecords;
		serviceInstance.exportListRecords = exportListRecords;

		serviceInstance.getViewRecord = getViewRecord;
		//// Record data presenting
		serviceInstance.renderFieldValue = renderFieldValue;

		//#region << API calls >>
		///////////////////////
		function getAreaByName(areaName, successCallback, errorCallback) {
			$log.debug('webvellaAreas>providers>areas.service>getAreaByName> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + '/meta/entity/' + areaName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getCurrentAreaFromSitemap(areaName, sitemap) {
			var currentArea = {};

			for (var i = 0; i < sitemap.length; i++) {
				if (sitemap[i].name == areaName) {
					currentArea = sitemap[i];
				}
			}

			//Serialize the JSON attachments object
			currentArea.attachments = angular.fromJson(currentArea.attachments);

			currentArea.attachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			return currentArea;
		}

		///////////////////////
		function getCurrentEntityFromArea(entityName, area) {
			var currentEntity = {};

			for (var i = 0; i < area.attachments.length; i++) {
				if (area.attachments[i].name == entityName) {
					currentEntity = area.attachments[i];
				}
			}

			return currentEntity;
		}


		///////////////////////
		function getViewByName(viewName, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAreas>providers>areas.service>getViewMetaByName> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function createEntityRecord(postObject, entityName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createEntityRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		/////////////////////
		function getListRecords(listName, entityName, page, search, successCallback, errorCallback) {
			//api/v1/en_US/record/{entityName}/list/{listName}
			$log.debug('webvellaAreas>providers>areas.service>getListRecords> function called ' + moment().format('HH:mm:ss SSSS'));
			if (!search || search == "") {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + page }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				search = encodeURIComponent(search);
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + page + '?search=' + search }).then(function getSuccessCallback(response){handleSuccessResult(response.data, response.status, successCallback, errorCallback);}, function getErrorCallback(response) {handleErrorResult(response.data, response.status, errorCallback);});
			}
		}

		///////////////////////
		function getRecordsByFieldAndRegex(entityName, fieldName, postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>getRecordsByFieldAndRegex> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/regex/' + fieldName, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}


		///////////////////////
		function getViewRecord(recordId, viewName, entityName, successCallback, errorCallback) {
			//"api/v1/en_US/record/{entityName}/view/{viewName}/{id}"
			$log.debug('webvellaAreas>providers>areas.service>getEntityRecord> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/view/' + viewName + '/' + recordId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#region << Field data presentation ///////////////////////////////////////////

		//General Compile method
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
					return "TRUE";
				}
				else {
					return "FALSE";
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
					var fileName = data.slice(lastSlashIndex,data.length);
				return "<a href='" + data + "' taget='_blank' class='link-icon'>"+fileName + "</a>";
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
				else if (data.length == 1){
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

		//#region << Import Export >>

		///////////////////////
		function importEntityRecords(entityName, fileTempPath, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>importEntityRecords> function called ' + moment().format('HH:mm:ss SSSS'));
			var postObject = {};
			postObject.fileTempPath = fileTempPath;
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/import', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function exportListRecords(entityName, listName, count, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>exportListRecords> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + "/export?count=" + count }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion	
		
		//// Aux methods //////////////////////////////////////////////////////

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
				$log.debug('webvellaAreas>providers>areas.service>getAreaByName> result failure: successCallback not a function or missing  ' + moment().format('HH:mm:ss SSSS'));
				alert("The successCallback argument is not a function or missing");
				return;
			}

			if (!data.success) {
				//when the validation errors occurred
				if (errorCallback === undefined || typeof (errorCallback) != "function") {
					$log.debug('webvellaAreas>providers>areas.service>getAreaByName> result failure: errorCallback not a function or missing  ' + moment().format('HH:mm:ss SSSS'));
					alert("The errorCallback argument in handleSuccessResult is not a function or missing");
					return;
				}
				errorCallback(data);
			}
			else {
				$log.debug('webvellaAreas>providers>areas.service>getAreaByName> result success: get object  ' + moment().format('HH:mm:ss SSSS'));
				successCallback(data);
			}
		}

	}
})();
