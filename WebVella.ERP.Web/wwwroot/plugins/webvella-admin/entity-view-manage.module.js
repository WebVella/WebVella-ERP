/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewManageController', controller)
        .controller('ManageSectionModalController', ManageSectionModalController)
        .controller('ManageRowModalController', ManageRowModalController)
		.controller('ManageFromRelationModalController', ManageFromRelationModalController);

	//#region << Configuration >> /////////////////////////
	config.$inject = ['$stateProvider'];
	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-view-manage', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/views/:viewName',
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
					controller: 'WebVellaAdminEntityViewManageController',
					templateUrl: '/plugins/webvella-admin/entity-view-manage.view.html',
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
			if (resolvedCurrentUser.roles[i] === "bdc56420-caf0-4030-8a0e-d264938e0cda") {
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
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
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
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
				}, 0);
			}
			else {
				//Remove the current view from the list to avoid loop
				var libraryWithoutTheCurrentView = [];

				for (var i = 0; i < response.object.length; i++) {
					if (response.object[i].type !== "view") {
						libraryWithoutTheCurrentView.push(response.object[i]);
					}
					else if (response.object[i].type === "view" && response.object[i].viewName !== $stateParams.viewName) {
						libraryWithoutTheCurrentView.push(response.object[i]);
					}
				}
				defer.resolve(libraryWithoutTheCurrentView);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
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
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				$timeout(function () {
					alert("error in response!");
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
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$stateParams', '$timeout', 'pageTitle', '$uibModal',
                            'resolvedCurrentEntityMeta', 'webvellaAdminService', 'ngToast', 'resolvedViewLibrary', 'resolvedEntityRelationsList'];
	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, $stateParams, $timeout, pageTitle, $uibModal,
                        resolvedCurrentEntityMeta, webvellaAdminService, ngToast, resolvedViewLibrary, resolvedEntityRelationsList) {
		$log.debug('webvellaAdmin>entity-details> START controller.exec ' + moment().format('HH:mm:ss SSSS'));

		/* jshint validthis:true */
		var contentData = this;
		//#region << General init >>
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		contentData.pageTitle = "Entity Views | " + pageTitle;
		$timeout(function(){
		$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
		//Hide side menu
		$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);
		$rootScope.currentSectionName = "Entities";
		//#endregion

		//#region << Initialize View and Content Region >>
		contentData.view = {};
		for (var i = 0; i < contentData.entity.recordViews.length; i++) {
			if (contentData.entity.recordViews[i].name === $stateParams.viewName) {
				contentData.view = fastCopy(contentData.entity.recordViews[i]);
			}
		}
		contentData.viewContentRegion = {};
		for (var i = 0; i < contentData.view.regions.length; i++) {
			if (contentData.view.regions[i].name === "content") {
				contentData.viewContentRegion = contentData.view.regions[i];
			}
		}
		//#endregion

		//#region << item Library init >>
		var alreadyUsedItemDataNames = [];
		//Get all items from the view and add their dataNames in the already used 
		contentData.generateAlreadyUsed = function () {
			alreadyUsedItemDataNames = [];
			for (var i = 0; i < contentData.viewContentRegion.sections.length; i++) {
				for (var j = 0; j < contentData.viewContentRegion.sections[i].rows.length; j++) {
					for (var k = 0; k < contentData.viewContentRegion.sections[i].rows[j].columns.length; k++) {
						for (var m = 0; m < contentData.viewContentRegion.sections[i].rows[j].columns[k].items.length; m++) {
							if (contentData.viewContentRegion.sections[i].rows[j].columns[k].items[m].meta) {
								alreadyUsedItemDataNames.push(contentData.viewContentRegion.sections[i].rows[j].columns[k].items[m].dataName); //dataName should be used instead meta.id to cover the case with same items from different relations (or no relation and with relation)
							}
						}
					}
				}
			}
		}
		contentData.generateAlreadyUsed();
		contentData.relationsList = fastCopy(resolvedEntityRelationsList);
		contentData.fullLibrary = {};
		contentData.fullLibrary.items = fastCopy(resolvedViewLibrary);
		contentData.library = {};
		contentData.library.relations = [];
		contentData.library.items = [];
		contentData.sortLibrary = function () {
			contentData.library.items = contentData.library.items.sort(function (a, b) {
				if (a.fieldName < b.fieldName) return -1;
				if (a.fieldName > b.fieldName) return 1;
				return 0;
			});
		}
		contentData.checkIfRelationAddedToLibrary = function(relationName){
			if(contentData.library.relations.length > 0){
				for (var i = 0; i < contentData.library.relations.length; i++) {
					 if(contentData.library.relations[i].relationName ===  relationName  && contentData.library.relations[i].addedToLibrary){
						return true;
					 }
				}
				return false;
			}
			else {
				return false;
			}
		}

		contentData.generateLibrary = function (generateRelationOptions) {
			contentData.library.items = [];
			if(generateRelationOptions){
				contentData.library.relations = [];
			}
			contentData.fullLibrary.items.forEach(function (item) {
				//Initially remove all items that are from not activated relation and the relationOptions
				if ((item.meta && alreadyUsedItemDataNames.indexOf(item.dataName) === -1) || !item.meta) {
					switch (item.type) {
						case "field":
							contentData.library.items.push(item);
							break;
						case "view":
							if (item.viewId !== contentData.view.id) {
								contentData.library.items.push(item);
							}
							break;
						case "list":
							contentData.library.items.push(item);
							break;
						case "relationOptions":
							if(generateRelationOptions){
								item.addedToLibrary = false;
								item.sameOriginTargetEntity = false;
								for (var r = 0; r < contentData.relationsList.length; r++) {
									if (item.relationName === contentData.relationsList[r].name && contentData.relationsList[r].originEntityId === contentData.relationsList[r].targetEntityId) {
										item.sameOriginTargetEntity = true;
									}
								}
								contentData.library.relations.push(item);
							}
							break;
						case "fieldFromRelation":
							if(contentData.checkIfRelationAddedToLibrary(item.relationName)){
								contentData.library.items.push(item);
							}
							break;
						case "viewFromRelation":
							if(contentData.checkIfRelationAddedToLibrary(item.relationName)){
								contentData.library.items.push(item);
							}
							break;
						case "listFromRelation":
							if(contentData.checkIfRelationAddedToLibrary(item.relationName)){
								contentData.library.items.push(item);
							}
							break;
						case "treeFromRelation":
							if(contentData.checkIfRelationAddedToLibrary(item.relationName)){
								contentData.library.items.push(item);
							}
							break;
					}
				}
			});
			contentData.sortLibrary();
		}

		contentData.generateLibrary(true);

		//Extract the direction change information from the view if present
		for (var k = 0; k < contentData.view.relationOptions.length; k++) {
			for (var m = 0; m < contentData.library.relations.length; m++) {
				if (contentData.view.relationOptions[k].relationName === contentData.library.relations[m].relationName) {
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
 
		//#region << Regenerate library >>
		contentData.regenerateLibrary = function () {
			contentData.generateAlreadyUsed();
			contentData.generateLibrary(false);
		}

		//#endregion

		//#region << Section Management >>

		//Create or Update view section
		contentData.manageSectionModalOpen = function (sectionObj, weight) {
			// ReSharper disable once UnusedLocals
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageSectionModal.html',
				controller: 'ManageSectionModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					parentData: function () { return contentData; },
					section: function () { return sectionObj },
					weight: function () { return weight }
				}
			});

		}

		//Remove section
		var tempCopyView = {};
		var tempCopyViewRegion = {}
		contentData.removeSection = function (id) {
			var isConfirmed = confirm("Are you sure that you need to remove this section?");
			if (isConfirmed === true) {
				// 1. Copy the view and contentRegion in a temp object
				tempCopyView = fastCopy(contentData.view);
				tempCopyViewRegion = fastCopy(contentData.viewContentRegion);
				// 2. Apply the change to the temp object
				tempCopyViewRegion.sections = webvellaAdminService.safeRemoveArrayPlace(tempCopyViewRegion.sections, id);
				// 3. Apply the changes of the temp ContentViewRegion to the temp view object
				for (var i = 0; i < tempCopyView.regions.length; i++) {
					if (tempCopyView.regions[i].name === "content") {
						tempCopyView.regions[i] = tempCopyViewRegion;
					}
				}
				//Try update with the new view
				webvellaAdminService.updateEntityView(tempCopyView, contentData.entity.name, successSectionRemoveCallback, errorSectionRemoveCallback);

			}
		}

		function successSectionRemoveCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});

			//Initialize both view and the content region with the new value
			contentData.view = tempCopyView;
			contentData.viewContentRegion = tempCopyViewRegion;
			contentData.regenerateLibrary();
		}
		function errorSectionRemoveCallback(response) {
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});
		}

		//#endregion

		//#region << Row Management >>

		//Create view row
		contentData.manageRowModalOpen = function (rowObj, sectionObj, weight) {
			// ReSharper disable once UnusedLocals
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageRowModal.html',
				controller: 'ManageRowModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					parentData: function () { return contentData; },
					row: function () { return rowObj },
					section: function () { return sectionObj },
					weight: function () { return weight }
				}
			});

		}

		//Remove row
		contentData.removeRow = function (id, sectionId) {
			var isConfirmed = confirm("Are you sure that you need to remove this row?");
			if (isConfirmed === true) {
				// 1. Copy the view and contentRegion in a temp object
				var tempCopyView = fastCopy(contentData.view);
				var tempCopyViewRegion = fastCopy(contentData.viewContentRegion);
				// 2. Apply the change to the temp object
				for (var m = 0; m < tempCopyViewRegion.sections.length; m++) {
					if (tempCopyViewRegion.sections[m].id === sectionId) {
						tempCopyViewRegion.sections[m].rows = webvellaAdminService.safeRemoveArrayPlace(tempCopyViewRegion.sections[m].rows, id);
					}
				}
				// 3. Apply the changes of the temp ContentViewRegion to the temp view object
				for (var i = 0; i < tempCopyView.regions.length; i++) {
					if (tempCopyView.regions[i].name === "content") {
						tempCopyView.regions[i] = tempCopyViewRegion;
					}
				}
				//Try update with the new view
				webvellaAdminService.updateEntityView(tempCopyView, contentData.entity.name, successRowRemoveCallback, errorRowRemoveCallback);

			}
		}
		function successRowRemoveCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});

			//Initialize both view and the content region with the new value

			contentData.view = response.object;
			for (var i = 0; i < response.object.regions.length; i++) {
				if (response.object.regions[i].name === "content") {
					contentData.viewContentRegion = response.object.regions[i];
				}
			}
			contentData.regenerateLibrary();
		}
		function errorRowRemoveCallback(response) {
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});
		}

		//#endregion

		//#region << Manage FromRelationModal >>
		var openFromRelationSettingsModal = function (fieldItem, eventObj, orderChangedOnly) {
			//Init
			var moveFailure = function () {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				contentData.regenerateLibrary();
			};

			var droppedItem = fastCopy(fieldItem);
			var relation = null;
			for (var j = 0; j < contentData.relationsList.length; j++) {
				if (contentData.relationsList[j].id === droppedItem.relationId) {
					relation = contentData.relationsList[j];
				}
			}
			if (relation === null) {
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
				if (eventObj.dest.sortableScope.element[0].id !== "library") {
					contentData.regenerateLibrary();
				}

			};

			function successCallback(response) {
				if (response.success) {
					ngToast.create({
						className: 'success',
						content: '<span class="go-green">Success:</span> ' + response.message
					});
					//Creates error - Unbinds the contentData.ViewContentRegion from the drop zone so it needs to be commented until fixed
					//for (var i = 0; i < response.object.regions.length; i++) {
					//	if (response.object.regions[i].name === "content") {
					//		contentData.viewContentRegion = response.object.regions[i];
					//	}
					//}
					if (eventObj !== null) {
						moveSuccess();
					}
				}
				else {
					errorCallback(response);
					if (eventObj !== null) {
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
				if (eventObj !== null) {
					moveFailure();
				}
			}

			function openModal(response) {
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
					for (var k = 0; k < contentData.viewContentRegion.sections.length; k++) {
						for (var l = 0; l < contentData.viewContentRegion.sections[k].rows.length; l++) {
							for (var m = 0; m < contentData.viewContentRegion.sections[k].rows[l].columns.length; m++) {
								for (var n = 0; n < contentData.viewContentRegion.sections[k].rows[l].columns[m].items.length; n++) {
									if (fieldObject.type === "fieldFromRelation" && contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].dataName === fieldObject.dataName) {
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldLabel = fieldObject.fieldLabel;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldPlaceholder = fieldObject.fieldPlaceholder;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldHelpText = fieldObject.fieldHelpText;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldRequired = fieldObject.fieldRequired;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldLookupList = fieldObject.fieldLookupList;
									}
									else if (fieldObject.type === "viewFromRelation" && contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].dataName === fieldObject.dataName) {
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldLabel = fieldObject.fieldLabel;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldPlaceholder = fieldObject.fieldPlaceholder;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldHelpText = fieldObject.fieldHelpText;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldRequired = fieldObject.fieldRequired;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldLookupList = fieldObject.fieldLookupList;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldManageView = fieldObject.fieldManageView;
									}
									else if (fieldObject.type === "listFromRelation" && contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].dataName === fieldObject.dataName) {
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldLabel = fieldObject.fieldLabel;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldPlaceholder = fieldObject.fieldPlaceholder;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldHelpText = fieldObject.fieldHelpText;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldRequired = fieldObject.fieldRequired;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldLookupList = fieldObject.fieldLookupList;
										contentData.viewContentRegion.sections[k].rows[l].columns[m].items[n].fieldManageView = fieldObject.fieldManageView;
									}
								}
							}
						}
					}

					var tempView = fastCopy(contentData.view);
					for (var i = 0; i < tempView.regions.length; i++) {
						if (tempView.regions[i].name == "content") {
							tempView.regions[i] = contentData.viewContentRegion;
						}
					}
					////2. Call the service
					webvellaAdminService.updateEntityView(tempView, contentData.entity.name, successCallback, errorCallback);
					//return;
				});

			}

			function getRelatedEntityMetaSuccessCallback(response) {
				openModal(response);
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

			if (droppedItem.entityName === contentData.entity.name) {
				//the dropped item has relation to the current entity so no reason to make http request
				var response = {};
				response.success = true;
				response.object = contentData.entity;
				getRelatedEntityMetaSuccessCallback(response);
			}
			else {
				webvellaAdminService.getEntityMeta(droppedItem.entityName, getRelatedEntityMetaSuccessCallback, getRelatedEntityMetaErrorCallback);
			}
		};
		//#endregion

		//#region << Drag & Drop Management >>

		function executeDragViewChange(eventObj, orderChangedOnly) {

			var moveSuccess = function () {
				// Prevent from dragging back to library use remove link instead
				if (eventObj.dest.sortableScope.element[0].id !== "library") {
	 				contentData.regenerateLibrary();
				}
			};
			var moveFailure = function () {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.item);
				contentData.regenerateLibrary();
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
					moveSuccess();
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
				moveFailure();
			}

			if ((eventObj.source.itemScope.item.type === "fieldFromRelation" || eventObj.source.itemScope.item.type === "viewFromRelation" || eventObj.source.itemScope.item.type === "listFromRelation") && !orderChangedOnly) {
				$timeout(function () {
					openFromRelationSettingsModal(eventObj.source.itemScope.modelValue, eventObj, orderChangedOnly);
				}, 0);
			}
			else if (eventObj.source.itemScope.item.type === "html" && !orderChangedOnly) {
				$timeout(function () {
					openHtmlContentModal(eventObj.source.itemScope.modelValue, eventObj, orderChangedOnly);
				}, 0);
			}
			else {
				//cannot be managed
				for (var i = 0; i < contentData.view.regions.length; i++) {
					if (contentData.view.regions[i].name === "content") {
						contentData.view.regions[i] = fastCopy(contentData.viewContentRegion);
					}
				}
				webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);

			}
		}

		contentData.dragControlListeners = {
			accept: function () {
				//// Prevent from the same field in the same column
				//var draggedDataName = sourceItemHandleScope.itemScope.modelValue.dataName;
				//for(var k = 0; k< destSortableScope.modelValue.length;k++){
				//	if(destSortableScope.modelValue[k].dataName === draggedDataName){
				//		return false;
				//	}

				//}
				return true;
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


		contentData.libraryDragControlListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				if (sourceItemHandleScope.itemScope.element[0].id !== "library" && destSortableScope.element[0].id === "library") {
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
			},
			clone: false
		};

		contentData.dragItemRemove = function (column, itemDataName) {
			contentData.itemScheduledForRemoval = null;
			var index = -1;
			for (var i = 0; i < column.items.length; i++) {
				if (column.items[i].dataName === itemDataName) {
					contentData.itemScheduledForRemoval = column.items[i];
					index = i;
				}
			}
			function successCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> ' + response.message
				});
				contentData.regenerateLibrary();
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				$state.reload();
			}

			column.items.splice(index, 1);
			for (var i = 0; i < contentData.view.regions.length; i++) {
				if (contentData.view.regions[i].name === "content") {
					contentData.view.regions[i] = fastCopy(contentData.viewContentRegion);
				}
			}
			webvellaAdminService.updateEntityView(contentData.view, contentData.entity.name, successCallback, errorCallback);
		}

		//#endregion

		//#region << Relations >>

		contentData.changeRelationDirection = function (relation) {
			if (relation.direction === "origin-target") {
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
					if (contentData.library.relations[j].relationName === relation.relationName) {
						if (contentData.library.relations[j].direction === "origin-target") {
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
				contentData.fullLibrary.items.forEach(function (item) {
					if (item.relationName && item.relationName === relation.relationName) {
						if (item.meta && alreadyUsedItemDataNames.indexOf(item.dataName) === -1) {
							switch (item.type) {
								case "fieldFromRelation":
									contentData.library.items.push(item);
									break;
								case "viewFromRelation":
									if (item.viewId !== contentData.view.id) {
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
					else if (item.relationName !== relation.relationName) {
						tempRelationChangeLibrary.push(item);
					}
				});
				contentData.library.items = tempRelationChangeLibrary;
				relation.addedToLibrary = false;
			}
			contentData.sortLibrary();
		}

		contentData.getRelationType = function (relationId) {
			for (var i = 0; i < contentData.relationsList.length; i++) {
				if (contentData.relationsList[i].id === relationId) {
					return contentData.relationsList[i].relationType;
				}
			}
			return 0;
		}

		contentData.manageFieldFromRelation = function (item) {
			openFromRelationSettingsModal(item, null, false);
		}

		//#endregion



		$log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

	}
	//#endregion

	//#region << Modal Controllers >> /////////////////////

	//Section Modal
	ManageSectionModalController.$inject = ['parentData', 'section', 'weight', '$uibModalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state', '$scope'];
	/* @ngInject */
	function ManageSectionModalController(parentData, section, weight, $uibModalInstance, $log, webvellaAdminService, ngToast, $timeout, $state, $scope) {
		$log.debug('webvellaAdmin>entities>createSectionModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */

		//#region << Initialize >>
		var popupData = this;
		popupData.section = null;
		popupData.isUpdate = true;
		popupData.isValid = true;
		if (section === null) {
			popupData.isUpdate = false;
			popupData.section = fastCopy(webvellaAdminService.initViewSection());
			popupData.section.weight = weight;
		}
		else {
			popupData.section = fastCopy(section);
		}
		//#endregion

		popupData.ok = function () {
			popupData.view = fastCopy(parentData.view);
			//Find the content region, which is subject of this screen
			popupData.viewContentRegion = {};
			for (var i = 0; i < popupData.view.regions.length; i++) {
				if (popupData.view.regions[i].name === "content") {
					popupData.viewContentRegion = popupData.view.regions[i];
				}
			}
			// Validate unique username on add. It cannot be managed on update
			if (!popupData.isUpdate) {
				popupData.isValid = true;
				if (!popupData.viewContentRegion.sections) {
					popupData.viewContentRegion.sections = []; //If the view was newly created the viewContentRegion will be an empty object
				}
				for (var i = 0; i < popupData.viewContentRegion.sections.length; i++) {
					if (popupData.viewContentRegion.sections[i].name === popupData.section.name) {
						popupData.isValid = false;
					}
				}
				if (!popupData.isValid) {
					$scope.manageSection.name.$dirty = true;
					$scope.manageSection.name.$invalid = true;
					$scope.manageSection.name.$pristine = false;
					$scope.manageSection.name.$setValidity("unique", false);
				}
			}
			//#region << Update the temporary view object for submission >>
			if (popupData.isUpdate && popupData.isValid) {
				popupData.viewContentRegion.sections = webvellaAdminService.safeUpdateArrayPlace(popupData.section, popupData.viewContentRegion.sections);
			}
			else if (popupData.isValid) {
				popupData.viewContentRegion.sections = webvellaAdminService.safeAddArrayPlace(popupData.section, popupData.viewContentRegion.sections);
			}
			//#endregion

			if (popupData.isValid) {
				//Update the view with the correct values for the content region
				for (var i = 0; i < popupData.view.regions.length; i++) {
					if (popupData.view.regions[i].name === "content") {
						popupData.view.regions[i] = popupData.viewContentRegion;
					}
				}

				webvellaAdminService.updateEntityView(popupData.view, parentData.entity.name, successCallback, errorCallback);
			}
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
			//Initialize both view and the content region
			parentData.view = fastCopy(response.object);
			for (var i = 0; i < parentData.view.regions.length; i++) {
				if (parentData.view.regions[i].name === "content") {
					parentData.viewContentRegion = parentData.view.regions[i];
				}
			}


		}

		function errorCallback(response) {
			popupData.hasError = true;
			popupData.errorMessage = response.message;

		}
		$log.debug('webvellaAdmin>entities>createSectionModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	//Row Modal
	ManageRowModalController.$inject = ['parentData', 'row', 'section', 'weight', '$uibModalInstance', '$log', 'webvellaAdminService',
				'ngToast'];
	/* @ngInject */
	function ManageRowModalController(parentData, row, section, weight, $uibModalInstance, $log, webvellaAdminService,
				ngToast) {
		$log.debug('webvellaAdmin>entities>createRowModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.section = fastCopy(section);
		popupData.rowOptions = webvellaAdminService.getRowColumnCountVariationsArray();


		popupData.isUpdate = true;
		if (row === null) {
			popupData.isUpdate = false;
			popupData.row = fastCopy(webvellaAdminService.initViewRow(1));
			popupData.row.weight = fastCopy(weight);
			popupData.selectedRowOption = popupData.rowOptions[0];
		}
		else {
			popupData.row = fastCopy(row);
			var selectedColVariationKey = webvellaAdminService.getRowColumnCountVariationKey(row)
			for (var i = 0; i < popupData.rowOptions.length; i++) {
				if (selectedColVariationKey === popupData.rowOptions[i].key) {
					popupData.selectedRowOption = popupData.rowOptions[i];
				}
			}
		}

		popupData.ok = function () {
			//#region << 1. Get the current view and currentContentRegion >>
			popupData.view = fastCopy(parentData.view);
			//Find the content region, which is subject of this screen
			popupData.viewContentRegion = {};
			for (var i = 0; i < popupData.view.regions.length; i++) {
				if (popupData.view.regions[i].name === "content") {
					popupData.viewContentRegion = popupData.view.regions[i];
				}
			}
			//#endregion
			//#region << 2. In the current section and recalculate the rows position in it based on the requested change >>
			if (popupData.isUpdate) {
				//A. Check if the row's column differ from the original number
				var originalRowColumns = 0;
				for (var i = 0; i < parentData.viewContentRegion.sections.length; i++) {
					if (parentData.viewContentRegion.sections[i].name === popupData.section.name) {
						for (var j = 0; j < parentData.viewContentRegion.sections[i].rows.length; j++) {
							if (parseInt(parentData.viewContentRegion.sections[i].rows[j].weight) === parseInt(row.weight)) {
								originalRowColumns = parentData.viewContentRegion.sections[i].rows[j].columns.length;
							}
						}
					}
				}

				//B. If columns differ add to the end or remove from the end
				if (originalRowColumns > popupData.selectedRowOption.columns) {
					//Columns need to be removed
					var columnsToRemove = originalRowColumns - popupData.selectedRowOption.columns;
					popupData.row.columns.splice(columnsToRemove * -1);

				}
				else if (originalRowColumns < popupData.selectedRowOption.columns) {
					//Columns need to be added
					var columnsToAdd = popupData.selectedRowOption.columns - originalRowColumns;

					for (var m = 0; m < columnsToAdd; m++) {
						var column = webvellaAdminService.initViewRowColumn(popupData.selectedRowOption.columns);
						popupData.row.columns.push(column);
					}
				}
				//C. Fix the gridColCount for each column
				var columnsCountArray = webvellaAdminService.convertRowColumnCountVariationKeyToArray(popupData.selectedRowOption.key);
				for (var i = 0; i < popupData.row.columns.length; i++) {
					popupData.row.columns[i].gridColCount = columnsCountArray[i];
				}
				//D. Update
				popupData.section.rows = webvellaAdminService.safeUpdateArrayPlace(popupData.row, popupData.section.rows);
			}
			else {
				popupData.row.columns = webvellaAdminService.initViewRow(popupData.selectedRowOption.key).columns;
				popupData.section.rows = webvellaAdminService.safeAddArrayPlace(popupData.row, popupData.section.rows);
			}
			//#endregion
			//#region << 3. Update the contentRegion & Feed in the updated ContentRegion in the view>>
			for (var i = 0; i < popupData.viewContentRegion.sections.length; i++) {
				if (popupData.viewContentRegion.sections[i].id === popupData.section.id) {
					popupData.viewContentRegion.sections[i] = popupData.section;
				}
			}
			for (var i = 0; i < popupData.view.regions.length; i++) {
				if (popupData.view.regions[i].name === "content") {
					popupData.view.regions[i] = popupData.viewContentRegion;
				}
			}

			//#endregion
			//#region << 4. Call the view update service >>
			webvellaAdminService.updateEntityView(popupData.view, parentData.entity.name, successCallback, errorCallback);
			//#endregion
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + response.message
			});
			$uibModalInstance.close('success');
			//Initialize both view and the content region
			parentData.view = fastCopy(response.object);
			for (var i = 0; i < parentData.view.regions.length; i++) {
				if (parentData.view.regions[i].name === "content") {
					parentData.viewContentRegion = parentData.view.regions[i];
				}
			}
			parentData.regenerateLibrary();

		}

		function errorCallback(response) {
			popupData.hasError = true;
			popupData.errorMessage = response.message;

		}
		$log.debug('webvellaAdmin>entities>createRowModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


	ManageFromRelationModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state', 'eventObj',
			'fieldObj', 'relatedEntityMeta', 'orderChangedOnly'];
	/* @ngInject */
	function ManageFromRelationModalController(parentData, $uibModalInstance, $log, webvellaAdminService, ngToast, $timeout, $state, eventObj,
			fieldObj, relatedEntityMeta, orderChangedOnly) {
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
			if (popupData.entity.recordLists[i].type === "lookup") {
				if (popupData.entity.recordLists[i].default && popupData.lookupDefaultIndex === -1) {
					popupData.lookupDefaultIndex = index;
				}
				popupData.lookupLists.push(popupData.entity.recordLists[i]);
				index++;
			}
		}

		if (popupData.field.fieldLookupList && popupData.field.fieldLookupList !== "") {
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
			if (popupData.entity.recordViews[i].type === "quick_create") {
				if (popupData.entity.recordViews[i].default && popupData.quickCreateDefaultIndex === -1) {
					popupData.quickCreateDefaultIndex = index;
				}
				popupData.quickCreateViews.push(popupData.entity.recordViews[i]);
				index++;
			}
		}
		if (popupData.field.fieldManageView && popupData.field.fieldManageView !== "") {
			//should stick with the selected value
		}
		else if (popupData.lookupDefaultIndex > -1 && popupData.quickCreateViews.length > 0) {
			//no selected so we should preselect the first default;
			popupData.field.fieldManageView = popupData.quickCreateViews[popupData.lookupDefaultIndex].name;
		}
		else if (popupData.quickCreateViews.length > 0) {
			popupData.field.fieldManageView = popupData.quickCreateViews[0].name;
		}
		else if (popupData.field.type === "listFromRelation" || popupData.field.type === "viewFromRelation") {

			//should alert for error if it is list or view
			popupData.error = true;
			popupData.errorMessage = "The target entity '" + popupData.entity.name + "' has no 'quick_create' views. It should have at least one";
		}


		popupData.ok = function () {
			$uibModalInstance.close(popupData.field);
		};

		popupData.cancel = function () {
			if (eventObj !== null && !orderChangedOnly) {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				//we are currently copying so no need to return it back
				//eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.task);
			}
			$uibModalInstance.dismiss('cancel');
		};

		///// Aux
		//function successCallback(response) {
		//	ngToast.create({
		//		className: 'success',
		//		content: '<span class="go-green">Success:</span> ' + response.message
		//	});
		//	$uibModalInstance.close('success');
		//}

		//function errorCallback(response) {
		//	popupData.hasError = true;
		//	popupData.errorMessage = response.message;

		//}
		$log.debug('webvellaAdmin>entities>createRowModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	//#endregion

})();

