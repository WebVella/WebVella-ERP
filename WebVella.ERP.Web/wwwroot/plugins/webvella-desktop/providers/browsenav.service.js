/* browsenav.service.js */

/**
* @desc service for managing and generating the Desktop browse section menu / icons
* @data menu item is stored in $rootScope.webvellaDesktop.browsenav;
*/

(function () {
    'use strict';

    angular
        .module('webvellaDesktop')
        .service('webvellaDesktopBrowsenavService', service);

    service.$inject = ['$log', '$rootScope'];

    /* @ngInject */
    function service($log, $rootScope) {
        var self = this;

        self.generateMenuItemFromArea = generateMenuItemFromArea;
        self.addItem = addItem;
        self.initBrowsenav = initBrowsenav;


        /////////////
        function generateMenuItemFromArea(area) {
            $log.debug('webvellaDesktop>providers>browsenav.factory>generateMenuItemFromArea> function called');

            //Redirect to the first entity of the area
            var firstEntityName = null;
            var firstEntitySectionName = null;
            for (var i = 0; i < area.sections.length; i++) {
                if (area.sections[i].entities.length > 0) {
                    firstEntityName = area.sections[i].entities[0].name;
                    firstEntitySectionName = area.sections[i].name;
                    break;
                }
            }

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

        /////////////
        function addItem(menuItem) {
            $log.debug('webvellaDesktop>providers>browsenav.factory>addItem> function called');
            $rootScope.webvellaDesktop.browsenav.push(menuItem);
            $rootScope.webvellaDesktop.browsenav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-browsenav-updated', $rootScope.webvellaDesktop.browsenav);
            $log.debug('rootScope>events> "webvellaDesktop-browsenav-updated" emitted');
        }

        //////////////
        function initBrowsenav() {
            $log.error("ready");
            $log.debug('webvellaDesktop>providers>browsenav.factory>initBrowsenav> function called');
            $rootScope.$broadcast('webvellaDesktop-browsenav-ready');
            $rootScope.webvellaDesktop.browsenav = [];
        }
    }
})();