/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
		.controller('ManageViewServiceCodeModalController', ManageViewServiceCodeModalController)
		.controller('AddManageViewMenuItemModalController', AddManageViewMenuItemModalController)
        .controller('WebVellaAdminEntityViewManageMenuController', controller);

	//#region << Configuration >> /////////////////////////
	config.$inject = ['$stateProvider'];
	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-view-manage-actions', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/views/:viewName/actions',
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
					controller: 'WebVellaAdminEntityViewManageMenuController',
					templateUrl: '/plugins/webvella-admin/entity-view-manage-actions.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta
			},
			data: {

			}
		});
	};
	//#endregion

	//#region << Resolve >> ///////////////////////////////
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
		$log.debug('webvellaAdmin>entity-details> BEGIN resolveCurrentEntityMeta state.resolved ' + moment().format('HH:mm:ss SSSS'));
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
		$log.debug('webvellaAdmin>entity-details> END resolveCurrentEntityMeta state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	//#endregion

	//#region << Controller >> ////////////////////////////
	controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal', '$timeout',
                            'resolvedCurrentEntityMeta', 'webvellaAdminService', 'webvellaAreasService', 'ngToast'];
	/* @ngInject */
	function controller($filter, $scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal, $timeout,
                        resolvedCurrentEntityMeta, webvellaAdminService, webvellaAreasService, ngToast) {
		$log.debug('webvellaAdmin>entity-view-manage-menu> START controller.exec ' + moment().format('HH:mm:ss SSSS'));

		/* jshint validthis:true */
		var ngCtrl = this;
		//#region << Initialize Current Entity >>
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Update page title & Hide side menu>>
		ngCtrl.pageTitle = "Entity Views | " + pageTitle;
		$timeout(function () {
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide side menu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		}, 0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#endregion

		//#region << Initialize View and Content Region >>
		ngCtrl.view = {};
		ngCtrl.originalView = {};
		for (var i = 0; i < ngCtrl.entity.recordViews.length; i++) {
			if (ngCtrl.entity.recordViews[i].name === $stateParams.viewName) {
				ngCtrl.view = fastCopy(ngCtrl.entity.recordViews[i]);
				ngCtrl.originalView = fastCopy(ngCtrl.entity.recordViews[i]);
			}
		}
		//#endregion


		//#region << Modals >>
		ngCtrl.manageServiceCodeModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageServiceCodeModal.html',
				controller: 'ManageViewServiceCodeModalController',
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
				controller: 'AddManageViewMenuItemModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					parentData: function () { return ngCtrl; },
					menuItem: function () { return menuItem; }
				}
			});
		}
		//#endregion


		$log.debug('webvellaAdmin>entity-view-manage-menu> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

	}


	//#region << Modal Controllers >>
	ManageViewServiceCodeModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaRootService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function ManageViewServiceCodeModalController(parentData, $uibModalInstance, $log, webvellaRootService, ngToast, $timeout, $state) {
		$log.debug('webvellaAdmin>entities>ManageViewServiceCodeModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.parentData = parentData;

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

	AddManageViewMenuItemModalController.$inject = ['parentData','menuItem', '$uibModalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function AddManageViewMenuItemModalController(parentData,menuItem, $uibModalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
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
		  popupCtrl.menuItem = webvellaAdminService.initViewMenuItem();
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
		popupCtrl.menuOptions = webvellaAdminService.getViewMenuOptions();
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
