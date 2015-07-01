/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreasRecordViewController', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-areas-record-view', {
            parent: 'webvella-areas-base',
            url: '/:areaName/:entityName/:recordId/:viewName', // /areas/areaName/sectionName/entityName after the parent state is prepended
            views: {
                "topnavView": {
                    controller: 'WebVellaAreasTopnavController',
                    templateUrl: '/plugins/webvella-areas/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "sidebarView": {
                	controller: 'WebVellaAreasRecordViewSidebarController',
                	templateUrl: '/plugins/webvella-areas/view-record-sidebar.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                    controller: 'WebVellaAreasRecordViewController',
                    templateUrl: '/plugins/webvella-areas/record-view.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
                resolvedExtendedViewData: resolveExtendedViewData
            },
            data: {

            }
        });
    };


    // Run //////////////////////////////////////
    run.$inject = ['$log'];

    /* @ngInject */
    function run($log) {
        $log.debug('webvellaAreas>entities> BEGIN module.run');

        $log.debug('webvellaAreas>entities> END module.run');
    };


    //#region << Resolve Function >>
    resolveExtendedViewData.$inject = ['$q', '$log', 'webvellaAreasService', '$stateParams'];
    /* @ngInject */
    function resolveExtendedViewData($q, $log, webvellaAreasService, $stateParams) {
        $log.debug('webvellaAreas>entities> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();
        var record = {};
        var extendedView = {};
        //// Process
        function getRecordSuccessCallback(response) {
            record = response.object;
            //Cycle through the view, find all fields and attach their data and meta info
            for (var regionIndex = 0; regionIndex < extendedView.regions.length; regionIndex++) {
            	if (extendedView.regions[regionIndex].name == "content") {
            		for (var sectionIndex = 0; sectionIndex < extendedView.regions[regionIndex].sections.length; sectionIndex++) {
            			for (var rowIndex = 0; rowIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows.length; rowIndex++) {
            				for (var columnIndex = 0; columnIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns.length; columnIndex++) {
            					for (var itemIndex = 0; itemIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items.length; itemIndex++) {
            						for (var metaIndex = 0; metaIndex < record.fieldsMeta.length; metaIndex++) {
            							if (record.fieldsMeta[metaIndex].id === extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].fieldId) {
            								extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].meta = record.fieldsMeta[metaIndex];
            							}
            						}
            					}
            				}
            			}
            		}
            	}
            }
            extendedView.data = record.data;
            defer.resolve(extendedView);
        }

        //// Process
        function getViewSuccessCallback(response) {
            extendedView = response.object;
            webvellaAreasService.getEntityRecord($stateParams.recordId, $stateParams.entityName, getRecordSuccessCallback, errorCallback);
        }

        function errorCallback(response) {
            alert("Error getting the view");
        }

        webvellaAreasService.getViewByName($stateParams.viewName, $stateParams.entityName, getViewSuccessCallback, errorCallback);

        // Return
        $log.debug('webvellaAreas>entities> END state.resolved');
        return defer.promise;
    }

    //#endregion


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', '$scope', 'pageTitle', 'webvellaRootService', 'webvellaAdminService',
        'resolvedSitemap', '$timeout', 'resolvedExtendedViewData', 'ngToast', 'wvAppConstants'];

    /* @ngInject */
    function controller($log, $rootScope, $state,$scope, pageTitle, webvellaRootService,webvellaAdminService,
        resolvedSitemap, $timeout, resolvedExtendedViewData, ngToast, wvAppConstants) {
        $log.debug('webvellaAreas>entities> BEGIN controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        //#region <<Set pageTitle>>
        contentData.pageTitle = "Area Entities | " + pageTitle;
        webvellaRootService.setPageTitle(contentData.pageTitle);
        contentData.siteMap = angular.copy(resolvedSitemap);
        contentData.currentArea = null;
        for (var i = 0; i < contentData.siteMap.data.length; i++) {
        	if (contentData.siteMap.data[i].name == $state.params.areaName) {
        		contentData.currentArea = contentData.siteMap.data[i];
        	};
        }
        webvellaRootService.setBodyColorClass(contentData.currentArea.color);
        //#endregion

        //#region << Initialize view and regions>>
        contentData.recordView = angular.copy(resolvedExtendedViewData);
        contentData.contentRegion = null;
        contentData.sidebarRegion = null;
        for (var i = 0; i < contentData.recordView.regions.length; i++) {
            if (contentData.recordView.regions[i].name === "content") {
                contentData.contentRegion = contentData.recordView.regions[i];
            }
            else if (contentData.recordView.regions[i].name === "sidebar") {
                contentData.sidebarRegion = contentData.recordView.regions[i];
            }
        }
        contentData.viewData = contentData.recordView.data[0];
        //#endregion

    	//#region << Intialize current entity >>
        contentData.currentEntity = null;
        for (var i = 0; i < contentData.currentArea.entities.length; i++) {
        	if (contentData.currentArea.entities[i].name === $state.params.entityName) {
        		contentData.currentEntity = contentData.currentArea.entities[i];
        	}
        }
		//#endregion

        //#region << Logic >>

        contentData.toggleSectionCollapse = function (section) {
            section.collapsed = !section.collapsed;
        }

        contentData.fieldUpdate = function (item, data) {
        	data = data.toString().trim();
        	contentData.patchObject = {};
        	var validation = {
        		success: true,
				message: "successful validation"
        	};
        	switch(item.fieldTypeId) {

        		//Auto increment number
				case 1:
        		validation = checkInt(data);
        		if (!validation.success) {
        			return validation.message;
        		}
        		break;
        	}
        	contentData.patchObject[item.fieldName] = data;
        	//patchRecord(recordId, entityName, patchObject, successCallback, errorCallback)
        	webvellaAdminService.patchRecord(contentData.viewData.id, contentData.currentEntity.name, contentData.patchObject, patchSuccessCallback, patchFailedCallback);
        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
        	});
        	return true;
        }
        function patchFailedCallback(response) {
        	ngToast.create({
        		className: 'error',
        		content: '<span class="go-red">Error:</span> ' + response.message
        	});
        	return false;
        }

    	//Auto increment
        contentData.getAutoIncrementString = function (item) {
        	var fieldValue = contentData.viewData[item.fieldName];
        	if (!fieldValue) {
        		return "empty";
        	}
        	else if (item.meta.displayFormat) {
        		return item.meta.displayFormat.replace("{0}", fieldValue);
        	}
        	else {
        		return fieldValue;
        	}
        }

    	//Checkbox
        contentData.getCheckboxString = function (item) {
        	var fieldValue = contentData.viewData[item.fieldName];
        	if (fieldValue) {
        		return "true";
        	}
        	else {
        		return "false";
        	}
        }

    	//DateTime 
        $scope.picker = { opened: false };

        $scope.openPicker = function () {
        	$timeout(function () {
        		$scope.picker.opened = true;
        	});
        };

        $scope.closePicker = function () {
        	$scope.picker.opened = false;
        };
    	//#endregion

        $log.debug('webvellaAreas>entities> END controller.exec');
    }

})();
