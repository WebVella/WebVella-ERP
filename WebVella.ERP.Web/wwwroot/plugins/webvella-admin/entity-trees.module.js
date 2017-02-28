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
					templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
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
				resolvedEntityRecordTrees: resolveEntityRecordTrees,
				resolvedEntityList:resolveEntityList
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

	//#endregion

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$sce', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedRelationsList', 'webvellaCoreService',
					'$uibModal', 'resolvedEntityRecordTrees','$timeout','$translate','resolvedEntityList','$stateParams'];
	
	function controller($scope, $sce, $log, $rootScope, $state, pageTitle, resolvedRelationsList, webvellaCoreService,
					$uibModal, resolvedEntityRecordTrees,$timeout,$translate,resolvedEntityList,$stateParams) {

		
		var ngCtrl = this;

		//#region << Init >>
		ngCtrl.search = {};
		ngCtrl.allRelations = resolvedRelationsList;
		ngCtrl.currentEntityRelation = [];
		ngCtrl.entity =  webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName,resolvedEntityList);
		ngCtrl.trees = resolvedEntityRecordTrees.recordTrees;
		//Update page title
		$translate(['RECORD_TREE_LIST_PAGE_TITLE','ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_TREE_LIST_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
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
				size: "lg",
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
						'resolvedEligibleRelationsList', 'webvellaCoreService','$translate','$scope'];
	
	function createTreeModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl,
						resolvedEligibleRelationsList, webvellaCoreService,$translate,$scope) {
		
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.tree = webvellaCoreService.initTree();
		popupCtrl.entity = fastCopy(ngCtrl.entity);
		popupCtrl.eligibleRelations = fastCopy(resolvedEligibleRelationsList);
		popupCtrl.selectedRelation = {};
		popupCtrl.nodeIdField = {};
		popupCtrl.nodeParentIdField = {};
		popupCtrl.nodeNameField = {};
		popupCtrl.nodeLabelField = {};
		popupCtrl.nodeWeightField = {};
		popupCtrl.nodeNameEligibleFields = [];
		popupCtrl.nodeLabelEligibleFields = [];
		popupCtrl.nodeWeightEligibleFields = [];		
		popupCtrl.validation = webvellaCoreService.initValidationObject();

		if (popupCtrl.eligibleRelations.length > 0) {
			popupCtrl.tree.relationId = popupCtrl.eligibleRelations[0].id;
			popupCtrl.selectedRelation = popupCtrl.eligibleRelations[0];
		}



		popupCtrl.ok = function () {
			popupCtrl.validation = webvellaCoreService.initValidationObject();
			if(popupCtrl.tree.name == ""){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"There are validation errors","name","required field")					
			}

			if(popupCtrl.tree.label == ""){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"There are validation errors","label","required field")					
			}

			if(popupCtrl.tree.nodeNameFieldId == null){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"There are validation errors","nodeNameField","required field")					
			}
			if(popupCtrl.tree.nodeLabelFieldId == null){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"There are validation errors","nodeLabelField","required field")					
			}
			if(popupCtrl.tree.nodeWeightFieldId == null){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"There are validation errors","nodeWeightField","required field")					
			}

			if(!$scope.createFrom.$invalid && !popupCtrl.validation.isInvalid ) {
				webvellaCoreService.createEntityTree(popupCtrl.tree, popupCtrl.entity.name, successCallback, errorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
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
			popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,response.message,null,null);
			for (var i = 0; i < response.errors.length; i++) {
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,response.message,response.errors[i].key,response.errors[i].message);
			}
		}
	
		//#region << Node options >>
		for (var i = 0; i < popupCtrl.entity.fields.length; i++) {
			//Fill dictionaries
			switch (popupCtrl.entity.fields[i].fieldType) {
				case 1: //Auto-increment
					if (popupCtrl.entity.fields[i].required) {
						popupCtrl.nodeLabelEligibleFields.push(popupCtrl.entity.fields[i]);
						popupCtrl.nodeNameEligibleFields.push(popupCtrl.entity.fields[i]);
						popupCtrl.nodeWeightEligibleFields.push(popupCtrl.entity.fields[i]);
					}
					break;
				case 12: //Guid
					if (popupCtrl.entity.fields[i].required) {
						popupCtrl.nodeWeightEligibleFields.push(popupCtrl.entity.fields[i]);
					}
					break;
				case 16: //Guid
					if (popupCtrl.entity.fields[i].required) {
						popupCtrl.nodeLabelEligibleFields.push(popupCtrl.entity.fields[i]);
						popupCtrl.nodeNameEligibleFields.push(popupCtrl.entity.fields[i]);
					}
					break;
				case 18: // Text
					if (popupCtrl.entity.fields[i].required) {
						popupCtrl.nodeLabelEligibleFields.push(popupCtrl.entity.fields[i]);
						popupCtrl.nodeNameEligibleFields.push(popupCtrl.entity.fields[i]);
					}
					break;
			}
		}

		popupCtrl.nodeNameEligibleFields = popupCtrl.nodeNameEligibleFields.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		popupCtrl.nodeLabelEligibleFields = popupCtrl.nodeLabelEligibleFields.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		function getFieldById(fieldId){
			for (var i = 0; i < popupCtrl.entity.fields.length; i++) {
				if(popupCtrl.entity.fields[i].id == fieldId){
					return popupCtrl.entity.fields[i];
				}
			}

			return null;
		}

		$scope.$watch("popupCtrl.selectedRelation",function(newValue,oldValue){
			if(!isEmpty(newValue)){
				popupCtrl.tree.relationId = newValue.id;
				var originField =  getFieldById(newValue.originFieldId);
				var targetField =  getFieldById(newValue.targetFieldId);
				if(originField != null){
					popupCtrl.nodeIdField = originField
					popupCtrl.tree.nodeIdFieldId = originField.id;
				}
				if(targetField != null){
					popupCtrl.nodeParentIdField = targetField;
					popupCtrl.tree.nodeParentIdFieldId = targetField.id;
				}
			}
		});

		$scope.$watch("popupCtrl.nodeNameField",function(newValue,oldValue){
			if(!isEmpty(newValue)){
				popupCtrl.tree.nodeNameFieldId = newValue.id;
			}
		});

		$scope.$watch("popupCtrl.nodeLabelField",function(newValue,oldValue){
			if(!isEmpty(newValue)){
				popupCtrl.tree.nodeLabelFieldId = newValue.id;
			}
		});

		$scope.$watch("popupCtrl.nodeWeightField",function(newValue,oldValue){
			if(!isEmpty(newValue)){
				popupCtrl.tree.nodeWeightFieldId = newValue.id;
			}
		});

		//#endregion	
	



	};

	//#endregion
})();


