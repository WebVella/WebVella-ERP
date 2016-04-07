/* base.module.js */

/**
* @desc this the base module of the Desktop plugin. Its only tasks is to check the topNavFactory and redirect to the first menu item state
*/

(function () {
	'use strict';

	angular
        .module('webvellaDevelopers', ['ui.router'])
        .config(config)
        .run(run)
        .controller('WebVellaDevelopersBaseController', WebVellaDevelopersBaseController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	/* @ngInject */
	function config($stateProvider) {
		$stateProvider.state('webvella-developers-base', {
			//abstract: true,
			url: '/developers', //will be added to all children states
			views: {
				"rootView": {
					controller: 'WebVellaDevelopersBaseController',
					templateUrl: '/plugins/webvella-developers/base.view.html',
					controllerAs: 'pluginData'
				}
			},
			resolve: {
				//here you can resolve any plugin wide data you need. It will be available for all children states. Parent resolved objects can be injected in the functions too
				pageTitle: function () {
					return "Webvella ERP";
				}
			},
			data: {}
		});
	};


	// Run //////////////////////////////////////
	run.$inject = ['$log', '$rootScope', 'webvellaDesktopBrowsenavFactory', 'Upload', 'webvellaRootService'];

	/* @ngInject */
	function run($log, $rootScope, webvellaDesktopBrowsenavFactory, upload, webvellaRootService) {
		$log.debug('webvellaDevelopers>base> BEGIN module.run ' + moment().format('HH:mm:ss SSSS'));
		//Allow visible only to admins
		$rootScope.$on('webvellaDesktop-browsenav-ready', function (event) {
			var currentUser = webvellaRootService.getCurrentUser();
			if (currentUser.roles.indexOf("bdc56420-caf0-4030-8a0e-d264938e0cda") > -1) {
				var item = {
					"label": "Developers",
					"stateName": "webvella-developers-base",
					"stateParams": {},
					"parentName": "",
					"folder":"Admin",
					"nodes": [],
					"weight": 101.0,
					"color": "purple",
					"iconName": "code"
				};
				//If item is admin
				webvellaDesktopBrowsenavFactory.addItem(item);
			}
		});
		$log.debug('webvellaDevelopers>base> END module.run ' + moment().format('HH:mm:ss SSSS'));
	};


	// Controller ///////////////////////////////
	WebVellaDevelopersBaseController.$inject = ['$log', '$scope', '$timeout', '$rootScope', 'webvellaDevelopersQueryService', 'Upload', 'webvellaAdminService'];

	/* @ngInject */
	function WebVellaDevelopersBaseController($log, $scope, $timeout,$rootScope, queryService, Upload, webvellaAdminService) {
		$log.debug('webvellaDevelopers>base> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var pluginData = this;

		pluginData.getGender = function(){
			return "male";
		}

		pluginData.getName = function(){
			return "one";
		}

		pluginData.boz = "<a ng-click='click(1)' href='#'>Click me</a>";
	}

})();

