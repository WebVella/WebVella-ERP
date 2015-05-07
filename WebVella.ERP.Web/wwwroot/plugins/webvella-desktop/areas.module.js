/* home.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvella-desktop-areas', ['ui.router'])
        .config(config)
        .controller('WebVellaDesktopAreasController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-desktop-areas', {
            parent: 'webvella-desktop-base',
            url: '/areas', //  /desktop/areas after the parent state is prepended
            views: {
                "contentView": {
                    controller: 'WebVellaDesktopAreasController',
                    templateUrl: '/plugins/webvella-desktop/areas.view.html',
                    controllerAs: 'desktopAreasData'
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
        var desktopAreasData = this;
        $rootScope.pageTitle = "Areas | " + pageTitle;


        desktopAreasData.goHome = function () {
            $state.go("webvella-root-home");
        }

        activate();

        function activate() { }
    }

})();
