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
                    controllerAs: 'contentData'
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
        var contentData = this;
        //#region << Initialize Current Entity >>
        contentData.entity = fastCopy(resolvedCurrentEntityMeta);
        //#endregion

        //#region << Update page title & Hide side menu>>
        contentData.pageTitle = "Entity Views | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
			//Hide side menu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);
		$rootScope.currentSectionName = "Entities";
    	//#endregion

    	//Awesome font icon names array 
        contentData.icons = getFontAwesomeIconNames();

    	//#region << Initialize View and Content Region >>
        contentData.view = {};
        contentData.originalView = {};
        for (var i = 0; i < contentData.entity.recordViews.length; i++) {
        	if (contentData.entity.recordViews[i].name === $stateParams.viewName) {
        		contentData.view = fastCopy(contentData.entity.recordViews[i]);
        		contentData.originalView = fastCopy(contentData.entity.recordViews[i]);
        	}
        }
        //#endregion
        contentData.nameIsChanged = false;
		contentData.renderFieldValue = webvellaAreasService.renderFieldValue;

		
		//#region << Html field >>
		

        contentData.fieldUpdate = function (key, data) {
        	contentData.nameIsChanged = false;
        	contentData.patchObject = {};
        	contentData.patchObject[key] = data;
        	if (key == 'name') {
        		contentData.nameIsChanged = true;

        	}
        	webvellaAdminService.patchEntityView(contentData.patchObject, contentData.originalView.name, $stateParams.entityName, patchSuccessCallback, patchFailedCallback);

        }

        function patchSuccessCallback(response) {
        	ngToast.create({
        		className: 'success',
        		content: '<span class="go-green">Success:</span> ' + response.message
        	});
        	//webvellaAdminService.regenerateAllAreaAttachments();
        	if (contentData.nameIsChanged) {
        		$timeout(function () {
        			$state.go("webvella-admin-entity-view-manage-info", { entityName: $stateParams.entityName,viewName:contentData.view.name }, { reload: true });
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
        contentData.deleteViewModal = function () {
        	var modalInstance = $uibModal.open({
        		animation: false,
        		templateUrl: 'deleteViewModal.html',
        		controller: 'DeleteViewModalController',
        		controllerAs: "popupData",
        		size: "",
        		resolve: {
        			parentData: function () { return contentData; }
        		}
        	});
        }

        contentData.viewTypes = [
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

        contentData.showViewType = function () {
        	var selected = $filter('filter')(contentData.viewTypes, { name: contentData.view.type });
        	return (contentData.view.type && selected.length) ? selected[0].label : 'empty';
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
    	var popupData = this;
    	popupData.parentData = parentData;

    	popupData.ok = function () {

    		webvellaAdminService.deleteEntityView(popupData.parentData.view.name, popupData.parentData.entity.name, successCallback, errorCallback);

    	};

    	popupData.cancel = function () {
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
    			$state.go("webvella-admin-entity-views", { entityName: popupData.parentData.entity.name }, { reload: true });
    		}, 0);
    	}

    	function errorCallback(response) {
    		popupData.hasError = true;
    		popupData.errorMessage = response.message;


    	}
    	$log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };

    //#endregion

})();
