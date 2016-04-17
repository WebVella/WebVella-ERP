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
					controllerAs: 'contentData'
				}
			},
			resolve: {
				resolvedListRecords: resolveListRecords
			},
			data: {

			}
		});
	};


	//#region << Resolve Function >>

	////////////////////////
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
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords', 'resolvedCurrentEntityMeta',
		'resolvedEntityRelationsList', 'resolvedCurrentUser', 'ngToast', '$sessionStorage', '$location', '$window'];

	/* @ngInject */
	function controller($filter, $log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaRootService, webvellaAdminService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords, resolvedCurrentEntityMeta,
		resolvedEntityRelationsList, resolvedCurrentUser, ngToast, $sessionStorage, $location, $window) {
		$log.debug('webvellaAreas>entities> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		contentData.records = fastCopy(resolvedListRecords.data);
		contentData.recordsMeta = fastCopy(resolvedListRecords.meta);
		contentData.relationsMeta = fastCopy(resolvedEntityRelationsList);
		contentData.$sessionStorage = $sessionStorage;
		//#region << Set Environment >>
		contentData.pageTitle = "Area Entities | " + pageTitle;
		webvellaRootService.setPageTitle(contentData.pageTitle);
		contentData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
		contentData.stateParams = $stateParams;
		webvellaRootService.setBodyColorClass(contentData.currentArea.color);
		contentData.moreListsOpened = false;
		contentData.moreListsInputFocused = false;
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		contentData.generalLists = [];
		contentData.entity.recordLists.forEach(function (list) {
			if (list.type == "general") {
				contentData.generalLists.push(list);
			}
		});
		contentData.generalLists.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });


		contentData.area = {};
		for (var i = 0; i < resolvedSitemap.data.length; i++) {
			if (resolvedSitemap.data[i].name == $stateParams.areaName) {
				contentData.area = resolvedSitemap.data[i];
			}
		}


		contentData.area.attachments = angular.fromJson(contentData.area.attachments);
		contentData.areaEntityAttachments = {};
		for (var i = 0; i < contentData.area.attachments.length; i++) {
			if (contentData.area.attachments[i].name === contentData.entity.name) {
				contentData.areaEntityAttachments = contentData.area.attachments[i];
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
			var result = fastCopy(contentData.selectedView.name);

			if (contentData.recordsMeta.viewNameOverride && contentData.recordsMeta.viewNameOverride.length > 0) {
				//var arrayOfTemplateKeys = contentData.recordsMeta.viewNameOverride.match(/\{(\w+)\}/g); 
				var arrayOfTemplateKeys = contentData.recordsMeta.viewNameOverride.match(/\{([\$\w]+)\}/g); //Include support for matching also data from relations which include $ symbol
				var resultStringStorage = fastCopy(contentData.recordsMeta.viewNameOverride);

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
		contentData.selectedView = {};
		for (var j = 0; j < contentData.entity.recordViews.length; j++) {
			if (contentData.entity.recordViews[j].name === contentData.areaEntityAttachments.view.name) {
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

		contentData.entity.recordLists = contentData.entity.recordLists.sort(function (a, b) {
			return parseFloat(a.weight) - parseFloat(b.weight);
		});

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
				$state.go("webvella-entity-records", { areaName: $stateParams.areaName, entityName: $stateParams.entityName, listName: $stateParams.listName, page: 1, search: contentData.searchQuery }, { reload: true });
			}, 1);

		}
		//#endregion

		//#region << filter Query >>
		//contentData.recordsMeta.columns
		contentData.filterQuery = {};
		contentData.listIsFiltered = false;
		contentData.columnDictionary = {};
		contentData.columnDataNamesArray = [];
		contentData.queryParametersArray = [];
		//Extract the available columns
		contentData.recordsMeta.columns.forEach(function (column) {
			if (contentData.columnDataNamesArray.indexOf(column.dataName) == -1) {
				contentData.columnDataNamesArray.push(column.dataName);
			}
			contentData.columnDictionary[column.dataName] = column;
		});
		//Extract available url query strings
		var queryObject = $location.search();
		for (var key in queryObject) {
			if (contentData.queryParametersArray.indexOf(key) == -1) {
				contentData.queryParametersArray.push(key);
			}
		}

		contentData.columnDataNamesArray.forEach(function (dataName) {
			if (contentData.queryParametersArray.indexOf(dataName) > -1) {
				contentData.listIsFiltered = true;
				var columnObj = contentData.columnDictionary[dataName];
				//some data validations and conversions	
				switch (columnObj.meta.fieldType) {
					//TODO if percent convert to > 1 %
					case 14:
						if (checkDecimal(queryObject[dataName])) {
							contentData.filterQuery[dataName] = queryObject[dataName] * 100;
						}
						break;
					default:
						contentData.filterQuery[dataName] = queryObject[dataName];
						break;

				}
			}
		});

		contentData.clearQueryFilter = function () {
			for (var activeFilter in contentData.filterQuery) {
				$location.search(activeFilter, null);
			}
			$window.location.reload();
		}

		contentData.applyQueryFilter = function () {
			//TODO - Convert percent into 0 < x < 1

		}

		//#endregion


		//#region << Logic >> //////////////////////////////////////

		contentData.getRelation = function (relationName) {
			for (var i = 0; i < contentData.relationsMeta.length; i++) {
				if (contentData.relationsMeta[i].name == relationName) {
					//set current entity role
					if (contentData.entity.id == contentData.relationsMeta[i].targetEntityId && contentData.entity.id == contentData.relationsMeta[i].originEntityId) {
						contentData.relationsMeta[i].currentEntityRole = 3; //both origin and target
					}
					else if (contentData.entity.id == contentData.relationsMeta[i].targetEntityId && contentData.entity.id != contentData.relationsMeta[i].originEntityId) {
						contentData.relationsMeta[i].currentEntityRole = 2; //target
					}
					else if (contentData.entity.id != contentData.relationsMeta[i].targetEntityId && contentData.entity.id == contentData.relationsMeta[i].originEntityId) {
						contentData.relationsMeta[i].currentEntityRole = 1; //origin
					}
					else if (contentData.entity.id != contentData.relationsMeta[i].targetEntityId && contentData.entity.id != contentData.relationsMeta[i].originEntityId) {
						contentData.relationsMeta[i].currentEntityRole = 0; //possible problem
					}
					return contentData.relationsMeta[i];
				}
			}
			return null;
		}

		contentData.goDesktopBrowse = function () {
			webvellaRootService.GoToState("webvella-desktop-browse", {});
		}

		contentData.selectPage = function (page) {
			var params = {
				areaName: $stateParams.areaName,
				entityName: $stateParams.entityName,
				listName: $stateParams.listName,
				page: page
			};
			webvellaRootService.GoToState($state, $state.current.name, params);
		}

		contentData.currentUser = fastCopy(resolvedCurrentUser);

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

		contentData.isCurrentListAreaDefault = function () {
			for (var i = 0; i < contentData.area.attachments.length; i++) {
				if (contentData.area.attachments[i].name == contentData.entity.name) {
					if (contentData.area.attachments[i].list.name == contentData.currentListView.name) {
						return true;
					}
					else {
						return false;
					}
				}
			}
		}

		contentData.exportModal = undefined;
		contentData.openExportModal = function () {
			contentData.exportModal = $uibModal.open({
				animation: false,
				templateUrl: 'exportModalContent.html',
				controller: 'exportModalController',
				controllerAs: "popupData",
				//size: "lg",
				resolve: {
					contentData: function () {
						return contentData;
					}
				}
			});
		}
		//Close the modal on state change
		$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
			if (contentData.exportModal) {
				contentData.exportModal.dismiss();
			}
		})

		contentData.importModal = undefined;
		contentData.openImportModal = function () {
			contentData.importModal = $uibModal.open({
				animation: false,
				templateUrl: 'importModalContent.html',
				controller: 'importModalController',
				controllerAs: "popupData",
				//size: "lg",
				resolve: {
					contentData: function () {
						return contentData;
					}
				}
			});
		}
		//Close the modal on state change
		$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
			if (contentData.importModal) {
				contentData.importModal.dismiss();
			}
		})

		contentData.checkEntityPermissions = function (permissionsCsv) {
			return webvellaRootService.userHasEntityPermissions(contentData.entity, permissionsCsv);
		}

		contentData.saveStateParamsToSessionStorage = function () {
			contentData.$sessionStorage["last-list-params"] = $stateParams;
		}


		//#endregion

		//#region << Render >>
		contentData.renderFieldValue = webvellaAreasService.renderFieldValue;
		contentData.getAutoIncrementPrefix = function (column) {
			var keyIndex = column.meta.displayFormat.indexOf('{0}');
			return column.meta.displayFormat.slice(0, keyIndex);
		}

		//#endregion
		$log.debug('webvellaAreas>entities> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

	exportModalController.$inject = ['$uibModalInstance', '$log', 'webvellaAreasService', 'webvellaAdminService', 'ngToast', '$timeout', '$state', '$location', 'contentData', '$stateParams', '$scope'];
	function exportModalController($uibModalInstance, $log, webvellaAreasService, webvellaAdminService, ngToast, $timeout, $state, $location, contentData, $stateParams, $scope) {
		$log.debug('webvellaAreas>records>exportModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		var popupData = this;
		popupData.contentData = fastCopy(contentData);
		popupData.loading = false;
		popupData.hasError = false;
		popupData.errorMessage = "";
		popupData.count = -1;
		popupData.countHasSize = true;
		popupData.downloadFilePath = null;

		popupData.count = popupData.contentData.currentListView.pageSize;


		popupData.exportSuccessCallback = function (response) {
			//popupData.loading = false;
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success </span> Records successfully exported!'
			});
			popupData.downloadFilePath = response.object;

		}
		popupData.exportErrorCallback = function (response) {
			popupData.loading = false;
			//popupData.hasError = true;
			//popupData.errorMessage = response.message;
		}

		popupData.ok = function () {
			popupData.loading = true;
			popupData.hasError = false;
			popupData.errorMessage = "";
			if (popupData.count == 0) {
				popupData.hasError = true;
				popupData.loading = false;
				popupData.errorMessage = "Records export count could not be 0";
			}
			else {
				if (!popupData.countHasSize) {
					popupData.count = -1;
				}
				webvellaAreasService.exportListRecords(popupData.contentData.entity.name, popupData.contentData.currentListView.name, popupData.count, popupData.exportSuccessCallback, popupData.exportErrorCallback);
			}
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		$log.debug('webvellaAreas>records>exportModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


	importModalController.$inject = ['$uibModalInstance', '$log', 'webvellaAreasService', 'webvellaAdminService', 'ngToast', '$timeout', '$state', '$location', 'contentData', '$stateParams', '$scope'];
	function importModalController($uibModalInstance, $log, webvellaAreasService, webvellaAdminService, ngToast, $timeout, $state, $location, contentData, $stateParams, $scope) {
		$log.debug('webvellaAreas>records>importModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		var popupData = this;
		popupData.contentData = fastCopy(contentData);
		popupData.uploadedFile = null;
		popupData.uploadedFilePath = null;
		popupData.uploadProgress = 0;
		popupData.loading = false;
		popupData.hasError = false;
		popupData.errorMessage = "";

		popupData.upload = function (file) {
			popupData.uploadedFilePath = null;
			popupData.uploadProgress = 0;

			if (file != null) {
				popupData.uploadSuccessCallback = function (response) {
					popupData.uploadedFilePath = response.object.url;
				}
				popupData.uploadErrorCallback = function (response) {
					alert(response.message);
				}
				popupData.uploadProgressCallback = function (response) {
					$timeout(function () {
						popupData.uploadProgress = parseInt(100.0 * response.loaded / response.total);
					}, 100);
				}

				webvellaAdminService.uploadFileToTemp(file, file.name, popupData.uploadProgressCallback, popupData.uploadSuccessCallback, popupData.uploadErrorCallback);

			}
		}

		popupData.deleteFileUpload = function () {
			$timeout(function () {
				popupData.uploadedFile = null;
				popupData.uploadedFilePath = null;
				popupData.uploadProgress = 0;
			}, 100);
		}

		popupData.importSuccessCallback = function (response) {
			//popupData.loading = false;
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success </span> Records successfully imported!'
			});
			//$uibModalInstance.dismiss('cancel');
			$state.reload();
		}
		popupData.importErrorCallback = function (response) {
			popupData.loading = false;
			//popupData.hasError = true;
			//popupData.errorMessage = response.message;
		}

		popupData.ok = function () {
			popupData.loading = true;
			popupData.hasError = false;
			popupData.errorMessage = "";

			if (popupData.uploadedFilePath == null || popupData.uploadedFilePath == "") {
				popupData.loading = false;
				popupData.hasError = true;
				popupData.errorMessage = "You need to upload a CSV file first";
			}
			else if (!popupData.uploadedFile.name.endsWith(".csv")) {
				popupData.loading = false;
				popupData.hasError = true;
				popupData.errorMessage = "This is not a CSV file";
			}
			else {
				webvellaAreasService.importEntityRecords(popupData.contentData.entity.name, popupData.uploadedFilePath, popupData.importSuccessCallback, popupData.importErrorCallback);
			}
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		$log.debug('webvellaAreas>records>importModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}
})();
