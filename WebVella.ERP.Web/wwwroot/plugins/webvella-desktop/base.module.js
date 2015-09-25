/* base.module.js */

/**
* @desc this the base module of the Desktop plugin. Its only tasks is to check the topNavFactory and redirect to the first menu item state
*/

(function () {
    'use strict';

    angular
        .module('webvellaDesktop', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaDesktopBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-desktop-base', {
            abstract: true,
            //parent: 'webvella-root',
            url: '', //will be added to all children states
            views: {
                "rootView": {
                    controller: 'WebVellaDesktopBaseController',
                    templateUrl: '/plugins/webvella-desktop/base.view.html',
                    controllerAs: 'pluginData'
                }
            },
            resolve: {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                pageTitle: function () {
                    return "Webvella ERP";
                },
                resolvedCurrentUser: resolveCurrentUser
            },
            data: { }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopTopnavFactory', 'webvellaDesktopBrowsenavFactory'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopTopnavFactory, webvellaDesktopBrowsenavFactory) {
    	$log.debug('webvellaDesktop>base> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));

    	$log.debug('webvellaDesktop>base> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };

	// Resolve /////////////////////////////////
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




    // Controller ///////////////////////////////
    controller.$inject = ['$scope','$log', '$rootScope', '$state', '$stateParams', 'webvellaDesktopTopnavFactory', '$timeout','webvellaAdminService','resolvedCurrentUser'];

    /* @ngInject */
    function controller($scope,$log, $rootScope, $state, $stateParams, webvellaDesktopTopnavFactory, $timeout,webvellaAdminService,resolvedCurrentUser) {
    	$log.debug('webvellaDesktop>base> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));

        /* jshint validthis:true */
        var pluginData = this;
        pluginData.topnav = [];
        pluginData.user = angular.copy(resolvedCurrentUser);
        //Making topnav pluggable
        ////1. CONSTRUCTOR initialize the factory
        webvellaDesktopTopnavFactory.initTopnav();
        ////2. READY hook listener
        var readyTopnavDestructor = $rootScope.$on("webvellaDesktop-topnav-ready", function (event, data) {
            //All actions you want to be done after the "Ready" hook is cast
        })
        ////3. UPDATED hook listener
        var updateTopnavDestructor = $rootScope.$on("webvellaDesktop-topnav-updated", function (event, data) {
            pluginData.topnav = data;
            activate();
        });
        ////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
        $scope.$on("$destroy", function () {
            readyTopnavDestructor();
            updateTopnavDestructor();
        });
        ////5. Bootstrap the pluggable element and cast the Ready hook
        //Push the Browse area menu
        var item = {
            "label": "Browse",
            "stateName": "webvella-desktop-browse",
            "stateParams": {},
            "parentName": "",
            "nodes": [],
            "weight": 1.0
        };
        webvellaDesktopTopnavFactory.addItem(item);
        $rootScope.$emit("webvellaDesktop-topnav-ready");
        $log.debug('rootScope>events> "webvellaDesktop-topnav-ready" emitted ' + moment().format('HH:mm:ss SSSS'));


        pluginData.logout = function () {
        	webvellaAdminService.logout(
                    function (response) {
                    	//  $window.location = '#/login';
                    	$timeout(function () {
                    		$state.go('webvella-root-login');
                    	}, 0);
                    },
                    function (response) { });
        }


        $log.debug('webvellaDesktop>base> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

        function activate() {
            if (pluginData.topnav.length > 0) {
                $timeout(function () {
                    $state.go(pluginData.topnav[0].stateName, pluginData.topnav[0].stateParams)
                }, 0);
               
            }
        }
    }

})();
