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
           parent: 'webvella-root',
            url: '/areas', //will be added to all children states
            views: {
                "pluginView": {
                    controller: 'WebVellaAreasBaseController',
                    templateUrl: '/plugins/webvella-areas/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function (pageTitle) {
                    return "Areas | " + pageTitle;
                }
            },
            data: {
                //Custom data is inherited by the parent state 'webvella-root', but it can be overwritten if necessary. Available for all child states in this plugin
            }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$rootScope', 'webvellaDesktopTopnavFactory'];

    /* @ngInject */
    function run($rootScope, webvellaDesktopTopnavFactory) {
        var item = {
            "label": "Browse",
            "stateName": "webvella-areas-desktop",
            "stateParams": {
                "param": 1
            },
            "weight": 1
        };
        webvellaDesktopTopnavFactory.addTopnavItem(item);
        item = {
            "label": "Browse 2",
            "stateName": "webvella-areas-desktop-2",
            "stateParams": {
                "param": 1
            },
            "weight": 11
        };
        webvellaDesktopTopnavFactory.addTopnavItem(item);
    };


    // Resolve Function /////////////////////////
    resolvingFunction.$inject = [];

    /* @ngInject */
    function resolvingFunction() {
        return dependencies.getData();
    }


    // Controller ///////////////////////////////
    controller.$inject = [];

    /* @ngInject */
    function controller() {
        /* jshint validthis:true */
        var pluginData = this;

        activate();

        function activate() { }
    }

})();
