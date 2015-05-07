/* home.module.js */

/**
* @desc handles the "/" url. Manages a login form and redirects the user to "webvella-desktop-areas" state if authenticated successfully. The only module that should be accessed by not logged user
*/

(function () {
    'use strict';

    angular
        .module('webvella-root-home', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaRootHomeController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-root-home', {
            parent: 'webvella-root',
            url: '/',
            views: {
                "pluginView": {
                    controller: 'WebVellaRootHomeController',
                    templateUrl: '/plugins/webvella-root/home.view.html',
                    controllerAs: 'homeData'
                }
            },
            resolve: {
                
            }
        });
    };

    // Run //////////////////////////////////////
    run.$inject = [];
    /* @ngInject */
    function run() { };

    // Controller ///////////////////////////////
    controller.$inject = ['$state','currentUser'];

    /* @ngInject */
    function controller($state, currentUser) {
        /* jshint validthis:true */
        var homeData = this;
        

        activate();

        function activate() {

            if (currentUser != null) {
                $state.go("webvella-desktop-areas")
            }
        }
    }

})();
