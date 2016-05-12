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
					templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminManageEntityTreeNodesController',
					templateUrl: '/plugins/webvella-admin/entity-tree-nodes-manage.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedRelationsList: resolveRelationsList,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedCurrentEntityRecordTree: resolveCurrentEntityRecordTree
			},
			data: {

			}
		});
	};


	//#region << Resolve Function >>/////////////////////////

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

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', '$translate'];

	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout, $translate) {
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
					'$uibModal', 'resolvedCurrentEntityRecordTree', 'webvellaCoreService', 'ngToast', '$translate'];

	function controller($scope, $sce, $log, $q, $rootScope, $state, $timeout, $stateParams, pageTitle, resolvedRelationsList, resolvedCurrentEntityMeta,
					$uibModal, resolvedCurrentEntityRecordTree, webvellaCoreService, ngToast, $translate) {

		var ngCtrl = this;

		//#region << Init >>
		ngCtrl.search = {};
		ngCtrl.allRelations = resolvedRelationsList;
		ngCtrl.currentEntityRelation = [];
		ngCtrl.entity = resolvedCurrentEntityMeta;
		ngCtrl.tree = resolvedCurrentEntityRecordTree;
		//Awesome font icon names array 
		ngCtrl.icons = getFontAwesomeIconNames();
		//#region << Update page title & hide the side menu >>
		$translate(['RECORD_TREE_MANAGE_PAGE_TITLE', 'ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_TREE_MANAGE_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		//#endregion  
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
				$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
						timeout: 7000
					});
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
				webvellaCoreService.getRecordsByListMeta(defaultLookupList, ngCtrl.entity.name, 1,null, null, successCallback, errorCallback);
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
				$translate(['SUCCESS_MESSAGE_LABEL', 'RECORD_TREE_REFRESHED']).then(function (translations) {
					ngToast.create({
						className: 'success',
						content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.RECORD_TREE_REFRESHED
					});
				});
				//Get the whole tree data again to refresh as there could be sub-children
				ngCtrl.tree = response.object;
			}

			function getTreeErrorCallback(response) {
				$translate(['ERROR_MESSAGE_LABEL','RECORD_TREE_CANNOT_REINIT_DUE_TO']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + translations.RECORD_TREE_CANNOT_REINIT_DUE_TO + ' ' + response.message,
						timeout: 7000
					});
				});
			}


			function successCallback(response) {
				//Get the whole tree data again to refresh as there could be sub-children
				webvellaCoreService.getRecordsByTreeName($stateParams.treeName, $stateParams.entityName, getTreeSuccessCallback, getTreeErrorCallback);
			}

			function errorCallback(response) {
				$translate(['ERROR_MESSAGE_LABEL','RECORD_TREE_CANNOT_SET_AS_PARENT_DUE_TO']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + translations.RECORD_TREE_CANNOT_SET_AS_PARENT_DUE_TO + ' ' + response.message,
						timeout: 7000
					});
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
					changeRecordParentAndRefreshTree(event.source.nodeScope.$modelValue.id, event.dest.nodesScope.$parent.$modelValue.id)
				}
			}
		}

		//#endregion
	}

	//#region << Modals >>

	AddNewTreeNodeModalController.$inject = ['ngCtrl', '$uibModalInstance', '$log', '$q', '$stateParams', 'resolvedLookupRecords',
        'resolvedAllRootNodeIds', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];

	function AddNewTreeNodeModalController(ngCtrl, $uibModalInstance, $log, $q, $stateParams, resolvedLookupRecords,
       resolvedAllRootNodeIds, webvellaCoreService, ngToast, $timeout, $state) {


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
			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, popupCtrl.ngCtrl.entity.name, 1, null, null, successCallback, errorCallback);
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

			webvellaCoreService.getRecordsByListMeta(popupCtrl.relationLookupList.meta, popupCtrl.ngCtrl.entity.name, page, null, null, successCallback, errorCallback);
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
		popupCtrl.canSelectRecord = function (record) {
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


