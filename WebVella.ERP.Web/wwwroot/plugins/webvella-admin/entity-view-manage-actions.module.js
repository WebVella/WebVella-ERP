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
		.controller('AddManageViewActionItemModalController', AddManageViewActionItemModalController)
        .controller('WebVellaAdminEntityViewManageMenuController', controller);

	//#region << Configuration >> /////////////////////////
	config.$inject = ['$stateProvider'];
	
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

		return defer.promise;
	}

	//#endregion

	//#region << Controller >> ////////////////////////////
	controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal', '$timeout',
                            'resolvedCurrentEntityMeta', 'webvellaCoreService', 'ngToast'];
	
	function controller($filter, $scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal, $timeout,
                        resolvedCurrentEntityMeta, webvellaCoreService, ngToast) {

		
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

		//#region << Order actionItems >>
		ngCtrl.orderActionItems = function () {
			ngCtrl.view.actionItems.sort(sort_by('menu', {name:'weight', primer: parseInt, reverse: false}));
		}
		ngCtrl.orderActionItems();
		//#endregion

		//#region << Modals >>
		ngCtrl.manageServiceCodeModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageServiceCodeModal.html',
				controller: 'ManageViewServiceCodeModalController',
				controllerAs: "popupCtrl",
				size: "width-100p",
				backdrop:"static",
				resolve: {
					parentData: function () { return ngCtrl; }
				}
			});
		}

		ngCtrl.addManageActionItemModal = function (actionItem) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'addManageActionItemModal.html',
				controller: 'AddManageViewActionItemModalController',
				controllerAs: "popupCtrl",
				size: "width-100p",
				backdrop:"static",
				resolve: {
					parentData: function () { return ngCtrl; },
					actionItem: function () { return actionItem; }
				}
			});
		}
		//#endregion
	}


	//#region << Modal Controllers >>
	ManageViewServiceCodeModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];
	
	function ManageViewServiceCodeModalController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state) {
		
		var popupCtrl = this;
		popupCtrl.parentData = fastCopy(parentData);
		popupCtrl.serviceCode = fastCopy(popupCtrl.parentData.view.serviceCode);
		//#region << Ace editor >>
		popupCtrl.aceOptions = {
			useWrapMode: true,
			showGutter: true,
			theme: 'twilight',
			mode: 'javascript',
			firstLineNumber: 1,
			onLoad: popupCtrl.aceOnLoad,
			onChange: popupCtrl.aceOnChange,
			advanced: {
				showPrintMargin: false,
				fontSize:"16px"
			}
		}

		popupCtrl.aceOnChange = function (event) { };
		//#endregion

		popupCtrl.loadDefaultScript = function(){
			webvellaCoreService.getFileContent("/api/v1/en_US/meta/entity/" + popupCtrl.parentData.entity.name +"/view/" + popupCtrl.parentData.view.name +"/service.js?defaultScript=true",getDefaultScriptSuccessCallback,getDefaultScriptErrorCallback);
		}

		popupCtrl.ok = function () {
			var postObj = {};
			postObj.serviceCode = fastCopy(popupCtrl.serviceCode);
			webvellaCoreService.patchEntityView(postObj, popupCtrl.parentData.view.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);
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
			parentData.view.serviceCode = response.object.serviceCode;
			$uibModalInstance.close('success');
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}

		function getDefaultScriptSuccessCallback(response) {
			popupCtrl.serviceCode = response.data;
		}

		function getDefaultScriptErrorCallback(response) {
			ngToast.create({
				className: 'danger',
				content: '<span class="go-green">Error:</span> ' + response.message
			});
		}
	};

	AddManageViewActionItemModalController.$inject = ['parentData', 'actionItem', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];
	
	function AddManageViewActionItemModalController(parentData, actionItem, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state) {
		
		var popupCtrl = this;
		popupCtrl.parentData = fastCopy(parentData);
		popupCtrl.originalActionItems = fastCopy(parentData.view.actionItems);
		popupCtrl.actionItem = {};
		popupCtrl.isEdit = false;
		if (actionItem != null) {
			popupCtrl.isEdit = true;
			popupCtrl.actionItem = fastCopy(actionItem);
		}
		else {
			popupCtrl.actionItem = webvellaCoreService.initViewActionItem();
			popupCtrl.actionItem.menu = "hidden";
			popupCtrl.actionItem.template = "";
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
			advanced: {
				showPrintMargin: false,
				fontSize:"16px"
			}
		}

		popupCtrl.aceOnChange = function (event) { };
		//#endregion

		//#region << List types >>
		popupCtrl.menuOptions = webvellaCoreService.getViewMenuOptions();
		//#endregion

		popupCtrl.validateName = function (name) {
			var isUnique = true;
			popupCtrl.nameError = false;
			popupCtrl.nameMessage = "";
			popupCtrl.originalActionItems.forEach(function (item) {
				if (item.name === name) {
					isUnique = false;
				}
			});
			if(!name){
				popupCtrl.nameError = true;
				popupCtrl.nameMessage = "required";
				return false;				
			}
			else if (!isUnique) {
				popupCtrl.nameError = true;
				popupCtrl.nameMessage = "should be unique for the current view";
				return false;
			}
			else {
				return true;
			}
		}

		popupCtrl.removeActionItemByName = function(name, proccessedArray){
			var newArray = [];
			for (var i = 0; i < proccessedArray.length; i++) {
			   if(proccessedArray[i].name !== name){
					newArray.push(proccessedArray[i]);
			   }
			}
			return newArray;
		}


		popupCtrl.ok = function () {
			if (popupCtrl.isEdit) {
				//Is name changed
				var nameIsChanged = true;
				if (popupCtrl.actionItem.name == actionItem.name) {
					nameIsChanged = false;
				}
				if(nameIsChanged && !popupCtrl.validateName(popupCtrl.actionItem.name)){
					return;
				}
				var updatedActionItems = popupCtrl.removeActionItemByName(actionItem.name,popupCtrl.originalActionItems)
				var postObj = {};
				postObj.actionItems = updatedActionItems;
				postObj.actionItems.push(popupCtrl.actionItem);
				webvellaCoreService.patchEntityView(postObj, popupCtrl.parentData.view.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);					
			}
			else {
				if (popupCtrl.validateName(popupCtrl.actionItem.name)) {
					var postObj = {};
					postObj.actionItems = popupCtrl.originalActionItems;
					postObj.actionItems.push(popupCtrl.actionItem);
					webvellaCoreService.patchEntityView(postObj, popupCtrl.parentData.view.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);
				}
			}

		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};


		popupCtrl.delete = function () {
			var updatedActionItems = popupCtrl.removeActionItemByName(actionItem.name,popupCtrl.originalActionItems)
			var postObj = {};
			postObj.actionItems = updatedActionItems;
			webvellaCoreService.patchEntityView(postObj, popupCtrl.parentData.view.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);
		};


		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			parentData.view.actionItems = response.object.actionItems;
			parentData.orderActionItems();
			$uibModalInstance.close('success');
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}
	};

	//#endregion

})();
