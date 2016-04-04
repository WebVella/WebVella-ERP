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

	/* @ngInject */
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
					controllerAs: 'contentData'
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
	/* @ngInject */
	function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
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

		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}


	resolveEntityRecordsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveEntityRecordsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

		webvellaAdminService.getEntityLists($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}
	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', 'resolvedEntityRecordsList'];
	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $uibModal, resolvedEntityRecordsList) {
		$log.debug('webvellaAdmin>entity-records-list> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		//#region << Initialize the current entity >>
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Update page title & hide the side menu >>
		contentData.pageTitle = "Entity Details | " + pageTitle;
		$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
		//Hide Sidemenu
		$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));

		contentData.showSidebar = function () {
			//Show Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		}
		//#endregion

		//#region << Initialize the lists >>
		contentData.lists = fastCopy(resolvedEntityRecordsList.recordLists);
		contentData.lists.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});
		//#endregion

		//Create new list modal
		contentData.createListModalInstance = undefined;
		contentData.createListModal = function () {
			contentData.createListModalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createListModal.html',
				controller: 'createListModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () { return contentData; }
				}
			});
		}
		//Close the modal on state change
		$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
			if (contentData.createListModalInstance) {
				contentData.createListModalInstance.dismiss();
			}
		})


		contentData.calculateStats = function (list) {
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
			contentData.createListModal();
		}

		contentData.copyList = function (list) {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'copyModal.html',
				controller: 'copyListModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					contentData: function () {
						return contentData;
					},
					list: function () {
						return list;
					}
				}
			});

		}


		$log.debug('webvellaAdmin>entity-records-list> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}
	//#endregion

	//// Modal Controllers
	createListModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'contentData', 'webvellaAdminService', 'webvellaRootService'];

	/* @ngInject */
	function createListModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, contentData, webvellaAdminService, webvellaRootService) {
		$log.debug('webvellaAdmin>entities>createViewModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.modalInstance = $uibModalInstance;
		popupData.contentData = fastCopy(contentData);
		popupData.list = webvellaAdminService.initList();
		//Check if there is an id column set, if not include it as it always should be there

		var idFieldGuid = null;
		for (var j = 0; j < popupData.contentData.entity.fields.length; j++) {
			if (popupData.contentData.entity.fields[j].name == "id") {
				idFieldGuid = popupData.contentData.entity.fields[j].id;
			}
		}
		//The Record Id data is automatically injected by the server. If you want the field to be visible to users you need to add it in the view

		popupData.ok = function () {
			webvellaAdminService.createEntityList(popupData.list, popupData.contentData.entity.name, successCallback, errorCallback);
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The view was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaRootService.GoToState($state, $state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaRootService.generateValidationMessages(response, popupData, popupData.entity, location);
		}

		$log.debug('webvellaAdmin>entities>createViewModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	copyListModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'contentData', 'list', 'webvellaAdminService', 'webvellaRootService'];
	/* @ngInject */
	function copyListModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, contentData, list, webvellaAdminService, webvellaRootService) {
		$log.debug('webvellaAdmin>entities>copyListModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.modalInstance = $uibModalInstance;
		popupData.list = fastCopy(list);
		popupData.currentEntity = fastCopy(contentData.entity);
		popupData.alternative = "new";
		popupData.listName = null;
		popupData.listNameValidationError = false;

		popupData.entityLists = []; //filter the current view

		for (var i = 0; i < popupData.currentEntity.recordLists.length; i++) {
			if (popupData.currentEntity.recordLists[i].name != popupData.list.name) {
				popupData.entityLists.push(popupData.currentEntity.recordLists[i]);
			}
		}

		popupData.selectedList = popupData.entityLists[0];

		popupData.ok = function () {
			popupData.listNameValidationError = false;
			if (popupData.alternative == "new") {
				if (popupData.listName == null || popupData.listName == "") {
					popupData.listNameValidationError = true;
				}
				else {
					var newList = fastCopy(popupData.list);
					newList.id = null;
					newList.name = popupData.listName;
					newList.label = popupData.listName;
					webvellaAdminService.createEntityList(newList, popupData.currentEntity.name, successCallback, errorCallback);
				}
			}
			else {
				var newList = fastCopy(popupData.list);
				var oldList = fastCopy(popupData.selectedList);
				oldList.viewNameOverride = newList.viewNameOverride;
				oldList.visibleColumnsCount = newList.visibleColumnsCount;
				oldList.pageSize = newList.pageSize;
				oldList.columns = newList.columns;
				oldList.query = newList.query;
				oldList.sorts = newList.sorts;
				oldList.relationOptions = newList.relationOptions;
				webvellaAdminService.updateEntityList(oldList, popupData.currentEntity.name, successCallback, errorCallback);
			}
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The list was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaRootService.GoToState($state, $state.current.name, {});
		}

		function errorCallback(response) {
			ngToast.create({
				className: 'danger',
				content: '<span class="go-red">Error:</span> ' + response.message
			});
		}

		$log.debug('webvellaAdmin>entities>copyListModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};



})();
