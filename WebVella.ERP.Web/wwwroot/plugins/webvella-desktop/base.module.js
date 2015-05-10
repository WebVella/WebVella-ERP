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
            data: { }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopTopnavFactory', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopTopnavFactory, webvellaDesktopBrowsenavFactory) {
        $log.debug('webvellaDesktop>base> BEGIN module.run');
        //Initialize the module storage
        $rootScope.webvellaDesktop = {};
        $rootScope.webvellaDesktop.browsenav = [];



        //Initialize the pluggable object made with factories, always when state is changed. (it fixes the duplication problem with browser back and forward buttons)
        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            webvellaDesktopTopnavFactory.initTopnav();
        })



        $log.debug('webvellaDesktop>base> END module.run');
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'webvellaDesktopTopnavFactory', '$timeout'];

    /* @ngInject */
    function controller($log, $rootScope, $state, $stateParams, webvellaDesktopTopnavFactory, $timeout) {
        $log.debug('webvellaDesktop>base> BEGIN controller.exec');

        /* jshint validthis:true */
        var pluginData = this;
        //Get the topnav items and redirect to the first one
        pluginData.topnav = [];
        

        //Maintain the topnav if there are new injections
        $rootScope.$on('webvellaDesktop-topnav-updated', function (event,newValue) {
            pluginData.topnav = newValue;
            activate();
        });
        //Emit hook for adding topnav menu items
        $rootScope.$emit('webvellaDesktop-topnav-ready');
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
