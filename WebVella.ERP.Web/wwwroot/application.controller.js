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

        activate();

        function activate() {
            //Register listener for any changes of the pageTitle that may be performed by the plugins
            $rootScope.$watch("pageTitle", function (newValue, oldValue) {
                appData.pageTitle = newValue;
            });

        }
    }

})();
