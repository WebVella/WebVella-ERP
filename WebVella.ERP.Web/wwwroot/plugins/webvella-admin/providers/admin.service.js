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
        serviceInstance.initEntity = initEntity;
        serviceInstance.createEntity = createEntity;
        serviceInstance.patchEntity = patchEntity;
        serviceInstance.getEntityMeta = getEntityMeta;
        serviceInstance.deleteEntity = deleteEntity;
        serviceInstance.initField = initField;
        serviceInstance.createField = createField;
        serviceInstance.updateField = updateField;
        serviceInstance.deleteField = deleteField;
        serviceInstance.initRelation = initRelation;
        serviceInstance.getRelationsList = getRelationsList;


        //#region << Entity >>

        ///////////////////////
        function getMetaEntityList(successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>getMetaEntityList> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function initEntity() {
            $log.debug('webvellaAdmin>providers>admin.service>initEntity> function called');
            var entity = {
                id: null,
                name: "",
                label: "",
                labelPlural: "",
                system: false,
                iconName: "database",
                weight: 1.0,
                recordPermissions: {
                    canRead: [],
                    canCreate: [],
                    canUpdate: [],
                    canDelete: []
                }
            };
            return entity;
        }


        ///////////////////////
        function createEntity(postObject, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>createEntity> function called');
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function getEntityMeta(name,successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>getEntityMeta> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/entity/'+ name }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function patchEntity(entityId, patchObject, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>patchEntity> function called');
            $http({ method: 'PATCH', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId, data: patchObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }


        ///////////////////////
        function deleteEntity(entityId, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>deleteEntity> function called');
            $http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }



        //#endregion << Entity >>

        //#region << Field >>


        ///////////////////////
        function initField(typeId) {
            $log.debug('webvellaAdmin>providers>admin.service>initField> function called');
            var field = {
                id: null,
                name: "",
                label: "",
                placeholderText: "",
                description: "",
                helpText: "",
                required: false,
                unique: false,
                searchable: false,
                auditable: false,
                system: false,
                fieldType: typeId,
            };

            switch (typeId) {
                case 1:
                    field.defaultValue = 1;
                    field.startingNumber = 1;
                    field.displayFormat = "";
                    break;
                case 2:
                    field.defaultValue = false;
                    break;
                case 3:
                    field.defaultValue = null;
                    field.minValue = null;
                    field.maxValue = null;
                    field.currency = {
                        currencySymbol: "",
                        currencyName: "",
                        position:0
                    };
                    break;
                case 4:
                    field.defaultValue = null;
                    field.format = "";
                    field.useCurrentTimeAsDefaultValue = false;
                    break;
                case 5:
                    field.defaultValue = null;
                    field.format = "";
                    field.useCurrentTimeAsDefaultValue = false;
                    break;
                case 6:
                    field.defaultValue = '';
                    field.maxLength = null;
                    break;
                case 7:
                    field.defaultValue = '';
                    break;
                case 8:
                    field.defaultValue = '';
                    break;
                case 9:
                    field.defaultValue = '';
                    break;
                case 10:
                    field.defaultValue = '';
                    field.maxLength = null;
                    field.visibleLineNumber = false;
                    break;
                case 11:
                    field.defaultValue = '';
                    field.options = [];
                    break;
                case 12:
                    field.defaultValue = null;
                    field.minValue = null;
                    field.maxValue = null;
                    field.decimalPlaces = 2;
                    break;
                case 13:
                    field.maxLength = null;
                    field.encrypted = true;
                    field.maskType = 1;
                    break;
                case 14:
                    field.defaultValue = null;
                    field.minValue = null;
                    field.maxValue = null;
                    field.decimalPlaces = 2;
                    break;
                case 15:
                    field.defaultValue = "";
                    field.format = "";
                    field.maxLength = null;
                    break;
                case 16:
                    field.defaultValue = "00000000-0000-0000-0000-000000000000";
                    break;
                case 17:
                    field.defaultValue = "";
                    field.options = [];
                    break;
                case 18:
                    field.defaultValue = "";
                    field.maxLength = null;
                    break;
                case 19:
                    field.defaultValue = "";
                    field.maxLength = null;
                    field.openTargetInNewWindow = false;
                    break;
                default:
                    break;
            }

            return field;
        }


        ///////////////////////
        function createField(postObject, entityId, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>createField> function called');
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function updateField(putObject, entityId, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>updateField> function called');
            $http({ method: 'PUT', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + putObject.id, data: putObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////////
        function deleteField(fieldId, entityId, successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>updateField> function called');
            $http({ method: 'DELETE', url: wvAppConstants.apiBaseUrl + 'meta/entity/' + entityId + '/field/' + fieldId }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        //#endregion

        //#region << Relations  >>

        ///////////////////////
        function initRelation() {
            $log.debug('webvellaAdmin>providers>admin.service>initRelation> function called');
            var relation = {
                id: null,
                name: "",
                label: "",
                description: "",
                system: false,
                relationType: 1,
                originEntityId: null,
                originFieldId: null,
                targetEntityId: null,
                targetFieldId: null,

            };
            return relation;
        }


        ///////////////////////
        function getRelationsList(successCallback, errorCallback) {
            $log.debug('webvellaAdmin>providers>admin.service>getRelationsList> function called');
            $http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'meta/relation/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        //#endregion


        //#region << Aux methods >>

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
                case 400:
                    if (errorCallback === undefined || typeof (errorCallback) != "function") {
                        $log.debug('webvellaAdmin>providers>admin.service> result failure: errorCallback not a function or missing ');
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallback(data);
                    break;
                default:
                    $log.debug('webvellaAdmin>providers>admin.service> result failure: API call finished with error: ' + status);
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaAdmin>providers>admin.service> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaAdmin>providers>admin.service> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                errorCallback(data);
            }
            else {
                $log.debug('webvellaAdmin>providers>admin.service> result success: get object ');
                successCallback(data);
            }
        }
        //#endregion
    }
})();