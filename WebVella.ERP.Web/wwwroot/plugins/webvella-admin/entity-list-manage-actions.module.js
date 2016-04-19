/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
		.controller('DeleteListModalController', deleteListModalController)
		.controller('ManageListServiceCodeModalController', ManageListServiceCodeModalController)
		.controller('AddManageListMenuItemModalController', AddManageListMenuItemModalController)
        .controller('WebVellaAdminEntityListManageMenuController', controller);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-list-manage-actions', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/lists/:listName/actions', //  /desktop/areas after the parent state is prepended
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
					controller: 'WebVellaAdminEntityListManageMenuController',
					templateUrl: '/plugins/webvella-admin/entity-list-manage-actions.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedViewLibrary: resolveViewLibrary,
				resolvedCurrentEntityList: resolveCurrentEntityList,
				resolvedEntityRelationsList: resolveEntityRelationsList,
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

	resolveCurrentEntityList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentEntityList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
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

		webvellaAdminService.getEntityList($stateParams.listName, $stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-records-list>resolveEntityRecordsList END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveViewLibrary.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveViewLibrary($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-views>resolveViewAvailableItems BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

		webvellaAdminService.getEntityViewLibrary($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-views>resolveViewAvailableItems END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveEntityRelationsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
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

		webvellaAdminService.getRelationsList(successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}
	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$timeout', 'ngToast', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', 'resolvedCurrentEntityList',
						'resolvedViewLibrary', 'webvellaAdminService', 'webvellaAreasService', 'resolvedEntityRelationsList'];
	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, $timeout, ngToast, pageTitle, resolvedCurrentEntityMeta, $uibModal, resolvedCurrentEntityList,
						resolvedViewLibrary, webvellaAdminService, webvellaAreasService, resolvedEntityRelationsList) {
		$log.debug('webvellaAdmin>entity-records-list> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var ngCtrl = this;
		ngCtrl.entity = resolvedCurrentEntityMeta;

		//#region << Update page title & hide the side menu >>
		ngCtrl.pageTitle = "Entity Details | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);

		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#endregion


 		//#region << Initialize the list >>
		ngCtrl.list = fastCopy(resolvedCurrentEntityList);
		ngCtrl.relationsList = fastCopy(resolvedEntityRelationsList);
		//#endregion
	

		//#region << Modals >>
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

		ngCtrl.manageServiceCodeModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageServiceCodeModal.html',
				controller: 'ManageListServiceCodeModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					parentData: function () { return ngCtrl; }
				}
			});
		}

		ngCtrl.addManageMenuItemModal = function (menuItem) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'addManageMenuItemModal.html',
				controller: 'AddManageListMenuItemModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					parentData: function () { return ngCtrl; },
					menuItem: function () { return menuItem; }
				}
			});
		}
		//#endregion


		$log.debug('webvellaAdmin>entity-records-list> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}
	//#endregion


	//#region << Modal Controllers >>
	deleteListModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function deleteListModalController(parentData, $uibModalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
		$log.debug('webvellaAdmin>entities>deleteListModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.parentData = parentData;

		popupCtrl.ok = function () {

			webvellaAdminService.deleteEntityList(popupCtrl.parentData.list.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);

		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
			$timeout(function () {
				$state.go("webvella-admin-entity-lists", { entityName: popupCtrl.parentData.entity.name }, { reload: true });
			}, 0);
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}
		$log.debug('webvellaAdmin>entities>deleteListModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};
	
	ManageListServiceCodeModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function ManageListServiceCodeModalController(parentData, $uibModalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
		$log.debug('webvellaAdmin>entities>ManageViewServiceCodeModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.parentData = parentData;
		popupCtrl.serviceCode = popupCtrl.parentData.list.serviceCode;

		//#region << Ace editor >>
		popupCtrl.aceOptions = {
			useWrapMode: true,
			showGutter: true,
			theme: 'twilight',
			mode: 'javascript',
			firstLineNumber: 1,
			onLoad: popupCtrl.aceOnLoad,
			onChange: popupCtrl.aceOnChange,
			advanced:{
				showPrintMargin:false
			}
		}
		popupCtrl.aceOnChange = function (event) {};
		//#endregion

		popupCtrl.ok = function () {
			var postObj = {};
			postObj.serviceCode = fastCopy(popupCtrl.serviceCode);
			webvellaAdminService.patchEntityList(postObj, popupCtrl.parentData.list.name, popupCtrl.parentData.entity.name, successCallback, errorCallback)
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			parentData.list.serviceCode = response.object.serviceCode;
			$uibModalInstance.close('success');
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}
		$log.debug('webvellaAdmin>entities>ManageViewServiceCodeModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	AddManageListMenuItemModalController.$inject = ['parentData','menuItem', '$uibModalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function AddManageListMenuItemModalController(parentData,menuItem, $uibModalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
		$log.debug('webvellaAdmin>entities>ManageViewServiceCodeModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.parentData = parentData;
		popupCtrl.menuItem = {};
		popupCtrl.isEdit = false;
		if(menuItem != null){
			popupCtrl.isEdit = false;
			popupCtrl.menuItem = fastCopy(menuItem);
		}
		else {
		  popupCtrl.menuItem = webvellaAdminService.initListMenuItem();
		  popupCtrl.menuItem.menu = "hidden";
		  popupCtrl.menuItem.template = "";
		}
		

		//#region << Ace editor >>
		popupCtrl.aceOptions = {
			useWrapMode: true,
			showGutter: true,
			theme: 'twilight',
			mode: 'xml',
			firstLineNumber: 1,
			onLoad: popupCtrl.aceOnLoad,
			onChange: popupCtrl.aceOnChange,
			advanced:{
				showPrintMargin:false
			}
		}
		popupCtrl.aceOnChange = function (event) {};
		//#endregion

		//#region << List types >>
		popupCtrl.menuOptions = webvellaAdminService.getListMenuOptions();
		//#endregion

		popupCtrl.ok = function () {

			//webvellaAdminService.deleteEntityList(popupCtrl.parentData.list.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);

		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
			$timeout(function () {
				$state.go("webvella-admin-entity-lists", { entityName: popupCtrl.parentData.entity.name }, { reload: true });
			}, 0);
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}
		$log.debug('webvellaAdmin>entities>ManageViewServiceCodeModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};	
	
	//#endregion

})();
