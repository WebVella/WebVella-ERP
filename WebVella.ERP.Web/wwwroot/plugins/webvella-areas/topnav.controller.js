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
    controller.$inject = ['$localStorage', '$log', '$rootScope', '$state', '$stateParams', 'resolvedSitemap', '$timeout', 'webvellaAreasService'];

    /* @ngInject */
    function controller($localStorage,$log, $rootScope, $state, $stateParams, resolvedSitemap, $timeout, webvellaAreasService) {
    	$log.debug('webvellaAreas>topnav> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var topnavData = this;
        topnavData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
        topnavData.navigateToHome = function () {
            $timeout(function () {
                $state.go("webvella-desktop-browse");
            }, 0);
        }
        topnavData.$storage = $localStorage;
        topnavData.toggleSideNav = function () {
        	topnavData.$storage.isMiniSidebar = !topnavData.$storage.isMiniSidebar;
        }

        $log.debug('webvellaAreas>topnav> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

})();
