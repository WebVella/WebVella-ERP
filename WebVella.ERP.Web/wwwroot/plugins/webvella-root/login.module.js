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
        .controller('WebVellaRootLoginController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-root-login', {
            url: '/login',
            views: {
                "rootView": {
                    controller: 'WebVellaRootLoginController',
                    templateUrl: '/plugins/webvella-root/login.view.html',
                    controllerAs: 'loginData'
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
        $log.debug('webvellaRoot>login> BEGIN module.run');

        $log.debug('webvellaRoot>login> END module.run');
    };


    // Resolve Function /////////////////////////
    resolvingFunction.$inject = ['$q'];

    /* @ngInject */
    function resolvingFunction($q) {
        $log.debug('webvellaRoot>login> BEGIN state.resolved');

        // Initialize
        var defer = $q.defer();
        
        // Process
        defer.resolve("test");

        // Return
        $log.debug('webvellaRoot>login> END state.resolved');
        return defer.promise;

    }




    // Controller ///////////////////////////////
    controller.$inject = ['$state', '$log', 'webvellaRootService', '$timeout'];

    /* @ngInject */
    function controller($state, $log, webvellaRootService, $timeout) {
        $log.debug('webvellaRoot>login> BEGIN controller.exec');
        /* jshint validthis:true */
        var loginData = this;
        loginData.email = "erp@webvella.com";
        loginData.password = "ttg";
        loginData.rememberMe = true;
        loginData.pageTitle = "Login";
        webvellaRootService.setPageTitle(loginData.pageTitle);
        activate();
        $log.debug('webvellaRoot>login> END controller.exec');

        loginData.doLogin = function(){
            webvellaRootService.login( loginData,
                                      function (response ) {
                                          $timeout(function () {
                                              $state.go('webvella-desktop-browse');
                                          }, 0);
                                      },
                                      function (response) {
                                         //show validation
                                      });
        }

        function activate() {

        }
    }

})();
