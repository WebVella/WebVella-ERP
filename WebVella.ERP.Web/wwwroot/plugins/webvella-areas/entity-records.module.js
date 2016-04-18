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
		.controller('exportModalController', exportModalController)
		.controller('importModalController', importModalController);



	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-entity-records', {
			parent: 'webvella-areas-base',
			url: '/:listName/:page?search',
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


	//#region << Resolve Function >>

	////////////////////////
 	loadDependency.$inject = ['$ocLazyLoad','$q','$http','$stateParams'];
	function loadDependency($ocLazyLoad, $q, $http,$stateParams){
        var lazyDeferred = $q.defer();

        return $ocLazyLoad.load ({
          name: 'webvellaAreas.recordsList',
          files: ['/plugins/webvella-support/assets/' +  $stateParams.entityName + '/list/' + $stateParams.listName + '/file.js']
        }).then(function() {
           return lazyDeferred.resolve("ready");
        });	
	
	}

 	loadPreloadScript.$inject = ['loadDependency','webvellaActionService','$q','$http','$stateParams'];
	function loadPreloadScript(loadDependency,webvellaActionService, $q, $http,$stateParams){
        var defer = $q.defer();
		webvellaActionService.preload(defer,$stateParams);
	}

	resolveListRecords.$inject = ['$q', '$log', 'webvellaAreasService', '$state', '$stateParams', '$timeout', 'ngToast'];
	/* @ngInject */
	function resolveListRecords($q, $log, webvellaAreasService, $state, $stateParams, $timeout, ngToast) {
		$log.debug('webvellaAreas>entity-records> BEGIN entity list resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();
		// Process get list success
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		if (!$stateParams.search) {
			$stateParams.search = null;
		}

		webvellaAreasService.getListRecords($stateParams.listName, $stateParams.entityName, $stateParams.page, $stateParams.search, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAreas>entity-records> END entity list resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}
	//#endregion


	// Controller ///////////////////////////////
	controller.$inject = ['$filter', '$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaRootService', 'webvellaAdminService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords', 'resolvedCurrentEntityMeta','webvellaActionService',
		'resolvedEntityRelationsList', 'resolvedCurrentUser', 'ngToast', '$sessionStorage', '$location', '$window'];

	/* @ngInject */
	function controller($filter, $log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaRootService, webvellaAdminService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords, resolvedCurrentEntityMeta,webvellaActionService,
		resolvedEntityRelationsList, resolvedCurrentUser, ngToast, $sessionStorage, $location, $window) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var ngCtrl = this;
		ngCtrl.records = fastCopy(resolvedListRecords.data);
		ngCtrl.recordsMeta = fastCopy(resolvedListRecords.meta);
		ngCtrl.relationsMeta = fastCopy(resolvedEntityRelationsList);
		ngCtrl.$sessionStorage = $sessionStorage;
		//#region << Set Environment >>
		ngCtrl.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
		ngCtrl.stateParams = $stateParams;
		webvellaRootService.setBodyColorClass(ngCtrl.currentArea.color);
		ngCtrl.moreListsOpened = false;
		ngCtrl.moreListsInputFocused = false;
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.generalLists = [];
		ngCtrl.entity.recordLists.forEach(function (list) {
			if (list.type == "general") {
				ngCtrl.generalLists.push(list);
			}
		});
		ngCtrl.generalLists.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });


		ngCtrl.area = {};
		for (var i = 0; i < resolvedSitemap.data.length; i++) {
			if (resolvedSitemap.data[i].name == $stateParams.areaName) {
				ngCtrl.area = resolvedSitemap.data[i];
			}
		}


		ngCtrl.area.attachments = angular.fromJson(ngCtrl.area.attachments);
		ngCtrl.areaEntityAttachments = {};
		for (var i = 0; i < ngCtrl.area.attachments.length; i++) {
			if (ngCtrl.area.attachments[i].name === ngCtrl.entity.name) {
				ngCtrl.areaEntityAttachments = ngCtrl.area.attachments[i];
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

		ngCtrl.generateViewName = function (record) {
			//default is the selected view in the area
			var result = fastCopy(ngCtrl.selectedView.name);

			if (ngCtrl.recordsMeta.viewNameOverride && ngCtrl.recordsMeta.viewNameOverride.length > 0) {
				//var arrayOfTemplateKeys = ngCtrl.recordsMeta.viewNameOverride.match(/\{(\w+)\}/g); 
				var arrayOfTemplateKeys = ngCtrl.recordsMeta.viewNameOverride.match(/\{([\$\w]+)\}/g); //Include support for matching also data from relations which include $ symbol
				var resultStringStorage = fastCopy(ngCtrl.recordsMeta.viewNameOverride);

				for (var i = 0; i < arrayOfTemplateKeys.length; i++) {
					if (arrayOfTemplateKeys[i] === "{areaName}" || arrayOfTemplateKeys[i] === "{entityName}" || arrayOfTemplateKeys[i] === "{page}" || arrayOfTemplateKeys[i] === "{searchQuery}") {
						switch (arrayOfTemplateKeys[i]) {
							case "{areaName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.areaName));
								break;
							case "{entityName}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.entityName));
								break;
							case "{page}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.page));
								break;
							case "{searchQuery}":
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug($stateParams.searchQuery));
								break;
						}
					}
					else {
						//Extract the dataName from string by removing the leading and the closing {}
						var dataName = arrayOfTemplateKeys[i].replace('{', '').replace('}', '');
						//Check template has corresponding list data value
						if (record[dataName] != undefined) {
							//YES -> check the value of this dataName and substitute with it in the string, even if it is null (toString)
							//Case 1 - data is not from relation (not starting with $)
							if (!dataName.startsWith('$')) {
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug(record[dataName].toString()));
							}
							else {
								//Case 2 - relation field
								resultStringStorage = resultStringStorage.replace(arrayOfTemplateKeys[i], convertToSlug(record[dataName][0].toString()));
							}

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
		ngCtrl.selectedView = {};
		for (var j = 0; j < ngCtrl.entity.recordViews.length; j++) {
			if (ngCtrl.entity.recordViews[j].name === ngCtrl.areaEntityAttachments.view.name) {
				ngCtrl.selectedView = ngCtrl.entity.recordViews[j];
				break;
			}
		}
		ngCtrl.currentPage = parseInt($stateParams.page);
		//Select the current list view details
		ngCtrl.currentListView = {};
		for (var i = 0; i < ngCtrl.entity.recordLists.length; i++) {
			if (ngCtrl.entity.recordLists[i].name === $stateParams.listName) {
				ngCtrl.currentListView = ngCtrl.entity.recordLists[i];
			}
		}

		ngCtrl.entity.recordLists = ngCtrl.entity.recordLists.sort(function (a, b) {
			return parseFloat(a.weight) - parseFloat(b.weight);
		});

		//#endregion

		//#region << Search >>
		ngCtrl.defaultSearchField = null;
		for (var k = 0; k < ngCtrl.currentListView.columns.length; k++) {
			if (ngCtrl.currentListView.columns[k].type == "field") {
				ngCtrl.defaultSearchField = ngCtrl.currentListView.columns[k];
				break;
			}
		}
		if (ngCtrl.defaultSearchField != null) {
			ngCtrl.searchQueryPlaceholder = "" + ngCtrl.defaultSearchField.meta.label;
		}


		ngCtrl.searchQuery = null;
		if ($stateParams.search) {
			ngCtrl.searchQuery = $stateParams.search;
		}
		ngCtrl.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				ngCtrl.submitSearchQuery();
			}
		}
		ngCtrl.submitSearchQuery = function () {
			$timeout(function () {
				$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, page: 1, search: ngCtrl.searchQuery }, { reload: true });
			}, 1);

		}
		//#endregion

		//#region << filter Query >>
		//ngCtrl.recordsMeta.columns
		ngCtrl.filterQuery = {};
		ngCtrl.listIsFiltered = false;
		ngCtrl.columnDictionary = {};
		ngCtrl.columnDataNamesArray = [];
		ngCtrl.queryParametersArray = [];
		//Extract the available columns
		ngCtrl.recordsMeta.columns.forEach(function (column) {
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
				//some data validations and convertions	
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


		//#region << Logic >> //////////////////////////////////////

		ngCtrl.getRelation = function (relationName) {
			for (var i = 0; i < ngCtrl.relationsMeta.length; i++) {
				if (ngCtrl.relationsMeta[i].name == relationName) {
					//set current entity role
					if (ngCtrl.entity.id == ngCtrl.relationsMeta[i].targetEntityId && ngCtrl.entity.id == ngCtrl.relationsMeta[i].originEntityId) {
						ngCtrl.relationsMeta[i].currentEntityRole = 3; //both origin and target
					}
					else if (ngCtrl.entity.id == ngCtrl.relationsMeta[i].targetEntityId && ngCtrl.entity.id != ngCtrl.relationsMeta[i].originEntityId) {
						ngCtrl.relationsMeta[i].currentEntityRole = 2; //target
					}
					else if (ngCtrl.entity.id != ngCtrl.relationsMeta[i].targetEntityId && ngCtrl.entity.id == ngCtrl.relationsMeta[i].originEntityId) {
						ngCtrl.relationsMeta[i].currentEntityRole = 1; //origin
					}
					else if (ngCtrl.entity.id != ngCtrl.relationsMeta[i].targetEntityId && ngCtrl.entity.id != ngCtrl.relationsMeta[i].originEntityId) {
						ngCtrl.relationsMeta[i].currentEntityRole = 0; //possible problem
					}
					return ngCtrl.relationsMeta[i];
				}
			}
			return null;
		}

		ngCtrl.goDesktopBrowse = function () {
			webvellaRootService.GoToState("webvella-desktop-browse", {});
		}

		ngCtrl.selectPage = function (page) {
			var params = {
				areaName: $stateParams.areaName,
				entityName: $stateParams.entityName,
				listName: $stateParams.listName,
				page: page
			};
			webvellaRootService.GoToState($state, $state.current.name, params);
		}

		ngCtrl.currentUser = fastCopy(resolvedCurrentUser);

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

		ngCtrl.isCurrentListAreaDefault = function () {
			for (var i = 0; i < ngCtrl.area.attachments.length; i++) {
				if (ngCtrl.area.attachments[i].name == ngCtrl.entity.name) {
					if (ngCtrl.area.attachments[i].list.name == ngCtrl.currentListView.name) {
						return true;
					}
					else {
						return false;
					}
				}
			}
		}

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

		ngCtrl.checkEntityPermissions = function (permissionsCsv) {
			return webvellaRootService.userHasEntityPermissions(ngCtrl.entity, permissionsCsv);
		}

		ngCtrl.saveStateParamsToSessionStorage = function () {
			ngCtrl.$sessionStorage["last-list-params"] = $stateParams;
		}


		//#endregion

		//#region << Render >>
		ngCtrl.renderFieldValue = webvellaAreasService.renderFieldValue;
		ngCtrl.getAutoIncrementPrefix = function (column) {
			var keyIndex = column.meta.displayFormat.indexOf('{0}');
			return column.meta.displayFormat.slice(0, keyIndex);
		}

		//#endregion

		//#region << External Template methods >>
		ngCtrl.actionService = webvellaActionService;
		ngCtrl.actionTemplate = '<a ng-click="ngCtrl.actionService.test(ngCtrl.entity.name)">'+
							'<i class="fa fa-wrench"></i> Call test'+
							'</a>'
		//#endregion

		$log.debug('webvellaAreas>entities> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

	exportModalController.$inject = ['$uibModalInstance', '$log', 'webvellaAreasService', 'webvellaAdminService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', '$stateParams', '$scope'];
	function exportModalController($uibModalInstance, $log, webvellaAreasService, webvellaAdminService, ngToast, $timeout, $state, $location, ngCtrl, $stateParams, $scope) {
		$log.debug('webvellaAreas>records>exportModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		var popupCtrl = this;
		popupCtrl.ngCtrl = fastCopy(ngCtrl);
		popupCtrl.loading = false;
		popupCtrl.hasError = false;
		popupCtrl.errorMessage = "";
		popupCtrl.count = -1;
		popupCtrl.countHasSize = true;
		popupCtrl.downloadFilePath = null;

		popupCtrl.count = popupCtrl.ngCtrl.currentListView.pageSize;


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
				webvellaAreasService.exportListRecords(popupCtrl.ngCtrl.entity.name, popupCtrl.ngCtrl.currentListView.name, popupCtrl.count, popupCtrl.exportSuccessCallback, popupCtrl.exportErrorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		$log.debug('webvellaAreas>records>exportModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


	importModalController.$inject = ['$uibModalInstance', '$log', 'webvellaAreasService', 'webvellaAdminService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', '$stateParams', '$scope'];
	function importModalController($uibModalInstance, $log, webvellaAreasService, webvellaAdminService, ngToast, $timeout, $state, $location, ngCtrl, $stateParams, $scope) {
		$log.debug('webvellaAreas>records>importModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
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

				webvellaAdminService.uploadFileToTemp(file, file.name, popupCtrl.uploadProgressCallback, popupCtrl.uploadSuccessCallback, popupCtrl.uploadErrorCallback);

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
				webvellaAreasService.importEntityRecords(popupCtrl.ngCtrl.entity.name, popupCtrl.uploadedFilePath, popupCtrl.importSuccessCallback, popupCtrl.importErrorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		$log.debug('webvellaAreas>records>importModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}
})();
