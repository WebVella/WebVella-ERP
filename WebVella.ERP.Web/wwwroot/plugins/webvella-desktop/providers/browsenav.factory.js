/* browsenav.factory.js */

/**
* @desc factory for managing and generating the Desktop browse section menu / icons
*/

(function () {
    'use strict';
    angular
        .module('webvellaDesktop')
        .factory('webvellaDesktopBrowsenavFactory', factory);

    factory.$inject = ['$log','$rootScope'];

    /* @ngInject */
    function factory($log,$rootScope) {
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
            $log.debug('webvellaDesktop>providers>browsenav.factory>generateInitializeFromAreas> function called');
            browsenav = [];
            areas.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            for (var i = 0; i < areas.length; i++) {
                var menuItem = {};
                menuItem.label = areas[i].label;
                menuItem.stateName = "webvella-areas-view";
                menuItem.stateParams = {
                    "areaName":areas[i].name
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
            $log.debug('webvellaDesktop>providers>browsenav.factory>addItem> function called');
            browsenav.push(menuItem);
            browsenav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-browsenav-updated', browsenav);
            $log.debug('rootScope>events> "webvellaDesktop-browsenav-updated" emitted');
            return browsenav
        }


        ////////////////

        function getBrowsenav() {
            $log.debug('webvellaDesktop>providers>browsenav.factory>getBrowsenav> function called');
            return browsenav
        }


    }
})();