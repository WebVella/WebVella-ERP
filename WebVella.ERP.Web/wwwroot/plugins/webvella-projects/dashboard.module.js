(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .config(config)
        .controller('WebVellaProjectsDashboardController', controller);

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
					controller: 'WebVellaProjectsDashboardController',
					templateUrl: '/plugins/webvella-projects/dashboard.view.html',
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
        'resolveDashboardData', '$timeout', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce'];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService,
        resolveDashboardData, $timeout, resolvedCurrentUser, $sessionStorage, $location, $window, $sce) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		ngCtrl.validation = {};
		ngCtrl.validation.hasError = false;
		ngCtrl.validation.errorMessage = "";
		ngCtrl.canSortList = false;
		//#endregion

	}
	//#endregion

})();
