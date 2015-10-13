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

    /* @ngInject */
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
                    controllerAs: 'contentData'
                }
            },
            resolve: {
            	checkedAccessPermission: checkAccessPermission,
                resolvedEntityMetaList: resolveEntityMetaList,
                resolvedRolesList:resolveRolesList
            },
            data: {

            }
        });
    };

	//#region << Resolve >>
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

    	$log.debug('webvellaAreas>entities> END check access permission ' + moment().format('HH:mm:ss SSSS'));
    	return defer.promise;
    }


    // Resolve EntityMetaList /////////////////////////
    resolveEntityMetaList.$inject = ['$q', '$log', 'webvellaAdminService'];

    /* @ngInject */
    function resolveEntityMetaList($q, $log, webvellaAdminService) {
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

        webvellaAdminService.getMetaEntityList(successCallback, errorCallback);

        // Return
        $log.debug('webvellaAdmin>entities> END state.resolved ' + moment().format('HH:mm:ss SSSS'));
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

	//#endregion

    // Controller ///////////////////////////////
    controller.$inject = ['$log', '$rootScope', '$state', 'pageTitle', 'resolvedEntityMetaList', '$uibModal', 'resolvedRolesList', 'webvellaAdminService'];

    /* @ngInject */
    function controller($log, $rootScope, $state, pageTitle, resolvedEntityMetaList, $uibModal, resolvedRolesList, webvellaAdminService) {
    	$log.debug('webvellaAdmin>entities> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var contentData = this;
        //Update page title
        contentData.pageTitle = "Entities | " + pageTitle;
        $rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
        contentData.entities = resolvedEntityMetaList.entities;
        contentData.entities = contentData.entities.sort(function (a, b) { 
            if(a.name < b.name) return -1;
            if(a.name > b.name) return 1;
            return 0; 
        });
        contentData.roles = resolvedRolesList.data;
        contentData.search = {};
        //Create new entity modal
        contentData.openAddEntityModal = function () {
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'createEntityModal.html',
                controller: 'CreateEntityModalController',
                controllerAs: "popupData",
                size: "lg",
                resolve: {
                    contentData: function () {
                        return contentData;
                    }
                }
            });

        }

        $log.debug('webvellaAdmin>entities> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }


    //// Modal Controllers
    createEntityController.$inject = ['$modalInstance', '$log', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];

    /* @ngInject */
    function createEntityController($modalInstance, $log, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, contentData) {
    	$log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var popupData = this;
        popupData.entity = webvellaAdminService.initEntity();
        popupData.roles = contentData.roles;
        
        //Processing the roles for generation the checkbox values
        popupData.entity.roles = [];

        for (var i = 0; i < popupData.roles.length; i++) {
            //Enable all checkboxes for administrators
            if (popupData.roles[i].name == "Administrator") {
                popupData.entity.recordPermissions.canRead.push(popupData.roles[i].id);
                popupData.entity.recordPermissions.canCreate.push(popupData.roles[i].id);
                popupData.entity.recordPermissions.canUpdate.push(popupData.roles[i].id);
                popupData.entity.recordPermissions.canDelete.push(popupData.roles[i].id);
            }

            //Now create the new entity.roles array
            var role = {};
            role.id = popupData.roles[i].id;
            role.name = popupData.roles[i].name;
            role.canRead = false;
            if (popupData.entity.recordPermissions.canRead.indexOf(popupData.roles[i].id) > -1) {
                role.canRead = true;
            }
            role.canCreate = false;
            if (popupData.entity.recordPermissions.canCreate.indexOf(popupData.roles[i].id) > -1) {
                role.canCreate = true;
            }
            role.canUpdate = false;
            if (popupData.entity.recordPermissions.canUpdate.indexOf(popupData.roles[i].id) > -1) {
                role.canUpdate = true;
            }
            role.canDelete = false;
            if (popupData.entity.recordPermissions.canDelete.indexOf(popupData.roles[i].id) > -1) {
                role.canDelete = true;
            }
            popupData.entity.roles.push(role);
        }
        
        function removeValueFromArray(array, value) {
            for (var i = array.length - 1; i >= 0; i--) {
                if (array[i] === value) {
                    array.splice(i, 1);
                    // break;       //<-- Uncomment  if only the first term has to be removed
                }
            }
        }

        popupData.toggleCanRead = function (roleId) {
            if (popupData.entity.recordPermissions.canRead.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canRead, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canRead.push(roleId);
            }
        }

        popupData.toggleCanCreate = function (roleId) {
            if (popupData.entity.recordPermissions.canCreate.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canCreate, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canCreate.push(roleId);
            }
        }

        popupData.toggleCanUpdate = function (roleId) {
            if (popupData.entity.recordPermissions.canUpdate.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canUpdate, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canUpdate.push(roleId);
            }
        }

        popupData.toggleCanDelete = function (roleId) {
            if (popupData.entity.recordPermissions.canDelete.indexOf(roleId) > -1) {
                //Found - should be removed
                removeValueFromArray(popupData.entity.recordPermissions.canDelete, roleId);
            }
            else {
                //Not Found - should be added
                popupData.entity.recordPermissions.canDelete.push(roleId);
            }
        }

        //Awesome font icon names array 
        popupData.icons = getFontAwesomeIconNames();

  


        popupData.ok = function () {
            webvellaAdminService.createEntity(popupData.entity, successCallback, errorCallback)
        };

        popupData.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> '+ 'The entity was successfully created'
            });
            $modalInstance.close('success');
            webvellaRootService.GoToState($state, $state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupData,popupData.entity, location);
        }
        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };

})();
