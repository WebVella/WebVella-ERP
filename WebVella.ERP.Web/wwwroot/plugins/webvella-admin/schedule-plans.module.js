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
							'resolvedSchedulePlansList', 'webvellaCoreService','$translate','$uibModal','ngToast'];
    
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedSchedulePlansList, webvellaCoreService,$translate,$uibModal,ngToast) {

        
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
    
		ngCtrl.triggerPlan = function(plan){


			function successCallback(response) {
				ngToast.create({
					className: 'success',
					content: '<span class="go-green">Success:</span> ' + "Schedule plan triggered!",
					timeout: 3000
				});
				webvellaCoreService.GoToState($state.current.name, {});
			}

			function errorCallback(response) {
				ngToast.create({
					className: 'error',
					content: '<span class="go-red">Error:</span> ' + response.message,
					timeout: 7000
				});
			}		
			successCallback();
			webvellaCoreService.triggerSchedulePlan(plan.id, successCallback, errorCallback);

		}
	
	}
    //#endregion


    //// Modal Controllers
    ManageModalController.$inject = ['$uibModalInstance', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'parentCtrl','selectedPlan','$translate'];
    
    function ManageModalController($uibModalInstance, webvellaCoreService, ngToast, $timeout, $state, $location, parentCtrl,selectedPlan,$translate) {
        
        var popupCtrl = this;
		popupCtrl.validation = webvellaCoreService.initValidationObject();
        popupCtrl.selectedPlan = fastCopy(selectedPlan);
		popupCtrl.planTypes = fastCopy(parentCtrl.planTypes);

		popupCtrl.initDate = function(date){
			if(date != null){
				return  moment(fastCopy(date)).toDate();
			}
			return null;
		}
		if(popupCtrl.selectedPlan.start_date){
			popupCtrl.selectedPlan.start_date = popupCtrl.initDate(popupCtrl.selectedPlan.start_date);
		}
		if(popupCtrl.selectedPlan.end_date){
			popupCtrl.selectedPlan.end_date = popupCtrl.initDate(popupCtrl.selectedPlan.end_date);
		}

		if(popupCtrl.selectedPlan.start_timespan){
			popupCtrl.selectedPlan.start_timespan = popupCtrl.initDate(popupCtrl.selectedPlan.start_timespan);
		}
		if(popupCtrl.selectedPlan.end_timespan){
			popupCtrl.selectedPlan.end_timespan = popupCtrl.initDate(popupCtrl.selectedPlan.end_timespan);
		}

		function validateSubmit(){
			popupCtrl.validation = webvellaCoreService.initValidationObject();
			if(isStringNullOrEmptyOrWhiteSpace(popupCtrl.selectedPlan.name)){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"Validation failed","name","* required field");
			}
			if(popupCtrl.selectedPlan.start_date != null  && popupCtrl.selectedPlan.end_date != null && moment(popupCtrl.selectedPlan.end_date).isBefore(moment(popupCtrl.selectedPlan.start_date))){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"Validation failed","end_date","* end date should be before start date");
			}
			if(popupCtrl.selectedPlan.type == 1 && popupCtrl.selectedPlan.interval_in_minutes == null){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"Validation failed","interval_in_minutes","* required field when type: interval");
			}
			if(popupCtrl.selectedPlan.type == 1 &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_monday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_tuesday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_wednesday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_thursday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_friday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_saturday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_sunday){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"Validation failed","schedule_days","* at least one week day is required when type: interval");
			}
			if(popupCtrl.selectedPlan.type == 2 &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_monday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_tuesday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_wednesday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_thursday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_friday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_saturday &&
				!popupCtrl.selectedPlan.schedule_days.scheduled_on_sunday){
				popupCtrl.validation = webvellaCoreService.setValidationError(popupCtrl.validation,"Validation failed","schedule_days","* at least one week day is required when type: daily");
			}
		}

		function createSubmitObject(){
			var submitObject = {};
			submitObject.id = popupCtrl.selectedPlan.id;
			submitObject.enabled = popupCtrl.selectedPlan.enabled;
			submitObject.name = popupCtrl.selectedPlan.name;
			submitObject.start_date = popupCtrl.selectedPlan.start_date;
			submitObject.end_date = popupCtrl.selectedPlan.end_date;
			submitObject.type = popupCtrl.selectedPlan.type;
			switch(submitObject.type){
				case 1:
					submitObject.interval_in_minutes = popupCtrl.selectedPlan.interval_in_minutes;		
					submitObject.start_timespan = popupCtrl.selectedPlan.start_timespan;	
					submitObject.end_timespan = popupCtrl.selectedPlan.end_timespan;
					submitObject.schedule_days = {};
					submitObject.schedule_days.scheduled_on_monday = popupCtrl.selectedPlan.schedule_days.scheduled_on_monday;	
					submitObject.schedule_days.scheduled_on_tuesday = popupCtrl.selectedPlan.schedule_days.scheduled_on_tuesday;	
					submitObject.schedule_days.scheduled_on_wednesday = popupCtrl.selectedPlan.schedule_days.scheduled_on_wednesday;	
					submitObject.schedule_days.scheduled_on_thursday = popupCtrl.selectedPlan.schedule_days.scheduled_on_thursday;	
					submitObject.schedule_days.scheduled_on_friday = popupCtrl.selectedPlan.schedule_days.scheduled_on_friday;	
					submitObject.schedule_days.scheduled_on_saturday = popupCtrl.selectedPlan.schedule_days.scheduled_on_saturday;	
					submitObject.schedule_days.scheduled_on_sunday = popupCtrl.selectedPlan.schedule_days.scheduled_on_sunday;	
					break;
				case 2:
					submitObject.schedule_days = {};
					submitObject.schedule_days.scheduled_on_monday = popupCtrl.selectedPlan.schedule_days.scheduled_on_monday;	
					submitObject.schedule_days.scheduled_on_tuesday = popupCtrl.selectedPlan.schedule_days.scheduled_on_tuesday;	
					submitObject.schedule_days.scheduled_on_wednesday = popupCtrl.selectedPlan.schedule_days.scheduled_on_wednesday;	
					submitObject.schedule_days.scheduled_on_thursday = popupCtrl.selectedPlan.schedule_days.scheduled_on_thursday;	
					submitObject.schedule_days.scheduled_on_friday = popupCtrl.selectedPlan.schedule_days.scheduled_on_friday;	
					submitObject.schedule_days.scheduled_on_saturday = popupCtrl.selectedPlan.schedule_days.scheduled_on_saturday;	
					submitObject.schedule_days.scheduled_on_sunday = popupCtrl.selectedPlan.schedule_days.scheduled_on_sunday;	
					break;
				default:
					break;			
			}

			return submitObject;
		}

        /// EXIT functions
        popupCtrl.ok = function () {
			validateSubmit();
			if(!popupCtrl.validation.isInvalid){
				var submitObject = createSubmitObject();
       			webvellaCoreService.updateSchedulePlan(popupCtrl.selectedPlan.id, submitObject, successCallback, errorCallback);
			}
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.close('dismiss');
        };

        /// Aux
        function successCallback(response) {
			ngToast.create({
				className: 'success',
				content: '<span class="go-green">Success:</span> ' + "Schedule plan saved!",
				timeout: 3000
			});
            $uibModalInstance.close('success');
            webvellaCoreService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
			ngToast.create({
				className: 'error',
				content: '<span class="go-red">Error:</span> ' + response.message,
				timeout: 7000
			});
        }
    };


})();
