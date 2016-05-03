/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewManageDataController', controller);

	//#region << Configuration >> /////////////////////////
	config.$inject = ['$stateProvider'];
	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-view-manage-data', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/views/:viewName/data',
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
					controller: 'WebVellaAdminEntityViewManageDataController',
					templateUrl: '/plugins/webvella-admin/entity-view-manage-data.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedOneRecord: resolveOneRecord
			},
			data: {

			}
		});
	};
	//#endregion

	//#region << Resolve >> ///////////////////////////////

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
	
	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}

 	resolveOneRecord.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
	
	function resolveOneRecord($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getRecordsWithoutList(null,null,1,$stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}
	

	//#endregion

	//#region << Controller >> ////////////////////////////
	controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal', '$timeout',
                            'resolvedCurrentEntityMeta', 'webvellaCoreService', 'ngToast','$translate','resolvedOneRecord','resolvedEntityList'];
	
	function controller($filter, $scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal, $timeout,
                        resolvedCurrentEntityMeta, webvellaCoreService, ngToast,$translate,resolvedOneRecord,resolvedEntityList) {
		var ngCtrl = this;
		ngCtrl.loading = {};
		//#region << Initialize Current Entity >>
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Update page title & hide the side menu >>
		$translate(['RECORD_VIEW_MANAGE_PAGE_TITLE', 'ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_VIEW_MANAGE_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
    	//#endregion

		//#region << Initialize View and Content Region >>
		ngCtrl.view = {};
		ngCtrl.originalView = {};
		for (var i = 0; i < ngCtrl.entity.recordViews.length; i++) {
			if (ngCtrl.entity.recordViews[i].name === $stateParams.viewName) {
				ngCtrl.view = fastCopy(ngCtrl.entity.recordViews[i]);
				ngCtrl.originalView = fastCopy(ngCtrl.entity.recordViews[i]);
			}
		}
		//#endregion
		ngCtrl.sampleRecordId = fastCopy(resolvedOneRecord.data[0].id);

		ngCtrl.getData = function (dataType) {
			function successCallback(response) {
				switch (dataType) {
					case "defaultData":
						ngCtrl.defaultData = response.object;
						break;
					case "defaultMeta":
						ngCtrl.defaultMeta =  response.object;
						break;
					case "customFull":
						ngCtrl.customFull  =  response;
						break;
				}
				ngCtrl.loading[dataType] = false;
 			}
			function errorCallback(response) {
				switch (dataType) {
					case "defaultData":
						ngCtrl.defaultData = "Error: " + response.message;
						break;
					case "defaultMeta":
						ngCtrl.defaultMeta =  "Error: " + response.message;
						break;
					case "customFull":
						ngCtrl.customFull  =  "Error: " + response.message;
						break;
				}				
				ngCtrl.loading[dataType] = false;
			}

			var sampleViewMeta = fastCopy(ngCtrl.view);

			ngCtrl.loading[dataType] = true;
			switch (dataType) {
				case "defaultData":
					ngCtrl.defaultMeta = null;
					sampleViewMeta.dataSourceUrl = null;
					webvellaCoreService.getRecordByViewMeta(ngCtrl.sampleRecordId,sampleViewMeta, ngCtrl.entity.name, successCallback, errorCallback);
					break;
				case "defaultMeta":
					ngCtrl.defaultData = null;
					var response = {};
					response.success = true;
					response.object = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList(sampleViewMeta.name,ngCtrl.entity.name,resolvedEntityList);
					successCallback(response);
					break;
				case "customFull":
					webvellaCoreService.getRecordByViewMeta(ngCtrl.sampleRecordId,sampleViewMeta, ngCtrl.entity.name, successCallback, errorCallback);
					break;
			}
			

		}


	}

})();
