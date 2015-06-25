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
            url: '/:areaName/:entityName/:viewName', // /areas/areaName/sectionName/entityName after the parent state is prepended
            views: {
                "topnavView": {
                    controller: 'WebVellaAreasTopnavController',
                    templateUrl: '/plugins/webvella-areas/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "sidebarView": {
                    controller: 'WebVellaAreasSidebarController',
                    templateUrl: '/plugins/webvella-areas/sidebar.view.html',
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
                for (var sectionIndex = 0; sectionIndex < extendedView.regions[regionIndex].sections.length; sectionIndex++) {
                    for (var rowIndex = 0; rowIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows.length; rowIndex++) {
                        for (var columnIndex = 0; columnIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns.length; columnIndex++) {
                            for (var itemIndex = 0; itemIndex < extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items.length; itemIndex++) {
                                for (var metaIndex = 0; metaIndex < record.fieldsMeta.length; metaIndex++) {
                                    if (record.fieldsMeta[metaIndex].id === extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].id) {
                                        extendedView.regions[regionIndex].sections[sectionIndex].rows[rowIndex].columns[columnIndex].items[itemIndex].meta = record.fieldsMeta[metaIndex];
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
            webvellaAreasService.getEntityRecord("121212", "area", getRecordSuccessCallback, errorCallback);
        }

        function errorCallback(response) {
            alert("Error getting the view");
        }

        webvellaAreasService.getViewMetaByName("area", "area", getViewSuccessCallback, errorCallback);

        // Return
        $log.debug('webvellaAreas>entities> END state.resolved');
        return defer.promise;
    }

    //#endregion


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'resolvedExtendedViewData'];

    /* @ngInject */
    function controller($log, $rootScope, $state, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, resolvedExtendedViewData) {
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


        //#region << logic >>

        contentData.toggleSectionCollapse = function (section) {
            section.collapsed = !section.collapsed;
        }

        //#endregion

        $log.debug('webvellaAreas>entities> END controller.exec');
    }

})();
