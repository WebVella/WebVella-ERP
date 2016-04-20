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
		.controller('ManageDataLinkModalController', ManageDataLinkModalController)
		.directive('queryItem', queryItem)
		.controller('queryItemController', queryItemController);

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
					templateUrl: '/plugins/webvella-admin/sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminEntityListManageController',
					templateUrl: '/plugins/webvella-admin/entity-list-manage.view.html',
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

	resolveCurrentEntityList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveCurrentEntityList($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {

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

		webvellaCoreService.getEntityList($stateParams.listName, $stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}

	resolveViewLibrary.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveViewLibrary($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {

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

		webvellaCoreService.getEntityViewLibrary($stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveEntityRelationsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {

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

		webvellaCoreService.getRelationsList(successCallback, errorCallback);
		return defer.promise;
	}
	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$timeout', 'ngToast', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', 'resolvedCurrentEntityList',
						'resolvedViewLibrary', 'webvellaCoreService', 'resolvedEntityRelationsList'];
	
	function controller($scope, $log, $rootScope, $state, $timeout, ngToast, pageTitle, resolvedCurrentEntityMeta, $uibModal, resolvedCurrentEntityList,
						resolvedViewLibrary, webvellaCoreService, resolvedEntityRelationsList) {

		
		var ngCtrl = this;
		ngCtrl.entity = resolvedCurrentEntityMeta;
		//Awesome font icon names array 
		ngCtrl.icons = getFontAwesomeIconNames();

		//#region << Update page title & hide the side menu >>
		ngCtrl.pageTitle = "Entity Details | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		},0);

		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#endregion

		//#region << List types >>
		ngCtrl.listTypeOptions = [
            {
            	key: "general",
            	value: "general"
            },
            {
            	key: "lookup",
            	value: "lookup"
            },
            {
            	key: "hidden",
            	value: "hidden"
            }
		];
		//#endregion

 		//#region << Initialize the list >>
		ngCtrl.list = fastCopy(resolvedCurrentEntityList);
		ngCtrl.relationsList = fastCopy(resolvedEntityRelationsList);

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
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			webvellaCoreService.regenerateAllAreaAttachments();
		}

		function patchSuccessCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			ngCtrl.list = response.object;
			ngCtrl.generateAlreadyUsed();
		}
		function patchErrorCallback(response) {
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});
		}

		ngCtrl.fieldUpdate = function (fieldName, data) {
			var postObj = {};
			postObj[fieldName] = data;
			webvellaCoreService.patchEntityList(postObj, ngCtrl.list.name, ngCtrl.entity.name, patchFieldSuccessCallback, patchErrorCallback)
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
		ngCtrl.fullLibrary.items = fastCopy(resolvedViewLibrary);
		//Fields list eligable to be options in the sort and query dropdowns
		ngCtrl.onlyFieldsLibrary = {};
		ngCtrl.onlyFieldsLibrary.items = [];
		ngCtrl.library = {};
		ngCtrl.library.relations = [];
		ngCtrl.library.items = [];

		ngCtrl.sortLibrary = function () {
			ngCtrl.library.items = ngCtrl.library.items.sort(function (a, b) {
				if (a.fieldName < b.fieldName) return -1;
				if (a.fieldName > b.fieldName) return 1;
				return 0;
			});
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
	deleteListModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];
	
	function deleteListModalController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state) {
		
		var popupCtrl = this;
		popupCtrl.parentData = parentData;

		popupCtrl.ok = function () {

			webvellaCoreService.deleteEntityList(popupCtrl.parentData.list.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);

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
	};


	ManageDataLinkModalController.$inject = ['$uibModalInstance', '$log'];
	
	function ManageDataLinkModalController($uibModalInstance, $log) {
		
		var popupCtrl = this;

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

	};

	//#endregion

	//#region << Query Directive >>
	queryItem.$inject = ['$compile', '$templateRequest', 'RecursionHelper'];
	
	function queryItem($compile, $templateRequest, RecursionHelper) {
		var directive = {
			controller: queryItemController,
			template: '<ng-include src="getTemplateUrl()"/>',
			restrict: 'E',
			scope: {
				currentQuery: '=',
				rootQuery: '=',
				parentQuery: '=?',
				pageScope: '=',
				queryIndex: '='
			},
			compile: function (element) {
				return RecursionHelper.compile(element, function (scope, iElement, iAttrs, controller, transcludeFn) {
					// Define your normal link function here.
					// Alternative: instead of passing a function,
					// you can also pass an object with 
					// a 'pre'- and 'post'-link function.
				});
			}
		};
		return directive;
	}

	queryItemController.$inject = ['$filter', '$log', '$state', '$scope', '$q', '$uibModal', 'ngToast', 'webvellaCoreService'];
	
	function queryItemController($filter, $log, $state, $scope, $q, $uibModal, ngToast, webvellaCoreService) {
		$scope.ngCtrl = $scope.pageScope;
		$scope.getTemplateUrl = function () {
			switch ($scope.currentQuery.queryType) {
				case "EQ":
					return 'queryRule.html';
				case "NOT":
					return 'queryRule.html';
				case "LT":
					return 'queryRule.html';
				case "LTE":
					return 'queryRule.html';
				case "GT":
					return 'queryRule.html';
				case "GTE":
					return 'queryRule.html';
				case "CONTAINS":
					return 'queryRule.html';
				case "STARTSWITH":
					return 'queryRule.html';
				case "AND":
					return 'querySection.html';
				case "OR":
					return 'querySection.html';
			}
		}
	}
	//#endregion


})();
