/* home.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntitiesController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entities', {
            parent: 'webvella-admin-base',
            url: '/entities', //  /desktop/areas after the parent state is prepended
            views: {
                "topnavView": {
                    controller: 'WebVellaAdminEntitiesTopnavController',
                    templateUrl: '/plugins/webvella-admin//topnav/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "contentView": {
                    controller: 'WebVellaAdminEntitiesController',
                    templateUrl: '/plugins/webvella-admin/entities.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                
            },
            data: {
                
            }
        });
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope','$state','pageTitle'];

    /* @ngInject */
    function controller($rootScope, $state, pageTitle) {
        /* jshint validthis:true */
        var contentData = this;
        contentData.pageTitle = "Entities | " + pageTitle;

        activate();

        function activate() { }
    }

})();
