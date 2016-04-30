/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListsController', controller)
		.controller('createListModalController', createListModalController)
		.controller('copyListModalController', copyListModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-lists', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/lists?createNew', //  /desktop/areas after the parent state is prepended
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
					controller: 'WebVellaAdminEntityListsController',
					templateUrl: '/plugins/webvella-admin/entity-lists.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRecordsList: resolveEntityRecordsList
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////
	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
	
	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {
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

		webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		return defer.promise;
	}


	resolveEntityRecordsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
	
	function resolveEntityRecordsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {
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

		webvellaCoreService.getEntityLists($stateParams.entityName, successCallback, errorCallback);
		return defer.promise;
	}
	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', 'resolvedEntityRecordsList','$timeout','$translate'];
	
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $uibModal, resolvedEntityRecordsList,$timeout,$translate) {

		
		var ngCtrl = this;
		//#region << Initialize the current entity >>
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Update page title & hide the side menu >>
		$translate(['RECORD_LISTS_PAGE_TITLE','ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_LISTS_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#endregion

		//#region << Initialize the lists >>
		ngCtrl.lists = fastCopy(resolvedEntityRecordsList.recordLists);
		ngCtrl.lists.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});
		//#endregion

		//Create new list modal
		ngCtrl.createListModalInstance = undefined;
		ngCtrl.createListModal = function () {
			ngCtrl.createListModalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createListModal.html',
				controller: 'createListModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () { return ngCtrl; }
				}
			});
		}
		//Close the modal on state change
		$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
			if (ngCtrl.createListModalInstance) {
				ngCtrl.createListModalInstance.dismiss();
			}
		})

		//Check if the param createNewList is true if yes - open the modal
		if ($state.params.createNew) {
			ngCtrl.createListModal();
		}

		ngCtrl.copyList = function (list) {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'copyModal.html',
				controller: 'copyListModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					list: function () {
						return list;
					}
				}
			});

		}


	}
	//#endregion

	//// Modal Controllers
	createListModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', 'webvellaCoreService','$translate'];

	
	function createListModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl, webvellaCoreService,$translate) {
		
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.ngCtrl = fastCopy(ngCtrl);
		popupCtrl.list = webvellaCoreService.initList();
		//Check if there is an id column set, if not include it as it always should be there

		var idFieldGuid = null;
		for (var j = 0; j < popupCtrl.ngCtrl.entity.fields.length; j++) {
			if (popupCtrl.ngCtrl.entity.fields[j].name == "id") {
				idFieldGuid = popupCtrl.ngCtrl.entity.fields[j].id;
			}
		}
		//The Record Id data is automatically injected by the server. If you want the field to be visible to users you need to add it in the view

		popupCtrl.ok = function () {
			webvellaCoreService.createEntityList(popupCtrl.list, popupCtrl.ngCtrl.entity.name, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL','RECORD_VIEW_SAVE_SUCCESS_MESSAGE']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.RECORD_VIEW_SAVE_SUCCESS_MESSAGE
				});
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.entity, location);
		}

	};

	copyListModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', 'list', 'webvellaCoreService','$translate'];
	
	function copyListModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl, list, webvellaCoreService,$translate) {
		
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.list = fastCopy(list);
		popupCtrl.currentEntity = fastCopy(ngCtrl.entity);
		popupCtrl.alternative = "new";
		popupCtrl.listName = null;
		popupCtrl.listNameValidationError = false;

		popupCtrl.entityLists = []; //filter the current view

		for (var i = 0; i < popupCtrl.currentEntity.recordLists.length; i++) {
			if (popupCtrl.currentEntity.recordLists[i].name != popupCtrl.list.name) {
				popupCtrl.entityLists.push(popupCtrl.currentEntity.recordLists[i]);
			}
		}

		popupCtrl.selectedList = popupCtrl.entityLists[0];

		popupCtrl.ok = function () {
			popupCtrl.listNameValidationError = false;
			if (popupCtrl.alternative == "new") {
				if (popupCtrl.listName == null || popupCtrl.listName == "") {
					popupCtrl.listNameValidationError = true;
				}
				else {
					var newList = fastCopy(popupCtrl.list);
					newList.id = null;
					newList.name = popupCtrl.listName;
					newList.label = popupCtrl.listName;
					webvellaCoreService.createEntityList(newList, popupCtrl.currentEntity.name, successCallback, errorCallback);
				}
			}
			else {
				var newList = fastCopy(popupCtrl.list);
				var oldList = fastCopy(popupCtrl.selectedList);
				oldList.visibleColumnsCount = newList.visibleColumnsCount;
				oldList.pageSize = newList.pageSize;
				oldList.columns = newList.columns;
				oldList.query = newList.query;
				oldList.sorts = newList.sorts;
				oldList.relationOptions = newList.relationOptions;
				webvellaCoreService.updateEntityList(oldList, popupCtrl.currentEntity.name, successCallback, errorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL','RECORD_VIEW_SAVE_COPIED_MESSAGE']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.RECORD_VIEW_SAVE_COPIED_MESSAGE
				});
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}

	};



})();
