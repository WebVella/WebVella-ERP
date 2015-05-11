/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaDevelopers')
        .service('webvellaDevelopersService', service);

    service.$inject = ['$log','$http', 'wvAppConstants'];

    /* @ngInject */
    function service($log, $http, wvAppConstants) {
        var serviceInstance = this;

        //serviceInstance.getMetaEntityList = getMetaEntityList;
        //serviceInstance.createEntity = createEntity;
        //serviceInstance.getEntityMeta = getEntityMeta;

        /////////////////////////
        //function getMetaEntityList(successCallback, errorCallback) {
        //    $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> function called');
        //    $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        //}

        /////////////////////////
        //function createEntity(postObject,successCallback, errorCallback) {
        //    $log.debug('webvellaAdmin>providers>admin.service>createEntity> function called');
        //    var postData = {
        //        id: postObject.id,
        //        name: postObject.name,
        //        label: postObject.label,
        //        pluralLabel: postObject.pluralLabel,
        //        system: postObject.system
        //    };
        //    $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity', data: postData }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        //}

        /////////////////////////
        //function getEntityMeta(name,successCallback, errorCallback) {
        //    $log.debug('webvellaAdmin>providers>admin.service>getEntityMeta> function called');
        //    $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/'+ name }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        //}

        //// Aux methods //////////////////////////////////////////////////////

    }
})();