/* base.module.js */

/**
* @desc this the base module of the Areas plugin
*/

(function () {
	'use strict';

	angular
        .module('webvellaProjects', ['ui.router'])
        .config(config)
		.run(run)
        .controller('WebVellaProjectsBaseController', controller);

	//#region << Configuration >>
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider.state('webvella-projects-base', {
			abstract: true,
			url: '/plugins/webvella-projects/',
			views: {
				"rootView": {
					controller: 'WebVellaProjectsBaseController',
					templateUrl: '/plugins/webvella-projects/base.view.html',
					controllerAs: 'baseCtrl'
				}
			},
			resolve: {
				pageTitle: function () {
					return GlobalCompanyName;
				},
				resolvedEntityList: resolveEntityList,
				resolvedCurrentUser: resolveCurrentUser,
				resolvedCurrentUserEntityPermissions: resolveCurrentUserEntityPermissions,
				resolvedEntityRelationsList: resolveEntityRelationsList
			}
		});
	};
	//#endregion

	//#region << Run >>
 	run.$inject = ['$log', '$rootScope', 'webvellaCoreService', '$translate'];
  
	function run($log, $rootScope, webvellaCoreService, $translate) {}
	//#endregion

	//#region << Resolve Function >>
	resolveEntityList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams'];
	function resolveEntityList($q, $log, webvellaCoreService, $state, $stateParams) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getEntityMetaList(successCallback, errorCallback);
		return defer.promise;
	}

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

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	function resolveEntityRelationsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {
		var defer = $q.defer();
		function successCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getRelationsList(successCallback, errorCallback);
		return defer.promise;
	}

	resolveCurrentUserEntityPermissions.$inject = ['$q', '$log', 'webvellaCoreService'];
	function resolveCurrentUserEntityPermissions($q, $log, webvellaCoreService) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getCurrentUserPermissions(successCallback, errorCallback);
		return defer.promise;
	}
 
	//#endregion

	//#region << Controller >>
	controller.$inject = ['$log', '$rootScope','$timeout'];
	function controller($log, $rootScope, $timeout) {
		var baseCtrl = this;
	}
	//#endregion

})();
