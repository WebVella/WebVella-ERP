/* some-name.factory.js */

/**
* @desc just a sample factory code
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
            addTopnavItem: addTopnavItem,
            getTopnav: getTopnav
        };
        //Some code

        return exports;

        ////////////////

        function addTopnavItem(item) {
            topnav.push(item);
            topnav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
        }


        function getTopnav() {
            return topnav;
        }
    }
})();