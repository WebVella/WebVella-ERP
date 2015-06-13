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
        $log.debug('webvellaAreas>view> BEGIN module.run');

        $log.debug('webvellaAreas>view> END module.run');
    };

    // Resolve Function /////////////////////////
    resolveCurrentArea.$inject = ['$q', '$log', 'webvellaAreasService','$stateParams'];

    /* @ngInject */
    function resolveCurrentArea($q, $log, webvellaAreasService, $stateParams) {
        $log.debug('webvellaAreas>view> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaAreasService.getAreaByName($stateParams.areaName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAreas>view> END state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'resolvedCurrentArea', '$timeout'];

    /* @ngInject */
    function controller($log, $rootScope, $state, resolvedCurrentArea, $timeout) {
        $log.debug('webvellaAreas>view> BEGIN controller.exec');
        /* jshint validthis:true */
        var contentData = this;

        //Redirect to the first entity of the area
        var requestedArea = resolvedCurrentArea;
        var firstEntityName = null;
        var firstEntitySectionName = null;

        if (requestedArea == null) {
            $log.debug('webvellaAreas>view> the resolved object resolvedCurrentArea is null');
            alert("No area with this name is found");
            return;
        }

        //Navigate to the first entity - the site meta Object is already sorted in the service
        for (var i = 0; i < requestedArea.sections.length; i++) {
            if (requestedArea.sections[i].entities.length > 0) {
                firstEntityName = requestedArea.sections[i].entities[0].name;
                firstEntitySectionName = requestedArea.sections[i].name;
                break;
            }
        }
        if (firstEntityName != null) {
            $timeout(function () {
                $state.go('webvella-entity-records', { areaName: requestedArea.name, entityName: firstEntityName });
            }, 0);
            
        }
        else {
            //If no entities related raise error and cancel navigation
            alert("This area has no entities attached");
        }

        activate();
        $log.debug('webvellaAreas>view> BEGIN controller.exec');
        function activate() { }
    }

})();
