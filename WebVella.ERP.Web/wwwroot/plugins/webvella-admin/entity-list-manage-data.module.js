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
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedCurrentEntityList: resolveCurrentEntityList
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', '$translate'];

	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout, $translate) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert("error in response!")
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert("error in response!")
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		return defer.promise;
	}

	resolveCurrentEntityList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', '$translate'];

	function resolveCurrentEntityList($q, $log, webvellaCoreService, $stateParams, $state, $timeout, $translate) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert("error in response!")
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert("error in response!")
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityList($stateParams.listName, $stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}

	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$timeout', 'ngToast', 'pageTitle',
	'resolvedCurrentEntityMeta', 'resolvedCurrentEntityList', 'webvellaCoreService', '$translate'];

	function controller($scope, $log, $rootScope, $state, $timeout, ngToast, pageTitle,
	resolvedCurrentEntityMeta, resolvedCurrentEntityList, webvellaCoreService, $translate) {

		var ngCtrl = this;
		ngCtrl.entity = resolvedCurrentEntityMeta;
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
		ngCtrl.list = fastCopy(resolvedCurrentEntityList);
		//#endregion

		ngCtrl.getData = function (dataType) {
			function successCallback(response) {
				switch (dataType) {
					case "defaultSample":
						ngCtrl.defaultData = response.object.data;
						break;
					case "defaultFull":
						ngCtrl.defaultData =  response.object;
						break;
					case "customSample":
						ngCtrl.customData  = response.object.data;
						break;
					case "customFull":
						ngCtrl.customData  =  response.object;
						break;
				}
				ngCtrl.loading[dataType] = false;
 			}
			function errorCallback(response) {
				switch (dataType) {
					case "defaultSample":
						ngCtrl.defaultData = "Error: " + response.message;
						break;
					case "defaultFull":
						ngCtrl.defaultData =  "Error: " + response.message;
						break;
					case "customSample":
						ngCtrl.customData  = "Error: " + response.message;
						break;
					case "customFull":
						ngCtrl.customData  =  "Error: " + response.message;
						break;
				}				
				ngCtrl.loading[dataType] = false;
			}

			var sampleListMeta = fastCopy(ngCtrl.list);

			ngCtrl.loading[dataType] = true;
			switch (dataType) {
				case "defaultSample":
					sampleListMeta.dataSourceUrl = null;
					break;
				case "defaultFull":
					sampleListMeta.dataSourceUrl = null;
					break;
				case "customSample":
					break;
				case "customFull":
					break;
			}
			webvellaCoreService.getRecordsByListMeta(sampleListMeta, ngCtrl.entity.name, 1, null, successCallback, errorCallback)

		}

	}
	//#endregion

})();
