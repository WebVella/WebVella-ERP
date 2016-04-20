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
					templateUrl: '/plugins/webvella-admin/sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminEntityListsController',
					templateUrl: '/plugins/webvella-admin/entity-lists.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRecordsList: resolveEntityRecordsList
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////
	checkAccessPermission.$inject = ['$q', '$log', 'resolvedCurrentUser', 'ngToast'];
	
	function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
		var defer = $q.defer();
		var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">Admin</span> area';
		var accessPermission = false;
		for (var i = 0; i < resolvedCurrentUser.roles.length; i++) {
			if (resolvedCurrentUser.roles[i] == "bdc56420-caf0-4030-8a0e-d264938e0cda") {
				accessPermission = true;
			}
		}

		if (accessPermission) {
			defer.resolve();
		}
		else {

			ngToast.create({
				className: 'error',
				content: messageContent,
				timeout: 7000
			});
			defer.reject("No access");
		}

		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		return defer.promise;
	}


	resolveEntityRecordsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveEntityRecordsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
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
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', 'resolvedEntityRecordsList','$timeout'];
	
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $uibModal, resolvedEntityRecordsList,$timeout) {

		
		var ngCtrl = this;
		//#region << Initialize the current entity >>
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Update page title & hide the side menu >>
		ngCtrl.pageTitle = "Entity Details | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		},0);
		$rootScope.adminSectionName = "Entities";

		ngCtrl.showSidebar = function () {
			//Show Sidemenu
			$timeout(function(){
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			},0);
		}
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


		ngCtrl.calculateStats = function (list) {
			var columnsCount = list.columns.length;

			if (columnsCount != 0) {
				return "<span class='go-green'>" + columnsCount + "</span> columns";
			}
			else {
				return "<span class='go-gray'>empty</span>";
			}
		}


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
	createListModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', 'webvellaCoreService'];

	
	function createListModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl, webvellaCoreService) {
		
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
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The view was successfully saved'
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

	copyListModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', 'list', 'webvellaCoreService'];
	
	function copyListModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl, list, webvellaCoreService) {
		
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
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The list was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			ngToast.create({
				className: 'danger',
				content: '<span class="go-red">Error:</span> ' + response.message
			});
		}

	};



})();
