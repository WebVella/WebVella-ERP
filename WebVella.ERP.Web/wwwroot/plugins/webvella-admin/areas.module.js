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
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedAreaRecordsList: resolveAreaRecordsList,
				resolvedRolesList: resolveRolesList,
				resolvedEntityList:resolveEntityList
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////

	resolveAreaRecordsList.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout', '$translate'];

	function resolveAreaRecordsList($q, $log, webvellaCoreService, $stateParams, $state, $timeout, $translate) {
		// Initialize
		var defer = $q.defer();
		// Process
		function successCallback(response) {
			if (response.object == null) {
				alert(errorInResponseMessage)
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				alert(errorInResponseMessage)
			}
			else {
				defer.reject(response.message);
			}
		}

		$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
			var errorInResponseMessage = translations.ERROR_IN_RESPONSE;
			webvellaCoreService.getRecordsWithoutList(null,null, null, "area", successCallback, errorCallback);
		});
		return defer.promise;
	}

	// Resolve Roles list /////////////////////////
	resolveRolesList.$inject = ['$q', '$log', 'webvellaCoreService'];

	function resolveRolesList($q, $log, webvellaCoreService) {

		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaCoreService.getRecordsWithoutList(null,null,null, "role", successCallback, errorCallback);

		return defer.promise;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////

 	resolveEntityList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams'];
	function resolveEntityList($q, $log, webvellaCoreService, $state, $stateParams) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getEntityMetaList(successCallback, errorCallback);
		return defer.promise;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedAreaRecordsList',
							'resolvedRolesList', 'resolvedEntityList', '$uibModal',
							'webvellaCoreService', '$timeout', '$translate'];

	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedAreaRecordsList,
						resolvedRolesList, resolvedEntityList, $uibModal,
						webvellaCoreService, $timeout, $translate) {

		var ngCtrl = this;
		ngCtrl.search = {};
		//#region << Update page title >>
		$translate(['AREA_LIST']).then(function (translations) {
			ngCtrl.pageTitle = translations.AREA_LIST + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		});
		//#endregion

		ngCtrl.areas = resolvedAreaRecordsList.data;
		ngCtrl.areas = ngCtrl.areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

		ngCtrl.roles = resolvedRolesList.data;
		ngCtrl.roles = ngCtrl.roles.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		ngCtrl.entities = resolvedEntityList;
		ngCtrl.entities = ngCtrl.entities.sort(function (a, b) {
			if (a.label < b.label) return -1;
			if (a.label > b.label) return 1;
			return 0;
		});

		//Create new entity modal
		ngCtrl.openManageAreaModal = function (currentArea) {
			if (currentArea != null) {
				ngCtrl.currentArea = currentArea;
			}
			else {
				ngCtrl.currentArea = webvellaCoreService.initArea();
			}
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageAreaModal.html',
				controller: 'ManageAreaModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					}
				}
			});
		}
	}
	//#endregion


	//// Modal Controllers
	manageAreaController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaCoreService', 'ngToast', '$timeout',
									'$state', '$location', 'ngCtrl', '$translate'];


	function manageAreaController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaCoreService, ngToast, $timeout,
									$state, $location, ngCtrl, $translate) {

		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.area = fastCopy(ngCtrl.currentArea);
		popupCtrl.areaEntityRelations = fastCopy(ngCtrl.areaEntityRelations);
		popupCtrl.roles = fastCopy(ngCtrl.roles);
		popupCtrl.entities = fastCopy(ngCtrl.entities);
		popupCtrl.attachments = [];
		popupCtrl.form = {};
		if (popupCtrl.area.attachments != null && popupCtrl.area.attachments.length > 0) {
			popupCtrl.attachments = angular.fromJson(popupCtrl.area.attachments);
		}

		popupCtrl.cleanEntities = [];

		//Add only entities that have default view and list
		for (var i = 0; i < popupCtrl.entities.length; i++) {
			var hasDefaultView = false;
			var hasDefaultList = false;
			//check if has default view
			for (var v = 0; v < popupCtrl.entities[i].recordViews.length; v++) {
				if (popupCtrl.entities[i].recordViews[v].default && (popupCtrl.entities[i].recordViews[v].type === "general" || popupCtrl.entities[i].recordViews[v].type === "hidden" )) {
					hasDefaultView = true;
				}
			}
			//check if has default list
			for (var l = 0; l < popupCtrl.entities[i].recordLists.length; l++) {
				if (popupCtrl.entities[i].recordLists[l].default && popupCtrl.entities[i].recordLists[l].type === "general") {
					hasDefaultList = true;
				}
			}

			if (hasDefaultView && hasDefaultList) {
				popupCtrl.cleanEntities.push(popupCtrl.entities[i]);
			}
		}
		//Soft alphabetically
		popupCtrl.cleanEntities = popupCtrl.cleanEntities.sort(function (a, b) {
			if (a.label < b.label) return -1;
			if (a.label > b.label) return 1;
			return 0;
		});


		popupCtrl.isUpdate = true;
		if (popupCtrl.area.id == null) {
			popupCtrl.isUpdate = false;
			//Select "administrator" and "regular" roles by default
			for (var i = 0; i < popupCtrl.roles.length; i++) {
				switch (popupCtrl.roles[i].name) {
					case "administrator":
						popupCtrl.area.roles.push(popupCtrl.roles[i].id);
						break;
					case "regular":
						popupCtrl.area.roles.push(popupCtrl.roles[i].id);
						break;
					default:
						break;
				}
			}
			popupCtrl.rolesValues = angular.fromJson(popupCtrl.area.roles);
		}
		else {
			//popupCtrl.area.roles = angular.fromJson(popupCtrl.area.roles);
			popupCtrl.rolesValues = angular.fromJson(popupCtrl.area.roles);
			//Remove the already subscribed from the available for attachment list
			popupCtrl.tempEntitiesList = [];
			for (var i = 0; i < popupCtrl.cleanEntities.length; i++) {
				var isSubscribed = false;
				//check if subscribed
				for (var j = 0; j < popupCtrl.attachments.length; j++) {
					if (popupCtrl.cleanEntities[i].name === popupCtrl.attachments[j].name) {
						isSubscribed = true;
					}
				}

				if (!isSubscribed) {
					popupCtrl.tempEntitiesList.push(popupCtrl.cleanEntities[i]);
				}
			}
			//Soft alphabetically
			popupCtrl.cleanEntities = popupCtrl.tempEntitiesList.sort(function (a, b) {
				if (a.name < b.name) return -1;
				if (a.name > b.name) return 1;
				return 0;
			});
		}

		//Awesome font icon names array 
		popupCtrl.icons = getFontAwesomeIconNames();

		//Manage inline edit
		popupCtrl.getViews = function (entityName) {
			var views = [];

			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == entityName) {
					var selectedEntity = popupCtrl.entities[i];
					for (var k = 0; k < selectedEntity.recordViews.length; k++) {
						if (selectedEntity.recordViews[k].type == "general" || selectedEntity.recordViews[k].type == "hidden") {
							views.push(selectedEntity.recordViews[k]);
						}
					}
					break;
				}
			}
			return views;
		}
		popupCtrl.updateattachmentView = function (attachment) {
			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == attachment.name) {
					for (var j = 0; j < popupCtrl.entities[i].recordViews.length; j++) {
						if (popupCtrl.entities[i].recordViews[j].name == attachment.view.name) {
							attachment.view.label = popupCtrl.entities[i].recordViews[j].label;
							break;
						}
					}

					break;
				}
			}
		}

		popupCtrl.getCreates = function (entityName) {
			var creates = [];

			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == entityName) {
					var selectedEntity = popupCtrl.entities[i];
					for (var k = 0; k < selectedEntity.recordViews.length; k++) {
						if (selectedEntity.recordViews[k].type == "create" || selectedEntity.recordViews[k].type == "hidden") {
							creates.push(selectedEntity.recordViews[k]);
						}
					}
					break;
				}
			}
			return creates;
		}
		popupCtrl.updateattachmentCreate = function (attachment) {
			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == attachment.name) {
					for (var j = 0; j < popupCtrl.entities[i].recordViews.length; j++) {
						if (popupCtrl.entities[i].recordViews[j].name == attachment.create.name) {
							attachment.create.label = popupCtrl.entities[i].recordViews[j].label;
							break;
						}
					}

					break;
				}
			}
		}

		popupCtrl.getLists = function (entityName) {
			var lists = [];

			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == entityName) {
					var selectedEntity = popupCtrl.entities[i];
					for (var k = 0; k < selectedEntity.recordLists.length; k++) {
						if (selectedEntity.recordLists[k].type == "general" || selectedEntity.recordLists[k].type == "hidden") {
							lists.push(selectedEntity.recordLists[k]);
						}
					}
					break;
				}
			}
			return lists;
		}
		popupCtrl.updateattachmentList = function (attachment) {
			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == attachment.name) {
					for (var j = 0; j < popupCtrl.entities[i].recordLists.length; j++) {
						if (popupCtrl.entities[i].recordLists[j].name == attachment.list.name) {
							attachment.list.label = popupCtrl.entities[i].recordLists[j].label;
							break;
						}
					}
					break;
				}
			}
		}


		//Attach entity
		popupCtrl.attachEntity = function (name) {
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

			selectedEntity.create = {
				name: null,
				label: null
			};

			selectedEntity.list = {
				name: null,
				label: null
			};

			for (var i = 0; i < popupCtrl.cleanEntities.length; i++) {
				if (popupCtrl.cleanEntities[i].name == name) {
					selectedEntity.name = popupCtrl.cleanEntities[i].name;
					selectedEntity.label = popupCtrl.cleanEntities[i].label;
					selectedEntity.labelPlural = popupCtrl.cleanEntities[i].labelPlural;
					selectedEntity.iconName = popupCtrl.cleanEntities[i].iconName;
					selectedEntity.weight = popupCtrl.cleanEntities[i].weight;
					for (var j = 0; j < popupCtrl.cleanEntities[i].recordViews.length; j++) {
						if (popupCtrl.cleanEntities[i].recordViews[j].default && (popupCtrl.cleanEntities[i].recordViews[j].type == "general" || popupCtrl.cleanEntities[i].recordViews[j].type == "hidden")) {
							selectedEntity.view.name = popupCtrl.cleanEntities[i].recordViews[j].name;
							selectedEntity.view.label = popupCtrl.cleanEntities[i].recordViews[j].label;
						}
					}
					for (var j = 0; j < popupCtrl.cleanEntities[i].recordViews.length; j++) {
						if (popupCtrl.cleanEntities[i].recordViews[j].default && (popupCtrl.cleanEntities[i].recordViews[j].type == "create" || popupCtrl.cleanEntities[i].recordViews[j].type == "hidden")) {
							selectedEntity.create.name = popupCtrl.cleanEntities[i].recordViews[j].name;
							selectedEntity.create.label = popupCtrl.cleanEntities[i].recordViews[j].label;
						}
					}
					for (var m = 0; m < popupCtrl.cleanEntities[i].recordLists.length; m++) {
						if (popupCtrl.cleanEntities[i].recordLists[m].default && (popupCtrl.cleanEntities[i].recordLists[m].type == "general" || popupCtrl.cleanEntities[i].recordLists[m].type == "hidden")) {
							selectedEntity.list.name = popupCtrl.cleanEntities[i].recordLists[m].name;
							selectedEntity.list.label = popupCtrl.cleanEntities[i].recordLists[m].label;
						}
					}
				}
			}
			//Add to subscribed 
			popupCtrl.attachments.push(selectedEntity);
			popupCtrl.attachments = popupCtrl.attachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
			popupCtrl.pendingEntity = null;
			//Remove from cleanEntities
			var attachedItemIndex = -1;
			for (var i = 0; i < popupCtrl.cleanEntities.length; i++) {
				if (popupCtrl.cleanEntities[i].name == selectedEntity.name) {
					attachedItemIndex = i;
					break;
				}
			}
			if (attachedItemIndex != -1) {
				popupCtrl.cleanEntities.splice(attachedItemIndex, 1);
			}
		}

		popupCtrl.attachURL = function () {
			var urlAttachmentObj = {
				name: null,
				label: null,
				url: null,
				labelPlural: null,
				iconName: null,
				weight: null,
				view: null,
				list: null,
				create: null
			};

			urlAttachmentObj.label = popupCtrl.pendingUrlLabel;
			urlAttachmentObj.url = popupCtrl.pendingUrl;
			urlAttachmentObj.iconName = popupCtrl.pendingUrlIconName;
			urlAttachmentObj.weight = popupCtrl.pendingUrlWeight;

			//Add to subscribed 
			popupCtrl.attachments.push(urlAttachmentObj);
			popupCtrl.attachments = popupCtrl.attachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

			popupCtrl.pendingUrlLabel = null;
			popupCtrl.pendingUrl = null;
			popupCtrl.pendingUrlIconName = null;
			popupCtrl.pendingUrlWeight = null;

		}


		//Delete subscribed entity
		popupCtrl.deleteAttachment = function (index) {
			var attachment = popupCtrl.attachments[index];
			//If entity
			if (attachment.url == null) {
				var unsubscribedEntity = {};
				for (var i = 0; i < popupCtrl.entities.length; i++) {
					if (popupCtrl.entities[i].name == attachment.name) {
						unsubscribedEntity = popupCtrl.entities[i];
						break;
					}
				}
				popupCtrl.cleanEntities.push(unsubscribedEntity);
				//Soft alphabetically
				popupCtrl.cleanEntities = popupCtrl.tempEntitiesList.sort(function (a, b) {
					if (a.name < b.name) return -1;
					if (a.name > b.name) return 1;
					return 0;
				});

				//Remove attachment
				var attachmentIndex = -1;
				for (var i = 0; i < popupCtrl.attachments.length; i++) {
					if (popupCtrl.attachments[i].name == attachment.name) {
						attachmentIndex = i;
						break;
					}
				}
				if (attachmentIndex != -1) {
					popupCtrl.attachments.splice(attachmentIndex, 1);
				}
			}
				//if URL
			else {
				popupCtrl.attachments.splice(index, 1);
			}

		}


		/// EXIT functions
		popupCtrl.validation = {};
		popupCtrl.validation.hasError = false;
		popupCtrl.validation.errorMessage = "";

		popupCtrl.ok = function () {
			popupCtrl.validation = {};
			popupCtrl.validation.hasError = false;
			popupCtrl.validation.errorMessage = "";
			if (!popupCtrl.area.name || popupCtrl.area.name == "" || !popupCtrl.area.label || popupCtrl.area.label == "" ||
				!popupCtrl.area.icon_name || popupCtrl.area.icon_name == "" || !popupCtrl.area.color || popupCtrl.area.color == "") {
				popupCtrl.validation.hasError = true;
				$translate(['REQUIRED_FIELDS_MISSING']).then(function (translations) {
					popupCtrl.validation.errorMessage = translations.REQUIRED_FIELDS_MISSING;
				});
			}
			if (!popupCtrl.validation.hasError) {
				if (!popupCtrl.isUpdate) {
					popupCtrl.area.roles = angular.toJson(popupCtrl.rolesValues);
					popupCtrl.area.attachments = angular.toJson(popupCtrl.attachments);
					webvellaCoreService.createRecord("area", popupCtrl.area, successCallback, errorCallback);
				}
				else {
					popupCtrl.area.roles = angular.toJson(popupCtrl.rolesValues);
					popupCtrl.area.attachments = angular.toJson(popupCtrl.attachments);
					webvellaCoreService.updateRecord(popupCtrl.area.id, "area", popupCtrl.area, successCallback, errorCallback);
				}
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL', 'AREA_SAVE_SUCCESS']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.AREA_SAVE_SUCCESS
				});
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.entity, location);
		}

		//Delete
		//Delete field
		//Create new field modal
		popupCtrl.deleteAreaModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'deleteAreaModal.html',
				controller: 'DeleteAreaModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentpopupCtrl: function () { return popupCtrl; }
				}
			});
		}
	};


	//// Modal Controllers
	DeleteAreaModalController.$inject = ['parentpopupCtrl', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state','$translate'];
 	function DeleteAreaModalController(parentpopupCtrl, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $translate) {

		var popupCtrl = this;
		popupCtrl.parentData = parentpopupCtrl;

		popupCtrl.ok = function () {

			webvellaCoreService.deleteRecord(popupCtrl.parentData.area.id, "area", successCallback, errorCallback);

		};

		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
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
			popupCtrl.parentData.modalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}

	};


})();
