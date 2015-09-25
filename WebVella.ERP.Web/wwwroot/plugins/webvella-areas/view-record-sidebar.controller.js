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
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedCurrentView', 'resolvedCurrentEntityMeta', 'resolvedSitemap', 'resolvedCurrentUser'];

    /* @ngInject */
    function controller($log, $rootScope, $state, $stateParams, resolvedCurrentView, resolvedCurrentEntityMeta, resolvedSitemap, resolvedCurrentUser) {
    	$log.debug('webvellaAreas>sidebar> BEGIN controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var sidebarData = this;
        sidebarData.view = resolvedCurrentView.meta;
        sidebarData.stateParams = $stateParams;
        sidebarData.entity = resolvedCurrentEntityMeta;
        sidebarData.currentUser = angular.copy(resolvedCurrentUser);
    	//#region << Select default list >>
        sidebarData.defaultEntityAreaListName = "";
    	//get the current area meta
	    for (var j = 0; j < resolvedSitemap.data.length; j++) {
	    	if (resolvedSitemap.data[j].name === $stateParams.areaName) {
	    		var areaSubscriptions = angular.fromJson(resolvedSitemap.data[j].subscriptions);
	    		for (var k = 0; k < areaSubscriptions.length; k++) {
				    if (areaSubscriptions[k].name === $stateParams.entityName) {
					    sidebarData.defaultEntityAreaListName = areaSubscriptions[k].list.name;
				    }
			    }
		    }
	    }
    	//convert stringified subscriptions to object and cycle and find the current entity

		//get the selected list name

    	//#endregion

    	//Generate menu items list
        sidebarData.items = [];
        var generalItem = {
        	name: "*",
        	label: "General",
        	iconName: "info-circle"
        };
        sidebarData.items.push(generalItem);

        for (var i = 0; i < sidebarData.view.sidebar.items.length; i++) {
        	var item = {};
        	item.name = sidebarData.view.sidebar.items[i].meta.name;
        	item.label = sidebarData.view.sidebar.items[i].meta.label;
        	if (sidebarData.view.sidebar.items[i].type === "view" || sidebarData.view.sidebar.items[i].type === "viewFromRelation") {
        		item.iconName = "file-text-o";
        	}
        	else if (sidebarData.view.sidebar.items[i].type === "list" || sidebarData.view.sidebar.items[i].type === "listFromRelation") {
        		item.iconName = "list";
        	}
        	sidebarData.items.push(item);
        }

        $log.debug('webvellaAreas>sidebar> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }

})();
