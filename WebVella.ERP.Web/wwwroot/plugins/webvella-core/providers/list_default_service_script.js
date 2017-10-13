// This is a lazy loaded service that is injected into the controller. Here you can write your own script that can interact with your
// dynamic html template.

// IMPORTANT: the service name should be generated with the format : "entityName_listName_list_service". This is how it will be looked for
// end injected in the controller. If it is not found it will not be injected.
// The service will be loaded in the controller as ngCtrl.actionService
// So if what to use a test method from this service in an action you need to call like 'ng-click="ngCtrl.actionService.test(ngCtrl)"'
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Objects accessible through the ngCtrl:
// ngCtrl.list.data => the records' data array
// ngCtrl.list.meta => the records' list meta
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
	.service('webvellaDefaultListActionService', service); // <<<<< CHANGE THE NAME TO COMPLY WITH THE FORMAT "entityName_listName_list_service"
	service.$inject = ['$log', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter', 'webvellaCoreService'];
	function service($log, $http, wvAppConstants, $timeout, ngToast, $filter, webvellaCoreService) {
		var serviceInstance = this;

		// Define your functions here
		serviceInstance.foo = foo;
		function foo(){
			console.log("foo");
		}

	}
})();