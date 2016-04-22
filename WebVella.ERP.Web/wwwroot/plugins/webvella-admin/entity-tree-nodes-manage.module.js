/* entity-relations.module.js */

/**
* @desc this module manages the entity relations screen in the administration
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminManageEntityTreeNodesController', controller)
		.controller('AddNewTreeNodeModalController', AddNewTreeNodeModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-tree-nodes-manage', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/trees/:treeName/nodes', //  /desktop/areas after the parent state is prepended
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
					controller: 'WebVellaAdminManageEntityTreeNodesController',
					templateUrl: '/plugins/webvella-admin/entity-tree-nodes-manage.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedRelationsList: resolveRelationsList,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedCurrentEntityRecordTree: resolveCurrentEntityRecordTree
			},
			data: {

			}
		});
	};


	//#region << Resolve Function >>/////////////////////////
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

	resolveRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$timeout'];
	
	function resolveRelationsList($q, $log, webvellaCoreService, $state, $timeout) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaCoreService.getRelationsList(successCallback, errorCallback);

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

	resolveCurrentEntityRecordTree.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$timeout', '$stateParams'];
	
	function resolveCurrentEntityRecordTree($q, $log, webvellaCoreService, $state, $timeout, $stateParams) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaCoreService.getRecordsByTreeName($stateParams.treeName, $stateParams.entityName, successCallback, errorCallback);
		return defer.promise;
	}

	//#endregion

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$sce', '$log', '$q', '$rootScope', '$state', '$timeout', '$stateParams', 'pageTitle', 'resolvedRelationsList', 'resolvedCurrentEntityMeta',
					'$uibModal', 'resolvedCurrentEntityRecordTree', 'webvellaCoreService', 'ngToast'];
	
	function controller($scope, $sce, $log, $q, $rootScope, $state, $timeout, $stateParams, pageTitle, resolvedRelationsList, resolvedCurrentEntityMeta,
					$uibModal, resolvedCurrentEntityRecordTree, webvellaCoreService, ngToast) {
		
		var ngCtrl = this;

		//#region << Init >>
		ngCtrl.search = {};
		ngCtrl.allRelations = fastCopy(resolvedRelationsList);
		ngCtrl.currentEntityRelation = [];
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.tree = fastCopy(resolvedCurrentEntityRecordTree);
		//Awesome font icon names array 
		ngCtrl.icons = getFontAwesomeIconNames();
		//Update page title
		ngCtrl.pageTitle = "Entity Trees | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#region << Init selected relation >>
		ngCtrl.selectedRelation = {};
		for (var i = 0; i < ngCtrl.allRelations.length; i++) {
			if (ngCtrl.allRelations[i].id == ngCtrl.tree.meta.relationId) {
				ngCtrl.selectedRelation = ngCtrl.allRelations[i];
			}
		}
		//#endregion
		//#endregion


		//#region << Manage tree >>

		ngCtrl.addButtonLoadingClass = {};

		ngCtrl.attachHoverEffectClass = {};

		ngCtrl.addNodeModal = function (node) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'addNewTreeNodeModal.html',
				controller: 'AddNewTreeNodeModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					resolvedLookupRecords: resolveLookupRecords,
					resolvedAllRootNodeIds: resolveAllRootNodeIds
				}
			});

			//On modal exit
			modalInstance.result.then(function (selectedRecord) {
				changeRecordParentAndRefreshTree(selectedRecord.id, node.recordId)
			});
		}

		var resolveLookupRecords = function () {
			// Initialize
			var defer = $q.defer();
			// Process
			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				defer.reject();
			}

			function successCallback(response) {
				defer.resolve(response.object); //Submitting the whole response to manage the error states
			}

			var defaultLookupList = null;
			//Find the default lookup field if none return null.
			for (var i = 0; i < ngCtrl.entity.recordLists.length; i++) {
				if (ngCtrl.entity.recordLists[i].default && ngCtrl.entity.recordLists[i].type == "lookup") {
					defaultLookupList = ngCtrl.entity.recordLists[i];
				}
			}

			if (defaultLookupList == null) {
				response.message = "This entity does not have default lookup list";
				response.success = false;
				errorCallback(response.object);
			}
			else {
				webvellaCoreService.getRecordsByListName(defaultLookupList.name, ngCtrl.entity.name, 1, null, successCallback, errorCallback);
			}


			return defer.promise;
		}

		var resolveAllRootNodeIds = function () {
			var rootNodeIds = [];
			for (var i = 0; i < ngCtrl.entity.recordTrees.length; i++) {
				for (var j = 0; j < ngCtrl.entity.recordTrees[i].rootNodes.length; j++) {
					rootNodeIds.push(ngCtrl.entity.recordTrees[i].rootNodes[j].recordId);
				}
			}
			return rootNodeIds;
		}

		ngCtrl.removeNodeModal = function (node) {
			changeRecordParentAndRefreshTree(node.recordId, null);
		}

		var changeRecordParentAndRefreshTree = function (selectedRecordId, parentId) {

			function getTreeSuccessCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> Tree refreshed'
				});
				//Get the whole tree data again to refresh as there could be sub-children
				ngCtrl.tree = response.object;
			}

			function getTreeErrorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> Cannot reinitialize the tree, due to ' + response.message,
					timeout: 7000
				});
			}


			function successCallback(response) {
				//Get the whole tree data again to refresh as there could be sub-children
				webvellaCoreService.getRecordsByTreeName($stateParams.treeName, $stateParams.entityName, getTreeSuccessCallback, getTreeErrorCallback);
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> Cannot set the selected record as parent, due to ' + response.message,
					timeout: 7000
				});
			}

			//Update the selected record by setting the node as its parent
			//Find the target field name 
			var targetFieldName = null;
			for (var i = 0; i < ngCtrl.entity.fields.length; i++) {
				if (ngCtrl.selectedRelation.targetFieldId == ngCtrl.entity.fields[i].id) {
					targetFieldName = ngCtrl.entity.fields[i].name;
				}
			}

			var patchObject = {};
			//Add to the tree as the node's child.
			patchObject[targetFieldName] = parentId;
			webvellaCoreService.patchRecord(selectedRecordId, ngCtrl.entity.name, patchObject, successCallback, errorCallback);

		}

		ngCtrl.treeOptions = {
			dropped: function (event) {
				var parentSwitched = false;
				if (event.dest.nodesScope.$parent.$modelValue.id != event.source.nodesScope.$parent.$modelValue.id) {
					parentSwitched = true;
				}
				if (parentSwitched) {
					changeRecordParentAndRefreshTree(event.source.nodeScope.$modelValue.id,event.dest.nodesScope.$parent.$modelValue.id)
				}
			}
		}

		//#endregion
	}

	//#region << Modals >>

	AddNewTreeNodeModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', 'resolvedLookupRecords',
        'resolvedAllRootNodeIds', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];
	
	function AddNewTreeNodeModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, resolvedLookupRecords,
       resolvedAllRootNodeIds,webvellaCoreService, ngToast, $timeout, $state) {

		
		var popupCtrl = this;
		//#region << Init >>
		popupCtrl.currentPage = 1;
		popupCtrl.hasWarning = false;
		popupCtrl.warningMessage = "";
		popupCtrl.ngCtrl = fastCopy(ngCtrl);
		popupCtrl.relationLookupList = fastCopy(resolvedLookupRecords);
		popupCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;
		//#endregion


		//#region << Logic >>

		//#region << Search >>
		popupCtrl.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				popupCtrl.submitSearchQuery();
			}
		}
		popupCtrl.submitSearchQuery = function () {
			function successCallback(response) {
				popupCtrl.relationLookupList = fastCopy(response.object);
			}
			function errorCallback(response) { }

			if (popupCtrl.searchQuery) {
				popupCtrl.searchQuery = popupCtrl.searchQuery.trim();
			}
			webvellaCoreService.getRecordsByListName(popupCtrl.relationLookupList.meta.name, popupCtrl.ngCtrl.entity.name, 1, null, successCallback, errorCallback);
		}
		//#endregion

		//#region << Paging >>
		popupCtrl.selectPage = function (page) {
			// Process
			function successCallback(response) {
				popupCtrl.relationLookupList = fastCopy(response.object);
				popupCtrl.currentPage = page;
			}

			function errorCallback(response) {

			}

			webvellaCoreService.getRecordsByListName(popupCtrl.relationLookupList.meta.name, popupCtrl.ngCtrl.entity.name, page, null, successCallback, errorCallback);
		}

		//#endregion


		//Find the target field name 
		var targetFieldName = null;
		for (var i = 0; i < popupCtrl.ngCtrl.entity.fields.length; i++) {
			if (popupCtrl.ngCtrl.selectedRelation.targetFieldId == popupCtrl.ngCtrl.entity.fields[i].id) {
				targetFieldName = popupCtrl.ngCtrl.entity.fields[i].name;
			}
		}		
		
		//Only nodes that has no parents or not already root nodes to a tree can be selected
		popupCtrl.canSelectRecord = function(record) {
				if (record[targetFieldName] != null || resolvedAllRootNodeIds.indexOf(record.id) > -1) {
					return false;
				}
				return true;
		}

		popupCtrl.selectSingleRecord = function (record) {
			$uibModalInstance.close(record);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		//#endregion

	};

	//#endregion

})();


