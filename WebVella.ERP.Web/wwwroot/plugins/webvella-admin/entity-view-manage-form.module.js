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
        .controller('ManageRegionModalController', ManageRegionModalController)
		.controller('ManageSectionModalController', ManageSectionModalController)
        .controller('ManageRowModalController', ManageRowModalController)
		.controller('ManageFromRelationModalController', ManageFromRelationModalController);

	//#region << Configuration >> /////////////////////////
	config.$inject = ['$stateProvider'];

	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-view-manage', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/views/:viewName/form/:regionName',
			params: {
				regionName: { value: "header", squash: true }
			},
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
					controller: 'WebVellaAdminEntityViewManageController',
					templateUrl: '/plugins/webvella-admin/entity-view-manage-form.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
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

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', '$translate'];

	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout, $translate) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object === null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
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

	resolveViewLibrary.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', '$translate'];

	function resolveViewLibrary($q, $log, webvellaCoreService, $stateParams, $state, $timeout, $translate) {

		// Initialize
		var defer = $q.defer();
		// Process
		function successCallback(response) {
			if (response.object === null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
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
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityViewLibrary($stateParams.entityName, successCallback, errorCallback);
		return defer.promise;
	}

	resolveEntityRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', '$translate'];

	function resolveEntityRelationsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout, $translate) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object === null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object === null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert(translations.ERROR_IN_RESPONSE);
				});
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getRelationsList(successCallback, errorCallback);
		return defer.promise;
	}

	//#endregion

	//#region << Controller >> ////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$stateParams', '$timeout', 'pageTitle', '$uibModal',
                            'resolvedCurrentEntityMeta', 'webvellaCoreService', 'ngToast', 'resolvedViewLibrary', 'resolvedEntityRelationsList', '$translate'];

	function controller($scope, $log, $rootScope, $state, $stateParams, $timeout, pageTitle, $uibModal,
                        resolvedCurrentEntityMeta, webvellaCoreService, ngToast, resolvedViewLibrary, resolvedEntityRelationsList, $translate) {


		var ngCtrl = this;
		//#region << General init >>
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.stateParams = $stateParams;
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
		ngCtrl.view = {};
		for (var i = 0; i < ngCtrl.entity.recordViews.length; i++) {
			if (ngCtrl.entity.recordViews[i].name === $stateParams.viewName) {
				ngCtrl.view = fastCopy(ngCtrl.entity.recordViews[i]);
			}
		}
		ngCtrl.viewSelectedRegion = {};
		for (var i = 0; i < ngCtrl.view.regions.length; i++) {
			if (ngCtrl.view.regions[i].name === ngCtrl.stateParams.regionName) {
				ngCtrl.viewSelectedRegion = ngCtrl.view.regions[i];
			}
		}
		//#endregion

		//#region << item Library init >>
		var alreadyUsedItemDataNames = [];
		//Get all items from the view and add their dataNames in the already used 
		ngCtrl.generateAlreadyUsed = function () {
			alreadyUsedItemDataNames = [];
			for (var i = 0; i < ngCtrl.viewSelectedRegion.sections.length; i++) {
				for (var j = 0; j < ngCtrl.viewSelectedRegion.sections[i].rows.length; j++) {
					for (var k = 0; k < ngCtrl.viewSelectedRegion.sections[i].rows[j].columns.length; k++) {
						for (var m = 0; m < ngCtrl.viewSelectedRegion.sections[i].rows[j].columns[k].items.length; m++) {
							if (ngCtrl.viewSelectedRegion.sections[i].rows[j].columns[k].items[m].meta) {
								alreadyUsedItemDataNames.push(ngCtrl.viewSelectedRegion.sections[i].rows[j].columns[k].items[m].dataName); //dataName should be used instead meta.id to cover the case with same items from different relations (or no relation and with relation)
							}
						}
					}
				}
			}
		}
		ngCtrl.generateAlreadyUsed();
		ngCtrl.relationsList = fastCopy(resolvedEntityRelationsList);
		ngCtrl.fullLibrary = {};
		ngCtrl.fullLibrary.items = fastCopy(resolvedViewLibrary);
		ngCtrl.library = {};
		ngCtrl.library.relations = [];
		ngCtrl.library.items = [];
		ngCtrl.sortLibrary = function () {
			ngCtrl.library.items.sort(sort_by("type","fieldName"));
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
				//Initially remove all items that are from not activated relation and the relationOptions
				if ((item.meta && alreadyUsedItemDataNames.indexOf(item.dataName) === -1) || !item.meta) {
					switch (item.type) {
						case "field":
							ngCtrl.library.items.push(item);
							break;
						case "view":
							if (item.viewId !== ngCtrl.view.id) {
								ngCtrl.library.items.push(item);
							}
							break;
						case "list":
							ngCtrl.library.items.push(item);
							break;
						case "relationOptions":
							if (generateRelationOptions) {
								item.addedToLibrary = false;
								item.sameOriginTargetEntity = false;
								for (var r = 0; r < ngCtrl.relationsList.length; r++) {
									if (item.relationName === ngCtrl.relationsList[r].name && ngCtrl.relationsList[r].originEntityId === ngCtrl.relationsList[r].targetEntityId) {
										item.sameOriginTargetEntity = true;
									}
								}
								ngCtrl.library.relations.push(item);
							}
							break;
						case "fieldFromRelation":
							if (ngCtrl.checkIfRelationAddedToLibrary(item.relationName)) {
								ngCtrl.library.items.push(item);
							}
							break;
						case "viewFromRelation":
							if (ngCtrl.checkIfRelationAddedToLibrary(item.relationName)) {
								ngCtrl.library.items.push(item);
							}
							break;
						case "listFromRelation":
							if (ngCtrl.checkIfRelationAddedToLibrary(item.relationName)) {
								ngCtrl.library.items.push(item);
							}
							break;
						case "treeFromRelation":
							if (ngCtrl.checkIfRelationAddedToLibrary(item.relationName)) {
								ngCtrl.library.items.push(item);
							}
							break;
					}
				}
			});
			ngCtrl.sortLibrary();
		}

		ngCtrl.generateLibrary(true);

		//Extract the direction change information from the view if present
		for (var k = 0; k < ngCtrl.view.relationOptions.length; k++) {
			for (var m = 0; m < ngCtrl.library.relations.length; m++) {
				if (ngCtrl.view.relationOptions[k].relationName === ngCtrl.library.relations[m].relationName) {
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

		//#region << Region Management >>

		ngCtrl.manageRegion = function (region) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageRegionModal.html',
				controller: 'ManageRegionModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentData: function () { return ngCtrl; },
					region: function () { return region }
				}
			});
		}

		//#endregion

		//#region << Section Management >>

		//Create or Update view section
		ngCtrl.manageSectionModalOpen = function (sectionObj, weight) {
			// ReSharper disable once UnusedLocals
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageSectionModal.html',
				controller: 'ManageSectionModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentData: function () { return ngCtrl; },
					section: function () { return sectionObj },
					weight: function () { return weight }
				}
			});

		}

		//Remove section
		var tempCopyView = {};
		var tempCopyViewRegion = {}
		ngCtrl.removeSection = function (id) {
			var isConfirmed = confirm("Are you sure that you need to remove this section?");
			if (isConfirmed === true) {
				// 1. Copy the view and contentRegion in a temp object
				tempCopyView = fastCopy(ngCtrl.view);
				tempCopyViewRegion = fastCopy(ngCtrl.viewSelectedRegion);
				// 2. Apply the change to the temp object
				tempCopyViewRegion.sections = webvellaCoreService.safeRemoveArrayPlace(tempCopyViewRegion.sections, id);
				// 3. Apply the changes of the temp ContentViewRegion to the temp view object
				for (var i = 0; i < tempCopyView.regions.length; i++) {
					if (tempCopyView.regions[i].name === ngCtrl.stateParams.regionName) {
						tempCopyView.regions[i] = tempCopyViewRegion;
					}
				}
				//Try update with the new view
				webvellaCoreService.updateEntityView(tempCopyView, ngCtrl.entity.name, successSectionRemoveCallback, errorSectionRemoveCallback);

			}
		}

		function successSectionRemoveCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});

			//Initialize both view and the content region with the new value
			ngCtrl.view = tempCopyView;
			ngCtrl.viewSelectedRegion = tempCopyViewRegion;
			ngCtrl.regenerateLibrary();
		}
		function errorSectionRemoveCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}

		//#endregion

		//#region << Row Management >>

		//Create view row
		ngCtrl.manageRowModalOpen = function (rowObj, sectionObj, weight) {
			// ReSharper disable once UnusedLocals
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageRowModal.html',
				controller: 'ManageRowModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentData: function () { return ngCtrl; },
					row: function () { return rowObj },
					section: function () { return sectionObj },
					weight: function () { return weight }
				}
			});

		}

		//Remove row
		ngCtrl.removeRow = function (id, sectionId) {
			var isConfirmed = confirm("Are you sure that you need to remove this row?");
			if (isConfirmed === true) {
				// 1. Copy the view and contentRegion in a temp object
				var tempCopyView = fastCopy(ngCtrl.view);
				var tempCopyViewRegion = fastCopy(ngCtrl.viewSelectedRegion);
				// 2. Apply the change to the temp object
				for (var m = 0; m < tempCopyViewRegion.sections.length; m++) {
					if (tempCopyViewRegion.sections[m].id === sectionId) {
						tempCopyViewRegion.sections[m].rows = webvellaCoreService.safeRemoveArrayPlace(tempCopyViewRegion.sections[m].rows, id);
					}
				}
				// 3. Apply the changes of the temp ContentViewRegion to the temp view object
				for (var i = 0; i < tempCopyView.regions.length; i++) {
					if (tempCopyView.regions[i].name === ngCtrl.stateParams.regionName) {
						tempCopyView.regions[i] = tempCopyViewRegion;
					}
				}
				//Try update with the new view
				webvellaCoreService.updateEntityView(tempCopyView, ngCtrl.entity.name, successRowRemoveCallback, errorRowRemoveCallback);

			}
		}
		function successRowRemoveCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});

			//Initialize both view and the content region with the new value

			ngCtrl.view = response.object;
			for (var i = 0; i < response.object.regions.length; i++) {
				if (response.object.regions[i].name === ngCtrl.stateParams.regionName) {
					ngCtrl.viewSelectedRegion = response.object.regions[i];
				}
			}
			ngCtrl.regenerateLibrary();
		}
		function errorRowRemoveCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}

		//#endregion

		//#region << Manage FromRelationModal >>
		var openFromRelationSettingsModal = function (fieldItem, eventObj, orderChangedOnly) {
			//Init
			var moveFailure = function () {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				ngCtrl.regenerateLibrary();
			};

			var droppedItem = fastCopy(fieldItem);
			var relation = null;
			for (var j = 0; j < ngCtrl.relationsList.length; j++) {
				if (ngCtrl.relationsList[j].id === droppedItem.relationId) {
					relation = ngCtrl.relationsList[j];
				}
			}
			if (relation === null) {
				$translate(['ERROR_MESSAGE_LABEL', 'VALIDATION_RELATION_NOT_FOUND']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + translations.VALIDATION_RELATION_NOT_FOUND,
						timeout: 7000
					});
				});
				moveFailure();
				return;
			}

			//Callbacks
			var moveSuccess = function () {
				// Prevent from dragging back to library use remove link instead
				if (eventObj.dest.sortableScope.element[0].id !== "library") {
					ngCtrl.regenerateLibrary();
				}

			};

			function successCallback(response) {
				if (response.success) {
					$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
						ngToast.create({
							className: 'success',
							content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
						});
					});
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
				$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
						timeout: 7000
					});
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
					for (var k = 0; k < ngCtrl.viewSelectedRegion.sections.length; k++) {
						for (var l = 0; l < ngCtrl.viewSelectedRegion.sections[k].rows.length; l++) {
							for (var m = 0; m < ngCtrl.viewSelectedRegion.sections[k].rows[l].columns.length; m++) {
								for (var n = 0; n < ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items.length; n++) {
									if (fieldObject.type === "fieldFromRelation" && ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].dataName === fieldObject.dataName) {
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldLabel = fieldObject.fieldLabel;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldPlaceholder = fieldObject.fieldPlaceholder;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldHelpText = fieldObject.fieldHelpText;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldRequired = fieldObject.fieldRequired;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldLookupList = fieldObject.fieldLookupList;
									}
									else if (fieldObject.type === "viewFromRelation" && ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].dataName === fieldObject.dataName) {
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldLabel = fieldObject.fieldLabel;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldPlaceholder = fieldObject.fieldPlaceholder;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldHelpText = fieldObject.fieldHelpText;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldRequired = fieldObject.fieldRequired;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldLookupList = fieldObject.fieldLookupList;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldManageView = fieldObject.fieldManageView;
									}
									else if (fieldObject.type === "listFromRelation" && ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].dataName === fieldObject.dataName) {
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldLabel = fieldObject.fieldLabel;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldPlaceholder = fieldObject.fieldPlaceholder;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldHelpText = fieldObject.fieldHelpText;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldRequired = fieldObject.fieldRequired;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldLookupList = fieldObject.fieldLookupList;
										ngCtrl.viewSelectedRegion.sections[k].rows[l].columns[m].items[n].fieldManageView = fieldObject.fieldManageView;
									}
								}
							}
						}
					}

					var tempView = fastCopy(ngCtrl.view);
					for (var i = 0; i < tempView.regions.length; i++) {
						if (tempView.regions[i].name == ngCtrl.stateParams.regionName) {
							tempView.regions[i] = ngCtrl.viewSelectedRegion;
						}
					}
					////2. Call the service
					webvellaCoreService.updateEntityView(tempView, ngCtrl.entity.name, successCallback, errorCallback);
					//return;
				});

			}

			function getRelatedEntityMetaSuccessCallback(response) {
				openModal(response);
			}

			function getRelatedEntityMetaErrorCallback(response) {
				$translate(['ERROR_MESSAGE_LABEL', 'VALIDATION_ENTITY_NOT_FOUND']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + translations.VALIDATION_ENTITY_NOT_FOUND,
						timeout: 7000
					});
				});
				moveFailure();
				return;
			}

			//Get the correct related entityMeta

			if (droppedItem.entityName === ngCtrl.entity.name) {
				//the dropped item has relation to the current entity so no reason to make http request
				var response = {};
				response.success = true;
				response.object = ngCtrl.entity;
				getRelatedEntityMetaSuccessCallback(response);
			}
			else {
				webvellaCoreService.getEntityMeta(droppedItem.entityName, getRelatedEntityMetaSuccessCallback, getRelatedEntityMetaErrorCallback);
			}
		};
		//#endregion

		//#region << Drag & Drop Management >>

		function executeDragViewChange(eventObj, orderChangedOnly) {

			var moveSuccess = function () {
				// Prevent from dragging back to library use remove link instead
				if (eventObj.dest.sortableScope.element[0].id !== "library") {
					ngCtrl.regenerateLibrary();
				}
			};
			var moveFailure = function () {
				eventObj.dest.sortableScope.removeItem(eventObj.dest.index);
				eventObj.source.itemScope.sortableScope.insertItem(eventObj.source.index, eventObj.source.itemScope.item);
				ngCtrl.regenerateLibrary();
			};

			function successCallback(response) {
				if (response.success) {
					$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
						ngToast.create({
							className: 'success',
							content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
						});
					});
					//ngCtrl.library.items = fastCopy(ngCtrl.originalLibrary);
					for (var i = 0; i < response.object.regions.length; i++) {
						if (response.object.regions[i].name === ngCtrl.stateParams.regionName) {
							ngCtrl.viewSelectedRegion = response.object.regions[i];
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
				$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
						timeout: 7000
					});
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
				for (var i = 0; i < ngCtrl.view.regions.length; i++) {
					if (ngCtrl.view.regions[i].name === ngCtrl.stateParams.regionName) {
						ngCtrl.view.regions[i] = fastCopy(ngCtrl.viewSelectedRegion);
					}
				}
				webvellaCoreService.updateEntityView(ngCtrl.view, ngCtrl.entity.name, successCallback, errorCallback);

			}
		}

		ngCtrl.dragControlListeners = {
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


		ngCtrl.libraryDragControlListeners = {
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

		ngCtrl.dragItemRemove = function (column, itemDataName) {
			ngCtrl.itemScheduledForRemoval = null;
			var index = -1;
			for (var i = 0; i < column.items.length; i++) {
				if (column.items[i].dataName === itemDataName) {
					ngCtrl.itemScheduledForRemoval = column.items[i];
					index = i;
				}
			}
			function successCallback(response) {
				$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
					ngToast.create({
						className: 'success',
						content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
					});
				});
				ngCtrl.regenerateLibrary();
			}

			function errorCallback(response) {
				$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
						timeout: 7000
					});
				});
				$state.reload();
			}

			column.items.splice(index, 1);
			for (var i = 0; i < ngCtrl.view.regions.length; i++) {
				if (ngCtrl.view.regions[i].name === ngCtrl.stateParams.regionName) {
					ngCtrl.view.regions[i] = fastCopy(ngCtrl.viewSelectedRegion);
				}
			}
			webvellaCoreService.updateEntityView(ngCtrl.view, ngCtrl.entity.name, successCallback, errorCallback);
		}

		//#endregion

		//#region << Relations >>

		ngCtrl.changeRelationDirection = function (relation) {
			if (relation.direction === "origin-target") {
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
				$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
					ngToast.create({
						className: 'success',
						content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
					});
				});
			}

			function errorCallback(response) {
				$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
					ngToast.create({
						className: 'error',
						content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
						timeout: 7000
					});
				});
				//Undo change
				for (var j = 0; j < ngCtrl.library.relations.length; j++) {
					if (ngCtrl.library.relations[j].relationName === relation.relationName) {
						if (ngCtrl.library.relations[j].direction === "origin-target") {
							ngCtrl.library.relations[j].direction = "target-origin";
						}
						else {
							ngCtrl.library.relations[j].direction = "origin-target";
						}
					}
				}
			}

			webvellaCoreService.updateEntityView(ngCtrl.view, ngCtrl.entity.name, successCallback, errorCallback);
		}

		ngCtrl.toggleRelationToLibrary = function (relation) {
			if (!relation.addedToLibrary) {
				ngCtrl.fullLibrary.items.forEach(function (item) {
					if (item.relationName && item.relationName === relation.relationName) {
						if (item.meta && alreadyUsedItemDataNames.indexOf(item.dataName) === -1) {
							switch (item.type) {
								case "fieldFromRelation":
									ngCtrl.library.items.push(item);
									break;
								case "viewFromRelation":
									if (item.viewId !== ngCtrl.view.id) {
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
					else if (item.relationName !== relation.relationName) {
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
				if (ngCtrl.relationsList[i].id === relationId) {
					return ngCtrl.relationsList[i].relationType;
				}
			}
			return 0;
		}

		ngCtrl.manageFieldFromRelation = function (item) {
			openFromRelationSettingsModal(item, null, false);
		}

		//#endregion
	}
	//#endregion

	//#region << Modal Controllers >> /////////////////////

	//Region modal

	ManageRegionModalController.$inject = ['parentData', 'region', '$uibModalInstance', '$log', 'webvellaCoreService',
				'ngToast', '$translate'];

	function ManageRegionModalController(parentData, region, $uibModalInstance, $log, webvellaCoreService,
				ngToast, $translate) {

		var popupCtrl = this;

		popupCtrl.isUpdate = true;
		if (region === null) {
			popupCtrl.isUpdate = false;
			popupCtrl.region = webvellaCoreService.initViewRegion();
		}
		else {
			popupCtrl.region = fastCopy(region);
		}

		popupCtrl.ok = function () {
			popupCtrl.view = fastCopy(parentData.view);
			if (popupCtrl.isUpdate) {
				for (var i = 0; i < popupCtrl.view.regions.length; i++) {
					if (popupCtrl.view.regions[i].name === popupCtrl.region.name) {
						popupCtrl.view.regions[i].label = popupCtrl.region.label;
						popupCtrl.view.regions[i].render = popupCtrl.region.render;
						popupCtrl.view.regions[i].cssClass = popupCtrl.region.cssClass;
						popupCtrl.view.regions[i].weight = popupCtrl.region.weight;
					}
				}
			}
			else {
				popupCtrl.view.regions.push(popupCtrl.region);
			}
			popupCtrl.view.regions.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
			webvellaCoreService.updateEntityView(popupCtrl.view, parentData.entity.name, successCallback, errorCallback);
		};

		popupCtrl.delete = function () {
			popupCtrl.view = fastCopy(parentData.view);
			if (popupCtrl.isUpdate && popupCtrl.region.name != "header") {
				var deletedRegionIndex = -1;
				for (var i = 0; i < popupCtrl.view.regions.length; i++) {
					if (popupCtrl.view.regions[i].name === popupCtrl.region.name) {
						deletedRegionIndex = i;
					}
				}
				popupCtrl.view.regions.splice(deletedRegionIndex, 1);
				popupCtrl.view.regions.sort(sort_by({ name: 'weight', primer: parseInt, reverse: false }));
				webvellaCoreService.updateEntityView(popupCtrl.view, parentData.entity.name, successCallback, errorCallback);
			}
			else {
				$uibModalInstance.dismiss('cancel');
			}
		}

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			//check if current region exists
			var regionExists = false;
			for (var i = 0; i < response.object.regions.length; i++) {
				if (response.object.regions[i].name === parentData.stateParams.regionName) {
					regionExists = true;
					break;
				}
			}

			//if the current regionName is not existing any more (deleted) redirect after the close
			if (regionExists) {
				parentData.view = response.object;
				$uibModalInstance.close('success');
			}
			else {
				webvellaCoreService.GoToState('webvella-admin-entity-view-manage',{entityName:parentData.stateParams.entityName,viewName: parentData.stateParams.viewName, regionName:"default"});
				$uibModalInstance.dismiss('cancel');
			}
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;

		}
	};

	//Section Modal
	ManageSectionModalController.$inject = ['parentData', 'section', 'weight', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$scope', '$translate'];

	function ManageSectionModalController(parentData, section, weight, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $scope, $translate) {



		//#region << Initialize >>
		var popupCtrl = this;
		popupCtrl.section = null;
		popupCtrl.isUpdate = true;
		popupCtrl.isValid = true;
		if (section === null) {
			popupCtrl.isUpdate = false;
			popupCtrl.section = fastCopy(webvellaCoreService.initViewSection());
			popupCtrl.section.weight = weight;
		}
		else {
			popupCtrl.section = fastCopy(section);
		}
		//#endregion

		popupCtrl.ok = function () {
			popupCtrl.view = fastCopy(parentData.view);
			//Find the content region, which is subject of this screen
			popupCtrl.viewSelectedRegion = {};
			for (var i = 0; i < popupCtrl.view.regions.length; i++) {
				if (popupCtrl.view.regions[i].name === parentData.stateParams.regionName) {
					popupCtrl.viewSelectedRegion = popupCtrl.view.regions[i];
				}
			}
			// Validate unique username on add. It cannot be managed on update
			if (!popupCtrl.isUpdate) {
				popupCtrl.isValid = true;
				if (!popupCtrl.viewSelectedRegion.sections) {
					popupCtrl.viewSelectedRegion.sections = []; //If the view was newly created the viewSelectedRegion will be an empty object
				}
				for (var i = 0; i < popupCtrl.viewSelectedRegion.sections.length; i++) {
					if (popupCtrl.viewSelectedRegion.sections[i].name === popupCtrl.section.name) {
						popupCtrl.isValid = false;
					}
				}
				if (!popupCtrl.isValid) {
					$scope.manageSection.name.$dirty = true;
					$scope.manageSection.name.$invalid = true;
					$scope.manageSection.name.$pristine = false;
					$scope.manageSection.name.$setValidity("unique", false);
				}
			}
			//#region << Update the temporary view object for submission >>
			if (popupCtrl.isUpdate && popupCtrl.isValid) {
				popupCtrl.viewSelectedRegion.sections = webvellaCoreService.safeUpdateArrayPlace(popupCtrl.section, popupCtrl.viewSelectedRegion.sections);
			}
			else if (popupCtrl.isValid) {
				popupCtrl.viewSelectedRegion.sections = webvellaCoreService.safeAddArrayPlace(popupCtrl.section, popupCtrl.viewSelectedRegion.sections);
			}
			//#endregion

			if (popupCtrl.isValid) {
				//Update the view with the correct values for the content region
				for (var i = 0; i < popupCtrl.view.regions.length; i++) {
					if (popupCtrl.view.regions[i].name === parentData.stateParams.regionName) {
						popupCtrl.view.regions[i] = popupCtrl.viewSelectedRegion;
					}
				}

				webvellaCoreService.updateEntityView(popupCtrl.view, parentData.entity.name, successCallback, errorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			$uibModalInstance.close('success');
			//Initialize both view and the content region
			parentData.view = fastCopy(response.object);
			for (var i = 0; i < parentData.view.regions.length; i++) {
				if (parentData.view.regions[i].name === parentData.stateParams.regionName) {
					parentData.viewSelectedRegion = parentData.view.regions[i];
				}
			}


		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;

		}
	};

	//Row Modal
	ManageRowModalController.$inject = ['parentData', 'row', 'section', 'weight', '$uibModalInstance', '$log', 'webvellaCoreService',
				'ngToast', '$translate'];

	function ManageRowModalController(parentData, row, section, weight, $uibModalInstance, $log, webvellaCoreService,
				ngToast, $translate) {

		var popupCtrl = this;
		popupCtrl.section = fastCopy(section);
		popupCtrl.rowOptions = webvellaCoreService.getRowColumnCountVariationsArray();


		popupCtrl.isUpdate = true;
		if (row === null) {
			popupCtrl.isUpdate = false;
			popupCtrl.row = fastCopy(webvellaCoreService.initViewRow(1));
			popupCtrl.row.weight = fastCopy(weight);
			popupCtrl.selectedRowOption = popupCtrl.rowOptions[0];
		}
		else {
			popupCtrl.row = fastCopy(row);
			var selectedColVariationKey = webvellaCoreService.getRowColumnCountVariationKey(row)
			for (var i = 0; i < popupCtrl.rowOptions.length; i++) {
				if (selectedColVariationKey === popupCtrl.rowOptions[i].key) {
					popupCtrl.selectedRowOption = popupCtrl.rowOptions[i];
				}
			}
		}

		popupCtrl.ok = function () {
			//#region << 1. Get the current view and currentContentRegion >>
			popupCtrl.view = fastCopy(parentData.view);
			//Find the content region, which is subject of this screen
			popupCtrl.viewSelectedRegion = {};
			for (var i = 0; i < popupCtrl.view.regions.length; i++) {
				if (popupCtrl.view.regions[i].name === parentData.stateParams.regionName) {
					popupCtrl.viewSelectedRegion = popupCtrl.view.regions[i];
				}
			}
			//#endregion
			//#region << 2. In the current section and recalculate the rows position in it based on the requested change >>
			if (popupCtrl.isUpdate) {
				//A. Check if the row's column differ from the original number
				var originalRowColumns = 0;
				for (var i = 0; i < parentData.viewSelectedRegion.sections.length; i++) {
					if (parentData.viewSelectedRegion.sections[i].name === popupCtrl.section.name) {
						for (var j = 0; j < parentData.viewSelectedRegion.sections[i].rows.length; j++) {
							if (parseInt(parentData.viewSelectedRegion.sections[i].rows[j].weight) === parseInt(row.weight)) {
								originalRowColumns = parentData.viewSelectedRegion.sections[i].rows[j].columns.length;
							}
						}
					}
				}

				//B. If columns differ add to the end or remove from the end
				if (originalRowColumns > popupCtrl.selectedRowOption.columns) {
					//Columns need to be removed
					var columnsToRemove = originalRowColumns - popupCtrl.selectedRowOption.columns;
					popupCtrl.row.columns.splice(columnsToRemove * -1);

				}
				else if (originalRowColumns < popupCtrl.selectedRowOption.columns) {
					//Columns need to be added
					var columnsToAdd = popupCtrl.selectedRowOption.columns - originalRowColumns;

					for (var m = 0; m < columnsToAdd; m++) {
						var column = webvellaCoreService.initViewRowColumn(popupCtrl.selectedRowOption.columns);
						popupCtrl.row.columns.push(column);
					}
				}
				//C. Fix the gridColCount for each column
				var columnsCountArray = webvellaCoreService.convertRowColumnCountVariationKeyToArray(popupCtrl.selectedRowOption.key);
				for (var i = 0; i < popupCtrl.row.columns.length; i++) {
					popupCtrl.row.columns[i].gridColCount = columnsCountArray[i];
				}
				//D. Update
				popupCtrl.section.rows = webvellaCoreService.safeUpdateArrayPlace(popupCtrl.row, popupCtrl.section.rows);
			}
			else {
				popupCtrl.row.columns = webvellaCoreService.initViewRow(popupCtrl.selectedRowOption.key).columns;
				popupCtrl.section.rows = webvellaCoreService.safeAddArrayPlace(popupCtrl.row, popupCtrl.section.rows);
			}
			//#endregion
			//#region << 3. Update the contentRegion & Feed in the updated ContentRegion in the view>>
			for (var i = 0; i < popupCtrl.viewSelectedRegion.sections.length; i++) {
				if (popupCtrl.viewSelectedRegion.sections[i].id === popupCtrl.section.id) {
					popupCtrl.viewSelectedRegion.sections[i] = popupCtrl.section;
				}
			}
			for (var i = 0; i < popupCtrl.view.regions.length; i++) {
				if (popupCtrl.view.regions[i].name === parentData.stateParams.regionName) {
					popupCtrl.view.regions[i] = popupCtrl.viewSelectedRegion;
				}
			}

			//#endregion
			//#region << 4. Call the view update service >>
			webvellaCoreService.updateEntityView(popupCtrl.view, parentData.entity.name, successCallback, errorCallback);
			//#endregion
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			$uibModalInstance.close('success');
			//Initialize both view and the content region
			parentData.view = fastCopy(response.object);
			for (var i = 0; i < parentData.view.regions.length; i++) {
				if (parentData.view.regions[i].name === parentData.stateParams.regionName) {
					parentData.viewSelectedRegion = parentData.view.regions[i];
				}
			}
			parentData.regenerateLibrary();

		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;

		}
	};


	ManageFromRelationModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', 'eventObj',
			'fieldObj', 'relatedEntityMeta', 'orderChangedOnly'];

	function ManageFromRelationModalController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, eventObj,
			fieldObj, relatedEntityMeta, orderChangedOnly) {

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
			if (popupCtrl.entity.recordLists[i].type === "lookup") {
				if (popupCtrl.entity.recordLists[i].default && popupCtrl.lookupDefaultIndex === -1) {
					popupCtrl.lookupDefaultIndex = index;
				}
				popupCtrl.lookupLists.push(popupCtrl.entity.recordLists[i]);
				index++;
			}
		}

		if (popupCtrl.field.fieldLookupList && popupCtrl.field.fieldLookupList !== "") {
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
			if (popupCtrl.entity.recordViews[i].type === "quick_create") {
				if (popupCtrl.entity.recordViews[i].default && popupCtrl.quickCreateDefaultIndex === -1) {
					popupCtrl.quickCreateDefaultIndex = index;
				}
				popupCtrl.quickCreateViews.push(popupCtrl.entity.recordViews[i]);
				index++;
			}
		}
		if (popupCtrl.field.fieldManageView && popupCtrl.field.fieldManageView !== "") {
			//should stick with the selected value
		}
		else if (popupCtrl.lookupDefaultIndex > -1 && popupCtrl.quickCreateViews.length > 0) {
			//no selected so we should preselect the first default;
			popupCtrl.field.fieldManageView = popupCtrl.quickCreateViews[popupCtrl.lookupDefaultIndex].name;
		}
		else if (popupCtrl.quickCreateViews.length > 0) {
			popupCtrl.field.fieldManageView = popupCtrl.quickCreateViews[0].name;
		}
		else if (popupCtrl.field.type === "listFromRelation" || popupCtrl.field.type === "viewFromRelation") {

			//should alert for error if it is list or view
			popupCtrl.error = true;
			popupCtrl.errorMessage = "The target entity '" + popupCtrl.entity.name + "' has no 'quick_create' views. It should have at least one";
		}


		popupCtrl.ok = function () {
			$uibModalInstance.close(popupCtrl.field);
		};

		popupCtrl.cancel = function () {
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
		//	popupCtrl.hasError = true;
		//	popupCtrl.errorMessage = response.message;

		//}
	};

	//#endregion

})();

