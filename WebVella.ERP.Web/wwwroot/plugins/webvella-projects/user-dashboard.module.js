(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .config(config)
        .controller('WebVellaProjectsUserDashboardController', controller);

	//#region << Configuration /////////////////////////////////// >>
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider
		//general list in an area
		.state('webvella-projects-dashboard', {
			parent: 'webvella-area-base',
			url: '/dashboard',
			params: {},
			views: {
				"topnavView": {
					controller: 'WebVellaAreasTopnavController',
					templateUrl: '/plugins/webvella-areas/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAreasSidebarController',
					templateUrl: '/plugins/webvella-areas/sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaProjectsUserDashboardController',
					templateUrl: '/plugins/webvella-projects/user-dashboard.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedCurrentViewData: function () { return null; },
				resolvedParentViewData: function () { return null; },
				resolvedDashboardData: resolveDashboardData
			},
			data: {

			}
		});
	};

	//#endregion

	//#region << Resolve Function >>

	////////////////////////

	resolveDashboardData.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams', '$timeout', 'ngToast', '$location', 'resolvedEntityList'];
	function resolveDashboardData($q, $log, webvellaCoreService, $state, $stateParams, $timeout, ngToast, $location, resolvedEntityList) {
		var defer = $q.defer();
		//function successCallback(response) {
		//	defer.resolve(response.object);
		//}
		//function errorCallback(response) {
		//	defer.reject(response.message);
		//}
		//var searchParams = $location.search();

		//webvellaCoreService.getRecordsByListMeta(list, $stateParams.entityName, $stateParams.page, null, searchParams, successCallback, errorCallback);

		defer.resolve(null);
		return defer.promise;
	}

	//#endregion

	//#region << Controller /////////////////////////////// >>
	controller.$inject = ['$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaCoreService',
        'resolvedDashboardData', '$timeout', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce','resolvedAreas'];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService,
        resolvedDashboardData, $timeout, resolvedCurrentUser, $sessionStorage, $location, $window, $sce,resolvedAreas) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		ngCtrl.validation = {};
		ngCtrl.validation.hasError = false;
		ngCtrl.validation.errorMessage = "";
		ngCtrl.canSortList = false;
		//#endregion

		//#region << Set Page meta >>
		ngCtrl.pageTitle = "My Dashboard | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion


	}
	//#endregion

})();
