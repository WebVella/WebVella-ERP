/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin')
        .service('webvellaAdminService', service);

    service.$inject = ['$log','$http', 'wvAppConstants'];

    /* @ngInject */
    function service($log, $http, wvAppConstants) {
        var serviceInstance = this;

        serviceInstance.getMetaEntityList = getMetaEntityList;
        serviceInstance.createEntity = createEntity;
        serviceInstance.getEntityMeta = getEntityMeta;
        serviceInstance.deleteEntity = deleteEntity;

        ///////////////////////
        function getMetaEntityList(successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function createEntity(postObject,successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>createEntity> function called');
            var postData = {
                id: postObject.id,
                name: postObject.name,
                label: postObject.label,
                pluralLabel: postObject.pluralLabel,
                system: postObject.system
            };
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity', data: postData }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function getEntityMeta(name,successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>getEntityMeta> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/'+ name }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function deleteEntity(entityId, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>deleteEntity> function called');
            $http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
                case 400:
                    if (errorCallback === undefined || typeof (errorCallback) != "function") {
                        $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> result failure: errorCallback not a function or missing ');
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallback(data);
                    break;
                default:
                    $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> result failure: API call finished with error: ' + status);
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                errorCallback(data);
            }
            else {
                $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> result success: get object ');
                successCallback(data);
            }
        }

    }
})();