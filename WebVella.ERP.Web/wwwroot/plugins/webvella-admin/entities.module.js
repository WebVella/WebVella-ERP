/* home.module.js */

/**
* @desc this module manages the application home desktop screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminEntitiesController', controller)
        .controller('CreateEntityModalController', createEntityController);

    // Configuration ///////////////////////////////////
    config.$inject = ['$stateProvider'];

    
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-entities', {
            parent: 'webvella-admin-base',
            url: '/entities', //  /desktop/areas after the parent state is prepended
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
                    controller: 'WebVellaAdminEntitiesController',
                    templateUrl: '/plugins/webvella-admin/entities.view.html',
                    controllerAs: 'ngCtrl'
                }
            },
            resolve: {
                resolvedEntityMetaList: resolveEntityMetaList,
                resolvedRolesList:resolveRolesList
            },
            data: {

            }
        });
    };

	//#region << Resolve >>

    // Resolve EntityMetaList /////////////////////////
    resolveEntityMetaList.$inject = ['$rootScope','$q', '$log', 'webvellaCoreService','$timeout'];

    
    function resolveEntityMetaList($rootScope,$q, $log, webvellaCoreService,$timeout) {
        // Initialize
        var defer = $q.defer();
        // Process
        function successCallback(response) {
            defer.resolve(response.object);
        }

        function errorCallback(response) {
        	defer.reject(response.message);
        }

        webvellaCoreService.getEntityMetaList(successCallback, errorCallback);

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

    	webvellaCoreService.getRecordsByListName("null","role", "null", null, successCallback, errorCallback);

    	return defer.promise;
    }

	//#endregion

    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'pageTitle', 'resolvedEntityMetaList', '$uibModal', 'resolvedRolesList', 'webvellaCoreService',
						'$timeout','$translate'];

    
    function controller($log, $rootScope, $state, pageTitle, resolvedEntityMetaList, $uibModal, resolvedRolesList, webvellaCoreService,
						$timeout,$translate) {
        
        var ngCtrl = this;
        //Update page title
		$translate(['ENTITIES']).then(function (translations) {
			ngCtrl.pageTitle = translations.ENTITIES + " | " + pageTitle;
			$rootScope.$emit("application-pageTitle-update", ngCtrl.pageTitle);
		});
        ngCtrl.entities = resolvedEntityMetaList.entities;
        ngCtrl.entities = ngCtrl.entities.sort(function (a, b) { 
            if(a.name < b.name) return -1;
            if(a.name > b.name) return 1;
            return 0; 
        });

        ngCtrl.roles = resolvedRolesList.data;
        ngCtrl.search = {};
        //Create new entity modal
        ngCtrl.openAddEntityModal = function () {
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'createEntityModal.html',
                controller: 'CreateEntityModalController',
                controllerAs: "popupCtrl",
                size: "lg",
                resolve: {
                    ngCtrl: function () {
                        return ngCtrl;
                    }
                }
            });

        }

    }


    //// Modal Controllers
    createEntityController.$inject = ['$uibModalInstance', '$log', 'webvellaCoreService', 'ngToast', '$timeout', '$state', '$location', 'ngCtrl','$translate'];

    
    function createEntityController($uibModalInstance, $log, webvellaCoreService, ngToast, $timeout, $state, $location, ngCtrl,$translate) {

        
        var popupCtrl = this;
        popupCtrl.entity = webvellaCoreService.initEntity();
        popupCtrl.roles = ngCtrl.roles;
        
        //Processing the roles for generation the checkbox values
        popupCtrl.entity.roles = [];

        for (var i = 0; i < popupCtrl.roles.length; i++) {
            //Enable all checkboxes for administrators
            if (popupCtrl.roles[i].name == "administrator") {
                popupCtrl.entity.recordPermissions.canRead.push(popupCtrl.roles[i].id);
                popupCtrl.entity.recordPermissions.canCreate.push(popupCtrl.roles[i].id);
                popupCtrl.entity.recordPermissions.canUpdate.push(popupCtrl.roles[i].id);
                popupCtrl.entity.recordPermissions.canDelete.push(popupCtrl.roles[i].id);
            }

            //Now create the new entity.roles array
            var role = {};
            role.id = popupCtrl.roles[i].id;
            role.name = popupCtrl.roles[i].name;
            role.canRead = false;
            if (popupCtrl.entity.recordPermissions.canRead.indexOf(popupCtrl.roles[i].id) > -1) {
                role.canRead = true;
            }
            role.canCreate = false;
            if (popupCtrl.entity.recordPermissions.canCreate.indexOf(popupCtrl.roles[i].id) > -1) {
                role.canCreate = true;
            }
            role.canUpdate = false;
            if (popupCtrl.entity.recordPermissions.canUpdate.indexOf(popupCtrl.roles[i].id) > -1) {
                role.canUpdate = true;
            }
            role.canDelete = false;
            if (popupCtrl.entity.recordPermissions.canDelete.indexOf(popupCtrl.roles[i].id) > -1) {
                role.canDelete = true;
            }
            popupCtrl.entity.roles.push(role);
        }
        
        function removeValueFromArray(array, value) {
            for (var i = array.length - 1; i >= 0; i--) {
                if (array[i] === value) {
                    array.splice(i, 1);
                    // break;       //<-- Uncomment  if only the first term has to be removed
                }
            }
        }

        popupCtrl.toggleCanRead = function (roleId) {
            if (popupCtrl.entity.recordPermissions.canRead.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupCtrl.entity.recordPermissions.canRead, roleId);
            }
            else {
                //Not Found - should be added
                popupCtrl.entity.recordPermissions.canRead.push(roleId);
            }
        }

        popupCtrl.toggleCanCreate = function (roleId) {
            if (popupCtrl.entity.recordPermissions.canCreate.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupCtrl.entity.recordPermissions.canCreate, roleId);
            }
            else {
                //Not Found - should be added
                popupCtrl.entity.recordPermissions.canCreate.push(roleId);
            }
        }

        popupCtrl.toggleCanUpdate = function (roleId) {
            if (popupCtrl.entity.recordPermissions.canUpdate.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupCtrl.entity.recordPermissions.canUpdate, roleId);
            }
            else {
                //Not Found - should be added
                popupCtrl.entity.recordPermissions.canUpdate.push(roleId);
            }
        }

        popupCtrl.toggleCanDelete = function (roleId) {
            if (popupCtrl.entity.recordPermissions.canDelete.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupCtrl.entity.recordPermissions.canDelete, roleId);
            }
            else {
                //Not Found - should be added
                popupCtrl.entity.recordPermissions.canDelete.push(roleId);
            }
        }

        //Awesome font icon names array 
        popupCtrl.icons = getFontAwesomeIconNames();

  


        popupCtrl.ok = function () {
            webvellaCoreService.createEntity(popupCtrl.entity, successCallback, errorCallback)
        };

        popupCtrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
			$translate(['SUCCESS_MESSAGE_LABEL', 'ENTITY_CREATE_SUCCESS']).then(function (translations) {
				ngToast.create({
					className: 'success',
					content:  translations.SUCCESS_MESSAGE_LABEL + " " + translations.ENTITY_CREATE_SUCCESS
				});
			});
            $uibModalInstance.close('success');
            $timeout(function () {
            	$state.go("webvella-admin-entity-details", { entityName: response.object.name}, { reload: true });
            }, 0);
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaCoreService.generateValidationMessages(response, popupCtrl,popupCtrl.entity, location);
        }
    };

})();
