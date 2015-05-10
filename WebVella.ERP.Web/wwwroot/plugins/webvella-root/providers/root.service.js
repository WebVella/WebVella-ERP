/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .service('webvellaRootService', service);

    service.$inject = ['$http', 'wvAppConstants','$log','$rootScope'];

    /* @ngInject */
    function service($http, wvAppConstants,$log,$rootScope) {
        var serviceInstance = this;

        serviceInstance.registerHookListener = registerHookListener;
        serviceInstance.launchHook = launchHook;
        serviceInstance.getSiteMeta = getSiteMeta;
        serviceInstance.setPageTitle = setPageTitle;
        serviceInstance.setBodyColorClass = setBodyColorClass;


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
        function getSiteMeta(successCallback, errorCallback) {
            $log.debug('webvellaRoot>providers>root.service>getSiteMeta> function called');
            $http({ method: 'GET', url: wvAppConstants.apiSandboxBaseUrl + '/root/meta' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
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
                    errorCallback();
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
                status = 400;//Bad request
                errorCallback();
            }
            else {
                //Updating the application siteMetaValue but first sorting 
                var siteMeta = data.object;

                //Sort areas
                siteMeta.areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

                //Sections sort
                for (var i = 0; i < siteMeta.areas.length; i++) {
                    siteMeta.areas[i].sections.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

                    //Sort entities
                    for (var j = 0; j < siteMeta.areas[i].sections.length; j++) {
                        siteMeta.areas[i].sections[j].entities.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
                    }
                }
                data.object = siteMeta;
                $log.debug('webvellaRoot>providers>root.service>getSiteMeta> result success: get object ');
                successCallback(data);
            }
        }

    }
})();