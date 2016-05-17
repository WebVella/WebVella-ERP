(function () {
	'use strict';
	angular
        .module('webvellaAreas')
		.controller('changeProjectMilestoneModalController', changeProjectMilestoneModalController)
		.controller('WebVellaTaskProjectMilestoneController', controller);

	controller.$inject = ['$scope', 'webvellaCoreService', '$stateParams', '$timeout','$uibModal','webvellaProjectsService','$q','$translate'];
	function controller($scope, webvellaCoreService, $stateParams, $timeout,$uibModal,webvellaProjectsService,$q,$translate) {
		var taskPMTemplateCtrl = this;

		//#region << Init >>
		var viewData = $scope.ngCtrl.view.data[0]["$view$project_milestone"][0];
		var viewMeta = $scope.ngCtrl.view.meta;

		var entityList = $scope.ngCtrl.entityList;
		var milestoneEntity = getEntityByName("wv_milestone");
		var projectEntity = getEntityByName("wv_project");
		var taskEntity = getEntityByName("wv_task");
		var currentUser =  $scope.ngCtrl.currentUser;
		taskPMTemplateCtrl.recordId = fastCopy($scope.ngCtrl.stateParams.recordId);
		taskPMTemplateCtrl.selectedProjectId = null;
		taskPMTemplateCtrl.selectedProjectName = null;
		taskPMTemplateCtrl.selectedMilestoneId = null;
		taskPMTemplateCtrl.selectedMilestoneName = null;
		if (viewData["$field$project_1_n_task$id"] && viewData["$field$project_1_n_task$id"].length > 0) {
			taskPMTemplateCtrl.selectedProjectId = viewData["$field$project_1_n_task$id"][0];
		}
		if (viewData["$field$project_1_n_task$name"] && viewData["$field$project_1_n_task$name"].length > 0) {
			taskPMTemplateCtrl.selectedProjectName = viewData["$field$project_1_n_task$name"][0];
		}
		if (viewData["$field$milestone_1_n_task$id"] && viewData["$field$milestone_1_n_task$id"].length > 0) {
			taskPMTemplateCtrl.selectedMilestoneId = viewData["$field$milestone_1_n_task$id"][0];
		}
		if (viewData["$field$milestone_1_n_task$name"] && viewData["$field$milestone_1_n_task$name"].length > 0) {
			taskPMTemplateCtrl.selectedMilestoneName = viewData["$field$milestone_1_n_task$name"][0];
		}

		//#endregion

		function currentUserHasUpdatePermission(checkedEntity) {
			var result = false;
			//Check first if the entity allows it
			var userHasUpdateEntityPermission = webvellaCoreService.userHasRecordPermissions(checkedEntity, "canUpdate");
			if (!userHasUpdateEntityPermission) {
				return false;
			}

			var item = null;
			switch (checkedEntity.name) {
				case "wv_milestone":
					item = getFieldFromEntityByName(taskEntity, "milestone_id");
					break;
				case "wv_project":
					item = getFieldFromEntityByName(taskEntity, "project_id");
					break;
			}

			if (!item.enableSecurity) {
				return true;
			}
			for (var i = 0; i < currentUser.roles.length; i++) {
				for (var k = 0; k < item.permissions.canUpdate.length; k++) {
					if (item.permissions.canUpdate[k] == currentUser.roles[i]) {
						result = true;
					}
				}
			}
			return result;

		}

		function getEntityByName(entityName) {
			var entity = null;
			for (var i = 0; i < entityList.length; i++) {
				if (entityList[i].name == entityName) {
					entity = entityList[i];
					break;
				}
			}
			return entity;
		}

		function getFieldFromEntityByName(entity, fieldName) {
			var field = null;
			for (var i = 0; i < entity.fields.length; i++) {
				if (entity.fields[i].name == fieldName) {
					field = entity.fields[i];
					break;
				}
			}
			return field;
		}

		taskPMTemplateCtrl.userCanUpdateMileStone = function () {
			return currentUserHasUpdatePermission(milestoneEntity);
		}

		taskPMTemplateCtrl.userCanUpdateProject = function () {
			return currentUserHasUpdatePermission(projectEntity);
		}

		taskPMTemplateCtrl.updateProjectMilestone = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'changeProjectMilestoneModal.html',
				controller: 'changeProjectMilestoneModalController',
				controllerAs: "popupCtrl",
				//size: "lg",
				resolve: {
					taskPMTemplateCtrl: function () { return taskPMTemplateCtrl; },
					myProjects: function(){
						var defer = $q.defer();
						// Process
						function successCallback(response) {
							if (response.object == null) {
								$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
									alert(translations.ERROR_IN_RESPONSE);
								});
							}
							else {
								defer.resolve(response.object);
							}
						}

						function errorCallback(response) {
							if (response.object == null) {
								$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
									alert(translations.ERROR_IN_RESPONSE);
								});
							}
							else {
								defer.reject(response.message);
							}
						}

						webvellaProjectsService.getMyProjectsList(successCallback, errorCallback);
						return defer.promise;
					},
					myMilestones: function(){
						var defer = $q.defer();
						// Process
						function successCallback(response) {
							if (response.object == null) {
								$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
									alert(translations.ERROR_IN_RESPONSE);
								});
							}
							else {
								defer.resolve(response.object);
							}
						}

						function errorCallback(response) {
							if (response.object == null) {
								$translate(['ERROR_IN_RESPONSE']).then(function (translations) {
									alert(translations.ERROR_IN_RESPONSE);
								});
							}
							else {
								defer.reject(response.message);
							}
						}

						webvellaProjectsService.getMyMilestonesList(successCallback, errorCallback);
						return defer.promise;
					}
				}
			});
		}

	}


	//// Modal Controllers
	changeProjectMilestoneModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'taskPMTemplateCtrl', 
		'webvellaCoreService','$translate','myProjects','myMilestones'];

	function changeProjectMilestoneModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, taskPMTemplateCtrl, 
		webvellaCoreService,$translate,myProjects,myMilestones) {
		
		var popupCtrl = this;
		var parentScope = taskPMTemplateCtrl;
		popupCtrl.project_id = 	parentScope.selectedProjectId;
		popupCtrl.milestone_id = parentScope.selectedMilestoneId;
		popupCtrl.myProjects = 	myProjects;
		popupCtrl.userCanUpdateProject = parentScope.userCanUpdateProject();
		var taskId = parentScope.recordId;
		var allMilestones =	myMilestones;
		
		popupCtrl.myProjectMilestones = [];

		function initMyProjectMilestones(){
			popupCtrl.myProjectMilestones = [];
			var emptyMilestone = {};
			emptyMilestone.id = null;
			emptyMilestone.name = "No milestone";
			popupCtrl.myProjectMilestones.push(emptyMilestone);	
			allMilestones.forEach(function(milestone){
				if(milestone.project_id == popupCtrl.project_id){
					var milestoneObj = {};
					milestoneObj.id = milestone.id;
					milestoneObj.name = milestone.name;
					popupCtrl.myProjectMilestones.push(milestoneObj);
				}
			});
		}


		initMyProjectMilestones();

		popupCtrl.projectChanged = function(){
			popupCtrl.milestone_id = null;
			initMyProjectMilestones();
		}


		popupCtrl.ok = function () {
			//Update the task
			var patchObject = {};
			patchObject.id = taskId;
			patchObject.project_id = popupCtrl.project_id;
			patchObject.milestone_id = popupCtrl.milestone_id;
			webvellaCoreService.patchRecord(taskId, "wv_task",patchObject, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL','RECORD_VIEW_SAVE_SUCCESS_MESSAGE']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.RECORD_VIEW_SAVE_SUCCESS_MESSAGE
				});
			});

			parentScope.selectedProjectId = popupCtrl.project_id;
			for (var i = 0; i < popupCtrl.myProjects.length; i++) {
			   if( popupCtrl.myProjects[i].id == popupCtrl.project_id){
				parentScope.selectedProjectName = popupCtrl.myProjects[i].name;				
				break;
			   }
			}

			parentScope.selectedMilestoneId = popupCtrl.milestone_id;
			for (var i = 0; i < popupCtrl.myProjectMilestones.length; i++) {
			   if( popupCtrl.myProjectMilestones[i].id == popupCtrl.milestone_id){
				parentScope.selectedMilestoneName = popupCtrl.myProjectMilestones[i].name;				
				break;
			   }
			}
	
			$uibModalInstance.close('success');
		}

		function errorCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}

	};

})();