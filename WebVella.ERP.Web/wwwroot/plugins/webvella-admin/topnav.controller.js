/* topnav.controller.js */

/**
* @desc this controller manages the top navigation section of page
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin')  //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAdminTopnavController', controller);

    // Controller ///////////////////////////////
    controller.$inject = ['$state', '$rootScope', '$timeout', 'webvellaCoreService','resolvedCurrentUser'];

    
    function controller($state, $rootScope, $timeout, webvellaCoreService,resolvedCurrentUser) {
        
        var topnavData = this;
        topnavData.currentArea = null;

        topnavData.logout = function () {
        	webvellaCoreService.logout(
                    function (response) {
                        //  $window.location = '#/login';
                        $timeout(function () {
                            $state.go('webvella-core-login');
                        }, 0);
                    },
                    function (response) {});
        }

		topnavData.adminSectionName = null;
		$rootScope.$watch("adminSectionName",function(newValue,oldValue){
			topnavData.adminSectionName = newValue;		
		});

		topnavData.adminSubSectionName = null;
		$rootScope.$watch("adminSubSectionName",function(newValue,oldValue){
			topnavData.adminSubSectionName = newValue;		
		});

		topnavData.currentUser = angular.copy(resolvedCurrentUser);
    }
    
})();
