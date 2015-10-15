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
		serviceInstance.createListFilter = createListFilter;
		serviceInstance.getListFilter = getListFilter;
		serviceInstance.deleteSelectedFilterRecords = deleteSelectedFilterRecords;
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

			//Serialize the JSON subscriptions object
			currentArea.subscriptions = angular.fromJson(currentArea.subscriptions);

			currentArea.subscriptions.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			return currentArea;
		}

		///////////////////////
		function getCurrentEntityFromArea(entityName, area) {
			var currentEntity = {};

			for (var i = 0; i < area.subscriptions.length; i++) {
				if (area.subscriptions[i].name == entityName) {
					currentEntity = area.subscriptions[i];
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
		function getListRecords(listName, entityName, filter, page, search, successCallback, errorCallback) {
			//api/v1/en_US/record/{entityName}/list/{listName}
			$log.debug('webvellaAreas>providers>areas.service>getListRecords> function called ' + moment().format('HH:mm:ss SSSS'));
			if (!search || search == "") {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + filter + '/' + page }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				search = encodeURIComponent(search);
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + filter + '/' + page + '?search=' + search }).then(function getSuccessCallback(response){handleSuccessResult(response.data, response.status, successCallback, errorCallback);}, function getErrorCallback(response) {handleErrorResult(response.data, response.status, errorCallback);});
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

		///////////////////////
		function createListFilter(postObject, entityName, listName, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>createListFilter> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'filter/' + entityName + '/' + listName, data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////////
		function getListFilter(filterId, successCallback, errorCallback) {
			$log.debug('webvellaAreas>providers>areas.service>getListFilter> function called ' + moment().format('HH:mm:ss SSSS'));
			if (filterId != "all") {
				$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'filter/' + filterId }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				var dummyData = {};
				dummyData.errors = [];
				dummyData.accessWarnings = [];
				dummyData.message = "Success";
				dummyData.object = null;
				dummyData.success = true;
				dummyData.timestamp = moment().utc();
				handleSuccessResult(dummyData, 200, successCallback, errorCallback)
			}
		}

		///////////////////////
		function deleteSelectedFilterRecords(filter_id, postObject, successCallback, errorCallback) {
			$log.debug('webvellaAdmin>providers>admin.service>deleteSelectedFilterRecords> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'filter/' + filter_id + '/delete-records', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
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
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					if (fieldMeta.displayFormat) {
						return fieldMeta.displayFormat.replace("{0}", data);
					}
					else {
						return data;
					}
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
					if (data) {
						return "<span class='go-green'>true</span>";
					}
					else {
						return "<span class='go-red'>false</span>";
					}
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
					return "<span class='go-green'>true</span>";
				}
				else {
					return "<span class='go-red'>false</span>";
				}

			}



		}
		//3.Currency
		function getCurrencyString(data, fieldMeta) {
			if (!data) {
				return "";
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
					return moment(data).format("DD MMM YYYY");;
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
					return moment(data).format("DD MMM YYYY HH:mm");
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
					return data;
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
					return "<a href='" + data + "' taget='_blank' class='link-icon'>view file</a>";
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
				return "<a href='" + data + "' taget='_blank' class='link-icon'>view file</a>";
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
					return data;
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
					return "<a target='_blank' href='" + data + "'><img src='" + data + "' class='table-image'/></a>";
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
					return data.replace(/(?:\r\n|\r|\n)/g, '<br />');
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
				return data.replace(/(?:\r\n|\r|\n)/g, '<br />');
			}
		}
		//11.Multiselect
		function getMultiselectString(data, fieldMeta) {
			var generatedStringArray = [];
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					for (var i = 0; i < data.length; i++) {
						var selected = $filter('filter')(fieldMeta.options, { key: data[i] });
						generatedStringArray.push((data[i] && selected.length) ? selected[0].value : 'empty');
					}
					return generatedStringArray.join(', ');
				}
				else {
					var htmlString = "<ul class='field-list'>";
					for (var i = 0; i < data.length; i++) {
						htmlString += "<li>" + getMultiselectString(data[i], fieldMeta) + "</li>";
					}
					htmlString += "</ul>";
					return htmlString;
				}
			}
			else {
				for (var i = 0; i < data.length; i++) {
					var selected = $filter('filter')(fieldMeta.options, { key: data[i] });
					generatedStringArray.push((data[i] && selected.length) ? selected[0].value : 'empty');
				}
				return generatedStringArray.join(', ');
			}
		}
		//12. Number
		function getNumberString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return data;
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
			if (!data) {
				return "";
			}
			else if (data instanceof Array) {
				if (data.length == 0) {
					return "";
				}
				else if (data.length == 1) {
					return data * 100 + "%";
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
					return phoneUtils.formatInternational(data);
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
					return data;
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
					var selected = $filter('filter')(fieldMeta.options, { key: data });
					return (data && selected.length) ? selected[0].value : 'empty';
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
					return data;
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
					return "<a href='" + data + "' target='_blank'>" + data + "</a>";
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
				return "<a href='" + data + "' target='_blank'>" + data + "</a>";
			}
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
