/* topnav.factory.js */

/**
* @desc factory for managing the desktop topnav object
*/

(function () {
    'use strict';
    angular
        .module('webvellaDesktop')
        .factory('webvellaDesktopTopnavFactory', factory);

    factory.$inject = ['$log','$rootScope'];

    /* @ngInject */
    function factory($log,$rootScope) {
        var topnav = [];
        var exports = {
            initTopnav:initTopnav,
            addItem: addItem,
            getTopnav: getTopnav
        };
        //Some code

        return exports;

        ////////////////

        function initTopnav() {
            $log.debug('webvellaDesktop>providers>topnav.factory>initTopnav> function called');
            topnav = [];
            return topnav;
        }

        function addItem(item) {
            $log.debug('webvellaDesktop>providers>topnav.factory>addItem> function called');
            topnav.push(item);
            topnav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-topnav-updated', topnav)
            $log.debug('rootScope>events> "webvellaDesktop-topnav-updated" emitted');
        }


        function getTopnav() {
            $log.debug('webvellaDesktop>providers>topnav.factory>getTopnav> function called');
            return topnav;
        }
    }
})();