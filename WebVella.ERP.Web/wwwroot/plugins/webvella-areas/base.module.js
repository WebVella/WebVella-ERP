/* base.module.js */

/**
* @desc this the base module of the Desktop plugin
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaAreasBaseController', controller);

    //#region << Configuration >>
    config.$inject = ['$stateProvider'];
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-areas-base', {
            abstract: true,
            url: '/areas', //will be added to all children states
            views: {
                "rootView": {
                    controller: 'WebVellaAreasBaseController',
                    templateUrl: '/plugins/webvella-areas/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function () {
                    return "Webvella ERP";
                },
                resolvedSitemap: resolveSitemap,
                resolvedCurrentUser: resolveCurrentUser
            }
        });
    };
    //#endregion

    //#region << Run >>
    run.$inject = ['$log', 'webvellaAreasService', 'webvellaDesktopBrowsenavFactory', '$rootScope'];
    /* @ngInject */
    function run($log, webvellaAreasService, webvellaDesktopBrowsenavFactory, $rootScope) {
    	$log.debug('webvellaAreas>base> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

    	$log.debug('webvellaAreas>base> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };
    //#endregion


	//#region << Resolve Function >>

    resolveSitemap.$inject = ['$q', '$log', 'webvellaRootService'];
	/* @ngInject */
    function resolveSitemap($q, $log, webvellaRootService) {
    	$log.debug('webvellaDesktop>browse> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	// Initialize
    	var defer = $q.defer();

    	// Process
    	function successCallback(response) {
    		defer.resolve(response.object);
    	}

    	function errorCallback(response) {
    		defer.resolve(response.object);
    	}

    	//webvellaRootService.getSitemap(successCallback, errorCallback);
    	webvellaRootService.getSitemap(successCallback, errorCallback);
    	
    	// Return
    	$log.debug('webvellaDesktop>browse> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }

    resolveCurrentUser.$inject = ['$q', '$log', 'webvellaAdminService', 'webvellaRootService', '$state', '$stateParams'];
	/* @ngInject */
    function resolveCurrentUser($q, $log, webvellaAdminService, webvellaRootService, $state, $stateParams) {
    	$log.debug('webvellaDesktop>browse> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	// Initialize
    	var defer = $q.defer();
    	// Process
    	function successCallback(response) {
    		defer.resolve(response.object);
    	}

    	function errorCallback(response) {
    		defer.resolve(response.object);
    	}

    	var currentUser = webvellaRootService.getCurrentUser();

    	webvellaAdminService.getUserById(currentUser.userId, successCallback, errorCallback);

    	// Return
    	$log.debug('webvellaDesktop>browse> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }

	//#endregion

    //#region << Controller >>
    controller.$inject = ['$log'];
    /* @ngInject */
    function controller($log) {
    	$log.debug('webvellaAreas>base> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var pluginData = this;

        activate();
        $log.debug('webvellaAreas>base> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
        function activate() { }
    }
    //#endregion

})();
