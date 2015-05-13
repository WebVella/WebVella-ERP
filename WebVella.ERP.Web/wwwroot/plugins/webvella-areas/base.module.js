/* base.module.js */

/**
* @desc this the base module of the Desktop plugin
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaAreasBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-areas-base', {
            abstract: true,
            url: '/areas', //will be added to all children states
            views: {
                "rootView": {
                    controller: 'WebVellaAreasBaseController',
                    templateUrl: '/plugins/webvella-areas/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function () {
                    return "Webvella ERP";
                }
            },
            data: {
                //Custom data is inherited by the parent state 'webvella-root', but it can be overwritten if necessary. Available for all child states in this plugin
            }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log'];

    /* @ngInject */
    function run($log) {
        $log.debug('webvellaAreas>base> BEGIN module.run');

        $log.debug('webvellaAreas>base> END module.run');
    };


    // Resolve Function /////////////////////////
    resolveSiteMeta.$inject = ['$q', '$log', 'webvellaRootService'];

    /* @ngInject */
    function resolveSiteMeta($q, $log, webvellaRootService) {
        $log.debug('webvellaRoot>base> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaRootService.getSiteMeta(successCallback, errorCallback);

        // Return
        $log.debug('webvellaRoot>base> END state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$log'];

    /* @ngInject */
    function controller($log) {
        $log.debug('webvellaAreas>base> BEGIN controller.exec');
        /* jshint validthis:true */
        var pluginData = this;

        activate();
        $log.debug('webvellaAreas>base> END controller.exec');
        function activate() { }
    }

})();
