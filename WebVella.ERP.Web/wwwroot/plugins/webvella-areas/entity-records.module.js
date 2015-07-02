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
            url: '/:areaName/:entityName/:listName/:filter/:page', // /areas/areaName/sectionName/entityName after the parent state is prepended
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
            	resolvedListRecords: resolveListRecords
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
    resolveListRecords.$inject = ['$q', '$log', 'webvellaAreasService', '$state', '$stateParams'];

	/* @ngInject */
    function resolveListRecords($q, $log, webvellaAreasService, $state, $stateParams) {
    	$log.debug('webvellaDesktop>browse> BEGIN state.resolved');
    	// Initialize
    	var defer = $q.defer();

    	// Process
    	function successCallback(response) {
    		defer.resolve(response.object);
    	}

    	function errorCallback(response) {
    		defer.resolve(response.object);
    	}

    	webvellaAreasService.getListRecords($stateParams.listName, $stateParams.entityName, $stateParams.filter, $stateParams.page, successCallback, errorCallback);

    	// Return
    	$log.debug('webvellaDesktop>browse> END state.resolved');
    	return defer.promise;
    }
	//#endregion


    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state','$stateParams', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords'];

    /* @ngInject */
    function controller($log, $rootScope, $state,$stateParams, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords) {
        $log.debug('webvellaAreas>entities> BEGIN controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.records = angular.copy(resolvedListRecords.data);
        contentData.recordsMeta = angular.copy(resolvedListRecords.fieldsMeta);

        //#region << Set Environment >>
        contentData.pageTitle = "Area Entities | " + pageTitle;
        webvellaRootService.setPageTitle(contentData.pageTitle);
        contentData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);

        webvellaRootService.setBodyColorClass(contentData.currentArea.color);

		//Get the current entity meta
        contentData.entity = webvellaAreasService.getCurrentEntityFromArea($stateParams.entityName, contentData.currentArea);

    	//Select default details view
        contentData.defaultView = {};
        for (var i = 0; i < contentData.entity.recordViews.length; i++) {
        	if (contentData.entity.recordViews[i].default) {
        		contentData.defaultView = contentData.entity.recordViews[i];
        	}
        }
        contentData.currentPage = parseInt($stateParams.page);
    	//Select the current list view details
		//TODO needs to be implemented
        contentData.currentListView = {
        	"pageSize": 25
        }

        //#endregion


        contentData.createNewRecordModal = function () {
            var record = {};
            record.id = guid();
            record["created_by"] = "f5588278-c0a1-4865-ac94-41dfa09bf8ac";
            record["last_modified_by"] = "f5588278-c0a1-4865-ac94-41dfa09bf8ac";
            record["created_on"] = moment().toISOString();
            record["last_modified_on"] = moment().toISOString();
            webvellaAreasService.createEntityRecord(record,"test",successCallback,errorCallback)
        }

        function successCallback(response) {
        	alert("success");

        }

        function errorCallback(response) {
            alert("error");
        }

        contentData.goDesktopBrowse = function () {
        	webvellaRootService.GoToState($state, "webvella-desktop-browse", {});
        }
        contentData.selectPage = function (page, event) {
        	var params = {
        		areaName:$stateParams.areaName,
        		entityName:$stateParams.entityName,
        		listName:$stateParams.listName,
        		filter:$stateParams.filter,
        		page:page,
        	};
        	webvellaRootService.GoToState($state, $state.current.name, params);
        }

    	//1.Auto increment
        contentData.getAutoIncrementString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (!fieldValue) {
        		return "";
        	}
        	else if (fieldMeta.displayFormat) {
        		return fieldMeta.displayFormat.replace("{0}", fieldValue);
        	}
        	else {
        		return fieldValue;
        	}
        }
    	//2.Checkbox
        contentData.getCheckboxString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (fieldValue) {
        		return "true";
        	}
        	else {
        		return "false";
        	}
        }
    	//3.Currency
        contentData.getCurrencyString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (!fieldValue) {
        		return "";
        	}
        	else if (fieldMeta.currency != null && fieldMeta.currency != {} && fieldMeta.currency.symbol) {
        		if (fieldMeta.currency.symbolPlacement == 1) {
        			return fieldMeta.currency.symbol + " " + fieldValue
        		}
        		else {
        			return fieldValue + " " + fieldMeta.currency.symbol
        		}
        	}
        	else {
        		return fieldValue;
        	}
        }
    	//4.Date
        contentData.getDateString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	return moment(fieldValue).format('DD MMMM YYYY');
        }

    	//5.Datetime
        contentData.getDateTimeString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	return moment(fieldValue).format('DD MMMM YYYY HH:mm');
        }

        $log.debug('webvellaAreas>entities> END controller.exec');
    }

})();
