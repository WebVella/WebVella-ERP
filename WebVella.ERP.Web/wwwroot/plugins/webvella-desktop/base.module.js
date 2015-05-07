/* base.module.js */

/**
* @desc this the base module of the Desktop plugin
*/

(function () {
    'use strict';

    angular
        .module('webvella-desktop-base', ['ui.router'])
        .config(config)
        .controller('WebVellaDesktopBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-desktop-base', {
            abstract: true,
            parent: 'webvella-root',
            url: '/desktop', //will be added to all children states
            views: {
                "pluginView": {
                    controller: 'WebVellaDesktopBaseController',
                    templateUrl: '/plugins/webvella-desktop/base.view.html',
                    controllerAs: 'desktopData'
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


    // Controller ///////////////////////////////
    controller.$inject = ['$state', '$stateParams'];

    /* @ngInject */
    function controller($state, $stateParams) {
        /* jshint validthis:true */
        var desktopData = this;

        activate();

        function activate() { }
    }

})();
