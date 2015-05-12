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
            abstract: true,
            //parent: 'webvella-root',
            url: '', //will be added to all children states
            views: {
                "rootView": {
                    controller: 'WebVellaDesktopBaseController',
                    templateUrl: '/plugins/webvella-desktop/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function () {
                    return "Webvella ERP";
                },
                resolvedSiteMeta: resolveSiteMeta
            },
            data: { }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopTopnavFactory', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopTopnavFactory, webvellaDesktopBrowsenavFactory) {
        $log.debug('webvellaDesktop>base> BEGIN module.run');

        $log.debug('webvellaDesktop>base> END module.run');
    };

    // Resolve Function /////////////////////////
    resolveSiteMeta.$inject = ['$q', '$log', 'webvellaRootService'];

    /* @ngInject */
    function resolveSiteMeta($q, $log, webvellaRootService) {
        $log.debug('webvellaRoot>base> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaRootService.getSiteMeta(successCallback, errorCallback);

        // Return
        $log.debug('webvellaRoot>base> END state.resolved');
        return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$scope','$log', '$rootScope', '$state', '$stateParams', 'webvellaDesktopTopnavFactory', '$timeout'];

    /* @ngInject */
    function controller($scope,$log, $rootScope, $state, $stateParams, webvellaDesktopTopnavFactory, $timeout) {
        $log.debug('webvellaDesktop>base> BEGIN controller.exec');

        /* jshint validthis:true */
        var pluginData = this;
        pluginData.topnav = [];

        //Making topnav pluggable
        ////1. CONSTRUCTOR initialize the factory
        webvellaDesktopTopnavFactory.initTopnav();
        ////2. READY hook listener
        var readyTopnavDestructor = $rootScope.$on("webvellaDesktop-topnav-ready", function (event, data) {
            //All actions you want to be done after the "Ready" hook is cast
        })
        ////3. UPDATED hook listener
        var updateTopnavDestructor = $rootScope.$on("webvellaDesktop-topnav-updated", function (event, data) {
            pluginData.topnav = data;
            activate();
        });
        ////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
        $scope.$on("$destroy", function () {
            readyTopnavDestructor();
            updateTopnavDestructor();
        });
        ////5. Bootstrap the pluggable element and cast the Ready hook
        //Push the Browse area menu
        var item = {
            "label": "Browse",
            "stateName": "webvella-desktop-browse",
            "stateParams": {},
            "parentName": "",
            "nodes": [],
            "weight": 1.0
        };
        webvellaDesktopTopnavFactory.addItem(item);
        $rootScope.$emit("webvellaDesktop-topnav-ready");
        $log.debug('rootScope>events> "webvellaDesktop-topnav-ready" emitted');

        $log.debug('webvellaDesktop>base> END controller.exec');

        function activate() {
            if (pluginData.topnav.length > 0) {
                $timeout(function () {
                    $state.go(pluginData.topnav[0].stateName, pluginData.topnav[0].stateParams)
                }, 0);
               
            }
        }
    }

})();
