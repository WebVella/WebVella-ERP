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
                pageTitle: function () {
                    return "Webvella ERP";
                }
            },
            data: {
                //Any injectable application wide variables you may need. Can be overwritten by the child states
            }
        });


    };


    //#region << Run >>
    run.$inject = ['$log', '$rootScope', '$state', '$timeout','$window'];
    /* @ngInject */
    function run($log, $rootScope, $state, $timeout, $window) {
    	$log.debug('webvellaRoot>base> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

        $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) { });

        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams, error) {
        	$window.scrollTo(0, 0);

        });

        $rootScope.$on('$stateNotFound', function () {
            // Redirect user to our login page
            $log.error("state not found");
        });

        $log.debug('webvellaRoot>base> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };
    //#endregion

    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$timeout', '$state', 'webvellaRootService'];

    /* @ngInject */
    function controller($log, $timeout, $state, webvellaRootService) {
    	$log.debug('webvellaRoot>base> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var rootData = this;

        var currentUser = webvellaRootService.getCurrentUser();
        if (currentUser != null) {
        	$timeout(function () {
        		$state.go("webvella-desktop-browse");
        	}, 0);
        }
        else {
        	$timeout(function () {
        		$state.go("webvella-root-login");
        	}, 0);

        }

        $log.debug('webvellaRoot>base> END controller.exec ' + moment().format('HH:mm:ss SSSS'));



    }

})();
