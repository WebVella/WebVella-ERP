/* sidebar.controller.js */

/**
* @desc this controller manages the sidebar of the areas section
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAdminSidebarController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$log','$rootScope', '$state'];

    /* @ngInject */
    function controller($log, $rootScope, $state) {
        $log.debug('webvellaAdmin>sidebar> BEGIN controller.exec');
        /* jshint validthis:true */
        var sidebarData = this;

        activate();
        $log.debug('webvellaAdmin>sidebar> END controller.exec');
        function activate() { }
    }

})();
