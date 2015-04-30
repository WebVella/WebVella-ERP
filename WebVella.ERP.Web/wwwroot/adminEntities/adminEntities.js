'use strict'

/* Admin Entities Module Definition
 * The main purpose of the module is to present and manage
 * the entities of the ERP system
 */
angular.module('adminEntities', [
  'ui.router'
])

/* State definition */
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider.state('adminEntities', {
		url: '/admin/entities',
		views: {
			"AreaLocalNavView": {
				controller: 'AdminLocalNavViewModuleController',
				templateUrl: 'navigation/admin-local-nav.html'
			},
			"AreasNavView": {
				controller: 'AreasNavViewModuleController',
				templateUrl: 'navigation/areas-nav.html'
			},
			"ActionsNavView": {
				controller: 'ActionsNavViewModuleController',
				templateUrl: 'navigation/actions-nav.html'
			},
			"ActionsLocalNavView": {
				controller: 'ActionsLocalNavViewModuleController',
				templateUrl: 'navigation/actions-local-nav.html'
			},
			"ContentView": {
				controller: 'AdminEntitiesModuleContentController',
				templateUrl: 'adminEntities/adminEntities.html'
			}
		},
		resolve: {
			resolveSiteMeta: ResolveSiteMetaResponse
		}
	});
}])

/**
 * Define the Controller for this state
 */
.controller('AdminEntitiesModuleContentController', function ($scope, $rootScope, $state, $log, $SiteMetaCache,$mdDialog, resolveSiteMeta) {

	if (resolveSiteMeta.success) {
		$SiteMetaCache.update(resolveSiteMeta.object);
	}
	else {
		alert("Error getting SiteMeta")
	}

	$scope.showAdvanced = function (ev) {
		$mdDialog.show({
			controller: CreateEntityModalController,
			templateUrl: 'createEntityModal.html',
			targetEvent: ev,
		})
		.then(function (answer) {
			$scope.alert = 'You said the information was "' + answer + '".';
		}, function () {
			$scope.alert = 'You cancelled the dialog.';
		});
	};
});

function CreateEntityModalController($scope, $mdDialog) {

	$scope.newEntity = {};

	$scope.newEntity.name = null;

	$scope.hide = function () {
		$mdDialog.hide();
	};
	$scope.cancel = function () {
		$mdDialog.cancel();
	};
	$scope.answer = function () {
		$mdDialog.hide($scope.newEntity);
	};
}

