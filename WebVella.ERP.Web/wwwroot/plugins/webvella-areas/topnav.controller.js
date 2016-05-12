/* topnav.controller.js */

/**
* @desc this controller manages the topnav of the areas section
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAreasTopnavController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$localStorage', '$log', '$rootScope', '$state', '$stateParams', 'resolvedAreas', '$timeout','webvellaCoreService', 'resolvedCurrentEntityMeta'];

    
    function controller($localStorage, $log, $rootScope, $state, $stateParams, resolvedAreas, $timeout,webvellaCoreService, resolvedCurrentEntityMeta) {
        
        var topnavData = this;
        topnavData.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		topnavData.currentEntity = resolvedCurrentEntityMeta;
        topnavData.$storage = $localStorage;
        topnavData.toggleSideNav = function () {
        	topnavData.$storage.isMiniSidebar = !topnavData.$storage.isMiniSidebar;
        }

        topnavData.logout = function () {
        	webvellaCoreService.logout(
                    function (response) {
                    	//  $window.location = '#/login';
                    	$timeout(function () {
                    		$state.go('webvella-core-login');
                    	}, 0);
                    },
                    function (response) { });
        }
    }

})();
