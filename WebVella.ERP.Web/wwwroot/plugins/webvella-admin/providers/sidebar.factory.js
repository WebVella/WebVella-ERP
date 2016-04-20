/* sidebar.factory.js */

/**
* @desc factory for managing the desktop topnav object
*/

(function () {
    'use strict';
    angular
        .module('webvellaAdmin')
        .factory('webvellaAdminSidebarFactory', factory);

    factory.$inject = ['$log','$rootScope','$timeout'];

    
    function factory($log,$rootScope,$timeout) {
        var sidebar = [];
        var exports = {
            initSidebar: initSidebar,
            addItem: addItem,
            getSidebar: getSidebar
        };
        //Some code

        return exports;

        ////////////////

        function initSidebar() {
            sidebar = [];
            return sidebar;
        }

        function addItem(item) {
            sidebar.push(item);
            sidebar.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
			$timeout(function(){
				$rootScope.$emit('webvellaDesktop-sidebar-updated', sidebar)
			},0);
        }


        function getSidebar() {
            return sidebar;
        }
    }
})();