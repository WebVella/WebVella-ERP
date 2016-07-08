/* topnav.controller.js */

/**
* @desc this controller manages the topnav of the areas section
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAreasTopnavController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$rootScope', '$state', '$stateParams', 'resolvedAreas', '$timeout','webvellaCoreService', 'resolvedCurrentEntityMeta','resolvedCurrentUser'];

    
    function controller($rootScope, $state, $stateParams, resolvedAreas, $timeout,webvellaCoreService, resolvedCurrentEntityMeta,resolvedCurrentUser) {
        
        var topnavData = this;
        topnavData.currentArea = webvellaCoreService.getCurrentAreaFromAreaList($stateParams.areaName, resolvedAreas.data);
		topnavData.currentArea.attachments = angular.fromJson(topnavData.currentArea.attachments);
		topnavData.areaLink = "";
		topnavData.firstAttachment = topnavData.currentArea.attachments[0];
		if(topnavData.firstAttachment.url && topnavData.firstAttachment != ""){
			topnavData.areaLink = topnavData.firstAttachment.url;
		}
		else {
			topnavData.areaLink = "#/areas/" + topnavData.currentArea.name +"/" + topnavData.firstAttachment.name +"/list-general/" + topnavData.firstAttachment.list.name;
		}
		topnavData.currentEntity = resolvedCurrentEntityMeta;
		topnavData.currentUser = angular.copy(resolvedCurrentUser);
        topnavData.logout = function () {
        	webvellaCoreService.logout(
                    function (response) {
                    	//  $window.location = '#/login';
                    	$timeout(function () {
                    		$state.go('webvella-core-login');
                    	}, 0);
                    },
                    function (response) { });
        }
    }

})();
