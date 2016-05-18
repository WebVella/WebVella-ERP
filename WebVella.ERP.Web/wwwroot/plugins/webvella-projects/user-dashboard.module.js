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
				resolvedDashboardActivities: resolveDashboardActivities,
				resolvedDashboardOwnedTasksModified:resolveDashboardOwnedTasksModified,
				resolvedDashboardOwnedBugsModified:resolveDashboardOwnedBugsModified
			},
			data: {

			}
		});
	};

	//#endregion

	//#region << Resolve Function >>

	////////////////////////
	resolveDashboardActivities.$inject = ['$q', '$log', '$state', '$stateParams', '$timeout', 'ngToast', '$location', 'webvellaProjectsService'];
	function resolveDashboardActivities($q, $log, $state, $stateParams, $timeout, ngToast, $location, webvellaProjectsService) {
		var defer = $q.defer();
		
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaProjectsService.getActivitiesList(1,"all",successCallback, errorCallback);
		return defer.promise;
	}

	////////////////////////
	resolveDashboardOwnedTasksModified.$inject = ['$q', '$log', '$state', '$stateParams', '$timeout', 'ngToast', '$location', 'webvellaProjectsService'];
	function resolveDashboardOwnedTasksModified($q, $log, $state, $stateParams, $timeout, ngToast, $location, webvellaProjectsService) {
		var defer = $q.defer();
		
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaProjectsService.getOwnerLastModifiedTasks(1,successCallback, errorCallback);
		return defer.promise;
	}

	////////////////////////
	resolveDashboardOwnedBugsModified.$inject = ['$q', '$log', '$state', '$stateParams', '$timeout', 'ngToast', '$location', 'webvellaProjectsService'];
	function resolveDashboardOwnedBugsModified($q, $log, $state, $stateParams, $timeout, ngToast, $location, webvellaProjectsService) {
		var defer = $q.defer();
		
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaProjectsService.getOwnerLastModifiedBugs(1,successCallback, errorCallback);
		return defer.promise;
	}

	//#endregion
 
	
	//#region << Controller /////////////////////////////// >>
	controller.$inject = ['$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaCoreService', 'webvellaProjectsService',
        'resolvedDashboardActivities', '$timeout', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce','resolvedAreas','ngToast',
		'resolvedDashboardOwnedTasksModified','resolvedDashboardOwnedBugsModified',];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService, webvellaProjectsService,
        resolvedDashboardActivities, $timeout, resolvedCurrentUser, $sessionStorage, $location, $window, $sce,resolvedAreas,ngToast,
		resolvedDashboardOwnedTasksModified,resolvedDashboardOwnedBugsModified) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		ngCtrl.loading = {};
		//#endregion

		//#region << Set Page meta >>
		ngCtrl.pageTitle = "My Dashboard | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion

		//#region << Activities >>
		var nextActivityPage = 2;
		ngCtrl.currentActivitiesLabel = "all";
		var initialActivitiesList = resolvedDashboardActivities;
		ngCtrl.activities = initialActivitiesList;
		ngCtrl.showMoreActivitiesButton = true;
		if(ngCtrl.activities.length < 10){
			ngCtrl.showMoreActivitiesButton = false;	
		}

		function loadActivitiesSuccessAppend(response){
			nextActivityPage++;
			ngCtrl.activities = ngCtrl.activities.concat(response.object);
			ngCtrl.loading.activities = false;
			if(response.object.length < 10){
				ngCtrl.showMoreActivitiesButton = false;				
			}
			else{
				ngCtrl.showMoreActivitiesButton = true;
			}
		}


		function loadActivitiesSuccessReload(response){
			nextActivityPage++;
			ngCtrl.activities = response.object;
			ngCtrl.loading.activities = false;
			if(response.object.length < 10){
				ngCtrl.showMoreActivitiesButton = false;				
			}
			else{
				ngCtrl.showMoreActivitiesButton = true;
			}
		}

		function loadActivitiesError(response){
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});		
			ngCtrl.loading.activities = false;
		}

		ngCtrl.loadMoreActivities = function(reload){
			ngCtrl.loading.activities = true;
			if(reload){
				webvellaProjectsService.getActivitiesList(1,ngCtrl.currentActivitiesLabel,loadActivitiesSuccessReload, loadActivitiesError);
			}
			else{
				webvellaProjectsService.getActivitiesList(nextActivityPage,ngCtrl.currentActivitiesLabel,loadActivitiesSuccessAppend, loadActivitiesError);
			}
		}

		ngCtrl.activityLabelChanged = function(){
			 ngCtrl.loadMoreActivities(true);
		}


		//#endregion
 
		//#region << Tasks >>
		var nextOwnedTaskPage = 2;
		var initialOwnedTasksList = resolvedDashboardOwnedTasksModified;
		ngCtrl.ownedTasks = initialOwnedTasksList;
		ngCtrl.showMoreOwnedTasksButton = true;
		if(ngCtrl.ownedTasks.length < 5){
			ngCtrl.showMoreOwnedTasksButton = false;	
		}

		function loadOwnedTasksSuccess(response){
			nextOwnedTaskPage++;
			ngCtrl.ownedTasks = ngCtrl.ownedTasks.concat(response.object);
			ngCtrl.loading.ownedTasks = false;
			if(response.object.length < 5){
				ngCtrl.showMoreOwnedTasksButton = false;				
			}
			else{
				ngCtrl.showMoreOwnedTasksButton = true;
			}
		}


		function loadOwnedTasksError(response){
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});		
			ngCtrl.loading.ownedTasks = false;
		}

		ngCtrl.loadMoreOwnedTasks = function(){
			ngCtrl.loading.ownedTasks = true;
			webvellaProjectsService.getOwnerLastModifiedTasks(nextActivityPage,loadOwnedTasksSuccess, loadOwnedTasksError);
		}

		//#endregion

		//#region << Bugs >>
		var nextOwnedBugPage = 2;
		var initialOwnedBugsList = resolvedDashboardOwnedBugsModified;
		ngCtrl.ownedBugs = initialOwnedBugsList;
		ngCtrl.showMoreOwnedBugsButton = true;
		if(ngCtrl.ownedBugs.length < 5){
			ngCtrl.showMoreOwnedBugsButton = false;	
		}

		function loadOwnedBugsSuccess(response){
			nextOwnedBugPage++;
			ngCtrl.ownedBugs = ngCtrl.ownedBugs.concat(response.object);
			ngCtrl.loading.ownedBugs = false;
			if(response.object.length < 5){
				ngCtrl.showMoreOwnedBugsButton = false;				
			}
			else{
				ngCtrl.showMoreOwnedBugsButton = true;
			}
		}


		function loadOwnedBugsError(response){
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});		
			ngCtrl.loading.ownedBugs = false;
		}

		ngCtrl.loadMoreOwnedBugs = function(){
			ngCtrl.loading.ownedBugs = true;
			webvellaProjectsService.getOwnerLastModifiedBugs(nextActivityPage,loadOwnedBugsSuccess, loadOwnedBugsError);
		}

		//#endregion


		//#region << Render >>
		ngCtrl.show = {};
		ngCtrl.showActivityInfo = function(activity){
			if(ngCtrl.show[activity.id]){
				ngCtrl.show[activity.id] = !ngCtrl.show[activity.id];
			}
			else {
				ngCtrl.show[activity.id] = true;
			}
		}

		ngCtrl.getTimeHumanizedString = function(date){
			return moment(date).fromNow();
		}

		//#endregion


	}
	//#endregion

})();
