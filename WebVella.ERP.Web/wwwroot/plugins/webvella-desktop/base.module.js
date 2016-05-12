/* base.module.js */

/**
* @desc this the base module of the Desktop plugin. Its only tasks is to check the topNavFactory and redirect to the first menu item state
*/

(function () {
	'use strict';

	angular
        .module('webvellaDesktop', ['ui.router'])
        .config(config)
        .controller('WebVellaDesktopBaseController', controller);

	//#region << Configuration >> ///////////////////////////////////
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider.state('webvella-desktop-base', {
			abstract: true,
			url: '',
			views: {
				"rootView": {
					controller: 'WebVellaDesktopBaseController',
					templateUrl: '/plugins/webvella-desktop/base.view.html?v=' + htmlCacheBreaker,
					controllerAs: 'baseCtrl'
				}
			},
			resolve: {
				pageTitle: function () {
					return "Webvella ERP";
				},
				resolvedCurrentUser: resolveCurrentUser
			},
			data: {}
		});
	};
	//#endregion

	//#region << Resolve >> /////////////////////////////////
	resolveCurrentUser.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams'];
	
	function resolveCurrentUser($q, $log, webvellaCoreService, $state, $stateParams) {
		var defer = $q.defer();
		var currentUser = webvellaCoreService.getCurrentUser();
		if (currentUser != null) {
			defer.resolve(currentUser);
		}
		else {
			defer.reject(null);
		}
		return defer.promise;
	}
	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$stateParams', 'webvellaDesktopTopnavFactory', '$timeout', 'webvellaCoreService', 'resolvedCurrentUser'];
	function controller($scope, $log, $rootScope, $state, $stateParams, webvellaDesktopTopnavFactory, $timeout, webvellaCoreService, resolvedCurrentUser) {
		var baseCtrl = this;
		baseCtrl.topnav = [];
		baseCtrl.user = resolvedCurrentUser;

		//Making topnav pluggable
		////1. CONSTRUCTOR initialize the factory
		webvellaDesktopTopnavFactory.initTopnav();
		////2. READY hook listener
		var readyTopnavDestructor = $rootScope.$on("webvellaDesktop-topnav-ready", function (event, data) {
			//All actions you want to be done after the "Ready" hook is cast
		})
		////3. UPDATED hook listener
		var updateTopnavDestructor = $rootScope.$on("webvellaDesktop-topnav-updated", function (event, data) {
			baseCtrl.topnav = data;
			activate();
		});
		////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
		$scope.$on("$destroy", function () {
			readyTopnavDestructor();
			updateTopnavDestructor();
		});

		$timeout(function () {
			$rootScope.$emit("webvellaDesktop-topnav-ready");
		}, 0);
		baseCtrl.logout = function () {
			webvellaCoreService.logout(
                    function (response) {
                    	$timeout(function () {
                    		$state.go('webvella-core-login');
                    	}, 0);
                    },
                    function (response) { });
		}

		baseCtrl.isNavActive = function (item) {
			if (item.label == $state.params.folder) {
				return true;
			}
			else {
				return false;
			}
		}

		function activate() {
			if (baseCtrl.topnav.length > 0) {
				$timeout(function () {
					if (!$state.params.folder) {
						$state.go(baseCtrl.topnav[0].stateName, baseCtrl.topnav[0].stateParams)
					}
				}, 0);

			}
		}
	}
	//#endregion
})();
