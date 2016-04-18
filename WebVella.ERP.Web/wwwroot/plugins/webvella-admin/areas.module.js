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
					controllerAs: 'ngCtrl'
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

		webvellaAdminService.getRecordsByEntityName("null", "area", "null", successCallback, errorCallback);


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

		webvellaAdminService.getRecordsByEntityName("null", "role", "null", successCallback, errorCallback);

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
		var ngCtrl = this;
		ngCtrl.search = {};
		//#region << Update page title >>
		ngCtrl.pageTitle = "Areas List | " + pageTitle;
		$timeout(function () {
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		}, 0);
		//#endregion

		ngCtrl.areas = fastCopy(resolvedAreaRecordsList.data);
		ngCtrl.areas = ngCtrl.areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

		ngCtrl.roles = fastCopy(resolvedRolesList.data);
		ngCtrl.roles = ngCtrl.roles.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		ngCtrl.entities = fastCopy(resolvedEntityMetaList.entities);
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
				ngCtrl.currentArea = webvellaAdminService.initArea();
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


		$log.debug('webvellaAdmin>areas-list> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}
	//#endregion


	//// Modal Controllers
	manageAreaController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl'];

	/* @ngInject */
	function manageAreaController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, ngCtrl) {
		$log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.area = fastCopy(ngCtrl.currentArea);
		popupCtrl.areaEntityRelations = fastCopy(ngCtrl.areaEntityRelations);
		popupCtrl.roles = fastCopy(ngCtrl.roles);
		popupCtrl.entities = fastCopy(ngCtrl.entities);
		popupCtrl.attachments = [];
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
				if (popupCtrl.entities[i].recordViews[v].default && popupCtrl.entities[i].recordViews[v].type === "general") {
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
			popupCtrl.modalTitle = $sce.trustAsHtml("Create new area");
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

			popupCtrl.modalTitle = $sce.trustAsHtml('Edit area <span class="go-green">' + popupCtrl.area.label + '</span>');
		}

		//Awesome font icon names array 
		popupCtrl.icons = getFontAwesomeIconNames();

		//Manage inline edit
		popupCtrl.getViews = function (entityName) {
			var views = [];

			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == entityName) {
					views = popupCtrl.entities[i].recordViews;
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

		popupCtrl.getLists = function (entityName) {
			var lists = [];

			for (var i = 0; i < popupCtrl.entities.length; i++) {
				if (popupCtrl.entities[i].name == entityName) {
					lists = popupCtrl.entities[i].recordLists;
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
						if (popupCtrl.cleanEntities[i].recordViews[j].default && popupCtrl.cleanEntities[i].recordViews[j].type == "general") {
							selectedEntity.view.name = popupCtrl.cleanEntities[i].recordViews[j].name;
							selectedEntity.view.label = popupCtrl.cleanEntities[i].recordViews[j].label;
						}
					}
					for (var m = 0; m < popupCtrl.cleanEntities[i].recordLists.length; m++) {
						if (popupCtrl.cleanEntities[i].recordLists[m].default && popupCtrl.cleanEntities[i].recordLists[m].type == "general") {
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
				list: null
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
				popupCtrl.validation.errorMessage = "Required fields are missing data";
			}
			if (!popupCtrl.validation.hasError) {
				if (!popupCtrl.isUpdate) {
					popupCtrl.area.roles = angular.toJson(popupCtrl.rolesValues);
					popupCtrl.area.attachments = angular.toJson(popupCtrl.attachments);
					webvellaAdminService.createRecord("area", popupCtrl.area, successCallback, errorCallback);
				}
				else {
					popupCtrl.area.roles = angular.toJson(popupCtrl.rolesValues);
					popupCtrl.area.attachments = angular.toJson(popupCtrl.attachments);
					webvellaAdminService.updateRecord(popupCtrl.area.id, "area", popupCtrl.area, successCallback, errorCallback);
				}
			}
		};

		popupCtrl.cancel = function () {
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
			webvellaRootService.generateValidationMessages(response, popupCtrl, popupCtrl.entity, location);
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

		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


	//// Modal Controllers
	DeleteAreaModalController.$inject = ['parentpopupCtrl', '$uibModalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

	/* @ngInject */
	function DeleteAreaModalController(parentpopupCtrl, $uibModalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
		$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.parentData = parentpopupCtrl;

		popupCtrl.ok = function () {

			webvellaAdminService.deleteRecord(popupCtrl.parentData.area.id,"area", successCallback, errorCallback);

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
			popupCtrl.parentData.modalInstance.close('success');
			webvellaRootService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}
		$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


})();
