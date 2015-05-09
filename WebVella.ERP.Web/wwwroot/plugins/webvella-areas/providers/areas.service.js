/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas')
        .service('webvellaAreasService', service);

    service.$inject = ['$http', 'wvAppConstants', 'webvellaAreasFactory'];

    /* @ngInject */
    function service($http, wvAppConstants, webvellaAreasFactory) {
        var serviceInstance = this;

        serviceInstance.getAllAreas = getAllAreas;


        ///////////////////////
        function getAllAreas(successCallback, errorCallback) {
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + '/entities/areas' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
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
                    errorCallback();
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
                status = 400;//Bad request
                errorCallback();
            }
            else {
                webvellaAreasFactory.updateAreas(data.object);
                successCallback(data);
            }
        }

    }
})();