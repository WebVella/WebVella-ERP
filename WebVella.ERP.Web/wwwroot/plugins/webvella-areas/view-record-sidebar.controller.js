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
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedParentViewData','resolvedCurrentView', 'resolvedCurrentEntityMeta', 
						'resolvedAreas', 'resolvedCurrentUser', '$sessionStorage','$timeout','webvellaCoreService','resolvedEntityList'];

    
    function controller($log, $rootScope, $state, $stateParams,resolvedParentViewData, resolvedCurrentView, resolvedCurrentEntityMeta, 
						resolvedAreas, resolvedCurrentUser, $sessionStorage,$timeout,webvellaCoreService,resolvedEntityList) {
        var sidebarData = this;
		if(resolvedCurrentView == null){
			sidebarData.view = null; //list in view page		
		}
		else {
			sidebarData.view = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.viewName,$stateParams.entityName,resolvedEntityList);
		}
		if(resolvedParentViewData == null){
		   sidebarData.parentView = null;
		}
		else{
			sidebarData.parentView = webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.parentViewName,$stateParams.entityName,resolvedEntityList);
		}
        sidebarData.stateParams = fastCopy($stateParams);
        sidebarData.entity = fastCopy(resolvedCurrentEntityMeta);
        sidebarData.currentUser = fastCopy(resolvedCurrentUser);
		sidebarData.$sessionStorage	= $sessionStorage;

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
		if(resolvedParentViewData == null){
			var generalItem = {
				is_parent: true,
				parentViewName: null,
        		name: sidebarData.view.name,
        		label: sidebarData.view.label,
        		iconName: sidebarData.view.iconName,
				type:"view"
			};
			sidebarData.items.push(generalItem);

			sidebarData.view.sidebar.items.forEach(function(sidebarItem){
        		var item = {};
        		item.name = sidebarItem.dataName;
        		item.label = sidebarItem.fieldLabel;
				item.is_parent = false;
				item.type = sidebarItem.type;
				item.parentViewName = sidebarData.view.name;
        		if (sidebarItem.type === "view" || sidebarItem.type === "viewFromRelation") {
					if(sidebarItem.type === "view"){
						 item.label = sidebarItem.meta.label;
					}
        			item.iconName = "file-text-o";
        			if (sidebarItem.meta.iconName) {
        				item.iconName =sidebarItem.meta.iconName;
        			}
        		}
        		else if (sidebarItem.type === "list" || sidebarItem.type === "listFromRelation") {
					if(sidebarItem.type === "list"){
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
        		label: sidebarData.parentView.label,
        		iconName: sidebarData.parentView.iconName,
				type:"view"
			};
			sidebarData.items.push(generalItem);

			sidebarData.parentView.sidebar.items.forEach(function(sidebarItem){
        		var item = {};
        		item.name = sidebarItem.dataName;
        		item.label = sidebarItem.fieldLabel;
				item.is_parent = false;
				item.type = sidebarItem.type;
				item.parentViewName = sidebarData.parentView.name;
        		if (sidebarItem.type === "view" || sidebarItem.type === "viewFromRelation") {
					if(sidebarItem.type === "view"){
						 item.label = sidebarItem.meta.label;
					}
        			item.iconName = "file-text-o";
        			if (sidebarItem.meta.iconName) {
        				item.iconName =sidebarItem.meta.iconName;
        			}
        		}
        		else if (sidebarItem.type === "list" || sidebarItem.type === "listFromRelation") {
					if(sidebarItem.type === "list"){
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
			if(item.type == "view"){
				if(item.is_parent && sidebarData.view != null &&  item.name == $stateParams.viewName ){ //the main details
					return true;
				} 
				else if(item.name == $stateParams.viewName)	{
					return true;
				}
				else {
					return false;
				}
			}
			if(item.type == "viewFromRelation"){
				if(item.name == $stateParams.viewName){
					return true;
				} 
				else {
					return false;
				}
			}
			if(item.type == "list"){
				if(item.name == $stateParams.listName){
					return true;
				} 
				else {
					return false;
				}
			}
			if(item.type == "listFromRelation"){
				if(item.name == $stateParams.listName){
					return true;
				} 
				else {
					return false;
				}
			}
        	return false;
        }

		sidebarData.goBack = function(){
			var useSessionBackUrl = false;
			if(sidebarData.$sessionStorage["last-list-params"]){
				//Check if the entity and the list name are the same
				var storedParams = sidebarData.$sessionStorage["last-list-params"];
				if(storedParams.areaName == sidebarData.stateParams.areaName &&  storedParams.entityName == sidebarData.stateParams.entityName){
					 useSessionBackUrl = true;
				}
			}
			
			if(useSessionBackUrl){
				$timeout(function () {
                    $state.go('webvella-area-list-general',sidebarData.$sessionStorage["last-list-params"]);
                }, 0);
			}
			else {
				var defaultParams = {};
				defaultParams.areaName = sidebarData.stateParams.areaName;
				defaultParams.entityName = sidebarData.stateParams.entityName;
				defaultParams.listName = sidebarData.defaultEntityAreaListName;
				defaultParams.page = 1;

			   	$timeout(function () {
                    $state.go('webvella-area-list-general',defaultParams);
                }, 0);
			}
			//href="#/areas/{{::sidebarData.stateParams.areaName}}/{{::sidebarData.stateParams.entityName}}/{{::sidebarData.defaultEntityAreaListName}}/1"
		}
    }

})();
