/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
		.module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
		.config(config)
		.controller('WebVellaAdminRolesController', controller)
		.controller('ManageRoleModalController', manageRoleController);
   
	///////////////////////////////////////////////////////
	/// Configuration
	///////////////////////////////////////////////////////

	config.$inject = ['$stateProvider'];

	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-roles', {
			parent: 'webvella-admin-base',
			url: '/roles', 
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
					controller: 'WebVellaAdminRolesController',
					templateUrl: '/plugins/webvella-admin/roles.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedRolesList: resolveRolesList
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////

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

		webvellaCoreService.getRecordsWithoutList(null,null,null,"role", successCallback, errorCallback);

		return defer.promise;
	}

	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', 
							'resolvedRolesList', '$uibModal', 'webvellaCoreService','$translate'];
	
	function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedRolesList, $uibModal, webvellaCoreService,$translate) {

		
		var ngCtrl = this;
		ngCtrl.search = {};

		//#region << Update page title & hide the side menu >>
		$translate(['ROLES_PAGE_TITLE', 'ROLES']).then(function (translations) {
			ngCtrl.pageTitle = translations.ROLES_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ROLES;
		});
		//#endregion

		ngCtrl.roles = resolvedRolesList.data;
		ngCtrl.roles = ngCtrl.roles.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});


		//Create new entity modal
		ngCtrl.openManageRoleModal = function (role) {
			if (role != null) {
				ngCtrl.currentRole = role;
			}
			else {
				ngCtrl.currentRole = {};
				ngCtrl.currentRole.id = null;
				ngCtrl.currentRole.name = "";
			}
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageRoleModal.html',
				controller: 'ManageRoleModalController',
				controllerAs: "popupCtrl",
				//size: "lg",
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
	manageRoleController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl','$translate'];
	
	function manageRoleController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaCoreService, ngToast, $timeout, $state, $location, ngCtrl,$translate) {
		
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.role = fastCopy(ngCtrl.currentRole);
		popupCtrl.isUpdate = true;
		if (popupCtrl.role.id == null) {
			popupCtrl.isUpdate = false;
			$translate(['ROLE_CREATE_MODAL_TITLE']).then(function (translations) {
				popupCtrl.modalTitle = translations.ROLE_CREATE_MODAL_TITLE;
			});
		}
		else {
			$translate('ROLE_MANAGE_MODAL_TITLE', { name: popupCtrl.role.name }).then(function (modalTitle) {
				popupCtrl.modalTitle =modalTitle;
			} );
		}

		popupCtrl.deleteRoleModal = function () {
			//Should block the ability to delete admin, guest, regular
			//On delete should update -> areas, entities, entity fields, users and other relevant items which has roles relations.
			alert("Not implemented yet");
		}

		/// EXIT functions
		popupCtrl.ok = function () {
			popupCtrl.validation = {};
			if (!popupCtrl.isUpdate) {
				webvellaCoreService.createRecord("role",popupCtrl.role, successCallback, errorCallback);
			}
			else {
				webvellaCoreService.updateRecord(popupCtrl.role.id, "role", popupCtrl.role, successCallback, errorCallback);
			} 
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL','ROLE_SAVE_SUCCESS_MESSAGE']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.ROLE_SAVE_SUCCESS_MESSAGE
				});
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.user, location);
		}
	};



})();
