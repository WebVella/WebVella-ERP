/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminSystemLogController', controller)
		.controller('ModalController',ModalController);
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

    config.$inject = ['$stateProvider'];

    
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-system-log', {
            parent: 'webvella-admin-base',
            url: '/system-log', 
            views: {
                "topnavView": {
                    controller: 'WebVellaAdminTopnavController',
                    templateUrl: '/plugins/webvella-admin/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "sidebarView": {
                    controller: 'WebVellaAdminSidebarController',
                    templateUrl: '/plugins/webvella-admin/sidebar.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                	controller: 'WebVellaAdminSystemLogController',
                	templateUrl: '/plugins/webvella-admin/system-log.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
                resolvedData: resolveData
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

    // Resolve Roles list /////////////////////////
    resolveData.$inject = ['$q', '$log', 'webvellaCoreService','$location'];
    
    function resolveData($q, $log, webvellaCoreService,$location) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

		var queryParams = $location.search();
		var search={};

		if(queryParams.fromDate != null){
			search.fromDate = queryParams.fromDate;
		}
		else{
			search.fromDate = null;
		}

		if(queryParams.untilDate != null){
			search.untilDate = queryParams.untilDate;
		}
		else{
			search.untilDate = null;
		}

		if(queryParams.type != null){
			search.type =  queryParams.type;
		}
		else{
			search.type = null;
		}

		if(queryParams.source != null){
			search.source = queryParams.source;
		}
		else{
			search.source = null;
		}

		if(queryParams.message != null){
			search.message = queryParams.message;
		}
		else{
			search.message = null;
		}

		if(queryParams.page != null){
			search.page =  _.parseInt(queryParams.page);
		}
		else{
			search.page = 1;
		}

		if(queryParams.notificationStatus != null){
			search.notificationStatus = queryParams.notificationStatus;
		}
		else{
			search.notificationStatus = null;
		}

        webvellaCoreService.getSystemLog(search.fromDate,search.untilDate,search.type,search.source,search.message,search.notificationStatus,search.page,10,successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout','$location', 
							'resolvedData', 'webvellaCoreService','$translate','$uibModal','ngToast'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,$location,
						resolvedData, webvellaCoreService,$translate,$uibModal,ngToast) {

        
        var ngCtrl = this;
		ngCtrl.currentPage = 1;
		ngCtrl.pageSize = 10;
		//#region << Update page title & hide the side menu >>
			ngCtrl.pageTitle = "System log" + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = "System log";

    	//#endregion
        ngCtrl.records = resolvedData;
		ngCtrl.isLoading = false;
		var queryObject = $location.search();
		for (var key in queryObject) {
			ngCtrl[key] = queryObject[key];
		}
		if(!ngCtrl.page){
			ngCtrl.page = 1;
			$location.search("page", null);
		}
		else{
			ngCtrl.page =  _.parseInt(ngCtrl.page);
		}

		if(!ngCtrl.fromDate){
			ngCtrl.fromDate = null;
		}		
		else{
			ngCtrl.fromDate = moment(ngCtrl.fromDate).toDate();
		}

		if(!ngCtrl.untilDate){
			ngCtrl.untilDate = null;
		}		
		else{
			ngCtrl.untilDate = moment(ngCtrl.untilDate).toDate();
		}

		if(!ngCtrl.type){
			ngCtrl.type = null;
		}		
		else{
			ngCtrl.type =  ngCtrl.type;
		}

		if(!ngCtrl.source){
			ngCtrl.source = null;
		}	

		if(!ngCtrl.message){
			ngCtrl.message = null;
		}	

		if(!ngCtrl.notificationStatus){
			ngCtrl.notificationStatus = null;
		}	
		else{
			ngCtrl.notificationStatus =  ngCtrl.notificationStatus;
		}

		ngCtrl.loadLogs = function(page){
			ngCtrl.isLoading = true;
			
			function successCallback(response){
				ngCtrl.records = response.object;
				if(page != 1){
					$location.search("page",page);
				}
				else {
					$location.search("page",null);
				}
				ngCtrl.page = page;

				if(ngCtrl.fromDate){
					$location.search("fromDate",ngCtrl.fromDate);
					ngCtrl.fromDate = moment(ngCtrl.fromDate).toDate();
				}
				else {
					$location.search("fromDate",null);
				}
				if(ngCtrl.untilDate){
					$location.search("untilDate",ngCtrl.untilDate);
					ngCtrl.untilDate = moment(ngCtrl.untilDate).toDate();
				}
				else {
					$location.search("untilDate",null);
				}
				if(ngCtrl.type){
					$location.search("type",ngCtrl.type);
				}
				else {
					$location.search("type",null);
				}
				if(ngCtrl.source){
					$location.search("source",ngCtrl.source);
				}
				else {
					$location.search("source",null);
				}
				if(ngCtrl.message){
					$location.search("message",ngCtrl.message);
				}
				else {
					$location.search("message",null);
				}
				if(ngCtrl.notificationStatus){
					$location.search("notificationStatus",ngCtrl.notificationStatus);
				}
				else {
					$location.search("notificationStatus",null);
				}
				ngCtrl.isLoading = false;
			}
			function errorCallback(response){
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
				});		
				ngCtrl.isLoading = false;
			}			
		
			if(ngCtrl.fromDate != null){
				ngCtrl.fromDate = moment(ngCtrl.fromDate).utc().toISOString();
			}
			if(ngCtrl.untilDate != null){
				ngCtrl.untilDate = moment(ngCtrl.untilDate).utc().toISOString();
			}

			webvellaCoreService.getSystemLog(ngCtrl.fromDate,ngCtrl.untilDate,ngCtrl.type,ngCtrl.source,ngCtrl.message,ngCtrl.notificationStatus,page,ngCtrl.pageSize,successCallback, errorCallback);
		}


		ngCtrl.selectPage = function(page){
			ngCtrl.loadLogs(page);
		}

		ngCtrl.submitFilter = function () {
			ngCtrl.loadLogs(1);
		}

        //Create new entity modal
        ngCtrl.openModal = function (record) {
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'detailsModal.html',
                controller: 'ModalController',
                controllerAs: "popupCtrl",
                size: "lg",
                resolve: {
                    parentCtrl: function () {
                        return ngCtrl;
                    },
                    selectedRecord: function () {
                        return record;
                    }
                }
            });

        }
    

	
	}
    //#endregion


    //// Modal Controllers
    ModalController.$inject = ['$uibModalInstance', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'parentCtrl','selectedRecord','$translate'];
    
    function ModalController($uibModalInstance, webvellaCoreService, ngToast, $timeout, $state, $location, parentCtrl,selectedRecord,$translate) {
        
        var popupCtrl = this;
		popupCtrl.validation = webvellaCoreService.initValidationObject();
        popupCtrl.selectedRecord = fastCopy(selectedRecord);
		popupCtrl.logTypes = fastCopy(parentCtrl.logTypes);



        popupCtrl.cancel = function () {
            $uibModalInstance.close('dismiss');
        };

 
    };


})();
