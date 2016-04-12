/* areas-lists.module.js */

/**
* @desc this module manages the entity record lists in the admin screen
*/

(function () {
    'use strict';

    angular
        .module('webvellaAdmin') //only gets the module, already initialized in the base.module of the plugin. The lack of dependency [] makes the difference.
        .config(config)
        .controller('WebVellaAdminRolesController', controller)
        .controller('ManageRoleModalController', manageRoleController);
   
    ///////////////////////////////////////////////////////
    /// Configuration
    ///////////////////////////////////////////////////////

    config.$inject = ['$stateProvider'];

    /* @ngInject */
    function config($stateProvider) {
        $stateProvider.state('webvella-admin-roles', {
            parent: 'webvella-admin-base',
            url: '/roles', 
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
                	controller: 'WebVellaAdminRolesController',
                	templateUrl: '/plugins/webvella-admin/roles.view.html',
                    controllerAs: 'contentData'
                }
            },
            resolve: {
            	checkedAccessPermission: checkAccessPermission,
                resolvedRolesList: resolveRolesList
            },
            data: {

            }
        });
    };


    //#region << Resolve Functions >>/////////////////////////

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

    //#region << Controller >> ///////////////////////////////
    controller.$inject = ['$scope', '$log', '$rootScope', '$state', 'pageTitle','$timeout', 
							'resolvedRolesList', '$uibModal', 'webvellaAdminService'];
    /* @ngInject */
    function controller($scope, $log, $rootScope, $state, pageTitle,$timeout,
						resolvedRolesList, $uibModal, webvellaAdminService) {
    	$log.debug('webvellaAdmin>roles> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var contentData = this;
        contentData.search = {};

        //#region << Update page title >>
        contentData.pageTitle = "User List | " + pageTitle;
		$timeout(function(){
			$rootScope.$emit("application-pageTitle-update", contentData.pageTitle);
		},0);
    	//#endregion

        contentData.roles = fastCopy(resolvedRolesList.data);
        contentData.roles = contentData.roles.sort(function (a, b) {
            if (a.name < b.name) return -1;
            if (a.name > b.name) return 1;
            return 0;
        });


        //Create new entity modal
        contentData.openManageRoleModal = function (role) {
        	if (role != null) {
        		contentData.currentRole = role;
            }
            else {
        		contentData.currentRole = {};
        		contentData.currentRole.id = null;
        		contentData.currentRole.name = "";
            }
        	var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'manageRoleModal.html',
                controller: 'ManageRoleModalController',
                controllerAs: "popupData",
                //size: "lg",
                resolve: {
                    contentData: function () {
                        return contentData;
                    }
                }
            });

        }



        $log.debug('webvellaAdmin>roles> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    }
    //#endregion


    //// Modal Controllers
    manageRoleController.$inject = ['$uibModalInstance', '$log', '$sce', '$uibModal', '$filter', 'webvellaAdminService', 'webvellaRootService', 'ngToast', '$timeout', '$state', '$location', 'contentData'];
    /* @ngInject */
    function manageRoleController($uibModalInstance, $log, $sce, $uibModal, $filter, webvellaAdminService, webvellaRootService, ngToast, $timeout, $state, $location, contentData) {
    	$log.debug('webvellaAdmin>entities>createEntityModal> START controller.exec ' + moment().format('HH:mm:ss SSSS'));
        /* jshint validthis:true */
        var popupData = this;
        popupData.modalInstance = $uibModalInstance;
        popupData.role = fastCopy(contentData.currentRole);
        popupData.isUpdate = true;
        if (popupData.role.id == null) {
        	popupData.isUpdate = false;
            popupData.modalTitle = "Create new role";
        }
        else {
            popupData.modalTitle ='Edit role <span class="go-green">' + popupData.role.name + '</span>';
        }

        popupData.deleteRoleModal = function () {
        	//Should block the ability to delete admin, guest, regular
			//On delete should update -> areas, entities, entity fields, users and other relevant items which has roles relations.
        	alert("Not implemented yet");
		}

        /// EXIT functions
        popupData.ok = function () {
        	popupData.validation = {};
        	if (!popupData.isUpdate) {
        		webvellaAdminService.createRecord("role",popupData.role, successCallback, errorCallback);
            }
            else {
        		webvellaAdminService.updateRecord(popupData.role.id, "role", popupData.role, successCallback, errorCallback);
            } 
        };

        popupData.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        /// Aux
        function successCallback(response) {
            ngToast.create({
                className: 'success',
                content: '<span class="go-green">Success:</span> ' + 'The role was successfully saved'
            });
            $uibModalInstance.close('success');
            webvellaRootService.GoToState($state.current.name, {});
        }

        function errorCallback(response) {
            var location = $location;
            //Process the response and generate the validation Messages
            webvellaRootService.generateValidationMessages(response, popupData, popupData.user, location);
        }


        $log.debug('webvellaAdmin>entities>createEntityModal> END controller.exec ' + moment().format('HH:mm:ss SSSS'));
    };



})();
