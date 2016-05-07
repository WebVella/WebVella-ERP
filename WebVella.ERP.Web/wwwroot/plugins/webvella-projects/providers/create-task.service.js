// IMPORTANT: You must always have at least webvellaViewActionService defined or the page will not load
// The methods inside it are optional 
// For usage in action items, the service is bound to the controller with ngCtrl.actionService. So if 
// what to use a test method from this service in an action you need to call like 'ng-click=""ngCtrl.actionService.test()""'
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Objects accessible through the ngCtrl:
// ngCtrl.view.data => the data of the main view
// ngCtrl.view.meta => the meta of the main view
// ngCtrl.selectedSidebarItem.data => the data of the current sidebar view / list
// ngCtrl.selectedSidebarItem.meta => the meta of the current sidebar view / list
// ngCtrl.entity    => the current entity's meta
// ngCtrl.entityRelations => list of all relations of the entity
// ngCtrl.areas		=> the current areas in the site and their meta, attached entities and etc.
// ngCtrl.currentUser => the current user
// ngCtrl.$sessionStorage => copy of the session local storage service
// ngCtrl.stateParams => all state parameters

// IMPORTANT: all data is two way bound, which means it will be watched by angular and any changes propagated. If you want to get a copy of one of the objects, without the binding
// use the var copyObject = fastCopy(originalObject); . This will break the binding.

(function () {
	'use strict';
	angular
    .module('webvellaAreas')
	.service('webvellaViewActionService', service);
	service.$inject = ['$log','$location', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter', 'webvellaCoreService', 'webvellaProjectsService', '$translate'];
	function service($log, $location, $http, wvAppConstants, $timeout, ngToast, $filter, webvellaCoreService, webvellaProjectsService, $translate) {
		var serviceInstance = this;
		//PRELOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Preload function. Here you can place script that will be executed BEFORE the page load (while resolving the state)
		serviceInstance.preload = preload;
		function preload(defer, state) {

			defer.resolve();
			return defer.promise;
		}
		//ONLOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Onload function. Here you can place script that will be executed as the FIRST function from the controller after
		// the main objects are initialized. With the ngCtrl you can access the scope of the controller
		serviceInstance.onload = onload;
		function onload(ngCtrl, rootScope, state) {

			return true; //true for success, or string for an error message to be presented to the user
		}
		//POSTLOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Postload function. Here you can place script that will be executed as the LAST function from the controller
		//with the ngCtrl you can access the scope of the controller
		serviceInstance.postload = postload;
		function postload(ngCtrl, rootScope, state) {

			return true; //true for success, or string for an error message to be presented to the user
		}
		//FIELD UPDATE
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		serviceInstance.createTask = createTask;
		function createTask(ngCtrl) {
			webvellaProjectsService.createTask(ngCtrl.view.data, createTaskSuccessCallback, createTaskErrorCallback)
		}
		function createTaskSuccessCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + ' ' + response.message
				});
			});
			$location.path("/areas/projects/wv_task/list-general/my_tasks/1");
		}
		function createTaskErrorCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
		}
		//DELETE RECORD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		serviceInstance.deleteRecord = deleteRecord;
		function deleteRecord(ngCtrl) {
			webvellaCoreService.viewAction_deleteRecord(ngCtrl);
		}

		//CUSTOM
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//User functions. Here you can place all action functions that you need to be executed on action item interaction. 
		//They can be used inside your actions or custom views with ngCtrl.actionService.function_name(params). As params
		//you can use all data from the controller scope


	}
})();