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
            $log.debug('webvellaAdmin>providers>sidebar.factory>initSidebar> function called');
            sidebar = [];
            return sidebar;
        }

        function addItem(item) {
            $log.debug('webvellaAdmin>providers>sidebar.factory>addItem> function called');
            sidebar.push(item);
            sidebar.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-topnav-updated', sidebar)
            $log.debug('rootScope>events> "webvellaAdmin-sidebar-updated" emitted');
        }


        function getSidebar() {
            $log.debug('webvellaAdmin>providers>sidebar.factory>getSidebar> function called');
            return sidebar;
        }
    }
})();