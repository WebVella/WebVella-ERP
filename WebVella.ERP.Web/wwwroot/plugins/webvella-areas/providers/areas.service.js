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
        serviceInstance.getAutoIncrementString = getAutoIncrementString;
        serviceInstance.getCheckboxString = getCheckboxString;
        serviceInstance.getCurrencyString = getCurrencyString;
        serviceInstance.getDateString = getDateString;
        serviceInstance.getDateTimeString = getDateTimeString;
        serviceInstance.getEmailString = getEmailString;
        serviceInstance.getFileString = getFileString;
        serviceInstance.getHtmlString = getHtmlString;
        serviceInstance.getImageString = getImageString;
        serviceInstance.getTextareaString = getTextareaString;
        serviceInstance.getMultiselectString = getMultiselectString;
        serviceInstance.getNumberString = getNumberString;
        serviceInstance.getPasswordString = getPasswordString;
        serviceInstance.getPercentString = getPercentString;
        serviceInstance.getPhoneString = getPhoneString;
        serviceInstance.getGuidString = getGuidString;
        serviceInstance.getDropdownString = getDropdownString;
        serviceInstance.getTextString = getTextString;
        serviceInstance.getUrlString = getUrlString;

		//#region << API calls >>
    	///////////////////////
        function getAreaByName(areaName, successCallback, errorCallback) {
        	$log.debug('webvellaAreas>providers>areas.service>getAreaByName> function called ' + moment().format('HH:mm:ss SSSS'));
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + '/meta/entity/' + areaName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
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
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function createEntityRecord(postObject, entityName, successCallback, errorCallback) {
        	$log.debug('webvellaAdmin>providers>admin.service>createEntityRecord> function called ' + moment().format('HH:mm:ss SSSS'));
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

		/////////////////////
        function getListRecords(listName, entityName, filter, page,search, successCallback, errorCallback) {
        	//api/v1/en_US/record/{entityName}/list/{listName}
        	$log.debug('webvellaAreas>providers>areas.service>getListRecords> function called ' + moment().format('HH:mm:ss SSSS'));
        	if (!search || search == "") {
        		$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + filter + '/' + page}).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        	}
        	else {
        		search = encodeURIComponent(search);
        		$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + filter + '/' + page + '?search=' + search }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        	}
        }

    	///////////////////////
        function getRecordsByFieldAndRegex(entityName, fieldName, postObject, successCallback, errorCallback) {
        	$log.debug('webvellaAdmin>providers>admin.service>getRecordsByFieldAndRegex> function called ' + moment().format('HH:mm:ss SSSS'));
        	$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/regex/' + fieldName, data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        ///////////////////////
        function getViewRecord(recordId, viewName, entityName, successCallback, errorCallback) {
            //"api/v1/en_US/record/{entityName}/view/{viewName}/{id}"
        	$log.debug('webvellaAreas>providers>areas.service>getEntityRecord> function called ' + moment().format('HH:mm:ss SSSS'));
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/view/' + viewName + '/' + recordId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

    	///////////////////////
        function createListFilter(postObject, entityName, listName, successCallback, errorCallback) {
        	$log.debug('webvellaAdmin>providers>admin.service>createListFilter> function called ' + moment().format('HH:mm:ss SSSS'));
        	$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'filter/' + entityName + '/' + listName, data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

    	///////////////////////
        function getListFilter(filterId, successCallback, errorCallback) {
        	$log.debug('webvellaAreas>providers>areas.service>getListFilter> function called ' + moment().format('HH:mm:ss SSSS'));
        	if (filterId != "all") {
        		$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'filter/' + filterId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
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
        	$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'filter/' + filter_id + '/delete-records', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }
		//#endregion

    	//#region << Field data presentation ///////////////////////////////////////////
		//1.Auto increment
		function getAutoIncrementString (data, fieldMeta) {
			if (!data) {
				return "";
			}
			else if (fieldMeta.displayFormat) {
				return fieldMeta.displayFormat.replace("{0}", data);
			}
			else {
				return data;
			}
		}  
    	//2.Checkbox
		function getCheckboxString(data, fieldMeta) {
			if (data) {
				return "<span class='go-green'>true</span>";
			}
			else {
				return "<span class='go-red'>false</span>";
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
			else {
				return moment(data).format("DD MMM YYYY");
			}
		}
    	//5.Datetime
		function getDateTimeString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else {
				return moment(data).format("DD MMM YYYY HH:mm");
			}
		}
    	//6.Email
		function getEmailString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else {
				//There is a problem in Angular when having in href -> the href is not rendered
				//return "<a href='mailto:" + fieldValue + "' data-rel='external'>" + fieldValue + "</a>";
				return data;
			}
		}
    	//7.File
		function getFileString(data, fieldMeta) {
			if (!data) {
				return "";
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
			else {
				return data;
			}
		}
    	//9.Image
		function getImageString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else {
				return "<img src='" + data + "' class='table-image'/>";
			}
		}
    	//10. Textarea
		function getTextareaString(data, fieldMeta) {
			return data;
		}
    	//11.Multiselect
		function getMultiselectString(data, fieldMeta) {
			var generatedStringArray = [];
			if (data.length === 0) {
				return "";
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
			return data;
		}
    	//13. Password
		function getPasswordString(data, fieldMeta) {
			return "******";
		}
    	//14.Percent
		function getPercentString(data, fieldMeta) {
			if (!data) {
				return "";
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
			else {
				return phoneUtils.formatInternational(data);
			}
		}
    	//16. Guid
		function getGuidString(data, fieldMeta) {
			return data;
		}
    	//17.Dropdown
		function getDropdownString(data, fieldMeta) {
			if (!data) {
				return "";
			}
			else {
				var selected = $filter('filter')(fieldMeta.options, { key: data });
				return (data && selected.length) ? selected[0].value : 'empty';
			}

		}
    	//18. Text
        function getTextString(data, fieldMeta) {
        	return data;
        }
    	//19.Url
        function getUrlString(data, fieldMeta) {
        	if (!data) {
        		return "";
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
                    	content: messageString
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
