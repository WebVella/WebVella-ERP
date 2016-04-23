/* entity-views.module.js */

/**
* @desc this module manages the entity views in the admin screen
*/

(function () {
	'use strict';

	angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewsController', controller)
	    .controller('CreateViewModalController', createViewModalController)
		.controller('CopyViewModalController', CopyViewModalController);

	// Configuration ///////////////////////////////////
	config.$inject = ['$stateProvider'];

	
	function config($stateProvider) {
		$stateProvider.state('webvella-admin-entity-views', {
			parent: 'webvella-admin-base',
			url: '/entities/:entityName/views', //  /desktop/areas after the parent state is prepended
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
					controller: 'WebVellaAdminEntityViewsController',
					templateUrl: '/plugins/webvella-admin/entity-views.view.html',
					controllerAs: 'ngCtrl'
				}
			},
			resolve: {
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta
			},
			data: {

			}
		});
	};


	// Resolve Function /////////////////////////

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
	
	function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {
		// Initialize
		var defer = $q.defer();

		// Process
		function successCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
			}
			else {
				defer.resolve(response.object);
			}
		}

		function errorCallback(response) {
			if (response.object == null) {
				$timeout(function () {
					alert("error in response!")
				}, 0);
			}
			else {
				defer.reject(response.message);
			}
		}

		webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		return defer.promise;
	}

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal', '$timeout'];

	
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $uibModal, $timeout) {
		
		var ngCtrl = this;
		ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
		ngCtrl.views = fastCopy(resolvedCurrentEntityMeta.recordViews);
		if (ngCtrl.views === null) {
			ngCtrl.views = [];
		}
		ngCtrl.views.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		//Update page title
		ngCtrl.pageTitle = "Entity Views | " + pageTitle;
		$timeout(function () {
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		}, 0);
		$rootScope.adminSectionName = "Entities";
		ngCtrl.showSidebar = function () {
			//Show Sidemenu
			$timeout(function () {
				$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			}, 0);
		}

		ngCtrl.calculateStats = function (view) {
			var itemsCount = 0;
			var sectionsCount = 0;
			for (var i = 0; i < view.regions.length; i++) {
				if (view.regions[i].name == "content") {
					sectionsCount = view.regions[i].sections.length;
					var sections = view.regions[i].sections;
					for (var j = 0; j < sections.length; j++) {
						var rows = sections[j].rows;
						for (var m = 0; m < rows.length; m++) {
							var columns = rows[m].columns;
							for (var k = 0; k < columns.length; k++) {
								var items = columns[k].items;
								itemsCount += items.length;
							}
						}
					}
				}
			}


			if (sectionsCount != 0) {
				return "<span class='go-green'>" + itemsCount + "</span> items and <span class='go-green'>" + sectionsCount + "</span>  sections";
			}
			else {
				return "<span class='go-gray'>empty</span>";
			}
		}


		//Create new view modal
		ngCtrl.createView = function () {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createViewModal.html',
				controller: 'CreateViewModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					}
				}
			});

		}

		//Cppy new view modal
		ngCtrl.copyView = function (view) {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'copyModal.html',
				controller: 'CopyViewModalController',
				controllerAs: "popupCtrl",
				size: "",
				resolve: {
					ngCtrl: function () {
						return ngCtrl;
					},
					view: function () {
						return view;
					}
				}
			});

		}
	}


	//// Modal Controllers
	createViewModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', 'webvellaCoreService'];
	
	function createViewModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl, webvellaCoreService) {
		
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.view = webvellaCoreService.initView();
		popupCtrl.currentEntity = fastCopy(ngCtrl.entity);

		popupCtrl.ok = function () {
			webvellaCoreService.createEntityView(popupCtrl.view, popupCtrl.currentEntity.name, successCallback, errorCallback);
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The view was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.entity, location);
		}
	};


	CopyViewModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl', 'view', 'webvellaCoreService'];
	
	function CopyViewModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, ngCtrl, view, webvellaCoreService) {
		
		var popupCtrl = this;
		popupCtrl.modalInstance = $uibModalInstance;
		popupCtrl.view = fastCopy(view);
		popupCtrl.currentEntity = fastCopy(ngCtrl.entity);
		popupCtrl.alternative = "new";
		popupCtrl.viewName = null;
		popupCtrl.viewNameValidationError = false;

		popupCtrl.entityViews = []; //filter the current view

		for (var i = 0; i < popupCtrl.currentEntity.recordViews.length; i++) {
			if (popupCtrl.currentEntity.recordViews[i].name != popupCtrl.view.name) {
				popupCtrl.entityViews.push(popupCtrl.currentEntity.recordViews[i]);
			}
		}

		popupCtrl.selectedView = popupCtrl.entityViews[0];

		popupCtrl.ok = function () {
			popupCtrl.viewNameValidationError = false;
			if (popupCtrl.alternative == "new") {
				if (popupCtrl.viewName == null || popupCtrl.viewName == "") {
					popupCtrl.viewNameValidationError = true;
				}
				else {
					var newView = fastCopy(popupCtrl.view);
					newView.id = null;
					newView.name = popupCtrl.viewName;
					newView.label = popupCtrl.viewName;
					webvellaCoreService.createEntityView(newView, popupCtrl.currentEntity.name, successCallback, errorCallback);
				}
			}
			else {
				var newView = fastCopy(popupCtrl.view);
				var oldView = fastCopy(popupCtrl.selectedView);
				oldView.regions = newView.regions;
				oldView.sidebar = newView.sidebar;
				oldView.relationOptions = newView.relationOptions;
				webvellaCoreService.updateEntityView(oldView, popupCtrl.currentEntity.name, successCallback, errorCallback);
			}
		};

		popupCtrl.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The view was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaCoreService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			ngToast.create({
				className: 'danger',
				content: '<span class="go-red">Error:</span> ' + response.message
			});
		}
	};
 })();


