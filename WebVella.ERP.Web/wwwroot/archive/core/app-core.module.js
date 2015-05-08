/* app-core.module.js */

/**
* @desc this is the core angularJS application for the WebVella ERP
*/

// In order to dynamically add the plug-in names, JS and CSS includes, the appCore module definition is in /Views/Home/Index.cshtml, which is also the main one pager wrapper

(function () {
    'use strict';

    angular
        .module('wvApp')
        .config(config)
        .run(run);


    // Configuration ///////////////////////////////////
    config.$inject = ['$urlRouterProvider'];

    /* @ngInject */
    function config($urlRouterProvider) {
        $urlRouterProvider.otherwise('/');
    };


    // Run //////////////////////////////////////
    run.$inject = ['$rootScope'];

    /* @ngInject */
    function run($rootScope) {

    };



})();