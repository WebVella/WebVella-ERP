/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaDevelopers')
        .service('webvellaDevelopersQueryService', service);

    service.$inject = ['$log','$http', 'wvAppConstants','ngToast'];

    /* @ngInject */
    function service($log, $http, wvAppConstants,ngToast) {
        var serviceInstance = this;

        serviceInstance.executeSampleQuery = executeSampleQuery;
        serviceInstance.createSampleQueryDataStructure = createSampleQueryDataStructure;
        serviceInstance.executeSampleRelationRecordUpdate = executeSampleRelationRecordUpdate;
        serviceInstance.moveFile = moveFile;
        serviceInstance.deleteFile = deleteFile;

        

        /////////////////////////
        function executeSampleRelationRecordUpdate(postObject, successCallback, errorCallback) {
            $log.debug('webvellaDevelopers>providers>query.service>execute executeSampleRelationRecordUpdate> function called');
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'record/relation', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        /////////////////////////
        function executeSampleQuery(postObject,successCallback, errorCallback) {
        	$log.debug('webvellaDevelopers>providers>query.service>execute sample query> function called');
            var postData = {};
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/developers/query/execute-sample-query', data: postData }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

    	/////////////////////////
        function createSampleQueryDataStructure(postObject, successCallback, errorCallback) {
        	$log.debug('webvellaDevelopers>providers>query.service>execute sample query> function called');
        	var postData = {};
        	$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/developers/query/create-sample-query-data-structure', data: postData }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        /////////////////////////
        function moveFile(postObject, successCallback, errorCallback) {
        	$log.debug('webvellaDevelopers>providers>query.service>move fs> function called');
            $http({ method: 'POST', url: "/fs/move/", data: postObject })
                .success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); })
                .error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        /////////////////////////
        function deleteFile(file, successCallback, errorCallback) {
        	$log.debug('webvellaDevelopers>providers>query.service>move fs> function called');
            $http({ method: 'DELETE', url: "/fs/delete" + file })
                .success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); })
                .error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        //// Aux methods //////////////////////////////////////////////////////

    	// Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
        	$log.debug("error:", data, status, errorCallback);
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
        				$log.debug('webvellaDevelopers>providers>query.service> result failure: errorCallback not a function or missing ');
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
        			$log.debug('webvellaDevelopers>providers>query.service> result failure: API call finished with error: ' + status);
        			alert("An API call finished with error: " + status);
        			break;
        	}
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
          
        	if (successCallback === undefined || typeof (successCallback) != "function") {
        		$log.debug('webvellaDevelopers>providers>query.service> result failure: successCallback not a function or missing ');
        		alert("The successCallback argument is not a function or missing");
        		return;
        	}

        	if (!data.success) {
        		//when the validation errors occurred
        		if (errorCallback === undefined || typeof (errorCallback) != "function") {
        			$log.debug('webvellaDevelopers>providers>query.service> result failure: errorCallback not a function or missing ');
        			alert("The errorCallback argument in handleSuccessResult is not a function or missing");
        			return;
        		}
        		errorCallback(data);
        	}
        	else {
        		$log.debug('webvellaDevelopers>providers>query.service> result success: get object ');
        		successCallback(data);
        	}
        }
    }
})();