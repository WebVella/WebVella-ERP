/* base.module.js */

/**
* @desc this the base module of the Areas plugin
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas', ['ui.router'])
        .config(config)
        .controller('WebVellaAreasBaseController', controller);

	//#region << Configuration >>
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider.state('webvella-area-base', {
			abstract: true,
			url: '/areas/:areaName/:entityName',
			views: {
				"rootView": {
					controller: 'WebVellaAreasBaseController',
					templateUrl: '/plugins/webvella-areas/base.view.html',
					controllerAs: 'baseCtrl'
				}
			},
			resolve: {
				pageTitle: function () {
					return GlobalCompanyName;
				},
				resolvedEntityList: resolveEntityList,
				resolvedAreas: resolveAreas,
				resolvedCurrentUser: resolveCurrentUser,
				resolvedCurrentUserEntityPermissions: resolveCurrentUserEntityPermissions,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRelationsList: resolveEntityRelationsList,
				checkedAccessPermission: checkAccessPermission
			}
		});
	};
	//#endregion

	//#region << Resolve Function >>
 	resolveAreas.$inject = ['$q', '$log', 'webvellaCoreService','$stateParams','$rootScope','$timeout'];
	function resolveAreas($q, $log, webvellaCoreService, $stateParams,$rootScope,$timeout) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getRecordsWithoutList(null,null,null,"area", successCallback, errorCallback);
		return defer.promise;
	}

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

	resolveCurrentEntityMeta.$inject = ['$q', '$log', '$state', '$stateParams','resolvedEntityList'];
	function resolveCurrentEntityMeta($q, $log, $state, $stateParams,resolvedEntityList) {
		var defer = $q.defer();
		for (var i = 0; i < resolvedEntityList.length; i++) {
			if(resolvedEntityList[i].name == $stateParams.entityName){
				defer.resolve(resolvedEntityList[i]);	
				break;
			}
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

	checkAccessPermission.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', 'resolvedAreas', 'resolvedCurrentUser', 'ngToast'];
	function checkAccessPermission($q, $log, webvellaCoreService, $stateParams, resolvedAreas, resolvedCurrentUser, ngToast) {
		var defer = $q.defer();
		var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">' + $stateParams.areaName + '</span> area';
		var accessPermission = webvellaCoreService.applyAreaAccessPolicy($stateParams.areaName, resolvedCurrentUser, resolvedAreas);
		if (accessPermission) {
			defer.resolve();
		}
		else {

			ngToast.create({
				className: 'error',
				content: messageContent,
				timeout: 7000
			});
			defer.reject("Area > No access. You do not have access to this area!");
		}
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
