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

		var list = webvellaCoreService.getEntityRecordListFromEntitiesMetaList($stateParams.listName, $stateParams.entityName, resolvedEntityList);

		webvellaCoreService.getRecordsByListMeta(list, $stateParams.entityName, $stateParams.page, $stateParams, $location.search(), successCallback, errorCallback);
		return defer.promise;
	}

	resolveParentViewData.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', 'resolvedCurrentEntityMeta', 'resolvedEntityList'];
	function resolveParentViewData($q, $log, webvellaCoreService, $stateParams, $state, $timeout, resolvedCurrentEntityMeta, resolvedEntityList) {

		// Initialize
		var defer = $q.defer();
		// Process
		function successCallback(response) {
			if (response.object === null) {
			    alert("error in response! " + response.message);
			}
			else if (response.object === null) {
				alert("The view with name: " + $stateParams.parentViewName + " does not exist");
			} else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
			    alert("error in response! " + response.message);
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
		webvellaCoreService.getRecordByViewMeta($stateParams.recordId, parentView, $stateParams.entityName, $stateParams, successCallback, errorCallback);

		return defer.promise;
	}

	resolveRecordListDataFromView.$inject = ['$q','$location', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', 'resolvedParentViewData', 'resolvedEntityList', 'resolvedEntityRelationsList', 'ngToast'];
	function resolveRecordListDataFromView($q, $location,$log, webvellaCoreService, $stateParams, $state, $timeout, resolvedParentViewData, resolvedEntityList, resolvedEntityRelationsList, ngToast) {
		//Temporary method will be replaced when the proper API is ready
		// Initialize
		var defer = $q.defer();
		var safeListNameAndEntity = webvellaCoreService.getSafeListNameAndEntityName($stateParams.listName, $stateParams.entityName, resolvedEntityRelationsList);
		var getListMeta = webvellaCoreService.getEntityRecordListFromEntitiesMetaList(safeListNameAndEntity.listName, safeListNameAndEntity.entityName, resolvedEntityList);
		if (getListMeta.dataSourceUrl != null && getListMeta.dataSourceUrl != "") {
			//This list has a dynamicSourceUrl defined
			webvellaCoreService.getRecordsByListMeta(getListMeta, safeListNameAndEntity.entityName, 1, $stateParams, $location.search(), successCallback, errorCallback);
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
	controller.$inject = ['$q', '$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaCoreService', '$injector',
        'resolvedAreas', 'resolvedRecordListData', 'resolvedEntityList', 'resolvedCurrentEntityMeta', '$timeout', '$translate',
		'resolvedEntityRelationsList', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce', 'resolvedParentViewData'];

	function controller($q, $log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService, $injector,
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
		else{
			ngCtrl.$sessionStorage["last-list-params"] = fastCopy($stateParams);		
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
			//The $relation.field_name format needs to be converted to dataNameFormat
			if (key.startsWith("$")) {
				var proccessedKey = key;
				proccessedKey = proccessedKey.substring(1);
				var proccessedKeyArray = proccessedKey.split(".");
				proccessedKey = "$field$" + proccessedKeyArray[0] + "$" + proccessedKeyArray[1];
				queryObject[proccessedKey] = fastCopy(queryObject[key]);
				delete queryObject[key];
				key = proccessedKey;
			}

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
					case 2: //Checkbox
						if(!queryObject[dataName]){
							ngCtrl.filterQuery[dataName] = null;
						}
						break;
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
				if (activeFilter.startsWith("$field")) {
					var dataNameArray = activeFilter.split("$");
					activeFilter = "$" + dataNameArray[2] + "." + dataNameArray[3];
				}
				$location.search(activeFilter, null);
			}
			var searchParams = $location.search(); //For some reason here are returned params from the previous state so it needs to be inited
			ngCtrl.filterQuery = {};
			ngCtrl.listIsFiltered = false;
			ngCtrl.show_filter = false;
			webvellaCoreService.getRecordsByListMeta(ngCtrl.list.meta, safeListNameAndEntityName.entityName, $stateParams.page, $stateParams, searchParams, ngCtrl.ReloadRecordsSuccessCallback, ngCtrl.ReloadRecordsErrorCallback);
		}

		ngCtrl.applyQueryFilter = function () {
			ngCtrl.filterLoading = true;
			var queryFieldsCount = 0;
			for (var filter in ngCtrl.filterQuery) {
				if (ngCtrl.filterQuery[filter] && ngCtrl.filterQuery[filter] != '') {
					queryFieldsCount++;
					for (var i = 0; i < ngCtrl.list.meta.columns.length; i++) {
						if (ngCtrl.list.meta.columns[i].dataName == filter) {
							var selectedField = ngCtrl.list.meta.columns[i].meta;
							//When field from relation, the data name needs to be converted to $relation.field_name
							if (filter.startsWith("$field")) {
								var dataNameArray = filter.split("$");
								filter = "$" + dataNameArray[2] + "." + dataNameArray[3];
								ngCtrl.filterQuery[filter] = fastCopy(ngCtrl.filterQuery[ngCtrl.list.meta.columns[i].dataName]);
								delete ngCtrl.filterQuery[ngCtrl.list.meta.columns[i].dataName];
								$location.search(ngCtrl.list.meta.columns[i].dataName, null);
							}
							switch (selectedField.fieldType) {
								case 4: //Date
									$location.search(filter, moment(ngCtrl.filterQuery[filter], 'D MMM YYYYY').toISOString());
									break;
								case 5: //Datetime
									$location.search(filter, moment(ngCtrl.filterQuery[filter], 'D MMM YYYYY HH:mm').toISOString());
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
					//When field from relation, the data name needs to be converted to $relation.field_name
					var dataName = fastCopy(filter);
					if (filter.startsWith("$field")) {
						var dataNameArray = filter.split("$");
						filter = "$" + dataNameArray[2] + "." + dataNameArray[3];
					}
					delete ngCtrl.filterQuery[dataName];
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
			$timeout(function () {
				ngCtrl.filterLoading = false;
			}, 300);
			ngCtrl.list.data = response.object;
			//fieldName and dataName are different when fromRelation (the second $ is a dot)
			for (var key in ngCtrl.filterQuery) {
				//The $relation.field_name format needs to be converted to dataNameFormat
				if (key.startsWith("$")) {
					var proccessedKey = key;
					proccessedKey = proccessedKey.substring(1);
					var proccessedKeyArray = proccessedKey.split(".");
					proccessedKey = "$field$" + proccessedKeyArray[0] + "$" + proccessedKeyArray[1];
					ngCtrl.filterQuery[proccessedKey] = fastCopy(ngCtrl.filterQuery[key]);
					delete ngCtrl.filterQuery[key];
				}
			}
		}

		ngCtrl.ReloadRecordsErrorCallback = function (response) {
			$timeout(function () {
				ngCtrl.filterLoading = false;
			}, 300);
			alert(response.message);
		}

		//#endregion

		//#region << Extract fields that are supported in the query to be filters>>
		ngCtrl.fieldsInQueryArray = webvellaCoreService.extractSupportedFilterFields(ngCtrl.list);
		if (ngCtrl.fieldsInQueryArray.fieldNames.length > 0) {
			ngCtrl.show_filter = true;
		}
		ngCtrl.checkIfFieldSetInQuery = function (dataName) {
			//fieldName and dataName are different when fromRelation (the second $ is a dot)
			if (dataName.startsWith("$field")) {
				var dataNameArray = dataName.split("$");
				dataName = "$" + dataNameArray[2] + "." + dataNameArray[3];
			}
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
					'QUERY_RULE_STARTSWITH_LABEL', 'QUERY_RULE_NOT_STARTSWITH_LABEL','QUERY_RULE_FTS_LABEL']).then(function (translations) {
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
							},
							{
								key: "FTS",
								value: translations.QUERY_RULE_FTS_LABEL
							}
						];

					});
		//#endregion
		ngCtrl.getFilterInputPlaceholder = function (dataName) {
			//convert field from relation to its proper value
			if (dataName.startsWith("$field")) {
				var dataNameArray = dataName.split("$");
				dataName = "$" + dataNameArray[2] + "." + dataNameArray[3];
			}
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
			$timeout(function () {
				ngCtrl.filterLoading = false;
			}, 300);
			ngCtrl.list.data = response.object;
			if (ngCtrl.currentPage != 1) {
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

			searchParams.relationId = null;
			searchParams.relatedRecordId = null;
			searchParams.direction = "origin-target";

			if ($stateParams.parentViewName != null) {
				searchParams.relatedRecordId = $stateParams.recordId;
				for (var i = 0; i < ngCtrl.parentView.meta.sidebar.items.length; i++) {
					var sidebarItem = ngCtrl.parentView.meta.sidebar.items[i];
					if (sidebarItem.listName == ngCtrl.list.meta.name) {
						searchParams.relationId = sidebarItem.relationId;
						break;
					}
				}
			}

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
			var listEntity = webvellaCoreService.getEntityMetaFromEntityList(safeListNameAndEntityName.entityName, resolvedEntityList);
			return webvellaCoreService.userHasRecordPermissions(listEntity, permissionsCsv);
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

			searchParams.relationId = null;
			searchParams.relatedRecordId = null;
			searchParams.direction = "origin-target";

			if ($stateParams.parentViewName != null) {
				searchParams.relatedRecordId = $stateParams.recordId;
				for (var i = 0; i < ngCtrl.parentView.meta.sidebar.items.length; i++) {
					var sidebarItem = ngCtrl.parentView.meta.sidebar.items[i];
					if (sidebarItem.listName == ngCtrl.list.meta.name) {
						searchParams.relationId = sidebarItem.relationId;
						break;
					}
				}
			}

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
				size: "lg",
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
				size: "width-100p",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					fieldTypes: resolveFieldTypes
				}
			});
		}

		var resolveFieldTypes = function () {
			var defer = $q.defer();
			function getTypesSuccess(response) {
				defer.resolve(response);
			}
			webvellaCoreService.getFieldTypes(getTypesSuccess);
			return defer.promise;
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

			var keyIndex = 0;
			if (column.meta.displayFormat) {
				keyIndex = column.meta.displayFormat.indexOf('{0}');
			}
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

		ngCtrl.generateHighlightString = function () {
			if (ngCtrl.parentView && ngCtrl.parentView.data) {
				ngCtrl.showPageTitleAuxLabelSecondary = true;
				return webvellaCoreService.generateHighlightString(ngCtrl.list.meta,ngCtrl.parentView.data, ngCtrl.stateParams, "title");
			}
			else {
				return webvellaCoreService.generateHighlightString(ngCtrl.list.meta, ngCtrl.list.data[0], ngCtrl.stateParams, "title");
			}
		}

		ngCtrl.generateAuxHighlightString = function () {
			if (ngCtrl.parentView && ngCtrl.parentView.data) {
				return webvellaCoreService.generateHighlightString(ngCtrl.parentView.meta, ngCtrl.parentView.data[0], ngCtrl.stateParams, "title");
			}
			else {
				return webvellaCoreService.generateHighlightString(ngCtrl.list.meta, ngCtrl.list.data[0], ngCtrl.stateParams, "title");
			}
		}

		ngCtrl.generateSublistHighlightString = function (listMeta) {
			if (ngCtrl.parentView && ngCtrl.parentView.data) {
				ngCtrl.showPageTitleAuxLabelSecondary = true;
				return webvellaCoreService.generateHighlightString(listMeta,ngCtrl.parentView.data, ngCtrl.stateParams, "title");
			}
			else {
				return webvellaCoreService.generateHighlightString(listMeta, ngCtrl.list.data[0], ngCtrl.stateParams, "title");
			}
		}

		//ngCtrl.generateHighlightString = function () {
		//	if (ngCtrl.parentView && ngCtrl.parentView.data) {
		//		ngCtrl.showPageTitleAuxLabelSecondary = true;
		//		return webvellaCoreService.generateHighlightString(ngCtrl.parentView.meta, ngCtrl.parentView.data[0], ngCtrl.stateParams, "title");
		//	}
		//	else {
		//		return webvellaCoreService.generateHighlightString(ngCtrl.list.meta, null, ngCtrl.stateParams, "title");
		//	}
		//}

		//ngCtrl.generateAuxHighlightString = function () {
		//	if (ngCtrl.parentView && ngCtrl.parentView.data) {
		//		return webvellaCoreService.generateHighlightString(ngCtrl.list.meta, ngCtrl.parentView.data[0], ngCtrl.stateParams, "label");
		//	}
		//	else {
		//		return webvellaCoreService.generateHighlightString(ngCtrl.list.meta, null, ngCtrl.stateParams, "label");
		//	}
		//}

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

		//Manage list
		ngCtrl.getListManageUrl = function () {
			return "/#/admin/entities/" + $stateParams.entityName + "/lists/" + $stateParams.listName;
		}

		ngCtrl.userIsAdmin = function () {
			return webvellaCoreService.userIsInRole("bdc56420-caf0-4030-8a0e-d264938e0cda");
		}

		//#endregion
	}
	//#endregion

	//#region << Modal Controller /////////////////////////////// >>
	exportModalController.$inject = ['$uibModalInstance', 'webvellaCoreService', 'ngToast', 'ngCtrl', '$location'];
	function exportModalController($uibModalInstance, webvellaCoreService, ngToast, ngCtrl, $location) {
		var popupCtrl = this;
		popupCtrl.ngCtrl = fastCopy(ngCtrl);
		popupCtrl.loading = false;
		popupCtrl.hasError = false;
		popupCtrl.errorMessage = "";
		popupCtrl.count = -1;
		popupCtrl.countHasSize = false;
		popupCtrl.downloadFilePath = null;
		popupCtrl.listHasExternalDataSource = false;
		if(popupCtrl.ngCtrl.list.meta.dataSourceUrl != null && popupCtrl.ngCtrl.list.meta.dataSourceUrl != ""){
			popupCtrl.listHasExternalDataSource = true;	
		}

		popupCtrl.count = popupCtrl.ngCtrl.list.meta.pageSize;

		popupCtrl.getExportFields = function () {
			var columnsForExport = [];
			for (var i = 0; i < popupCtrl.ngCtrl.list.meta.columns.length; i++) {
				var currentColumnMeta = popupCtrl.ngCtrl.list.meta.columns[i];
				if (currentColumnMeta.type == "field" || currentColumnMeta.type == "fieldFromRelation") {
					columnsForExport.push(currentColumnMeta.fieldName);
				}
			}
			return columnsForExport;
		}

		popupCtrl.exportSuccessCallback = function (response) {
			//popupCtrl.loading = false;
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success </span> Records successfully exported!'
			});
			//popupCtrl.downloadFilePath = response.object;
			$uibModalInstance.close('dismiss');

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

				webvellaCoreService.exportListRecords(popupCtrl.ngCtrl.entity.name, popupCtrl.ngCtrl.list.meta.name, popupCtrl.count, $location.search(), popupCtrl.exportSuccessCallback, popupCtrl.exportErrorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
		};
	}

	importModalController.$inject = ['$scope', '$uibModalInstance', 'webvellaCoreService', 'ngToast', '$timeout', '$state', 'ngCtrl', 'fieldTypes', '$sce'];
	function importModalController($scope, $uibModalInstance, webvellaCoreService, ngToast, $timeout, $state, ngCtrl, fieldTypes, $sce) {
		var popupCtrl = this;
		popupCtrl.ngCtrl = fastCopy(ngCtrl);
		popupCtrl.uploadedFile = null;
		popupCtrl.uploadedFilePath = null;
		popupCtrl.uploadProgress = 0;
		popupCtrl.loading = false;
		popupCtrl.hasError = false;
		popupCtrl.errorMessage = "";
		popupCtrl.accordion = {};
		popupCtrl.accordion.clipboard = {};
		popupCtrl.accordion.clipboard.active = true;
		popupCtrl.accordion.file = {};
		popupCtrl.accordion.file.active = false;
		popupCtrl.activeStep = 1;
		popupCtrl.createFieldCount = 0;
		popupCtrl.entityFieldsObject = {};
		//Init entityFields array
		for (var i = 0; i < popupCtrl.ngCtrl.entityList.length; i++) {
			var currentEntity = popupCtrl.ngCtrl.entityList[i];
			popupCtrl.entityFieldsObject[currentEntity.name] = currentEntity.fields;

		}


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

				webvellaCoreService.uploadFileToTemp(file, popupCtrl.uploadProgressCallback, popupCtrl.uploadSuccessCallback, popupCtrl.uploadErrorCallback);

			}
		}

		popupCtrl.deleteFileUpload = function () {
			$timeout(function () {
				popupCtrl.uploadedFile = null;
				popupCtrl.uploadedFilePath = null;
				popupCtrl.uploadProgress = 0;
			}, 0);
		}

		popupCtrl.importErrorCallback = function (response) {
			popupCtrl.loading = false;
			//popupCtrl.hasError = true;
			//popupCtrl.errorMessage = response.message;
		}

		popupCtrl.evaluateAndImport = function () {
			popupCtrl.hasError = false;
			popupCtrl.evaluationResult.records = popupCtrl.removeUnderscore(popupCtrl.evaluationResult.records, 'array');
			popupCtrl.evaluationResult.commands = popupCtrl.removeUnderscore(popupCtrl.evaluationResult.commands, 'object');
			webvellaCoreService.evaluateImportEntityRecords(popupCtrl.ngCtrl.entity.name, popupCtrl.uploadedFilePath, "evaluate-import", popupCtrl.clipboard, popupCtrl.evaluationResult.commands, popupCtrl.importSuccessCallback, popupCtrl.evaluationErrorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
		};

		popupCtrl.CloseAfterImport = function () {
			$uibModalInstance.close('dismiss');
			$state.reload();
		};

		popupCtrl.addUnderscore = function (targetObject, type) {
			if (type == "array") {
				for (var i = 0; i < targetObject.length; i++) {
					var record = targetObject[i];
					for (var property in record) {
						if (property.startsWith("$")) {
							record["_" + property] = record[property];
							delete record[property];
						}
					}
				};
			}
			else if (type == "object") {
				var record = targetObject;
				for (var property in record) {
					if (property.startsWith("$")) {
						record["_" + property] = record[property];
						delete record[property];
					}
				}
			}
			return targetObject;
		}

		popupCtrl.removeUnderscore = function (targetObject, type) {
			if (type == "array") {
				for (var i = 0; i < targetObject.length; i++) {
					var record = targetObject[i];
					for (var property in record) {
						if (property.startsWith("_$")) {
							var newProperty = property.substring(1);
							record[newProperty] = record[property];
							delete record[property];
						}
					}
				};
			}
			else if (type == "object") {
				var record = targetObject;
				for (var property in record) {
					if (property.startsWith("_$")) {
						var newProperty = property.substring(1);
						record[newProperty] = record[property];
						delete record[property];
					}
				}
			}
			return targetObject;
		}


		//EVALUATE
		//init eval object
		popupCtrl.evaluationResult = {};
		popupCtrl.evaluationResult.commands = null;
		popupCtrl.columnHasError = {};
		popupCtrl.columnHasWarning = {};
		popupCtrl.evaluate = function () {
			popupCtrl.hasError = false;
			webvellaCoreService.evaluateImportEntityRecords(popupCtrl.ngCtrl.entity.name, popupCtrl.uploadedFilePath, "evaluate", popupCtrl.clipboard, popupCtrl.evaluationResult.commands, popupCtrl.evaluationSuccessCallback, popupCtrl.evaluationErrorCallback);
		};

		popupCtrl.ifColumnHasError = function (errorArray) {
			for (var i = 0; i < errorArray.length; i++) {
				if (errorArray[i] != null && errorArray[i] != "") {
					return true;
				}
			}
			return false;
		}

		popupCtrl.ifColumnHasWarning = function (warningArray) {
			for (var i = 0; i < warningArray.length; i++) {
				if (warningArray[i] != null && warningArray[i] != "") {
					return true;
				}
			}
			return false;
		}

		popupCtrl.evaluationSuccessCallback = function (response) {
			response.object.records = popupCtrl.addUnderscore(response.object.records, 'array');
			response.object.commands = popupCtrl.addUnderscore(response.object.commands, 'object');
			response.object.errors = popupCtrl.addUnderscore(response.object.errors, 'object');

			popupCtrl.evaluationResult = response.object;
			for (var columnName in popupCtrl.evaluationResult.errors) {
				if (popupCtrl.ifColumnHasError(popupCtrl.evaluationResult.errors[columnName])) {
					popupCtrl.columnHasError[columnName] = true;
				}
				else {
					popupCtrl.columnHasError[columnName] = false;
				}
			}
			for (var columnName in popupCtrl.evaluationResult.warnings) {
				if (popupCtrl.ifColumnHasWarning(popupCtrl.evaluationResult.warnings[columnName])) {
					popupCtrl.columnHasWarning[columnName] = true;
				}
				else {
					popupCtrl.columnHasWarning[columnName] = false;
				}
			}
			//Calculate fields to create
			updateCreateFieldCount()

			popupCtrl.activeStep = 2;
		}

		popupCtrl.importSuccessCallback = function (response) {
			popupCtrl.evaluationResult = response.object;
			popupCtrl.activeStep = 3;
		}


		function updateCreateFieldCount() {
			popupCtrl.createFieldCount = 0;
			for (var commandObject in popupCtrl.evaluationResult.commands) {
				if (popupCtrl.evaluationResult.commands[commandObject].command == "to_create") {
					popupCtrl.createFieldCount++;
				}
			}
		}

		$scope.$watch("popupCtrl.evaluationResult.commands", function () {
			updateCreateFieldCount();
		}, true);


		popupCtrl.evaluationErrorCallback = function (response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;
		}

		popupCtrl.getEntityFieldsFromType = function (type, entityName) {
			var fields = [];
			if (entityName == null) {
				popupCtrl.ngCtrl.entity.fields.forEach(function (field) {
					if (field.fieldType == type) {
						fields.push(field);
					}
				});
			}
			else if (popupCtrl.entityFieldsObject[entityName] != undefined) {
				popupCtrl.entityFieldsObject[entityName].forEach(function (field) {
					if (field.fieldType == type) {
						fields.push(field);
					}
				});
			}

			return fields;
		}

		popupCtrl.fieldTypes = [];

		function getFieldTypesSuccessCallback(response) {
			popupCtrl.fieldTypes = response;
		}
		webvellaCoreService.getFieldTypes(getFieldTypesSuccessCallback);

		popupCtrl.getFieldTypeLabel = function (typeId) {
			for (var i = 0; i < popupCtrl.fieldTypes.length; i++) {
				if (popupCtrl.fieldTypes[i].id == typeId) {
					return popupCtrl.fieldTypes[i].label;
				}
			}
			return "";
		}

		popupCtrl.updateExistingFieldCommand = function (command) {
			if (command.relationName == '') {
				for (var i = 0; i < popupCtrl.ngCtrl.entity.fields.length; i++) {
					if (popupCtrl.ngCtrl.entity.fields[i].name == command.fieldName) {
						command.fieldLabel = popupCtrl.ngCtrl.entity.fields[i].label;
						return;
					}
				}
			}
			else if (popupCtrl.entityFieldsObject[command.entityName] != undefined) {
				for (var i = 0; i < popupCtrl.entityFieldsObject[command.entityName].length; i++) {
					if (popupCtrl.entityFieldsObject[command.entityName][i].name == command.fieldName) {
						command.fieldLabel = popupCtrl.entityFieldsObject[command.entityName][i].label;
						return;
					}
				}
			}
		}

		popupCtrl.updateColumnCommand = function (key, command) {
			switch (command.command) {
				case "no_import":
					command.fieldType = 18;
					break;
				case "to_update":
					break;
				case "to_create":
					command.fieldType = 18;
					break;
			}
		}

		popupCtrl.recalculateCreateFields = function () {
			popupCtrl.evaluationResult.stats.to_create = 0;
			for (var command in popupCtrl.evaluationResult.commands) {
				if (command.command == "to_create") {
					popupCtrl.evaluationResult.stats.to_create = popupCtrl.evaluationResult.stats.to_create + 1;
				}
			}


		}

		//BACK
		popupCtrl.back = function () {
			popupCtrl.activeStep = 1;
		};


		//IMPORT
		popupCtrl.import = function () {
			popupCtrl.activeStep = 3;
		};


		popupCtrl.importTypes = [
			{
				key: "no_import",
				value: "Do not import"
			},
			{
				key: "to_update",
				value: "To existing field"
			},
			{
				key: "to_create",
				value: "Create new field"
			}];

		popupCtrl.fieldTypes = fieldTypes;

		popupCtrl.getFieldTypeDescription = function (typeId) {
			var response = "";
			for (var i = 0; i < fieldTypes.length; i++) {
				if (fieldTypes[i].id == typeId) {
					response = fieldTypes[i].description;
					break;
				}
			}
			return $sce.trustAsHtml(response);
		}


	}
	//#endregion

})();
