// This is a lazy loaded service that is injected into the controller. Here you can write your own script that can interact with your
// dynamic html template.

// IMPORTANT: the service name should be generated with the format : "entityName_viewName_view_service". This is how it will be looked for
// end injected in the controller. If it is not found it will not be injected.
// The service will be loaded in the controller as ngCtrl.actionService
// So if what to use a test method from this service in an action you need to call like 'ng-click=""ngCtrl.actionService.test()""'
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
	.service('wv_task_create_view_service', service);
	service.$inject = ['$log','$location', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter', 'webvellaCoreService', 'webvellaProjectsService', '$translate'];
	function service($log, $location, $http, wvAppConstants, $timeout, ngToast, $filter, webvellaCoreService, webvellaProjectsService, $translate) {
		var serviceInstance = this;

		//Create task
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

	}
})();