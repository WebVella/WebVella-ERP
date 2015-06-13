/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreaEntityRecordsontroller', controller);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-entity-records', {
            parent: 'webvella-areas-base',
            url: '/:areaName/:entityName', // /areas/areaName/sectionName/entityName after the parent state is prepended
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
                    controller: 'WebVellaAreaEntityRecordsontroller',
                    templateUrl: '/plugins/webvella-areas/entity-records.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
            	//resolvedSitemap: resolveSitemap - from the parent
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


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state','$stateParams', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService'];

    /* @ngInject */
    function controller($log, $rootScope, $state,$stateParams, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, webvellaAreasService) {
        $log.debug('webvellaAreas>entities> BEGIN controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        //#region << Set Environment >>
        contentData.pageTitle = "Area Entities | " + pageTitle;
        webvellaRootService.setPageTitle(contentData.pageTitle);
        contentData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);
        webvellaRootService.setBodyColorClass(contentData.currentArea.color);

        contentData.entity = webvellaAreasService.getCurrentEntityFromArea($stateParams.entityName, contentData.currentArea);
        //#endregion


        contentData.createNewRecordModal = function () {
            var record = {};
            record.id = guid();
            record["name"] = "sales";
            record["created_by"] = "f5588278-c0a1-4865-ac94-41dfa09bf8ac";
            record["last_modified_by"] = "f5588278-c0a1-4865-ac94-41dfa09bf8ac";
            record["created_on"] = moment().toISOString();
            record["color"] = "green";
            record["icon_name"] = "money";
            record["label"] = "Sales";
            record["weight"] = 1.0;
            webvellaAreasService.createEntityRecord(record,"area",successCallback,errorCallback)
        }

        function successCallback(response) {
            alert("success");
        }

        function errorCallback(response) {
            alert("error");
        }

        contentData.goDesktopBrowse = function () {
            $timeout(function () {
                $state.go("webvella-desktop-browse");
            }, 0);
            
        }


        $log.debug('webvellaAreas>entities> END controller.exec');
    }

})();
