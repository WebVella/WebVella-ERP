//#region << topnav >>

/**
* @desc factory for managing the desktop topnav object
*/

(function () {
    'use strict';
    angular
        .module('webvellaDesktop')
        .factory('webvellaDesktopTopnavFactory', factory);

    factory.$inject = ['$log','$rootScope','$timeout','webvellaCoreService'];
    function factory($log,$rootScope,$timeout,webvellaCoreService) {
        var topnav = [];
        var exports = {
            initTopnav:initTopnav,
            addItem: addItem,
            getTopnav: getTopnav
        };
        return exports;

        ////////////////

        function initTopnav() {
            topnav = [];
            return topnav;
        }

        function addItem(item) {
			//check if user has permission to view the nodes of the item
			var currentUser = webvellaCoreService.getCurrentUser();
			var permittedNodes = [];
			for (var j = 0; j < item.nodes.length; j++) {
    			var node = item.nodes[j];
				var nodeRoles = angular.fromJson(item.nodes[j].roles);
				var nodeCanBeViewed = false;

				for (var i = 0; i < nodeRoles.length; i++) {
					var roleId = nodeRoles[i];
					if(currentUser.roles.indexOf(roleId) != -1){
						nodeCanBeViewed = true;
						break;
					}
				}
				if(nodeCanBeViewed){
					permittedNodes.push(node);
				}

			};			

			//Check if not Admin area as it has no nodes
			var isAdminAndUserIsAdmin = false;
			if(item.type == "admin" && currentUser.roles.indexOf("bdc56420-caf0-4030-8a0e-d264938e0cda") != -1){
				isAdminAndUserIsAdmin = true;
			}

			if(permittedNodes.length > 0 || isAdminAndUserIsAdmin){
			item.nodes =  permittedNodes;
			//check label is not already added
			var navLabelAlreadyAdded = false;
        	for (var i = 0; i < topnav.length; i++) {
				 if(topnav[i].label === item.label){
				 	 navLabelAlreadyAdded = true;
				 }
        	}
			if(!navLabelAlreadyAdded){
				topnav.push(item);
				topnav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
				$timeout(function(){
					$rootScope.$emit('webvellaDesktop-topnav-updated', topnav)
				},0);
			}

			}
        }


        function getTopnav() {
			console.log(topnav);
            return topnav;
        }
    }
})();

//#endregion

//#region << browsenav >>

/**
* @desc factory for managing and generating the Desktop browse section menu / icons
*/

(function () {
    'use strict';
    angular
        .module('webvellaDesktop')
        .factory('webvellaDesktopBrowsenavFactory', factory);

    factory.$inject = ['$log', '$rootScope','$timeout'];

    function factory($log, $rootScope, $timeout) {
        var browsenav = [];

        var exports = {
            generateMenuItemFromArea: generateMenuItemFromArea,
            addItem: addItem,
            getBrowsenav: getBrowsenav,
            initBrowsenav: initBrowsenav
        };
        return exports;

        ////////////////
        function generateMenuItemFromArea(area) {
            //Redirect to the first entity of the area
            var menuItem = {};
            menuItem.label = area.label;
            menuItem.weight = area.weight;
			menuItem.url = null;
            menuItem.color = area.color;
            menuItem.iconName = area.icon_name;
			menuItem.folder = area.folder;
            menuItem.stateName = "webvella-area-list-general";
            var areaAttachments = angular.fromJson(area.attachments);
            //Safety check - if area has attachments. 
            if (areaAttachments == null || areaAttachments.length == 0) {
                return null;
            }

            areaAttachments.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight) });

            //If attachments has not url (it is an entity)
			if(areaAttachments[0].url == null){
				//Safety check - if first subscription has default list 
				if (areaAttachments[0].list == null || areaAttachments[0].list.name == null || areaAttachments[0].list.name == "") {
					$log.error(area.name + 'is not rendered, because there is no default list for the entity ' + area.entities[0].name);
					return null;
				}

				menuItem.stateParams = {
					"areaName": area.name,
					"entityName": areaAttachments[0].name,
					"listName": areaAttachments[0].list.name,
					"page": 1
				};
			}
			else {
				menuItem.url = areaAttachments[0].url;
				menuItem.stateName = "url";		
				menuItem.stateParams = {};
			}

        	//Roles
            menuItem.roles = angular.fromJson(area.roles);
            return menuItem
        }
        ////////////////
        function addItem(menuItem) {
            browsenav.push(menuItem);
            browsenav.sort(function (a, b) { return parseFloat(a.weight) - parseFloat(b.weight); });
			$timeout(function(){
				$rootScope.$emit('webvellaDesktop-browsenav-updated', browsenav);
			},0);
            return browsenav
        }
        ////////////////
        function getBrowsenav() {
            return browsenav
        }
        ////////////////
        function initBrowsenav() {
            browsenav = [];
            return browsenav
        }
    }
})();

//#endregion