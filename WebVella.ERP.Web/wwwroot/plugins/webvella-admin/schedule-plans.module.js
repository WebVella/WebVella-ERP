/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminSchedulePlansController', controller)
		.controller('ManageModalController',ManageModalController);
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

    config.$inject = ['$stateProvider'];

    
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-schedule-plans', {
            parent: 'webvella-admin-base',
            url: '/schedule-plans', 
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
                	controller: 'WebVellaAdminSchedulePlansController',
                	templateUrl: '/plugins/webvella-admin/schedule-plans.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
                resolvedSchedulePlansList: resolveSchedulePlansList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

    // Resolve Roles list /////////////////////////
    resolveSchedulePlansList.$inject = ['$q', '$log', 'webvellaCoreService'];
    
    function resolveSchedulePlansList($q, $log, webvellaCoreService) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaCoreService.getSchedulePlanList(successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', 
							'resolvedSchedulePlansList', 'webvellaCoreService','$translate','$uibModal'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedSchedulePlansList, webvellaCoreService,$translate,$uibModal) {

        
        var ngCtrl = this;
        ngCtrl.search = {};

		//#region << Update page title & hide the side menu >>
		$translate(['SCHEDULE_PLANS', 'SCHEDULE_PLANS']).then(function (translations) {
			ngCtrl.pageTitle = translations.SCHEDULE_PLANS + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.SCHEDULE_PLANS;
		});
    	//#endregion

        ngCtrl.plans = resolvedSchedulePlansList.data;
		if(ngCtrl.plans != null && ngCtrl.plans.length > 0){
			ngCtrl.plans = ngCtrl.plans.sort(function (a, b) {
				if (a.name < b.name) return -1;
				if (a.name > b.name) return 1;
				return 0;
			});
		}

		ngCtrl.planTypes=[
			{
				key:1,
				value:"Interval"
			},
			{
				key:2,
				value:"Daily"
			},
			{
				key:3,
				value:"Weekly"
			},
			{
				key:4,
				value:"Monthly"
			}
		]

		ngCtrl.getPlanTypeName = function(keyId){
			var typeIndex = _.findIndex(ngCtrl.planTypes,function(record){return record.key == keyId});
			if(typeIndex > -1){
				return ngCtrl.planTypes[typeIndex].value;
			}
			else{
				return "Unknown";
			}
		}


        //Create new entity modal
        ngCtrl.openManageModal = function (plan) {
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'manageModal.html',
                controller: 'ManageModalController',
                controllerAs: "popupCtrl",
                size: "lg",
                resolve: {
                    parentCtrl: function () {
                        return ngCtrl;
                    },
                    selectedPlan: function () {
                        return plan;
                    }
                }
            });

        }
    }
    //#endregion


    //// Modal Controllers
    ManageModalController.$inject = ['$uibModalInstance', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'parentCtrl','selectedPlan','$translate'];
    
    function ManageModalController($uibModalInstance, webvellaCoreService, ngToast, $timeout, $state, $location, parentCtrl,selectedPlan,$translate) {
        
        var popupCtrl = this;
        popupCtrl.selectedPlan = fastCopy(selectedPlan);
		popupCtrl.planTypes = fastCopy(parentCtrl.planTypes);

        /// EXIT functions
        popupCtrl.ok = function () {
        	popupCtrl.validation = {};
        	if (!popupCtrl.isUpdate) {
        		webvellaCoreService.createRecord("role",popupCtrl.role, successCallback, errorCallback);
            }
            else {
        		webvellaCoreService.updateRecord(popupCtrl.role.id, "role", popupCtrl.role, successCallback, errorCallback);
            } 
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.close('dismiss');
        };

        /// Aux
        function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL','ROLE_SAVE_SUCCESS_MESSAGE']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + translations.ROLE_SAVE_SUCCESS_MESSAGE
				});
			});
            $uibModalInstance.close('success');
            webvellaCoreService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaCoreService.generateValidationMessages(response, popupCtrl, popupCtrl.user, location);
        }
    };



})();
