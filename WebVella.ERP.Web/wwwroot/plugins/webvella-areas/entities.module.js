/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreasEntitiesController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-areas-entities', {
            parent: 'webvella-areas-base',
            url: '/entities', //  /desktop/areas after the parent state is prepended
            views: {
                "contentView": {
                    controller: 'WebVellaAreasEntitiesController',
                    templateUrl: '/plugins/webvella-areas/entities.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {

            },
            data: {

            }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$rootScope', 'webvellaDesktopTopnavFactory'];

    /* @ngInject */
    function run($rootScope, webvellaDesktopTopnavFactory) {
        console.log("run executed")
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope', '$state', 'pageTitle'];

    /* @ngInject */
    function controller($rootScope, $state, pageTitle) {
        /* jshint validthis:true */
        var contentData = this;
        contentData.pageTitle = "Areas | " + pageTitle;


        contentData.goHome = function () {
            $state.go("webvella-root-home");
        }

        activate();

        function activate() { }
    }

})();
