// IMPORTANT: You must always have at least webvellaActionService defined or the page will not load
// The methods inside it are optional 
// For usage in action items, the service is bound to the controller with ngCtrl.actionService. So if 
// what to use a test method from this service in an action you need to call like 'ng-click="ngCtrl.actionService.test()"'
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
	.service('webvellaActionService', service);
	service.$inject = ['$log', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter','webvellaCoreService'];
	function service($log, $http, wvAppConstants, $timeout, ngToast, $filter,webvellaCoreService) {
		var serviceInstance = this;
		
		// PRELOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Execution: This script will be executed BEFORE the page load (while resolving the state)
		// Parameters: defer, state
		serviceInstance.preload = preload;
		function preload(defer,state){
			defer.resolve();
			return defer.promise;
		}

		// ONLOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Execution: This script will be executed as the FIRST function from the controller after the main objects are initialized.
		//			  With the ngCtrl you can access the scope of the controller
		// Parameters: ngCtrl, rootScope, state
		serviceInstance.onload = onload;
		function onload(ngCtrl,rootScope, state){

			return true; //true for success, or string for an error message to be presented to the user
		}
		
		// POSTLOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Execution: This script will be executed as the LAST function from the controller
		// Parameters: ngCtrl, rootScope, state
		serviceInstance.postload = postload;
		function postload(ngCtrl,rootScope, state){

			return true; //true for success, or string for an error message to be presented to the user
		}

		// PAGE TITLE ACTIONS
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Execution: This block should contain functions for the user actions rendered inside the page title above the list.
		// Parameters: It is up to the action template what will be passed as parameter. Accessible is the ngCtrl object.
		
		// <<<<< No functions defined yet >>>>>>

		// PAGE TITLE DROPDOWN ACTIONS
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Execution: This block should contain functions for the user actions rendered inside the page title dropdown above the list
		// Parameters: It is up to the action template what will be passed as parameter. Accessible is the ngCtrl object.
		
		// <<<<< No functions defined yet >>>>>>

		// RECORD ROW ACTIONS
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Execution: This block should contain functions for the user actions rendered on each record row of the list
		// Parameters: It is up to the action template what will be passed as parameter. Accessible are the record object
		//			   and the ngCtrl
		
		serviceInstance.getRecordDetailsUrl = getRecordDetailsUrl;
		function getRecordDetailsUrl(record,ngCtrl) {
			return webvellaCoreService.listAction_getRecordDetailsUrl(record,ngCtrl);
		}

		// RECORD ROW DROPDOWN ACTIONS
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Execution: This block should contain methods for the user actions rendered on the dropdown in each record row of the list
		// Parameters: It is up to the action template what will be passed as parameter. Accessible are the record object
		//			   and the ngCtrl
		
		// Define your functions here

	}
})();