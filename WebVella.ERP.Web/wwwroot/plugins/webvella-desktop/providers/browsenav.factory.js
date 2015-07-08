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
            var menuItem = {};
            menuItem.label = area.label;
            menuItem.weight = area.weight;
            menuItem.color = area.color;
            menuItem.iconName = area.icon_name;
            menuItem.stateName = "webvella-entity-records";

            if (area.entities.length > 0) {
            	area.entities.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });
            	var defaultListName = "";
            	for (var i = 0; i < area.entities[0].recordLists.length; i++) {
            		if (area.entities[0].recordLists[i].default) {
            			defaultListName = area.entities[0].recordLists[i].name;
            		}
            	}
            	menuItem.stateParams = {
            		"areaName": area.name,
            		"entityName": area.entities[0].name,
            		"listName": defaultListName,
					"filter":"all",
            		"page": 1
            	};
            }
            else {
            	return null;
            }


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