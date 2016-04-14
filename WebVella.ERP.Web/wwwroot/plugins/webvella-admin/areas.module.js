/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plug-in. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminAreasController', controller)
        .controller('ManageAreaModalController', manageAreaController)
        .controller('DeleteAreaModalController', DeleteAreaModalController);

	///////////////////////////////////////////////////////
	/// Configuration
	///////////////////////////////////////////////////////

	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-areas-lists', {
			parent: 'webvella-admin-base',
			url: '/areas',
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
					controller: 'WebVellaAdminAreasController',
					templateUrl: '/plugins/webvella-admin/areas.view.html',
					controllerAs: 'contentData'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedAreaRecordsList: resolveAreaRecordsList,
				resolvedRolesList: resolveRolesList,
				resolvedEntityMetaList: resolveEntityMetaList
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


	resolveAreaRecordsList.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveAreaRecordsList($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>areas-list>resolveAreaRecordsList BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

		webvellaAdminService.getRecordsByEntityName("null", "area", "null", "null", successCallback, errorCallback);


		// Return
		$log.debug('webvellaAdmin>areas-list>resolveAreaRecordsList END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	// Resolve Roles list /////////////////////////
	resolveRolesList.$inject = ['$q', '$log', 'webvellaAdminService'];
	/* @ngInject */
	function resolveRolesList($q, $log, webvellaAdminService) {
		$log.debug('webvellaAdmin>entities> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaAdminService.getRecordsByEntityName("null", "role", "null", "null", successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	// Resolve EntityMetaList /////////////////////////
	resolveEntityMetaList.$inject = ['$q', '$log', 'webvellaAdminService'];

	/* @ngInject */
	function resolveEntityMetaList($q, $log, webvellaAdminService) {
		$log.debug('webvellaAdmin>entities> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaAdminService.getMetaEntityList(successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedAreaRecordsList',
							'resolvedRolesList', 'resolvedEntityMetaList', '$uibModal',
                            'webvellaAdminService', '$timeout'];
	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedAreaRecordsList,
						resolvedRolesList, resolvedEntityMetaList, $uibModal,
                        webvellaAdminService, $timeout) {
		$log.debug('webvellaAdmin>areas-list> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		contentData.search = {};
		//#region << Update page title >>
		contentData.pageTitle = "Areas List | " + pageTitle;
		$timeout(function () {
			$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
		}, 0);
		//#endregion

		contentData.areas = fastCopy(resolvedAreaRecordsList.data);
		contentData.areas = contentData.areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

		contentData.roles = fastCopy(resolvedRolesList.data);
		contentData.roles = contentData.roles.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		contentData.entities = fastCopy(resolvedEntityMetaList.entities);
		contentData.entities = contentData.entities.sort(function (a, b) {
			if (a.label < b.label) return -1;
			if (a.label > b.label) return 1;
			return 0;
		});

		//Create new entity modal
		contentData.openManageAreaModal = function (currentArea) {
			if (currentArea != null) {
				contentData.currentArea = currentArea;
			}
			else {
				contentData.currentArea = webvellaAdminService.initArea();
			}
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageAreaModal.html',
				controller: 'ManageAreaModalController',
				controllerAs: "popupData",
				size: "lg",
				resolve: {
					contentData: function () {
						return contentData;
					}
				}
			});

		}


		$log.debug('webvellaAdmin>areas-list> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}
	//#endregion


	//// Modal Controllers
	manageAreaController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];

	/* @ngInject */
	function manageAreaController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, contentData) {
		$log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.modalInstance = $uibModalInstance;
		popupData.area = fastCopy(contentData.currentArea);
		popupData.areaEntityRelations = fastCopy(contentData.areaEntityRelations);
		popupData.roles = fastCopy(contentData.roles);
		popupData.entities = fastCopy(contentData.entities);
		popupData.attachments = [];
		if (popupData.area.attachments != null && popupData.area.attachments.length > 0) {
			popupData.attachments = angular.fromJson(popupData.area.attachments);
		}

		popupData.cleanEntities = [];

		//Add only entities that have default view and list
		for (var i = 0; i < popupData.entities.length; i++) {
			var hasDefaultView = false;
			var hasDefaultList = false;
			//check if has default view
			for (var v = 0; v < popupData.entities[i].recordViews.length; v++) {
				if (popupData.entities[i].recordViews[v].default && popupData.entities[i].recordViews[v].type === "general") {
					hasDefaultView = true;
				}
			}
			//check if has default list
			for (var l = 0; l < popupData.entities[i].recordLists.length; l++) {
				if (popupData.entities[i].recordLists[l].default && popupData.entities[i].recordLists[l].type === "general") {
					hasDefaultList = true;
				}
			}

			if (hasDefaultView && hasDefaultList) {
				popupData.cleanEntities.push(popupData.entities[i]);
			}
		}
		//Soft alphabetically
		popupData.cleanEntities = popupData.cleanEntities.sort(function (a, b) {
			if (a.label < b.label) return -1;
			if (a.label > b.label) return 1;
			return 0;
		});


		popupData.isUpdate = true;
		if (popupData.area.id == null) {
			popupData.isUpdate = false;
			popupData.modalTitle = $sce.trustAsHtml("Create new area");
			//Select "administrator" and "regular" roles by default
			for (var i = 0; i < popupData.roles.length; i++) {
				switch (popupData.roles[i].name) {
					case "administrator":
						popupData.area.roles.push(popupData.roles[i].id);
						break;
					case "regular":
						popupData.area.roles.push(popupData.roles[i].id);
						break;
					default:
						break;
				}
			}
			popupData.rolesValues = angular.fromJson(popupData.area.roles);
		}
		else {
			//popupData.area.roles = angular.fromJson(popupData.area.roles);
			popupData.rolesValues = angular.fromJson(popupData.area.roles);
			//Remove the already subscribed from the available for attachment list
			popupData.tempEntitiesList = [];
			for (var i = 0; i < popupData.cleanEntities.length; i++) {
				var isSubscribed = false;
				//check if subscribed
				for (var j = 0; j < popupData.attachments.length; j++) {
					if (popupData.cleanEntities[i].name === popupData.attachments[j].name) {
						isSubscribed = true;
					}
				}

				if (!isSubscribed) {
					popupData.tempEntitiesList.push(popupData.cleanEntities[i]);
				}
			}
			//Soft alphabetically
			popupData.cleanEntities = popupData.tempEntitiesList.sort(function (a, b) {
				if (a.name < b.name) return -1;
				if (a.name > b.name) return 1;
				return 0;
			});

			popupData.modalTitle = $sce.trustAsHtml('Edit area <span class="go-green">' + popupData.area.label + '</span>');
		}

		//Awesome font icon names array 
		popupData.icons = getFontAwesomeIconNames();

		//Manage inline edit
		popupData.getViews = function (entityName) {
			var views = [];

			for (var i = 0; i < popupData.entities.length; i++) {
				if (popupData.entities[i].name == entityName) {
					views = popupData.entities[i].recordViews;
					break;
				}
			}
			return views;
		}
		popupData.updateattachmentView = function (attachment) {
			for (var i = 0; i < popupData.entities.length; i++) {
				if (popupData.entities[i].name == attachment.name) {
					for (var j = 0; j < popupData.entities[i].recordViews.length; j++) {
						if (popupData.entities[i].recordViews[j].name == attachment.view.name) {
							attachment.view.label = popupData.entities[i].recordViews[j].label;
							break;
						}
					}

					break;
				}
			}
		}

		popupData.getLists = function (entityName) {
			var lists = [];

			for (var i = 0; i < popupData.entities.length; i++) {
				if (popupData.entities[i].name == entityName) {
					lists = popupData.entities[i].recordLists;
					break;
				}
			}
			return lists;
		}
		popupData.updateattachmentList = function (attachment) {
			for (var i = 0; i < popupData.entities.length; i++) {
				if (popupData.entities[i].name == attachment.name) {
					for (var j = 0; j < popupData.entities[i].recordLists.length; j++) {
						if (popupData.entities[i].recordLists[j].name == attachment.list.name) {
							attachment.list.label = popupData.entities[i].recordLists[j].label;
							break;
						}
					}
					break;
				}
			}
		}


		//Attach entity
		popupData.attachEntity = function (name) {
			//Find the entity
			var selectedEntity = {
				name: null,
				label: null,
				url: null,
				labelPlural: null,
				iconName: null,
				weight: null
			};
			selectedEntity.view = {
				name: null,
				label: null
			};

			selectedEntity.list = {
				name: null,
				label: null
			};

			for (var i = 0; i < popupData.cleanEntities.length; i++) {
				if (popupData.cleanEntities[i].name == name) {
					selectedEntity.name = popupData.cleanEntities[i].name;
					selectedEntity.label = popupData.cleanEntities[i].label;
					selectedEntity.labelPlural = popupData.cleanEntities[i].labelPlural;
					selectedEntity.iconName = popupData.cleanEntities[i].iconName;
					selectedEntity.weight = popupData.cleanEntities[i].weight;
					for (var j = 0; j < popupData.cleanEntities[i].recordViews.length; j++) {
						if (popupData.cleanEntities[i].recordViews[j].default && popupData.cleanEntities[i].recordViews[j].type == "general") {
							selectedEntity.view.name = popupData.cleanEntities[i].recordViews[j].name;
							selectedEntity.view.label = popupData.cleanEntities[i].recordViews[j].label;
						}
					}
					for (var m = 0; m < popupData.cleanEntities[i].recordLists.length; m++) {
						if (popupData.cleanEntities[i].recordLists[m].default && popupData.cleanEntities[i].recordLists[m].type == "general") {
							selectedEntity.list.name = popupData.cleanEntities[i].recordLists[m].name;
							selectedEntity.list.label = popupData.cleanEntities[i].recordLists[m].label;
						}
					}
				}
			}
			//Add to subscribed 
			popupData.attachments.push(selectedEntity);
			popupData.attachments = popupData.attachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			popupData.pendingEntity = null;
			//Remove from cleanEntities
			var attachedItemIndex = -1;
			for (var i = 0; i < popupData.cleanEntities.length; i++) {
				if (popupData.cleanEntities[i].name == selectedEntity.name) {
					attachedItemIndex = i;
					break;
				}
			}
			if (attachedItemIndex != -1) {
				popupData.cleanEntities.splice(attachedItemIndex, 1);
			}
		}

		popupData.attachURL = function () {
			var urlAttachmentObj = {
				name: null,
				label: null,
				url: null,
				labelPlural: null,
				iconName: null,
				weight: null,
				view: null,
				list: null
			};

			urlAttachmentObj.label = popupData.pendingUrlLabel;
			urlAttachmentObj.url = popupData.pendingUrl;
			urlAttachmentObj.iconName = popupData.pendingUrlIconName;
			urlAttachmentObj.weight = popupData.pendingUrlWeight;

			//Add to subscribed 
			popupData.attachments.push(urlAttachmentObj);
			popupData.attachments = popupData.attachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

			popupData.pendingUrlLabel = null;
			popupData.pendingUrl = null;
			popupData.pendingUrlIconName = null;
			popupData.pendingUrlWeight = null;

		}


		//Delete subscribed entity
		popupData.deleteAttachment = function (index) {
			var attachment = popupData.attachments[index];
			//If entity
			if (attachment.url == null) {
				var unsubscribedEntity = {};
				for (var i = 0; i < popupData.entities.length; i++) {
					if (popupData.entities[i].name == attachment.name) {
						unsubscribedEntity = popupData.entities[i];
						break;
					}
				}
				popupData.cleanEntities.push(unsubscribedEntity);
				//Soft alphabetically
				popupData.cleanEntities = popupData.tempEntitiesList.sort(function (a, b) {
					if (a.name < b.name) return -1;
					if (a.name > b.name) return 1;
					return 0;
				});

				//Remove attachment
				var attachmentIndex = -1;
				for (var i = 0; i < popupData.attachments.length; i++) {
					if (popupData.attachments[i].name == attachment.name) {
						attachmentIndex = i;
						break;
					}
				}
				if (attachmentIndex != -1) {
					popupData.attachments.splice(attachmentIndex, 1);
				}
			}
			//if URL
			else {
				popupData.attachments.splice(index, 1);
			}

		}


		/// EXIT functions
		popupData.validation = {};
		popupData.validation.hasError = false;
		popupData.validation.errorMessage = "";

		popupData.ok = function () {
			popupData.validation = {};
			popupData.validation.hasError = false;
			popupData.validation.errorMessage = "";
			if (!popupData.area.name || popupData.area.name == "" || !popupData.area.label || popupData.area.label == "" ||
				!popupData.area.icon_name || popupData.area.icon_name == "" || !popupData.area.color || popupData.area.color == "") {
				popupData.validation.hasError = true;
				popupData.validation.errorMessage = "Required fields are missing data";
			}
			if (!popupData.validation.hasError) {
				if (!popupData.isUpdate) {
					popupData.area.roles = angular.toJson(popupData.rolesValues);
					popupData.area.attachments = angular.toJson(popupData.attachments);
					webvellaAdminService.createRecord("area", popupData.area, successCallback, errorCallback);
				}
				else {
					popupData.area.roles = angular.toJson(popupData.rolesValues);
					popupData.area.attachments = angular.toJson(popupData.attachments);
					webvellaAdminService.updateRecord(popupData.area.id, "area", popupData.area, successCallback, errorCallback);
				}
			}
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The area was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaRootService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaRootService.generateValidationMessages(response, popupData, popupData.entity, location);
		}

		//Delete
		//Delete field
		//Create new field modal
		popupData.deleteAreaModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'deleteAreaModal.html',
				controller: 'DeleteAreaModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					parentPopupData: function () { return popupData; }
				}
			});
		}

		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


	//// Modal Controllers
	DeleteAreaModalController.$inject = ['parentPopupData', '$uibModalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

	/* @ngInject */
	function DeleteAreaModalController(parentPopupData, $uibModalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
		$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.parentData = parentPopupData;

		popupData.ok = function () {

			webvellaAdminService.deleteRecord(popupData.parentData.area.id,"area", successCallback, errorCallback);

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
			popupData.parentData.modalInstance.close('success');
			webvellaRootService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			popupData.hasError = true;
			popupData.errorMessage = response.message;


		}
		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


})();
