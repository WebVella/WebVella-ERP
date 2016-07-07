/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListManageDataController', controller);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];


	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-list-manage-data', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/lists/:listName/data', //  /desktop/areas after the parent state is prepended
			views: {
				"topnavView": {
					controller: 'WebVellaAdminTopnavController',
					templateUrl: '/plugins/webvella-admin/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAdminSidebarController',
					templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminEntityListManageDataController',
					templateUrl: '/plugins/webvella-admin/entity-list-manage-data.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedEntityList:resolveEntityList
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////

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

	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$timeout', 'ngToast', 'pageTitle',
	'webvellaCoreService', '$translate','resolvedEntityList','$stateParams'];

	function controller($scope, $log, $rootScope, $state, $timeout, ngToast, pageTitle,
	webvellaCoreService, $translate,resolvedEntityList,$stateParams) {

		var ngCtrl = this;
		ngCtrl.entity = webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName,resolvedEntityList);
		ngCtrl.loading = {};
		//#region << Update page title & hide the side menu >>
		$translate(['RECORD_LIST_TAB_ACTIONS_PAGE_TITLE', 'ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_LIST_TAB_ACTIONS_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#endregion

		//#region << Initialize the list >>
		ngCtrl.list = webvellaCoreService.getEntityRecordListFromEntitiesMetaList($stateParams.listName,$stateParams.entityName,resolvedEntityList);
		//#endregion

		ngCtrl.getData = function (dataType) {
			function successCallback(response) {
				switch (dataType) {
					case "defaultData":
						ngCtrl.defaultData = response.object;
						break;
					case "defaultMeta":
						ngCtrl.defaultData =  response.object;
						break;
					case "customFull":
						ngCtrl.customFull  = response;
						break;
				}
				ngCtrl.loading[dataType] = false;
 			}
			function errorCallback(response) {
				switch (dataType) {
					case "defaultData":
						ngCtrl.defaultData = "Error: " + response;
						break;
					case "defaultMeta":
						ngCtrl.defaultMeta =  "Error: " + response;
						break;
					case "customFull":
						ngCtrl.customFull  = "Error: " + response;
						break;
				}				
				ngCtrl.loading[dataType] = false;
			}

			var sampleListMeta = fastCopy(ngCtrl.list);

			ngCtrl.loading[dataType] = true;
			switch (dataType) {
				case "defaultData":
					ngCtrl.defaultMeta = null;
					sampleListMeta.dataSourceUrl = null;
					webvellaCoreService.getRecordsByListMeta(sampleListMeta, ngCtrl.entity.name, 1, null, null, successCallback, errorCallback);
					break;
				case "defaultMeta":
					ngCtrl.defaultData = null;
					var response = {};
					response.success = true;
					response.object = webvellaCoreService.getEntityRecordListFromEntitiesMetaList(sampleListMeta.name,ngCtrl.entity.name,resolvedEntityList);
					successCallback(response);
					break;
				case "customFull":
					webvellaCoreService.getRecordsByListMeta(sampleListMeta, ngCtrl.entity.name, 1, null, null, successCallback, errorCallback);
					break;
			}
			

		}

	}
	//#endregion

})();
