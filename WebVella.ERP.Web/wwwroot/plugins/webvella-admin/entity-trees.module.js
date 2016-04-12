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
					controllerAs: 'contentData'
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
		var contentData = this;

		//#region << Init >>
		contentData.search = {};
		contentData.allRelations = fastCopy(resolvedRelationsList);
		contentData.currentEntityRelation = [];
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		contentData.trees = fastCopy(resolvedEntityRecordTrees.recordTrees);
		//Update page title
		contentData.pageTitle = "Entity Trees | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);
		contentData.showSidebar = function () {
			//Show Sidemenu
			$timeout(function(){
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
			},0);
		}

		//#endregion

		//#region << Logic >>
		contentData.getRelationHtml = function (tree) {
			var result = "unknown";
			var selectedRelation = {};
			for (var i = 0; i < contentData.allRelations.length; i++) {
				if (contentData.allRelations[i].id == tree.relationId) {
					selectedRelation = contentData.allRelations[i];
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
		contentData.createTreeModal = function () {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createTreeModal.html',
				controller: 'CreateTreeModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					contentData: function () {
						return contentData;
					},
					resolvedEligibleRelationsList: resolveEligibleRelationsList
				}

			});

			function resolveEligibleRelationsList() {
				var eligibleRelations = [];
				for (var i = 0; i < contentData.allRelations.length; i++) {
					if (contentData.allRelations[i].relationType == 2) {
						if (contentData.allRelations[i].originEntityId == contentData.entity.id && contentData.allRelations[i].targetEntityId == contentData.entity.id) {
							eligibleRelations.push(contentData.allRelations[i]);
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
	createTreeModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'contentData',
						'resolvedEligibleRelationsList', 'webvellaAdminService', 'webvellaRootService'];
	/* @ngInject */
	function createTreeModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, contentData,
						resolvedEligibleRelationsList, webvellaAdminService, webvellaRootService) {
		$log.debug('webvellaAdmin>entities>createViewModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.modalInstance = $uibModalInstance;
		popupData.tree = webvellaAdminService.initTree();
		popupData.entity = fastCopy(contentData.entity);
		popupData.eligibleRelations = fastCopy(resolvedEligibleRelationsList);
		if (popupData.eligibleRelations.length > 0) {
			popupData.tree.relationId = popupData.eligibleRelations[0].id;
		}

		popupData.ok = function () {
			webvellaAdminService.createEntityTree(popupData.tree, popupData.entity.name, successCallback, errorCallback);
		};

		popupData.cancel = function () {
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
			webvellaRootService.generateValidationMessages(response, popupData, popupData.entity, location);
		}

		$log.debug('webvellaAdmin>entities>createViewModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};

	//#endregion
})();


