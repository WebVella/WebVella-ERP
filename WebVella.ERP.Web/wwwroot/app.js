/*!
 * WebVella ERP Application JS file
 * https://github.com/WebVella/WebVella-ERP/
 * @license MIT
 * v0.0.1-master
 */

'use strict';

var ErpApp = angular.module('ErpApp', [
    'ngMaterial', 'ngTouch', 'ui.router',
    'dashboard', 'navigation'
]);

ErpApp.config(function ($stateProvider, $urlRouterProvider, $mdThemingProvider) {
	//Set to override the default theme pallete settings
	$mdThemingProvider.theme( 'default' ).primaryPalette( 'indigo' ).accentPalette( 'grey' );
	$urlRouterProvider.otherwise('/dashboard');
}
);

ErpApp.constant(
	"$config", {
		apiBaseUrl: "/api",
		pageSize: 10
	}
);



ErpApp.controller('AppCtrl', function ($scope, $timeout, $mdSidenav, $mdUtil, $log) {

	//Side Menu Toggler
	$scope.toggleLeft = buildToggler('left');
	$scope.toggleRight = buildToggler('right');
	
	function buildToggler(navID) {
		var debounceFn = $mdUtil.debounce(function () {
			$mdSidenav(navID)
			  .toggle()
			  .then(function () {
			  	$log.debug("toggle " + navID + " is done");
			  });
		}, 0);
		return debounceFn;
	}


});