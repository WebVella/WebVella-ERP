/* application.controller.js */

/**
* @desc the main application controller
*/

(function () {
    'use strict';

    angular
        .module('wvApp')
        .config(config)
        .controller('ApplicationController', controller);


    // Configuration ///////////////////////////////////
    config.$inject = ['$httpProvider'];

    /* @ngInject */
    function config($httpProvider) {
        //$http.defaults.headers.delete = {};
        //$http.defaults.headers.delete['auth-token'] = 'C3PO R2D2';

        $httpProvider.interceptors.push(function ($q, $window) {
            return {
                'responseError': function (errorResponse) {
                    switch (errorResponse.status) {
                        case 403:
                            $window.location = '#/login';
                            //impossible to use state in the config, when i try to inject i get "Unknown provider: $state"
                            //cannot inject timeout too
                            //$state.go('webvella-root-login');
                            break;
                        case 500:
                            //$window.location = './500.html';
                            break;
                    }
                    return $q.reject(errorResponse);
                }
            }
        });
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope', '$log', '$cookies', '$localStorage', '$timeout', '$state'];

    /* @ngInject */
    function controller($rootScope, $log, $cookies, $localStorage, $timeout, $state) {
        $log.debug('vwApp> BEGIN controller.exec');
        /* jshint validthis:true */
        var appData = this;
        //Set page title
        appData.pageTitle = 'WebVella ERP';
        $rootScope.$on("application-pageTitle-update", function (event, newValue) {
            appData.pageTitle = newValue;
        });
        //Set the body color
        appData.bodyColor = "no-color";
        $rootScope.$on("application-body-color-update", function (event, color) {
            appData.bodyColor = color;
        });
        //Side menu toggle
        appData.$storage = $localStorage;
        if (!appData.$storage.isMiniSidebar) {
            appData.$storage.isMiniSidebar = false;
        }
        //appData.isMiniSidebar = false;
        //$rootScope.isMiniSidebar = false;
        //if ($cookies.get("isMiniSidebar") == "true") {
        //	appData.isMiniSidebar = true;
        //	$rootScope.isMiniSidebar = true;
        //}

        //$rootScope.$on("application-sidebar-mini-toggle", function (event) {
        //	appData.isMiniSidebar = !appData.isMiniSidebar;
        //	$rootScope.isMiniSidebar = appData.isMiniSidebar;
        //	$cookies.put("isMiniSidebar", appData.isMiniSidebar);
        //});
        //Side menu visibility
        appData.sideMenuIsVisible = true;
        $rootScope.$on("application-body-sidebar-menu-isVisible-update", function (event, isVisible) {
            appData.sideMenuIsVisible = isVisible;
        });

        //Redirect State (usefull when you need to redirect from resolve)
        $rootScope.$on("state-change-needed", function (event, stateName, stateParams) {
            $timeout(function () {
                $state.go(stateName, stateParams, { reload: true });
            }, 0);
        });

        activate();
        $log.debug('wvApp> END controller.exec');
        function activate() {


        }
    }

})();
