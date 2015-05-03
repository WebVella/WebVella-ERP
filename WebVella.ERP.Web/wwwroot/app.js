/*!
 * WebVella ERP Application JS file
 * https://github.com/WebVella/WebVella-ERP/
 * @license MIT
 * v0.0.1-master
 */

'use strict';

var ErpApp = angular.module('ErpApp', [
    'ngMaterial', 'ngTouch', 'ui.router', 'ngMessages',
    'areaEntity', 'navigation',
	'adminEntities', 'adminEntityManage'
]);


//Application Configuration
ErpApp.config(function ($stateProvider, $urlRouterProvider, $mdThemingProvider) {
	//Set to override the default theme pallete settings
	$mdThemingProvider.theme('default').primaryPalette('indigo').accentPalette('grey');
	$urlRouterProvider.otherwise('/area/area1/null/dashboard');

	//The URLs will be
	//area - area entity data list
	//admin - ERP administration
	//entity - area entity manage

});

ErpApp.controller('AppController', function ($scope, $timeout, $mdSidenav, $mdUtil, $log) {

	//Side Menu Toggler
	$scope.siteMeta = {};
	$scope.toggleLeft = buildToggler('left');
	$scope.toggleRight = buildToggler('right');
	$scope.$watch("siteMeta", function () {
		$scope.$broadcast("siteMeta-changed");
	});

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

var ResolveSiteMetaResponse = function ($q, $state, $config, $ApiService) {
	// Initialize
	var defer = $q.defer();

	// Process
	function successCallBack(response) {
		defer.resolve(response);
	}

	function errorCallBack(response) {
		defer.resolve(response);
	}

	$ApiService.getSiteMeta(successCallBack, errorCallBack);

	// Return
	return defer.promise;
};