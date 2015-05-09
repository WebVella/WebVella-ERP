/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .service('webvellaRootSiteMetaService', service);

    service.$inject = ['$http', 'wvAppConstants','$log'];

    /* @ngInject */
    function service($http, wvAppConstants,$log) {
        var serviceInstance = this;

        serviceInstance.getSiteMeta = getSiteMeta;


        ///////////////////////
        function getSiteMeta(successCallback, errorCallback) {
            $log.debug('webvellaRoot>providers>sitemeta>getSiteMeta> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + '/root/meta' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
                case 400:
                    if (errorCallback === undefined || typeof (errorCallback) != "function") {
                        $log.debug('webvellaRoot>providers>sitemeta>getSiteMeta> result failure: errorCallback not a function or missing ');
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallback();
                    break;
                default:
                    $log.debug('webvellaRoot>providers>sitemeta>getSiteMeta> result failure: API finished with error: ' + status);
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaRoot>providers>sitemeta>getSiteMeta> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaRoot>providers>sitemeta>getSiteMeta> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                status = 400;//Bad request
                errorCallback();
            }
            else {
                $log.debug('webvellaRoot>providers>sitemeta>getSiteMeta> result success: get object ');
                successCallback(data);
            }
        }

    }
})();