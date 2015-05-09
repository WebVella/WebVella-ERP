/* home.module.js */

/**
* @desc handles the "/" url. Manages a login form and redirects the user to "webvella-desktop-areas" state if authenticated successfully. The only module that should be accessed by not logged user
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')  //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .run(run)
        .controller('WebVellaRootHomeController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-root-home', {
            parent: 'webvella-root',
            url: '/',
            views: {
                "pluginView": {
                    controller: 'WebVellaRootHomeController',
                    templateUrl: '/plugins/webvella-root/home.view.html',
                    controllerAs: 'homeData'
                }
            },
            resolve: {
                
            }
        });
    };

    // Run //////////////////////////////////////
    run.$inject = ['$log'];
    /* @ngInject */
    function run($log) {
        $log.debug('webvellaRoot>home> BEGIN module.run');

        $log.debug('webvellaRoot>home> END module.run');
    };


    // Resolve Function /////////////////////////
    resolvingFunction.$inject = ['$q'];

    /* @ngInject */
    function resolvingFunction($q) {
        $log.debug('webvellaRoot>home> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();
        
        // Process
        defer.resolve("test");

        // Return
        $log.debug('webvellaRoot>home> END state.resolved');
        return defer.promise;

    }




    // Controller ///////////////////////////////
    controller.$inject = ['$state', 'currentUser', '$log'];

    /* @ngInject */
    function controller($state, currentUser, $log) {
        $log.debug('webvellaRoot>home> BEGIN controller.exec');
        /* jshint validthis:true */
        var homeData = this;
  
        activate();
        $log.debug('webvellaRoot>home> END controller.exec');
        function activate() {

            if (currentUser != null) {
                //If there is an user already logged in, redirect to desktop base
               $state.go("webvella-desktop-base")
            }
        }
    }

})();
