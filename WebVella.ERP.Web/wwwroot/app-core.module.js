/* app-core.module.js */

/**
* @desc this is the core angularJS application for the WebVella ERP
*/

// In order to dynamically add the plug-in names, JS and CSS includes, the appCore module definition is in /Views/Home/Index.cshtml, which is also the main one pager wrapper

(function () {
    'use strict';

    angular
        .module('appCore')
        .config(config)
        .run(run);


    // Configuration ///////////////////////////////////
    config.$inject = ['$urlRouterProvider'];

    /* @ngInject */
    function config($urlRouterProvider) {
        $urlRouterProvider.otherwise('/');
    };


    // Run //////////////////////////////////////
    run.$inject = ['siteMetaService','$rootScope'];

    /* @ngInject */
    function run(siteMetaService, $rootScope) {

        // Site Meta Object
        //  Managed with the siteMetaService. Could be directly accessible but better through the service
        $rootScope.siteMetaObject = {}; 

        function successCallbackSiteMeta() {
            //Value is already updated within the service. Any additional actions should be following
        };
        function errorCallbackSiteMeta() {
            //Error is already handled within the service. Any additional actions should be following
        };
        siteMetaService.updateSiteMetaObject(successCallbackSiteMeta, errorCallbackSiteMeta);


        // Hide Visual components
        $rootScope.hideSidebar = false;

    };



})();