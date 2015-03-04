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
            "contentView": {
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

}])