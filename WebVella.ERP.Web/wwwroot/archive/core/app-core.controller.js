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
    controller.$inject = ['$rootScope','$state'];

    /* @ngInject */
    function controller($rootScope, $state) {
        /* jshint validthis:true */
        var appData = this;
        appData.pageTitle = 'WebVella ERP';
        appData.isMiniSidebar = false;
        appData.currentArea = {};
        appData.currentArea.color = "blue-gray";


        appData.toggleSideNav = function () {
            appData.isMiniSidebar = !appData.isMiniSidebar;
        }

        activate();

        function activate() {
            $rootScope.$watch("currentArea", function (newValue,oldValue) {
                appData.currentArea = newValue;
            });
        }
    }

})();
