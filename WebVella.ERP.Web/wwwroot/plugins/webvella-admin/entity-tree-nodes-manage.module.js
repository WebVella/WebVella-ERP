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

	/* @ngInject */
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
					controllerAs: 'contentData'
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

	resolveRelationsList.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout'];
	/* @ngInject */
	function resolveRelationsList($q, $log, webvellaAdminService, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-relations> BEGIN resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaAdminService.getRelationsList(successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-relations> END resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

	resolveCurrentEntityRecordTree.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout', '$stateParams'];
	/* @ngInject */
	function resolveCurrentEntityRecordTree($q, $log, webvellaAdminService, $state, $timeout, $stateParams) {
		$log.debug('webvellaAdmin>entity-relations> BEGIN resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaAdminService.getRecordsByTreeName($stateParams.treeName, $stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-relations> END resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	//#endregion

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$sce', '$log', '$q', '$rootScope', '$state', '$timeout', '$stateParams', 'pageTitle', 'resolvedRelationsList', 'resolvedCurrentEntityMeta',
					'$uibModal', 'resolvedCurrentEntityRecordTree', 'webvellaAdminService', 'ngToast', 'webvellaAreasService'];
	/* @ngInject */
	function controller($scope, $sce, $log, $q, $rootScope, $state, $timeout, $stateParams, pageTitle, resolvedRelationsList, resolvedCurrentEntityMeta,
					$uibModal, resolvedCurrentEntityRecordTree, webvellaAdminService, ngToast, webvellaAreasService) {
		$log.debug('webvellaAdmin>entity-relations> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;

		//#region << Init >>
		contentData.search = {};
		contentData.allRelations = fastCopy(resolvedRelationsList);
		contentData.currentEntityRelation = [];
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		contentData.tree = fastCopy(resolvedCurrentEntityRecordTree);
		//Awesome font icon names array 
		contentData.icons = getFontAwesomeIconNames();
		//Update page title
		contentData.pageTitle = "Entity Trees | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);
		$rootScope.currentSectionName = "Entities";
		//#region << Init selected relation >>
		contentData.selectedRelation = {};
		for (var i = 0; i < contentData.allRelations.length; i++) {
			if (contentData.allRelations[i].id == contentData.tree.meta.relationId) {
				contentData.selectedRelation = contentData.allRelations[i];
			}
		}
		//#endregion
		//#endregion


		//#region << Manage tree >>

		contentData.addButtonLoadingClass = {};

		contentData.attachHoverEffectClass = {};

		contentData.addNodeModal = function (node) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'addNewTreeNodeModal.html',
				controller: 'AddNewTreeNodeModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () {
						return contentData;
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
			for (var i = 0; i < contentData.entity.recordLists.length; i++) {
				if (contentData.entity.recordLists[i].default && contentData.entity.recordLists[i].type == "lookup") {
					defaultLookupList = contentData.entity.recordLists[i];
				}
			}

			if (defaultLookupList == null) {
				response.message = "This entity does not have default lookup list";
				response.success = false;
				errorCallback(response.object);
			}
			else {
				webvellaAreasService.getListRecords(defaultLookupList.name, contentData.entity.name, "all", 1, null, successCallback, errorCallback);
			}


			return defer.promise;
		}

		var resolveAllRootNodeIds = function () {
			var rootNodeIds = [];
			for (var i = 0; i < contentData.entity.recordTrees.length; i++) {
				for (var j = 0; j < contentData.entity.recordTrees[i].rootNodes.length; j++) {
					rootNodeIds.push(contentData.entity.recordTrees[i].rootNodes[j].recordId);
				}
			}
			return rootNodeIds;
		}

		contentData.removeNodeModal = function (node) {
			changeRecordParentAndRefreshTree(node.recordId, null);
		}

		var changeRecordParentAndRefreshTree = function (selectedRecordId, parentId) {

			function getTreeSuccessCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> Tree refreshed'
				});
				//Get the whole tree data again to refresh as there could be sub-children
				contentData.tree = response.object;
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
				webvellaAdminService.getRecordsByTreeName($stateParams.treeName, $stateParams.entityName, getTreeSuccessCallback, getTreeErrorCallback);
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
			for (var i = 0; i < contentData.entity.fields.length; i++) {
				if (contentData.selectedRelation.targetFieldId == contentData.entity.fields[i].id) {
					targetFieldName = contentData.entity.fields[i].name;
				}
			}

			var patchObject = {};
			//Add to the tree as the node's child.
			patchObject[targetFieldName] = parentId;
			webvellaAdminService.patchRecord(selectedRecordId, contentData.entity.name, patchObject, successCallback, errorCallback);

		}

		contentData.treeOptions = {
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

		$log.debug('webvellaAdmin>entity-relations> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

	//#region << Modals >>

	AddNewTreeNodeModalController.$inject = ['contentData', '$uibModalInstance', '$log', '$q', '$stateParams', 'resolvedLookupRecords',
        'resolvedAllRootNodeIds', 'webvellaAreasService', 'ngToast', '$timeout', '$state'];
	/* @ngInject */
	function AddNewTreeNodeModalController(contentData, $uibModalInstance, $log, $q, $stateParams, resolvedLookupRecords,
       resolvedAllRootNodeIds,webvellaAreasService, ngToast, $timeout, $state) {

		$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		//#region << Init >>
		popupData.currentPage = 1;
		popupData.hasWarning = false;
		popupData.warningMessage = "";
		popupData.contentData = fastCopy(contentData);
		popupData.relationLookupList = fastCopy(resolvedLookupRecords);
		popupData.renderFieldValue = webvellaAreasService.renderFieldValue;
		//#endregion


		//#region << Logic >>

		//#region << Search >>
		popupData.checkForSearchEnter = function (e) {
			var code = (e.keyCode ? e.keyCode : e.which);
			if (code == 13) { //Enter keycode
				popupData.submitSearchQuery();
			}
		}
		popupData.submitSearchQuery = function () {
			function successCallback(response) {
				popupData.relationLookupList = fastCopy(response.object);
			}
			function errorCallback(response) { }

			if (popupData.searchQuery) {
				popupData.searchQuery = popupData.searchQuery.trim();
			}
			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.contentData.entity.name, "all", 1, popupData.searchQuery, successCallback, errorCallback);
		}
		//#endregion

		//#region << Paging >>
		popupData.selectPage = function (page) {
			// Process
			function successCallback(response) {
				popupData.relationLookupList = fastCopy(response.object);
				popupData.currentPage = page;
			}

			function errorCallback(response) {

			}

			webvellaAreasService.getListRecords(popupData.relationLookupList.meta.name, popupData.contentData.entity.name, "all", page, null, successCallback, errorCallback);
		}

		//#endregion


		//Find the target field name 
		var targetFieldName = null;
		for (var i = 0; i < popupData.contentData.entity.fields.length; i++) {
			if (popupData.contentData.selectedRelation.targetFieldId == popupData.contentData.entity.fields[i].id) {
				targetFieldName = popupData.contentData.entity.fields[i].name;
			}
		}		
		
		//Only nodes that has no parents or not already root nodes to a tree can be selected
		popupData.canSelectRecord = function(record) {
				if (record[targetFieldName] != null || resolvedAllRootNodeIds.indexOf(record.id) > -1) {
					return false;
				}
				return true;
		}

		popupData.selectSingleRecord = function (record) {
			$uibModalInstance.close(record);
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
		//#endregion

		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	//#endregion

})();


