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


	function config($stateProvider) {
		$stateProvider.state('webvella-admin-base', {
			'abstract': true,
			'url': '/admin', //will be added to all children states
			'views': {
				"rootView": {
					'controller': 'WebVellaAdminBaseController',
					'templateUrl': '/plugins/webvella-admin/base.view.html',
					'controllerAs': 'baseCtrl'
				}
			},
			'params': {
				"redirect": 'false'
			},
			'resolve': {
				//here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
				'pageTitle': function () {
					return GlobalCompanyName;
				},
				resolvedCurrentUser: resolveCurrentUser,
				checkedAccessPermission: checkAccessPermission
			},
			'data': {
				//Custom data is inherited by the parent state 'webvella-core', but it can be overwritten if necessary. Available for all child states in this plugin
			}
		});
	};

	// Run //////////////////////////////////////
	run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory', 'webvellaDesktopTopnavFactory', 'webvellaCoreService', '$translate'];


	function run($log, $rootScope, webvellaDesktopBrowsenavFactory, webvellaDesktopTopnavFactory, webvellaCoreService, $translate) {
		$rootScope.$on('webvellaDesktop-browsenav-ready', function () {
			//Allow visible only to admins
			var currentUser = webvellaCoreService.getCurrentUser();
			if (currentUser.roles.indexOf("bdc56420-caf0-4030-8a0e-d264938e0cda") > -1) {

				$translate(['ADMINISTRATION_CORE', 'ADMINISTRATION_ABBR']).then(function (translations) {
					var item = {
						"label": translations.ADMINISTRATION_CORE,
						"stateName": "webvella-admin-entities",
						"stateParams": {},
						"parentName": "",
						"folder": translations.ADMINISTRATION_ABBR,
						"nodes": [],
						"weight": 10000,
						"color": "red",
						"iconName": "cogs",
						"roles": "[\"bdc56420-caf0-4030-8a0e-d264938e0cda\"]"
					};
					webvellaDesktopBrowsenavFactory.addItem(item);

					var topNavItem = {
						"label": translations.ADMINISTRATION_ABBR,
						"type": "admin",
						"stateName": "webvella-desktop-browse",
						"stateParams": { folder: translations.ADMINISTRATION_ABBR },
						"parentName": "",
						"nodes": [],
						"weight": 9999
					}

					webvellaDesktopTopnavFactory.addItem(topNavItem);


				});
			}
		});
	};

	// Resolve ///////////////////////////////////
	checkAccessPermission.$inject = ['$q', '$log', 'resolvedCurrentUser', 'ngToast', '$translate'];

	function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast, $translate) {
		var defer = $q.defer();

		var accessPermission = false;
		for (var i = 0; i < resolvedCurrentUser.roles.length; i++) {
			if (resolvedCurrentUser.roles[i] == "bdc56420-caf0-4030-8a0e-d264938e0cda") {
				accessPermission = true;
			}
		}

		if (accessPermission) {
			defer.resolve();
		}
		else {
			$translate(['NO_ACCESS_TO_AREA']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.NO_ACCESS_TO_AREA,
					timeout: 7000

				});
			});
			defer.reject("Admin > No access. You do not have access to the administration area!");
		}
		return defer.promise;
	}

	resolveCurrentUser.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams', '$timeout'];

	function resolveCurrentUser($q, $log, webvellaCoreService, $state, $stateParams, $timeout) {
		// Initialize
		var defer = $q.defer();
		// Process
		var currentUser = webvellaCoreService.getCurrentUser();

		if (currentUser != null) {
			defer.resolve(currentUser);
		}
		else {
			defer.reject(null);
		}

		return defer.promise;
	}

	// Controller ///////////////////////////////
	controller.$inject = ['$log', '$scope', '$state', '$rootScope', '$stateParams', 'webvellaCoreService', 'webvellaAdminSidebarFactory', '$timeout', '$translate'];


	function controller($log, $scope, $state, $rootScope, $stateParams, webvellaCoreService, webvellaAdminSidebarFactory, $timeout, $translate) {

		var baseCtrl = this;
		baseCtrl.sidebar = [];
		//Making topnav pluggable
		////1. CONSTRUCTOR initialize the factory
		webvellaAdminSidebarFactory.initSidebar();
		////2. READY hook listener
		var readySidebarDestructor = $rootScope.$on("webvellaAdmin-sidebar-ready", function () {
			//All actions you want to be done after the "Ready" hook is cast
		});
		////3. UPDATED hook listener
		var updateSidebarDestructor = $rootScope.$on("webvellaAdmin-sidebar-updated", function (event, data) {
			baseCtrl.sidebar = data;
			activate();
		});
		////4. DESCTRUCTOR - hook listeners remove on scope destroy. This avoids duplication, as rootScope is never destroyed and new controller load will duplicate the listener
		$scope.$on("$destroy", function () {
			readySidebarDestructor();
			updateSidebarDestructor();
		});
		////5. Bootstrap the pluggable element and cast the Ready hook
		//Push the Regular menu items
		$translate(['ENTITIES', 'USERS']).then(function (translations) {
			var item = {
				"label": translations.ENTITIES,
				"stateName": "webvella-admin-entities",
				"stateParams": {},
				"parentName": "",
				"nodes": [],
				"weight": 1.0,
				"color": "red",
				"iconName": "cog"
			};
			baseCtrl.sidebar.push(item);
			item = {
				"label": translations.USERS,
				"stateName": "webvella-admin-users",
				"stateParams": {},
				"parentName": "",
				"nodes": [],
				"weight": 2.0,
				"color": "red",
				"iconName": "cog"
			};
			baseCtrl.sidebar.push(item);
			$timeout(function () {
				$rootScope.$emit("webvellaAdmin-sidebar-ready");
			}, 0);
		});
		activate();
		function activate() {
			// Change the body color to all child states to red
			webvellaCoreService.setBodyColorClass("red");

		}
	}

})();
