/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminPluginsController', controller);
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

    config.$inject = ['$stateProvider'];

    
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-plugins', {
            parent: 'webvella-admin-base',
            url: '/plugins', 
            views: {
                "topnavView": {
                    controller: 'WebVellaAdminTopnavController',
                    templateUrl: '/plugins/webvella-admin/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "sidebarView": {
                    controller: 'WebVellaAdminSidebarController',
                    templateUrl: '/plugins/webvella-admin/sidebar.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                	controller: 'WebVellaAdminPluginsController',
                	templateUrl: '/plugins/webvella-admin/plugins.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
                resolvedPluginsList: resolvePluginsList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

    // Resolve Roles list /////////////////////////
    resolvePluginsList.$inject = ['$q', '$log', 'webvellaCoreService'];
    
    function resolvePluginsList($q, $log, webvellaCoreService) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaCoreService.getPluginsList(successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', 
							'resolvedPluginsList', '$uibModal', 'webvellaCoreService'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedPluginsList, $uibModal, webvellaCoreService) {

        
        var ngCtrl = this;
        ngCtrl.search = {};

        //#region << Update page title >>
        ngCtrl.pageTitle = "Plugins List | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		},0);

    	//#endregion

        ngCtrl.plugins = fastCopy(resolvedPluginsList);

    }
    //#endregion

})();
