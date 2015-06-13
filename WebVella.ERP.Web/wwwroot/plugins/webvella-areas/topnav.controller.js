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
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedSitemap', '$timeout', 'webvellaAreasService', 'webvellaRootService'];

    /* @ngInject */
    function controller($log, $rootScope, $state, $stateParams, resolvedSitemap, $timeout, webvellaAreasService, webvellaRootService) {
        $log.debug('webvellaAreas>topnav> BEGIN controller.exec');
        /* jshint validthis:true */
        var topnavData = this;
        topnavData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
        topnavData.navigateToHome = function () {
            $timeout(function () {
                $state.go("webvella-desktop-browse");
            }, 0);
        }

        topnavData.toggleSideNav = function () {
        	webvellaRootService.toggleSidebar();
        }

        $log.debug('webvellaAreas>topnav> END controller.exec');
    }

})();
