/* sidebar.controller.js */

/**
* @desc this controller manages the sidebar of the areas section
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAreasSidebarController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedSitemap', 'webvellaAreasService'];

    /* @ngInject */
    function controller($log, $rootScope, $state,$stateParams, resolvedSitemap, webvellaAreasService) {
    	$log.debug('webvellaAreas>sidebar> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var sidebarData = this;
        sidebarData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
        sidebarData.currentArea.subscriptions = angular.fromJson(sidebarData.currentArea.subscriptions);
        $log.debug('webvellaAreas>sidebar> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

})();
