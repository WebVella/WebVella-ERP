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

		//#region << Task >>
		serviceInstance.createTask = createTask;

		//#endregion

		//#endregion


		//#region << Functions >> ///////////////////////////////////////////////////////////////////////////////////

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

		//#region << Task >>
		///////////////////////
		function createTask(postObject, successCallback, errorCallback) {
			$http({ method: 'POST', url: '/plugins/webvella-projects/api/task', data: postObject }).then(function getSuccessCallback(response) { handleSuccessResult(response.data, response.status, successCallback, errorCallback); }, function getErrorCallback(response) { handleErrorResult(response.data, response.status, errorCallback); });
		}

		//#endregion

		//#endregion
	}
})();
