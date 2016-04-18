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
    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-view-manage-info', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName/views/:viewName/info',
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
                    templateUrl: '/plugins/webvella-admin/entity-view-manage-info.view.html',
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
	/* @ngInject */
    function checkAccessPermission($q, $log, resolvedCurrentUser, ngToast) {
    	$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
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

    	$log.debug('webvellaAreas>entities> BEGIN check access permission ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }

    resolveCurrentEntityMeta.$inject = ['$q', '$log', 'webvellaAdminService', '$stateParams', '$state', '$timeout'];
    /* @ngInject */
    function resolveCurrentEntityMeta($q, $log, webvellaAdminService, $stateParams, $state, $timeout) {
    	$log.debug('webvellaAdmin>entity-details> BEGIN resolveCurrentEntityMeta state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

        webvellaAdminService.getEntityMeta($stateParams.entityName, successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entity-details> END resolveCurrentEntityMeta state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    //#endregion

    //#region << Controller >> ////////////////////////////
    controller.$inject = ['$filter', '$scope', '$log', '$rootScope', '$state', '$stateParams', 'pageTitle', '$uibModal', '$timeout',
                            'resolvedCurrentEntityMeta', 'webvellaAdminService','webvellaAreasService', 'ngToast'];
    /* @ngInject */
    function controller($filter, $scope, $log, $rootScope, $state, $stateParams, pageTitle, $uibModal, $timeout,
                        resolvedCurrentEntityMeta, webvellaAdminService,webvellaAreasService, ngToast) {
    	$log.debug('webvellaAdmin>entity-view-manage-info> START controller.exec ' + moment().format('HH:mm:ss SSSS'));

        /* jshint validthis:true */
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
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
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
		ngCtrl.renderFieldValue = webvellaAreasService.renderFieldValue;

		
		//#region << Html field >>
		

        ngCtrl.fieldUpdate = function (key, data) {
        	ngCtrl.nameIsChanged = false;
        	ngCtrl.patchObject = {};
        	ngCtrl.patchObject[key] = data;
        	if (key == 'name') {
        		ngCtrl.nameIsChanged = true;

        	}
        	webvellaAdminService.patchEntityView(ngCtrl.patchObject, ngCtrl.originalView.name, $stateParams.entityName, patchSuccessCallback, patchFailedCallback);

        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
        	});
        	webvellaAdminService.regenerateAllAreaAttachments();
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

        $log.debug('webvellaAdmin>entity-view-manage-info> END controller.exec ' + moment().format('HH:mm:ss SSSS'));

    }
    //#endregion

	//#region << Modal Controllers >>
    deleteViewModalController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state'];

	/* @ngInject */
    function deleteViewModalController(parentData, $uibModalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state) {
    	$log.debug('webvellaAdmin>entities>deleteFieldModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
    	/* jshint validthis:true */
    	var popupCtrl = this;
    	popupCtrl.parentData = parentData;

    	popupCtrl.ok = function () {

    		webvellaAdminService.deleteEntityView(popupCtrl.parentData.view.name, popupCtrl.parentData.entity.name, successCallback, errorCallback);

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
    	$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };

    //#endregion

})();
