/* some-name.module.js */

/**
* @desc just a sample controller code
*/

(function () {
    'use strict';

    angular
        .module('someNameModule', ['ui.router'])
        .config(config)
        .run(run)
        .controller('SomeNameController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider']; 
    
    /* @ngInject */
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
    		    resolvedSiteMeta: ResolveSiteMeta
    		}
    	});
    };


    // Run //////////////////////////////////////
    run.$inject = []; 

    /* @ngInject */
    function run() {};


    // Resolve Function /////////////////////////
    resolingFunction.$inject = ['dependencies]'];
    
    /* @ngInject */
    function resolingFunction(dependencies) {
        return dependencies.getData();
    }    


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope'];

    /* @ngInject */
    function controller($rootScope) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'controller';

        activate();

        function activate() { }
    }
    
})();
