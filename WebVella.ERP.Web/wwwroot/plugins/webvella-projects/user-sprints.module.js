(function () {
	'use strict';

	angular
        .module('webvellaAreas')
        .config(config)
        .controller('WebVellaProjectsUserSprintsController', controller)
		.controller('ShowSprintsListModalController', ShowSprintsListModalController)
		.controller('NewCommentModalController', NewCommentModalController)
		.controller('LogTimeModalController', LogTimeModalController)
		.controller('AttachTaskModalController', AttachTaskModalController);

	//#region << Configuration /////////////////////////////////// >>
	config.$inject = ['$stateProvider'];
	function config($stateProvider) {
		$stateProvider
		//general list in an area
		.state('webvella-projects-sprints', {
			parent: 'webvella-area-base',
			url: '/sprints/{sprintId}?scope',
			reloadOnSearch: false,
			params: {
				sprintId: { value: "current", squash: true },
				scope: { value: "user", squash: true }
			},
			views: {
				"topnavView": {
					controller: 'WebVellaAreasTopnavController',
					templateUrl: '/plugins/webvella-areas/topnav.view.html',
					controllerAs: 'topnavData'
				},
				"sidebarView": {
					controller: 'WebVellaAreasSidebarController',
					templateUrl: '/plugins/webvella-areas/sidebar.view.html',
					controllerAs: 'sidebarData'
				},
				"contentView": {
					controller: 'WebVellaProjectsUserSprintsController',
					templateUrl: '/plugins/webvella-projects/user-sprints.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedCurrentViewData: function () { return null; },
				resolvedParentViewData: function () { return null; },
				resolvedSprintData: resolveSprintData
			},
			data: {

			}
		});
	};

	//#endregion

	//#region << Resolve Function >>

	////////////////////////
	resolveSprintData.$inject = ['$q', '$log', '$state', '$stateParams', '$timeout', 'ngToast', '$location', 'webvellaProjectsService'];
	function resolveSprintData($q, $log, $state, $stateParams, $timeout, ngToast, $location, webvellaProjectsService) {
		var defer = $q.defer();

		function successCallback(response) {
			defer.resolve(response);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}

		webvellaProjectsService.getProjectSprintDetails($stateParams.sprintId, $stateParams.scope, successCallback, errorCallback);
		return defer.promise;
	}

	//#endregion


	//#region << Controller /////////////////////////////// >>
	controller.$inject = ['$log', '$uibModal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaCoreService', 'webvellaProjectsService',
        '$timeout', 'resolvedCurrentUser', '$sessionStorage', '$location', '$window', '$sce', 'resolvedAreas', 'ngToast', 'resolvedSprintData', '$q', '$translate'];

	function controller($log, $uibModal, $rootScope, $state, $stateParams, pageTitle, webvellaCoreService, webvellaProjectsService,
        $timeout, resolvedCurrentUser, $sessionStorage, $location, $window, $sce, resolvedAreas, ngToast, resolvedSprintData, $q, $translate) {

		//#region << ngCtrl initialization >>
		var ngCtrl = this;
		ngCtrl.loading = false;
		ngCtrl.sprint = resolvedSprintData.object;
		ngCtrl.scope = $stateParams.scope;
		ngCtrl.noSprints = false;
		ngCtrl.noAccess = false;
		if(ngCtrl.sprint == null){
			if(resolvedSprintData.message == "no access"){
				ngCtrl.noAccess = true;
			}
			else {
				ngCtrl.noSprints = true;
			}
		}

		//#endregion

		//#region << Set Page meta >>
		ngCtrl.pageTitle = "My Sprints | " + pageTitle;
		webvellaCoreService.setPageTitle(ngCtrl.pageTitle);
		ngCtrl.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		webvellaCoreService.setBodyColorClass(ngCtrl.currentArea.color);
		//#endregion

		//#region << Change scope >>

		ngCtrl.changeScope = function () {
			var oldScope = fastCopy(ngCtrl.scope);
			var newScope = "everyone";
			ngCtrl.loading = true;
			if (oldScope == "user") {
				newScope = "everyone";
			}
			else {
				newScope = "user";
			}

			function successCallback(response) {
				$location.search({ scope: newScope })
				ngCtrl.scope = newScope;
				ngCtrl.sprint = response.object;
				ngCtrl.loading = false;
			}
			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				ngCtrl.loading = false;
			}

			webvellaProjectsService.getProjectSprintDetails($stateParams.sprintId, newScope, successCallback, errorCallback);

		}

		//#endregion

		//#region << Render >>

		ngCtrl.getStatusString = function () {
			var status = "not-started";
			if (moment().isAfter(ngCtrl.sprint.start_date) && moment().subtract(1, 'days').isBefore(ngCtrl.sprint.end_date)) {
				return "<span class='go-green'>in progress</span>";
			}
			else if (moment().isAfter(ngCtrl.sprint.start_date) && moment().subtract(1, 'days').isAfter(ngCtrl.sprint.end_date)) {
				return "<span class='go-grey'>completed</span>";
			}
			else {
				return "<span class='go-blue'>not started</span>";
			}
		}

		ngCtrl.generateProgressBars = function (task) {
			var loggedBar = "";
			if (task["estimation"] == 0 && task["logged"] == 0) {
				loggedBar = "";
			}
			else if (task["estimation"] == 0 && task["logged"] != 0) {
				loggedBar = "<div class=\"progress-bar progress-bar-danger\" style=\"width: 100%\"></div>";
			}
			else {
				var progressPercent = Math.round((task["logged"] / task["estimation"]) * 100);
				//1. Logged are less than estimated
				if (progressPercent <= 100) {
					loggedBar = "<div class=\"progress-bar progress-bar-success\" style=\"width: " + progressPercent + "%\"></div>";
				}
				else {
					progressPercent = Math.round((task["estimation"] / task["logged"]) * 100);
					var overPercent = 100 - progressPercent;
					//2. logged are more than estimated - the difference should be a red bar
					loggedBar = "<div class=\"progress-bar progress-bar-success\" style=\"width: " + progressPercent + "%\"></div>" + "<div class=\"progress-bar progress-bar-danger\" style=\"width: " + overPercent + "%\"></div>";

				}
			}
			return $sce.trustAsHtml(loggedBar);
		}

		ngCtrl.getSprintProgressString = function () {
			if (ngCtrl.sprint["estimation"] == 0) {
				return "0%";
			}
			var progressPercent = Math.round((ngCtrl.sprint["logged"] / ngCtrl.sprint["estimation"]) * 100)
			if (progressPercent > 100) {
				return "<span class='go-red'>" + progressPercent + "%</span>"
			}
			else {
				return progressPercent + "%";
			}

		}


		ngCtrl.getDaysLeftString = function () {
			var returnString = "";
			var now = new Date();
			var untilStartDate = moment(ngCtrl.sprint.start_date).diff(now, 'days');
			if (untilStartDate > 0) {
				return "<span class='go-gray'>not started</span>";
			}
			else if (untilStartDate == 0) {
				//check if it is today or tomorrow
				if(moment(ngCtrl.sprint.start_date).date() == moment().date()){
					return "<span class='go-orange'>starts today</span>";
				}
				else {
					return "<span class='go-orange'>starts tomorrow</span>";
				}
			}
			var diffDays = moment(ngCtrl.sprint.end_date).add(1, 'days').diff(now, 'days');
			if (diffDays > 0) {
				returnString = diffDays + " days left";
			}
			else if (diffDays == 0) {
				returnString = "<span class='go-orange'>due date</span>";
			}
			else {
				returnString = "<span class='go-red'>" + diffDays * -1 + " days overdue</span>";
			}
			return returnString;
		}

		//#endregion

		//#region << Modals >>

		ngCtrl.showSprintsListModal = function () {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'sprintListModal.html',
				controller: 'ShowSprintsListModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					parentCtrl: function () { return ngCtrl; },
					resolvedSprintList: resolveSprintList
				}
			});
		}

		//Resolve function tree
		var resolveSprintList = function () {
			// Initialize
			var defer = $q.defer();

			// Process
			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				defer.reject();
			}
			function successCallback(response) {
				defer.resolve(response.object);
			}

			webvellaProjectsService.getAllSprints(1, 10, successCallback, errorCallback);

			return defer.promise;
		}

		ngCtrl.newCommentModal = function (task) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'newCommentModal.html',
				controller: 'NewCommentModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					parentCtrl: function () { return ngCtrl; },
					resolvedTask: function () { return task; }
				}
			});
		}

		ngCtrl.logTimeModal = function (task) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'logTimeModal.html',
				controller: 'LogTimeModalController',
				controllerAs: "popupCtrl",
				size: "lg",
				resolve: {
					parentCtrl: function () { return ngCtrl; },
					resolvedTask: function () { return task; }
				}
			});
		}

		ngCtrl.detachTaskModal = function (task) {
			var relationName = "wv_sprint_n_n_wv_task";
			var originFieldRecordId = $stateParams.sprintId;
			var attachTargetFieldRecordIds = [];
			var detachTargetFieldRecordIds = []
			detachTargetFieldRecordIds.push(task.id);
			webvellaCoreService.updateRecordRelation(relationName, originFieldRecordId, attachTargetFieldRecordIds, detachTargetFieldRecordIds, detachSuccess, errorCallback);

			function detachSuccess(response) {
				var index = -1;
				if (task.status == "not started") {
					for (var i = 0; i < ngCtrl.sprint.tasks_not_started.length; i++) {
						if (ngCtrl.sprint.tasks_not_started[i].id == task.id) {
							ngCtrl.sprint.tasks_not_started.splice(i, 1);
						}
					}
				}
				else {
					for (var i = 0; i < ngCtrl.sprint.tasks_in_progress.length; i++) {
						if (ngCtrl.sprint.tasks_in_progress[i].id == task.id) {
							ngCtrl.sprint.tasks_in_progress.splice(i, 1);
						}
					}
				}

				ngCtrl.sprint.estimation = ngCtrl.sprint.estimation - task.estimation;
				ngCtrl.sprint.logged = ngCtrl.sprint.logged - task.logged;
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
		}

		ngCtrl.attachTaskModal = function (status) {
			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'attachTaskModal.html',
				controller: 'AttachTaskModalController',
				controllerAs: "popupCtrl",
				size: "width-75p",
				resolve: {
					parentCtrl: function () { return ngCtrl; },
					resolvedSprintAvailableTasks: function () { return resolveSprintAvailableTasks(status); },
					status: function () { return status; },
				}
			});
		}

		//Resolve function tree
		var resolveSprintAvailableTasks = function (status) {
			// Initialize
			var defer = $q.defer();

			// Process
			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
				defer.reject();
			}
			function successCallback(response) {
				defer.resolve(response.object);
			}

			webvellaProjectsService.getProjectSprintAllTasks($stateParams.sprintId, $stateParams.scope, status, 1, 10, successCallback, errorCallback);

			return defer.promise;
		}

		//#endregion

		//#region << Sortable >>
		ngCtrl.dragControlListeners = {
			accept: function (sourceItemHandleScope, destSortableScope) {
				return true
			},
			itemMoved: function (eventObj) {
				ngCtrl.taskMoved(eventObj);
			},
			orderChanged: function (eventObj) {
			}
		};

		ngCtrl.taskMoved = function (eventObj) {
			var newStatusCode = eventObj.dest.sortableScope.element[0].parentElement.id;
			var movedTask = eventObj.source.itemScope.modelValue;
			var newTaskObj = {};
			newTaskObj.id = movedTask.id;
			switch (newStatusCode) {
				case "not-started":
					newTaskObj.status = "not started";
					break;
				case "in-progress":
					newTaskObj.status = "in progress";
					break;
				case "completed":
					newTaskObj.status = "completed";
					break;
			}
			webvellaCoreService.patchRecord(movedTask.id, "wv_task", newTaskObj, moveSuccessCallback, moveErrorCallback)
		}

		function moveSuccessCallback() {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + "Task status changed"
				});
			});
		}
		function moveErrorCallback() {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
			$state.reload();
		}
		//#endregion

	}
	//#endregion


	/*#region << Modals >>*/
	//// Modal Controllers
	ShowSprintsListModalController.$inject = ['parentCtrl', 'resolvedSprintList', '$uibModalInstance', '$log', 'webvellaProjectsService', 'ngToast', '$timeout', '$state', '$translate'];
	function ShowSprintsListModalController(parentCtrl, resolvedSprintList, $uibModalInstance, $log, webvellaProjectsService, ngToast, $timeout, $state, $translate) {

		var popupCtrl = this;
		popupCtrl.parentData = parentCtrl;
		popupCtrl.sprints = resolvedSprintList;
		popupCtrl.pageSize = 10;
		popupCtrl.currentPage = 1;

		popupCtrl.selectPage = function (page) {
			webvellaProjectsService.getAllSprints(page, popupCtrl.pageSize, successCallback, errorCallback);

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
			}
			function successCallback(response) {
				popupCtrl.sprints = response.object;
				popupCtrl.currentPage = page;
			}
		}

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
	};


	NewCommentModalController.$inject = ['parentCtrl', 'resolvedTask', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$translate'];
	function NewCommentModalController(parentCtrl, resolvedTask, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $translate) {

		var popupCtrl = this;
		popupCtrl.record = {};
		popupCtrl.record.id = null;
		popupCtrl.record.content = null;
		popupCtrl.record.task_id = fastCopy(resolvedTask.id);

		popupCtrl.ok = function () {
			webvellaCoreService.createRecord("wv_project_comment", popupCtrl.record, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
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

	LogTimeModalController.$inject = ['parentCtrl', 'resolvedTask', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$translate'];
	function LogTimeModalController(parentCtrl, resolvedTask, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $translate) {

		var popupCtrl = this;
		popupCtrl.record = {};
		popupCtrl.record.id = null;
		popupCtrl.record.hours = 0;
		popupCtrl.record.billable = true;
		popupCtrl.record.description = null;
		popupCtrl.record.log_date = new Date();
		popupCtrl.record.task_id = fastCopy(resolvedTask.id);

		popupCtrl.ok = function () {
			popupCtrl.record.log_date = moment(popupCtrl.record.log_date).utc().toDate();
			webvellaCoreService.createRecord("wv_timelog", popupCtrl.record, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
			parentCtrl.sprint.logged += popupCtrl.record.hours;
			resolvedTask.logged += popupCtrl.record.hours;
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

	AttachTaskModalController.$inject = ['parentCtrl', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$translate', 'resolvedSprintAvailableTasks', 'status', '$stateParams'];
	function AttachTaskModalController(parentCtrl, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $translate, resolvedSprintAvailableTasks, status, $stateParams) {

		var popupCtrl = this;
		popupCtrl.hasError = false;
		popupCtrl.status = status;
		popupCtrl.tasks = resolvedSprintAvailableTasks;
		popupCtrl.sprint = parentCtrl.sprint;
		popupCtrl.attachedTasksIds = [];
		popupCtrl.loading = {};
		if (popupCtrl.status == "not started") {
			popupCtrl.sprint.tasks_not_started.forEach(function (task) {
				popupCtrl.attachedTasksIds.push(task.id);
			});
		}
		else {
			popupCtrl.sprint.tasks_in_progress.forEach(function (task) {
				popupCtrl.attachedTasksIds.push(task.id);
			});
		}

		popupCtrl.isAttached = function (task) {
			if (popupCtrl.attachedTasksIds.indexOf(task.id) > -1) {
				return true;
			}
			else {
				return false;
			}
		}

		popupCtrl.attachTask = function (task) {
			var relationName = "wv_sprint_n_n_wv_task";
			var originFieldRecordId = popupCtrl.sprint.id;
			var attachTargetFieldRecordIds = [];
			attachTargetFieldRecordIds.push(task.id);
			var detachTargetFieldRecordIds = []
			popupCtrl.loading[task.id] = true;
			webvellaCoreService.updateRecordRelation(relationName, originFieldRecordId, attachTargetFieldRecordIds, detachTargetFieldRecordIds, attachSuccess, errorCallback);

			function attachSuccess(response) {
				if (popupCtrl.status == "not started") {
					popupCtrl.sprint.tasks_not_started.push(task);
				}
				else {
					popupCtrl.sprint.tasks_in_progress.push(task);
				}
				popupCtrl.attachedTasksIds.push(task.id);
				popupCtrl.sprint.estimation = popupCtrl.sprint.estimation + task.estimation;
				popupCtrl.sprint.logged = popupCtrl.sprint.logged + task.logged;
				popupCtrl.loading[task.id] = false;

			}
		}

		popupCtrl.detachTask = function (task) {
			var relationName = "wv_sprint_n_n_wv_task";
			var originFieldRecordId = popupCtrl.sprint.id;
			var attachTargetFieldRecordIds = [];
			var detachTargetFieldRecordIds = []
			detachTargetFieldRecordIds.push(task.id);
			popupCtrl.loading[task.id] = true;
			webvellaCoreService.updateRecordRelation(relationName, originFieldRecordId, attachTargetFieldRecordIds, detachTargetFieldRecordIds, detachSuccess, errorCallback);

			function detachSuccess(response) {
				var index = -1;
				if (popupCtrl.status == "not started") {
					for (var i = 0; i < popupCtrl.sprint.tasks_not_started.length; i++) {
						if (popupCtrl.sprint.tasks_not_started[i].id == task.id) {
							popupCtrl.sprint.tasks_not_started.splice(i, 1);
						}
					}
				}
				else {
					for (var i = 0; i < popupCtrl.sprint.tasks_in_progress.length; i++) {
						if (popupCtrl.sprint.tasks_in_progress[i].id == task.id) {
							popupCtrl.sprint.tasks_in_progress.splice(i, 1);
						}
					}
				}

				index = popupCtrl.attachedTasksIds.indexOf(task.id);
				popupCtrl.attachedTasksIds.splice(index, 1);

				popupCtrl.sprint.estimation = popupCtrl.sprint.estimation - task.estimation;
				popupCtrl.sprint.logged = popupCtrl.sprint.logged - task.logged;
				popupCtrl.loading[task.id] = false;
			}
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

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};
	};

	/*#endregion*/

})();
