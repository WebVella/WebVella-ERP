/* base.module.js */

/**
* @desc this the base module of the Desktop plugin. Its only tasks is to check the topNavFactory and redirect to the first menu item state
*/

(function () {
    'use strict';

    angular
        .module('webvellaDesktop', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaDesktopBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-desktop-base', {
            //abstract: true,
            parent: 'webvella-root',
            url: '/desktop', //will be added to all children states
            views: {
                "pluginView": {
                    controller: 'WebVellaDesktopBaseController',
                    templateUrl: '/plugins/webvella-desktop/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function (pageTitle) {
                    return "Desktop | " + pageTitle;
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

    };



    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope', '$state', '$stateParams', 'webvellaDesktopTopnavFactory'];

    /* @ngInject */
    function controller($rootScope, $state, $stateParams, webvellaDesktopTopnavFactory) {
        /* jshint validthis:true */
        var pluginData = this;

        activate();

        function activate() {
            //Get the topnav items and redirect to the first one
            pluginData.topnav = webvellaDesktopTopnavFactory.getTopnav();
            if (pluginData.topnav.length > 0) {
                $state.go(pluginData.topnav[0].stateName, pluginData.topnav[0].stateParams)
            }
        }
    }

})();
