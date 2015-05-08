/* base.module.js */

/**
* @desc this the base module of the Desktop plugin
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin', ['ui.router'])
        .config(config)
        .controller('WebVellaAdminBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-base', {
           abstract: true,
           parent: 'webvella-root',
            url: '/admin', //will be added to all children states
            views: {
                "pluginView": {
                    controller: 'WebVellaAdminBaseController',
                    templateUrl: '/plugins/webvella-admin/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function (pageTitle) {
                    return "Admin | " + pageTitle;
                }
            },
            data: {
                //Custom data is inherited by the parent state 'webvella-root', but it can be overwritten if necessary. Available for all child states in this plugin
            }
        });
    };


    // Controller ///////////////////////////////
    controller.$inject = [];

    /* @ngInject */
    function controller() {
        /* jshint validthis:true */
        var adminData = this;

        activate();

        function activate() { }
    }

})();
