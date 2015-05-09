/* topnav.factory.js */

/**
* @desc factory for managing the desktop topnav object
*/

(function () {
    'use strict';
    angular
        .module('webvellaDesktop')
        .factory('webvellaDesktopTopnavFactory', factory);

    factory.$inject = ['$rootScope'];

    /* @ngInject */
    function factory($rootScope) {
        var topnav = [];
        var exports = {
            addItem: addItem,
            getTopnav: getTopnav
        };
        //Some code

        return exports;

        ////////////////

        function addItem(item) {
            topnav.push(item);
            topnav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-topnav-updated', topnav)

        }


        function getTopnav() {
            return topnav;
        }
    }
})();