/* area.service.js */

/**
* @desc all actions with site area
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')
        .service('webvellaRootService', service);

    service.$inject = ['$cookies','$http', 'wvAppConstants', '$log', '$rootScope', '$window', '$location', '$anchorScroll', 'ngToast', '$timeout'];

    /* @ngInject */
    function service($cookies, $http, wvAppConstants, $log, $rootScope, $window, $location, $anchorScroll, ngToast, $timeout) {
        var serviceInstance = this;

        serviceInstance.registerHookListener = registerHookListener;
        serviceInstance.launchHook = launchHook;
        serviceInstance.setPageTitle = setPageTitle;
        serviceInstance.setBodyColorClass = setBodyColorClass;
        serviceInstance.getSitemap = getSitemap;
        serviceInstance.login = login;
        serviceInstance.generateValidationMessages = generateValidationMessages;
        serviceInstance.GoToState = GoToState;
        serviceInstance.getCurrentUser = getCurrentUser;

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

        ///////////////////
        function generateValidationMessages(response, scopeObj, formObject, location) {
            $log.debug('webvellaRoot>providers>root.service>generateValidationMessages> function called');
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
                content: '<span class="go-red">Error:</span> ' + response.message
            });
            //Scroll top
            // set the location.hash to the id of
            // the element you wish to scroll to.
            location.hash('modal-top');

            // call $anchorScroll()
            $anchorScroll();
        }

        //////////////////
        function GoToState(state, stateName, params) {
        	$log.debug('webvellaRoot>providers>root.service>GoToState> function called');

            $timeout(function () {
            	state.go(stateName, params, { reload: true });
            }, 0);
        }

    	////////////////////
        function getSitemap(successCallback, errorCallback) {
        	$log.debug('webvellaRoot>providers>root.service>getAreaEntities> function called');
        	$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'sitemap' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

        ////////////////////
        function login(postObject, successCallback, errorCallback) {
            $log.debug('webvellaRoot>providers>root.service>login> function called');
            $http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'login', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
        }

    	//////////////////
        function getCurrentUser() {
        	var user = null;
        	var test = "{\"userId\":\"eabd66fd-8de1-4d79-9674-447ee89921c2\",\"email\":\"erp@webvella.com\",\"firstName\":\"system\",\"lastName\":\"user\",\"token\":\"D6N3SV+bfoU5MfveY6D97DSb8K59ACmB+hCY9dvZPHa7P7CUzIrutTUPoSqB5jwCCuxpNDRDlnL+D/ibZhLQA2AGD25BHyx7VTuSn7bD7AfIMa7sr8YaO0g32Mkp+vbX7OOmUBAxe3lEtwUiac3+uGK/H6jc5/7LxG9+ALxWb0GmjppxMscAm4h9D/bn6AAfOtkRx80A7JYS2dAexUUugYycvlqxJNp2zw3BU3wT0sIOqQPNyy2nZJxfAwyBaYM7VS94VvwFXn9XZYP8571zQA==\"}"
        	var cookieValue = $cookies.get("erp-auth");
        	var cookieValueDecoded = decodeURIComponent(cookieValue);
        	user = angular.fromJson(cookieValueDecoded);
        	return user;
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
                        $log.debug('webvellaRoot>providers>root.service> result failure: errorCallback not a function or missing ');
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
                    $log.debug('webvellaRoot>providers>root.service> result failure: API finished with error: ' + status);
                    ngToast.create({
                    	className: 'error',
                    	content: '<span class="go-red">Error:</span> ' + 'An API call finished with error: ' + status
                    });
                    break;
            }
        }

        function handleSuccessResult(data, status, successCallback, errorCallback) {
            if (successCallback === undefined || typeof (successCallback) != "function") {
                $log.debug('webvellaRoot>providers>root.service> result failure: successCallback not a function or missing ');
                alert("The successCallback argument is not a function or missing");
                return;
            }

            if (!data.success) {
                //when the validation errors occurred
                if (errorCallback === undefined || typeof (errorCallback) != "function") {
                    $log.debug('webvellaRoot>providers>root.service> result failure: errorCallback not a function or missing ');
                    alert("The errorCallback argument in handleSuccessResult is not a function or missing");
                    return;
                }
                errorCallback(data);
            }
            else {
                 $log.debug('webvellaRoot>providers>root.service> result success: get object ');
                successCallback(data);
            }
        }

    }
})();