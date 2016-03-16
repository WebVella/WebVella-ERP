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
        	$log.debug('webvellaDesktop>providers>browsenav.factory>generateMenuItemFromArea> function called ' + moment().format('HH:mm:ss SSSS'));

            //Redirect to the first entity of the area
            var menuItem = {};
            menuItem.label = area.label;
            menuItem.weight = area.weight;
            menuItem.color = area.color;
            menuItem.iconName = area.icon_name;
			menuItem.folder = area.folder;
            menuItem.stateName = "webvella-entity-records";
            var entitySubscriptions = angular.fromJson(area.subscriptions);

            //Safty check - if area has subscriptions. 
            if (entitySubscriptions == []) {
                $log.error("area has no subscribed entities");
                return null;
            }

            entitySubscriptions.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

            //Safty check - if first subscription has default list 
            if (entitySubscriptions[0].list.name == null || entitySubscriptions[0].list.name == "") {
                $log.error(area.name + 'is not rendered, because there is no default list for the entity ' + area.entities[0].name);
                return null;
            }

            menuItem.stateParams = {
                "areaName": area.name,
                "entityName": entitySubscriptions[0].name,
                "listName": entitySubscriptions[0].list.name,
                "filter": "all",
                "page": 1
            };

        	//Roles
            menuItem.roles = angular.fromJson(area.roles);
			

            return menuItem
        }

        ////////////////

        function addItem(menuItem) {
            $log.debug('webvellaDesktop>providers>browsenav.factory>addItem> function called');
            browsenav.push(menuItem);
            browsenav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
            $rootScope.$emit('webvellaDesktop-browsenav-updated', browsenav);
            $log.debug('rootScope>events> "webvellaDesktop-browsenav-updated" emitted ' + moment().format('HH:mm:ss SSSS'));
            return browsenav
        }


        ////////////////

        function getBrowsenav() {
        	$log.debug('webvellaDesktop>providers>browsenav.factory>getBrowsenav> function called ' + moment().format('HH:mm:ss SSSS'));
            return browsenav
        }

        ////////////////

        function initBrowsenav() {
        	$log.debug('webvellaDesktop>providers>browsenav.factory>initBrowsenav> function called ' + moment().format('HH:mm:ss SSSS'));
            browsenav = [];
            return browsenav
        }
    }
})();