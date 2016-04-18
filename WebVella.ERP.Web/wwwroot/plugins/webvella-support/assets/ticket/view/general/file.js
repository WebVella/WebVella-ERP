/* ticket-general-view */
(function () {
	'use strict';

	angular
    .module('webvellaAreas')
	.service('webvellaActionService', service);

	service.$inject = ['$log', '$http', 'wvAppConstants', '$timeout', 'ngToast', '$filter','webvellaAreasService'];

	/* @ngInject */
	function service($log, $http, wvAppConstants, $timeout, ngToast, $filter,webvellaAreasService) {
		var serviceInstance = this;
		serviceInstance.test = test;

		function test(entityName) {
			$log.debug('webvellaAreas>providers>action.service>test> function called ' + moment().format('HH:mm:ss SSSS'));
			alert("test called with " + entityName);
		}


	}
	console.log("boz");

})();