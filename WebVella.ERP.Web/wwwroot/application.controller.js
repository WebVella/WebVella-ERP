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
    controller.$inject = ['$rootScope'];

    /* @ngInject */
    function controller($rootScope) {
        /* jshint validthis:true */
        var appData = this;
        appData.pageTitle = 'WebVella ERP';
        $rootScope.$watch("pageTitle", function (newValue,oldValue) {
            appData.pageTitle = newValue;
        });

        activate();

        function activate() { }
    }

})();
