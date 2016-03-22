/* sidebar.factory.js */

/**
* @desc factory for managing the desktop topnav object
*/

(function () {
    'use strict';
    angular
        .module('webvellaAdmin')
        .factory('webvellaAdminSidebarFactory', factory);

    factory.$inject = ['$log','$rootScope'];

    /* @ngInject */
    function factory($log,$rootScope) {
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
        	$log.debug('webvellaAdmin>providers>sidebar.factory>initSidebar> function called ' + moment().format('HH:mm:ss SSSS'));
            sidebar = [];
            return sidebar;
        }

        function addItem(item) {
        	$log.debug('webvellaAdmin>providers>sidebar.factory>addItem> function called ' + moment().format('HH:mm:ss SSSS'));
            sidebar.push(item);
            sidebar.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-sidebar-updated', sidebar)
            $log.debug('rootScope>events> "webvellaAdmin-sidebar-updated" emitted ' + moment().format('HH:mm:ss SSSS'));
        }


        function getSidebar() {
        	$log.debug('webvellaAdmin>providers>sidebar.factory>getSidebar> function called ' + moment().format('HH:mm:ss SSSS'));
            return sidebar;
        }
    }
})();