/* topnav.controller.js */

/**
* @desc this controller manages the top navigation section of page
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin')  //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAdminEntitiesTopnavController', controller);

    // Controller ///////////////////////////////
    controller.$inject = ['$state', '$rootScope', '$timeout'];

    /* @ngInject */
    function controller($state, $rootScope, $timeout) {
        /* jshint validthis:true */
        var topnavData = this;
        topnavData.currentArea = null;

        activate();

        function activate() {

            topnavData.navigateToHome = function () {
                $timeout(function () {
                    $state.go('home');
                }, 0);
                
            }

            $rootScope.$watch('currentArea', function (newValue, oldValue) {
                topnavData.currentArea = newValue;
            });

        }
    }
    
})();
