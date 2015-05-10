/* base.module.js */

/**
* @desc this the base module of the Desktop plugin
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaAdminBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-base', {
            abstract: true,
            parent: 'webvella-root',
            url: '', //will be added to all children states
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

    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory', 'webvellaRootService'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopBrowsenavFactory, webvellaRootService) {
        $log.debug('webvellaAdmin>base> BEGIN module.run');
        // Push the Admin to the Desktop Browse navigation
        $rootScope.$on('webvellaDesktop-browsenav-ready', function (event) {
            var item = {
                "label": "Administration",
                "stateName": "webvella-admin-entities",
                "stateParams": {},
                "parentName": "",
                "nodes": [],
                "weight": 100.0,
                "color": "red",
                "iconName": "cog"
            };

            webvellaDesktopBrowsenavFactory.addItem(item);
        });

        $log.debug('webvellaAdmin>base> END module.run');
    };

    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', 'webvellaRootService'];

    /* @ngInject */
    function controller($log, $rootScope, webvellaRootService) {
        $log.debug('webvellaAdmin>base> START controller.exec');
        /* jshint validthis:true */
        var adminData = this;

        activate();
        $log.debug('webvellaAdmin>base> END controller.exec');
        function activate() {
            // Change the body color to all child states to red
            webvellaRootService.setBodyColorClass("red");
        }
    }

})();
