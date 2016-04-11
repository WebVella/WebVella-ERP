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

	/* @ngInject */
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
					controllerAs: 'contentData'
				}
			},
			resolve: {
				checkedAccessPermission: checkAccessPermission,
				resolvedCurrentEntityMeta: resolveCurrentEntityMeta
			},
			data: {

			}
		});
	};


	// Resolve Function /////////////////////////
	checkAccessPermission.$inject = ['$q', '$log', 'resolvedCurrentUser', 'ngToast'];
	/* @ngInject */
	function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		var defer = $q.defer();
		var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">Admin</span> area';
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

			ngToast.create({
				className: 'error',
				content: messageContent,
				timeout: 7000
			});
			defer.reject("No access");
		}

		$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
	/* @ngInject */
	function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
		$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

		webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

		// Return
		$log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
		return defer.promise;
	}

	// Controller ///////////////////////////////
	controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'resolvedCurrentEntityMeta', '$uibModal'];

	/* @ngInject */
	function controller($scope, $log, $rootScope, $state, pageTitle, resolvedCurrentEntityMeta, $uibModal) {
		$log.debug('webvellaAdmin>entity-details> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var contentData = this;
		contentData.entity = fastCopy(resolvedCurrentEntityMeta);
		contentData.views = fastCopy(resolvedCurrentEntityMeta.recordViews);
		if (contentData.views === null) {
			contentData.views = [];
		}
		contentData.views.sort(function (a, b) {
			if (a.name < b.name) return -1;
			if (a.name > b.name) return 1;
			return 0;
		});

		//Update page title
		contentData.pageTitle = "Entity Views | " + pageTitle;
		$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
		//Hide Sidemenu
		$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));

		contentData.showSidebar = function () {
			//Show Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		}

		contentData.calculateStats = function (view) {
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
		contentData.createView = function () {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'createViewModal.html',
				controller: 'CreateViewModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					contentData: function () {
						return contentData;
					}
				}
			});

		}

		//Cppy new view modal
		contentData.copyView = function (view) {

			var modalInstance = $uibModal.open({
				animation: false,
				templateUrl: 'copyModal.html',
				controller: 'CopyViewModalController',
				controllerAs: "popupData",
				size: "",
				resolve: {
					contentData: function () {
						return contentData;
					},
					view: function () {
						return view;
					}
				}
			});

		}


		$log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	}


	//// Modal Controllers
	createViewModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'contentData', 'webvellaAdminService', 'webvellaRootService'];
	/* @ngInject */
	function createViewModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, contentData, webvellaAdminService, webvellaRootService) {
		$log.debug('webvellaAdmin>entities>createViewModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.modalInstance = $uibModalInstance;
		popupData.view = webvellaAdminService.initView();
		popupData.currentEntity = fastCopy(contentData.entity);

		popupData.ok = function () {
			webvellaAdminService.createEntityView(popupData.view, popupData.currentEntity.name, successCallback, errorCallback);
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The view was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaRootService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			var location = $location;
			//Process the response and generate the validation Messages
			webvellaRootService.generateValidationMessages(response, popupData, popupData.entity, location);
		}

		$log.debug('webvellaAdmin>entities>createViewModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};


	CopyViewModalController.$inject = ['$uibModalInstance', '$log', 'ngToast', '$timeout', '$state', '$location', 'contentData', 'view', 'webvellaAdminService', 'webvellaRootService'];
	/* @ngInject */
	function CopyViewModalController($uibModalInstance, $log, ngToast, $timeout, $state, $location, contentData, view, webvellaAdminService, webvellaRootService) {
		$log.debug('webvellaAdmin>entities>createViewModalController> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
		/* jshint validthis:true */
		var popupData = this;
		popupData.modalInstance = $uibModalInstance;
		popupData.view = fastCopy(view);
		popupData.currentEntity = fastCopy(contentData.entity);
		popupData.alternative = "new";
		popupData.viewName = null;
		popupData.viewNameValidationError = false;

		popupData.entityViews = []; //filter the current view

		for (var i = 0; i < popupData.currentEntity.recordViews.length; i++) {
			if (popupData.currentEntity.recordViews[i].name != popupData.view.name) {
				popupData.entityViews.push(popupData.currentEntity.recordViews[i]);
			}
		}

		popupData.selectedView = popupData.entityViews[0];

		popupData.ok = function () {
			popupData.viewNameValidationError = false;
			if (popupData.alternative == "new") {
				if (popupData.viewName == null || popupData.viewName == "") {
					popupData.viewNameValidationError = true;
				}
				else {
					var newView = fastCopy(popupData.view);
					newView.id = null;
					newView.name = popupData.viewName;
					newView.label = popupData.viewName;
					webvellaAdminService.createEntityView(newView, popupData.currentEntity.name, successCallback, errorCallback);
				}
			}
			else {
				var newView = fastCopy(popupData.view);
				var oldView = fastCopy(popupData.selectedView);
				oldView.regions = newView.regions;
				oldView.sidebar = newView.sidebar;
				oldView.relationOptions = newView.relationOptions;
				webvellaAdminService.updateEntityView(oldView, popupData.currentEntity.name, successCallback, errorCallback);
			}
		};

		popupData.cancel = function () {
			$uibModalInstance.dismiss('cancel');
		};

		/// Aux
		function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + 'The view was successfully saved'
			});
			$uibModalInstance.close('success');
			webvellaRootService.GoToState($state.current.name, {});
		}

		function errorCallback(response) {
			ngToast.create({
				className: 'danger',
				content: '<span class="go-red">Error:</span> ' + response.message
			});
		}

		$log.debug('webvellaAdmin>entities>createViewModalController> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
	};




})();


