/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .service('webvellaRootService', service);

    service.$inject = ['$http', 'wvAppConstants', '$log', '$rootScope', '$window', '$location', '$anchorScroll', 'ngToast', '$timeout'];

    /* @ngInject */
    function service($http, wvAppConstants, $log, $rootScope, $window, $location, $anchorScroll, ngToast, $timeout) {
        var serviceInstance = this;

        serviceInstance.registerHookListener = registerHookListener;
        serviceInstance.launchHook = launchHook;
        serviceInstance.getEntityRecordsByName = getEntityRecordsByName;
        serviceInstance.setPageTitle = setPageTitle;
        serviceInstance.setBodyColorClass = setBodyColorClass;
        serviceInstance.generateValidationMessages = generateValidationMessages;
        serviceInstance.reloadCurrentState = reloadCurrentState;

        ///////////////////////
        function registerHookListener(eventHookName, currentScope, executeOnHookFunction) {
            if (executeOnHookFunction === undefined || typeof (executeOnHookFunction) != "function") {
                $log.error('webvellaRoot>providers>root.service>registerHookListener> result failure: The executeOnHookFunction argument is not a function or missing ');
                alert("The executeOnHookFunction argument is not a function or missing ");
                return;
            }
            //When registering listener with $on, it returns automatically a function that can remove this listener. We will use it later
            var unregisterFunc = $rootScope.$on(eventHookName, function (event, data) {
                executeOnHookFunction(event, data);
            });
            //The listener should be manually removed as the rootScope is never destroyed, and this will lead to duplication the next time the controller is loaded
            currentScope.$on("$destroy", function () {
                unregisterFunc();
            });

            $log.debug('rootScope>events> "' + eventHookName + '" hook registered');
        }

        /////////////////////
        function launchHook(eventHookName, data) {
            $rootScope.$emit(eventHookName, data);
            $log.debug('rootScope>events> "'+ eventHookName + '" emitted');
        }

        ///////////////////////
        function setPageTitle(pageTitle) {
            $log.debug('webvellaRoot>providers>root.service>setPageTitle> function called');
            $rootScope.$emit("application-pageTitle-update", pageTitle);
            $log.debug('rootScope>events> "application-pageTitle-update" emitted');
        }

        //////////////////////
        function setBodyColorClass(color) {
            $log.debug('webvellaRoot>providers>root.service>setBodyColorClass> function called');
            $rootScope.$emit("application-body-color-update", color);
            $log.debug('rootScope>events> "application-body-color-update" emitted');
        }

        ////////////////////
        function getEntityRecordsByName(entityName,successCallback, errorCallback) {
            $log.debug('webvellaRoot>providers>root.service>getEntityRecords> function called');
            $http({ method: 'GET', url: wvAppConstants.apiSandboxBaseUrl + '/entity/' + entityName + '/records/list' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ///////////////////
        function generateValidationMessages(response, scopeObj, formObject, location) {
            $log.error('webvellaRoot>providers>root.service>generateValidationMessages> function called');
            //Fill in validationError boolean and message for each field according to the template
            // scopeDate.fieldNameError => boolean; scopeDate.fieldNameMessage => the error from the api; 
            for (var i = 0; i < response.errors.length; i++) {
                scopeObj[response.errors[i].key + "Message"] = response.errors[i].message;
                scopeObj[response.errors[i].key + "Error"] = true;
            }
            //Rebind the form with the data returned from the server
            formObject = response.object;
            //Notify with a toast about the error and show the server response.message
            ngToast.create({
                className: 'error',
                content: '<h4>Error</h4><p>' + response.message +'</p>'
            });
            //Scroll top
            // set the location.hash to the id of
            // the element you wish to scroll to.
            location.hash('modal-top');

            // call $anchorScroll()
            $anchorScroll();
        }

        //////////////////
        function reloadCurrentState(state) {
            $log.error('webvellaRoot>providers>root.service>reloadCurrentState> function called');
            $timeout(function () {
                state.go(state.current, {}, { reload: true });
            }, 0);
        }

        //// Aux methods //////////////////////////////////////////////////////

        // Global functions for result handling for all methods of this service
        function handleErrorResult(data, status, errorCallback) {
            switch (status) {
                case 400:
                    if (errorCallback === undefined || typeof (errorCallback) != "function") {
                        $log.debug('webvellaRoot>providers>root.service>getSiteMeta> result failure: errorCallback not a function or missing ');
                        alert("The errorCallback argument is not a function or missing");
                        return;
                    }
                    data.success = false;
                    errorCallback(data);
                    break;
                default:
                    $log.debug('webvellaRoot>providers>root.service>getSiteMeta> result failure: API finished with error: ' + status);
                    alert("An API call finished with error: " + status);
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaRoot>providers>root.service>getSiteMeta> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaRoot>providers>root.service>getSiteMeta> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                errorCallback(data);
            }
            else {
                 $log.debug('webvellaRoot>providers>root.service>getSiteMeta> result success: get object ');
                successCallback(data);
            }
        }

    }
})();