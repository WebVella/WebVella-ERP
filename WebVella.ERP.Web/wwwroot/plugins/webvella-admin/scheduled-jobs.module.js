/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminScheduledJobsController', controller)
		.controller('DetailsModalController',DetailsModalController);
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

    config.$inject = ['$stateProvider'];

    
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-scheduled-jobs', {
            parent: 'webvella-admin-base',
            url: '/jobs', 
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
                	controller: 'WebVellaAdminScheduledJobsController',
                	templateUrl: '/plugins/webvella-admin/scheduled-jobs.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
                resolvedScheduledJobsList: resolveScheduledJobsList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

    // Resolve Roles list /////////////////////////
    resolveScheduledJobsList.$inject = ['$q', '$log', 'webvellaCoreService','$location'];
    
    function resolveScheduledJobsList($q, $log, webvellaCoreService,$location) {
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

		if(queryParams.startFromDate != null){
			search.startFromDate = queryParams.startFromDate;
		}
		else{
			search.startFromDate = null;
		}

		if(queryParams.startToDate != null){
			search.startToDate = queryParams.startToDate;
		}
		else{
			search.startToDate = null;
		}

		if(queryParams.finishedFromDate != null){
			search.finishedFromDate = queryParams.finishedFromDate;
		}
		else{
			search.finishedFromDate = null;
		}

		if(queryParams.finishedToDate != null){
			search.finishedToDate = queryParams.finishedToDate;
		}
		else{
			search.finishedToDate = null;
		}

		if(queryParams.typeName != null){
			search.typeName = queryParams.typeName;
		}
		else{
			search.typeName = null;
		}

		if(queryParams.status != null){
			search.status = _.parseInt(queryParams.status);
		}
		else{
			search.status = null;
		}

		if(queryParams.priority != null){
			search.priority = _.parseInt(queryParams.priority);
		}
		else{
			search.priority = null;
		}

		if(queryParams.schedulePlanId != null){
			search.schedulePlanId = queryParams.schedulePlanId;
		}
		else{
			search.schedulePlanId = null;
		}

		search.currentPage = 1;
		if(queryParams.page != null && _.parseInt(queryParams.page) != 1){
			search.currentPage = _.parseInt(queryParams.page);
		}

        webvellaCoreService.getJobsList(search.startFromDate,search.startToDate,search.finishedFromDate,search.finishedToDate,search.typeName,search.status,search.priority,search.schedulePlanId,search.currentPage,10,successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', '$location',
							'resolvedScheduledJobsList', 'webvellaCoreService','$translate','$uibModal','ngToast'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,$location,
						resolvedScheduledJobsList, webvellaCoreService,$translate,$uibModal,ngToast) {

        
        var ngCtrl = this;
		ngCtrl.jobs = resolvedScheduledJobsList;
		ngCtrl.currentPage = 1;
		ngCtrl.pageSize = 10;
		ngCtrl.search = {};
		//#region << Update page title & hide the side menu >>
		ngCtrl.pageTitle = "Background jobs" + " | " + pageTitle;
		$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		$rootScope.adminSectionName = "Background jobs";
    	//#endregion
		ngCtrl.statusList = [
			{
				key:null,
				value:"any status"
			},
			{
				key:1,
				value:"pending"
			},
			{
				key:2,
				value:"running"
			},
			{
				key:3,
				value:"canceled"
			},
			{
				key:4,
				value:"failed"
			},
			{
				key:5,
				value:"finished"
			},
			{
				key:6,
				value:"aborted"
			}
		]
		ngCtrl.priorityList = [
			{
				key:null,
				value:"any priority"
			},
			{
				key:1,
				value:"low"
			},
			{
				key:2,
				value:"medium"
			},
			{
				key:3,
				value:"high"
			},
			{
				key:4,
				value:"higher"
			},
			{
				key:5,
				value:"highest"
			},
		]  
		ngCtrl.getStatusLabel = function(statusId){
			var index = _.findIndex(ngCtrl.statusList,function(o){return o.key == statusId});
			if(index > -1){
				return ngCtrl.statusList[index].value;
			}
			else{
				return "n/a";
			}
		}

		ngCtrl.getPriorityLabel = function(priorityId){
			var index = _.findIndex(ngCtrl.priorityList,function(o){return o.key == priorityId});
			if(index > -1){
				return ngCtrl.priorityList[index].value;
			}
			else{
				return "n/a";
			}
		}

        ngCtrl.openDetailsModal = function (job) {
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'detailsModal.html',
                controller: 'DetailsModalController',
                controllerAs: "popupCtrl",
                size: "lg",
                resolve: {
                    parentCtrl: function () {
                        return ngCtrl;
                    },
                    selectedJob: function () {
                        return job;
                    }
                }
            });

        }
 
		function initFromQueryParams(){
		ngCtrl.queryParams = $location.search();
		if(ngCtrl.queryParams.startFromDate != null){
			ngCtrl.search.startFromDate = moment(ngCtrl.queryParams.startFromDate).toDate();
		}
		else{
			ngCtrl.search.startFromDate = null;
		}

		if(ngCtrl.queryParams.startToDate != null){
			ngCtrl.search.startToDate = moment(ngCtrl.queryParams.startToDate).toDate();
		}
		else{
			ngCtrl.search.startToDate = null;
		}

		if(ngCtrl.queryParams.finishedFromDate != null){
			ngCtrl.search.finishedFromDate = moment(ngCtrl.queryParams.finishedFromDate).toDate();
		}
		else{
			ngCtrl.search.finishedFromDate = null;
		}

		if(ngCtrl.queryParams.finishedToDate != null){
			ngCtrl.search.finishedToDate = moment(ngCtrl.queryParams.finishedToDate).toDate();
		}
		else{
			ngCtrl.search.finishedToDate = null;
		}

		if(ngCtrl.queryParams.typeName != null){
			ngCtrl.search.typeName = ngCtrl.queryParams.typeName;
		}
		else{
			ngCtrl.search.typeName = null;
		}

		if(ngCtrl.queryParams.status != null){
			ngCtrl.search.status = _.parseInt(ngCtrl.queryParams.status);
		}
		else{
			ngCtrl.search.status = null;
		}

		if(ngCtrl.queryParams.priority != null){
			ngCtrl.search.priority = _.parseInt(ngCtrl.queryParams.priority);
		}
		else{
			ngCtrl.search.priority = null;
		}

		if(ngCtrl.queryParams.schedulePlanId != null){
			ngCtrl.search.schedulePlanId = ngCtrl.queryParams.schedulePlanId;
		}
		else{
			ngCtrl.search.schedulePlanId = null;
		}


		if(ngCtrl.queryParams.page != null && _.parseInt(ngCtrl.queryParams.page) != 1){
			ngCtrl.currentPage = _.parseInt(ngCtrl.queryParams.page);
		}

		}

		initFromQueryParams();

		ngCtrl.loadMorePages = function(page){
			function successCallback(response){
				ngCtrl.jobs = response.object;
				if(page == 1){
					$location.search("page",null);
				}
				else{
					$location.search("page",page);
				}
				$location.search("startFromDate",ngCtrl.search.startFromDate);
				$location.search("startToDate",ngCtrl.search.startToDate);
				$location.search("finishedFromDate",ngCtrl.search.finishedFromDate);
				$location.search("finishedToDate",ngCtrl.search.finishedToDate);
				$location.search("typeName",ngCtrl.search.typeName);
				$location.search("status",ngCtrl.search.status);
				$location.search("priority",ngCtrl.search.priority);
				$location.search("schedulePlanId",ngCtrl.search.schedulePlanId);
				ngCtrl.currentPage = page;
				if(ngCtrl.queryParams.startFromDate !=null){
					ngCtrl.search.startFromDate = moment(ngCtrl.queryParams.startFromDate).toDate();
				}
				if(ngCtrl.queryParams.startToDate !=null){
					ngCtrl.search.startToDate = moment(ngCtrl.queryParams.startToDate).toDate();
				}
				if(ngCtrl.queryParams.finishedFromDate !=null){
					ngCtrl.search.finishedFromDate = moment(ngCtrl.queryParams.finishedFromDate).toDate();
				}
				if(ngCtrl.queryParams.finishedToDate !=null){
					ngCtrl.search.finishedToDate = moment(ngCtrl.queryParams.finishedToDate).toDate();
				}
			}
			function errorCallback(response){
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});				
			}

			if(ngCtrl.search.startFromDate != null){
				ngCtrl.search.startFromDate = moment(ngCtrl.search.startFromDate).utc().toISOString();
			}
			if(ngCtrl.search.startToDate != null){
				ngCtrl.search.startToDate = moment(ngCtrl.search.startToDate).utc().toISOString();
			}

			if(ngCtrl.search.finishedFromDate != null){
				ngCtrl.search.finishedFromDate = moment(ngCtrl.search.finishedFromDate).utc().toISOString();
			}
			if(ngCtrl.search.finishedToDate != null){
				ngCtrl.search.finishedToDate = moment(ngCtrl.search.finishedToDate).utc().toISOString();
			}

			webvellaCoreService.getJobsList(ngCtrl.search.startFromDate,ngCtrl.search.startToDate,ngCtrl.search.finishedFromDate,ngCtrl.search.finishedToDate,ngCtrl.search.typeName,ngCtrl.search.status,ngCtrl.search.priority,ngCtrl.search.schedulePlanId,page,ngCtrl.pageSize,successCallback, errorCallback);
		}

		ngCtrl.submitFilter = function(){
			ngCtrl.loadMorePages(1);
		}

		ngCtrl.selectPage = function (page) {
			ngCtrl.loadMorePages(page);
		}

	}
    //#endregion

    //// Modal Controllers
    DetailsModalController.$inject = ['$uibModalInstance', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'parentCtrl','selectedJob','$translate'];
    
    function DetailsModalController($uibModalInstance, webvellaCoreService, ngToast, $timeout, $state, $location, parentCtrl,selectedJob,$translate) {
        
        var popupCtrl = this;
        popupCtrl.selectedJob = fastCopy(selectedJob);
		popupCtrl.parentCtrl = parentCtrl;
        popupCtrl.cancel = function () {
            $uibModalInstance.close('dismiss');
        };

    };



})();
