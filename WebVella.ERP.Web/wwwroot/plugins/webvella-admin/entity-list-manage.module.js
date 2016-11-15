/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListManageController', controller)
        .controller('DeleteListModalController', deleteListModalController)			

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-list-manage', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/lists/:listName', //  /desktop/areas after the parent state is prepended
			views: {
				"topnavView": {
					controller: 'WebVellaAdminTopnavController',
					templateUrl: '/plugins/webvella-admin/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAdminSidebarController',
					templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminEntityListManageController',
					templateUrl: '/plugins/webvella-admin/entity-list-manage.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedViewLibrary: resolveViewLibrary,
				resolvedEntityRelationsList: resolveEntityRelationsList,
				resolvedEntityList:resolveEntityList
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////

	resolveViewLibrary.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
	
	function resolveViewLibrary($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {

		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityViewLibrary($stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
	
	function resolveEntityRelationsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {

		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getRelationsList(successCallback, errorCallback);
		return defer.promise;
	}
	
 	resolveEntityList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams'];
	function resolveEntityList($q, $log, webvellaCoreService, $state, $stateParams) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getEntityMetaList(successCallback, errorCallback);
		return defer.promise;
	}
	
	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$timeout', 'ngToast', 'pageTitle', 'resolvedEntityList', '$uibModal',
						'resolvedViewLibrary', 'webvellaCoreService', 'resolvedEntityRelationsList','$translate','$stateParams'];
	
	function controller($scope, $log, $rootScope, $state, $timeout, ngToast, pageTitle, resolvedEntityList, $uibModal,
						resolvedViewLibrary, webvellaCoreService, resolvedEntityRelationsList,$translate,$stateParams) {

		
		var ngCtrl = this;
		ngCtrl.entity = webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName,resolvedEntityList);
		//Awesome font icon names array 
		ngCtrl.icons = getFontAwesomeIconNames();

		//#region << Update page title & hide the side menu >>
		$translate(['RECORD_LIST_TAB_BASIC_PAGE_TITLE','ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_LIST_TAB_BASIC_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		//Hide Sidemenu
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#endregion

		//#region << List types >>
		ngCtrl.listTypeOptions = [];
		$translate(['GENERAL','LOOKUP','HIDDEN']).then(function (translations) {
			ngCtrl.listTypeOptions = [
				{
            		key: "general",
            		value: translations.GENERAL
				},
				{
            		key: "lookup",
            		value: translations.LOOKUP
				},
				{
            		key: "hidden",
            		value: translations.HIDDEN
				}
			];
		});
		//#endregion

 		//#region << Initialize the list >>
		ngCtrl.list = webvellaCoreService.getEntityRecordListFromEntitiesMetaList($stateParams.listName,$stateParams.entityName,resolvedEntityList);
		ngCtrl.relationsList = resolvedEntityRelationsList;

		ngCtrl.defaultFieldName = null;
		function calculateDefaultSearchFieldName() {
			var name = null;
			for (var k = 0; k < ngCtrl.list.columns.length; k++) {
				if (ngCtrl.list.columns[k].type === "field") {
					name = ngCtrl.list.columns[k].meta.name;
					break;
				}
			}
			ngCtrl.defaultFieldName = name;
		}
		calculateDefaultSearchFieldName();

		function patchFieldSuccessCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + response.message
				});
			});
			webvellaCoreService.regenerateAllAreaAttachments();
		}

		function patchSuccessCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			ngCtrl.list = response.object;
			ngCtrl.generateAlreadyUsed();
		}
		function patchErrorCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}

		ngCtrl.fieldUpdate = function (fieldName, data) {
			if(fieldName == "dataSourceUrl" && data == ""){
				data = null;
			}
			if(fieldName == "dynamicHtmlTemplate" && data == ""){
				data = null;
			}
			var postObj = {};
			postObj[fieldName] = data;
			webvellaCoreService.patchEntityRecordList(postObj, ngCtrl.list.name, ngCtrl.entity.name, patchFieldSuccessCallback, patchErrorCallback)
		}


		//#endregion

		//#region << Initialize the library >>

		//Generate already used
		var alreadyUsedItemDataNames = [];
		ngCtrl.generateAlreadyUsed = function () {
			alreadyUsedItemDataNames = [];
			for (var i = 0; i < ngCtrl.list.columns.length; i++) {
				if (ngCtrl.list.columns[i].meta) {
					alreadyUsedItemDataNames.push(ngCtrl.list.columns[i].dataName);
				}
			}
		}
		ngCtrl.generateAlreadyUsed();
		ngCtrl.fullLibrary = {};
		ngCtrl.fullLibrary.items = resolvedViewLibrary;
		//Fields list eligable to be options in the sort and query dropdowns
		ngCtrl.onlyFieldsLibrary = {};
		ngCtrl.onlyFieldsLibrary.items = [];
		ngCtrl.library = {};
		ngCtrl.library.relations = [];
		ngCtrl.library.items = [];

		ngCtrl.sortLibrary = function () {
			ngCtrl.library.items.sort(sort_by("type","fieldName"));
		}
		ngCtrl.sortOnlyFieldsLibrary = function () {
			ngCtrl.onlyFieldsLibrary.items = ngCtrl.onlyFieldsLibrary.items.sort(function (a, b) {
				if (a.fieldName < b.fieldName) return -1;
				if (a.fieldName > b.fieldName) return 1;
				return 0;
			});
		}
		ngCtrl.checkIfRelationAddedToLibrary = function (relationName) {
			if (ngCtrl.library.relations.length > 0) {
				for (var i = 0; i < ngCtrl.library.relations.length; i++) {
					if (ngCtrl.library.relations[i].relationName === relationName && ngCtrl.library.relations[i].addedToLibrary) {
						return true;
					}
				}
				return false;
			}
			else {
				return false;
			}
		}


		ngCtrl.generateLibrary = function (generateRelationOptions) {
			ngCtrl.library.items = [];
			ngCtrl.onlyFieldsLibrary = {};
			ngCtrl.onlyFieldsLibrary.items = [];
			if (generateRelationOptions) {
				ngCtrl.library.relations = [];
			}
			ngCtrl.fullLibrary.items.forEach(function (item) {
				if ((item.meta && alreadyUsedItemDataNames.indexOf(item.dataName) == -1) || !item.meta) {
					switch (item.type) {
						case "field":
							ngCtrl.library.items.push(item);
							break;
						case "relationOptions":
							if (generateRelationOptions) {
								item.addedToLibrary = false;
								item.sameOriginTargetEntity = false;
								for (var r = 0; r < ngCtrl.relationsList.length; r++) {
									if (item.relationName == ngCtrl.relationsList[r].name && ngCtrl.relationsList[r].originEntityId == ngCtrl.relationsList[r].targetEntityId) {
										item.sameOriginTargetEntity = true;
									}
								}
								ngCtrl.library.relations.push(item);
							}
							break;
						case "view":
							ngCtrl.library.items.push(item);
							break;
						case "list":
							if (item.listId != ngCtrl.list.id) {
								ngCtrl.library.items.push(item);
							}
							break;
						case "fieldFromRelation":
							if(ngCtrl.checkIfRelationAddedToLibrary(item.relationName)){
								ngCtrl.library.items.push(item);
							}
							break;
						case "viewFromRelation":
							if(ngCtrl.checkIfRelationAddedToLibrary(item.relationName)){
								ngCtrl.library.items.push(item);
							}
							break;
						case "listFromRelation":
							if(ngCtrl.checkIfRelationAddedToLibrary(item.relationName)){
								ngCtrl.library.items.push(item);
							}
							break;
						case "treeFromRelation":
							if(ngCtrl.checkIfRelationAddedToLibrary(item.relationName)){
								ngCtrl.library.items.push(item);
							}
							break;
					}
				}
				if (item.type == "field") {
					ngCtrl.onlyFieldsLibrary.items.push(item);
				}
			});
			ngCtrl.sortLibrary();
			ngCtrl.sortOnlyFieldsLibrary();
		}

		ngCtrl.generateLibrary(true);

		//Extract the direction change information from the list if present
		for (var k = 0; k < ngCtrl.list.relationOptions.length; k++) {
			for (var m = 0; m < ngCtrl.library.relations.length; m++) {
				if (ngCtrl.list.relationOptions[k].relationName == ngCtrl.library.relations[m].relationName) {
					ngCtrl.library.relations[m].direction = ngCtrl.list.relationOptions[k].direction;
				}

			}

		}

		ngCtrl.library.relations = ngCtrl.library.relations.sort(function (a, b) {
			if (a.relationName < b.relationName) return -1;
			if (a.relationName > b.relationName) return 1;
			return 0;
		});


		//#endregion

		//#region << Regenerate library >>
		ngCtrl.regenerateLibrary = function () {
			ngCtrl.generateAlreadyUsed();
			ngCtrl.generateLibrary(false);
		}

		//#endregion

		//#region << Logic >>
		ngCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;

		//Delete list
		ngCtrl.deleteListModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'deleteListModal.html',
				controller: 'DeleteListModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentData: function () { return ngCtrl; }
				}
			});
		}

		//#endregion
	}
	//#endregion

	//#region << Modal Controllers >>
	deleteListModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state','$translate'];
	
	function deleteListModalController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $translate) {
		
		var popupCtrl = this;
		popupCtrl.parentData = parentData;

		popupCtrl.ok = function () {

			webvellaCoreService.deleteEntityRecordList(popupCtrl.parentData.list.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);

		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			$uibModalInstance.close('success');
			$timeout(function () {
				$state.go("webvella-admin-entity-lists", { entityName: popupCtrl.parentData.entity.name }, { reload: true });
			}, 0);
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;
			popupCtrl.errors = response.errors;
		}
	};


	//#endregion

})();
