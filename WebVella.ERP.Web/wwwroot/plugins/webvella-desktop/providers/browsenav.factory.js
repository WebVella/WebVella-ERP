/* browsenav.factory.js */

/**
* @desc factory for managing and generating the Desktop browse section menu / icons
*/

(function () {
    'use strict';
    angular
        .module('webvellaDesktop')
        .factory('webvellaDesktopBrowsenavFactory', factory);

    factory.$inject = ['$log', '$rootScope'];

    /* @ngInject */
    function factory($log, $rootScope) {
        var browsenav = [];

        var exports = {
            generateMenuItemFromArea: generateMenuItemFromArea,
            addItem: addItem,
            getBrowsenav: getBrowsenav,
            initBrowsenav: initBrowsenav
        };
        //Some code

        return exports;

        ////////////////

        function generateMenuItemFromArea(area) {
            $log.debug('webvellaDesktop>providers>browsenav.factory>generateMenuItemFromArea> function called');

            //Redirect to the first entity of the area
            var firstEntityName = null;
            var firstEntitySectionName = null;
            //When sections are implemented
            //for (var i = 0; i < area.sections.length; i++) {
            //    if (area.sections[i].entities.length > 0) {
            //        firstEntityName = area.sections[i].entities[0].name;
            //        firstEntitySectionName = area.sections[i].name;
            //        break;
            //    }
            //}

            //Working without sections 
            firstEntityName = "order";
            firstEntitySectionName = null;


            var menuItem = {};
            menuItem.label = area.label;

            if (firstEntityName != null) {
                menuItem.stateName = "webvella-areas-entities";
                menuItem.stateParams = {
                    "areaName": area.name,
                    "sectionName": firstEntitySectionName,
                    "entityName": firstEntityName
                };
            }
            else {
                //If no entities related raise error and cancel navigation
                alert("This area has no entities attached");
                $log.error('webvellaDesktop>providers>browsenav.factory>generateMenuItemFromArea> This area has no entities attached');
                menuItem.stateName = "webvella-root-home";
                menuItem.stateParams = {};
            }

            menuItem.parentName = "";
            menuItem.nodes = [];
            menuItem.weight = area.weight;
            menuItem.color = area.color;
            menuItem.iconName = area.iconName;
            return menuItem
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

        ////////////////

        function initBrowsenav() {
            $log.debug('webvellaDesktop>providers>browsenav.factory>initBrowsenav> function called');
            browsenav = [];
            return browsenav
        }
    }
})();