/* sidebar.controller.js */

/**
* @desc this controller manages the sidebar of the areas section
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAreasRecordViewSidebarController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedSitemap', 'webvellaAreasService'];

    /* @ngInject */
    function controller($log, $rootScope, $state,$stateParams, resolvedSitemap, webvellaAreasService) {
        $log.debug('webvellaAreas>sidebar> BEGIN controller.exec');
        /* jshint validthis:true */
        var sidebarData = this;
        sidebarData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
        $log.debug('webvellaAreas>sidebar> END controller.exec');
    }

})();
