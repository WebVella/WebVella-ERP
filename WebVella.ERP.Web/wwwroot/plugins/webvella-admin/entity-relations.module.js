/* entity-relations.module.js */

/**
* @desc this module manages the entity relations screen in the administration
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityRelationsController', controller)
        .controller('ManageRelationModalController', ManageRelationModalController)
        .controller('DeleteRelationModalController', DeleteRelationModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-relations', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/relations', //  /desktop/areas after the parent state is prepended
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
					controller: 'WebVellaAdminEntityRelationsController',
					templateUrl: '/plugins/webvella-admin/entity-relations.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedRelationsList: resolveRelationsList,
				resolvedEntityList:resolveEntityList
			},
			data: {

			}
		});
	};


	// Resolve Function /////////////////////////

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

		// Return
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

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', 'resolvedRelationsList', 
		'webvellaCoreService', 'resolvedEntityList', '$uibModal','$timeout','$translate'];

	
	function controller($scope, $log, $rootScope, $state,$stateParams, pageTitle, resolvedRelationsList, 
		webvellaCoreService, resolvedEntityList, $uibModal,$timeout,$translate) {
		
		var ngCtrl = this;
		ngCtrl.search = {};
		ngCtrl.allRelations = resolvedRelationsList;
		ngCtrl.currentEntityRelations = [];
		ngCtrl.entity = webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName,resolvedEntityList);
		ngCtrl.entityList = resolvedEntityList;

		//Initialize relations in the scope of this entity
		for (var i = 0; i < ngCtrl.allRelations.length; i++) {
			if (ngCtrl.allRelations[i].originEntityId == ngCtrl.entity.id || ngCtrl.allRelations[i].targetEntityId == ngCtrl.entity.id) {
				ngCtrl.currentEntityRelations.push(ngCtrl.allRelations[i]);
			}
		}

		//Sort
		ngCtrl.currentEntityRelations.sort(sort_by("name"));		
		
		//#region << Update page title & hide the side menu >>
		$translate(['ENTITY_RELATIONS','ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.ENTITY_RELATIONS + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		//#endregion

		//Update page title
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
		ngCtrl.manageRelationModal = function (relation) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'manageRelationModal.html',
				controller: 'ManageRelationModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					managedRelation: function () {
						var relationObject = null;
						if (relation != null) {
							for (var i = 0; i < resolvedRelationsList.length; i++) {
								if (relation.id === resolvedRelationsList[i].id) {
									relationObject = resolvedRelationsList[i];
								}
							}
						}
						return relationObject;
					}
				}
			});
		}

	}


	//// Modal Controllers
	ManageRelationModalController.$inject = ['$uibModalInstance', '$uibModal', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', 'managedRelation','$translate'];

	
	function ManageRelationModalController($uibModalInstance, $uibModal, $log, webvellaCoreService, ngToast, $timeout, $state, $location, ngCtrl, managedRelation,$translate) {
		
		var popupCtrl = this;
		popupCtrl.validation = {};
		popupCtrl.modalInstance = $uibModalInstance;
		if (managedRelation === null) {
			popupCtrl.relation = webvellaCoreService.initRelation();
		}
		else {
			popupCtrl.relation = fastCopy(managedRelation);
		}

		popupCtrl.currentEntity = ngCtrl.entity;

		//Relation type array
		popupCtrl.relationTypeDict = [
            {
            	name: "One to One (1:1)",
            	key: 1

            },
            {
            	name: "One to Many (1:N)",
            	key: 2

            },
            {
            	name: "Many to Many (N:N)",
            	key: 3

            }
		];

		popupCtrl.selectedOriginEntity = {};
		popupCtrl.selectedOriginEntity.fields = [{
			id: 1,
			name: "Select Origin Entity first",
			label: "Select Origin Entity first"
		}];
		popupCtrl.selectedOriginFieldEnabled = false;
		popupCtrl.selectedOriginField = popupCtrl.selectedOriginEntity.fields[0];
		popupCtrl.selectedTargetEntity = {};
		popupCtrl.selectedTargetEntity.fields = [{
			id: 1,
			name: "Select Target Entity first",
			label: "Select Target Entity first"
		}];
		popupCtrl.selectedTargetFieldEnabled = false;
		popupCtrl.selectedTargetField = popupCtrl.selectedTargetEntity.fields[0];
		popupCtrl.entities = ngCtrl.entityList;
		//Generate the eligible ORIGIN entities list. No limitation for the eligible field to have only one relation as Target;
		popupCtrl.eligibleOriginEntities = [];
		for (var i = 0; i < popupCtrl.entities.length; i++) {
			var entity = {};
			entity.id = popupCtrl.entities[i].id;
			entity.name = popupCtrl.entities[i].name;
			entity.label = popupCtrl.entities[i].label;
			entity.enabled = true;
			entity.fields = [];
			for (var j = 0; j < popupCtrl.entities[i].fields.length; j++) {
				if (popupCtrl.entities[i].fields[j].fieldType === 16 && popupCtrl.entities[i].fields[j].required && popupCtrl.entities[i].fields[j].unique) {
					var field = {};
					field.id = popupCtrl.entities[i].fields[j].id;
					field.name = popupCtrl.entities[i].fields[j].name;
					field.label = popupCtrl.entities[i].fields[j].label;
					field.required = popupCtrl.entities[i].fields[j].required;
					field.unique = popupCtrl.entities[i].fields[j].unique;
					entity.fields.push(field);
				}
			}
			//Add entity only if it has more than one field 
			if (entity.fields.length > 0) {
				popupCtrl.eligibleOriginEntities.push(entity);
			}
		}
		popupCtrl.eligibleOriginEntities = popupCtrl.eligibleOriginEntities.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});


		//Generate the eligible Target entities list. Enforced limitation for the eligible field to have only one relation as Target;
		//Generate an array of field Ids that are targets in relations
		popupCtrl.targetedFields = [];
		for (var i = 0; i < ngCtrl.allRelations.length; i++) {
			popupCtrl.targetedFields.push(ngCtrl.allRelations[i].targetFieldId);
		}

		popupCtrl.eligibleTargetEntities = [];
		for (var i = 0; i < popupCtrl.entities.length; i++) {
			var entity = {};
			entity.id = popupCtrl.entities[i].id;
			entity.name = popupCtrl.entities[i].name;
			entity.label = popupCtrl.entities[i].label;
			entity.enabled = true;
			entity.fields11 = [];
			entity.fields1n = [];
			entity.fieldsnn = [];
			for (var j = 0; j < popupCtrl.entities[i].fields.length; j++) {
				if (popupCtrl.entities[i].fields[j].fieldType === 16) {
					var field = {};
					field.id = popupCtrl.entities[i].fields[j].id;
					field.name = popupCtrl.entities[i].fields[j].name;
					field.label = popupCtrl.entities[i].fields[j].label;
					field.required = popupCtrl.entities[i].fields[j].required;
					field.unique = popupCtrl.entities[i].fields[j].unique;

					//Eligible for 1:1 relation. Conditions:
					//** - target should not be used by another 
					//** - id field cannot be target
					if (popupCtrl.targetedFields.indexOf(popupCtrl.entities[i].fields[j].id) === -1 &&
						popupCtrl.entities[i].fields[j].name != "id" && popupCtrl.entities[i].fields[j].required) {
						entity.fields11.push(field);
					}

					//Eligible for 1:N relation. Conditions:
					//** - target should not be used by another 
					//** - id field cannot be target
					if (popupCtrl.targetedFields.indexOf(popupCtrl.entities[i].fields[j].id) === -1 &&
						popupCtrl.entities[i].fields[j].name != "id") {
						entity.fields1n.push(field);
					}

					//Eligible for 1:N relation. Conditions:
					//** - target should be unique and required 
					if (field.required && field.unique) {
						entity.fieldsnn.push(field);
					}
				}
			}
			//Add entity only if it has more than one field 
			if (entity.fields11.length > 0 || entity.fields1n.length > 0 || entity.fieldsnn.length > 0) {
				entity.fields11 = entity.fields11.sort(function (a, b) {
					if (a.name < b.name) return -1;
					if (a.name > b.name) return 1;
					return 0;
				});
				entity.fields1n = entity.fields1n.sort(function (a, b) {
					if (a.name < b.name) return -1;
					if (a.name > b.name) return 1;
					return 0;
				});
				entity.fieldsnn = entity.fieldsnn.sort(function (a, b) {
					if (a.name < b.name) return -1;
					if (a.name > b.name) return 1;
					return 0;
				});
				popupCtrl.eligibleTargetEntities.push(entity);
			}
		}

		popupCtrl.eligibleTargetEntities = popupCtrl.eligibleTargetEntities.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});


		/////// Relation type changed
		popupCtrl.relationTypeChanged = function () {
			// Null the entity selections as on the relation type depend what kind of fields can be selected, and this is calculated on entity select change
			popupCtrl.selectedOriginEntity = null;
			popupCtrl.selectedOriginField = null;
			popupCtrl.selectedTargetEntity = null;
			popupCtrl.selectedTargetField = null;
		}

		///////
		popupCtrl.changeOriginEntity = function (newEntityModel) {
			//get only fields that are required
			var requiredFields = [];
			for (var i = 0; i < newEntityModel.fields.length; i++) {
				if (newEntityModel.fields[i].required && newEntityModel.fields[i].unique) {
					requiredFields.push(newEntityModel.fields[i]);
				}
			}
			newEntityModel.fields = requiredFields;
			newEntityModel.fields = newEntityModel.fields.sort(function (a, b) {
				if (a.name < b.name) return -1;
				if (a.name > b.name) return 1;
				return 0;
			});
			popupCtrl.selectedOriginField = newEntityModel.fields[0];
			popupCtrl.selectedOriginFieldEnabled = true;

			if ((popupCtrl.relation.relationType != 3) && popupCtrl.selectedOriginField && popupCtrl.selectedOriginField.id === popupCtrl.selectedTargetField.id) {
				popupCtrl.fieldsDuplicatedError = true;
			} else {
				popupCtrl.fieldsDuplicatedError = false;
			}
		}

		//////
		popupCtrl.changeTargetEntity = function (newEntityModel) {
			popupCtrl.selectedTargetField = null;
			popupCtrl.selectedTargetEntity.fields = [];
			switch (popupCtrl.relation.relationType) {
				case 1: //1:1 relation
					if (newEntityModel.fields11.length > 0) {
						popupCtrl.selectedTargetField = newEntityModel.fields11[0];
						popupCtrl.selectedTargetEntity.fields = newEntityModel.fields11;
					}
					break;
				case 2: //1:N relation
					if (newEntityModel.fields1n.length > 0) {
						popupCtrl.selectedTargetField = newEntityModel.fields1n[0];
						popupCtrl.selectedTargetEntity.fields = newEntityModel.fields1n;
					}
					break;
				case 3: //N:N relation
					if (newEntityModel.fieldsnn.length > 0) {
						popupCtrl.selectedTargetField = newEntityModel.fieldsnn[0];
						popupCtrl.selectedTargetEntity.fields = newEntityModel.fieldsnn;
					}
					break;
			}

			popupCtrl.selectedTargetFieldEnabled = true;
			if (popupCtrl.relation.relationType != 3 && popupCtrl.selectedTargetField && popupCtrl.selectedOriginField.id == popupCtrl.selectedTargetField.id) {
				popupCtrl.fieldsDuplicatedError = true;
			} else {
				popupCtrl.fieldsDuplicatedError = false;
			}
		}

		//Validation for duplicated fields
		popupCtrl.fieldsDuplicatedError = false;

		popupCtrl.changeField = function () {
			if ((popupCtrl.relation.relationType != 3) && popupCtrl.selectedOriginField.id == popupCtrl.selectedTargetField.id) {
				popupCtrl.fieldsDuplicatedError = true;
			} else {
				popupCtrl.fieldsDuplicatedError = false;
			}
		}


		//Initialize the selectedEntity and Fields if in manage mode
		if (popupCtrl.relation.id != null) {
			popupCtrl.selectedTargetEntityLabel = "";
			popupCtrl.selectedOriginEntityLabel = "";
			popupCtrl.selectedTargetFieldLabel = "";
			popupCtrl.selectedOriginFieldLabel = "";
			for (var i = 0; i < popupCtrl.entities.length; i++) {
				//initialize target
				if (popupCtrl.entities[i].id === popupCtrl.relation.targetEntityId) {
					popupCtrl.selectedTargetEntityLabel = popupCtrl.entities[i].name;
					for (var j = 0; j < popupCtrl.entities[i].fields.length; j++) {
						if (popupCtrl.entities[i].fields[j].id === popupCtrl.relation.targetFieldId) {
							popupCtrl.selectedTargetFieldLabel = popupCtrl.entities[i].fields[j].name;
						}
					}
				}
				// initialize origin
				if (popupCtrl.entities[i].id === popupCtrl.relation.originEntityId) {
					popupCtrl.selectedOriginEntityLabel = popupCtrl.entities[i].name;
					for (var j = 0; j < popupCtrl.entities[i].fields.length; j++) {
						if (popupCtrl.entities[i].fields[j].id === popupCtrl.relation.originFieldId) {
							popupCtrl.selectedOriginFieldLabel = popupCtrl.entities[i].fields[j].name;
						}
					}
				}
			}
		}


		//////
		popupCtrl.ok = function () {
			popupCtrl.validation = {};
			if(managedRelation == null){
				if(!popupCtrl.selectedOriginEntity.id || !popupCtrl.selectedOriginField.id || !popupCtrl.selectedTargetEntity.id || !popupCtrl.selectedTargetField.id){
					popupCtrl.validation.hasError = true;			
					popupCtrl.validation.message = "Missing required information";
					return;
				}
			}
			popupCtrl.relation.originEntityId = popupCtrl.selectedOriginEntity.id;
			popupCtrl.relation.originFieldId = popupCtrl.selectedOriginField.id;
			popupCtrl.relation.targetEntityId = popupCtrl.selectedTargetEntity.id;
			popupCtrl.relation.targetFieldId = popupCtrl.selectedTargetField.id;
			if (popupCtrl.relation.id === null) {
				webvellaCoreService.createRelation(popupCtrl.relation, successCallback, errorCallback)
			}
			else {
				var putObject = fastCopy(managedRelation);
				putObject.label = popupCtrl.relation.label;
				putObject.description = popupCtrl.relation.description;
				putObject.system = popupCtrl.relation.system;

				webvellaCoreService.updateRelation(putObject, successCallback, errorCallback)
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.close('dismiss');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL','ENTITY_CREATE_SUCCESS']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.ENTITY_CREATE_SUCCESS
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


		//Delete relation
		popupCtrl.deleteRelationModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'deleteRelationModal.html',
				controller: 'DeleteRelationModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentpopupCtrl: function () { return popupCtrl; }
				}
			});
		}
	};



	//// Modal Controllers
	DeleteRelationModalController.$inject = ['parentpopupCtrl', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state','$translate'];

	
	function DeleteRelationModalController(parentpopupCtrl, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state,$translate) {
		
		var popupCtrl = this;
		popupCtrl.parentData = parentpopupCtrl;

		popupCtrl.ok = function () {
			webvellaCoreService.deleteRelation(popupCtrl.parentData.relation.id, successCallback, errorCallback);
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
			$timeout(function () {
				$state.go("webvella-admin-entity-relations", { name: popupCtrl.parentData.currentEntity.name }, { reload: true });
			}, 0);
		}

		function errorCallback(response) {
			popupCtrl.hasError = true;
			popupCtrl.errorMessage = response.message;


		}
	};




})();


