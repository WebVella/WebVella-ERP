/* home.controller.js */

/**
* @desc this controller manages the data in the home page contentView section
*/

(function () {
    'use strict';

    angular
        .module('homeModule', ['ui.router'])
        .config(config)
        .controller('HomeController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];
    
    /* @ngInject */
    function config($stateProvider) {
    	$stateProvider.state('home', {
    		url: '/',
    		views: {
    			"topbarView": {
    				controller: 'HomeController',
    				templateUrl: 'core-modules/home/home.view.html',
                    controllerAs: 'contentData'
    			}
    		},
    		resolve: {}
    	});
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope'];

    /* @ngInject */
    function controller($rootScope) {
        /* jshint validthis:true */
        var contentData = this;

        activate();

        function activate() {
            contentData.items = [];
            for (var i = 0; i < 5; i++) {
                var item = {};
                contentData.items.push(item);
            }
        }
    }
    
})();
