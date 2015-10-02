/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreaEntityRecordsController', controller)
		.controller('SetFiltersModalController', SetFiltersModalController);



	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-entity-records', {
			parent: 'webvella-areas-base',
			url: '/:areaName/:entityName/:listName/:filter/:page?search',
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
				checkedAccessPermission: checkAccessPermission,
				resolvedListRecords: resolveListRecords,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRelationsList: resolveEntityRelationsList
			},
			data: {

			}
		});
	};


	//#region << Resolve Function >>

	checkAccessPermission.$inject = ['$q', '$log', 'webvellaRootService', '$stateParams', 'resolvedSitemap', 'resolvedCurrentUser', 'ngToast'];
	/* @ngInject */
	function checkAccessPermission($q, $log, webvellaRootService, $stateParams, resolvedSitemap, resolvedCurrentUser, ngToast) {
		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		var defer = $q.defer();
		var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">' + $stateParams.areaName + '</span> area';
		var accessPermission = webvellaRootService.applyAreaAccessPolicy($stateParams.areaName, resolvedCurrentUser, resolvedSitemap);
		if (accessPermission) {
			defer.resolve();
		}
		else {
			
			ngToast.create({
				className: 'error',
				content: messageContent
			});
			defer.reject("No access");
		}

		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	////////////////////////
	resolveListRecords.$inject = ['$q', '$log', 'webvellaAreasService', '$state', '$stateParams', '$timeout', 'ngToast'];
	/* @ngInject */
	function resolveListRecords($q, $log, webvellaAreasService, $state, $stateParams, $timeout, ngToast) {
		$log.debug('webvellaAreas>entity-records> BEGIN entity list resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();
		var listRecordsObject = {};
		listRecordsObject.filterRecords = null;
		var existingRemovedDelta = 0;

		// Process get list success
		function successCallback(response) {
			listRecordsObject = response.object;
			if ($stateParams.filter == "all") {

				defer.resolve(listRecordsObject);
			}
			else {
				webvellaAreasService.getListFilter($stateParams.filter, filterSuccessCallback, errorCallback);
			}
		}

		// Process get filter records
		function filterSuccessCallback(response) {
			listRecordsObject.filterRecords = response.object;
			if (listRecordsObject.filterRecords != null && listRecordsObject.filterRecords.data.length > 0) {
				//Maintain and apply list filters
				//In this operation as it is a system maintenance  
				//get and check filter records and list meta
				//if in the list there are removed fields or now unsearchable fields, which fields are part from the current filter, we need to update the current filter
				//and make new data request.
				var searchableFieldsChanged = false;
				var arrayOfFilterRecordsToBeRemoved = [];
				for (var k = 0; k < listRecordsObject.filterRecords.data.length; k++) {
					var fieldFound = null;
					for (var z = 0; z < listRecordsObject.meta.columns.length; z++) {
						if (listRecordsObject.meta.columns[z].meta.name == listRecordsObject.filterRecords.data[k].field_name) {
							fieldFound = listRecordsObject.meta.columns[z].meta;
						}
					}
					if (fieldFound == null || !fieldFound.searchable) {
						searchableFieldsChanged = true;
						arrayOfFilterRecordsToBeRemoved.push(listRecordsObject.filterRecords.data[k].id);
					}
				}

				if (searchableFieldsChanged) {
					//Call service for removing the filter records
					existingRemovedDelta = listRecordsObject.filterRecords.data.length - arrayOfFilterRecordsToBeRemoved.length;
					webvellaAreasService.deleteSelectedFilterRecords($stateParams.filter, arrayOfFilterRecordsToBeRemoved, updateFilterSuccessCallback, errorCallback)
				}
				else {
					defer.resolve(listRecordsObject);
				}

			}
			else {
				$timeout(function () {
					$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: "all", page: 1, search: $stateParams.search }, { reload: true });
				}, 0);
			}
		}

		// Process filter update
		function updateFilterSuccessCallback(response) {
			//Check if there are any records left in the filter to decide where to redirect - to all or to filter
			if (existingRemovedDelta > 0) {
				//The filter still has active records
				defer.resolve(listRecordsObject);
				ngToast.create({
					className: 'warning',
					content: '<span class="go-orange">Filter changed </span> ' + 'Administrative changes altered this filter'
				});
			}
			else {
				//The filter has no more records
				$timeout(function () {
					$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: "all", page: 1, search: $stateParams.search }, { reload: true });
					ngToast.create({
						className: 'warning',
						content: '<span class="go-orange">Filter removed </span> ' + 'Administrative changes removed this filter'
					});
				}, 0);
			}
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		if (!$stateParams.search) {
			$stateParams.search = null;
		}

		webvellaAreasService.getListRecords($stateParams.listName, $stateParams.entityName, $stateParams.filter, $stateParams.page, $stateParams.search, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAreas>entity-records> END entity list resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $state, $stateParams) {
		$log.debug('webvellaAdmin>entity-records> BEGIN entity list resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveEntityRelationsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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
				defer.reject(response.message);
			}
		}

		webvellaAdminService.getRelationsList(successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	//#endregion


	// Controller ///////////////////////////////
	controller.$inject = ['$filter', '$log', '$modal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords', 'resolvedCurrentEntityMeta', 
		'resolvedEntityRelationsList', 'resolvedCurrentUser', 'ngToast'];

	/* @ngInject */
	function controller($filter, $log, $modal, $rootScope, $state, $stateParams, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords, resolvedCurrentEntityMeta,
		resolvedEntityRelationsList, resolvedCurrentUser, ngToast) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
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

		contentData.area = {};
		for (var i = 0; i < resolvedSitemap.data.length; i++) {
			if (resolvedSitemap.data[i].name == $stateParams.areaName) {
				contentData.area = resolvedSitemap.data[i];
			}
		}


		contentData.area.subscriptions = angular.fromJson(contentData.area.subscriptions);
		contentData.areaEntitySubscription = {};
		for (var i = 0; i < contentData.area.subscriptions.length; i++) {
			if (contentData.area.subscriptions[i].name === contentData.entity.name) {
				contentData.areaEntitySubscription = contentData.area.subscriptions[i];
				break;
			}
		}


		//Slugify function
		function convertToSlug(Text) {
			return Text
				.toLowerCase()
				.replace(/ /g, '-')
				.replace(/[^\w-]+/g, '')
			;
		}

		contentData.generateViewName = function (record) {
			//default is the selected view in the area
			var result = angular.copy(contentData.selectedView.name);

			if (contentData.recordsMeta.viewNameOverride && contentData.recordsMeta.viewNameOverride.length > 0) {
				var arrayOfTemplateKeys = contentData.recordsMeta.viewNameOverride.match(/\{(\w+)\}/g);
				var resultStringStorage = angular.copy(contentData.recordsMeta.viewNameOverride);

				for (var i = 0; i < arrayOfTemplateKeys.length; i++) {
					if ( arrayOfTemplateKeys[i] === "{areaName}" || arrayOfTemplateKeys[i] === "{entityName}" || arrayOfTemplateKeys[i] === "{filter}" || arrayOfTemplateKeys[i] === "{page}" || arrayOfTemplateKeys[i] === "{searchQuery}") {
						switch (arrayOfTemplateKeys[i]) {
							case "{areaName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
							case "{entityName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
							case "{filter}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
							case "{page}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
							case "{searchQuery}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
						}
					}
					else {
						//Extract the dataName from string by removing the leading and the closing {}
						var dataName = arrayOfTemplateKeys[i].replace('{', '').replace('}', '');
						//Check template has corresponding list data value
						if (record[dataName] != undefined) {
							//YES -> check the value of this dataName and substitute with it in the string, even if it is null (toString)
							resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug(record[dataName].toString()));
						}
						else {
							//NO -> substitute the template key with the dataName only, as no value could be extracted
							resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug(dataName));
						}
					}

				}
				result = resultStringStorage;
			}

			return result;
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

		//#region << Init filters >>
		contentData.filterChangeRequested = false;
		if (resolvedListRecords.filterRecords == undefined) {
			resolvedListRecords.filterRecords = null;
		}
		else {
			contentData.filterRecords = angular.copy(resolvedListRecords.filterRecords);
		}

		if (contentData.filterRecords == null || contentData.filterRecords.data == null) {
			contentData.filterRecords = {};
			contentData.filterRecords.data = [];
		}
		else {
			for (var i = 0; i < contentData.filterRecords.data.length; i++) {
				contentData.filterRecords.data[i].helper = angular.fromJson(contentData.filterRecords.data[i].helper);
				var helperDataAray = contentData.filterRecords.data[i].helper.data;
				for (var k = 0; k < helperDataAray.length; k++) {
					helperDataAray[k].label = decodeURIComponent(helperDataAray[k].label);
				}
			}
		}

		//Order the filters the same way the columns in the list are ordered
		var temporaryFilterArray = [];
		if (contentData.filterRecords.data.length > 0) {
			for (var k = 0; k < resolvedListRecords.meta.columns.length; k++) {
				for (var j = 0; j < contentData.filterRecords.data.length; j++) {
					if (resolvedListRecords.meta.columns[k].meta.name == contentData.filterRecords.data[j].field_name) {
						temporaryFilterArray.push(contentData.filterRecords.data[j]);
					}
				}
			}
			contentData.filterRecords.data = angular.copy(temporaryFilterArray);
		}

		//#endregion

		//#region << Search >>
		contentData.defaultSearchField = null;
		for (var k = 0; k < contentData.currentListView.columns.length; k++) {
			if (contentData.currentListView.columns[k].type == "field") {
				contentData.defaultSearchField = contentData.currentListView.columns[k];
				break;
			}
		}
		if (contentData.defaultSearchField != null) {
			contentData.searchQueryPlaceholder = "" + contentData.defaultSearchField.meta.label;
		}


		contentData.searchQuery = null;
		if ($stateParams.search) {
			contentData.searchQuery = $stateParams.search;
		}
		contentData.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				contentData.submitSearchQuery();
			}
		}
		contentData.submitSearchQuery = function () {
			$timeout(function () {
				$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: $stateParams.filter,page:1, search: contentData.searchQuery }, { reload: true });
			}, 1);

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

		contentData.currentUser = angular.copy(resolvedCurrentUser);

		contentData.currentUserRoles = contentData.currentUser.roles;

		contentData.currentUserHasReadPermission = function (column) {
			var result = false;
			if (!column.meta.enableSecurity || column.meta.permissions == null) {
				return true;
			}
			for (var i = 0; i < contentData.currentUserRoles.length; i++) {
				for (var k = 0; k < column.meta.permissions.canRead.length; k++) {
					if (column.meta.permissions.canRead[k] == contentData.currentUserRoles[i]) {
						result = true;
					}
				}
			}
			return result;
		}

		contentData.filterChangeRequested = false;
		//TODO: this object does not support relations. To support them the properties should not be only the field_name but plus relation_name
		contentData.filtersPendingRemoval = {};
		//enlist all filters as pending false
		for (var k = 0; k < contentData.filterRecords.data.length; k++) {
			contentData.filtersPendingRemoval[contentData.filterRecords.data[k].field_name] = false;
		}

		contentData.requestFilterRemoval = function (filterRecord) {
			if (contentData.filtersPendingRemoval[filterRecord.field_name]) {
				//this filter was already scheduled for removal. Second click on it - probably the user wants to cancel the remove
				contentData.filtersPendingRemoval[filterRecord.field_name] = false;
				var noMoreFiltersPendingRemoval = true;
				for (var property in contentData.filtersPendingRemoval) {
					if (contentData.filtersPendingRemoval[property]) {
						noMoreFiltersPendingRemoval = false;
					}
				}
				if (noMoreFiltersPendingRemoval) {
					contentData.filterChangeRequested = false;
				}
			}
			else {
				contentData.filtersPendingRemoval[filterRecord.field_name] = true;
				contentData.filterChangeRequested = true;
			}
		}

		contentData.applyFilterChange = function () {
			//Process the action
			function successCallback(response) {
				$timeout(function () {
					$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: contentData.filterId, page: 1, search: $stateParams.search }, { reload: true });
					ngToast.create({
						className: 'info',
						content: '<span class="go-blue"><i class="fa fa-refresh fa-spin"></i> Wait! </span> ' + 'Applying filter ...'
					});
				}, 0);
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message
				});
			}

			//Check if all filters are enabled or all are disabled
			contentData.noFiltersArePending = true;
			contentData.allFiltersArePending = true;
			for (var property in contentData.filtersPendingRemoval) {
				if (contentData.filtersPendingRemoval[property]) {
					//pending removal
					contentData.noFiltersArePending = false;
				}
				else {
					//not pending removal
					contentData.allFiltersArePending = false;
				}
			}

			//Case 1: All filters are pending removal - should redirect to all
			if (contentData.allFiltersArePending) {
				$timeout(function () {
					//TODO: Decide whether we should delete the filter
					$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: "all", page: 1, search: $stateParams.search }, { reload: true });
				}, 0);
			}
				//Case 2: All filters are enabled - the button should be already disabled so do nothing
			else if (contentData.noFiltersArePending) {

			}
			else {
				//Filter change is needed 
				contentData.filterId = moment().format('YYYYMMDDHHmmssSSS');
				var filterArray = [];
				for (var j = 0; j < contentData.filterRecords.data.length; j++) {
					if (!contentData.filtersPendingRemoval[contentData.filterRecords.data[j].field_name]) {
						var activeFilter = angular.copy(contentData.filterRecords.data[j]);
						activeFilter.id = null;
						activeFilter.filter_id = contentData.filterId;
						activeFilter.helper = angular.toJson(activeFilter.helper);
						//remove the special $$ properties
						activeFilter = angular.fromJson(angular.toJson(activeFilter));
						filterArray.push(activeFilter)
					}
				}
				webvellaAreasService.createListFilter(filterArray, $stateParams.entityName, $stateParams.listName, successCallback, errorCallback);
			}
		}

		//#endregion

		//#region << Columns render>> //////////////////////////////////////
		//1.Auto increment
		contentData.getAutoIncrementString = function (record, field) {
			
			var fieldValue = record[field.dataName];
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
			if (fieldValue) {
				return "<span class='label label-success'>true</span>";
			}
			else {
				return "<span class='label label-danger'>false</span>";
			}
		}
		//3.Currency
		contentData.getCurrencyString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
			return moment(fieldValue).format("DD MMMM YYYY");
		}
		//5.Datetime
		contentData.getDateTimeString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
			return moment(fieldValue).format("DD MMMM YYYY HH:mm");
		}
		//6.Email
		contentData.getEmailString = function (record, field) {
			var fieldValue = record[field.dataName];
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
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
			if (field.type == "fieldFromRelation") {
				fieldValue = record[field.dataName][0];
			}
			if (fieldValue) {
				return "<a href='" + fieldValue + "' target='_blank'>" + fieldValue + "</a>";
			}
			else {
				return "";
			}
		}
		//#endregion

		//#region << Modals >> ////////////////////////////////////

		//filter modal
		contentData.openSetFiltersModal = function () {
			var modalInstance = $modal.open({
				animation: false,
				templateUrl: 'setFiltersModalContent.html',
				controller: 'SetFiltersModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () {
						return contentData;
					},
					currentFilterRecords: function () {
						//TODO: apply getting the current filter records
						return null;
					}
				}
			});

		}
		//#endregion

		$log.debug('webvellaAreas>entities> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

	//// Modal Controllers
	SetFiltersModalController.$inject = ['$modalInstance', '$log', 'webvellaAreasService', 'ngToast', '$timeout', '$state', '$location', 'contentData', '$stateParams','$scope'];
	/* @ngInject */
	function SetFiltersModalController($modalInstance, $log, webvellaAreasService, ngToast, $timeout, $state, $location, contentData, $stateParams,$scope) {
		$log.debug('webvellaAreas>records>SetFiltersModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		var popupData = this;
		popupData.contentData = angular.copy(contentData);
		popupData.currentUserRoles = angular.copy(contentData.currentUserRoles);
		popupData.currentUser = angular.copy(contentData.currentUser);
		popupData.filterRecordsList = angular.copy(contentData.filterRecords.data);

		//#region << Match type >>
		popupData.matchTypesDictionary = {};
		popupData.selectedMatchType = {};
		var exactMatch = {
			key: "exact",
			value: "Exact match"
		};
		var exactMatchAND = {
			key: "exact_and",
			value: "Exactly match all"
		};
		var exactMatchOR = {
			key: "exact_or",
			value: "Exact match at least one"
		};
		var rangeMatch = {
			key: "range",
			value: "Range"
		}
		var periodMatch = {
			key: "period",
			value: "Period"
		};
		var regexMatch = {
			key: "regex",
			value: "Regular expression"
		};

		//auto increment type 1
		popupData.matchTypesDictionary["1"] = [];
		popupData.matchTypesDictionary["1"].push(exactMatch);
		popupData.matchTypesDictionary["1"].push(rangeMatch);
		popupData.matchTypesDictionary["1"].push(regexMatch);

		//Checkbox type 2
		popupData.matchTypesDictionary["2"] = [];
		popupData.matchTypesDictionary["2"].push(exactMatch);

		//Currency type 3
		popupData.matchTypesDictionary["3"] = [];
		popupData.matchTypesDictionary["3"].push(exactMatch);
		popupData.matchTypesDictionary["3"].push(rangeMatch);
		popupData.matchTypesDictionary["3"].push(regexMatch);

		//Date type 4
		popupData.matchTypesDictionary["4"] = [];
		popupData.matchTypesDictionary["4"].push(exactMatch);
		popupData.matchTypesDictionary["4"].push(rangeMatch);
		popupData.matchTypesDictionary["4"].push(periodMatch);
		popupData.matchTypesDictionary["4"].push(regexMatch);

		//Date type 5
		popupData.matchTypesDictionary["5"] = [];
		popupData.matchTypesDictionary["5"].push(exactMatch);
		popupData.matchTypesDictionary["5"].push(rangeMatch);
		popupData.matchTypesDictionary["5"].push(periodMatch);
		popupData.matchTypesDictionary["5"].push(regexMatch);

		//Email type 6
		popupData.matchTypesDictionary["6"] = [];
		popupData.matchTypesDictionary["6"].push(exactMatch);
		popupData.matchTypesDictionary["6"].push(regexMatch);

		//File 7
		popupData.matchTypesDictionary["7"] = [];
		popupData.matchTypesDictionary["7"].push(exactMatch);
		popupData.matchTypesDictionary["7"].push(regexMatch);

		//HTML 7
		popupData.matchTypesDictionary["8"] = [];
		popupData.matchTypesDictionary["8"].push(exactMatch);
		popupData.matchTypesDictionary["8"].push(regexMatch);

		//Image 9
		popupData.matchTypesDictionary["9"] = [];
		popupData.matchTypesDictionary["9"].push(exactMatch);
		popupData.matchTypesDictionary["9"].push(regexMatch);

		//HTML 10
		popupData.matchTypesDictionary["10"] = [];
		popupData.matchTypesDictionary["10"].push(exactMatch);
		popupData.matchTypesDictionary["10"].push(regexMatch);

		//Multiselect 11
		popupData.matchTypesDictionary["11"] = [];
		popupData.matchTypesDictionary["11"].push(exactMatchAND);
		popupData.matchTypesDictionary["11"].push(exactMatchOR);
		popupData.matchTypesDictionary["11"].push(regexMatch);

		//Number 12
		popupData.matchTypesDictionary["12"] = [];
		popupData.matchTypesDictionary["12"].push(exactMatch);
		popupData.matchTypesDictionary["12"].push(rangeMatch);
		popupData.matchTypesDictionary["12"].push(regexMatch);

		//Password 13 - skip cannot be searchable

		//Percent 14
		popupData.matchTypesDictionary["14"] = [];
		popupData.matchTypesDictionary["14"].push(exactMatch);
		popupData.matchTypesDictionary["14"].push(rangeMatch);
		popupData.matchTypesDictionary["14"].push(regexMatch);

		//Phone 15
		popupData.matchTypesDictionary["15"] = [];
		popupData.matchTypesDictionary["15"].push(exactMatch);
		popupData.matchTypesDictionary["15"].push(regexMatch);

		//Guid 16
		popupData.matchTypesDictionary["16"] = [];
		popupData.matchTypesDictionary["16"].push(exactMatch);
		popupData.matchTypesDictionary["16"].push(regexMatch);

		//Dropdown 17
		popupData.matchTypesDictionary["17"] = [];
		popupData.matchTypesDictionary["17"].push(exactMatch);
		popupData.matchTypesDictionary["17"].push(regexMatch);

		//Text 18
		popupData.matchTypesDictionary["18"] = [];
		popupData.matchTypesDictionary["18"].push(exactMatch);
		popupData.matchTypesDictionary["18"].push(regexMatch);

		//Url 19
		popupData.matchTypesDictionary["19"] = [];
		popupData.matchTypesDictionary["19"].push(exactMatch);
		popupData.matchTypesDictionary["19"].push(regexMatch);

		popupData.periodDictionary = [
		{
			key: "hour",
			value: "Past hour"
		},
		{
			key: "day",
			value: "Past 24 hours"
		},
		{
			key: "week",
			value: "Past week"
		},
		{
			key: "month",
			value: "Past month"
		},
		{
			key: "year",
			value: "Past year"
		}
		];
		popupData.selectedPeriod = popupData.periodDictionary[3];


		popupData.isDisabledDropdown = function (array) {
			if (array.length < 2) {
				return true;
			}
			else {
				return false;
			}

		}

		//#endregion 


		//#region << generate searchable fields list
		//1. Get the list meta and find who are the searchable fields
		popupData.filterColumns = [];
		for (var m = 0; m < popupData.contentData.currentListView.columns.length; m++) {
			//is this field visible for the currentUser
			var userHasReadPermissionForField = false;
			if (!popupData.contentData.currentListView.columns[m].meta.enableSecurity) {
				userHasReadPermissionForField = true;
			}
			else {
				for (var r = 0; r < popupData.currentUserRoles.length; r++) {
					for (var p = 0; p < popupData.contentData.currentListView.columns[m].meta.permissions.canRead.length; p++) {
						if (popupData.currentUserRoles[r] == popupData.contentData.currentListView.columns[m].meta.permissions.canRead[p]) {
							userHasReadPermissionForField = true;
							break;
						}
					}
				}
			}
			switch (popupData.contentData.currentListView.columns[m].type) {
				case "field":
					if (popupData.contentData.currentListView.columns[m].meta.searchable && (!popupData.contentData.currentListView.columns[m].meta.enableSecurity || (popupData.contentData.currentListView.columns[m].meta.enableSecurity && userHasReadPermissionForField))) {
						var filterObject = {};
						filterObject = popupData.contentData.currentListView.columns[m];
						filterObject.match_type = popupData.matchTypesDictionary[filterObject.meta.fieldType.toString()][0].key;
						filterObject.data = [];
						for (var j = 0; j < popupData.filterRecordsList.length; j++) {
							if (popupData.filterRecordsList[j].field_name == filterObject.meta.name) {
								filterObject.data = angular.fromJson(popupData.filterRecordsList[j].values);
								for (var dd = 0; dd < filterObject.data.length; dd++) {
									filterObject.data[dd] = decodeURIComponent(filterObject.data[dd]);
								}
								filterObject.match_type = popupData.filterRecordsList[j].match_type;
							}

						}
						filterObject.loading = false;
						popupData.filterColumns.push(filterObject);
					}
					break;
				default:
					break;
			}
		}
		// Rules:
		// Simple fields -> depending on the field type
		// 1:1 (field is target) -> lookuplist
		// 1:1 (field is origin) -> textbox
		// 1:N (field is target) -> lookuplist
		// 1:N (field is origin) -> textbox
		// N:N for each of the relations we should generate a tab as it is a different field -> multiselect list

		//The helper object that helps show the filters to the user should include
		//Applied filter field name and field Id. For the selected values - record id(could be the selected option key) value
		//If applied filters are for related fields - related entity name, entity id, field name, field id, record id, value

		//If the showed on screen (not popup filters are changed) the popup button should become an apply button

		popupData.tabLoading = false;
		popupData.tabSelected = function () {
			popupData.tabLoading = true;
			$timeout(function () {
				popupData.tabLoading = false;
			}, 500);
		}
		popupData.getTabExtraCssClass = function (column) {
			if (column.data.length > 0) {
				return "active-filter";
			}
			return "";
		}

		popupData.fieldTypes = [
					{
						"id": 1,
						"name": "AutoNumberField",
						"label": "Auto increment number",
						"description": "If you need a automatically incremented number with each new record, this is the field you need. You can customize the display format also."
					},
					{
						"id": 2,
						"name": "CheckboxField",
						"label": "Checkbox",
						"description": "The simple on and off switch. This field allows you to get a True (checked) or False (unchecked) value."
					},
					{
						"id": 3,
						"name": "CurrencyField",
						"label": "Currency",
						"description": "A currency amount can be entered and will be represented in a suitable formatted way"
					},
					{
						"id": 4,
						"name": "DateField",
						"label": "Date",
						"description": "A data pickup field, that can be later converting according to a provided pattern"
					},
					{
						"id": 5,
						"name": "DateTimeField",
						"label": "Date & Time",
						"description": "A date and time can be picked up and later presented according to a provided pattern"
					},
					{
						"id": 6,
						"name": "EmailField",
						"label": "Email",
						"description": "An email can be entered by the user, which will be validated and presented accordingly"
					},
					{
						"id": 7,
						"name": "FileField",
						"label": "File",
						"description": "File upload field. Files will be stored within the system."
					},
					{
						"id": 8,
						"name": "HtmlField",
						"label": "HTML",
						"description": "Provides the ability of entering and presenting an HTML code. Supports multiple input languages."
					},
					{
						"id": 9,
						"name": "ImageField",
						"label": "Image",
						"description": "Image upload field. Images will be stored within the system"
					},
					{
						"id": 10,
						"name": "MultiLineTextField",
						"label": "Textarea",
						"description": "A textarea for plain text input. Will be cleaned and stripped from any web unsafe content"
					},
					{
						"id": 11,
						"name": "MultiSelectField",
						"label": "Multiple select",
						"description": "Multiple values can be selected from a provided list"
					},
					{
						"id": 12,
						"name": "NumberField",
						"label": "Number",
						"description": "Only numbers are allowed. Leading zeros will be stripped."
					},
					{
						"id": 13,
						"name": "PasswordField",
						"label": "Password",
						"description": "This field is suitable for submitting passwords or other data that needs to stay encrypted in the database"
					},
					{
						"id": 14,
						"name": "PercentField",
						"label": "Percent",
						"description": "This will automatically present any number you enter as a percent value"
					},
					{
						"id": 15,
						"name": "PhoneField",
						"label": "Phone",
						"description": "Will allow only valid phone numbers to be submitted"
					},
					{
						"id": 16,
						"name": "GuidField",
						"label": "Identifier GUID",
						"description": "Very important field for any entity to entity relation and required by it"
					},
					{
						"id": 17,
						"name": "SelectField",
						"label": "Dropdown",
						"description": "One value can be selected from a provided list"
					},
					{
						"id": 18,
						"name": "TextField",
						"label": "Text",
						"description": "A simple text field. One of the most needed field nevertheless"
					},
					{
						"id": 19,
						"name": "UrlField",
						"label": "URL",
						"description": "This field will validate local and static URLs. Will present them accordingly"
					}
		];

		popupData.getFieldTypeName = function (column) {
			var fieldLabel = " ";
			for (var l = 0; l < popupData.fieldTypes.length; l++) {
				if (popupData.fieldTypes[l].id == column.meta.fieldType) {
					fieldLabel = popupData.fieldTypes[l].label;
					break;
				}
			}
			return fieldLabel;
		}

		popupData.clearFilter = function (column) {
			column.data = [];
		}

		popupData.clearAllFilters = function () {
			for (var j = 0; j < popupData.filterColumns.length; j++) {
				popupData.filterColumns[j].data = [];
			}
		}

		popupData.addMultiSelectOption = function (column) {
			column.data.push("");
		}

		popupData.removeMultiSelectOption = function (index,column) {
			column.data.splice(index,1);
		}


		//#endregion


		popupData.calendars = {};
		popupData.openCalendar = function (event, name) {
			popupData.calendars[name] = true;
		}

		popupData.dateMatchTypeChanged = function (column) {
			if (column.match_type == "period") {
				column.data[0] = "month";
			}
			else if (column.data[0] == "month") {
				column.data = [];
			}
		}

		popupData.filterId = null;

		popupData.ok = function () {
			//Create the new filter (update currently is not developed as the filter should act as query params search - new options change the link
			// Generate the filter array
			var filterArray = [];
			popupData.filterId = moment().format('YYYYMMDDHHmmssSSS');
			for (var j = 0; j < popupData.filterColumns.length; j++) {
				if (popupData.filterColumns[j].data != null && popupData.filterColumns[j].data.length > 0) {
					var filterRecord = {};
					filterRecord.filter_id = popupData.filterId;
					filterRecord.field_name = popupData.filterColumns[j].meta.name;
					filterRecord.match_type = popupData.filterColumns[j].match_type;
					filterRecord.entity_name = $stateParams.entityName;
					filterRecord.list_name = $stateParams.listName;
					filterRecord.values = [];
					//Generate relation name
					switch (popupData.filterColumns[j].type) {
						default:
							//field
							filterRecord.relation_name = null;
							break;
					}

					//Generate helper 
					var helperObject = {};
					helperObject.label = popupData.filterColumns[j].meta.label;;
					helperObject.name = popupData.filterColumns[j].meta.name;
					helperObject.type = popupData.filterColumns[j].type;
					helperObject.fieldType = popupData.filterColumns[j].meta.fieldType;
					helperObject.data = [];
					for (var m = 0; m < popupData.filterColumns[j].data.length; m++) {
						var valueRecord = {};
						switch (popupData.filterColumns[j].meta.fieldType) {
							case 1: // Auto increment
								valueRecord.value = encodeURIComponent(angular.copy(popupData.filterColumns[j].data[m]));
								if (popupData.filterColumns[j].match_type == "range") {
									var firstValue = "any";
									if (popupData.filterColumns[j].data[0]) {
										firstValue = popupData.filterColumns[j].data[0];
									}
									var secondValue = "any";
									if (popupData.filterColumns[j].data[1]) {
										secondValue = popupData.filterColumns[j].data[1];
									}
									valueRecord.label = "<span class='go-gray'>from </span>" + firstValue + " <span class='go-gray'>to</span> " + secondValue;
								}
								else {
									// Exact
									valueRecord.label = valueRecord.value;
								}
								break;
							case 2: //Checkbox
								valueRecord.value = encodeURIComponent(angular.copy(popupData.filterColumns[j].data[m]));
								valueRecord.label = valueRecord.value;
								break;
							case 3: // Currency
								valueRecord.value = encodeURIComponent(angular.copy(popupData.filterColumns[j].data[m]));
								if (popupData.filterColumns[j].match_type == "range") {
									var firstValue = "any";
									if (popupData.filterColumns[j].data[0]) {
										firstValue = popupData.filterColumns[j].data[0];
									}
									var secondValue = "any";
									if (popupData.filterColumns[j].data[1]) {
										secondValue = popupData.filterColumns[j].data[1];
									}
									valueRecord.label = "<span class='go-gray'>from " + popupData.filterColumns[j].meta.currency.code + " </span>" + firstValue + " <span class='go-gray'>to " + popupData.filterColumns[j].meta.currency.code + "</span> " + secondValue;
								}
								else {
									// Exact
									valueRecord.label = popupData.filterColumns[j].meta.currency.code + " " + valueRecord.value;
								}
								break;
							case 4: //Date

								if (popupData.filterColumns[j].match_type == "range") {
									valueRecord.value = moment(popupData.filterColumns[j].data[m]).utc().toISOString();
									var firstValue = "any";
									if (popupData.filterColumns[j].data[0]) {
										firstValue = moment(popupData.filterColumns[j].data[0]).format("DD MMM YYYY")
									}
									var secondValue = "any";
									if (popupData.filterColumns[j].data[1]) {
										secondValue = moment(popupData.filterColumns[j].data[1]).format("DD MMM YYYY")
									}

									valueRecord.label = "<span class='go-gray'>from </span>" + firstValue + " <span class='go-gray'>to </span> " + secondValue;
								}
								else if (popupData.filterColumns[j].match_type == "period") {
									for (var p = 0; p < popupData.periodDictionary.length; p++) {
										if (popupData.filterColumns[j].data[m] == popupData.periodDictionary[p].key) {
											valueRecord.value = popupData.filterColumns[j].data[m];
											valueRecord.label = popupData.periodDictionary[p].value;
										}
									}
								}
								else if (popupData.filterColumns[j].match_type == "regex") {
									valueRecord.value = "regex";
									valueRecord.label = "regex";
								}
								else {
									// Exact
									valueRecord.value = moment(popupData.filterColumns[j].data[m]).utc().toISOString();
									valueRecord.label = moment(popupData.filterColumns[j].data[m]).format("DD MMM YYYY");
								}
								break;
							case 5: //Datetime
								if (popupData.filterColumns[j].match_type == "range") {
									valueRecord.value = moment(popupData.filterColumns[j].data[m]).utc().toISOString();
									var firstValue = "any";
									if (popupData.filterColumns[j].data[0]) {
										firstValue = moment(popupData.filterColumns[j].data[0]).format("DD MMM YYYY HH:mm")
									}
									var secondValue = "any";
									if (popupData.filterColumns[j].data[1]) {
										secondValue = moment(popupData.filterColumns[j].data[1]).format("DD MMM YYYY HH:mm")
									}
									
									valueRecord.label = "<span class='go-gray'>from </span>" + firstValue + " <span class='go-gray'>to </span> " + secondValue;
								}
								else if (popupData.filterColumns[j].match_type == "period") {
									for (var p = 0; p < popupData.periodDictionary.length; p++) {
										if (popupData.filterColumns[j].data[m] == popupData.periodDictionary[p].key) {
											valueRecord.value = popupData.filterColumns[j].data[m];
											valueRecord.label = popupData.periodDictionary[p].value;
										}
									}
								}
								else if (popupData.filterColumns[j].match_type == "regex") {
									valueRecord.value = "regex";
									valueRecord.label = "regex";
								}
								else {
									// Exact
									valueRecord.value = moment(popupData.filterColumns[j].data[m]).utc().toISOString();
									valueRecord.label = moment(popupData.filterColumns[j].data[m]).format("DD MMM YYYY HH:mm");
								}
								break;
							case 12: // Number
								valueRecord.value = encodeURIComponent(angular.copy(popupData.filterColumns[j].data[m]));
								if (popupData.filterColumns[j].match_type == "range") {
									var firstValue = "any";
									if (popupData.filterColumns[j].data[0]) {
										firstValue = popupData.filterColumns[j].data[0];
									}
									var secondValue = "any";
									if (popupData.filterColumns[j].data[1]) {
										secondValue = popupData.filterColumns[j].data[1];
									}
									valueRecord.label = "<span class='go-gray'>from </span>" + firstValue + " <span class='go-gray'>to </span> " + secondValue;
								}
								else {
									// Exact
									valueRecord.label = valueRecord.value;
								}
								break;

							case 14: // Percent
								valueRecord.value = encodeURIComponent(angular.copy(popupData.filterColumns[j].data[m])); //In the help object we will not need to convert to less than 1 decimal
								if (popupData.filterColumns[j].match_type == "range") {
									var firstValue = "any";
									if (popupData.filterColumns[j].data[0]) {
										firstValue = popupData.filterColumns[j].data[0];
									}
									var secondValue = "any";
									if (popupData.filterColumns[j].data[1]) {
										secondValue = popupData.filterColumns[j].data[1];
									}
									valueRecord.label = "<span class='go-gray'>from </span>" + firstValue + "% <span class='go-gray'>to </span> " + secondValue + "%";
								}
								else {
									// Exact
									valueRecord.label = valueRecord.value + "%";
								}
								break;

							default: // Email, file, Html, Image, Textarea, Multiselect
								valueRecord.value = encodeURIComponent(angular.copy(popupData.filterColumns[j].data[m]));
								// Exact
								valueRecord.label = valueRecord.value;
								break;
						}
						helperObject.data.push(valueRecord);
					}
					filterRecord.helper = angular.toJson(helperObject);

					//Generate values
					for (var k = 0; k < popupData.filterColumns[j].data.length; k++) {
						if (popupData.filterColumns[j].type == "field") {
							//fix the undefined value for ranges when only one part of the range is selected
							if (popupData.filterColumns[j].match_type == "range" && k == 0) { // We initialize on the first pass
								if (!popupData.filterColumns[j].data[0]) {
									popupData.filterColumns[j].data[0] = "";
								}
								if (!popupData.filterColumns[j].data[1]) {
									popupData.filterColumns[j].data[1] = "";
								}
							}

							switch (popupData.filterColumns[j].meta.fieldType) {
								case 4: //Date - this needs to be done to ensure that in the database is store the ISO and UTC data value
									if (popupData.filterColumns[j].match_type == "exact" || popupData.filterColumns[j].match_type == "range") {
										var utcIsoDate = moment(popupData.filterColumns[j].data[k]).utc().toISOString();
										filterRecord.values.push(encodeURIComponent(utcIsoDate));
									}
									else {
										filterRecord.values.push(encodeURIComponent(angular.copy(popupData.filterColumns[j].data[k])));
									}
									break;
								case 5: //Datetime - this needs to be done to ensure that in the database is store the ISO and UTC data value
									if (popupData.filterColumns[j].match_type == "exact" || popupData.filterColumns[j].match_type == "range") {
										var utcIsoDate = moment(popupData.filterColumns[j].data[k]).utc().toISOString();
										filterRecord.values.push(encodeURIComponent(utcIsoDate));
									}
									else {
										filterRecord.values.push(encodeURIComponent(angular.copy(popupData.filterColumns[j].data[k])));
									}
									break;
								case 14: //Percent
										//need to convert to decimal 0 <= val <= 100 Divide by 100
										//Hack for proper javascript division
										$scope.Math = window.Math;
										var helpNumber = 10000000;
										var multipliedValue = $scope.Math.round(popupData.filterColumns[j].data[k] * helpNumber);
										var numberToSubmit = multipliedValue / (100 * helpNumber);
										filterRecord.values.push(encodeURIComponent(numberToSubmit));
									break;
								default:
									filterRecord.values.push(encodeURIComponent(angular.copy(popupData.filterColumns[j].data[k])));
									break;
							}
						}
					}
					filterRecord.id = null;
					filterRecord.created_by = popupData.currentUser.id;
					filterRecord.last_modified_by = popupData.currentUser.id;
					filterRecord.values = angular.toJson(filterRecord.values);
					filterArray.push(filterRecord);
				}
			}

			//Process the action
			function successCallback(response) {
				$timeout(function () {
					$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: popupData.filterId, page: 1, search: $stateParams.search }, { reload: true });
					ngToast.create({
						className: 'info',
						content: '<span class="go-blue"><i class="fa fa-refresh fa-spin"></i> Wait! </span> ' + 'Applying filter ...'
					});
					$modalInstance.close('success');
				}, 0);
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message
				});
			}

			//Case 1: filter array empty
			if (filterArray.length == 0) {
				if ($stateParams.filter != "all") {
					$timeout(function () {
						//TODO: Decide whether we should delete the filter
						$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, filter: "all", page: 1, search: $stateParams.search }, { reload: true });
						$modalInstance.dismiss('cancel');
					}, 0);
				}
				else {
					$modalInstance.dismiss('cancel');
				}
			}
			else {
				//Case 2: filter array the same - means all filter fields - relation name - match types - values are matching with the preloaded filterRecords
				var filterArrayIsTheSame = true;
				if (popupData.filterRecordsList.length != filterArray.length) {
					filterArrayIsTheSame = false
				}
				else {
					for (var f = 0; f < filterArray.length; f++) {
						for (var s = 0; s < popupData.filterRecordsList.length; s++) {
							var arrayRow = filterArray[f];
							var filterRecord = popupData.filterRecordsList[s];
							if (arrayRow.field_name == filterRecord.field_name && arrayRow.relation_name == filterRecord.relation_name && (arrayRow.match_type != filterRecord.match_type || arrayRow.values != filterRecord.values)) {
								filterArrayIsTheSame = false;
							}
							else {
								//Handle the case when the filters are switch - the 1 old filter is removed and 1 new is added
							}
						}
					}
				}
				if (filterArrayIsTheSame) {
					$modalInstance.dismiss('cancel');
				}
					//Case 3: filter array changed or new - needs create
				else {
					webvellaAreasService.createListFilter(filterArray, $stateParams.entityName, $stateParams.listName, successCallback, errorCallback);
				}
			}
		};

		popupData.cancel = function () {
			$modalInstance.dismiss('cancel');
		};

		$log.debug('webvellaAreas>records>SetFiltersModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


})();
