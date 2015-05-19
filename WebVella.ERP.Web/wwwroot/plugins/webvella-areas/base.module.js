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

    //#region << Configuration >>
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
                },
                resolvedCurrentArea: resolveCurrentArea
            }
        });
    };
    //#endregion

    //#region << Run >>
    run.$inject = ['$log', 'webvellaAreasService', 'webvellaDesktopBrowsenavFactory', '$rootScope'];
    /* @ngInject */
    function run($log, webvellaAreasService, webvellaDesktopBrowsenavFactory, $rootScope) {
        $log.debug('webvellaAreas>base> BEGIN module.run');

        $log.debug('webvellaAreas>base> END module.run');
    };
    //#endregion


    // Resolve Function /////////////////////////
    resolveCurrentArea.$inject = ['$q', '$log', 'webvellaAreasService', '$stateParams'];

    /* @ngInject */
    function resolveCurrentArea($q, $log, webvellaAreasService, $stateParams) {
        $log.debug('webvellaAreas>entities> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaAreasService.getAreaByName("area", successCallback, errorCallback);

        // Return
        $log.debug('webvellaAreas>entities> END state.resolved');
        return defer.promise;
    }



    //#region << Controller >>
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
    //#endregion

})();
