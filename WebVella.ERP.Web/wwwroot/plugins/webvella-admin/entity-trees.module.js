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
	
	function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
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

		return defer.promise;
	}

	resolveRelationsList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$timeout'];
	
	function resolveRelationsList($q, $log, webvellaCoreService, $state, $timeout) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaCoreService.getRelationsList(successCallback, errorCallback);

		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {
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

		webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);
		return defer.promise;
	}

	resolveEntityRecordTrees.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$timeout', '$stateParams'];
	
	function resolveEntityRecordTrees($q, $log, webvellaCoreService, $state, $timeout, $stateParams) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			defer.resolve(response.object);
		}

		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaCoreService.getEntityTreesMeta($stateParams.entityName, successCallback, errorCallback);
		return defer.promise;
	}

	//#endregion

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$sce', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedRelationsList', 'resolvedCurrentEntityMeta',
					'$uibModal', 'resolvedEntityRecordTrees','$timeout'];
	
	function controller($scope, $sce, $log, $rootScope, $state, pageTitle, resolvedRelationsList, resolvedCurrentEntityMeta,
					$uibModal, resolvedEntityRecordTrees,$timeout) {

		
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
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		ngCtrl.showSidebar = function () {
			//Show Sidemenu
			$timeout(function(){
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
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
	}

	//#region << Modal Controllers >>
	createTreeModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl',
						'resolvedEligibleRelationsList', 'webvellaCoreService'];
	
	function createTreeModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl,
						resolvedEligibleRelationsList, webvellaCoreService) {
		
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.tree = webvellaCoreService.initTree();
		popupCtrl.entity = fastCopy(ngCtrl.entity);
		popupCtrl.eligibleRelations = fastCopy(resolvedEligibleRelationsList);
		if (popupCtrl.eligibleRelations.length > 0) {
			popupCtrl.tree.relationId = popupCtrl.eligibleRelations[0].id;
		}

		popupCtrl.ok = function () {
			webvellaCoreService.createEntityTree(popupCtrl.tree, popupCtrl.entity.name, successCallback, errorCallback);
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
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.entity, location);
		}
	};

	//#endregion
})();


