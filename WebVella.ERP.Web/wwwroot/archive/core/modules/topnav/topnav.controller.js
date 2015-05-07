/* topnav.controller.js */

/**
* @desc this controller manages the top navigation section of page
*/

(function () {
    'use strict';

    angular
        .module('topnavModule', [])
        .controller('TopnavController', controller);

    // Controller ///////////////////////////////
    controller.$inject = ['$state','$rootScope']; 

    /* @ngInject */
    function controller($state,$rootScope) {
        /* jshint validthis:true */
        var topnavData = this;
        topnavData.currentArea = null;

        activate();

        function activate() {

            topnavData.navigateToHome = function () {
                $state.go('home');
            }

            $rootScope.$watch('currentArea', function (newValue, oldValue) {
                topnavData.currentArea = newValue;
            });

        }
    }
    
})();
