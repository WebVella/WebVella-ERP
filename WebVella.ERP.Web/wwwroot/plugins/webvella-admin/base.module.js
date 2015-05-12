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
            url: '/admin', //will be added to all children states
            views: {
                "rootView": {
                    controller: 'WebVellaAdminBaseController',
                    templateUrl: '/plugins/webvella-admin/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            params: {
                "redirect": 'false'
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function () {
                    return "Webvella ERP";
                }
            },
            data: {
                //Custom data is inherited by the parent state 'webvella-root', but it can be overwritten if necessary. Available for all child states in this plugin
            }
        });
    };

    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopBrowsenavFactory) {
        $log.debug('webvellaAdmin>base> BEGIN module.run');
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
    controller.$inject = ['$log', '$scope','$state', '$rootScope','$stateParams', 'webvellaRootService', 'webvellaAdminSidebarFactory', '$timeout'];

    /* @ngInject */
    function controller($log, $scope,$state, $rootScope,$stateParams, webvellaRootService, webvellaAdminSidebarFactory, $timeout) {
        $log.debug('webvellaAdmin>base> START controller.exec');
        /* jshint validthis:true */
        var adminData = this;
        adminData.sidebar = [];

        //Making topnav pluggable
        ////1. CONSTRUCTOR initialize the factory
        webvellaAdminSidebarFactory.initSidebar();
        ////2. READY hook listener
        var readySidebarDestructor = $rootScope.$on("webvellaAdmin-sidebar-ready", function (event, data) {
            //All actions you want to be done after the "Ready" hook is cast
        })
        ////3. UPDATED hook listener
        var updateSidebarDestructor = $rootScope.$on("webvellaAdmin-sidebar-updated", function (event, data) {
            adminData.sidebar = data;
            activate();
        });
        ////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
        $scope.$on("$destroy", function () {
            readySidebarDestructor();
            readySidebarDestructor();
        });
        ////5. Bootstrap the pluggable element and cast the Ready hook
        //Push the Regular menu items
        var item = {
            "label": "Entities",
            "stateName": "webvella-admin-entities",
            "stateParams": {},
            "parentName": "",
            "nodes": [],
            "weight": 1.0,
            "color": "red",
            "iconName": "cog"
        };
        adminData.sidebar.push(item);
        item = {
            "label": "Users",
            "stateName": "webvella-admin-users",
            "stateParams": {},
            "parentName": "",
            "nodes": [],
            "weight": 2.0,
            "color": "red",
            "iconName": "cog"
        };
        adminData.sidebar.push(item);
        $rootScope.$emit("webvellaAdmin-sidebar-ready");
        $log.debug('rootScope>events> "webvellaAdmin-sidebar-ready" emitted');

        activate();
        $log.debug('webvellaAdmin>base> END controller.exec');
        function activate() {
            // Change the body color to all child states to red
            webvellaRootService.setBodyColorClass("red");
            
        }
    }

})();
