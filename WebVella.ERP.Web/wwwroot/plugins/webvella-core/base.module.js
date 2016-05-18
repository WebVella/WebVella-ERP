/* base.module.js */

/**
* @desc base / root module of the application. Core task is to bootstrap and initialize the application and get the layouts. 
* To achieve this purpose it manages the abstract 'webvella-core-layout-*' state which resolves and pass to all its children 
* the core data loaded by the database.
* It also ensures that not logged users have access only to the login form at "/"
* Data storage: $rootscope.plugins.webvellaCore
*/

(function () {
    'use strict';

    angular
        .module('webvellaCore', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaCoreBaseController', controller);

    //#region << Configuration /////////////////////////////////// >>
    config.$inject = ['$stateProvider'];
    function config($stateProvider) {
        $stateProvider.state('webvella-core', {
            abstract: true,
            url: "/",
            views: {
                "rootView": {
                    controller: 'WebVellaCoreBaseController',
                    controllerAs: 'rootData',
                    template: "<div ui-view='pluginView'></div>"
                }
            },
            resolve: {
                pageTitle: function () {
                    return GlobalCompanyName;
                }
            },
            data: {}
        });
    };
	//#endregion

    //#region << Run >>
    run.$inject = ['$log', '$rootScope', '$window'];
    function run($log, $rootScope, $window) {
        //$rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
		//	alert("error while loading the new page: " + error)
		//});
        //$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams, error) {
        //	$window.scrollTo(0, 0);
        //});
        //$rootScope.$on('$stateNotFound', function () {
        //    alert("requested page not found")
        //});
    };
    //#endregion

    //#region << Controller /////////////////////////////// >>
    controller.$inject = ['$timeout', '$state', 'webvellaCoreService'];
    function controller($timeout, $state, webvellaCoreService) {
        var rootData = this;
        var currentUser = webvellaCoreService.getCurrentUser();
        if (currentUser != null) {
        	$timeout(function () {
        		$state.go("webvella-desktop-browse");
        	}, 0);
        }
        else {
        	$timeout(function () {
        		$state.go("webvella-core-login");
        	}, 0);

        }
    }
	//#endregion

})();
