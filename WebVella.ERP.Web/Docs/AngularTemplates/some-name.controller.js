/* some-name.controller.js */

/**
* @desc just a sample controller code
*/

(function () {
    'use strict';

    angular
        .module('appCore')
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
    		    resolvedSiteMeta: ResolveSiteMeta
    		}
    	});
    };


    // Run //////////////////////////////////////
    run.$inject = []; 

    function run() {};


    // Resolve Function /////////////////////////
    resolingFunction.$inject = ['dependencies]'];
    
    
    function resolingFunction(dependencies) {
        return dependencies.getData();
    }    


    // Controller ///////////////////////////////
    controller.$inject = ['dependencies']; 

    
    function controller(dependencies) {
        
        var vm = this;
        vm.title = 'controller';

        activate();

        function activate() { }
    }
    
})();
