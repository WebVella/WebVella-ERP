'use strict'

/* Area Entity Module Definition
 * The main purpose of the module is to present the instances of one of the area's entities
*/
angular.module('areaEntity', [
  'ui.router'
])

/* State definition */
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider.state('areaEntity', {
		url: '/area/:areaName/:sectionName/:entityName',
		views: {
			"AreaLocalNavView": {
				controller: 'AreaLocalNavViewModuleController',
				templateUrl: 'navigation/area-local-nav.html'
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
				controller: 'AreaEnitityModuleContentController',
				templateUrl: 'area-entity/area-entity.html'
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
.controller('AreaEnitityModuleContentController', function ($scope, $rootScope, $state, $log, $SiteMetaCache, resolveSiteMeta) {
	if (resolveSiteMeta.success) {
		$SiteMetaCache.update(resolveSiteMeta.object);
	}
	else {
		alert("Error getting SiteMeta")
	}
});

