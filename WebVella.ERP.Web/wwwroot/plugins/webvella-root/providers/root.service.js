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
		serviceInstance.logout = logout;
		serviceInstance.generateValidationMessages = generateValidationMessages;
		serviceInstance.GoToState = GoToState;
		serviceInstance.getCurrentUser = getCurrentUser;
		serviceInstance.getCurrentUserPermissions = getCurrentUserPermissions;
		serviceInstance.applyAreaAccessPolicy = applyAreaAccessPolicy;

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

			$log.debug('rootScope>events> "' + eventHookName + '" hook registered ' + moment().format('HH:mm:ss SSSS'));
		}

		/////////////////////
		function launchHook(eventHookName, data) {
			$rootScope.$emit(eventHookName, data);
			$log.debug('rootScope>events> "'+ eventHookName + '" emitted ' + moment().format('HH:mm:ss SSSS'));
		}

		///////////////////////
		function setPageTitle(pageTitle) {
			$log.debug('webvellaRoot>providers>root.service>setPageTitle> function called ' + moment().format('HH:mm:ss SSSS'));
			$rootScope.$emit("application-pageTitle-update", pageTitle);
			$log.debug('rootScope>events> "application-pageTitle-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		}

		//////////////////////
		function setBodyColorClass(color) {
			$log.debug('webvellaRoot>providers>root.service>setBodyColorClass> function called ' + moment().format('HH:mm:ss SSSS'));
			$rootScope.$emit("application-body-color-update", color);
			$log.debug('rootScope>events> "application-body-color-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		}

		///////////////////
		function generateValidationMessages(response, scopeObj, formObject, location) {
			$log.debug('webvellaRoot>providers>root.service>generateValidationMessages> function called ' + moment().format('HH:mm:ss SSSS'));
			//Fill in validationError boolean and message for each field according to the template
			// scopeDate.fieldNameError => boolean; scopeDate.fieldNameMessage => the error from the api; 
			scopeObj.validation = {};
			for (var i = 0; i < response.errors.length; i++) {
				scopeObj.validation[response.errors[i].key] = {};
				scopeObj.validation[response.errors[i].key]["message"] = response.errors[i].message;
				scopeObj.validation[response.errors[i].key]["state"] = true;
			}
			//Rebind the form with the data returned from the server
			formObject = response.object;
			//Notify with a toast about the error and show the server response.message
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
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
			$log.debug('webvellaRoot>providers>root.service>GoToState> function called ' + moment().format('HH:mm:ss SSSS'));

			$timeout(function () {
				state.go(stateName, params, { reload: true });
			}, 0);
		}

		////////////////////
		function getSitemap(successCallback, errorCallback) {
			$log.debug('webvellaRoot>providers>root.service>getAreaEntities> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'sitemap' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		////////////////////
		function login(postObject, successCallback, errorCallback) {
			$log.debug('webvellaRoot>providers>root.service>login> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'user/login', data: postObject }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		////////////////////
		function logout(successCallback, errorCallback) {
			$log.debug('webvellaRoot>providers>root.service>logout> function called' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'POST', url: wvAppConstants.apiBaseUrl + 'user/logout', data: {} }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
		}

		//////////////////
		function getCurrentUser() {
			var user = null;
			var cookieValue = $cookies.get("erp-auth");
			if (cookieValue) {
				var cookieValueDecoded = decodeURIComponent(cookieValue);
				user = angular.fromJson(cookieValueDecoded);
			}
			return user;
		}

		////////////////////
		function getCurrentUserPermissions(successCallback, errorCallback) {
			$log.debug('webvellaRoot>providers>root.service>getCurrentUserPermissions> function called ' + moment().format('HH:mm:ss SSSS'));
			$http({ method: 'GET', url: wvAppConstants.apiBaseUrl + 'user/permissions' }).success(function (data, status, headers, config) { handleSuccessResult(data, status, successCallback, errorCallback); }).error(function (data, status, headers, config) { handleErrorResult(data, status, errorCallback); });
			//var response = {};
			//response.success = true;
			//response.object = [];
			//var permission = {
			//	entityId: "",
			//	entityName:"item",
			//	canRead: true,
			//	canCreate: true,
			//	canUpdate: false,
			//	canDelete: true
			//};
			//response.object.push(permission);
			//permission = {
			//	entityId: "",
			//	entityName:"item_media",
			//	canRead: true,
			//	canCreate: true,
			//	canUpdate: true,
			//	canDelete: true
			//};
			//response.object.push(permission);
			//handleSuccessResult(response, status, successCallback, errorCallback);
		}

		///////////////////////////////////////////////////////////////////////
		function applyAreaAccessPolicy(areaName, currentUser, sitemap) {
			if (currentUser == null) {
				return false;
			}
	
			var currentAreaObject = null;
			for (var i = 0; i < sitemap.data.length; i++) {
				if (sitemap.data[i].name == areaName) {
					currentAreaObject = sitemap.data[i];
				}
			}
			if (currentAreaObject == null) {
				return false;
			}

			var areaRoles = angular.fromJson(currentAreaObject.roles);
			var userHasAreaRole = false;
			for (var j = 0; j < areaRoles.length; j++) {
				for (var k = 0; k < currentUser.roles.length; k++) {
					if (currentUser.roles[k] == areaRoles[j]) {
						userHasAreaRole = true;
						break;
					}

				}
			}
			if (userHasAreaRole) {
				return true;
			}
			else {
				return false;
			}
		}


		//// Aux methods //////////////////////////////////////////////////////

		// Global functions for result handling for all methods of this service
		function handleErrorResult(data, status, errorCallback) {
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
						$log.debug('webvellaRoot>providers>root.service> result failure: errorCallback not a function or missing ' + moment().format('HH:mm:ss SSSS'));
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
						content: messageString,
						timeout: 7000
					});
					errorCallback(data);
					break;
				default:
					$log.debug('webvellaRoot>providers>root.service> result failure: API finished with error: ' + status +' ' + moment().format('HH:mm:ss SSSS'));
					ngToast.create({
						className: 'error',
						content: '<span class="go-red">Error:</span> ' + 'An API call finished with error: ' + status
					});
					break;
			}
		}

		function handleSuccessResult(data, status, successCallback, errorCallback) {
			if (successCallback === undefined || typeof (successCallback) != "function") {
				$log.debug('webvellaRoot>providers>root.service> result failure: successCallback not a function or missing  ' + moment().format('HH:mm:ss SSSS'));
				alert("The successCallback argument is not a function or missing");
				return;
			}

			if (!data.success) {
				//when the validation errors occurred
				if (errorCallback === undefined || typeof (errorCallback) != "function") {
					$log.debug('webvellaRoot>providers>root.service> result failure: errorCallback not a function or missing  ' + moment().format('HH:mm:ss SSSS'));
					alert("The errorCallback argument in handleSuccessResult is not a function or missing");
					return;
				}
				errorCallback(data);
			}
			else {
				$log.debug('webvellaRoot>providers>root.service> result success: get object  ' + moment().format('HH:mm:ss SSSS'));
				successCallback(data);
			}
		}

	}
})();
