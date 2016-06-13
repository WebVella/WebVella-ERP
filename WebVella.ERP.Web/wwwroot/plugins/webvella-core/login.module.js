/* home.module.js */

/**
* @desc handles the "/" url. Manages a login form and redirects the user to "webvella-desktop-areas" state if authenticated successfully. The only module that should be accessed by not logged user
*/

(function () {
	'use strict';

	angular
        .module('webvellaCore')  //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaCoreLoginController', controller);

	//#region << Configuration >> ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	function config($stateProvider) {
		$stateProvider.state('webvella-core-login', {
			url: '/login?returnUrl',
			views: {
				"rootView": {
					controller: 'WebVellaCoreLoginController',
					templateUrl: '/plugins/webvella-core/login.view.html',
					controllerAs: 'loginData'
				}
			},
			resolve: {
				pageTitle: function () {
					return GlobalCompanyName;
				}
			}
		});
	};
	//#endregion

	//#region << Controller /////////////////////////////// >>
	controller.$inject = ['webvellaCoreService', '$timeout', 'pageTitle', '$stateParams','$location'];
	function controller(webvellaCoreService, $timeout, pageTitle, $stateParams,$location) {
		var loginData = this;
		loginData.loginIsActive = false;
		var currentUser = webvellaCoreService.getCurrentUser();
		if (currentUser != null) {
			$timeout(function () {
				webvellaCoreService.GoToState("webvella-desktop-browse", {});
			}, 10);
		}
		else {
			$timeout(function () {
				loginData.loginIsActive = true;
			}, 10);
		}

		//$timeout(function () {
		//	loginData.loginIsActive = true;
		//}, 2000);

		loginData.rememberMe = false;
		loginData.pageTitle = "Login | " + pageTitle;
		webvellaCoreService.setPageTitle(loginData.pageTitle);

		loginData.ValidationErrors = false;

		webvellaCoreService.setPageTitle(loginData.pageTitle);

		loginData.doLogin = function () {
			webvellaCoreService.login(loginData, function (response) {
				if ($stateParams.returnUrl != null) {
					var returnUrl = decodeURI($stateParams.returnUrl);
					$location.search("returnUrl", null);
					$location.path(returnUrl);
				}
				else {
					webvellaCoreService.GoToState("webvella-desktop-browse", {});
				}
			}, function (response) {
				//alert(response.message);
			});
		}
	}
	//#endregion
})();
