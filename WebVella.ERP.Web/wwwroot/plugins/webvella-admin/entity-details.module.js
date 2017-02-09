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
                    templateUrl: '/plugins/webvella-admin/sidebar-avatar-only.view.html',
                    controllerAs: 'sidebarData'
                },
                "contentView": {
                    controller: 'WebVellaAdminEntityDetailsController',
                    templateUrl: '/plugins/webvella-admin/entity-details.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
                resolvedRolesList: resolveRolesList,
                resolvedAreasList: resolveAreasList,
				resolvedEntityList:resolveEntityList
            },
            data: {

            }
        });
    };


	// Resolve Function /////////////////////////

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

    	webvellaCoreService.getRecordsWithoutList(null,null,null,"role", successCallback, errorCallback);

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

        webvellaCoreService.getRecordsWithoutList(null,null,null,"area", successCallback, errorCallback);

        return defer.promise;
    }

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



    // Controller ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle', 'ngToast', '$uibModal','resolvedEntityList',
        'resolvedRolesList', 'webvellaCoreService', 'resolvedAreasList', '$timeout','$translate','$stateParams'];

    
    function controller($scope, $log, $rootScope, $state, pageTitle, ngToast, $uibModal,resolvedEntityList,
        resolvedRolesList, webvellaCoreService, resolvedAreasList, $timeout,$translate,$stateParams) {
        
        var ngCtrl = this;
        ngCtrl.entity = webvellaCoreService.getEntityMetaFromEntityList($stateParams.entityName,resolvedEntityList);
        //Update page title
		$translate(['ENTITY','DETAILS','ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.ENTITY + " > " + ngCtrl.entity.label +" > " + translations.DETAILS +" | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
			$rootScope.adminSectionName = translations.ENTITIES;
		});
		$rootScope.adminSubSectionName = ngCtrl.entity.label;
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
        ngCtrl.areas = resolvedAreasList.data;
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
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + ' ' + response.message
				});
			});
            webvellaCoreService.regenerateAllAreaAttachments();
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
    }


    //// Modal Controllers
    deleteEntityController.$inject = ['parentData', '$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state','$translate'];

    
    function deleteEntityController(parentData, $uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state,$translate) {
        
        var popupCtrl = this;
        popupCtrl.entity = parentData.entity;

        popupCtrl.ok = function () {
            webvellaCoreService.deleteEntity(popupCtrl.entity.id, successCallback, errorCallback)
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.close('dismiss');
        };

        /// Aux
        function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content: translations.SUCCESS_MESSAGE_LABEL + ' ' + response.message
				});
			});
            $uibModalInstance.close('success');
            $timeout(function() {
                $state.go("webvella-admin-entities");
            }, 0);
        }

        function errorCallback(response) {
            popupCtrl.hasError = true;
            popupCtrl.errorMessage = response.message;


        }
    };

})();
