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
    	$log.debug('webvellaAreas>base>resolveSitemap> BEGIN sitemap resolved ' + moment().format('HH:mm:ss SSSS'));
    	// Initialize
    	var defer = $q.defer();

    	// Process
    	function successCallback(response) {
    		defer.resolve(response.object);
    	}

    	function errorCallback(response) {
    		defer.reject(response.message);
    	}
    	webvellaRootService.getSitemap(successCallback, errorCallback);
    	
    	// Return
    	$log.debug('webvellaAreas>base>resolveSitemap> END sitemap resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }

    resolveCurrentUser.$inject = ['$q', '$log', 'webvellaAdminService', 'webvellaRootService', '$state', '$stateParams'];
	/* @ngInject */
    function resolveCurrentUser($q, $log, webvellaAdminService, webvellaRootService, $state, $stateParams) {
    	$log.debug('webvellaAreas>base>resolveCurrentUser> BEGIN user resolved ' + moment().format('HH:mm:ss SSSS'));
    	// Initialize
    	var defer = $q.defer();
    	// Process
    	var currentUser = webvellaRootService.getCurrentUser();
    	if (currentUser != null) {
    		defer.resolve(currentUser);
    	}
    	else {
    		defer.reject(null);
    	}

    	// Return
    	$log.debug('webvellaAreas>base>resolveCurrentUser> END user resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }

	//#endregion

    //#region << Controller >>
    controller.$inject = ['$log', '$stateParams', 'webvellaRootService', 'resolvedCurrentUser', 'resolvedSitemap', 'ngToast', '$window'];
    /* @ngInject */
    function controller($log, $stateParams, webvellaRootService, resolvedCurrentUser, resolvedSitemap, ngToast, $window) {
    	$log.debug('webvellaAreas>base> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
    	var pluginData = this;
        $log.debug('webvellaAreas>base> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }
    //#endregion

})();
