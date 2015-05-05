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
    				controllerAs: 'topbarData'
    			}
    		},
    		resolve: {
    		    resolvedSiteMeta: ResolveSiteMeta
    		}
    	});
    };


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope','areaService'];

    /* @ngInject */
    function controller($rootScope, areaService) {
        /* jshint validthis:true */
        var topbarData = this;
        topbarData.showCollapsedNav = false;

        activate();

        function activate() {
            topbarData.areas = $rootScope.siteMeta.areas;

            topbarData.toggleCollapsedNav = function () {
                topbarData.showCollapsedNav = !topbarData.showCollapsedNav;
            }

            topbarData.navigateToArea = function (areaName) {
                areaService.navigateToArea(areaName);
            }
        }
    }
    
})();
