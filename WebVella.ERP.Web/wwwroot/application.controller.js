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


	//#region << Configuration  >> ///////////////////////////////////
	config.$inject = ['$httpProvider', 'wvAppConstants', '$translateProvider', '$sceDelegateProvider', '$locationProvider', '$qProvider', '$provide'];
	function config($httpProvider, wvAppConstants, $translateProvider, $sceDelegateProvider, $locationProvider, $qProvider, $provide) {

		//#region << Request interceptors >>
		$httpProvider.interceptors.push(function ($q, $location, ngToast, $cookies, $rootScope, $timeout) {
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
							timeout: 3000
						});
						var cookieValue = $cookies.remove("erp-auth");
						//we need to wait for this operation to finish before redirect
						$location.search("returnUrl",encodeURI($location.path()))
						$timeout(function () {
							$location.path("/login");
							return $q.reject(errorResponse);
						}, 100);
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
		//#endregion

		//#region << FastClick attach >>
		if ('addEventListener' in document) {
			document.addEventListener('DOMContentLoaded', function () {
				FastClick.attach(document.body);
			}, false);
		}
		//#endregion


		//REmove the !(exclamation) after the hash
		$locationProvider.hashPrefix('');

		//#region << Translation >>
		$translateProvider.preferredLanguage(GlobalLanguage);
		switch (GlobalLanguage) {
			case "bg":
				$translateProvider.translations(GlobalLanguage, translationsBG);
				break;
			case "en":
				$translateProvider.translations(GlobalLanguage, translationsEN);
				break;
			case "es":
				$translateProvider.translations(GlobalLanguage, translationsES);
				break;
			case "ru":
				$translateProvider.translations(GlobalLanguage, translationsRU);
				break;
		}
		$translateProvider.useSanitizeValueStrategy(null);
		//#endregion
		$qProvider.errorOnUnhandledRejections(false);
		//#region << Dynamic template providers
			//$sceDelegateProvider.resourceUrlWhitelist([
			//	'http://www.refsnesdata.no/**'
			//]);
		//#endregion

		//one second delay for clicks
		$provide.decorator('ngClickDirective', ['$delegate', '$timeout', function ($delegate, $timeout) {
			var original = $delegate[0].compile;
			var delay = 1000;
			$delegate[0].compile = function (element, attrs, transclude) {

				var disabled = false;
				function onClick(evt) {
					if (disabled) {
						evt.preventDefault();
						evt.stopImmediatePropagation();
					} else {
						disabled = true;
						$timeout(function () {
							disabled = false;
						}, delay, false);
					}
				}
				//   scope.$on('$destroy', function () { iElement.off('click', onClick); });
				element.on('click', onClick);

				return original(element, attrs, transclude);
			};
			return $delegate;
		}]);

	}
	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$rootScope', '$log', '$cookies', '$localStorage', '$timeout', '$state', 'webvellaCoreService','ngToast'];
	function controller($rootScope, $log, $cookies, $localStorage, $timeout, $state, webvellaCoreService,ngToast) {
		var appData = this;
		//Set page title
		appData.pageTitle = GlobalCompanyName;

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

		//Redirect State (usefull when you need to redirect from resolve)
		$rootScope.$on("state-change-needed", function (event, redirectObject) {
			event.preventDefault();
			$timeout(function () {
				$state.go(redirectObject.stateName, redirectObject.params, { reload: true });
			}, 0);
		});

		$rootScope.$on("$stateChangeSuccess", function () {
			$rootScope.adminSectionName = null;
			$rootScope.adminSubSectionName = null;
		});

		$rootScope.$on("$stateChangeError", function (event, toState, toParams, fromState, fromParams, error) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + error,
					timeout: 7000
				});
		 });
		//Set up object for the view and list Action services
		$rootScope.actionService = {};
	}
	//#endregion

})();
