/* projects.service.js */

/**
* @desc Javascript API core service
*/

(function () {
	'use strict';

	angular
        .module('webvellaProjects')
        .service('webvellaProjectsService', service);

	service.$inject = ['$cookies', '$q', '$http', '$log', '$location', 'wvAppConstants', '$rootScope', '$anchorScroll', 'ngToast',
				'$timeout', 'Upload', '$translate', '$filter'];


	function service($cookies, $q, $http, $log, $location, wvAppConstants, $rootScope, $anchorScroll, ngToast,
				$timeout, Upload, $translate, $filter) {
		var serviceInstance = this;

		//#region << Include functions >> ///////////////////////////////////////////////////////////////////////////////////
		serviceInstance.getMyProjectsList = getMyProjectsList;
		serviceInstance.getMyMilestonesList = getMyMilestonesList;
		serviceInstance.getActivitiesList = getActivitiesList;
		serviceInstance.getOwnerLastModifiedTasks = getOwnerLastModifiedTasks;
		serviceInstance.getOwnerLastModifiedBugs = getOwnerLastModifiedBugs;
		serviceInstance.getProjectTimelogReport = getProjectTimelogReport;
		serviceInstance.getAllSprints = getAllSprints;
		serviceInstance.getProjectSprintDetails = getProjectSprintDetails;
		serviceInstance.getProjectSprintAllTasks = getProjectSprintAllTasks;
		//#endregion

		///
		//#region << Functions >> ///////////////////////////////////////////////////////////////////////////////////
		function getMyProjectsList(successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/project/list/my-projects" }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		///////////////////
		function getMyMilestonesList(successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/milestone/list/my-milestones" }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////
		function getActivitiesList(page, label, successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/activity/list/all?page=" + page + "&label=" + label }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////
		function getOwnerLastModifiedTasks(page, successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/task/list/last-updated-for-owner?page=" + page }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		///////////////////
		function getOwnerLastModifiedBugs(page, successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/bug/list/last-updated-for-owner?page=" + page }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		//////////////////
		function getProjectTimelogReport(month, year, successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/report/project-timelog?year=" + year + "&month=" + month }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		/////////////////
		function getAllSprints(page, pageSize, successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/sprint/list?page=" + page + "&pageSize=" + pageSize }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}
		////////////////
		function getProjectSprintDetails(sprintId, scope, successCallback, errorCallback) {
			if (sprintId == "current") {
				$http({ method: 'GET', url: "/plugins/webvella-projects/api/sprint/" + "?scope=" + scope }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
			else {
				$http({ method: 'GET', url: "/plugins/webvella-projects/api/sprint/" + sprintId + "?scope=" + scope }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
			}
		}

		////////////////
		function getProjectSprintAllTasks(sprintId, scope, status, page, pageSize, successCallback, errorCallback) {
			$http({ method: 'GET', url: "/plugins/webvella-projects/api/sprint/" + sprintId + "/available-tasks?scope=" + scope + "&page=" + page + "&pageSize=" + pageSize  + "&status=" + status}).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}


		//#region << Global HTTP Error and Success Handlers >>
		function handleErrorResult(data, status, errorCallback) {
			switch (status) {
				case 401: {
					//handled globally by http observer
					break;
				}
				case 403: {
					//handled globally by http observer
					break;
				}
				case 400:
					if (errorCallback === undefined || typeof (errorCallback) != "function") {
						alert("The errorCallback argument is not a function or missing");
						return;
					}
					data.success = false;
					var messageString = '<div><span class="go-red">Error:</span> ' + data.message + '</div>';
					if (data.errors.length > 0) {
						messageString += '<ul>';
						for (var i = 0; i < data.errors.length; i++) {
							messageString += '<li>' + data.errors[i].message + '</li>';
						}
						messageString += '</ul>';
					}
					ngToast.create({
						className: 'error',
						content: messageString,
						timeout: 7000
					});
					errorCallback(data);
					break;
				default:
					alert("An API call finished with error: " + status);
					break;
			}
		}
		function handleSuccessResult(data, status, successCallback, errorCallback) {
			if (successCallback === undefined || typeof (successCallback) != "function") {
				alert("The successCallback argument is not a function or missing");
				return;
			}

			if (!data.success) {
				//when the validation errors occurred
				if (errorCallback === undefined || typeof (errorCallback) != "function") {
					alert("The errorCallback argument in handleSuccessResult is not a function or missing");
					return;
				}
				errorCallback(data);
			}
			else {
				successCallback(data);
			}

		}
		//#endregion

		//#endregion
	}
})();
