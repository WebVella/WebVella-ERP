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
				resolvedRelationsList: resolveRelationsList,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
				resolvedEntityRecordTrees: resolveEntityRecordTrees
			},
			data: {

			}
		});
	};


	//#region << Resolve Function >>/////////////////////////

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

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout','$translate'];
	
	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout,$translate) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert("error in response!")
				});
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
					alert("error in response!")
				});
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
					'$uibModal', 'resolvedEntityRecordTrees','$timeout','$translate'];
	
	function controller($scope, $sce, $log, $rootScope, $state, pageTitle, resolvedRelationsList, resolvedCurrentEntityMeta,
					$uibModal, resolvedEntityRecordTrees,$timeout,$translate) {

		
		var ngCtrl = this;

		//#region << Init >>
		ngCtrl.search = {};
		ngCtrl.allRelations = fastCopy(resolvedRelationsList);
		ngCtrl.currentEntityRelation = [];
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.trees = fastCopy(resolvedEntityRecordTrees.recordTrees);
		//Update page title
		$translate(['RECORD_TREE_LIST_PAGE_TITLE','ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_TREE_LIST_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		ngCtrl.showSidebar = function () {
			//Show Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
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
						'resolvedEligibleRelationsList', 'webvellaCoreService','$translate'];
	
	function createTreeModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl,
						resolvedEligibleRelationsList, webvellaCoreService,$translate) {
		
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
			$translate(['SUCCESS_MESSAGE_LABEL','RECORD_TREE_SAVE_SUCCESS_MESSAGE']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.RECORD_TREE_SAVE_SUCCESS_MESSAGE
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
	};

	//#endregion
})();


