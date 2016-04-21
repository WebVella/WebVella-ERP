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
		$stateProvider.state('webvella-entity-records', {
			parent: 'webvella-areas-base',
			url: '/:listName/:page',
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
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				loadDependency: loadDependency,
				loadPreloadScript: loadPreloadScript,
				resolvedListRecords: resolveListRecords
			},
			data: {

			}
		});
	};
	
	//#endregion

	//#region << Resolve Function >>

	////////////////////////
 	loadDependency.$inject = ['$ocLazyLoad','$q','$http','$stateParams','resolvedCurrentEntityMeta','wvAppConstants'];
	function loadDependency($ocLazyLoad, $q, $http,$stateParams,resolvedCurrentEntityMeta,wvAppConstants){
        var lazyDeferred = $q.defer();

		var listServiceJavascriptPath = wvAppConstants.apiBaseUrl + 'meta/entity/' +  $stateParams.entityName + '/list/' + $stateParams.listName + '/service.js?v=' + resolvedCurrentEntityMeta.hash;

		var loadFilesArray = [];
		loadFilesArray.push(listServiceJavascriptPath);

        return $ocLazyLoad.load ({
          name: 'webvellaAreas.recordsList',
          files: loadFilesArray
        }).then(function() {
           return lazyDeferred.resolve("ready");
        });	
	
	}

 	loadPreloadScript.$inject = ['loadDependency','webvellaActionService','$q','$http','$state'];
	function loadPreloadScript(loadDependency,webvellaActionService, $q, $http,$state){
        var defer = $q.defer();

		if (webvellaActionService.preload === undefined || typeof (webvellaActionService.preload) != "function") {
			console.log("No webvellaActionService.preload function. Skipping");
			defer.resolve();
			return defer.promise;
		}
		else {
			webvellaActionService.preload(defer,$state);
		}
	}

	resolveListRecords.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams', '$timeout', 'ngToast'];
	function resolveListRecords($q, $log, webvellaCoreService, $state, $stateParams, $timeout, ngToast) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getRecordsByListName($stateParams.listName, $stateParams.entityName, $stateParams.page, successCallback, errorCallback);
		return defer.promise;
	}

	//#endregion

	//#region << Controller /////////////////////////////// >>
	controller.$inject = ['$log', '$uibModal', '$rootScope','$state', '$stateParams', 'pageTitle', 'webvellaCoreService',
        'resolvedAreas', 'resolvedListRecords', 'resolvedCurrentEntityMeta','webvellaActionService',
		'resolvedEntityRelationsList', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window','$sce'];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService,
        resolvedAreas, resolvedListRecords, resolvedCurrentEntityMeta,webvellaActionService,
		resolvedEntityRelationsList, resolvedCurrentUser, $sessionStorage, $location, $window, $sce) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		ngCtrl.validation = {};
		ngCtrl.validation.hasError = false;
		ngCtrl.validation.errorMessage = "";
		ngCtrl.currentPage = parseInt($stateParams.page);
		//#endregion

		//#region << Set Page meta >>
		ngCtrl.pageTitle = "Area Entities | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion
		
		//#region << Initialize main objects >>
		ngCtrl.list = {};
		ngCtrl.list.data = fastCopy(resolvedListRecords.data);
		ngCtrl.list.meta = fastCopy(resolvedListRecords.meta);
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.entityRelations = fastCopy(resolvedEntityRelationsList);
		ngCtrl.areas = fastCopy(resolvedAreas.data);
		ngCtrl.currentUser = fastCopy(resolvedCurrentUser);
		ngCtrl.$sessionStorage = $sessionStorage;
		ngCtrl.stateParams = $stateParams;
		//#endregion

		//#region << Run  webvellaActionService.onload >>
		if (webvellaActionService.onload === undefined || typeof (webvellaActionService.onload) != "function") {
			$log.warn("No webvellaActionService.onload function. Skipping");
		}
		else {
			var actionsOnLoadResult = webvellaActionService.onload(ngCtrl,$rootScope,$state);
			if(actionsOnLoadResult != true){
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
		if(ngCtrl.list.meta.columnWidthsCSV){
			columnWidthsArray = ngCtrl.list.meta.columnWidthsCSV.split(',');
		}
		var visibleColumns =  ngCtrl.list.meta.visibleColumnsCount;
		if(columnWidthsArray.length > 0){
		   	for (var i = 0; i < visibleColumns; i++) {
				if(columnWidthsArray.length >= i +1){
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
			$window.location.reload();
		}

		ngCtrl.applyQueryFilter = function () {
			//TODO - Convert percent into 0 < x < 1

		}

		//#endregion
  
		//#region << Logic >> 

		ngCtrl.selectPage = function (page) {
			var params = {
				areaName: $stateParams.areaName,
				entityName: $stateParams.entityName,
				listName: $stateParams.listName,
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

 		ngCtrl.checkEntityPermissions = function (permissionsCsv) {
			return webvellaCoreService.userHasEntityPermissions(ngCtrl.entity, permissionsCsv);
		}

		ngCtrl.saveStateParamsToSessionStorage = function () {
			ngCtrl.$sessionStorage["last-list-params"] = $stateParams;
		}
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

		//#region << List actions and webvellaActionService bind >>
		ngCtrl.actionService = webvellaActionService;
		ngCtrl.pageTitleActions = [];
		ngCtrl.pageTitleDropdownActions = [];
		ngCtrl.recordRowActions = [];
		ngCtrl.recordRowDropdownActions = [];
		ngCtrl.pageBottomActions = [];
		ngCtrl.list.meta.actionItems.sort(sort_by('menu', {name:'weight', primer: parseInt, reverse: false}));
		ngCtrl.list.meta.actionItems.forEach(function(actionItem){
			switch(actionItem.menu){
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

		//#region << Run  webvellaActionService.postload >>
		if (webvellaActionService.postload === undefined || typeof (webvellaActionService.postload) != "function") {
			$log.warn("No webvellaActionService.postload function. Skipping");
		}
		else {
			var actionsOnLoadResult = webvellaActionService.postload(ngCtrl,$rootScope,$state);
			if(actionsOnLoadResult != true){
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
