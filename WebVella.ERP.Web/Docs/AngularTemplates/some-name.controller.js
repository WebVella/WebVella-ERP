/* some-name.controller.js */

/**
* @desc just a sample controller code
*/

(function () {
    'use strict';

    angular
        .module('app')
        .config(config)
        .run(run)
        .controller('SomeNameController', controller);

    // Config ///////////////////////////////////
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
    			resolvedData:resolingFunction
    		}
    	});
    };


    // Run //////////////////////////////////////
    run.$inject = []; 

    function run() {};


    // Resolve Function /////////////////////////
    resolingFunction.$inject = ['dependencies]'];
    
    /* @ngInject */
    function resolingFunction(dependencies) {
        return dependencies.getData();
    }    


    // Controller ///////////////////////////////
    controller.$inject = ['dependencies']; 

    /* @ngInject */
    function controller(dependencies) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'controller';

        activate();

        function activate() { }
    }
    
})();
