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
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedCurrentView', 'resolvedCurrentEntityMeta', 
						'resolvedSitemap', 'resolvedCurrentUser', 'pluginAuxPageName','$sessionStorage','$timeout'];

    /* @ngInject */
    function controller($log, $rootScope, $state, $stateParams, resolvedCurrentView, resolvedCurrentEntityMeta, 
						resolvedSitemap, resolvedCurrentUser, pluginAuxPageName,$sessionStorage,$timeout) {
    	$log.debug('webvellaAreas>sidebar> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var sidebarData = this;
        sidebarData.view = resolvedCurrentView.meta;
        sidebarData.stateParams = $stateParams;
        sidebarData.entity = resolvedCurrentEntityMeta;
        sidebarData.currentUser = angular.copy(resolvedCurrentUser);
		sidebarData.$sessionStorage	= $sessionStorage;

    	//#region << Select default list >>
        sidebarData.defaultEntityAreaListName = "";
    	//get the current area meta
	    for (var j = 0; j < resolvedSitemap.data.length; j++) {
	    	if (resolvedSitemap.data[j].name === $stateParams.areaName) {
	    		var areaAttachments = angular.fromJson(resolvedSitemap.data[j].subscriptions);
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
        var generalItem = {
        	name: "*",
        	label: "Details",
        	iconName: "info-circle"
        };
        sidebarData.items.push(generalItem);

        for (var i = 0; i < sidebarData.view.sidebar.items.length; i++) {
        	var item = {};
        	item.name = sidebarData.view.sidebar.items[i].dataName;
        	item.label = sidebarData.view.sidebar.items[i].meta.label;
        	if (sidebarData.view.sidebar.items[i].type === "view" || sidebarData.view.sidebar.items[i].type === "viewFromRelation") {
        		item.iconName = "file-text-o";
        		if (sidebarData.view.sidebar.items[i].meta.iconName) {
        			item.iconName = sidebarData.view.sidebar.items[i].meta.iconName;
        		}
        	}
        	else if (sidebarData.view.sidebar.items[i].type === "list" || sidebarData.view.sidebar.items[i].type === "listFromRelation") {
        		item.iconName = "list";
        		if (sidebarData.view.sidebar.items[i].meta.iconName) {
        			item.iconName = sidebarData.view.sidebar.items[i].meta.iconName;
        		}
        	}
        	sidebarData.items.push(item);
        }

        sidebarData.isItemActive = function (item) {
        	if (!$stateParams.auxPageName) {
        		if (item.name == pluginAuxPageName) {
        		return true;
        		}
        	}
        	if (item.name == $stateParams.auxPageName) {
        		return true;
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
                    $state.go('webvella-entity-records',sidebarData.$sessionStorage["last-list-params"]);
                }, 0);
			}
			else {
				var defaultParams = {};
				defaultParams.areaName = sidebarData.stateParams.areaName;
				defaultParams.entityName = sidebarData.stateParams.entityName;
				defaultParams.listName = sidebarData.defaultEntityAreaListName;
				defaultParams.filter = "all";
				defaultParams.page = 1;
				defaultParams.search = null;

			   	$timeout(function () {
                    $state.go('webvella-entity-records',defaultParams);
                }, 0);
			}
			//href="#/areas/{{::sidebarData.stateParams.areaName}}/{{::sidebarData.stateParams.entityName}}/{{::sidebarData.defaultEntityAreaListName}}/all/1"
		}


        $log.debug('webvellaAreas>sidebar> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

})();
