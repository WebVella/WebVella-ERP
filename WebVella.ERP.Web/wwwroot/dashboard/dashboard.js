'use strict'

/* Module definition
 * The dependencies block here is also where component dependencies should be
 * specified, as shown below.
 */
angular.module('dashboard', [
  'ui.router'
])

/* State definition */
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider.state('dashboard', {
		url: '/dashboard',
		views: {
			"AreaLocalNavView": {
				controller: 'AreaLocalNavViewModuleCtrl',
				templateUrl: 'navigation/area-local-nav.html'
			},
			"AreasNavView": {
				controller: 'AreasNavViewModuleCtrl',
				templateUrl: 'navigation/areas-nav.html'
			},
			"ActionsNavView": {
				controller: 'ActionsNavViewModuleCtrl',
				templateUrl: 'navigation/actions-nav.html'
			},
			"ActionsLocalNavView": {
				controller: 'ActionsLocalNavViewModuleCtrl',
				templateUrl: 'navigation/actions-local-nav.html'
			},
			"ContentView": {
				controller: 'DashboardModuleContentCtrl',
				templateUrl: 'dashboard/dashboard.html'
			}
		},
		resolve: {}
	});
}])

/**
 * Define the Controller for this state
 */
.controller('DashboardModuleContentCtrl', ['$scope', '$rootScope', '$state', function ($scope, $rootScope, $state) {

}]);