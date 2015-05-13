/* base.module.js */

/**
* @desc base / root module of the application. Core task is to bootstrap and initialize the application and get the layouts. 
* To achieve this purpose it manages the abstract 'webvella-root-layout-*' state which resolves and pass to all its children 
* the core data loaded by the database.
* It also ensures that not logged users have access only to the login form at "/"
* Data storage: $rootscope.plugins.webvellaRoot
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaRootBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-root', {
            abstract: true,
            url: "/",
            views: {
                "rootView": {
                    controller: 'WebVellaRootBaseController',
                    controllerAs: 'rootData',
                    template: "<div ui-view='pluginView'></div>"
                    //templateProvider: function ($templateFactory) {
                    //    return $templateFactory.fromUrl('/api/root/get-ui-template/layout-0');
                    //}
                }
            },
            resolve: {
                //here you can resolve any application wide data you need. It will be available for all children states
                currentUser: ['$q', '$log', function ($q, $log) {
                    var deferred = $q.defer();
                    $log.debug("webvellaRoot>base>config>resolve> user not authenticated")
                    //Simulate not logged user
                    //deferred.reject('notAuthenticated');
                    //Simulate logged user
                    deferred.resolve('logged');
                    return deferred.promise;
                }],
                pageTitle: function () {
                    return "Webvella ERP";
                }
            },
            data: {
                //Any injectable application wide variables you may need. Can be overwritten by the child states
            }
        });


    };


    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', '$state', '$timeout'];

    /* @ngInject */
    function run($log, $rootScope, $state, $timeout) {
        $log.debug('webvellaRoot>base> BEGIN module.run');

        $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
            // Redirect user to our login page
            $log.debug("webvellaRoot>base>module.run> State change error: " + error);
            switch (error) {
                case "notAuthenticated":
                    $timeout(function () {
                        $state.go('webvella-root-login');
                    }, 0);
                    break;
                default:
                    alert(error);
                    break;
            }

        });

        $rootScope.$on('$stateNotFound', function () {
            // Redirect user to our login page
            $log.error("state not found");
        });

        $log.debug('webvellaRoot>base> END module.run');
    };





    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$timeout', '$state'];

    /* @ngInject */
    function controller($log, $timeout, $state) {
        $log.debug('webvellaRoot>base> BEGIN controller.exec');
        /* jshint validthis:true */
        var rootData = this;

        activate();
        $log.debug('webvellaRoot>base> END controller.exec');

        $timeout(function () {
            $state.go("webvella-desktop-browse");
        }, 0);

        ////////////
        function activate() {
        }
    }

})();
