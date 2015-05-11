/* home.module.js */

/**
* @desc handles the "/" url. Manages a login form and redirects the user to "webvella-desktop-areas" state if authenticated successfully. The only module that should be accessed by not logged user
*/

(function () {
    'use strict';

    angular
        .module('webvellaRoot')  //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .run(run)
        .controller('WebVellaRootNotFoundController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-root-not-found', {
            parent: 'webvella-root',
            url: 'page-not-found',
            views: {
                "pluginView": {
                    controller: 'WebVellaRootNotFoundController',
                    templateUrl: '/plugins/webvella-root/not-found.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                
            }
        });
    };

    // Run //////////////////////////////////////
    run.$inject = ['$log'];
    /* @ngInject */
    function run($log) {
        $log.debug('webvellaRoot>home> BEGIN module.run');

        $log.debug('webvellaRoot>home> END module.run');
    };


    // Resolve Function /////////////////////////
    resolvingFunction.$inject = ['$q'];

    /* @ngInject */
    function resolvingFunction($q) {
        $log.debug('webvellaRoot>home> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();
        
        // Process
        defer.resolve("test");

        // Return
        $log.debug('webvellaRoot>home> END state.resolved');
        return defer.promise;

    }




    // Controller ///////////////////////////////
    controller.$inject = ['$state', 'currentUser', '$log', 'pageTitle', 'webvellaRootService', '$timeout'];

    /* @ngInject */
    function controller($state, currentUser, $log, pageTitle, webvellaRootService, $timeout) {
        $log.debug('webvellaRoot>home> BEGIN controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.pageTitle = "Not Found |" + pageTitle;
        webvellaRootService.setPageTitle(contentData.pageTitle);
        activate();
        $log.debug('webvellaRoot>home> END controller.exec');
        function activate() {

        }
    }

})();
