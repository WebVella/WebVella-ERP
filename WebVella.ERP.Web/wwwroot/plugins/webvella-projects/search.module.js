(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .config(config)
        .controller('WebVellaProjectsSearchController', controller);

	//#region << Configuration /////////////////////////////////// >>
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider
		//general list in an area
		.state('webvella-projects-search', {
			parent: 'webvella-area-base',
			url: '/search?query',
			params: {
				query: { value: "", squash: true }
			},
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
					controller: 'WebVellaProjectsSearchController',
					templateUrl: '/plugins/webvella-projects/search.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedCurrentViewData: function () { return null; },
				resolvedParentViewData: function () { return null; },
				resolvedSearchData: resolveSearchData
			},
			data: {

			}
		});
	};

	//#endregion

	//#region << Resolve Function >>

	////////////////////////
	resolveSearchData.$inject = ['$q', '$log', '$state', '$stateParams', '$timeout', 'ngToast', '$location', 'webvellaProjectsService'];
	function resolveSearchData($q, $log, $state, $stateParams, $timeout, ngToast, $location, webvellaProjectsService) {
		var defer = $q.defer();
		
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaProjectsService.getSearchResults($stateParams.query,successCallback, errorCallback);
		return defer.promise;
	}

	

	//#endregion
 
	
	//#region << Controller /////////////////////////////// >>
	controller.$inject = ['$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaCoreService', 'webvellaProjectsService',
        '$timeout', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce','resolvedAreas','ngToast','resolvedSearchData',];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService, webvellaProjectsService,
        $timeout, resolvedCurrentUser, $sessionStorage, $location, $window, $sce,resolvedAreas,ngToast, resolvedSearchData) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		ngCtrl.loading = {};
		//#endregion

		//#region << Set Page meta >>
		ngCtrl.pageTitle = "Search | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion
		ngCtrl.results = resolvedSearchData;
		ngCtrl.query = $stateParams.query;

		ngCtrl.submitSearchSuccessCallback = function(response){
			$location.search("query", ngCtrl.query);
			ngCtrl.results = response.object;
			$window.scrollTo(0, 0);
		}
		ngCtrl.submitSearchErrorCallback = function(response){
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});			
		}

		ngCtrl.submitSearch = function(){
			webvellaProjectsService.getSearchResults(ngCtrl.query,ngCtrl.submitSearchSuccessCallback, ngCtrl.submitSearchErrorCallback);
		}
	}
	//#endregion

})();
