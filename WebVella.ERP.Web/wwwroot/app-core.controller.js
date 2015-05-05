/* app-core.controller.js */

/**
* @desc the core application controller
*/

(function () {
    'use strict';

    angular
        .module('appCore')
        .controller('AppCoreController', controller);

  
    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope'];

    /* @ngInject */
    function controller($rootScope) {
        /* jshint validthis:true */
        var appData = this;

        appData.pageTitle = 'WebVella ERP';
        appData.class = "green";
        appData.isMiniSidebar = false;

        appData.toggleSideNav = function () {
            appData.isMiniSidebar = !appData.isMiniSidebar;
        }

        activate();

        function activate() {

        }
    }

})();
