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
					templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminEntityViewManageMenuController',
					templateUrl: '/plugins/webvella-admin/entity-view-manage-actions.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedEntityList:resolveEntityList
			},
			data: {

			}
		});
	};
	//#endregion

	//#region << Resolve >> ///////////////////////////////

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

	//#region << Controller >> ////////////////////////////
	controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal', '$timeout',
                            'resolvedEntityList', 'webvellaCoreService', 'ngToast','$translate'];
	
	function controller($filter, $scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal, $timeout,
                        resolvedEntityList, webvellaCoreService, ngToast,$translate) {

		
		var ngCtrl = this;

		//#region << Initialize Current Entity >>
		ngCtrl.entity = webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName,resolvedEntityList);
		//#endregion

		//#region << Update page title & hide the side menu >>
		$translate(['RECORD_VIEW_MANAGE_PAGE_TITLE', 'ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_VIEW_MANAGE_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
    	//#endregion

		//#region << Initialize View and Content Region >>
        ngCtrl.view = fastCopy(webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.viewName, $stateParams.entityName,resolvedEntityList));
        ngCtrl.originalView = fastCopy(webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.viewName, $stateParams.entityName,resolvedEntityList));
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

		ngCtrl.addManageActionItemModal = function (actionItem,selectTemplateFirst) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'addManageActionItemModal.html',
				controller: 'AddManageViewActionItemModalController',
				controllerAs: "popupCtrl",
				size: "width-75p",
				backdrop:"static",
				resolve: {
					parentData: function () { return ngCtrl; },
					actionItem: function () { return actionItem; },
					selectTemplateFirst: function(){ return selectTemplateFirst;}
				}
			});
		}
		//#endregion
	}


	//#region << Modal Controllers >>
	ManageViewServiceCodeModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state','$translate'];
	
	function ManageViewServiceCodeModalController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state,$translate) {
		
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
			$uibModalInstance.close('dismiss');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
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
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}
	};

	AddManageViewActionItemModalController.$inject = ['parentData', 'actionItem', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state','$translate','selectTemplateFirst'];
	
	function AddManageViewActionItemModalController(parentData, actionItem, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state,$translate,selectTemplateFirst) {
		
		var popupCtrl = this;
		popupCtrl.parentData = fastCopy(parentData);
		popupCtrl.originalActionItems = fastCopy(parentData.view.actionItems);
		popupCtrl.actionItem = {};
		popupCtrl.isEdit = false;
		popupCtrl.selectTemplateFirst = fastCopy(selectTemplateFirst);
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
			$uibModalInstance.close('dismiss');
		};


		popupCtrl.delete = function () {
			var updatedActionItems = popupCtrl.removeActionItemByName(actionItem.name,popupCtrl.originalActionItems)
			var postObj = {};
			postObj.actionItems = updatedActionItems;
			webvellaCoreService.patchEntityView(postObj, popupCtrl.parentData.view.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);
		};


		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			parentData.view.actionItems = response.object.actionItems;
			parentData.orderActionItems();
			$uibModalInstance.close('success');
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;
		}

		popupCtrl.selectTemplate = function(templateName){
			popupCtrl.actionItem = webvellaCoreService.initViewActionItemTemplate(templateName);
			popupCtrl.selectTemplateFirst = false;
		}

		popupCtrl.getTemplateCurrentMenu = function(templateName){
			var selectedMenus = "";
			popupCtrl.originalActionItems.forEach(function(actionItem){
				if(actionItem.name == templateName){
					selectedMenus += "<div class='go-green'>"+actionItem.menu + "</div>";
				}							
			});
			if(selectedMenus == ""){
				selectedMenus = "<div class='go-grey'>not used</div>";
			}
			else {
				selectedMenus += "<small class='go-grey'>(use with new name)</small>";
			}

			return selectedMenus;		
		}

	};

	//#endregion

})();
