/* browsenav.factory.js */

/**
* @desc factory for managing and generating the Desktop browse section menu / icons
*/

(function () {
    'use strict';
    angular
        .module('webvellaDesktop')
        .factory('webvellaDesktopBrowsenavFactory', factory);

    factory.$inject = ['$rootScope'];

    /* @ngInject */
    function factory($rootScope) {
        var browsenav = [];
        var exports = {
            generateInitializeFromAreas: generateInitializeFromAreas,
            addItem: addItem,
            getBrowsenav: getBrowsenav
        };
        //Some code

        return exports;

        ////////////////

        function generateInitializeFromAreas(areas) {
            browsenav = [];
            areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            for (var i = 0; i < areas.length; i++) {
                var menuItem = {};
                menuItem.label = areas[i].label;
                menuItem.stateName = "webvella-areas-view";
                menuItem.stateParams = {
                    "name":areas[i].name
                };
                menuItem.parentName = "";
                menuItem.nodes = [];
                menuItem.weight = areas[i].weight;
                menuItem.color = areas[i].color;
                menuItem.iconName = areas[i].iconName;
                browsenav.push(menuItem);
            }

            return browsenav
        }

        ////////////////

        function addItem(menuItem) {
            browsenav.push(menuItem);
            browsenav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-browsenav-updated', browsenav);
            return browsenav
        }


        ////////////////

        function getBrowsenav() {
            return browsenav
        }


    }
})();