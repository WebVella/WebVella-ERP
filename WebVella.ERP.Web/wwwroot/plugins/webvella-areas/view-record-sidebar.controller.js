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
    controller.$inject = ['$log', '$rootScope', '$state', '$stateParams', 'resolvedExtendedViewData', 'resolvedCurrentEntityMeta', 'webvellaAreasService'];

    /* @ngInject */
    function controller($log, $rootScope, $state, $stateParams, resolvedExtendedViewData,resolvedCurrentEntityMeta, webvellaAreasService) {
        $log.debug('webvellaAreas>sidebar> BEGIN controller.exec');
        /* jshint validthis:true */
        var sidebarData = this;
        sidebarData.view = resolvedExtendedViewData;
        sidebarData.stateParams = $stateParams;
        sidebarData.entity = resolvedCurrentEntityMeta;
    	//Select default list
        sidebarData.defaultList = {};
        for (var i = 0; i < sidebarData.entity.recordLists.length; i++) {
        	if (sidebarData.entity.recordLists[i].default) {
        		sidebarData.defaultList = sidebarData.entity.recordLists[i];
        		break;
        	}
        }
        sidebarData.stateParams.listName = sidebarData.defaultList.name;

    	//Generate menu items list
        sidebarData.items = [];
        var generalItem = {
        	name: "$",
        	label: "General",
        	iconName: "info-circle"
        };
        sidebarData.items.push(generalItem);

        for (var i = 0; i < sidebarData.view.sidebar.items.length; i++) {
        	var item = {};
        	if (sidebarData.view.sidebar.items[i].type == "view" || sidebarData.view.sidebar.items[i].type == "viewFromRelation") {
        		item.name = sidebarData.view.sidebar.items[i].viewName;
        		item.label = sidebarData.view.sidebar.items[i].viewLabel;
        		item.iconName = "file-text-o";
        	}
        	else if (sidebarData.view.sidebar.items[i].type == "list" || sidebarData.view.sidebar.items[i].type == "listFromRelation") {
        		item.name = sidebarData.view.sidebar.items[i].listName;
        		item.label = sidebarData.view.sidebar.items[i].listLabel;
        		item.iconName = "list";
        	}
        	sidebarData.items.push(item);
        }

        $log.debug('webvellaAreas>sidebar> END controller.exec');
    }

})();
