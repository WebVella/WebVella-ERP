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
    controller.$inject = ['$rootScope','$log'];

    /* @ngInject */
    function controller($rootScope, $log) {
        $log.debug('vwApp> BEGIN controller.exec');
        /* jshint validthis:true */
        var appData = this;
        appData.pageTitle = 'WebVella ERP';
        $rootScope.$on("application-pageTitle", function (event,newValue) {
            appData.pageTitle = newValue;
        });
        activate();
        $log.debug('wvApp> END controller.exec');
        function activate() {


        }
    }

})();
