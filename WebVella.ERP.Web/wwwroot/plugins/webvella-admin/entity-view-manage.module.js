/* entity-view-manage.module.js */

/**
* @desc this module manages a single entity view in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityViewManageInfoController', controller)
		.controller('DeleteViewModalController', deleteViewModalController);

    //#region << Configuration >> /////////////////////////
    config.$inject = ['$stateProvider'];
    
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-view-manage-info', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/views/:viewName',
            views: {
                "topnavView": {
                    controller: 'WebVellaAdminTopnavController',
                    templateUrl: '/plugins/webvella-admin/topnav.view.html',
                    controllerAs: 'topnavData'
                },
                "sidebarView": {
                    controller: 'WebVellaAdminSidebarController',
                    templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                    controller: 'WebVellaAdminEntityViewManageInfoController',
                    templateUrl: '/plugins/webvella-admin/entity-view-manage.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
				resolvedEntityList:resolveEntityList
			},
            data: {

            }
        });
    };
    //#endregion

	//#region << Resolve >> ///////////////////////////////
 	resolveEntityList.$inject = ['$q', '$log', 'webvellaCoreService', '$state', '$stateParams'];
	function resolveEntityList($q, $log, webvellaCoreService, $state, $stateParams) {
		var defer = $q.defer();
		function successCallback(response) {
			defer.resolve(response.object);
		}
		function errorCallback(response) {
			defer.reject(response.message);
		}
		webvellaCoreService.getEntityMetaList(successCallback, errorCallback);
		return defer.promise;
	}
    //#endregion

    //#region << Controller >> ////////////////////////////
    controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal', '$timeout',
                            'webvellaCoreService', 'ngToast','$translate', 'resolvedEntityList'];
    
    function controller($filter, $scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal, $timeout,
                        webvellaCoreService, ngToast,$translate, resolvedEntityList) {

        
        var ngCtrl = this;
        //#region << Initialize Current Entity >>
        ngCtrl.entity = webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName,resolvedEntityList);
        //#endregion

		//#region << Update page title & hide the side menu >>
		$translate(['RECORD_VIEW_MANAGE_PAGE_TITLE', 'ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.RECORD_VIEW_MANAGE_PAGE_TITLE + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
    	//#endregion

    	//Awesome font icon names array 
        ngCtrl.icons = getFontAwesomeIconNames();

    	//#region << Initialize View and Content Region >>
        ngCtrl.view = fastCopy(webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.viewName, $stateParams.entityName,resolvedEntityList));
        ngCtrl.originalView = fastCopy(webvellaCoreService.getEntityRecordViewFromEntitiesMetaList($stateParams.viewName, $stateParams.entityName,resolvedEntityList));

        //#endregion
        ngCtrl.nameIsChanged = false;
		ngCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;

		
		//#region << Html field >>
		

        ngCtrl.fieldUpdate = function (key, data) {
			if(key == "dataSourceUrl" && data == ""){
				data = null;
			}
			if(key == "dynamicHtmlTemplate" && data == ""){
				data = null;
			}

        	ngCtrl.nameIsChanged = false;
        	ngCtrl.patchObject = {};
        	ngCtrl.patchObject[key] = data;
        	if (key == 'name') {
        		ngCtrl.nameIsChanged = true;

        	}
        	webvellaCoreService.patchEntityView(ngCtrl.patchObject, ngCtrl.originalView.name, $stateParams.entityName, patchSuccessCallback, patchFailedCallback);

        }

        function patchSuccessCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
        	webvellaCoreService.regenerateAllAreaAttachments();
        	if (ngCtrl.nameIsChanged) {
        		$timeout(function () {
        			$state.go("webvella-admin-entity-view-manage-info", { entityName: $stateParams.entityName,viewName:ngCtrl.view.name }, { reload: true });
        		}, 0);
        	}
        	return true;
        }
        function patchFailedCallback(response) {
			$translate(['ERROR_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'error',
					content: translations.ERROR_MESSAGE_LABEL + ' ' + response.message,
					timeout: 7000
				});
			});
        	return false;
        }

    	//Delete view
        ngCtrl.deleteViewModal = function () {
        	var modalInstance = $uibModal.open({
        		animation: false,
        		templateUrl: 'deleteViewModal.html',
        		controller: 'DeleteViewModalController',
        		controllerAs: "popupCtrl",
        		size: "",
        		resolve: {
        			parentData: function () { return ngCtrl; }
        		}
        	});
        }

        ngCtrl.viewTypes = [
		{
			name: "general",
			label: "general"
		},
		{
			name: "quick_view",
			label: "quick view"
		},
		{
			name: "create",
			label: "create"
		},
		{
			name: "quick_create",
			label: "quick create"
		},
		{
			name: "hidden",
			label: "hidden"
		}
        ];

        ngCtrl.showViewType = function () {
        	var selected = $filter('filter')(ngCtrl.viewTypes, { name: ngCtrl.view.type });
        	return (ngCtrl.view.type && selected.length) ? selected[0].label : 'empty';
        };

    }
    //#endregion

	//#region << Modal Controllers >>
    deleteViewModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state','$translate'];

	
    function deleteViewModalController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state,$translate) {
    	
    	var popupCtrl = this;
    	popupCtrl.parentData = parentData;

    	popupCtrl.ok = function () {

    		webvellaCoreService.deleteEntityView(popupCtrl.parentData.view.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);

    	};

    	popupCtrl.cancel = function () {
    		$uibModalInstance.dismiss('cancel');
    	};

    	/// Aux
    	function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + " " + response.message
				});
			});
    		$uibModalInstance.close('success');
    		$timeout(function () {
    			$state.go("webvella-admin-entity-views", { entityName: popupCtrl.parentData.entity.name }, { reload: true });
    		}, 0);
    	}

    	function errorCallback(response) {
    		popupCtrl.hasError = true;
    		popupCtrl.errorMessage = response.message;


    	}
    };

    //#endregion

})();
