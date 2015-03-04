/*!
 * WebVella ERP Application JS file
 * https://github.com/WebVella/WebVella-ERP/
 * @license MIT
 * v0.0.1-master
 */

'use strict';

var erpApp = angular.module('erpApp', [
    'ngMaterial', 'ui.router',
    'dashboard'
]);

erpApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/dashboard');
    }
]);

erpApp.constant(
	"$config", {
	    apiBaseUrl: "/api",
	    pageSize: 10
	}
);



erpApp.controller('AppCtrl', function ($scope) {
  });