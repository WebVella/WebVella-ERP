/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas')
        .service('webvellaAreasService', service);

    service.$inject = ['$log','$http', 'wvAppConstants','$timeout'];

    /* @ngInject */
    function service($log, $http, wvAppConstants, $timeout) {
        var serviceInstance = this;

        serviceInstance.getAreaByName = getAreaByName;
        serviceInstance.getCurrentAreaFromSitemap = getCurrentAreaFromSitemap;
        serviceInstance.getCurrentEntityFromArea = getCurrentEntityFromArea;
        serviceInstance.getViewByName = getViewByName;
        serviceInstance.getEntityRecord = getEntityRecord;
        serviceInstance.createEntityRecord = createEntityRecord;
        serviceInstance.getListRecords = getListRecords;
        ///////////////////////
        function getAreaByName(areaName, successCallback, errorCallback) {
            $log.debug('webvellaAreas>providers>areas.service>getAreaByName> function called');
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
        	currentArea.entities.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
        	return currentArea;
        }

    	///////////////////////
        function getCurrentEntityFromArea(entityName, area) {
        	var currentEntity = {};

        	for (var i = 0; i < area.entities.length; i++) {
        		if (area.entities[i].name == entityName) {
        			currentEntity = area.entities[i];
        		}
        	}

        	return currentEntity;
        }


        ///////////////////////
        function getViewByName(viewName, entityName, successCallback, errorCallback) {
            $log.debug('webvellaAreas>providers>areas.service>getViewMetaByName> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityName + '/view/' + viewName }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function getEntityRecord(recordId, entityName, successCallback, errorCallback) {
        	$log.debug('webvellaAreas>providers>areas.service>getEntityRecord> function called');
        	$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'record/' + entityName + '/' + recordId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function createEntityRecord(postObject, entityName, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>createEntityRecord> function called');
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/' + entityName, data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

		/////////////////////
        function getListRecords(listName, entityName,page, successCallback, errorCallback) {
        	$log.debug('webvellaAreas>providers>areas.service>getListRecords> function called');
        	var response = {};
        	response.success = true;
        	response.message = "success";
        	response.object = {
        		"fieldsMeta": [
                  {
                  	"fieldType": 18,
                  	"defaultValue": "",
                  	"maxLength": null,
                  	"id": "60b9dfe5-6dc7-4445-9481-83a1655b9a63",
                  	"name": "id",
                  	"label": "Id",
                  	"placeholderText": null,
                  	"description": null,
                  	"helpText": null,
                  	"required": true,
                  	"unique": false,
                  	"searchable": false,
                  	"auditable": false,
                  	"system": true
                  },
                  {
                  	"fieldType": 18,
                  	"defaultValue": "",
                  	"maxLength": null,
                  	"id": "60b9dfe5-6dc7-4445-9481-83a1655b9a60",
                  	"name": "title",
                  	"label": "Title",
                  	"placeholderText": null,
                  	"description": null,
                  	"helpText": null,
                  	"required": true,
                  	"unique": false,
                  	"searchable": false,
                  	"auditable": false,
                  	"system": false
                  },
                  {
                  	"fieldType": 18,
                  	"defaultValue": "",
                  	"maxLength": null,
                  	"id": "37964efb-f66b-4be9-b8ce-2820054946a6",
                  	"name": "content",
                  	"label": "Content",
                  	"placeholderText": null,
                  	"description": null,
                  	"helpText": null,
                  	"required": false,
                  	"unique": false,
                  	"searchable": false,
                  	"auditable": false,
                  	"system": false
                  }

        		],
        		"data": [
                  {
                  	"id": "47964efb-f66b-4be9-b8ce-2820054946a6",
                  	"username": "boz",
                  	"content": "post 3 content",
                  }
        		]
        	};
        	handleSuccessResult(response, status, successCallback, errorCallback);
        }

        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
                case 400:
                    if (errorCallback === undefined || typeof (errorCallback) != "function") {
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallback(data);
                    break;
                default:
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaAreas>providers>areas.service>getAreaByName> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaAreas>providers>areas.service>getAreaByName> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                errorCallback(data);
            }
            else {
                $log.debug('webvellaAreas>providers>areas.service>getAreaByName> result success: get object ');
                successCallback(data);
            }
        }

    }
})();