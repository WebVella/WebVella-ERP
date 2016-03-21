/* base.module.js */

/**
* @desc this the base module of the Desktop plugin
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaAdminBaseController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-base', {
            'abstract': true,
            'url': '/admin', //will be added to all children states
            'views': {
                "rootView": {
                    'controller': 'WebVellaAdminBaseController',
                    'templateUrl': '/plugins/webvella-admin/base.view.html',
                    'controllerAs': 'pluginData'
                }
            },
            'params': {
                "redirect": 'false'
            },
            'resolve': {
                //here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
                'pageTitle': function () {
                    return "Webvella ERP";
                },
               	resolvedCurrentUser: resolveCurrentUser
            },
            'data': {
                //Custom data is inherited by the parent state 'webvella-root', but it can be overwritten if necessary. Available for all child states in this plugin
            }
        });
    };

    // Run //////////////////////////////////////
    run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory','webvellaDesktopTopnavFactory', 'webvellaRootService'];

    /* @ngInject */
    function run($log, $rootScope, webvellaDesktopBrowsenavFactory,webvellaDesktopTopnavFactory, webvellaRootService) {
    	$log.debug('webvellaAdmin>base> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));
        $rootScope.$on('webvellaDesktop-browsenav-ready', function () {
			//Allow visible only to admins
        	var currentUser = webvellaRootService.getCurrentUser();
            if (currentUser.roles.indexOf("bdc56420-caf0-4030-8a0e-d264938e0cda") > -1) {
            	var item = {
            		"label": "Administration",
            		"stateName": "webvella-admin-entities",
            		"stateParams": {},
            		"parentName": "",
					"folder":"Admin",
            		"nodes": [],
            		"weight": 100.0,
            		"color": "red",
            		"iconName": "cogs",
            		"roles": "[\"bdc56420-caf0-4030-8a0e-d264938e0cda\"]"
            	};
            	webvellaDesktopBrowsenavFactory.addItem(item);

				var topNavItem = {
					"label": "Admin",
					"stateName": "webvella-desktop-browse",
					"stateParams": {folder:"Admin"},
					"parentName": "",
					"nodes": [],
					"weight": 9999					
				}

				webvellaDesktopTopnavFactory.addItem(topNavItem);
            }
        });

        $log.debug('webvellaAdmin>base> END module.run ' + moment().format('HH:mm:ss SSSS'));
    };

	// Resolve ///////////////////////////////////
    resolveCurrentUser.$inject = ['$q', '$log', 'webvellaAdminService', 'webvellaRootService', '$state', '$stateParams'];
	/* @ngInject */
    function resolveCurrentUser($q, $log, webvellaAdminService, webvellaRootService, $state, $stateParams) {
    	$log.debug('webvellaAdmin>base>resolveCurrentUser> BEGIN user resolved ' + moment().format('HH:mm:ss SSSS'));
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
    	$log.debug('webvellaAdmin>base>resolveCurrentUser> END user resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$scope','$state', '$rootScope','$stateParams', 'webvellaRootService', 'webvellaAdminSidebarFactory'];

    /* @ngInject */
    function controller($log, $scope,$state, $rootScope,$stateParams, webvellaRootService, webvellaAdminSidebarFactory) {
    	$log.debug('webvellaAdmin>base> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var adminData = this;
        adminData.sidebar = [];

        //Making topnav pluggable
        ////1. CONSTRUCTOR initialize the factory
        webvellaAdminSidebarFactory.initSidebar();
        ////2. READY hook listener
        var readySidebarDestructor = $rootScope.$on("webvellaAdmin-sidebar-ready", function() {
            //All actions you want to be done after the "Ready" hook is cast
        });
        ////3. UPDATED hook listener
        var updateSidebarDestructor = $rootScope.$on("webvellaAdmin-sidebar-updated", function (event, data) {
            adminData.sidebar = data;
            activate();
        });
        ////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
        $scope.$on("$destroy", function () {
            readySidebarDestructor();
            updateSidebarDestructor();
        });
        ////5. Bootstrap the pluggable element and cast the Ready hook
        //Push the Regular menu items
        var item = {
            "label": "Entities",
            "stateName": "webvella-admin-entities",
            "stateParams": {},
            "parentName": "",
            "nodes": [],
            "weight": 1.0,
            "color": "red",
            "iconName": "cog"
        };
        adminData.sidebar.push(item);
        item = {
            "label": "Users",
            "stateName": "webvella-admin-users",
            "stateParams": {},
            "parentName": "",
            "nodes": [],
            "weight": 2.0,
            "color": "red",
            "iconName": "cog"
        };
        adminData.sidebar.push(item);
        $rootScope.$emit("webvellaAdmin-sidebar-ready");
        $log.debug('rootScope>events> "webvellaAdmin-sidebar-ready" emitted ' + moment().format('HH:mm:ss SSSS'));

        activate();
        $log.debug('webvellaAdmin>base> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
        function activate() {
            // Change the body color to all child states to red
            webvellaRootService.setBodyColorClass("red");
            
        }
    }

})();
