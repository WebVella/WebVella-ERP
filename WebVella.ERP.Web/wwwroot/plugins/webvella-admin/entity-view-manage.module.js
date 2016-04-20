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
                    templateUrl: '/plugins/webvella-admin/sidebar.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                    controller: 'WebVellaAdminEntityViewManageInfoController',
                    templateUrl: '/plugins/webvella-admin/entity-view-manage.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
            	checkedAccessPermission: checkAccessPermission,
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta
            },
            data: {

            }
        });
    };
    //#endregion

	//#region << Resolve >> ///////////////////////////////
    checkAccessPermission.$inject = ['$q', '$log', 'resolvedCurrentUser', 'ngToast'];
	
    function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
    	var defer = $q.defer();
    	var messageContent = '<span class="go-red">No access:</span> You do not have access to the <span class="go-red">Admin</span> area';
    	var accessPermission = false;
    	for (var i = 0; i < resolvedCurrentUser.roles.length; i++) {
    		if (resolvedCurrentUser.roles[i] == "bdc56420-caf0-4030-8a0e-d264938e0cda") {
    			accessPermission = true;
    		}
    	}

    	if (accessPermission) {
    		defer.resolve();
    	}
    	else {

    		ngToast.create({
    			className: 'error',
    			content: messageContent,
    			timeout: 7000
    		});
    		defer.reject("No access");
    	}

    	return defer.promise;
    }

    resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaCoreService', '$stateParams', '$state', '$timeout'];
    
    function resolveCurrentEntityMeta($q, $log, webvellaCoreService, $stateParams, $state, $timeout) {
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            if (response.object == null) {
                $timeout(function () {
                    alert("error in response!")
                }, 0);
            }
            else {
                defer.resolve(response.object);
            }
        }

        function errorCallback(response) {
            if (response.object == null) {
                $timeout(function () {
                    alert("error in response!")
                }, 0);
            }
            else {
            	defer.reject(response.message);
            }
        }

        webvellaCoreService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ////////////////////////////
    controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal', '$timeout',
                            'resolvedCurrentEntityMeta', 'webvellaCoreService', 'ngToast'];
    
    function controller($filter, $scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal, $timeout,
                        resolvedCurrentEntityMeta, webvellaCoreService, ngToast) {

        
        var ngCtrl = this;
        //#region << Initialize Current Entity >>
        ngCtrl.entity = fastCopy(resolvedCurrentEntityMeta);
        //#endregion

        //#region << Update page title & Hide side menu>>
        ngCtrl.pageTitle = "Entity Views | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide side menu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
    	//#endregion

    	//Awesome font icon names array 
        ngCtrl.icons = getFontAwesomeIconNames();

    	//#region << Initialize View and Content Region >>
        ngCtrl.view = {};
        ngCtrl.originalView = {};
        for (var i = 0; i < ngCtrl.entity.recordViews.length; i++) {
        	if (ngCtrl.entity.recordViews[i].name === $stateParams.viewName) {
        		ngCtrl.view = fastCopy(ngCtrl.entity.recordViews[i]);
        		ngCtrl.originalView = fastCopy(ngCtrl.entity.recordViews[i]);
        	}
        }
        //#endregion
        ngCtrl.nameIsChanged = false;
		ngCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;

		
		//#region << Html field >>
		

        ngCtrl.fieldUpdate = function (key, data) {
        	ngCtrl.nameIsChanged = false;
        	ngCtrl.patchObject = {};
        	ngCtrl.patchObject[key] = data;
        	if (key == 'name') {
        		ngCtrl.nameIsChanged = true;

        	}
        	webvellaCoreService.patchEntityView(ngCtrl.patchObject, ngCtrl.originalView.name, $stateParams.entityName, patchSuccessCallback, patchFailedCallback);

        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
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
        	ngToast.create({
        		className: 'error',
        		content: '<span class="go-red">Error:</span> ' + response.message,
        		timeout: 7000
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
    deleteViewModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];

	
    function deleteViewModalController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state) {
    	
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
    		ngToast.create({
    			className: 'success',
    			content: '<span class="go-green">Success:</span> ' + response.message
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
