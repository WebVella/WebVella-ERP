/* sidebar.controller.js */

/**
* @desc this controller manages the sidebar of the areas section
*/

(function () {
	'use strict';

	angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .controller('WebVellaAreasRecordViewSidebarController', controller);


	// Controller ///////////////////////////////
	controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedParentViewData', 'resolvedCurrentViewData', 'resolvedCurrentEntityMeta',
						'resolvedAreas', 'resolvedCurrentUser', '$sessionStorage', '$timeout', 'webvellaCoreService', 'resolvedEntityList','$location','$localStorage'];


	function controller($log, $rootScope, $state, $stateParams, resolvedParentViewData, resolvedCurrentViewData, resolvedCurrentEntityMeta,
						resolvedAreas, resolvedCurrentUser, $sessionStorage, $timeout, webvellaCoreService, resolvedEntityList,$location,$localStorage) {
		var sidebarData = this;
		sidebarData.viewData = null;
		if (resolvedCurrentViewData == null) {
			sidebarData.view = null; //list in view page		
		}
		else {
			sidebarData.view = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.viewName, $stateParams.entityName, resolvedEntityList);
			sidebarData.viewData = resolvedCurrentViewData[0];
		}
		if (resolvedParentViewData == null) {
			sidebarData.parentView = null;
		}
		else {
			sidebarData.viewData = resolvedParentViewData[0];
			sidebarData.parentView = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName, $stateParams.entityName, resolvedEntityList);
		}
		sidebarData.stateParams = $stateParams;
		sidebarData.entity = resolvedCurrentEntityMeta;
		sidebarData.currentUser = resolvedCurrentUser;
		sidebarData.$sessionStorage = $sessionStorage;

		//#region << Select default list >>
		sidebarData.defaultEntityAreaListName = "";
		//get the current area meta
		for (var j = 0; j < resolvedAreas.data.length; j++) {
			if (resolvedAreas.data[j].name === $stateParams.areaName) {
				var areaAttachments = angular.fromJson(resolvedAreas.data[j].attachments);
				for (var k = 0; k < areaAttachments.length; k++) {
					if (areaAttachments[k].name === $stateParams.entityName) {
						sidebarData.defaultEntityAreaListName = areaAttachments[k].list.name;
					}
				}
			}
		}
		//#endregion

		//Generate menu items list
		sidebarData.items = [];
		sidebarData.sidebarTopActions = [];
		if (resolvedParentViewData == null) {
			var generalItem = {
				is_parent: true,
				parentViewName: null,
				name: sidebarData.view.name,
				label: webvellaCoreService.generateHighlightString(sidebarData.view,sidebarData.viewData,sidebarData.stateParams,"label"),
				iconName: sidebarData.view.iconName,
				type: "view"
			};
			sidebarData.items.push(generalItem);

			sidebarData.view.sidebar.items.forEach(function (sidebarItem) {
				var item = {};
				item.name = sidebarItem.dataName;
				item.label = sidebarItem.fieldLabel;
				item.is_parent = false;
				item.type = sidebarItem.type;
				item.parentViewName = sidebarData.view.name;
				if (sidebarItem.type === "view" || sidebarItem.type === "viewFromRelation") {
					if (sidebarItem.type === "view") {
						item.label = sidebarItem.meta.label;
					}
					item.iconName = "file-text-o";
					if (sidebarItem.meta.iconName) {
						item.iconName = sidebarItem.meta.iconName;
					}
				}
				else if (sidebarItem.type === "list" || sidebarItem.type === "listFromRelation") {
					if (sidebarItem.type === "list") {
						item.label = sidebarItem.meta.label;
					}
					item.iconName = "list";
					if (sidebarItem.meta.iconName) {
						item.iconName = sidebarItem.meta.iconName;
					}
				}
				sidebarData.items.push(item);

			});

			sidebarData.view.actionItems.forEach(function (actionItem) {
				switch (actionItem.menu) {
					case "sidebar-top":
						sidebarData.sidebarTopActions.push(actionItem);
						break;
				}
			});

		}
		else {
			var generalItem = {
				is_parent: true,
				name: sidebarData.parentView.name,
				parentViewName: null,
				label: webvellaCoreService.generateHighlightString(sidebarData.parentView,sidebarData.viewData,sidebarData.stateParams,"label"),
				iconName: sidebarData.parentView.iconName,
				type: "view"
			};
			sidebarData.items.push(generalItem);

			sidebarData.parentView.sidebar.items.forEach(function (sidebarItem) {
				var item = {};
				item.name = sidebarItem.dataName;
				item.label = sidebarItem.fieldLabel;
				item.is_parent = false;
				item.type = sidebarItem.type;
				item.parentViewName = sidebarData.parentView.name;
				if (sidebarItem.type === "view" || sidebarItem.type === "viewFromRelation") {
					if (sidebarItem.type === "view") {
						item.label = sidebarItem.meta.label;
					}
					item.iconName = "file-text-o";
					if (sidebarItem.meta.iconName) {
						item.iconName = sidebarItem.meta.iconName;
					}
				}
				else if (sidebarItem.type === "list" || sidebarItem.type === "listFromRelation") {
					if (sidebarItem.type === "list") {
						item.label = sidebarItem.meta.label;
					}
					item.iconName = "list";
					if (sidebarItem.meta.iconName) {
						item.iconName = sidebarItem.meta.iconName;
					}
				}
				sidebarData.items.push(item);

			});

			sidebarData.parentView.actionItems.forEach(function (actionItem) {
				switch (actionItem.menu) {
					case "sidebar-top":
						sidebarData.sidebarTopActions.push(actionItem);
						break;
				}
			});

		}

		sidebarData.isItemActive = function (item) {
			if (item.type == "view") {
				if (item.is_parent && sidebarData.view != null && item.name == $stateParams.viewName) { //the main details
					return true;
				}
				else if (item.name == $stateParams.viewName) {
					return true;
				}
				else {
					return false;
				}
			}
			if (item.type == "viewFromRelation") {
				if (item.name == $stateParams.viewName) {
					return true;
				}
				else {
					return false;
				}
			}
			if (item.type == "list") {
				if (item.name == $stateParams.listName) {
					return true;
				}
				else {
					return false;
				}
			}
			if (item.type == "listFromRelation") {
				if (item.name == $stateParams.listName) {
					return true;
				}
				else {
					return false;
				}
			}
			return false;
		}

		sidebarData.goBack = function () {

			var useSessionBackUrl = false;
			if ($stateParams.returnUrl) {
				var returnUrl = decodeURI($stateParams.returnUrl);
				
				delete sidebarData.$sessionStorage["last-list-params"];
				$location.search("returnUrl", null);
				$location.path(returnUrl);
			}
			else {
				if (sidebarData.$sessionStorage["last-list-params"]) {
					//Check if the entity and the list name are the same
					var storedParams = sidebarData.$sessionStorage["last-list-params"];
					//if (storedParams.areaName == sidebarData.stateParams.areaName && storedParams.entityName == sidebarData.stateParams.entityName) {
						useSessionBackUrl = true;
					//}
				}

				if (useSessionBackUrl) {
					$timeout(function () {
						$state.go('webvella-area-list-general', sidebarData.$sessionStorage["last-list-params"]);
					}, 0);
				}
				else {
					var defaultParams = {};
					defaultParams.areaName = sidebarData.stateParams.areaName;
					defaultParams.entityName = sidebarData.stateParams.entityName;
					defaultParams.listName = sidebarData.defaultEntityAreaListName;
					defaultParams.page = 1;

					$timeout(function () {
						$state.go('webvella-area-list-general', defaultParams);
					}, 0);
				}
			}

		}

        sidebarData.$storage = $localStorage;
        sidebarData.toggleSideNav = function () {
        	sidebarData.$storage.isMiniSidebar = !sidebarData.$storage.isMiniSidebar;
        }

		sidebarData.generateHighlightString = function(item){
			  return webvellaCoreService.generateHighlightString(item,sidebarData.viewData,sidebarData.stateParams,"label");
		}

		sidebarData.setCurrentBreadcrumb = function(hoveredSidebarMenuLabel){
			if(sidebarData.$storage.isMiniSidebar){
				$rootScope.hoveredSidebarMenuLabel = hoveredSidebarMenuLabel;
			}
		}

	}

})();
