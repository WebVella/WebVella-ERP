/* entity-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityListManageColumnsController', controller);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-list-manage-columns', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/lists/:listName/columns', //  /desktop/areas after the parent state is prepended
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
					controller: 'WebVellaAdminEntityListManageColumnsController',
					templateUrl: '/plugins/webvella-admin/entity-list-manage-columns.view.html',
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
			webvellaAdminService.regenerateAllAreaAttachments();
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

		ngCtrl.updateColumns = function (orderChangedOnly) {
			var postObj = {};
			postObj.columns = ngCtrl.list.columns;
			calculateDefaultSearchFieldName();
			webvellaAdminService.patchEntityList(postObj, ngCtrl.list.name, ngCtrl.entity.name, patchSuccessCallback, patchErrorCallback)
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

		//#region << Drag & Drop >>
		ngCtrl.moveToColumns = function (item, index) {
			//Add Item at the end of the columns list
			ngCtrl.list.columns.push(item);
			//Remove from library
			ngCtrl.updateColumns(true);
			ngCtrl.regenerateLibrary();
		}
		ngCtrl.moveToLibrary = function (item, index) {
			//Remove from library
			ngCtrl.list.columns.splice(index, 1);
			ngCtrl.updateColumns(false);
			ngCtrl.regenerateLibrary();
		}
		ngCtrl.dragControlListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				return true
			},
			itemMoved: function (eventObj) {
				ngCtrl.updateColumns(true);
				ngCtrl.regenerateLibrary();
			},
			orderChanged: function (eventObj) {
				ngCtrl.updateColumns(true);
				ngCtrl.regenerateLibrary();
			}
		};

		ngCtrl.dragLibraryListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				return true
			},
			itemMoved: function (eventObj) {
				ngCtrl.updateColumns(false);
				ngCtrl.regenerateLibrary();
			},
			orderChanged: function (eventObj) {
				ngCtrl.updateColumns(true);
				ngCtrl.regenerateLibrary();
			}
		};

		//#endregion

		//#region << Relations >>

		ngCtrl.changeRelationDirection = function (relation) {
			if (relation.direction == "origin-target") {
				relation.direction = "target-origin";
			}
			else {
				relation.direction = "origin-target";
			}
			ngCtrl.list.relationOptions = [];

			for (var i = 0; i < ngCtrl.library.relations.length; i++) {
				var relation = fastCopy(ngCtrl.library.relations[i]);
				delete relation.addedToLibrary;
				delete relation.sameOriginTargetEntity;
				ngCtrl.list.relationOptions.push(relation);
			}

			function successCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> ' + response.message
				});
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				//Undo change
				for (var j = 0; j < ngCtrl.library.relations.length; j++) {
					if (ngCtrl.library.relations[j].relationName == relation.relationName) {
						if (ngCtrl.library.relations[j].direction == "origin-target") {
							ngCtrl.library.relations[j].direction = "target-origin";
						}
						else {
							ngCtrl.library.relations[j].direction = "origin-target";
						}
					}
				}
			}

			webvellaAdminService.updateEntityList(ngCtrl.list, ngCtrl.entity.name, successCallback, errorCallback);
		}

		ngCtrl.toggleRelationToLibrary = function (relation) {
			if (!relation.addedToLibrary) {
				ngCtrl.fullLibrary.items.forEach(function (item) {
					if (item.relationName && item.relationName == relation.relationName) {
						if (item.meta && alreadyUsedItemDataNames.indexOf(item.dataName) == -1) {
							switch (item.type) {
								case "fieldFromRelation":
									ngCtrl.library.items.push(item);
									break;
								case "viewFromRelation":
									ngCtrl.library.items.push(item);
									break;
								case "listFromRelation":
									if (item.listId != ngCtrl.list.id) {
										ngCtrl.library.items.push(item);
									}
									break;
								case "treeFromRelation":
									ngCtrl.library.items.push(item);
									break;
							}
						}
					}
				});
				relation.addedToLibrary = true;
			}
			else {
				var tempRelationChangeLibrary = [];
				ngCtrl.library.items.forEach(function (item) {
					if (!item.relationName) {
						tempRelationChangeLibrary.push(item);
					}
					else if (item.relationName != relation.relationName) {
						tempRelationChangeLibrary.push(item);
					}
				});
				ngCtrl.library.items = tempRelationChangeLibrary;
				relation.addedToLibrary = false;
			}
			ngCtrl.sortLibrary();
		}

		ngCtrl.getRelationType = function (relationId) {
			for (var i = 0; i < ngCtrl.relationsList.length; i++) {
				if (ngCtrl.relationsList[i].id == relationId) {
					return ngCtrl.relationsList[i].relationType;
				}
			}
			return 0;
		}

		//#endregion

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
	//#endregion

})();
