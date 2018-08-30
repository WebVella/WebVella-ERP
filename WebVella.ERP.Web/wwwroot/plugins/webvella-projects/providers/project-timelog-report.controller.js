// IMPORTANT: You must always have at least webvellaListActionService defined or the page will not load
// The methods inside it are optional 
// For usage in action items, the service is bound to the controller with ngCtrl.actionService. So if 
// what to use a test method from this service in an action you need to call like 'ng-click=""ngCtrl.actionService.test()""'
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Objects accessible through the ngCtrl:
// ngCtrl.list.data => the records' data array
// ngCtrl.list.meta => the records' list meta
// ngCtrl.entity    => the current entity's meta
// ngCtrl.entityRelations => list of all relations of the entity
// ngCtrl.areas		=> the current areas in the site and their meta, attached entities and etc.
// ngCtrl.currentUser => the current user
// ngCtrl.$sessionStorage => copy of the session local storage service
// ngCtrl.stateParams => all state parameters

// IMPORTANT: all data is two way bound, which means it will be watched by angular and any changes propagated. If you want to get a copy of one of the objects, without the binding
// use the var copyObject = fastCopy(originalObject); . This will break the binding.

(function () {
	'use strict';
	angular
    .module('webvellaAreas')
	.config(config)
	.controller('ProjectTimeLogReportController', ProjectTimeLogReportController);

	//#region << Configuration >> ///////////////////////////////////
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider.state('webvella-project-timelog-report', {
			parent: 'webvella-area-base',
			url: '/report',
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
					controller: 'ProjectTimeLogReportController',
					templateUrl: '/plugins/webvella-projects/templates/project-timelog-report.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
			},
			data: {

			}
		});
	};
	//#endregion

	ProjectTimeLogReportController.$inject = ['$stateParams', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter', 'webvellaCoreService', 
				'$uibModal', '$rootScope', 'resolvedAreas', 'pageTitle','webvellaProjectsService'];
	function ProjectTimeLogReportController($stateParams, $http, wvAppConstants, $timeout, ngToast, $filter, webvellaCoreService, 
				$uibModal, $rootScope, resolvedAreas, pageTitle,webvellaProjectsService) {

		var ngCtrl = this;
		//#region <<Set Page meta>>
		ngCtrl.pageTitle = "Project report | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion
		ngCtrl.projects = [];
		ngCtrl.totalBillable = 0;
		ngCtrl.totalNotBillable = 0;
		ngCtrl.loading = false;
		ngCtrl.months = [
		{
			key: 1,
			value: "January"
		},
		{
			key: 2,
			value: "February"
		},
		{
			key: 3,
			value: "March"
		},
		{
			key: 4,
			value: "April"
		},
		{
			key: 5,
			value: "May"
		},
		{
			key: 6,
			value: "June"
		},
		{
			key: 7,
			value: "July"
		},
		{
			key: 8,
			value: "August"
		},
		{
			key: 9,
			value: "September"
		},
		{
			key: 10,
			value: "October"
		},
		{
			key: 11,
			value: "November"
		},
		{
			key: 12,
			value: "December"
		},

		];

		ngCtrl.years = [
			{
				key: 2016,
				value: "2016"
			},
			{
				key: 2017,
				value: "2017"
			},
			{
				key: 2018,
				value: "2018"
			},
			{
				key: 2019,
				value: "2019"
			}
		];
		ngCtrl.selectedMonth = moment().subtract(1, 'months').month() + 1;	//moment returns from 0 to 11
		ngCtrl.selectedYear = moment().year();


		ngCtrl.generateReport = function(){
			ngCtrl.loading = true;
			ngCtrl.projects = [];
			webvellaProjectsService.getProjectTimelogReport(ngCtrl.selectedMonth,ngCtrl.selectedYear,ngCtrl.successCallback,ngCtrl.errorCallback);
		}

		ngCtrl.successCallback = function(response){
			ngCtrl.loading = false;
			ngCtrl.projects = response.object.projects;
			ngCtrl.totalBillable = response.object.billable;
			ngCtrl.totalNotBillable = response.object.not_billable;
		}

		ngCtrl.errorCallback = function(response){
			ngCtrl.loading = false;
			alert(response.message);
		}
	}

})();

