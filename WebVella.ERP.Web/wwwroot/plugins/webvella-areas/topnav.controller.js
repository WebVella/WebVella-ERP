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
    controller.$inject = ['$localStorage', '$log', '$rootScope', '$state', '$stateParams', 'resolvedSitemap', '$timeout', 'webvellaAreasService', 'webvellaAdminService'];

    /* @ngInject */
    function controller($localStorage, $log, $rootScope, $state, $stateParams, resolvedSitemap, $timeout, webvellaAreasService, webvellaAdminService) {
    	$log.debug('webvellaAreas>topnav> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var topnavData = this;
        topnavData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
        topnavData.$storage = $localStorage;
        topnavData.toggleSideNav = function () {
        	topnavData.$storage.isMiniSidebar = !topnavData.$storage.isMiniSidebar;
        }

        topnavData.logout = function () {
        	webvellaAdminService.logout(
                    function (response) {
                    	//  $window.location = '#/login';
                    	$timeout(function () {
                    		$state.go('webvella-root-login');
                    	}, 0);
                    },
                    function (response) { });
        }

        $log.debug('webvellaAreas>topnav> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

})();
