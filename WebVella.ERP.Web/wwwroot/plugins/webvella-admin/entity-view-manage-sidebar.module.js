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
					templateUrl: '/plugins/webvella-admin/topnav.view.html?v=' + htmlCacheBreaker,
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAdminSidebarController',
					templateUrl: '/plugins/webvella-admin/sidebar.view.html?v=' + htmlCacheBreaker,
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaAdminEntityViewManageSidebarController',
					templateUrl: '/plugins/webvella-admin/entity-view-manage-sidebar.view.html?v=' + htmlCacheBreaker,
					controllerAs: 'contentData'
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
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal',
                            'resolvedCurrentEntityMeta', 'webvellaAdminService', 'ngToast', 'resolvedViewLibrary', 'resolvedEntityRelationsList'];
	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal,
                        resolvedCurrentEntityMeta, webvellaAdminService, ngToast, resolvedViewLibrary, resolvedEntityRelationsList) {
		$log.debug('webvellaAdmin>entity-details> START controller.exec ' + moment().format('HH:mm:ss SSSS'));

		/* jshint validthis:true */
		var contentData = this;
		//#region << Initialize Current Entity >>
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		//#endregion

		//#region << Update page title & Hide side menu>>
		contentData.pageTitle = "Entity Views | " + pageTitle;
		$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
		//Hide side menu
		$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		$scope.$on("$destroy", function () {
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		});
		//#endregion

		//#region << Initialize View and Content Region >>
		contentData.view = {};
		for (var i = 0; i < contentData.entity.recordViews.length; i++) {
			if (contentData.entity.recordViews[i].name == $stateParams.viewName) {
				contentData.view = fastCopy(contentData.entity.recordViews[i]);
			}
		}

		var alreadyUsedItemIds = [];
		contentData.generateAlreadyUsed = function () {
			for (var i = 0; i < contentData.view.sidebar.items.length; i++) {
				if (contentData.view.sidebar.items[i].meta) {
					alreadyUsedItemIds.push(contentData.view.sidebar.items[i].meta.id);
				}
			}
		}
		contentData.generateAlreadyUsed();
		contentData.relationsList = fastCopy(resolvedEntityRelationsList);
		contentData.tempLibrary = {};
		contentData.tempLibrary.items = fastCopy(resolvedViewLibrary);
		contentData.tempLibrary.items = contentData.tempLibrary.items.sort(function (a, b) {
			if (a.type < b.type) return -1;
			if (a.type > b.type) return 1;
			return 0;
		});
		contentData.library = {};
		contentData.library.relations = [];
		contentData.library.items = [];
		contentData.tempLibrary.items.forEach(function (item) {
			if ((item.meta && alreadyUsedItemIds.indexOf(item.meta.id) == -1) || !item.meta) {
				//Initially remove all items that are from relation or relationOptions
				switch (item.type) {
					//case "field":
					//	contentData.library.items.push(item);
					//	break;
					case "view":
						if (item.viewId != contentData.view.id) {
							contentData.library.items.push(item);
						}
						break;
					case "list":
						contentData.library.items.push(item);
						break;
					case "relationOptions":
						item.addedToLibrary = false;
						item.sameOriginTargetEntity = false;
						for (var r = 0; r < contentData.relationsList.length; r++) {
							if (item.relationName == contentData.relationsList[r].name && contentData.relationsList[r].originEntityId == contentData.relationsList[r].targetEntityId) {
								item.sameOriginTargetEntity = true;
							}
						}
						contentData.library.relations.push(item);
						break;
				}
			}
		});
		function sortLibrary() {
			contentData.library.items = contentData.library.items.sort(function (a, b) {
				if (a.dataName < b.dataName) return -1;
				if (a.dataName > b.dataName) return 1;
				return 0;
			});
		}
		sortLibrary();
		contentData.originalLibrary = fastCopy(contentData.library.items);


		//Extract the direction change information from the view if present
		for (var k = 0; k < contentData.view.relationOptions.length; k++) {
			for (var m = 0; m < contentData.library.relations.length; m++) {
				if (contentData.view.relationOptions[k].relationName == contentData.library.relations[m].relationName) {
					contentData.library.relations[m].direction = contentData.view.relationOptions[k].direction;
				}

			}

		}

		contentData.library.relations = contentData.library.relations.sort(function (a, b) {
			if (a.relationName < b.relationName) return -1;
			if (a.relationName > b.relationName) return 1;
			return 0;
		});


		//#endregion

		//#region << Manage FromRelationModal >>
		var openFromRelationSettingsModal = function (fieldItem, eventObj, orderChangedOnly) {
			//Init
			var droppedItem = fastCopy(fieldItem);
			var relation = null;
			for (var j = 0; j < contentData.relationsList.length; j++) {
				if (contentData.relationsList[j].id == droppedItem.relationId) {
					relation = contentData.relationsList[j];
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

				}
				else {
					//we need to destroy the dropped object

				}
			};
			var moveFailure = function () {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				//we are copying them currently only
				//eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.item);
			};

			function successCallback(response) {
				if (response.success) {
					ngToast.create({
						className: 'success',
						content: '<span class="go-green">Success:</span> ' + response.message
					});
					//contentData.library.items = fastCopy(contentData.originalLibrary);
					for (var i = 0; i < response.object.regions.length; i++) {
						if (response.object.regions[i].name === "content") {
							contentData.viewContentRegion = response.object.regions[i];
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
					controllerAs: "popupData",
					backdrop: 'static',
					size: "",
					resolve: {
						parentData: function () { return contentData; },
						eventObj: eventObj,
						relatedEntityMeta: response.object,
						fieldObj: fieldItem,
						orderChangedOnly: orderChangedOnly
					}
				});

				modalInstance.result.then(function (fieldObject) {
					for (var i = 0; i < contentData.view.sidebar.items.length; i++) {
						if (fieldObject.dataName == contentData.view.sidebar.items[i].dataName) {
							contentData.view.sidebar.items[i] = fieldObject;
						}
					}
					//Remove service properties
					contentData.view = fastCopy(contentData.view);
					////2. Call the service
					webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
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

			if (droppedItem.entityName == contentData.entity.name) {
				//the dropped item has relation to the current entity so no reason to make http request
				var response = {};
				response.success = true;
				response.object = contentData.entity;
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
					//contentData.library.items = fastCopy(contentData.originalLibrary);
					contentData.view.sidebar.items = response.object.sidebar.items;
					contentData.generateAlreadyUsed();
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
			}
			//#endregion

			if ((eventObj.source.itemScope.item.type == "viewFromRelation" || eventObj.source.itemScope.item.type == "listFromRelation") && !orderChangedOnly) {
				openFromRelationSettingsModal(eventObj.source.itemScope.modelValue, eventObj, orderChangedOnly);
			}
			else {
				//1. Clean contentData.view from system properties like $$hashKey
				contentData.view.sidebar.items = fastCopy(contentData.view.sidebar.items);
				//contentData.view = angular.fromJson(angular.toJson(contentData.view));
				////2. Call the service
				webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
			}
		}

		contentData.dragControlListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				return true
			},
			itemMoved: function (eventObj) {
				//Item is moved from one column to another
				executeDragViewChange(eventObj,true);
			},
			orderChanged: function (eventObj) {
				//Item is moved within the same column
				executeDragViewChange(eventObj,true);
			}
		};

		contentData.libraryDragControlListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				if (sourceItemHandleScope.itemScope.element[0].id != "library" && destSortableScope.element[0].id == "library") {
					return false;
				}
				return true;
			},
			itemMoved: function (eventObj) {
				//Item is moved from one column to another
				executeDragViewChange(eventObj,false);
			},
			orderChanged: function (eventObj) {
				//Item is moved within the same column
				executeDragViewChange(eventObj, true);
			}
		};

		contentData.dragItemRemove = function (index) {
			contentData.itemScheduledForRemoval = contentData.view.sidebar.items[index];
			function successCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> ' + response.message
				});
				contentData.library.items.push(contentData.itemScheduledForRemoval);
				sortLibrary();
				if (contentData.itemScheduledForRemoval.meta) {
					var itemIndexInUsed = alreadyUsedItemIds.indexOf(contentData.itemScheduledForRemoval.meta.id);
					if (itemIndexInUsed > -1) {
						alreadyUsedItemIds.slice(itemIndexInUsed, 1);
					}
				}
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				$state.reload();
			}
			contentData.view.sidebar.items.splice(index, 1);
			webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
		}



		//#endregion

		//#region << Relations >>

		contentData.changeRelationDirection = function (relation) {
			if (relation.direction == "origin-target") {
				relation.direction = "target-origin";
			}
			else {
				relation.direction = "origin-target";
			}
			contentData.view.relationOptions = [];

			for (var i = 0; i < contentData.library.relations.length; i++) {
				var relation = fastCopy(contentData.library.relations[i]);
				delete relation.addedToLibrary;
				delete relation.sameOriginTargetEntity;
				contentData.view.relationOptions.push(relation);
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
				for (var j = 0; j < contentData.library.relations.length; j++) {
					if (contentData.library.relations[j].relationName == relation.relationName) {
						if (contentData.library.relations[j].direction == "origin-target") {
							contentData.library.relations[j].direction = "target-origin";
						}
						else {
							contentData.library.relations[j].direction = "origin-target";
						}
					}
				}
			}
			webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
		}

		contentData.toggleRelationToLibrary = function (relation) {
			if (!relation.addedToLibrary) {
				contentData.tempLibrary.items.forEach(function (item) {
					if (item.relationName && item.relationName == relation.relationName) {
						switch (item.type) {
							//case "fieldFromRelation":
							//	contentData.library.items.push(item);
							//	break;
							case "viewFromRelation":
								if (item.viewId != contentData.view.id) {
									contentData.library.items.push(item);
								}
								break;
							case "listFromRelation":
								contentData.library.items.push(item);
								break;
							case "treeFromRelation":
								contentData.library.items.push(item);
								break;
						}
					}
				});
				relation.addedToLibrary = true;
			}
			else {
				var tempRelationChangeLibrary = [];
				contentData.library.items.forEach(function (item) {
					if (!item.relationName) {
						tempRelationChangeLibrary.push(item);
					}
					else if (item.relationName != relation.relationName) {
						tempRelationChangeLibrary.push(item);
					}
				});
				contentData.library.items = tempRelationChangeLibrary;
				relation.addedToLibrary = false;
			}
			sortLibrary();
		}

		contentData.getRelationType = function (relationId) {
			for (var i = 0; i < contentData.relationsList.length; i++) {
				if (contentData.relationsList[i].id == relationId) {
					return contentData.relationsList[i].relationType;
				}
			}
			return 0;
		}

		contentData.manageFieldFromRelation = function (item) {
			openFromRelationSettingsModal(item, null);
		}
		//#endregion

		$log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

	}
	//#endregion

	ManageFromRelationModalController.$inject = ['parentData', '$modalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state', 'eventObj', 'fieldObj', 'relatedEntityMeta'];
	/* @ngInject */
	function ManageFromRelationModalController(parentData, $modalInstance, $log, webvellaAdminService, ngToast, $timeout, $state, eventObj, fieldObj, relatedEntityMeta) {
		$log.debug('webvellaAdmin>entities>createRowModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.parentData = fastCopy(parentData);
		popupData.field = fastCopy(fieldObj);
		popupData.entity = fastCopy(relatedEntityMeta);
		popupData.quickCreateViews = [];
		popupData.quickCreateDefaultIndex = -1;
		popupData.lookupLists = [];
		popupData.lookupDefaultIndex = -1;

		popupData.entity.recordViews.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
		popupData.entity.recordLists.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

		//Lookup
		var index = 0;
		for (var i = 0; i < popupData.entity.recordLists.length; i++) {
			if (popupData.entity.recordLists[i].type == "lookup") {
				if (popupData.entity.recordLists[i].default && popupData.lookupDefaultIndex == -1) {
					popupData.lookupDefaultIndex = index;
				}
				popupData.lookupLists.push(popupData.entity.recordLists[i]);
				index++;
			}
		}

		if (popupData.field.fieldLookupList && popupData.field.fieldLookupList != "") {
			//should stick with the selected value
		}
		else if (popupData.quickCreateDefaultIndex > -1 && popupData.lookupLists.length > 0) {
			//no selected so we should preselect the first default;
			popupData.field.fieldLookupList = popupData.lookupLists[popupData.quickCreateDefaultIndex].name;
		}
		else if (popupData.lookupLists.length > 0) {
			popupData.field.fieldLookupList = popupData.lookupLists[0].name;
		}
		else {
			//should alert for error
			popupData.error = true;
			popupData.errorMessage = "The target entity '" + popupData.entity.name + "' has no 'lookup' lists. It should have at least one";
		}

		//Quick create
		index = 0;
		for (var i = 0; i < popupData.entity.recordViews.length; i++) {
			if (popupData.entity.recordViews[i].type == "quick_create") {
				if (popupData.entity.recordViews[i].default && popupData.quickCreateDefaultIndex == -1) {
					popupData.quickCreateDefaultIndex = index;
				}
				popupData.quickCreateViews.push(popupData.entity.recordViews[i]);
				index++;
			}
		}
		if (popupData.field.fieldManageView && popupData.field.fieldManageView != "") {
			//should stick with the selected value
		}
		else if (popupData.lookupDefaultIndex > -1 && popupData.quickCreateViews.length > 0) {
			//no selected so we should preselect the first default;
			popupData.field.fieldManageView = popupData.quickCreateViews[popupData.lookupDefaultIndex].name;
		}
		else if (popupData.quickCreateViews.length > 0) {
			popupData.field.fieldManageView = popupData.quickCreateViews[0].name;
		}
		else if (popupData.field.type == "listFromRelation" || popupData.field.type == "viewFromRelation") {

			//should alert for error if it is list or view
			popupData.error = true;
			popupData.errorMessage = "The target entity '" + popupData.entity.name + "' has no 'quick_create' views. It should have at least one";
		}


		popupData.ok = function () {
			$modalInstance.close(popupData.field);
		};

		popupData.cancel = function () {
			if (eventObj != null) {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				//we are currently copying so no need to return it back
				//eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.task);
			}
			$modalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$modalInstance.close('success');
		}

		function errorCallback(response) {
			popupData.hasError = true;
			popupData.errorMessage = response.message;

		}
		$log.debug('webvellaAdmin>entities>createRowModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


})();
