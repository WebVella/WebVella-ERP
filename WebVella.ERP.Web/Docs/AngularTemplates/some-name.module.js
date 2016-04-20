/* some-name.module.js */

/**
* @desc just a sample controller code
*/

(function () {
    'use strict';

    angular
        .module('somePlugInOrModuleName', ['ui.router'])
        .config(config)
        .run(run)
        .controller('SomeNameController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    
    function config($stateProvider) {
        $stateProvider.state('stateName', {
            url: '/',
            views: {
                "namedView": {
                    controller: 'SomeNameController',
                    templateUrl: 'module/name.view.html',
                    controllerAs: 'vm'
                }
            },
            resolve: {
                resolvedSiteMeta: resolvingFunction
            }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log'];

    
    function run($log) {
    };


    // Resolve Function /////////////////////////
    resolvingFunction.$inject = ['$q'];

    
    function resolvingFunction($q) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        siteMetaService.getUpdateSiteMeta(successCallback, errorCallback);

        return defer.promise;

    }


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope'];

    function controller($rootScope) {
        var vm = this;
        vm.title = 'controller';

        activate();

        ///////////
        function activate() { }
    }

})();
