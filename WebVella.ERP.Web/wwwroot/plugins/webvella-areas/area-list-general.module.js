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
				loadPreloadScript: loadPreloadScript,
				resolvedCurrentView: function () { return null; },
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
				loadPreloadScript: loadPreloadScript,
				resolvedCurrentView: function () { return null },	//for the sidebar to render
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
			var dataNameArray = fastCopy($stateParams.listName).split('$');
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

	loadPreloadScript.$inject = ['loadDependency', 'webvellaListActionService', '$q', '$http', '$state'];
	function loadPreloadScript(loadDependency, webvellaListActionService, $q, $http, $state) {
		var defer = $q.defer();

		if (webvellaListActionService.preload === undefined || typeof (webvellaListActionService.preload) != "function") {
			console.log("No webvellaListActionService.preload function. Skipping");
			defer.resolve();
			return defer.promise;
		}
		else {
			webvellaListActionService.preload(defer, $state);
		}
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
				defer.reject(response.message);
			}
		}

		var userHasUpdateEntityPermission = webvellaCoreService.userHasRecordPermissions(resolvedCurrentEntityMeta, "canRead");
		if (!userHasUpdateEntityPermission) {
			alert("you do not have permissions to view records from this entity!");
			defer.reject("you do not have permissions to view records from this entity");
		}

		var parentView = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);
		webvellaCoreService.getRecordByViewMeta($stateParams.recordId, parentView, $stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}

	resolveRecordListDataFromView.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', 'resolvedParentViewData', 'resolvedEntityList', 'resolvedEntityRelationsList','ngToast'];
	function resolveRecordListDataFromView($q, $log, webvellaCoreService, $stateParams, $state, $timeout, resolvedParentViewData, resolvedEntityList, resolvedEntityRelationsList,ngToast) {
		//Temporary method will be replaced when the proper API is ready
		// Initialize
		var defer = $q.defer();
		var safeListNameAndEntity = webvellaCoreService.getSafeListNameAndEntityName($stateParams.listName, $stateParams.entityName, resolvedEntityRelationsList)
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
	controller.$inject = ['$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaCoreService',
        'resolvedAreas', 'resolvedRecordListData', 'resolvedEntityList', 'resolvedCurrentEntityMeta', 'webvellaListActionService', '$timeout',
		'resolvedEntityRelationsList', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce'];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService,
        resolvedAreas, resolvedRecordListData, resolvedEntityList, resolvedCurrentEntityMeta, webvellaListActionService, $timeout,
		resolvedEntityRelationsList, resolvedCurrentUser, $sessionStorage, $location, $window, $sce) {

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
		ngCtrl.list.data = fastCopy(resolvedRecordListData);
		ngCtrl.list.meta = webvellaCoreService.getEntityRecordListFromEntitiesMetaList(listName, listEntityName, resolvedEntityList);
		ngCtrl.entityList = fastCopy(resolvedEntityList);
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.entityRelations = fastCopy(resolvedEntityRelationsList);
		ngCtrl.areas = fastCopy(resolvedAreas.data);
		ngCtrl.currentUser = fastCopy(resolvedCurrentUser);
		ngCtrl.$sessionStorage = $sessionStorage;
		ngCtrl.stateParams = $stateParams;
		//#endregion

		//#region << Set Page meta >>
		ngCtrl.pageTitle = ngCtrl.list.meta.label + " | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion

		//#region << Run  webvellaListActionService.onload >>
		if (webvellaListActionService.onload === undefined || typeof (webvellaListActionService.onload) != "function") {
			$log.warn("No webvellaListActionService.onload function. Skipping");
		}
		else {
			var actionsOnLoadResult = webvellaListActionService.onload(ngCtrl, $rootScope, $state);
			if (actionsOnLoadResult != true) {
				ngCtrl.validation.hasError = true;
				ngCtrl.validation.errorMessage = $sce.trustAsHtml(actionsOnLoadResult);
			}
		}
		//#endregion

		//#region << Alternative general lists dropdown >>
		ngCtrl.generalLists = [];
		ngCtrl.entity.recordLists.forEach(function (list) {
			if (list.type == "general") {
				ngCtrl.generalLists.push(list);
			}
		});
		ngCtrl.generalLists.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
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
					//TODO if percent convert to > 1 %
					case 14:
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
			for (var activeFilter in ngCtrl.filterQuery) {
				$location.search(activeFilter, null);
			}
			var searchParams = $location.search();
			ngCtrl.filterQuery = {};
			ngCtrl.listIsFiltered = false;
			ngCtrl.show_filter = false;
			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, $stateParams.entityName, $stateParams.page, null, searchParams, ngCtrl.ReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);
		}

		ngCtrl.applyQueryFilter = function () {
			//TODO - Convert percent into 0 < x < 1

			//TODO - Convert date to ISO
			var queryFieldsCount = 0;
			for (var filter in ngCtrl.filterQuery) {
				if (ngCtrl.filterQuery[filter] == "") {
					$location.search(filter, null);
				}
				else {
					queryFieldsCount++;
					$location.search(filter, ngCtrl.filterQuery[filter]);
				}
			}
			//$window.location.reload();
			var searchParams = $location.search();
			if (queryFieldsCount > 0) {
				ngCtrl.listIsFiltered = true;
			}
			//Find the entity of the list. It could not be the current one as it could be listFromRelation case
			var listEntityName = $stateParams.entityName;
			for (var i = 0; i < ngCtrl.entityList.length; i++) {
				for (var j = 0; j < ngCtrl.entityList[i].recordLists.length; j++) {
					if (ngCtrl.entityList[i].recordLists[j].id == ngCtrl.list.meta.id) {
						listEntityName = fastCopy(ngCtrl.entityList[i].name);
					}
				}
			}

			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, listEntityName, 1, null, searchParams, ngCtrl.ReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);
		}

		ngCtrl.ReloadRecordsSuccessCallback = function (response) {
			ngCtrl.list.data = response.object.data;
		}

		ngCtrl.ReloadRecordsErrorCallback = function (response) {
			alert(response.message);
		}

		ngCtrl.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				ngCtrl.applyQueryFilter();
			}
		}



		//#endregion

		//#region << Extract fields that are supported in the query to be filters>>
		ngCtrl.fieldsInQueryArray = webvellaCoreService.extractSupportedFilterFields(ngCtrl.list);
		ngCtrl.checkIfFieldSetInQuery = function (fieldName) {
			if (ngCtrl.fieldsInQueryArray.indexOf(fieldName) != -1) {
				return true;
			}
			else {
				return false;
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

		ngCtrl.selectPage = function (page) {
			var params = {
				areaName: $stateParams.areaName,
				entityName: entityName,
				listName: listName,
				page: page
			};
			webvellaCoreService.GoToState($state, $state.current.name, params);
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
			return webvellaCoreService.userHasRecordPermissions(ngCtrl.entity, permissionsCsv);
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
			var listEntityName = $stateParams.entityName;
			for (var i = 0; i < ngCtrl.entityList.length; i++) {
				for (var j = 0; j < ngCtrl.entityList[i].recordLists.length; j++) {
					if (ngCtrl.entityList[i].recordLists[j].id == ngCtrl.list.meta.id) {
						listEntityName = fastCopy(ngCtrl.entityList[i].name);
					}
				}
			}
			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, listEntityName, 1, null, searchParams, ngCtrl.ReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);

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
		ngCtrl.getAutoIncrementPrefix = function (column) {
			var keyIndex = column.meta.displayFormat.indexOf('{0}');
			return column.meta.displayFormat.slice(0, keyIndex);
		}

		//#endregion

		//#region << List actions and webvellaListActionService bind >>
		ngCtrl.actionService = webvellaListActionService;
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
		//#endregion

		//#region << Run  webvellaListActionService.postload >>
		if (webvellaListActionService.postload === undefined || typeof (webvellaListActionService.postload) != "function") {
			$log.warn("No webvellaListActionService.postload function. Skipping");
		}
		else {
			var actionsOnLoadResult = webvellaListActionService.postload(ngCtrl, $rootScope, $state);
			if (actionsOnLoadResult != true) {
				ngCtrl.validation.hasError = true;
				ngCtrl.validation.errorMessage = $sce.trustAsHtml(actionsOnLoadResult);
			}
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
			popupCtrl.downloadFilePath = response.object;

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
					}, 100);
				}

				webvellaCoreService.uploadFileToTemp(file, file.name, popupCtrl.uploadProgressCallback, popupCtrl.uploadSuccessCallback, popupCtrl.uploadErrorCallback);

			}
		}

		popupCtrl.deleteFileUpload = function () {
			$timeout(function () {
				popupCtrl.uploadedFile = null;
				popupCtrl.uploadedFilePath = null;
				popupCtrl.uploadProgress = 0;
			}, 100);
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
