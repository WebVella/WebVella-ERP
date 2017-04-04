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
    resolveScheduledJobsList.$inject = ['$q', '$log', 'webvellaCoreService'];
    
    function resolveScheduledJobsList($q, $log, webvellaCoreService) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaCoreService.getJobsList(successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', 
							'resolvedScheduledJobsList', 'webvellaCoreService','$translate','$uibModal','ngToast'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedScheduledJobsList, webvellaCoreService,$translate,$uibModal,ngToast) {

        
        var ngCtrl = this;
		ngCtrl.jobs = resolvedScheduledJobsList;

		//#region << Update page title & hide the side menu >>
		ngCtrl.pageTitle = "Background jobs" + " | " + pageTitle;
		$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		$rootScope.adminSectionName = "Background jobs";
    	//#endregion

		ngCtrl.statusList = [
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
