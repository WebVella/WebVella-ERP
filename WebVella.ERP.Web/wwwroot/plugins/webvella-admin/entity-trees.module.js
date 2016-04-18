/* entity-relations.module.js */

/**
* @desc this module manages the entity relations screen in the administration
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityTreesController', controller)
		.controller('CreateTreeModalController', createTreeModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-trees', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/trees', //  /desktop/areas after the parent state is prepended
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
					controller: 'WebVellaAdminEntityTreesController',
					templateUrl: '/plugins/webvella-admin/entity-trees.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedRelationsList: resolveRelationsList,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRecordTrees: resolveEntityRecordTrees
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

	resolveEntityRecordTrees.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$timeout', '$stateParams'];
	/* @ngInject */
	function resolveEntityRecordTrees($q, $log, webvellaAdminService, $state, $timeout, $stateParams) {
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

		webvellaAdminService.getEntityTreesMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-relations> END resolveRelationsList state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	//#endregion

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$sce', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedRelationsList', 'resolvedCurrentEntityMeta',
					'$uibModal', 'resolvedEntityRecordTrees','$timeout'];
	/* @ngInject */
	function controller($scope, $sce, $log, $rootScope, $state, pageTitle, resolvedRelationsList, resolvedCurrentEntityMeta,
					$uibModal, resolvedEntityRecordTrees,$timeout) {
		$log.debug('webvellaAdmin>entity-relations> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var ngCtrl = this;

		//#region << Init >>
		ngCtrl.search = {};
		ngCtrl.allRelations = fastCopy(resolvedRelationsList);
		ngCtrl.currentEntityRelation = [];
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.trees = fastCopy(resolvedEntityRecordTrees.recordTrees);
		//Update page title
		ngCtrl.pageTitle = "Entity Trees | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		ngCtrl.showSidebar = function () {
			//Show Sidemenu
			$timeout(function(){
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
			},0);
		}

		//#endregion

		//#region << Logic >>
		ngCtrl.getRelationHtml = function (tree) {
			var result = "unknown";
			var selectedRelation = {};
			for (var i = 0; i < ngCtrl.allRelations.length; i++) {
				if (ngCtrl.allRelations[i].id == tree.relationId) {
					selectedRelation = ngCtrl.allRelations[i];
				}
			}
			if (selectedRelation) {
				if (selectedRelation.relationType == 2) {
					result = $sce.trustAsHtml(selectedRelation.name + ' <span class="badge badge-primary badge-inverse" title="One to Many" style="margin-left:5px;">1 : N</span>');
				}
				else if (selectedRelation.relationType == 3) {
					result = $sce.trustAsHtml(selectedRelation.name + ' <span class="badge badge-primary badge-inverse" title="Many to Many" style="margin-left:5px;">N : N</span>');
				}
			}
			return result;
		}
		//#endregion

		//#region << Modals >>
		ngCtrl.createTreeModal = function () {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createTreeModal.html',
				controller: 'CreateTreeModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					resolvedEligibleRelationsList: resolveEligibleRelationsList
				}

			});

			function resolveEligibleRelationsList() {
				var eligibleRelations = [];
				for (var i = 0; i < ngCtrl.allRelations.length; i++) {
					if (ngCtrl.allRelations[i].relationType == 2) {
						if (ngCtrl.allRelations[i].originEntityId == ngCtrl.entity.id && ngCtrl.allRelations[i].targetEntityId == ngCtrl.entity.id) {
							eligibleRelations.push(ngCtrl.allRelations[i]);
						}
					}
				}
				return eligibleRelations;
			}
		}

		//#endregion

		$log.debug('webvellaAdmin>entity-relations> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}

	//#region << Modal Controllers >>
	createTreeModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl',
						'resolvedEligibleRelationsList', 'webvellaAdminService', 'webvellaRootService'];
	/* @ngInject */
	function createTreeModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl,
						resolvedEligibleRelationsList, webvellaAdminService, webvellaRootService) {
		$log.debug('webvellaAdmin>entities>createViewModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.tree = webvellaAdminService.initTree();
		popupCtrl.entity = fastCopy(ngCtrl.entity);
		popupCtrl.eligibleRelations = fastCopy(resolvedEligibleRelationsList);
		if (popupCtrl.eligibleRelations.length > 0) {
			popupCtrl.tree.relationId = popupCtrl.eligibleRelations[0].id;
		}

		popupCtrl.ok = function () {
			webvellaAdminService.createEntityTree(popupCtrl.tree, popupCtrl.entity.name, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The tree was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaRootService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaRootService.generateValidationMessages(response, popupCtrl, popupCtrl.entity, location);
		}

		$log.debug('webvellaAdmin>entities>createViewModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	//#endregion
})();


