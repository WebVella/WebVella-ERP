/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreaEntityRecordsController', controller);


	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-entity-records', {
			parent: 'webvella-areas-base',
			url: '/:areaName/:entityName/:listName/:filter/:page',
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
					controller: 'WebVellaAreaEntityRecordsController',
					templateUrl: '/plugins/webvella-areas/entity-records.view.html',
					controllerAs: 'contentData'
				}
			},
			resolve: {
				resolvedListRecords: resolveListRecords,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedCurrentArea: resolveCurrentArea,
				resolvedEntityRelationsList: resolveEntityRelationsList,
				resolvedCurrentUser: resolveCurrentUser
			},
			data: {

			}
		});
	};


	//#region << Run //////////////////////////////////////
	run.$inject = ['$log'];

	/* @ngInject */
	function run($log) {
		$log.debug('webvellaAreas>entities> BEGIN module.run');

		$log.debug('webvellaAreas>entities> END module.run');
	};

	//#endregion

	//#region << Resolve Function >>
	resolveListRecords.$inject = ['$q', '$log', 'webvellaAreasService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveListRecords($q, $log, webvellaAreasService, $state, $stateParams) {
		$log.debug('webvellaDesktop>browse> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		webvellaAreasService.getListRecords($stateParams.listName, $stateParams.entityName, $stateParams.filter, $stateParams.page, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>browse> END state.resolved');
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $state, $stateParams) {
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved');
		return defer.promise;
	}


	resolveCurrentArea.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentArea($q, $log, webvellaAdminService, $state, $stateParams) {
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		webvellaAdminService.getAreaByName($stateParams.areaName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved');
		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveEntityRelationsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					$state.go("webvella-root-not-found");
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		webvellaAdminService.getRelationsList(successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved');
		return defer.promise;
	}

	resolveCurrentUser.$inject = ['$q', '$log', 'webvellaAdminService', 'webvellaRootService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentUser($q, $log, webvellaAdminService, webvellaRootService, $state, $stateParams) {
		$log.debug('webvellaDesktop>browse> BEGIN state.resolved');
		// Initialize
		var defer = $q.defer();
		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.resolve(response.object);
		}

		var currentUser = webvellaRootService.getCurrentUser();

		webvellaAdminService.getUserById(currentUser.userId, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>browse> END state.resolved');
		return defer.promise;
	}


	//#endregion


	// Controller ///////////////////////////////
	controller.$inject = ['$filter', '$log', '$modal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords', 'resolvedCurrentEntityMeta', 'resolvedCurrentArea',
		'resolvedEntityRelationsList', 'resolvedCurrentUser'];

	/* @ngInject */
	function controller($filter, $log, $modal, $rootScope, $state, $stateParams, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords, resolvedCurrentEntityMeta, resolvedCurrentArea,
		resolvedEntityRelationsList, resolvedCurrentUser) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec');
		/* jshint validthis:true */
		var contentData = this;
		contentData.records = angular.copy(resolvedListRecords.data);
		contentData.recordsMeta = angular.copy(resolvedListRecords.meta);
		contentData.relationsMeta = resolvedEntityRelationsList;

		//#region << Set Environment >>
		contentData.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(contentData.pageTitle);
		contentData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
		contentData.stateParams = $stateParams;
		webvellaRootService.setBodyColorClass(contentData.currentArea.color);

		//Get the current meta
		contentData.entity = angular.copy(resolvedCurrentEntityMeta);
		contentData.area = angular.copy(resolvedCurrentArea.data[0]);
		contentData.area.subscriptions = angular.fromJson(contentData.area.subscriptions);
		contentData.areaEntitySubscription = {};
		for (var i = 0; i < contentData.area.subscriptions.length; i++) {
			if (contentData.area.subscriptions[i].name === contentData.entity.name) {
				contentData.areaEntitySubscription = contentData.area.subscriptions[i];
				break;
			}
		}


		//Select default details view
		contentData.selectedView = {};
		for (var j = 0; j < contentData.entity.recordViews.length; j++) {
			if (contentData.entity.recordViews[j].name === contentData.areaEntitySubscription.view.name) {
				contentData.selectedView = contentData.entity.recordViews[j];
				break;
			}
		}
		contentData.currentPage = parseInt($stateParams.page);
		//Select the current list view details
		contentData.currentListView = {};
		for (var i = 0; i < contentData.entity.recordLists.length; i++) {
			if (contentData.entity.recordLists[i].name === $stateParams.listName) {
				contentData.currentListView = contentData.entity.recordLists[i];
			}
		}

		//#endregion

		//#region << Logic >> //////////////////////////////////////

		contentData.goDesktopBrowse = function () {
			webvellaRootService.GoToState($state, "webvella-desktop-browse", {});
		}

		contentData.selectPage = function (page) {
			var params = {
				areaName: $stateParams.areaName,
				entityName: $stateParams.entityName,
				listName: $stateParams.listName,
				filter: $stateParams.filter,
				page: page
			};
			webvellaRootService.GoToState($state, $state.current.name, params);
		}

		contentData.currentUserRoles = angular.copy(resolvedCurrentUser).data[0].$user_role;

		contentData.currentUserHasReadPermission = function (column) {
			var result = false;
			if (!column.meta.enableSecurity || column.meta.permissions == null) {
				return true;
			}
			for (var i = 0; i < contentData.currentUserRoles.length; i++) {
				for (var k = 0; k < column.meta.permissions.canRead.length; k++) {
					if (column.meta.permissions.canRead[k] == contentData.currentUserRoles[i].id) {
						result = true;
					}
				}
			}
			return result;
		}

		//#endregion

		//#region << Columns render>> //////////////////////////////////////
		//1.Auto increment
		contentData.getAutoIncrementString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else if (field.meta.displayFormat) {
				return field.meta.displayFormat.replace("{0}", fieldValue);
			}
			else {
				return fieldValue;
			}
		}
		//2.Checkbox
		contentData.getCheckboxString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "true";
			}
			else {
				return "false";
			}
		}
		//3.Currency
		contentData.getCurrencyString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else if (field.meta.currency != null && field.meta.currency !== {} && field.meta.currency.symbol) {
				if (field.meta.currency.symbolPlacement === 1) {
					return field.meta.currency.symbol + " " + fieldValue;
				}
				else {
					return fieldValue + " " + field.meta.currency.symbol;
				}
			}
			else {
				return fieldValue;
			}
		}
		//4.Date
		contentData.getDateString = function (record, field) {
			var fieldValue = record[field.dataName];
			return moment(fieldValue).format("DD MMMM YYYY");
		}
		//5.Datetime
		contentData.getDateTimeString = function (record, field) {
			var fieldValue = record[field.dataName];
			return moment(fieldValue).format("DD MMMM YYYY HH:mm");
		}
		//6.Email
		contentData.getEmailString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				//There is a problem in Angular when having in href -> the href is not rendered
				//return "<a href='mailto:" + fieldValue + "' data-rel='external'>" + fieldValue + "</a>";
				return fieldValue;
			}
			else {
				return "";
			}
		}
		//7.File
		contentData.getFileString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "<a href='" + fieldValue + "' taget='_blank' class='link-icon'>view file</a>";
			}
			else {
				return "";
			}
		}
		//8.Html
		contentData.getHtmlString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return fieldValue;
			}
			else {
				return "";
			}
		}
		//9.Image
		contentData.getImageString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "<img src='" + fieldValue + "' class='table-image'/>";
			}
			else {
				return "";
			}
		}
		//11.Multiselect
		contentData.getMultiselectString = function (record, field) {
			var fieldValueArray = record[field.dataName];
			var generatedStringArray = [];
			if (fieldValueArray.length === 0) {
				return "";
			}
			else {
				for (var i = 0; i < fieldValueArray.length; i++) {
					var selected = $filter('filter')(field.meta.options, { key: fieldValueArray[i] });
					generatedStringArray.push((fieldValueArray[i] && selected.length) ? selected[0].value : 'empty');
				}
				return generatedStringArray.join(', ');

			}

		}
		//14.Percent
		contentData.getPercentString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return fieldValue * 100 + "%";
			}
		}
		//15.Phone
		contentData.getPhoneString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				return phoneUtils.formatInternational(fieldValue);
			}
		}
		//17.Dropdown
		contentData.getDropdownString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (!fieldValue) {
				return "";
			}
			else {
				var selected = $filter('filter')(field.meta.options, { key: fieldValue });
				return (fieldValue && selected.length) ? selected[0].value : 'empty';
			}

		}
		//18.Url
		contentData.getUrlString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (fieldValue) {
				return "<a href='" + fieldValue + "' target='_blank'>" + fieldValue + "</a>";
			}
			else {
				return "";
			}
		}
		//#endregion

		//#region << Modals >> ////////////////////////////////////

		//#endregion

		$log.debug('webvellaAreas>entities> END controller.exec');
	}


})();
