/* entities.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAreas') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAreaEntityRecordsontroller', controller)
		.controller('createRecordModalController', createRecordModalController);
   

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
                resolvedListRecords: resolveListRecords,
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedCurrentArea: resolveCurrentArea
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

    resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
    /* @ngInject */
    function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $state, $stateParams) {
        $log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved');
        return defer.promise;
    }


    resolveCurrentArea.$inject = ['$q', '$log', 'webvellaAdminService', '$state', '$stateParams'];
    /* @ngInject */
    function resolveCurrentArea($q, $log, webvellaAdminService, $state, $stateParams) {
        $log.debug('webvellaDesktop>resolveCurrentEntityMeta> BEGIN state.resolved');
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
            defer.resolve(response.object);
        }

        webvellaAdminService.getAreaByName($stateParams.areaName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaDesktop>resolveCurrentEntityMeta> END state.resolved');
        return defer.promise;
    }

	//#endregion


    // Controller ///////////////////////////////
    controller.$inject = ['$filter', '$log', '$modal', '$rootScope', '$state', '$stateParams', 'pageTitle', 'webvellaRootService',
        'resolvedSitemap', '$timeout', 'webvellaAreasService', 'resolvedListRecords', 'resolvedCurrentEntityMeta', 'resolvedCurrentArea'];

    /* @ngInject */
    function controller($filter,$log,$modal, $rootScope, $state, $stateParams, pageTitle, webvellaRootService,
        resolvedSitemap, $timeout, webvellaAreasService, resolvedListRecords, resolvedCurrentEntityMeta, resolvedCurrentArea) {
        $log.debug('webvellaAreas>entities> BEGIN controller.exec');
        /* jshint validthis:true */
        var contentData = this;
        contentData.records = angular.copy(resolvedListRecords.data);
        contentData.recordsMeta = angular.copy(resolvedListRecords.meta);

        //#region << Set Environment >>
        contentData.pageTitle = "Area Entities | " + pageTitle;
        webvellaRootService.setPageTitle(contentData.pageTitle);
        contentData.currentArea = webvellaAreasService.getCurrentAreaFromSitemap($stateParams.areaName, resolvedSitemap.data);

        webvellaRootService.setBodyColorClass(contentData.currentArea.color);

		//Get the current meta
        contentData.entity = angular.copy(resolvedCurrentEntityMeta);
        contentData.area = angular.copy(resolvedCurrentArea.data[0]);
        contentData.area.subscriptions = angular.fromJson(contentData.area.subscriptions);
        contentData.areaEntitySubscription = {};
        for (var i = 0; i < contentData.area.subscriptions.length; i++) {
            if (contentData.area.subscriptions[i].name == contentData.entity.name) {
                contentData.areaEntitySubscription = contentData.area.subscriptions[i];
                break;
            }
        }


    	//Select default details view
        contentData.selectedView = {};
        for (var i = 0; i < contentData.entity.recordViews.length; i++) {
            if (contentData.entity.recordViews[i].name == contentData.areaEntitySubscription.view.name) {
                contentData.selectedView = contentData.entity.recordViews[i];
        		break;
        	}
        }
        contentData.currentPage = parseInt($stateParams.page);
    	//Select the current list view details
        contentData.currentListView = {};
        for (var i = 0; i < contentData.entity.recordLists.length; i++) {
        	if (contentData.entity.recordLists[i].name == $stateParams.listName) {
        		contentData.currentListView = contentData.entity.recordLists[i];
        	}
        }

    	//#endregion

    	//Create new record modal
        contentData.createRecordModal = function () {
        	var modalInstance = $modal.open({
        		animation: false,
        		templateUrl: 'createRecordModal.html',
        		controller: 'createRecordModalController',
        		controllerAs: "popupData",
        		size: "",
        		resolve: {
        			parentData: function () { return contentData; }
        		}
        	});
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
    	//6.Email
        contentData.getEmailString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (fieldValue) {
        		return "<a href='mailto:" + fieldValue + "' target='_blank'>" + fieldValue + "</a>";
        	}
        	else {
        		return "";
        	}
        }
    	//7.File
        contentData.getFileString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (fieldValue) {
        		return "<a href='"+fieldValue+"' taget='_blank' class='link-icon'>view file</a>";
        	}
        	else {
        		return "";
        	}
        }
    	//8.Html
        contentData.getHtmlString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (fieldValue) {
        		return fieldValue;
        	}
        	else {
        		return "";
        	}
        }
    	//9.Image
        contentData.getImageString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (fieldValue) {
        		return "<img src='" + fieldValue + "' class='table-image'/>";
        	}
        	else {
        		return "";
        	}
        }
    	//11.Multiselect
        contentData.getMultiselectString = function (record, fieldMeta) {
        	var fieldValueArray = record[fieldMeta.name];
        	var generatedStringArray = [];
        	if (fieldValueArray.length == 0) {
        		return "";
        	}
        	else {
        		for (var i = 0; i < fieldValueArray.length; i++) {
        			var selected = $filter('filter')(fieldMeta.options, { key: fieldValueArray[i] });
        			generatedStringArray.push((fieldValueArray[i] && selected.length) ? selected[0].value : 'empty');
        		}
        		return generatedStringArray.join(', ');

        	}

        }
    	//14.Percent
        contentData.getPercentString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (!fieldValue) {
        		return "";
        	}
        	else {
        		return fieldValue * 100 + "%";
        	}
        }
    	//15.Phone
        contentData.getPhoneString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (!fieldValue) {
        		return "";
        	}
        	else {
        		return phoneUtils.formatInternational(fieldValue);
        	}
        }
    	//17.Dropdown
        contentData.getDropdownString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (!fieldValue) {
        		return "";
        	}
        	else {
        		var selected = $filter('filter')(fieldMeta.options, { key: fieldValue });
        		return (fieldValue && selected.length) ? selected[0].value : 'empty';
        	}

        }
    	//18.Url
        contentData.getUrlString = function (record, fieldMeta) {
        	var fieldValue = record[fieldMeta.name];
        	if (fieldValue) {
        		return "<a href='" + fieldValue + "' target='_blank'>" + fieldValue + "</a>";
        	}
        	else {
        		return "";
        	}
        }
        $log.debug('webvellaAreas>entities> END controller.exec');
    }


	//// Modal Controllers
    createRecordModalController.$inject = ['$stateParams','parentData', '$modalInstance', '$log', 'webvellaAreasService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

	/* @ngInject */
    function createRecordModalController($stateParams,parentData, $modalInstance, $log, webvellaAreasService, webvellaRootService, ngToast, $timeout, $state) {
    	$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec');
    	/* jshint validthis:true */
    	var popupData = this;
    	popupData.parentData = parentData;
    	popupData.entityMeta = parentData.entity;
    	popupData.ok = function () {
    		var response = {};
    		response.message = "All fine";
    		successCallback(response);
    		//webvellaAreasService.createEntityRecord(popupData.record, popupData.parentData.entity.name, successCallback, errorCallback);
    	};

    	popupData.cancel = function () {
    		$modalInstance.dismiss('cancel');
    	};

    	/// Aux
    	function successCallback(response) {
    		ngToast.create({
    			className: 'success',
    			content: '<span class="go-green">Success:</span> ' + response.message
    		});
    		$modalInstance.close('success');
    		$timeout(function () {
    			$state.go("webvella-entity-records", {
    				areaName: $stateParams.areaName,
    				entityName: $stateParams.entityName,
    				listName: $stateParams.listName,
    				filter: $stateParams.filter,
    				page: $stateParams.page

    			}, { reload: true });
    		}, 0);
    	}

    	function errorCallback(response) {
    		popupData.hasError = true;
    		popupData.errorMessage = response.message;


    	}
    	$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec');
    };


})();
