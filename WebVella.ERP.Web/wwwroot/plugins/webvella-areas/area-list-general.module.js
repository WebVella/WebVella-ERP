(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .config(config)
        .controller('WebVellaAreaEntityRecordsController', controller)
		.controller('exportModalController', exportModalController)
		.controller('importModalController', importModalController);

	//#region << Configuration /////////////////////////////////// >>
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider
		//general list in an area
		.state('webvella-area-list-general', {
			parent: 'webvella-area-base',
			url: '/list-general/:listName/:page',
			params: {
				page: { value: "1", squash: true }
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
					controller: 'WebVellaAreaEntityRecordsController',
					templateUrl: '/plugins/webvella-areas/area-list-general.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				loadDependency: loadDependency,
				resolvedCurrentViewData: function () { return null; },
				resolvedParentViewData: function () { return null; },
				resolvedRecordListData: resolveRecordListData
			},
			data: {

			}
		})
		//general list in an a view with the view sidebar
		.state('webvella-area-list-general-in-view', {
			parent: 'webvella-area-base',
			url: '/:viewType/sb/:parentViewName/:recordId/list-general/:listName/:page', //Data name will be used for the list
			params: {
				page: { value: "1", squash: true }
			},
			views: {
				"topnavView": {
					controller: 'WebVellaAreasTopnavController',
					templateUrl: '/plugins/webvella-areas/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAreasRecordViewSidebarController',
					templateUrl: '/plugins/webvella-areas/view-record-sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAreaEntityRecordsController',
					templateUrl: '/plugins/webvella-areas/area-list-general.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				loadDependency: loadDependency,
				resolvedCurrentViewData: function () { return null },	//for the sidebar to render
				resolvedParentViewData: resolveParentViewData,
				resolvedRecordListData: resolveRecordListDataFromView
			},
			data: {

			}
		});
	};

	//#endregion

	//#region << Resolve Function >>

	////////////////////////
	loadDependency.$inject = ['$ocLazyLoad', '$q', '$http', '$stateParams', 'resolvedCurrentEntityMeta', 'wvAppConstants', '$timeout', 'resolvedEntityRelationsList'];
	function loadDependency($ocLazyLoad, $q, $http, $stateParams, resolvedCurrentEntityMeta, wvAppConstants, $timeout, resolvedEntityRelationsList) {
		var lazyDeferred = $q.defer();
		var listServiceJavascriptPath = "";
		//Case 1. List is in area
		if ($stateParams.listName.indexOf('$') == -1) {
			listServiceJavascriptPath = wvAppConstants.apiBaseUrl + 'meta/entity/' + $stateParams.entityName + '/list/' + $stateParams.listName + '/service.js?v=' + resolvedCurrentEntityMeta.hash;
		}
		else {
			//Case 2. List is in a view and the listName is a dataName	
			var dataNameArray = $stateParams.listName.split('$');
			if (dataNameArray.length < 3 || dataNameArray.length > 4) {
				lazyDeferred.reject("The list dataName is not correct");
			}
			else if (dataNameArray.length == 3) {
				//e.g. "$list$lookup"
				var listRealName = dataNameArray[2];
				listServiceJavascriptPath = wvAppConstants.apiBaseUrl + 'meta/entity/' + $stateParams.entityName + '/list/' + listRealName + '/service.js?v=' + resolvedCurrentEntityMeta.hash;	//do not have the hash of the real entity here
			}
			else {
				//e.g. "$list$project_1_n_ticket$general"
				//extract the real list name
				var listRealName = dataNameArray[3];
				//find the other entity in the relation so we can include it in the request
				var listRealEntity = null;
				resolvedEntityRelationsList.forEach(function (relation) {
					if (relation.name == dataNameArray[2]) {
						if (relation.originEntityName == $stateParams.entityName) {
							listRealEntity = relation.targetEntityName;
						}
						else if (relation.targetEntityName == $stateParams.entityName) {
							listRealEntity = relation.originEntityName;
						}
					}
				});
				if (listRealEntity == null) {
					lazyDeferred.reject("Cannot find the list real entity name");
				}
				else {
					listServiceJavascriptPath = wvAppConstants.apiBaseUrl + 'meta/entity/' + listRealEntity + '/list/' + listRealName + '/service.js?v=' + moment().toISOString();	//do not have the hash of the real entity here
				}
			}

		}



		var loadFilesArray = [];
		loadFilesArray.push(listServiceJavascriptPath);

		return $ocLazyLoad.load({
			cache: false,
			files: loadFilesArray
		}).then(function () {
			return lazyDeferred.resolve("ready");
		});

	}

	resolveRecordListData.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams', '$timeout', 'ngToast', '$location', 'resolvedEntityList'];
	function resolveRecordListData($q, $log, webvellaCoreService, $state, $stateParams, $timeout, ngToast, $location, resolvedEntityList) {
		var defer = $q.defer();

		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		var searchParams = $location.search();

		var list = webvellaCoreService.getEntityRecordListFromEntitiesMetaList($stateParams.listName, $stateParams.entityName, resolvedEntityList);

		webvellaCoreService.getRecordsByListMeta(list, $stateParams.entityName, $stateParams.page, null, searchParams, successCallback, errorCallback);
		return defer.promise;
	}

	resolveParentViewData.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', 'resolvedCurrentEntityMeta', 'resolvedEntityList'];
	function resolveParentViewData($q, $log, webvellaCoreService, $stateParams, $state, $timeout, resolvedCurrentEntityMeta, resolvedEntityList) {

		// Initialize
		var defer = $q.defer();
		// Process
		function successCallback(response) {
			if (response.object === null) {
				alert("error in response!");
			}
			else if (response.object === null) {
				alert("The view with name: " + $stateParams.parentViewName + " does not exist");
			} else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				alert("error in response!");
			}
			else {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				defer.reject(response.message);
			}
		}

		var userHasReadEntityPermission = webvellaCoreService.userHasRecordPermissions(resolvedCurrentEntityMeta, "canRead");
		if (!userHasReadEntityPermission) {
			alert("you do not have permissions to view records from this entity!");
			defer.reject("you do not have permissions to view records from this entity");
		}

		var parentView = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);
		webvellaCoreService.getRecordByViewMeta($stateParams.recordId, parentView, $stateParams.entityName, null, successCallback, errorCallback);

		return defer.promise;
	}

	resolveRecordListDataFromView.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', 'resolvedParentViewData', 'resolvedEntityList', 'resolvedEntityRelationsList', 'ngToast'];
	function resolveRecordListDataFromView($q, $log, webvellaCoreService, $stateParams, $state, $timeout, resolvedParentViewData, resolvedEntityList, resolvedEntityRelationsList, ngToast) {
		//Temporary method will be replaced when the proper API is ready
		// Initialize
		var defer = $q.defer();
		var safeListNameAndEntity = webvellaCoreService.getSafeListNameAndEntityName($stateParams.listName, $stateParams.entityName, resolvedEntityRelationsList);
		var getListMeta = webvellaCoreService.getEntityRecordListFromEntitiesMetaList(safeListNameAndEntity.listName, safeListNameAndEntity.entityName, resolvedEntityList);
		if (getListMeta.dataSourceUrl != null && getListMeta.dataSourceUrl != "") {
			//This list has a dynamicSourceUrl defined
			webvellaCoreService.getRecordsByListMeta(getListMeta, safeListNameAndEntity.entityName, 1, $stateParams, null, successCallback, errorCallback);
		}
		else {
			//No dynamicSourceUrl defined
			var parentViewMeta = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);
			// Find list dataName
			var listData = null;
			parentViewMeta.sidebar.items.forEach(function (item) {
				if (item.dataName == $stateParams.listName) {
					listData = resolvedParentViewData[0][item.dataName];
				}
			});

			if (listData == null) {
				//list not found
				defer.reject("list not found");
			}
			else {
				defer.resolve(listData);
			}
		}

		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});
			defer.reject(response.message);
		}

		return defer.promise;
	}
	//#endregion

	//#region << Controller /////////////////////////////// >>
	controller.$inject = ['$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaCoreService', '$injector',
        'resolvedAreas', 'resolvedRecordListData', 'resolvedEntityList', 'resolvedCurrentEntityMeta', '$timeout', '$translate',
		'resolvedEntityRelationsList', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce', 'resolvedParentViewData'];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService, $injector,
        resolvedAreas, resolvedRecordListData, resolvedEntityList, resolvedCurrentEntityMeta, $timeout, $translate,
		resolvedEntityRelationsList, resolvedCurrentUser, $sessionStorage, $location, $window, $sce, resolvedParentViewData) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		ngCtrl.validation = {};
		ngCtrl.validation.hasError = false;
		ngCtrl.validation.errorMessage = "";
		ngCtrl.currentPage = parseInt($stateParams.page);
		ngCtrl.canSortList = false;
		//#endregion

		//#region << Init the list name >>
		var safeListNameAndEntityName = webvellaCoreService.getSafeListNameAndEntityName($stateParams.listName, $stateParams.entityName, resolvedEntityRelationsList)
		var listName = safeListNameAndEntityName.listName;
		var listEntityName = safeListNameAndEntityName.entityName;
		//#endregion

		//#region << Initialize main objects >>
		ngCtrl.list = {};
		ngCtrl.list.data = resolvedRecordListData;
		ngCtrl.list.meta = webvellaCoreService.getEntityRecordListFromEntitiesMetaList(listName, listEntityName, resolvedEntityList);
		ngCtrl.entityList = resolvedEntityList;
		ngCtrl.entity = resolvedCurrentEntityMeta;
		ngCtrl.entityRelations = resolvedEntityRelationsList;
		ngCtrl.areas = resolvedAreas.data;
		ngCtrl.currentUser = resolvedCurrentUser;
		ngCtrl.$sessionStorage = $sessionStorage;
		ngCtrl.stateParams = $stateParams;
		ngCtrl.parentView = {};
		ngCtrl.parentView.data = null;
		ngCtrl.parentView.meta = null;
		if (resolvedParentViewData != null) {
			ngCtrl.parentView.data = resolvedParentViewData;
			ngCtrl.parentView.meta = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);
		}

		//#endregion

		//#region << Set Page meta >>
		ngCtrl.pageTitle = ngCtrl.list.meta.label + " | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion

		//#region << Alternative general lists dropdown >>
		ngCtrl.generalLists = [];
		if ($stateParams.parentViewName == null) {
			ngCtrl.entity.recordLists.forEach(function (list) {
				if (list.type == "general") {
					ngCtrl.generalLists.push(list);
				}
			});
			ngCtrl.generalLists.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
		}
		//#endregion

		//#region << Column widths from CSV >>
		ngCtrl.columnWidths = [];
		var columnWidthsArray = [];
		if (ngCtrl.list.meta.columnWidthsCSV) {
			columnWidthsArray = ngCtrl.list.meta.columnWidthsCSV.split(',');
		}
		var visibleColumns = ngCtrl.list.meta.visibleColumnsCount;
		if (columnWidthsArray.length > 0) {
			for (var i = 0; i < visibleColumns; i++) {
				if (columnWidthsArray.length >= i + 1) {
					ngCtrl.columnWidths.push(columnWidthsArray[i]);
				}
				else {
					ngCtrl.columnWidths.push("auto");
				}
			}
		}
		else {
			//set all to auto
			for (var i = 0; i < visibleColumns; i++) {
				ngCtrl.columnWidths.push("auto");
			}
		}

		//#endregion

		//#region << List filter row >>
		ngCtrl.filterQuery = {};
		ngCtrl.listIsFiltered = false;
		ngCtrl.columnDictionary = {};
		ngCtrl.columnDataNamesArray = [];
		ngCtrl.queryParametersArray = [];
		ngCtrl.filterLoading = false;
		//Extract the available columns
		ngCtrl.list.meta.columns.forEach(function (column) {
			if (ngCtrl.columnDataNamesArray.indexOf(column.dataName) == -1) {
				ngCtrl.columnDataNamesArray.push(column.dataName);
			}
			ngCtrl.columnDictionary[column.dataName] = column;
		});
		//Extract available url query strings
		var queryObject = $location.search();
		for (var key in queryObject) {
			if (ngCtrl.queryParametersArray.indexOf(key) == -1) {
				ngCtrl.queryParametersArray.push(key);
			}
		}

		ngCtrl.columnDataNamesArray.forEach(function (dataName) {
			if (ngCtrl.queryParametersArray.indexOf(dataName) > -1) {
				ngCtrl.listIsFiltered = true;
				var columnObj = ngCtrl.columnDictionary[dataName];
				//some data validations and conversions	
				switch (columnObj.meta.fieldType) {
					case 4: //Date
						ngCtrl.filterQuery[dataName] = moment(queryObject[dataName]).format('D MMM YYYY');
						break;
					case 5: //Datetime
						ngCtrl.filterQuery[dataName] = moment(queryObject[dataName]).format('D MMM YYYY HH:mm');
						break;
					case 14:  //TODO if percent convert to > 1 %
						if (checkDecimal(queryObject[dataName])) {
							ngCtrl.filterQuery[dataName] = queryObject[dataName] * 100;
						}
						break;
					default:
						ngCtrl.filterQuery[dataName] = queryObject[dataName];
						break;

				}
			}
		});

		ngCtrl.clearQueryFilter = function () {
			ngCtrl.filterLoading = true;
			for (var activeFilter in ngCtrl.filterQuery) {
				$location.search(activeFilter, null);
			}
			var searchParams = $location.search();
			ngCtrl.filterQuery = {};
			ngCtrl.listIsFiltered = false;
			ngCtrl.show_filter = false;
			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, safeListNameAndEntityName.entityName, $stateParams.page, $stateParams, searchParams, ngCtrl.ReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);
		}

		ngCtrl.applyQueryFilter = function () {
			ngCtrl.filterLoading = true;
			var queryFieldsCount = 0;
			for (var filter in ngCtrl.filterQuery) {
				if(ngCtrl.filterQuery[filter]){
					queryFieldsCount ++;
					for (var i = 0; i < ngCtrl.list.meta.columns.length; i++) {
						if(ngCtrl.list.meta.columns[i].meta.name == filter){
							var selectedField = ngCtrl.list.meta.columns[i].meta;
							switch(selectedField.fieldType){
								case 4: //Date
									$location.search(filter, moment(ngCtrl.filterQuery[filter],'D MMM YYYYY').toISOString());
									break;
								case 5: //Datetime
									$location.search(filter, moment(ngCtrl.filterQuery[filter],'D MMM YYYYY HH:mm').toISOString());
									break;
								case 14: //Percent
									$location.search(filter, ngCtrl.filterQuery[filter] / 100);
									break;
								default:
									$location.search(filter, ngCtrl.filterQuery[filter]);
									break;
							}
						}
					}
				}
				else {
					$location.search(filter, null);
				}
			}
			//$window.location.reload();
			var searchParams = $location.search();
			if (queryFieldsCount > 0) {
				ngCtrl.listIsFiltered = true;
			}
			//Find the entity of the list. It could not be the current one as it could be listFromRelation case
			var listEntityName = safeListNameAndEntityName.entityName;

			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, listEntityName, 1, $stateParams, searchParams, ngCtrl.ReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);
		}

		ngCtrl.ReloadRecordsSuccessCallback = function (response) {
			$timeout(function(){
				ngCtrl.filterLoading = false;
			},300);
			ngCtrl.list.data = response.object;
		}

		ngCtrl.ReloadRecordsErrorCallback = function (response) {
			$timeout(function(){
				ngCtrl.filterLoading = false;
			},300);
			alert(response.message);
		}

		//#endregion

		//#region << Extract fields that are supported in the query to be filters>>
		ngCtrl.fieldsInQueryArray = webvellaCoreService.extractSupportedFilterFields(ngCtrl.list);
		ngCtrl.checkIfFieldSetInQuery = function (dataName) {
			if (ngCtrl.fieldsInQueryArray.fieldNames.indexOf(dataName) != -1) {
				return true;
			}
			else {
				return false;
			}
		}

		ngCtrl.allQueryComparisonList = [];
		//#region << Query Dictionary >>
		$translate(['QUERY_RULE_EQ_LABEL', 'QUERY_RULE_NOT_LABEL', 'QUERY_RULE_LT_LABEL', 'QUERY_RULE_LTE_LABEL',
					'QUERY_RULE_GT_LABEL', 'QUERY_RULE_GTE_LABEL', 'QUERY_RULE_CONTAINS_LABEL', 'QUERY_RULE_NOT_CONTAINS_LABEL',
					'QUERY_RULE_STARTSWITH_LABEL', 'QUERY_RULE_NOT_STARTSWITH_LABEL']).then(function (translations) {
						ngCtrl.allQueryComparisonList = [
							{
								key: "EQ",
								value: translations.QUERY_RULE_EQ_LABEL
							},
							{
								key: "NOT",
								value: translations.QUERY_RULE_NOT_LABEL
							},
							{
								key: "LT",
								value: translations.QUERY_RULE_LT_LABEL
							},
							{
								key: "LTE",
								value: translations.QUERY_RULE_LTE_LABEL
							},
							{
								key: "GT",
								value: translations.QUERY_RULE_GT_LABEL
							},
							{
								key: "GTE",
								value: translations.QUERY_RULE_GTE_LABEL
							},
							{
								key: "CONTAINS",
								value: translations.QUERY_RULE_CONTAINS_LABEL
							},
							{
								key: "NOTCONTAINS",
								value: translations.QUERY_RULE_NOT_CONTAINS_LABEL
							},
							{
								key: "STARTSWITH",
								value: translations.QUERY_RULE_STARTSWITH_LABEL
							},
							{
								key: "NOTSTARTSWITH",
								value: translations.QUERY_RULE_NOT_STARTSWITH_LABEL
							}
						];

					});
		//#endregion
		ngCtrl.getFilterInputPlaceholder = function (dataName) {
			var fieldIndex = ngCtrl.fieldsInQueryArray.fieldNames.indexOf(dataName);
			if (fieldIndex == -1) {
				return "";
			}
			else {
				var fieldQueryType = ngCtrl.fieldsInQueryArray.queryTypes[fieldIndex];
				for (var i = 0; i < ngCtrl.allQueryComparisonList.length; i++) {
					if (ngCtrl.allQueryComparisonList[i].key == fieldQueryType) {
						return ngCtrl.allQueryComparisonList[i].value;
					}
				}
				return "";
			}
		}
		//#endregion

		//#region << List sort >>
		//Check if the list has a sort rule with the needed data-link object for sorting through the url
		var listSortRules = fastCopy(ngCtrl.list.meta.sorts);
		for (var i = 0; i < listSortRules.length; i++) {
			if (listSortRules[i].fieldName != null && listSortRules[i].fieldName.trim().startsWith("{")) {
				var dataLinkObject = angular.fromJson(listSortRules[i].fieldName);
				if (dataLinkObject.name == "url_sort" && dataLinkObject.option == "sortBy" && dataLinkObject.settings.order == "sortOrder") {
					ngCtrl.canSortList = true;
				}
			}
		}

		//#endregion

		//#region << Logic >> 
		ngCtrl.PagingReloadRecordsSuccessCallback = function (response) {
			$timeout(function(){
				ngCtrl.filterLoading = false;
			},300);
			ngCtrl.list.data = response.object;
			if(ngCtrl.currentPage != 1){
				$location.search("page", ngCtrl.currentPage);
			}
			else {
				$location.search("page", null);
			}			
			$window.scrollTo(0, 0);
		}
		ngCtrl.selectPage = function (page) {
			var searchParams = $location.search();
			var listEntityName = safeListNameAndEntityName.entityName;
			ngCtrl.currentPage = page;
			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, listEntityName, page, $stateParams, searchParams, ngCtrl.PagingReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);
		}

		ngCtrl.currentUserRoles = ngCtrl.currentUser.roles;

		ngCtrl.currentUserHasReadPermission = function (column) {
			var result = false;
			if (!column.meta.enableSecurity || column.meta.permissions == null) {
				return true;
			}
			for (var i = 0; i < ngCtrl.currentUserRoles.length; i++) {
				for (var k = 0; k < column.meta.permissions.canRead.length; k++) {
					if (column.meta.permissions.canRead[k] == ngCtrl.currentUserRoles[i]) {
						result = true;
					}
				}
			}
			return result;
		}

		ngCtrl.userHasRecordPermissions = function (permissionsCsv) {
			//Get the entity of the current list or view
			var listEntity = webvellaCoreService.getEntityMetaFromEntityList(safeListNameAndEntityName.entityName,resolvedEntityList);
			return webvellaCoreService.userHasRecordPermissions(listEntity, permissionsCsv);
		}

		ngCtrl.saveStateParamsToSessionStorage = function () {
			ngCtrl.$sessionStorage["last-list-params"] = $stateParams;
		}

		//#region << Sort >>
		ngCtrl.sortObject = {}; // dataName = order // no property, "ascending" "descending"

		//Init already sorted params
		var preloadedSortBy = $location.search().sortBy;
		var preloadedSortOrder = $location.search().sortOrder;
		if (preloadedSortBy && preloadedSortOrder && (preloadedSortOrder == "ascending" || preloadedSortOrder == "descending")) {
			ngCtrl.sortObject[preloadedSortBy] = preloadedSortOrder;
		}
		ngCtrl.toggleSort = function (column) {
			if (ngCtrl.sortObject[column.dataName]) {
				switch (ngCtrl.sortObject[column.dataName]) {
					case "ascending":
						ngCtrl.sortObject[column.dataName] = "descending";
						$location.search("sortBy", column.dataName);
						$location.search("sortOrder", "descending");
						break;
					case "descending":
						delete ngCtrl.sortObject[column.dataName];
						$location.search("sortBy", null);
						$location.search("sortOrder", null);
						break;
				}
			}
			else {
				ngCtrl.sortObject = {};
				ngCtrl.sortObject[column.dataName] = "ascending";
				$location.search("sortBy", column.dataName);
				$location.search("sortOrder", "ascending");
			}

			var searchParams = $location.search();
			var listEntityName = safeListNameAndEntityName.entityName;
			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, listEntityName, 1, $stateParams, searchParams, ngCtrl.ReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);

		}

		//#endregion

		//#endregion

		//#region << Modals >>

		ngCtrl.exportModal = undefined;
		ngCtrl.openExportModal = function () {
			ngCtrl.exportModal = $uibModal.open({
				animation: false,
				templateUrl: 'exportModalContent.html',
				controller: 'exportModalController',
				controllerAs: "popupCtrl",
				//size: "lg",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					}
				}
			});
		}
		//Close the modal on state change
		$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
			if (ngCtrl.exportModal) {
				ngCtrl.exportModal.dismiss();
			}
		})

		ngCtrl.importModal = undefined;
		ngCtrl.openImportModal = function () {
			ngCtrl.importModal = $uibModal.open({
				animation: false,
				templateUrl: 'importModalContent.html',
				controller: 'importModalController',
				controllerAs: "popupCtrl",
				//size: "lg",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					}
				}
			});
		}
		//Close the modal on state change
		$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
			if (ngCtrl.importModal) {
				ngCtrl.importModal.dismiss();
			}
		})
		//#endregion

		//#region << Render >>
		ngCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;
		ngCtrl.getAutoIncrementString = function (column) {
			var returnObject = {};
			returnObject.prefix = null;
			returnObject.suffix = null;
			var keyIndex = column.meta.displayFormat.indexOf('{0}');
			if (keyIndex == 0) {
				return null;
			}
			else {
				returnObject.prefix = column.meta.displayFormat.slice(0, keyIndex);
				if (keyIndex + 3 < column.meta.displayFormat.length) {
					returnObject.suffix = column.meta.displayFormat.slice(keyIndex + 3, column.meta.displayFormat.length);
				}
				return returnObject;
			}
		}


		ngCtrl.showPageTitleAuxLabelSecondary = false;

		ngCtrl.generateHighlightString = function(){
			if(ngCtrl.parentView && ngCtrl.parentView.data){
				ngCtrl.showPageTitleAuxLabelSecondary = true;
				return webvellaCoreService.generateHighlightString(ngCtrl.parentView.meta,ngCtrl.parentView.data[0],ngCtrl.stateParams,"title");
			}
			else {
				return webvellaCoreService.generateHighlightString( ngCtrl.list.meta,null,ngCtrl.stateParams,"title");
			}
		}

		ngCtrl.generateAuxHighlightString = function(){
			if(ngCtrl.parentView && ngCtrl.parentView.data){
				return webvellaCoreService.generateHighlightString(ngCtrl.list.meta,ngCtrl.parentView.data[0],ngCtrl.stateParams,"label");
			}
			else {
				return webvellaCoreService.generateHighlightString( ngCtrl.list.meta,null,ngCtrl.stateParams,"label");
			}
		}

		//#endregion

		//#region << List actions  >>
		//load the actionService methods from rootScope
		//ngCtrl.actionService = $rootScope.actionService;
		var serviceName = safeListNameAndEntityName.entityName + "_" + safeListNameAndEntityName.listName + "_list_service";
		try {
			ngCtrl.actionService = $injector.get(serviceName);
		}
		catch (err) {
			//console.log(err);
			ngCtrl.actionService = {};
		}
		ngCtrl.pageTitleActions = [];
		ngCtrl.pageTitleDropdownActions = [];
		ngCtrl.recordRowActions = [];
		ngCtrl.recordRowDropdownActions = [];
		ngCtrl.pageBottomActions = [];
		ngCtrl.list.meta.actionItems.sort(sort_by('menu', { name: 'weight', primer: parseInt, reverse: false }));
		ngCtrl.list.meta.actionItems.forEach(function (actionItem) {
			switch (actionItem.menu) {
				case "page-title":
					ngCtrl.pageTitleActions.push(actionItem);
					break;
				case "page-title-dropdown":
					ngCtrl.pageTitleDropdownActions.push(actionItem);
					break;
				case "record-row":
					ngCtrl.recordRowActions.push(actionItem);
					break;
				case "record-row-dropdown":
					ngCtrl.recordRowDropdownActions.push(actionItem);
					break;
				case "page-bottom":
					ngCtrl.pageBottomActions.push(actionItem);
					break;
			}
		});
		ngCtrl.getRecordCreateUrl = function () {
			return webvellaCoreService.listAction_getRecordCreateUrl(ngCtrl);
		}

		ngCtrl.getRecordDetailsUrl = function (record) {
			return webvellaCoreService.listAction_getRecordDetailsUrl(record, ngCtrl);
		}

		//#endregion
	}
	//#endregion

	//#region << Modal Controller /////////////////////////////// >>
	exportModalController.$inject = ['$uibModalInstance', 'webvellaCoreService', 'ngToast', 'ngCtrl'];
	function exportModalController($uibModalInstance, webvellaCoreService, ngToast, ngCtrl) {
		var popupCtrl = this;
		popupCtrl.ngCtrl = fastCopy(ngCtrl);
		popupCtrl.loading = false;
		popupCtrl.hasError = false;
		popupCtrl.errorMessage = "";
		popupCtrl.count = -1;
		popupCtrl.countHasSize = true;
		popupCtrl.downloadFilePath = null;

		popupCtrl.count = popupCtrl.ngCtrl.list.meta.pageSize;

		popupCtrl.exportSuccessCallback = function (response) {
			//popupCtrl.loading = false;
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success </span> Records successfully exported!'
			});
			//popupCtrl.downloadFilePath = response.object;
			$uibModalInstance.dismiss('cancel');

		}
		popupCtrl.exportErrorCallback = function (response) {
			popupCtrl.loading = false;
			//popupCtrl.hasError = true;
			//popupCtrl.errorMessage = response.message;
		}

		popupCtrl.ok = function () {
			popupCtrl.loading = true;
			popupCtrl.hasError = false;
			popupCtrl.errorMessage = "";
			if (popupCtrl.count == 0) {
				popupCtrl.hasError = true;
				popupCtrl.loading = false;
				popupCtrl.errorMessage = "Records export count could not be 0";
			}
			else {
				if (!popupCtrl.countHasSize) {
					popupCtrl.count = -1;
				}
				webvellaCoreService.exportListRecords(popupCtrl.ngCtrl.entity.name, popupCtrl.ngCtrl.list.meta.name, popupCtrl.count, popupCtrl.exportSuccessCallback, popupCtrl.exportErrorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
	}
	importModalController.$inject = ['$uibModalInstance', 'webvellaCoreService', 'ngToast', '$timeout', '$state', 'ngCtrl'];
	function importModalController($uibModalInstance, webvellaCoreService, ngToast, $timeout, $state, ngCtrl) {
		var popupCtrl = this;
		popupCtrl.ngCtrl = fastCopy(ngCtrl);
		popupCtrl.uploadedFile = null;
		popupCtrl.uploadedFilePath = null;
		popupCtrl.uploadProgress = 0;
		popupCtrl.loading = false;
		popupCtrl.hasError = false;
		popupCtrl.errorMessage = "";

		popupCtrl.upload = function (file) {
			popupCtrl.uploadedFilePath = null;
			popupCtrl.uploadProgress = 0;

			if (file != null) {
				popupCtrl.uploadSuccessCallback = function (response) {
					popupCtrl.uploadedFilePath = response.object.url;
				}
				popupCtrl.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				popupCtrl.uploadProgressCallback = function (response) {
					$timeout(function () {
						popupCtrl.uploadProgress = parseInt(100.0 * response.loaded / response.total);
					}, 0);
				}

				webvellaCoreService.uploadFileToTemp(file, file.name, popupCtrl.uploadProgressCallback, popupCtrl.uploadSuccessCallback, popupCtrl.uploadErrorCallback);

			}
		}

		popupCtrl.deleteFileUpload = function () {
			$timeout(function () {
				popupCtrl.uploadedFile = null;
				popupCtrl.uploadedFilePath = null;
				popupCtrl.uploadProgress = 0;
			}, 0);
		}

		popupCtrl.importSuccessCallback = function (response) {
			//popupCtrl.loading = false;
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success </span> Records successfully imported!'
			});
			//$uibModalInstance.dismiss('cancel');
			$state.reload();
		}
		popupCtrl.importErrorCallback = function (response) {
			popupCtrl.loading = false;
			//popupCtrl.hasError = true;
			//popupCtrl.errorMessage = response.message;
		}

		popupCtrl.ok = function () {
			popupCtrl.loading = true;
			popupCtrl.hasError = false;
			popupCtrl.errorMessage = "";

			if (popupCtrl.uploadedFilePath == null || popupCtrl.uploadedFilePath == "") {
				popupCtrl.loading = false;
				popupCtrl.hasError = true;
				popupCtrl.errorMessage = "You need to upload a CSV file first";
			}
			else if (!popupCtrl.uploadedFile.name.endsWith(".csv")) {
				popupCtrl.loading = false;
				popupCtrl.hasError = true;
				popupCtrl.errorMessage = "This is not a CSV file";
			}
			else {
				webvellaCoreService.importEntityRecords(popupCtrl.ngCtrl.entity.name, popupCtrl.uploadedFilePath, popupCtrl.importSuccessCallback, popupCtrl.importErrorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
	}
	//#endregion

})();
