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
	service.$inject = ['$log', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter','webvellaCoreService'];
	function service($log, $http, wvAppConstants, $timeout, ngToast, $filter,webvellaCoreService) {
		var serviceInstance = this;
		//PRELOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Preload function. Here you can place script that will be executed BEFORE the page load (while resolving the state)
		serviceInstance.preload = preload;
		function preload(defer,state){

			defer.resolve();
			return defer.promise;
		}
		//ONLOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Onload function. Here you can place script that will be executed as the FIRST function from the controller after
		// the main objects are initialized. With the ngCtrl you can access the scope of the controller
		serviceInstance.onload = onload;
		function onload(ngCtrl,rootScope, state){

			return true; //true for success, or string for an error message to be presented to the user
		}
		//POSTLOAD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Postload function. Here you can place script that will be executed as the LAST function from the controller
		//with the ngCtrl you can access the scope of the controller
		serviceInstance.postload = postload;
		function postload(ngCtrl,rootScope, state){

			return true; //true for success, or string for an error message to be presented to the user
		}
		//FIELD UPDATE
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		serviceInstance.fieldUpdate = fieldUpdate;
		function fieldUpdate(item,data,recordId,ngCtrl){
			 webvellaCoreService.viewAction_fieldUpdate(item,data,recordId,ngCtrl);
		}
		//DELETE RECORD
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		serviceInstance.deleteRecord = deleteRecord;
		function deleteRecord(ngCtrl){
			 webvellaCoreService.viewAction_deleteRecord(ngCtrl);
		}

		//CUSTOM
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//User functions. Here you can place all action functions that you need to be executed on action item interaction. 
		//They can be used inside your actions or custom views with ngCtrl.actionService.function_name(params). As params
		//you can use all data from the controller scope
	

	}
})();