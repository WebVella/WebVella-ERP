/* sidebar.controller.js */

/**
* @desc this controller manages the sidebar of the areas section
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAreasSidebarController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedSitemap', 'webvellaAreasService', 'resolvedCurrentUser','$timeout','$location'];

    /* @ngInject */
    function controller($log, $rootScope, $state, $stateParams, resolvedSitemap, webvellaAreasService, resolvedCurrentUser,$timeout,$location) {
    	$log.debug('webvellaAreas>sidebar> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var sidebarData = this;
        sidebarData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
        sidebarData.currentArea.attachments = angular.fromJson(sidebarData.currentArea.attachments);
        sidebarData.currentUser = angular.copy(resolvedCurrentUser);

		sidebarData.isCurrentAttachmentActive = function(attachment){
			if(attachment.name == $stateParams.entityName){
				return true;
			}
			else if($location.path().startsWith(attachment.url) ){
				return true;
			}
			else {
				return false;
			}
		}

        $log.debug('webvellaAreas>sidebar> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

})();
