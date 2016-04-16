/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreasViewController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-areas-view', {
            abstract:true,
            parent: 'webvella-areas-base',
            url: '/view/:areaName', //  /areas/view/:name after the parent state is prepended
            views: {
                "contentView": {
                    controller: 'WebVellaAreasViewController',
                    template: '<p>Area view screen, should redirect to the list of the first entity</p>',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedCurrentArea: resolveCurrentArea
            },
            data: {

            }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log'];

    /* @ngInject */
    function run($log) {
    	$log.debug('webvellaAreas>view> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

    	$log.debug('webvellaAreas>view> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };

    // Resolve Function /////////////////////////
    resolveCurrentArea.$inject = ['$q', '$log', 'webvellaAreasService','$stateParams'];

    /* @ngInject */
    function resolveCurrentArea($q, $log, webvellaAreasService, $stateParams) {
    	$log.debug('webvellaAreas>view> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaAreasService.getAreaByName($stateParams.areaName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAreas>view> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'resolvedCurrentArea', '$timeout'];

    /* @ngInject */
    function controller($log, $rootScope, $state, resolvedCurrentArea, $timeout) {
    	$log.debug('webvellaAreas>view> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var contentData = this;
        activate();
        $log.debug('webvellaAreas>view> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        function activate() { }
    }

})();
