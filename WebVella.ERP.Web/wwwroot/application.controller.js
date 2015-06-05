/* application.controller.js */

/**
* @desc the main application controller
*/

(function () {
    'use strict';

    angular
        .module('wvApp')
        .controller('ApplicationController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope', '$log', '$cookies'];

    /* @ngInject */
    function controller($rootScope, $log, $cookies) {
        $log.debug('vwApp> BEGIN controller.exec');
        /* jshint validthis:true */
        var appData = this;
        //Set page title
        appData.pageTitle = 'WebVella ERP';
        $rootScope.$on("application-pageTitle-update", function (event,newValue) {
            appData.pageTitle = newValue;
        });
        //Set the body color
        appData.bodyColor = "no-color";
        $rootScope.$on("application-body-color-update", function (event, color) {
            appData.bodyColor = color;
        });
        //Side menu toggle
        appData.isMiniSidebar = false;
        if ($cookies.isMiniSidebar) {
            appData.isMiniSidebar = true;
        }
        
        $rootScope.$on("application-sidebar-mini-toggle", function (event) {
            appData.isMiniSidebar = !appData.isMiniSidebar;
            $cookies.isMiniSidebar = appData.isMiniSidebar;

        });
        //Side menu visibility
        appData.sideMenuIsVisible = true;
        $rootScope.$on("application-body-sidebar-menu-isVisible-update", function (event, isVisible) {
            appData.sideMenuIsVisible = isVisible;
        });


        activate();
        $log.debug('wvApp> END controller.exec');
        function activate() {


        }
    }

})();
