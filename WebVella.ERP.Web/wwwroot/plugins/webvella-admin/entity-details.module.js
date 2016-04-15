/* entity-details.module.js */

/**
* @desc this module manages the entity record details in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntityDetailsController', controller)
        .controller('DeleteEntityModalController', deleteEntityController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entity-details', {
            parent: 'webvella-admin-base',
            url: '/entities/:entityName', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntityDetailsController',
                    templateUrl: '/plugins/webvella-admin/entity-details.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
            	checkedAccessPermission: checkAccessPermission,
                resolvedCurrentEntityMeta: resolveCurrentEntityMeta,
                resolvedRolesList: resolveRolesList,
                resolvedAreasList: resolveAreasList
            },
            data: {

            }
        });
    };


	// Resolve Function /////////////////////////
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
    	$log.debug('webvellaAdmin>entity-details> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
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
        $log.debug('webvellaAdmin>entity-details> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

	// Resolve Roles list /////////////////////////
    resolveRolesList.$inject = ['$q', '$log', 'webvellaAdminService'];
	/* @ngInject */
    function resolveRolesList($q, $log, webvellaAdminService) {
    	$log.debug('webvellaAdmin>entities> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	// Initialize
    	var defer = $q.defer();

    	// Process
    	function successCallback(response) {
    		defer.resolve(response.object);
    	}

    	function errorCallback(response) {
    		defer.reject(response.message);
    	}

    	webvellaAdminService.getRecordsByEntityName("null", "role", "null", "null", successCallback, errorCallback);

    	// Return
    	$log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }


    // Resolve Roles list /////////////////////////
    resolveAreasList.$inject = ['$q', '$log', 'webvellaAdminService'];
    /* @ngInject */
    function resolveAreasList($q, $log, webvellaAdminService) {
    	$log.debug('webvellaAdmin>entities> BEGIN state.resolved ' + moment().format('HH:mm:ss SSSS'));
        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaAdminService.getRecordsByEntityName("null", "area", "null", "null", successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
        return defer.promise;
    }

    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'ngToast', 'resolvedCurrentEntityMeta', '$uibModal',
        'resolvedRolesList', 'webvellaAdminService', 'resolvedAreasList', '$timeout','webvellaAreasService'];

    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle, ngToast, resolvedCurrentEntityMeta, $uibModal,
        resolvedRolesList, webvellaAdminService, resolvedAreasList, $timeout,webvellaAreasService) {
    	$log.debug('webvellaAdmin>entity-details> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var contentData = this;
        contentData.entity = resolvedCurrentEntityMeta;
        //Update page title
        contentData.pageTitle = "Entity > " + contentData.entity.label +" > Details | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
			$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = contentData.entity.label;

		contentData.showSidebar = function(){
		        //Show Sidemenu
				$timeout(function(){
					$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
					$log.debug('rootScope>events> "application-body-sidebar-menu-isVisible-update" emitted ' + moment().format('HH:mm:ss SSSS'));
				},0);
		}

        //Create new entity modal
        contentData.openDeleteEntityModal = function () {
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'deleteEntityModal.html',
                controller: 'DeleteEntityModalController',
                controllerAs: "popupData",
                size: "",
                resolve: {
                    parentData: function () { return contentData; }
                }
            });

        }

        //Awesome font icon names array 
        contentData.icons = getFontAwesomeIconNames();

        //Get Areas list and selected areas for the entity
        contentData.areas = fastCopy(resolvedAreasList.data);
        contentData.areas = contentData.areas.sort(function (a, b) {
            if (a.label < b.label) return -1;
            if (a.label > b.label) return 1;
            return 0;
        });

        //Generate roles and checkboxes
        contentData.entity.roles = [];
        for (var i = 0; i < resolvedRolesList.data.length; i++) {

            //Now create the new entity.roles array
            var role = {};
            role.id = resolvedRolesList.data[i].id;
            role.label = resolvedRolesList.data[i].name;
            role.canRead = false;
            if (contentData.entity.recordPermissions.canRead.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canRead = true;
            }
            role.canCreate = false;
            if (contentData.entity.recordPermissions.canCreate.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canCreate = true;
            }
            role.canUpdate = false;
            if (contentData.entity.recordPermissions.canUpdate.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canUpdate = true;
            }
            role.canDelete = false;
            if (contentData.entity.recordPermissions.canDelete.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canDelete = true;
            }
            contentData.entity.roles.push(role);
        }

        contentData.fieldUpdate = function (key, data) {
            contentData.patchObject = {};
            contentData.patchObject[key] = data;
            webvellaAdminService.patchEntity(contentData.entity.id, contentData.patchObject, patchSuccessCallback, patchFailedCallback);
        }

        // Helper function
		contentData.renderFieldValue = webvellaAreasService.renderFieldValue;


        function removeValueFromArray(array, value) {
            for (var i = array.length - 1; i >= 0; i--) {
                if (array[i] === value) {
                    array.splice(i, 1);
                    // break;       //<-- Uncomment  if only the first term has to be removed
                }
            }
        }

        contentData.permissionPatch = function (roleId, key, isEnabled) {
            contentData.patchObject = {};
            contentData.patchObject.recordPermissions = {};
            contentData.patchObject.recordPermissions = contentData.entity.recordPermissions;
            if (isEnabled) {
                contentData.entity.recordPermissions[key].push(roleId);
            }
            else {
                removeValueFromArray(contentData.entity.recordPermissions[key], roleId);
            }
            contentData.patchObject.recordPermissions[key] = contentData.entity.recordPermissions[key];
            webvellaAdminService.patchEntity(contentData.entity.id, contentData.patchObject, patchSuccessCallback, patchFailedCallback);
        }


        function patchSuccessCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + response.message
            });
            //webvellaAdminService.regenerateAllAreaAttachments();
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

        $log.debug('webvellaAdmin>entity-details> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }


    //// Modal Controllers
    deleteEntityController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaAdminService', 'ngToast', '$timeout', '$state'];

    /* @ngInject */
    function deleteEntityController(parentData, $uibModalInstance, $log, webvellaAdminService, ngToast, $timeout, $state) {
    	$log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var popupData = this;
        popupData.entity = parentData.entity;

        popupData.ok = function () {
            webvellaAdminService.deleteEntity(popupData.entity.id, successCallback, errorCallback)
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
            $timeout(function() {
                $state.go("webvella-admin-entities");
            }, 10);
        }

        function errorCallback(response) {
            popupData.hasError = true;
            popupData.errorMessage = response.message;


        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };

})();
