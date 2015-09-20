/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas')
        .service('webvellaAreasService', service);

    service.$inject = ['$log','$http', 'wvAppConstants','$timeout','ngToast'];

    /* @ngInject */
    function service($log, $http, wvAppConstants, $timeout, ngToast) {
        var serviceInstance = this;

        serviceInstance.getAreaByName = getAreaByName;
        serviceInstance.getCurrentAreaFromSitemap = getCurrentAreaFromSitemap;
        serviceInstance.getCurrentEntityFromArea = getCurrentEntityFromArea;
        serviceInstance.getViewByName = getViewByName;
        serviceInstance.createEntityRecord = createEntityRecord;
        serviceInstance.getListRecords = getListRecords;
        serviceInstance.getViewRecord = getViewRecord;
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
        		$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/list/' + listName + '/' + filter + '/' + page + '?search=' + search }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        	}
        }

        ///////////////////////
        function getViewRecord(recordId, viewName, entityName, successCallback, errorCallback) {
            //"api/v1/en_US/record/{entityName}/view/{viewName}/{id}"
        	$log.debug('webvellaAreas>providers>areas.service>getEntityRecord> function called ' + moment().format('HH:mm:ss SSSS'));
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/view/' + viewName + '/' + recordId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
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
