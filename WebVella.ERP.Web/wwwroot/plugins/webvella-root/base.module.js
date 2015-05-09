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
            url: "",
            views: {
                "rootView": {
                    controller: 'WebVellaRootBaseController',
                    controllerAs: 'rootData',
                    template:"<div ui-view='pluginView'></div>"
                    //templateProvider: function ($templateFactory) {
                    //    return $templateFactory.fromUrl('/api/root/get-ui-template/layout-0');
                    //}
                }
            },
            resolve: {
                //here you can resolve any application wide data you need. It will be available for all children states
                currentUser: function () {
                    //TODO - Will be substituted with a service call after implementation
                    return { email: 'email@domain.com', roles:["administrator"] };
                },
                pageTitle: function () {
                    return "Webvella ERP";
                },
                resolvedSiteMeta:resolveSiteMeta
            },
            data: {
                //Any injectable application wide variables you may need. Can be overwritten by the child states
            }
        });

  
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log'];

    /* @ngInject */
    function run($log) {
        $log.debug('webvellaRoot>base> BEGIN module.run');

        $log.debug('webvellaRoot>base> END module.run');
    };


    // Resolve Function /////////////////////////
    resolveSiteMeta.$inject = ['$q','$log', 'webvellaRootSiteMetaService'];

    /* @ngInject */
    function resolveSiteMeta($q,$log, webvellaRootSiteMetaService) {
        $log.debug('webvellaRoot>base> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallBack(response) {
            defer.resolve(response.object);
        }

        function errorCallBack(response) {
            defer.resolve(response.object);
        }

        webvellaRootSiteMetaService.getSiteMeta(successCallBack, errorCallBack);

        // Return
        $log.debug('webvellaRoot>base> END state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$log'];

    /* @ngInject */
    function controller($log) {
        $log.debug('webvellaRoot>base> BEGIN controller.exec');
        /* jshint validthis:true */
        var rootData = this;

        activate();
        $log.debug('webvellaRoot>base> END controller.exec');

        ////////////
        function activate() {
        }
    }

})();
