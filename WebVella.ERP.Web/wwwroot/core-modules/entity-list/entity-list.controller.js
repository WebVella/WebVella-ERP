/* entity-list.controller.js */

/**
* @desc this controller manages the data when presenting an Entity list to the user
*/

(function () {
    'use strict';

    angular
        .module('entityListModule', ['ui.router'])
        .config(config)
        .controller('EntityListController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider']; 
    
    /* @ngInject */
    function config($stateProvider) {
    	$stateProvider.state('entity-list', {
    	    url: '/area/:areaName/:sectionName/:entityName',
    		views: {
    		    "sidebarView": {
    		        controller: 'SidebarController',
    		        templateUrl: 'core-modules/sidebar/sidebar.view.html',
    		        controllerAs: 'sidebarData'
    		    },
    		    "topnavView": {
    		        controller: 'TopnavController',
    		        templateUrl: 'core-modules/topnav/topnav.view.html',
    		        controllerAs: 'topnavData'
    		    },
    		    "titleView": {
    		        controller: 'PageTitleController',
    		        templateUrl: 'core-modules/page-title/page-title.view.html',
    		        controllerAs: 'pageTitleData'
    		    },
    			"contentView": {
    			    controller: 'EntityListController',
    				templateUrl: 'core-modules/entity-list/entity-list.view.html',
                    controllerAs: 'entityListData'
    			}
    		},
    		resolve: {}
    	});
    };


    // Controller ///////////////////////////////
    controller.$inject = [];

    /* @ngInject */
    function controller() {
        /* jshint validthis:true */
        var entityListData = this;

        activate();

        function activate() {


        }
    }
    
})();
