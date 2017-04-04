/* sidebar.controller.js */

/**
* @desc this controller manages the sidebar of the areas section
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAdminSidebarController', controller);


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'resolvedCurrentUser','$localStorage'];
     
    function controller($log, $rootScope, $state, resolvedCurrentUser, $localStorage) {
        
        var sidebarData = this;
        sidebarData.isMiniSidebar = $rootScope.isMiniSidebar;
		sidebarData.stateName = $state.current.name;
        $rootScope.$on("application-sidebar-mini-toggle", function (event) {
        	sidebarData.isMiniSidebar = $rootScope.isMiniSidebar;
        });
        sidebarData.currentUser = angular.copy(resolvedCurrentUser);

        sidebarData.$storage = $localStorage;
        sidebarData.toggleSideNav = function () {
        	sidebarData.$storage.isMiniSidebar = !sidebarData.$storage.isMiniSidebar;
        }
    }

})();
