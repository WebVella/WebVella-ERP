/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewManageSidebarController', controller)
		.controller('ManageFromRelationModalController', ManageFromRelationModalController);

	//#region << Configuration >> /////////////////////////
	config.$inject = ['$stateProvider'];
	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-view-manage-sidebar', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/views/:viewName/sidebar',
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
					controller: 'WebVellaAdminEntityViewManageSidebarController',
					templateUrl: '/plugins/webvella-admin/entity-view-manage-sidebar.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRelationsList: resolveEntityRelationsList,
				resolvedViewLibrary: resolveViewLibrary
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
				//Remove the current view from the list to avoid loop
				var libraryWithoutTheCurrentView = [];
				for (var i = 0; i < response.object.length; i++) {
					if (response.object[i].type != "view" && response.object[i].type != "field"
						&& response.object[i].type != "fieldFromRelation" && response.object[i].type != "html") {
						libraryWithoutTheCurrentView.push(response.object[i]);
					}
					else if (response.object[i].type == "view" && response.object[i].viewName != $stateParams.viewName) {
						libraryWithoutTheCurrentView.push(response.object[i]);
					}
				}


				defer.resolve(libraryWithoutTheCurrentView);
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

	//#region << Controller >> ////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal','$timeout',
                            'resolvedCurrentEntityMeta', 'webvellaAdminService', 'ngToast', 'resolvedViewLibrary', 'resolvedEntityRelationsList'];
	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal,$timeout,
                        resolvedCurrentEntityMeta, webvellaAdminService, ngToast, resolvedViewLibrary, resolvedEntityRelationsList) {
		$log.debug('webvellaAdmin>entity-details> START controller.exec ' + moment().format('HH:mm:ss SSSS'));

		/* jshint validthis:true */
		var ngCtrl = this;
		//#region << General init >>
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.pageTitle = "Entity Views | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide side menu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		//#endregion

		//#region << Initialize View >>
		ngCtrl.view = {};
		for (var i = 0; i < ngCtrl.entity.recordViews.length; i++) {
			if (ngCtrl.entity.recordViews[i].name == $stateParams.viewName) {
				ngCtrl.view = fastCopy(ngCtrl.entity.recordViews[i]);
			}
		}
		//#endregion

		//#region << item Library init >>
		var alreadyUsedItemDataNames = [];
		ngCtrl.generateAlreadyUsed = function () {
			alreadyUsedItemDataNames = [];
			for (var i = 0; i < ngCtrl.view.sidebar.items.length; i++) {
				if (ngCtrl.view.sidebar.items[i].meta) {
					alreadyUsedItemDataNames.push(ngCtrl.view.sidebar.items[i].dataName);
				}
			}
		}
		ngCtrl.generateAlreadyUsed();
		ngCtrl.relationsList = fastCopy(resolvedEntityRelationsList);
		ngCtrl.fullLibrary = {};
		ngCtrl.fullLibrary.items = fastCopy(resolvedViewLibrary);
		ngCtrl.fullLibrary.items = ngCtrl.fullLibrary.items.sort(function (a, b) {
			if (a.type < b.type) return -1;
			if (a.type > b.type) return 1;
			return 0;
		});
		ngCtrl.library = {};
		ngCtrl.library.relations = [];
		ngCtrl.library.items = [];

		ngCtrl.sortLibrary = function () {
			ngCtrl.library.items = ngCtrl.library.items.sort(function (a, b) {
				if (a.dataName < b.dataName) return -1;
				if (a.dataName > b.dataName) return 1;
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
			if (generateRelationOptions) {
				ngCtrl.library.relations = [];
			}
			ngCtrl.fullLibrary.items.forEach(function (item) {
				if ((item.meta && alreadyUsedItemDataNames.indexOf(item.dataName) == -1) || !item.meta) {
					//Initially remove all items that are from relation or relationOptions
					switch (item.type) {
						//case "field":
						//	ngCtrl.library.items.push(item);
						//	break;
						case "view":
							if (item.viewId != ngCtrl.view.id) {
								ngCtrl.library.items.push(item);
							}
							break;
						//case "list":
						//	ngCtrl.library.items.push(item);
						//	break;
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
						//case "viewFromRelation":
						//	if(ngCtrl.checkIfRelationAddedToLibrary(item.relationName)){
						//		ngCtrl.library.items.push(item);
						//	}
						//	break;
						//case "listFromRelation":
						//	if(ngCtrl.checkIfRelationAddedToLibrary(item.relationName)){
						//		ngCtrl.library.items.push(item);
						//	}
						//	break;
					}
				}
			});
			ngCtrl.sortLibrary();
		}

		ngCtrl.generateLibrary(true);


		//Extract the direction change information from the view if present
		for (var k = 0; k < ngCtrl.view.relationOptions.length; k++) {
			for (var m = 0; m < ngCtrl.library.relations.length; m++) {
				if (ngCtrl.view.relationOptions[k].relationName == ngCtrl.library.relations[m].relationName) {
					ngCtrl.library.relations[m].direction = ngCtrl.view.relationOptions[k].direction;
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

		//#region << Manage FromRelationModal >>
		var openFromRelationSettingsModal = function (fieldItem, eventObj, orderChangedOnly) {
			//Init
			var droppedItem = fastCopy(fieldItem);
			var relation = null;
			for (var j = 0; j < ngCtrl.relationsList.length; j++) {
				if (ngCtrl.relationsList[j].id == droppedItem.relationId) {
					relation = ngCtrl.relationsList[j];
				}
			}
			if (relation == null) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> item relation not found',
					timeout: 7000
				});
				moveFailure();
				return;
			}

			//Callbacks
			var moveSuccess = function () {
				// Prevent from dragging back to library use remove link instead
				if (eventObj.dest.sortableScope.element[0].id != "library") {
					ngCtrl.regenerateLibrary();
				}
				else {
					//we need to destroy the dropped object

				}
			};
			var moveFailure = function () {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				ngCtrl.regenerateLibrary();
			};

			function successCallback(response) {
				if (response.success) {
					ngToast.create({
						className: 'success',
						content: '<span class="go-green">Success:</span> ' + response.message
					});
					for (var i = 0; i < response.object.regions.length; i++) {
						if (response.object.regions[i].name === "content") {
							ngCtrl.viewContentRegion = response.object.regions[i];
						}
					}
					if (eventObj != null) {
						moveSuccess();
					}
				}
				else {
					errorCallback(response);
					if (eventObj != null) {
						moveFailure();
					}
				}
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				if (eventObj != null) {
					moveFailure();
				}
			}

			function getRelatedEntityMetaSuccessCallback(response) {

				var modalInstance = $uibModal.open({
					animation: false,
					templateUrl: 'manageFromRelationModal.html',
					controller: 'ManageFromRelationModalController',
					controllerAs: "popupCtrl",
					backdrop: 'static',
					size: "",
					resolve: {
						parentData: function () { return ngCtrl; },
						eventObj: eventObj,
						relatedEntityMeta: response.object,
						fieldObj: fieldItem,
						orderChangedOnly: orderChangedOnly
					}
				});

				modalInstance.result.then(function (fieldObject) {
					for (var i = 0; i < ngCtrl.view.sidebar.items.length; i++) {
						if (fieldObject.dataName == ngCtrl.view.sidebar.items[i].dataName) {
							ngCtrl.view.sidebar.items[i] = fieldObject;
						}
					}
					//Remove service properties
					ngCtrl.view = fastCopy(ngCtrl.view);
					////2. Call the service
					webvellaAdminService.updateEntityView(ngCtrl.view, ngCtrl.entity.name, successCallback, errorCallback);
					return;
				});
			}

			function getRelatedEntityMetaErrorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> could not get the related entity meta - ' + response.message,
					timeout: 7000
				});
				moveFailure();
				return;
			}

			//Get the correct related entityMeta

			if (droppedItem.entityName == ngCtrl.entity.name) {
				//the dropped item has relation to the current entity so no reason to make http request
				var response = {};
				response.success = true;
				response.object = ngCtrl.entity;
				getRelatedEntityMetaSuccessCallback(response);
			}
			else {
				var relatedEntityName = null;
				webvellaAdminService.getEntityMeta(droppedItem.entityName, getRelatedEntityMetaSuccessCallback, getRelatedEntityMetaErrorCallback);
			}
		};
		//#endregion

		//#region << Drag & Drop Management >>

		function executeDragViewChange(eventObj, orderChangedOnly) {
			//#region << 1.Define functions >>
			var moveSuccess, moveFailure, successCallback, errorCallback;

			function successCallback(response) {
				if (response.success) {
					ngToast.create({
						className: 'success',
						content: '<span class="go-green">Success:</span> ' + response.message
					});
					ngCtrl.view.sidebar.items = response.object.sidebar.items;
					ngCtrl.regenerateLibrary();
				}
				else {
					errorCallback(response);
					moveFailure();
				}
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				ngCtrl.regenerateLibrary();
			}
			//#endregion

			if ((eventObj.source.itemScope.item.type == "viewFromRelation" || eventObj.source.itemScope.item.type == "listFromRelation") && !orderChangedOnly) {
				openFromRelationSettingsModal(eventObj.source.itemScope.modelValue, eventObj, orderChangedOnly);
			}
			else {
				//1. Clean ngCtrl.view from system properties like $$hashKey
				ngCtrl.view.sidebar.items = fastCopy(ngCtrl.view.sidebar.items);
				//ngCtrl.view = angular.fromJson(angular.toJson(ngCtrl.view));
				////2. Call the service
				webvellaAdminService.updateEntityView(ngCtrl.view, ngCtrl.entity.name, successCallback, errorCallback);
			}
		}

		ngCtrl.dragControlListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				return true
			},
			itemMoved: function (eventObj) {
				//Item is moved from one column to another
				executeDragViewChange(eventObj, true);
			},
			orderChanged: function (eventObj) {
				//Item is moved within the same column
				executeDragViewChange(eventObj, true);
			}
		};

		ngCtrl.libraryDragControlListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				if (sourceItemHandleScope.itemScope.element[0].id != "library" && destSortableScope.element[0].id == "library") {
					return false;
				}
				return true;
			},
			itemMoved: function (eventObj) {
				//Item is moved from one column to another
				executeDragViewChange(eventObj, false);
			},
			orderChanged: function (eventObj) {
				//Item is moved within the same column
				executeDragViewChange(eventObj, true);
			}
		};

		ngCtrl.dragItemRemove = function (itemDataName) {
			ngCtrl.itemScheduledForRemoval = null;
			var index = -1;
			for (var i = 0; i < ngCtrl.view.sidebar.items.length; i++) {
				if (ngCtrl.view.sidebar.items[i].dataName === itemDataName) {
					ngCtrl.itemScheduledForRemoval = ngCtrl.view.sidebar.items[i];
					index = i;
				}
			}


			function successCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> ' + response.message
				});
				ngCtrl.regenerateLibrary();
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				$state.reload();
			}
			ngCtrl.view.sidebar.items.splice(index, 1);
			webvellaAdminService.updateEntityView(ngCtrl.view, ngCtrl.entity.name, successCallback, errorCallback);
		}



		//#endregion

		//#region << Relations >>

		ngCtrl.changeRelationDirection = function (relation) {
			if (relation.direction == "origin-target") {
				relation.direction = "target-origin";
			}
			else {
				relation.direction = "origin-target";
			}
			ngCtrl.view.relationOptions = [];

			for (var i = 0; i < ngCtrl.library.relations.length; i++) {
				var relation = fastCopy(ngCtrl.library.relations[i]);
				delete relation.addedToLibrary;
				delete relation.sameOriginTargetEntity;
				ngCtrl.view.relationOptions.push(relation);
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
			webvellaAdminService.updateEntityView(ngCtrl.view, ngCtrl.entity.name, successCallback, errorCallback);
		}

		ngCtrl.toggleRelationToLibrary = function (relation) {
			if (!relation.addedToLibrary) {
				ngCtrl.fullLibrary.items.forEach(function (item) {
					if (item.relationName && item.relationName == relation.relationName) {
						switch (item.type) {
							//case "fieldFromRelation":
							//	ngCtrl.library.items.push(item);
							//	break;
							case "viewFromRelation":
								if (item.viewId != ngCtrl.view.id) {
									ngCtrl.library.items.push(item);
								}
								break;
							case "listFromRelation":
								ngCtrl.library.items.push(item);
								break;
							case "treeFromRelation":
								ngCtrl.library.items.push(item);
								break;
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
			sortLibrary();
		}

		ngCtrl.getRelationType = function (relationId) {
			for (var i = 0; i < ngCtrl.relationsList.length; i++) {
				if (ngCtrl.relationsList[i].id == relationId) {
					return ngCtrl.relationsList[i].relationType;
				}
			}
			return 0;
		}

		ngCtrl.manageFieldFromRelation = function (item) {
			openFromRelationSettingsModal(item, null);
		}
		//#endregion

		$log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

	}
	//#endregion

	ManageFromRelationModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state', 'eventObj', 'fieldObj', 'relatedEntityMeta'];
	/* @ngInject */
	function ManageFromRelationModalController(parentData, $uibModalInstance, $log, webvellaAdminService, ngToast, $timeout, $state, eventObj, fieldObj, relatedEntityMeta) {
		$log.debug('webvellaAdmin>entities>createRowModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.parentData = fastCopy(parentData);
		popupCtrl.field = fastCopy(fieldObj);
		popupCtrl.entity = fastCopy(relatedEntityMeta);
		popupCtrl.quickCreateViews = [];
		popupCtrl.quickCreateDefaultIndex = -1;
		popupCtrl.lookupLists = [];
		popupCtrl.lookupDefaultIndex = -1;

		popupCtrl.entity.recordViews.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
		popupCtrl.entity.recordLists.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

		//Lookup
		var index = 0;
		for (var i = 0; i < popupCtrl.entity.recordLists.length; i++) {
			if (popupCtrl.entity.recordLists[i].type == "lookup") {
				if (popupCtrl.entity.recordLists[i].default && popupCtrl.lookupDefaultIndex == -1) {
					popupCtrl.lookupDefaultIndex = index;
				}
				popupCtrl.lookupLists.push(popupCtrl.entity.recordLists[i]);
				index++;
			}
		}

		if (popupCtrl.field.fieldLookupList && popupCtrl.field.fieldLookupList != "") {
			//should stick with the selected value
		}
		else if (popupCtrl.quickCreateDefaultIndex > -1 && popupCtrl.lookupLists.length > 0) {
			//no selected so we should preselect the first default;
			popupCtrl.field.fieldLookupList = popupCtrl.lookupLists[popupCtrl.quickCreateDefaultIndex].name;
		}
		else if (popupCtrl.lookupLists.length > 0) {
			popupCtrl.field.fieldLookupList = popupCtrl.lookupLists[0].name;
		}
		else {
			//should alert for error
			popupCtrl.error = true;
			popupCtrl.errorMessage = "The target entity '" + popupCtrl.entity.name + "' has no 'lookup' lists. It should have at least one";
		}

		//Quick create
		index = 0;
		for (var i = 0; i < popupCtrl.entity.recordViews.length; i++) {
			if (popupCtrl.entity.recordViews[i].type == "quick_create") {
				if (popupCtrl.entity.recordViews[i].default && popupCtrl.quickCreateDefaultIndex == -1) {
					popupCtrl.quickCreateDefaultIndex = index;
				}
				popupCtrl.quickCreateViews.push(popupCtrl.entity.recordViews[i]);
				index++;
			}
		}
		if (popupCtrl.field.fieldManageView && popupCtrl.field.fieldManageView != "") {
			//should stick with the selected value
		}
		else if (popupCtrl.lookupDefaultIndex > -1 && popupCtrl.quickCreateViews.length > 0) {
			//no selected so we should preselect the first default;
			popupCtrl.field.fieldManageView = popupCtrl.quickCreateViews[popupCtrl.lookupDefaultIndex].name;
		}
		else if (popupCtrl.quickCreateViews.length > 0) {
			popupCtrl.field.fieldManageView = popupCtrl.quickCreateViews[0].name;
		}
		else if (popupCtrl.field.type == "listFromRelation" || popupCtrl.field.type == "viewFromRelation") {

			//should alert for error if it is list or view
			popupCtrl.error = true;
			popupCtrl.errorMessage = "The target entity '" + popupCtrl.entity.name + "' has no 'quick_create' views. It should have at least one";
		}


		popupCtrl.ok = function () {
			$uibModalInstance.close(popupCtrl.field);
		};

		popupCtrl.cancel = function () {
			if (eventObj != null) {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				//we are currently copying so no need to return it back
				//eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.task);
			}
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;

		}
		$log.debug('webvellaAdmin>entities>createRowModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


})();
