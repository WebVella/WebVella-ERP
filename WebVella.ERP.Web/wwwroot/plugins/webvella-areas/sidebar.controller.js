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
    controller.$inject = ['$log','$rootScope', '$state'];

    /* @ngInject */
    function controller($log, $rootScope, $state) {
        $log.debug('webvellaAreas>sidebar> BEGIN controller.exec');
        /* jshint validthis:true */
        var sidebarData = this;

        activate();
        $log.debug('webvellaAreas>sidebar> END controller.exec');
        function activate() { }
    }

})();
