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
        .controller('WebVellaRootErrorController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-root-error', {
            url: '/errors/:code?url',
            views: {
                "rootView": {
                    controller: 'WebVellaRootErrorController',
                    templateUrl: '/plugins/webvella-root/error.view.html',
                    controllerAs: 'errorData'
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
    	$log.debug('webvellaRoot>home> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

    	$log.debug('webvellaRoot>home> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };


    // Resolve Function /////////////////////////
    resolvingFunction.$inject = ['$q'];

    /* @ngInject */
    function resolvingFunction($q) {
    	$log.debug('webvellaRoot>home> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
        // Initialize
        var defer = $q.defer();

        // Process
        defer.resolve("test");

        // Return
        $log.debug('webvellaRoot>home> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;

    }




    // Controller ///////////////////////////////
    controller.$inject = ['$state', '$log', 'webvellaRootService', '$stateParams', '$timeout'];

    /* @ngInject */
    function controller($state, $log, webvellaRootService, $stateParams, $timeout) {
    	$log.debug('webvellaRoot>home> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var errorData = this;

        errorData.code = $stateParams.code;
        errorData.url = $stateParams.url;

        switch (errorData.code) {
            case '401':
                errorData.title = "Unauthorized";
                errorData.text = "The requested page or resource require you to authenticate.";
                break;
            case '403':
                errorData.title = "Forbidden";
                errorData.text = "The access to requested page or resource is forbidden.";
                break;
            case '404':

                errorData.title = "Not found";
                errorData.text = "The requested page or resource is not found.";
                break;
            case '500':
                errorData.title = "Internal Server Error";
                errorData.text = "An unexpected error occurred. The system administrators will be notified about it.";
                //todo we may ask for more details about it in the future
                break;
            default:
                $timeout(function () {
                    $state.go('webvella-root-error', { 'code': '404', 'url': errorData.url });
                }, 0);
        }


        webvellaRootService.setPageTitle("Error | " + errorData.title);



        activate();
        $log.debug('webvellaRoot>home> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
        function activate() {
        }
    }

})();
