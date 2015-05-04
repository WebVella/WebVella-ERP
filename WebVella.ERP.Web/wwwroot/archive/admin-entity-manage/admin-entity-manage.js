'use strict'

/* Admin Entities Module Definition
 * The main purpose of the module is to present and manage
 * the entities of the ERP system
 */
angular.module('adminEntityManage', [
  'ui.router'
])

/* State definition */
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider.state('adminEntityManage', {
		url: '/admin/entities/:entityName/manage',
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
				controller: 'AdminEntityManageModuleContentController',
				templateUrl: 'admin-entity-manage/admin-entity-manage.html'
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
.controller('AdminEntityManageModuleContentController', function ($scope, $rootScope, $state, $log, $SiteMetaCache, $mdDialog, $q, resolveSiteMeta) {

	if (resolveSiteMeta.success) {
		$SiteMetaCache.update(resolveSiteMeta.object);
	}
	else {
		alert("Error getting SiteMeta")
	}


});


