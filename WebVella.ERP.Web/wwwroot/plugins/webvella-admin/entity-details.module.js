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
                    controllerAs: 'ngCtrl'
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

	// Resolve Roles list /////////////////////////
    resolveRolesList.$inject = ['$q', '$log', 'webvellaCoreService'];
	
    function resolveRolesList($q, $log, webvellaCoreService) {
    	// Initialize
    	var defer = $q.defer();

    	// Process
    	function successCallback(response) {
    		defer.resolve(response.object);
    	}

    	function errorCallback(response) {
    		defer.reject(response.message);
    	}

    	webvellaCoreService.getRecordsByListName("null","role", "null", successCallback, errorCallback);

    	return defer.promise;
    }


    // Resolve Roles list /////////////////////////
    resolveAreasList.$inject = ['$q', '$log', 'webvellaCoreService'];
    
    function resolveAreasList($q, $log, webvellaCoreService) {

        // Initialize
        var defer = $q.defer();

        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaCoreService.getRecordsByListName("null","area", "null", successCallback, errorCallback);

        return defer.promise;
    }

    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'ngToast', 'resolvedCurrentEntityMeta', '$uibModal',
        'resolvedRolesList', 'webvellaCoreService', 'resolvedAreasList', '$timeout'];

    
    function controller($scope, $log, $rootScope, $state, pageTitle, ngToast, resolvedCurrentEntityMeta, $uibModal,
        resolvedRolesList, webvellaCoreService, resolvedAreasList, $timeout) {
        
        var ngCtrl = this;
        ngCtrl.entity = resolvedCurrentEntityMeta;
        //Update page title
        ngCtrl.pageTitle = "Entity > " + ngCtrl.entity.label +" > Details | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			//Hide Sidemenu
			$rootScope.$emit("application-body-sidebar-menu-isVisible-update", false);
		},0);
		$rootScope.adminSectionName = "Entities";
		$rootScope.adminSubSectionName = ngCtrl.entity.label;

		ngCtrl.showSidebar = function(){
		        //Show Sidemenu
				$timeout(function(){
					$rootScope.$emit("application-body-sidebar-menu-isVisible-update", true);
				},0);
		}

        //Create new entity modal
        ngCtrl.openDeleteEntityModal = function () {
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'deleteEntityModal.html',
                controller: 'DeleteEntityModalController',
                controllerAs: "popupCtrl",
                size: "",
                resolve: {
                    parentData: function () { return ngCtrl; }
                }
            });

        }

        //Awesome font icon names array 
        ngCtrl.icons = getFontAwesomeIconNames();

        //Get Areas list and selected areas for the entity
        ngCtrl.areas = fastCopy(resolvedAreasList.data);
        ngCtrl.areas = ngCtrl.areas.sort(function (a, b) {
            if (a.label < b.label) return -1;
            if (a.label > b.label) return 1;
            return 0;
        });

        //Generate roles and checkboxes
        ngCtrl.entity.roles = [];
        for (var i = 0; i < resolvedRolesList.data.length; i++) {

            //Now create the new entity.roles array
            var role = {};
            role.id = resolvedRolesList.data[i].id;
            role.label = resolvedRolesList.data[i].name;
            role.canRead = false;
            if (ngCtrl.entity.recordPermissions.canRead.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canRead = true;
            }
            role.canCreate = false;
            if (ngCtrl.entity.recordPermissions.canCreate.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canCreate = true;
            }
            role.canUpdate = false;
            if (ngCtrl.entity.recordPermissions.canUpdate.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canUpdate = true;
            }
            role.canDelete = false;
            if (ngCtrl.entity.recordPermissions.canDelete.indexOf(resolvedRolesList.data[i].id) > -1) {
                role.canDelete = true;
            }
            ngCtrl.entity.roles.push(role);
        }

        ngCtrl.fieldUpdate = function (key, data) {
            ngCtrl.patchObject = {};
            ngCtrl.patchObject[key] = data;
            webvellaCoreService.patchEntity(ngCtrl.entity.id, ngCtrl.patchObject, patchSuccessCallback, patchFailedCallback);
        }

        // Helper function
		ngCtrl.renderFieldValue = webvellaCoreService.renderFieldValue;


        function removeValueFromArray(array, value) {
            for (var i = array.length - 1; i >= 0; i--) {
                if (array[i] === value) {
                    array.splice(i, 1);
                    // break;       //<-- Uncomment  if only the first term has to be removed
                }
            }
        }

        ngCtrl.permissionPatch = function (roleId, key, isEnabled) {
            ngCtrl.patchObject = {};
            ngCtrl.patchObject.recordPermissions = {};
            ngCtrl.patchObject.recordPermissions = ngCtrl.entity.recordPermissions;
            if (isEnabled) {
                ngCtrl.entity.recordPermissions[key].push(roleId);
            }
            else {
                removeValueFromArray(ngCtrl.entity.recordPermissions[key], roleId);
            }
            ngCtrl.patchObject.recordPermissions[key] = ngCtrl.entity.recordPermissions[key];
            webvellaCoreService.patchEntity(ngCtrl.entity.id, ngCtrl.patchObject, patchSuccessCallback, patchFailedCallback);
        }


        function patchSuccessCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + response.message
            });
            webvellaCoreService.regenerateAllAreaAttachments();
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
    }


    //// Modal Controllers
    deleteEntityController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state'];

    
    function deleteEntityController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state) {
        
        var popupCtrl = this;
        popupCtrl.entity = parentData.entity;

        popupCtrl.ok = function () {
            webvellaCoreService.deleteEntity(popupCtrl.entity.id, successCallback, errorCallback)
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
            $timeout(function() {
                $state.go("webvella-admin-entities");
            }, 10);
        }

        function errorCallback(response) {
            popupCtrl.hasError = true;
            popupCtrl.errorMessage = response.message;


        }
    };

})();
