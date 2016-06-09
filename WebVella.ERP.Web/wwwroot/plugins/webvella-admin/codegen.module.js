/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plug-in. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminCodegenController', controller);

	///////////////////////////////////////////////////////
	/// Configuration
	///////////////////////////////////////////////////////

	config.$inject = ['$stateProvider'];


	function config($stateProvider) {
		$stateProvider.state('webvella-admin-codegen', {
			parent: 'webvella-admin-base',
			url: '/codegen',
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
					controller: 'WebVellaAdminCodegenController',
					templateUrl: '/plugins/webvella-admin/codegen.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
			},
			data: {

			}
		});
	};


	//#region << Resolve Functions >>/////////////////////////


	//#endregion

	//#region << Controller >> ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', '$uibModal',
							'webvellaCoreService', '$timeout', '$translate', '$localStorage', '$http'];

	function controller($scope, $log, $rootScope, $state, pageTitle, $uibModal,
						webvellaCoreService, $timeout, $translate, $localStorage, $http) {

		var ngCtrl = this;
		ngCtrl.codegenSettings = {};
		//#region << Update page title >>
		$translate(['CODE_GENERATION']).then(function (translations) {
			ngCtrl.pageTitle = translations.CODE_GENERATION + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		});
		//#endregion


		//region << Codegen >>
		ngCtrl.aceOptions = {
			useWrapMode: true,
			showGutter: true,
			theme: 'twilight',
			mode: 'csharp',
			firstLineNumber: 1,
			onLoad: function () { },
			onChange: function () { },
			advanced: {
				showPrintMargin: false,
				fontSize: "16px"
			}
		}

		ngCtrl.executeCodeGen = function () {
			ngCtrl.hasError = false;
			ngCtrl.errorMessage = "";
			$http({
				method: 'POST',
				url: '/admin/tools/evaluate-changes?cb=' + moment().toISOString(),
				data: { 
					connectionString: ngCtrl.connectionString,
					includeEntityMeta: ngCtrl.includeEntityMeta,
					includeEntityRelations:ngCtrl.includeEntityRelations,
					includeAreas: ngCtrl.includeAreas,
					includeRoles: ngCtrl.includeRoles
				}
			}).then(function successCallback(response) {
				if (response.data.success) {
					ngCtrl.metaChanges = response.data;
				}
				else {
					ngCtrl.hasError = true;
					ngCtrl.errorMessage = "System error: " + response.data.message;
				}
			}, function errorCallback(response) {
				ngCtrl.hasError = true;
				ngCtrl.errorMessage = "System error: " + response.data.message;
			});
		}

		//#endregion

		//#region << Connection string management >>
		ngCtrl.connectionString = "";
		ngCtrl.includeEntityMeta = false;
		ngCtrl.includeEntityRelations = false;
		ngCtrl.includeAreas = false;
		ngCtrl.includeRoles = false;
		ngCtrl.$storage = $localStorage;
		ngCtrl.editConnectionString = true;
		if (ngCtrl.$storage.codegenSettings) {
			if (ngCtrl.$storage.codegenSettings.connectionString) {
				ngCtrl.editConnectionString = false;
				ngCtrl.connectionString = fastCopy(ngCtrl.$storage.codegenSettings.connectionString);
			}
			ngCtrl.includeEntityMeta = fastCopy(ngCtrl.$storage.codegenSettings.includeEntityMeta);
			ngCtrl.includeEntityRelations = fastCopy(ngCtrl.$storage.codegenSettings.includeEntityRelations);
			ngCtrl.includeAreas = fastCopy(ngCtrl.$storage.codegenSettings.includeAreas);
			ngCtrl.includeRoles = fastCopy(ngCtrl.$storage.codegenSettings.includeRoles);
			//ngCtrl.executeCodeGen();
		}

		ngCtrl.setConnectionString = function () {
			ngCtrl.hasCSError = false;
			ngCtrl.metaChanges = null;
			if (!ngCtrl.connectionString) {
				ngCtrl.hasCSError = true;
				ngCtrl.errorCSMessage = "Connection string is required"
			}
			else {
				ngCtrl.updateLocalStorage();
				ngCtrl.editConnectionString = false;
				//ngCtrl.executeCodeGen();
			}
		}

		ngCtrl.changeConnectionString = function () {
			ngCtrl.editConnectionString = true;
		}

		ngCtrl.updateLocalStorage = function () {
			ngCtrl.codegenSettings = {};
			ngCtrl.codegenSettings.connectionString	=  ngCtrl.connectionString;
			ngCtrl.codegenSettings.includeEntityMeta = fastCopy(ngCtrl.includeEntityMeta);
			ngCtrl.codegenSettings.includeEntityRelations = fastCopy(ngCtrl.includeEntityRelations);
			ngCtrl.codegenSettings.includeAreas = fastCopy(ngCtrl.includeAreas);
			ngCtrl.codegenSettings.includeRoles = fastCopy(ngCtrl.includeRoles);
			ngCtrl.$storage.codegenSettings = fastCopy(ngCtrl.codegenSettings);
		}
		//#endregion



	}
	//#endregion

})();
