/* application.controller.js */

/**
* @desc the main application controller
*/

(function () {
	'use strict';

	angular
        .module('wvApp')
        .config(config)
        .controller('ApplicationController', controller);


	// Configuration ///////////////////////////////////
	config.$inject = ['$httpProvider', 'wvAppConstants'];

	/* @ngInject */
	function config($httpProvider, wvAppConstants) {

		$httpProvider.interceptors.push(function ($q, $location, ngToast, $cookies, $rootScope) {
			return {
				'request': function (request) {
					if (request.url.indexOf(wvAppConstants.apiBaseUrl) > -1) {
						if (request.url.indexOf("?") > -1) {
							//there are query string params in the request
							request.url = request.url + "&v=" + moment().format("YYYYMMDDHHmmssSSS");
						}
						else {
							//there are no query strings in the params
							request.url = request.url + "?v=" + moment().format("YYYYMMDDHHmmssSSS");
						}
					}
					else if (request.url.indexOf("/plugins/") > -1) {
						if (request.url.indexOf("?") > -1) {
							//there are query string params in the request
							request.url = request.url + "&v=" + wvAppConstants.htmlCacheBreaker;
						}
						else {
							//there are no query strings in the params
							request.url = request.url + "?v=" + wvAppConstants.htmlCacheBreaker;
						}
					}

					return $q.resolve(request);
				},
				'responseError': function (errorResponse) {
					if (errorResponse.status === 401) {
						//Check if already called if yes do not call redirect or show message
						ngToast.create({
							className: 'error',
							content: '<span class="go-red">Error code ' + errorResponse.status + '</span> ' + errorResponse.statusText,
							timeout: 7000
						});
						var cookieValue = $cookies.remove("erp-auth");
						$location.path("/login");//.search({ returnUrl: angular.toJson(document.URL) });
						return $q.reject(errorResponse);
					}
					else if (errorResponse.status === 403) {
						ngToast.create({
							className: 'error',
							content: '<span class="go-red">Error code ' + errorResponse.status + '</span> ' + errorResponse.statusText,
							timeout: 7000
						});
						return $q.reject(errorResponse);
					}
					else if (errorResponse.status === 500) {
						ngToast.create({
							className: 'error',
							content: '<span class="go-red">Error code ' + errorResponse.status + '</span> ' + errorResponse.statusText,
							timeout: 7000
						});
						return $q.reject(errorResponse);
					}
					return $q.reject(errorResponse);
				}
			}
		});
		delete $httpProvider.defaults.headers.common["X-Requested-With"];
	}


	// Controller ///////////////////////////////
	controller.$inject = ['$rootScope', '$log', '$cookies', '$localStorage', '$timeout', '$state', 'webvellaRootService'];

	/* @ngInject */
	function controller($rootScope, $log, $cookies, $localStorage, $timeout, $state, webvellaRootService) {
		$log.debug('vwApp> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var appData = this;
		//Set page title
		appData.pageTitle = 'WebVella ERP';
		$rootScope.$on("application-pageTitle-update", function (event, newValue) {
			appData.pageTitle = newValue;
		});
		//Set the body color
		appData.bodyColor = "no-color";
		$rootScope.$on("application-body-color-update", function (event, color) {
			appData.bodyColor = color;
		});
		//Side menu toggle
		appData.$storage = $localStorage;
		if (!appData.$storage.isMiniSidebar) {
			appData.$storage.isMiniSidebar = false;
		}
		//appData.isMiniSidebar = false;
		//$rootScope.isMiniSidebar = false;
		//if ($cookies.get("isMiniSidebar") == "true") {
		//	appData.isMiniSidebar = true;
		//	$rootScope.isMiniSidebar = true;
		//}

		//$rootScope.$on("application-sidebar-mini-toggle", function (event) {
		//	appData.isMiniSidebar = !appData.isMiniSidebar;
		//	$rootScope.isMiniSidebar = appData.isMiniSidebar;
		//	$cookies.put("isMiniSidebar", appData.isMiniSidebar);
		//});
		//Side menu visibility
		appData.sideMenuIsVisible = true;
		$rootScope.$on("application-body-sidebar-menu-isVisible-update", function (event, isVisible) {
			appData.sideMenuIsVisible = isVisible;
		});

		//Redirect State (usefull when you need to redirect from resolve)
		$rootScope.$on("state-change-needed", function (event, stateName, stateParams) {
			$timeout(function () {
				$state.go(stateName, stateParams, { reload: true });
			}, 0);
		});

		$log.debug('wvApp> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

})();
